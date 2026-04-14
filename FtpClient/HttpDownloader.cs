// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com 

using System;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace FtpClient
{
    public static class HttpDownloader
    {
        public static void Download(string url, string targetFolder)
        {

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    var fn = Path.GetFileName(
                        Uri.UnescapeDataString(new Uri(response.ResponseUri.AbsoluteUri).AbsolutePath));


                    var target = Path.Combine(targetFolder, fn);
                    var ext = Path.GetExtension(fn);

                    if (ext.ToLower().Equals(".urls"))
                    {
                        //скачали файл зі списком для закачки

                        var responseStream = response.GetResponseStream();
                        using (var fileStream = File.Create(target))
                        {
                            responseStream?.CopyTo(fileStream);
                        }

                        responseStream?.Dispose();

                        var links = File.ReadAllLines(target);

                        foreach (var link in links)
                        {
                            Download(link, targetFolder);
                        }

                        File.Delete(target);


                    }
                    else
                    {
                        var responseStream = response.GetResponseStream();
                        using (var fileStream = File.Create(Path.Combine(targetFolder, fn)))
                        {
                            responseStream?.CopyTo(fileStream);
                        }
                        responseStream?.Dispose();
                    }

                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
