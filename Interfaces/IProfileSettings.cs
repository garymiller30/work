using System;

namespace Interfaces
{
    public interface IProfileSettings
    {
        string ProfileName { get; set; }
        IBaseSettings GetBaseSettings();
        IMailSettings GetMail();
        IFileBrowserSettings GetFileBrowser();
        IJobSettings GetJobSettings();

        IPdfConverterSettings GetPdfConverterSettings(); 

        /// <summary>
        /// закрити вікно після вставки тексту  в "опис"
        /// </summary>
        bool CloseAfterPasteText { get; set; }
        bool HideCategory { get; set; }

        string OLVState { get; set; }

        decimal CountExplorers { get; set; }
    }
}