using System.Collections.Generic;
using Interfaces;

namespace FtpClient
{
    /// <summary>
    /// Параметри завантаження файлу з ftp
    /// </summary>
    public class DownloadFileParam : IDownloadFileParam
    {
        public string OrderNumber { get; set; }

        /// <summary>
        /// адреса серверу
        /// </summary>
        public string Server { get; set; }
        /// <summary>
        /// користувач
        /// </summary>
        public string User { get; set; }
        /// <summary>
        /// пароль
        /// </summary>
        public string Password { get; set; }


        public string RootDirectory { get; set; }
        /// <summary>
        /// список файлів на закачку
        /// </summary>
        public List<IFtpFileExt> File { get; set; } = new List<IFtpFileExt>();
        /// <summary>
        /// режим підключення (активний/пасивний)
        /// </summary>
        public bool ActiveMode { get; set; }
        /// <summary>
        /// кодова сторінка
        /// </summary>
        public int CodePage { get; set; }
        

    }
}
