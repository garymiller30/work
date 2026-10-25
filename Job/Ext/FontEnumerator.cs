using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class FontEnumerator
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public class LOGFONT
    {
        public int lfHeight;
        public int lfWidth;
        public int lfEscapement;
        public int lfOrientation;
        public int lfWeight;
        public byte lfItalic;
        public byte lfUnderline;
        public byte lfStrikeOut;
        public byte lfCharSet;
        public byte lfOutPrecision;
        public byte lfClipPrecision;
        public byte lfQuality;
        public byte lfPitchAndFamily;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string lfFaceName;
    }

    [DllImport("gdi32.dll", CharSet = CharSet.Auto)]
    private static extern int EnumFontFamiliesEx(
        IntPtr hdc,
        [In] LOGFONT lpLogFont,
        FONTENUMPROC lpEnumFontFamExProc,
        IntPtr lParam,
        uint dwFlags);

    public delegate int FONTENUMPROC(
        ref ENUMLOGFONTEX lpelfe,
        ref NEWTEXTMETRICEX lpntme,
        uint fontType,
        IntPtr lParam);

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct ENUMLOGFONTEX
    {
        public LOGFONT elfLogFont;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string elfFullName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string elfStyle;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string elfScript;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct NEWTEXTMETRICEX
    {
        public uint ntmFlags;
        public uint ntmSizeEM;
        public uint ntmCellHeight;
        public uint ntmAvgWidth;
    }

    [DllImport("user32.dll")]
    private static extern IntPtr GetDC(IntPtr hWnd);

    [DllImport("user32.dll")]
    private static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

    private static List<string> _allFontFaces = new List<string>();

    private static int EnumFamCallBack(ref ENUMLOGFONTEX lpelfe, ref NEWTEXTMETRICEX lpntme, uint fontType, IntPtr lParam)
    {
        if (!string.IsNullOrEmpty(lpelfe.elfFullName))
        {
            if (!_allFontFaces.Contains(lpelfe.elfFullName))
            {
                _allFontFaces.Add(lpelfe.elfFullName);
            }
        }
        return 1; // Продовжити сканування
    }

    public static string[] GetInstalledFontFaces()
    {
        _allFontFaces.Clear();

        // Отримуємо дійсний дескриптор екрану замість IntPtr.Zero
        IntPtr hdc = GetDC(IntPtr.Zero);
        if (hdc == IntPtr.Zero)
            return Array.Empty<string>();

        try
        {
            LOGFONT lf = new LOGFONT
            {
                lfCharSet = 1,              // DEFAULT_CHARSET — обов'язково для пошуку всіх наборів
                lfFaceName = string.Empty   // Порожній рядок означає перелік усіх сімейств/накреслень
            };

            FONTENUMPROC callBackDelegate = new FONTENUMPROC(EnumFamCallBack);
            EnumFontFamiliesEx(hdc, lf, callBackDelegate, IntPtr.Zero, 0);
        }
        finally
        {
            ReleaseDC(IntPtr.Zero, hdc);
        }

        _allFontFaces.Sort();
        return _allFontFaces.ToArray();
    }
}