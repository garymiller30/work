using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualBasic.FileIO;

namespace TIFFB2JDF
{
    public class Jdf
    {

        public string ShablonPath { get; set; }
        private string JobName { get; set; }

        public NamePatternSettings PatternSettings { get; set; } = new NamePatternSettings();


        private readonly List<string> _files = new List<string>();

        
        readonly List<Sheet> _sheets = new List<Sheet>();

        public void AddFile(string file)
        {
            _files.Add(file);
        }

        public void AddFiles(IEnumerable<string> files)
        {
            _files.AddRange(files);
        }

        public void RemoveFile(string file)
        {
            _files.Remove(file);
        }

        public void CreateJdf(string outputDirectopy)
        {
            if (File.Exists(ShablonPath))
            {
                //var sheets = PatternSettings.GetSheets(_files);

                var jobnames = PatternSettings.GetJobNames(_files); // отримати всі можливі назви робіт


                foreach (var jobname in jobnames)
                {
                    var sheets = PatternSettings.GetSheetsByName(_files, jobname); // отримати всі листи

                    foreach (var sheet1 in sheets)
                    {

                        var sheetsIn = new List<Sheet>();

                        JobName = PatternSettings.GetCustomer(_files) + "_" + jobname + "-" + sheet1;

                        var sheet = new Sheet { SheetName = sheet1 };

                        sheetsIn.Add(sheet);

                        List<string> forMove = new List<string>();

                        var sides = PatternSettings.GetFrontBacksBySheet(_files,jobname, sheet1);

                        foreach (var side in sides)
                        {
                            if (Enum.TryParse(side, true, out Side.Sides sideEnum))
                            {
                                var siden = sheet.AddSide(sideEnum);

                                foreach (var file in _files)
                                {
                                    var sep = PatternSettings.GetSeparation(file,jobname, sheet1, side);

                                    if (!string.IsNullOrEmpty(sep))
                                    {
                                        siden.AddSeparation(sep, Path.GetFileName(file));
                                        forMove.Add(file);
                                    }
                                }
                            }
                        }

                        var jdf = File.ReadAllText(ShablonPath);

                        var sb = new StringBuilder(jdf);

                        JobName += $"_[{forMove.Count}]";


                        sb.Replace("[:jobid:]", JobName);
                        sb.Replace("[:sentDateTime:]", DateTime.UtcNow.ToString(CultureInfo.InvariantCulture));

                        sb.Replace("[:ColorPool:]", ColorsManager.GetXml());

                        var runlist = sheetsIn.Aggregate(string.Empty, (current, s) => current + s.GetXml());

                        sb.Replace("[:RunList:]", runlist);

                        var dirName = Path.Combine(outputDirectopy, JobName) + ".fbdi";

                        if (!Directory.Exists(dirName))
                        {
                            Directory.CreateDirectory(dirName);
                        }

                        File.WriteAllText(Path.Combine(dirName, "JobStart.jdf"), sb.ToString());

                        foreach (var file in forMove)
                        {
                            FileSystem.MoveFile(file, Path.Combine(dirName, Path.GetFileName(file)), UIOption.AllDialogs);
                        }

                        File.WriteAllText(Path.Combine(dirName, "JobEnd.jmf"), Properties.Settings.Default.JobEnd);
                    }

                }


                

                
            }


        }
    }
}
