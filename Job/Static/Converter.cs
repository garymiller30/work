using System.IO;
using Interfaces;
using JobSpace.Models;

namespace JobSpace.Static
{
    public static class Converter
    {
        public static FileSystemInfoExt ToFileSystemInfoExt(this FileSystemInfo fi)
        {
            return ConvertToFileSystemInfoExt(fi);
        }

        public static FileSystemInfoExt ConvertToFileSystemInfoExt(FileSystemInfo fi)
        {

            FileSystemInfoExt fsie = new FileSystemInfoExt(fi);
            
            
            return fsie;
        }


        public static void GetExtendedFileInfoFormat(this FileSystemInfoExt file)
        {
            FileFormatsUtil.GetFormat(file);
            PdfUtils.GetFileCreator(file);
        }

        public static void GetExtendedFileInfoFormat(this IFileSystemInfoExt file)
        {
            FileFormatsUtil.GetFormat(file);
            PdfUtils.GetFileCreator(file);
        }

        public static void GetExtendedFileInfo(this FileSystemInfoExt file)
        {
            FileFormatsUtil.GetFormat(file);
            PdfUtils.GetColorspaces(file);
            PdfUtils.GetFileCreator(file);
        }

        public static void GetExtendedFileInfo(this IFileSystemInfoExt file)
        {
            FileFormatsUtil.GetFormat(file);
            PdfUtils.GetColorspaces(file);
            PdfUtils.GetFileCreator(file);
        }
    }
}
