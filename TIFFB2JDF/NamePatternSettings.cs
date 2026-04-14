using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TIFFB2JDF
{
    public class NamePatternSettings
    {
        readonly List<NamePattern> _patterns = new List<NamePattern>();

        public string Separator { get; set; }

        public NamePattern AddPattern(PatternEnum type, string separator = null)
        {
            var p = new NamePattern
            {
                PatternEnums = type,
                Separator = separator
            };
            _patterns.Add(p);
            return p;
        }

        /// <summary>
        /// отримати друкарські аркуші
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public List<string> GetSheets(IEnumerable<string> files)
        {

            var sheets = new List<string>();

            foreach (var file in files)
            {
                var fileName = Path.GetFileNameWithoutExtension(file);

                var s = fileName?.Split(new[] { Separator }, StringSplitOptions.None);

                if (s?.Length == _patterns.Count)
                {
                    var idx = GetIdxByPattern(PatternEnum.PageNumber);
                    if (idx != -1)
                    {
                        sheets.Add(s[idx]);
                    }
                }
            }
            var g = sheets.Distinct().ToList();

            return g;
        }

        /// <summary>
        /// отримати назву роботи
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public string GetJobName(IEnumerable<string> files)
        {
            var sheets = new List<string>();

            foreach (var file in files)
            {
                var fileName = Path.GetFileNameWithoutExtension(file);

                var s = fileName?.Split(new[] { Separator }, StringSplitOptions.None);

                if (s?.Length == _patterns.Count)
                {
                    var idx = GetIdxByPattern(PatternEnum.JobName);
                    if (idx != -1)
                    {
                        sheets.Add(s[idx]);
                    }
                }
            }

            var g = sheets.Distinct().ToList();

            return g.Any()? g[0]:string.Empty;
        }

        /// <summary>
        /// отримати лицьові та зворотні сторони
        /// </summary>
        /// <param name="files"></param>
        /// <param name="jobname"></param>
        /// <param name="sheet1"></param>
        /// <returns></returns>
        public List<string> GetFrontBacksBySheet(IEnumerable<string> files,string jobname, string sheet1)
        {
            var sheets = new List<string>();

            foreach (var file in files)
            {
                var fileName = Path.GetFileNameWithoutExtension(file);

                var s = fileName?.Split(new[] { Separator }, StringSplitOptions.None);

                if (s?.Length == _patterns.Count)
                {

                    var idxName = GetIdxByPattern(PatternEnum.JobName);
                    if (idxName != -1)
                    {
                        if (s[idxName].Equals(jobname))
                        {
                            var idx = GetIdxByPattern(PatternEnum.PageNumber);
                            if (idx != -1 && s[idx] == sheet1)
                            {
                                idx = GetIdxByPattern(PatternEnum.FrontBack);
                                if (idx != -1)
                                {
                                    sheets.Add(s[idx]);
                                }
                            }
                        }
                    }
                }
            }

            var g = sheets.Distinct().ToList();

            return g;
        }

        /// <summary>
        /// отимати колір, маючи номер аркушу та сторону
        /// </summary>
        /// <param name="file"></param>
        /// <param name="jobname"></param>
        /// <param name="sheet1"></param>
        /// <param name="side"></param>
        /// <returns></returns>
        public string GetSeparation(string file,string jobname, string sheet1, string side)
        {
            var ret = string.Empty;

            var fileName = Path.GetFileNameWithoutExtension(file);
            var s = fileName?.Split(new[] { Separator }, StringSplitOptions.None);

            if (s?.Length == _patterns.Count)
            {

                var idxName = GetIdxByPattern(PatternEnum.JobName);
                if (idxName != -1)
                {
                    if (s[idxName].Equals(jobname))
                    {
                        var idx = GetIdxByPattern(PatternEnum.PageNumber);
                        if (idx != -1 && s[idx] == sheet1)
                        {
                            idx = GetIdxByPattern(PatternEnum.FrontBack);
                            if (idx != -1 && s[idx] == side)
                            {
                                idx = GetIdxByPattern(PatternEnum.Color);
                                if (idx != -1)
                                {
                                    return s[idx];
                                }
                            }
                        }
                    }
                }
            }
        return ret;
        }

        /// <summary>
        /// отримати замовника
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public string GetCustomer(List<string> files)
        {
            var sheets = new List<string>();

            foreach (var file in files)
            {
                var fileName = Path.GetFileNameWithoutExtension(file);

                var s = fileName?.Split(new[] { Separator }, StringSplitOptions.None);

                if (s?.Length == _patterns.Count)
                {
                    var idx = GetIdxByPattern(PatternEnum.Customer);
                    if (idx != -1)
                    {
                        sheets.Add(s[idx]);
                    }
                }
            }

            var g = sheets.Distinct().ToList();

            return g.Any() ? g[0]: string.Empty;
        }

        int GetIdxByPattern(PatternEnum pattern)
        {
            var fod = _patterns.FirstOrDefault(x => x.PatternEnums == pattern);
            return fod != null ? _patterns.IndexOf(fod) : -1;
        }

        public List<string> GetJobNames(List<string> files)
        {
            var sheets = new List<string>();

            foreach (var file in files)
            {
                var fileName = Path.GetFileNameWithoutExtension(file);

                var s = fileName?.Split(new[] { Separator }, StringSplitOptions.None);

                if (s?.Length == _patterns.Count)
                {
                    var idx = GetIdxByPattern(PatternEnum.JobName);
                    if (idx != -1)
                    {
                        sheets.Add(s[idx]);
                    }
                }
            }

            var g = sheets.Distinct().ToList();

            return g;
        }

        public List<string> GetSheetsByName(List<string> files, string jobname)
        {
            var sheets = new List<string>();

            foreach (var file in files)
            {
                var fileName = Path.GetFileNameWithoutExtension(file);

                var s = fileName?.Split(new[] { Separator }, StringSplitOptions.None);

                if (s?.Length == _patterns.Count)
                {

                    var idxName = GetIdxByPattern(PatternEnum.JobName);
                    if (idxName != -1)
                    {
                        if (s[idxName].Equals(jobname))
                        {
                            var idx = GetIdxByPattern(PatternEnum.PageNumber);
                            if (idx != -1)
                            {
                                sheets.Add(s[idx]);
                            }

                        }
                    }
                }
            }
            var g = sheets.Distinct().ToList();

            return g;
        }
    }
}
