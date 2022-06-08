using System.Collections.Generic;

namespace Interfaces
{
    public interface IDownloadFileParam
    {
        string OrderNumber { get; set; }

        string Server { get; set; }
        /// <summary>
        /// користувач
        /// </summary>
        string User { get; set; }
        /// <summary>
        /// пароль
        /// </summary>
        string Password { get; set; }


        string RootDirectory { get; set; }
        /// <summary>
        /// список файлів на закачку
        /// </summary>
        List<IFtpFileExt> File { get; set; }
        /// <summary>
        /// режим підключення (активний/пасивний)
        /// </summary>
        bool ActiveMode { get; set; }
        /// <summary>
        /// кодова сторінка
        /// </summary>
        int CodePage { get; set; }
    }
}
