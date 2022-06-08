using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TIFFB2JDF;

namespace UnitTestTIFF2JDF
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var jdf = new Jdf();

            jdf.PatternSettings.Separator = "~";
            jdf.PatternSettings.AddPattern(PatternEnum.Customer);
            jdf.PatternSettings.AddPattern(PatternEnum.JobName);
            jdf.PatternSettings.AddPattern(PatternEnum.PageNumber);
            jdf.PatternSettings.AddPattern(PatternEnum.FrontBack);
            jdf.PatternSettings.AddPattern(PatternEnum.Color);


            jdf.ShablonPath = @"p:\MEGA\Projects\work\ActiveWorks\bin\x86\Debug\JDF\JobStart.jdf";

            jdf.AddFile("IPR~#1507_Vilni_ludi_papka~1~Front~Black.tif");
            jdf.AddFile("IPR~#1507_Vilni_ludi_papka~1~Front~Magenta.tif");
            jdf.AddFile("IPR~#1507_Vilni_ludi_papka~1~Front~Yellow.tif");


            jdf.CreateJdf(@"p:\MEGA\Projects\work\ActiveWorks\bin\x86\Debug\");

        }
    }
}
