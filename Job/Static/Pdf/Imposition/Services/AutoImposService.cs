using Interfaces;
using JobSpace.Profiles;
using JobSpace.Static.Pdf.Imposition.Models.AutoImpos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace JobSpace.Static.Pdf.Imposition.Services
{
    public class AutoImposService
    {
        private const string RulesFileName = "rules.json";
        private const int MinimumScore = 60;

        private readonly Profile _profile;

        public AutoImposService(Profile profile)
        {
            _profile = profile;
        }

        public List<AutoImposRule> LoadRules()
        {
            string path = GetRulesPath();
            if (!File.Exists(path))
                return new List<AutoImposRule>();

            try
            {
                string json = File.ReadAllText(path);
                return JsonSerializer.Deserialize<List<AutoImposRule>>(json) ?? new List<AutoImposRule>();
            }
            catch
            {
                return new List<AutoImposRule>();
            }
        }

        public void SaveRules(List<AutoImposRule> rules)
        {
            Directory.CreateDirectory(_profile.ImposService.AutoImposPath);
            string json = JsonSerializer.Serialize(rules ?? new List<AutoImposRule>());
            File.WriteAllText(GetRulesPath(), json);
        }

        private string GetRulesPath()
        {
            return Path.Combine(_profile.ImposService.AutoImposPath, RulesFileName);
        }

        private static bool HasSavedImposition(string jobFolder)
        {
            if (string.IsNullOrWhiteSpace(jobFolder))
                return false;

            return File.Exists(Path.Combine(jobFolder, ".impos", "imposition.json"));
        }

        public AutoImposMatch TryFindBestMatch(ImposInputParam parameters, IEnumerable<IFileSystemInfoExt> inputFiles)
        {
            if (parameters == null || inputFiles == null)
                return null;

            if (HasSavedImposition(parameters.JobFolder))
                return null;

            var rules = LoadRules();
            if (rules.Count == 0)
                return null;

            var formats = inputFiles
                .Where(file => file != null && !file.IsDir)
                .Select(file => file.Format)
                .Where(format => format.cntPages > 0 || format.Width > 0 || format.Height > 0)
                .ToList();

            if (formats.Count == 0)
                return null;

            var first = formats[0];
            var normalized = Normalize(first.Width, first.Height);
            var metadata = new AutoImposPdfMetadata
            {
                PageWidth = normalized.width,
                PageHeight = normalized.height,
                PageCount = formats.Sum(format => format.cntPages)
            };

            return rules
                .Select(rule => ScoreRule(rule, parameters.Job, metadata))
                .Where(match => match != null && match.Score >= MinimumScore)
                .OrderByDescending(match => match.Score)
                .ThenByDescending(match => match.Rule.Priority)
                .FirstOrDefault(match => LoadPrintSheets(match));
        }

        private AutoImposMatch ScoreRule(AutoImposRule rule, IJob job, AutoImposPdfMetadata metadata)
        {
            if (rule == null || string.IsNullOrWhiteSpace(rule.PrintSheetTemplateFile))
                return null;

            var match = new AutoImposMatch { Rule = rule, Score = rule.Priority };

            if (!string.IsNullOrWhiteSpace(rule.Customer))
            {
                if (!string.Equals(rule.Customer, job?.Customer, StringComparison.InvariantCultureIgnoreCase))
                    return null;

                match.Score += 50;
                match.Reasons.Add("замовник");
            }

            if (!string.IsNullOrWhiteSpace(rule.CategoryId))
            {
                string categoryId = job?.CategoryId?.ToString();
                if (!string.Equals(rule.CategoryId, categoryId, StringComparison.InvariantCultureIgnoreCase))
                    return null;

                match.Score += 30;
                match.Reasons.Add("категорія");
            }

            if (rule.PageWidth.HasValue && rule.PageHeight.HasValue)
            {
                var ruleFormat = Normalize(rule.PageWidth.Value, rule.PageHeight.Value);
                decimal tolerance = Math.Max(rule.PageFormatTolerance, 0);

                if (Math.Abs(ruleFormat.width - metadata.PageWidth) > tolerance ||
                    Math.Abs(ruleFormat.height - metadata.PageHeight) > tolerance)
                {
                    return null;
                }

                match.Score += 40;
                match.Reasons.Add("формат");
            }

            if (rule.ExactPageCount.HasValue)
            {
                if (metadata.PageCount != rule.ExactPageCount.Value)
                    return null;

                match.Score += 40;
                match.Reasons.Add("кількість сторінок");
            }

            if (rule.PageCountMultipleOf.HasValue && rule.PageCountMultipleOf.Value > 0)
            {
                if (metadata.PageCount % rule.PageCountMultipleOf.Value != 0)
                    return null;

                match.Score += 20;
                match.Reasons.Add("кратність сторінок");
            }

            return match;
        }

        private bool LoadPrintSheets(AutoImposMatch match)
        {
            string fileName = match.Rule.PrintSheetTemplateFile;
            string path = Path.IsPathRooted(fileName)
                ? fileName
                : Path.Combine(_profile.ImposService.PrintSheetsPath, fileName);

            var sheets = _profile.ImposService.LoadPrintSheets(path);
            if (sheets.Count == 0)
                return false;

            match.PrintSheets = sheets.Select(sheet => sheet.Copy()).ToList();
            return true;
        }

        private static (decimal width, decimal height) Normalize(decimal width, decimal height)
        {
            return width <= height ? (width, height) : (height, width);
        }

        private sealed class AutoImposPdfMetadata
        {
            public decimal PageWidth { get; set; }
            public decimal PageHeight { get; set; }
            public int PageCount { get; set; }
        }
    }
}
