using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace Job.Controllers
{
    public sealed class SignaController : ISignaController
    {

        private readonly string _signaFile;
        //private string _customer = "customer";
        //private string _description = "description";
        //private string _number = "number";
        private int _cntPages = 2;
        //private decimal _width = 210.0m;
        //private decimal _height = 297.0m;

        private string[] _dataSig;

        private readonly List<AbstractCommand> _commands = new List<AbstractCommand>(8);

        public SignaController(string signaFile)
        {
            _signaFile = signaFile;
        }

        public ISignaController SetCustomer(string customer)
        {
            _commands.Add(new CommandReplaceText
            {
                Replace = customer,
                Cid = "{CID=2",
                Offset = 4,
            });
            
            return this;
        }

        public ISignaController SetJobNumber(string number)
        {
            _commands.Add(new CommandReplaceText
            {
                Replace = number,
                Cid = "{CID=1",
                Offset = 5,
            });
            return this;
        }

        public ISignaController SetJobDescription(string description)
        {
            _commands.Add(new CommandReplaceText
            {
                Replace = description,
                Cid = "{CID=1",
                Offset = 4,
            });

            return this;
        }

        public ISignaController SetPageCount(int cntPages)
        {
            _cntPages = cntPages;
            return this;

        }

        public ISignaController SetPageWidth(decimal width)
        {
            _commands.Add(new CommandReplaceDecimal
            {
                Replace = Math.Round(width,1)*2.83465m,
                Cid = "{CID=63",
                Offset = 4,
            });

            _commands.Add(new CommandReplaceDecimal
            {
                Replace = Math.Round(width,1)*2.83465m,
                Cid = "{CID=81",
                Offset = 4,
            });

            _commands.Add(new CommandReplaceDecimal
            {
                Replace = Math.Round(width,1)*2.83465m,
                Cid = "{CID=87",
                Offset = 4,
            });

            _commands.Add(new CommandReplaceDecimal
            {
                Replace = Math.Round(width,1)*2.83465m,
                Cid = "{CID=105",
                Offset = 4,
            });

            return this;
        }

        public ISignaController SetPageHeight(decimal height)
        {
            _commands.Add(new CommandReplaceDecimal
            {
                Replace = Math.Round(height,1)*2.83465m,
                Offset = 4,
                Cid = "{CID=64",
            });
            _commands.Add(new CommandReplaceDecimal
            {
                Replace = Math.Round(height,1)*2.83465m,
                Offset = 4,
                Cid = "{CID=82",
            });
            _commands.Add(new CommandReplaceDecimal
            {
                Replace = Math.Round(height,1)*2.83465m,
                Offset = 4,
                Cid = "{CID=88",
            });
            _commands.Add(new CommandReplaceDecimal
            {
                Replace = Math.Round(height,1)*2.83465m,
                Offset = 4,
                Cid = "{CID=106",
            });
           
            return this;
        }

        public void Save()
        {
            if (_commands.Any())
            {
                // extract data.sig
                _dataSig = ExtractDataSigFile(_signaFile);
            
                // set parameters
                SetParameters();
            
                // save data.sig
                ReplaceDataSigFile();

            }

        }

        private void ReplaceDataSigFile()
        {
            using (var archive = ZipFile.Open(_signaFile, ZipArchiveMode.Update))
            {
                var entry = archive.GetEntry("data.sig");
                entry.Delete();

                var newEntry = archive.CreateEntry("data.sig", CompressionLevel.Optimal);
                using (var entryStream = newEntry.Open())
                {
                    byte[] bytes = ConvertToByteArray(_dataSig);

                    entryStream.Write(bytes, 0, bytes.Length);
                }
            }
        }

        private byte[] ConvertToByteArray(string[] dataSig)
        {
            var sb = new StringBuilder();

            for (int i = 0; i < dataSig.Length; i++)
            {
                sb.Append(dataSig[i]);

                if (i != dataSig.Length - 1)
                {
                    sb.Append(';');
                    sb.Append('\n');
                }
            }

            return Encoding.UTF8.GetBytes(sb.ToString());

        }

        public void ChangeSignaOrderNumber(string destFile, string number)
        {
            using (var archive = ZipFile.Open(destFile, ZipArchiveMode.Update))
            {
                var entry = archive.GetEntry("data.sig");
                using (var memoryWrite = new MemoryStream())    // 
                {
                    var tmp = Path.GetTempFileName();
                    entry.ExtractToFile(tmp, overwrite:true);
                    var bytes = File.ReadAllBytes(tmp);
                    var cnt = 4;
                    var j = 0;
                    bool isFinded = false;

                    for (j = 0; j < bytes.Length; j++)
                    {
                        if (bytes[j] == 0x22)
                        {
                            if (bytes[j - 1] == 0x0a)
                            {
                                if (cnt == 0)
                                {
                                    isFinded = true;
                                    break;
                                }

                                cnt--;
                            }
                        }
                    }

                    if (isFinded)
                    {
                        // Знайшли місце з номером
                        //взяли довжину минулого номеру
                        var len = bytes[j + 2];
                        memoryWrite.Write(bytes, 0, j + 1);
                        var lenSize = BitConverter.GetBytes(number.Length);
                        memoryWrite.Write(new byte[] { 0x00 }, 0, 1);
                        memoryWrite.Write(lenSize, 0, 1);
                        var str = Encoding.ASCII.GetBytes(number);
                        memoryWrite.Write(str, 0, str.Length);
                        memoryWrite.Write(bytes, j + 3 + len, bytes.Length - (j + 3 + len));

                        using (FileStream file = new FileStream("data.sig", FileMode.Create, FileAccess.Write))
                        {
                            memoryWrite.Seek(0, SeekOrigin.Begin);
                            memoryWrite.CopyTo(file);
                        }
                        entry.Delete();

                        var newEntry = archive.CreateEntry("data.sig", CompressionLevel.Optimal);
                        using (var entryStream = newEntry.Open())
                        {
                            memoryWrite.Seek(0, SeekOrigin.Begin);
                            memoryWrite.CopyTo(entryStream);
                        }
                    }

                    File.Delete(tmp);
                }
            }

        }


        private void SetParameters()
        {
            _commands.ForEach(x=>x.Process(_dataSig));

            //FindAndReplaceText(_dataSig, "{CID=1;", 5, _number);
            //FindAndReplaceText(_dataSig, "{CID=1;", 4, _description);
            //FindAndReplaceText(_dataSig, "{CID=2;", 4, _customer);
            //FindAndReplaceDecimal(_dataSig, "{CID=63;", 4, _width*2.83465m);
            //FindAndReplaceDecimal(_dataSig, "{CID=81;", 4, _width*2.83465m);
            //FindAndReplaceDecimal(_dataSig, "{CID=87;", 4, _width*2.83465m);
            //FindAndReplaceDecimal(_dataSig, "{CID=105;", 4, _width*2.83465m);
            //FindAndReplaceDecimal(_dataSig, "{CID=64;", 4, _height*2.83465m);
            //FindAndReplaceDecimal(_dataSig, "{CID=82;", 4, _height*2.83465m);
            //FindAndReplaceDecimal(_dataSig, "{CID=88;", 4, _height*2.83465m);
            //FindAndReplaceDecimal(_dataSig, "{CID=106;", 4, _height*2.83465m);
            





        }

/*
        private void FindAndReplaceDecimal(string[] dataSig, string cid, int offset, decimal dcml)
        {
            var strCnt = dcml.ToString("F12",CultureInfo.CreateSpecificCulture("en-US"));

            var idx = Array.FindIndex(dataSig, r => r.Contains(cid));
            idx += offset;
            byte[] buff = new byte[strCnt.Length + 2];

            using (var stream = new MemoryStream(buff, true))
            {

                stream.WriteByte((byte)'=');
                stream.Write(Encoding.Default.GetBytes(strCnt), 0, strCnt.Length);
                stream.WriteByte((byte)';');

                stream.Seek(0, SeekOrigin.Begin);
                using (var reader = new StreamReader(stream))
                {
                    dataSig[idx] = reader.ReadToEnd();
                }

            }
        }
*/

/*
        private void FindAndReplaceInt(string[] dataSig, string cid, int offset, int cnt)
        {
            var strCnt = cnt.ToString();

            var idx = Array.FindIndex(dataSig, r => r.Contains(cid));
            idx += offset;
            byte[] buff = new byte[strCnt.Length + 2];

            using (var stream = new MemoryStream(buff, true))
            {

                stream.WriteByte((byte)'=');
                stream.Write(Encoding.Default.GetBytes(strCnt), 0, strCnt.Length);
                stream.WriteByte((byte)';');

                stream.Seek(0, SeekOrigin.Begin);
                using (var reader = new StreamReader(stream))
                {
                    dataSig[idx] = reader.ReadToEnd();
                }

            }
        }
*/

/*
        private void FindAndReplaceText(string[] dataSig, string cid, int offset, string replaceText)
        {
            var idx = Array.FindIndex(dataSig, r => r.Contains(cid));
            idx += offset;
            byte[] buff = new byte[replaceText.Length + 5];

            using (var stream = new MemoryStream(buff, true))
            {

                stream.WriteByte((byte)'"');
                stream.WriteByte(0);
                stream.WriteByte((byte)replaceText.Length);
                stream.Write(Encoding.Default.GetBytes(replaceText), 0, replaceText.Length);
                stream.WriteByte((byte)'"');
                stream.WriteByte((byte)';');

                stream.Seek(0, SeekOrigin.Begin);
                using (var reader = new StreamReader(stream))
                {
                    dataSig[idx] = reader.ReadToEnd();
                }

            }

        }
*/

        private string[] ExtractDataSigFile(string signaFile)
        {
            string[] bytes = null;

            using (var archive = ZipFile.Open(signaFile, ZipArchiveMode.Read))
            {
                var entry = archive.GetEntry("data.sig");
                var tmp = Path.GetTempFileName();

                entry.ExtractToFile(tmp,overwrite: true);

                using (var reader = new StreamReader(tmp))
                {
                    var str = reader.ReadToEnd();
                    bytes = str.Split(new string[]{";\n"},StringSplitOptions.None);
                }

                File.Delete(tmp);
            }
            return bytes;

        }

        abstract class AbstractCommand
        {
            public string Cid { get; set; }
            public int Offset { get; set; }
            public object Replace { get; set; }

            public abstract void Process(string[] dataSig);
        }

        class CommandReplaceText : AbstractCommand
        {
            public override void Process(string [] dataSig)
            {
                var replaceText = (string) Replace;
                var idx = Array.FindIndex(dataSig, r => r.Contains(Cid));
                idx += Offset;
                byte[] buff = new byte[replaceText.Length + 4];

                using (var stream = new MemoryStream(buff,writable: true))
                {

                    stream.WriteByte((byte)'"');
                    stream.WriteByte(0);
                    stream.WriteByte((byte)replaceText.Length);
                    stream.Write(Encoding.Default.GetBytes(replaceText), 0, replaceText.Length);
                    stream.WriteByte((byte)'"');
                    //stream.WriteByte((byte)';');

                    stream.Seek(0, SeekOrigin.Begin);

                    using (var reader = new StreamReader(stream))
                    {
                        dataSig[idx] = reader.ReadToEnd();
                    }

                }
            }
        }

       

        class CommandReplaceDecimal : AbstractCommand
        {
            public override void Process(string [] dataSig)
            {
                var dcml = (decimal) Replace;


                var strCnt = dcml.ToString("F12",CultureInfo.CreateSpecificCulture("en-US"));

                var idx = Array.FindIndex(dataSig, r => r.Contains(Cid));
                idx += Offset;
                byte[] buff = new byte[strCnt.Length + 1];

                using (var stream = new MemoryStream(buff, writable: true))
                {

                    stream.WriteByte((byte)'=');
                    stream.Write(Encoding.Default.GetBytes(strCnt), 0, strCnt.Length);
                    //stream.WriteByte((byte)';');

                    stream.Seek(0, SeekOrigin.Begin);
                    using (var reader = new StreamReader(stream))
                    {
                        dataSig[idx] = reader.ReadToEnd();
                    }
                }
            }
        }

    }
}
