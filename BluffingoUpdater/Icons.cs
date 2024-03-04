using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace BluffingoUpdater
{
    class Icons
    {
        public static Icon Extract(string filePath, int index, bool largeIcon = true)
        {
            if (filePath == null)
                throw new ArgumentNullException(nameof(filePath));

            IntPtr hIcon;
            if (largeIcon)
            {
                ExtractIconEx(filePath, index, out hIcon, IntPtr.Zero, 1);
            }
            else
            {
                ExtractIconEx(filePath, index, IntPtr.Zero, out hIcon, 1);
            }

            return hIcon != IntPtr.Zero ? Icon.FromHandle(hIcon) : null;
        }

        [DllImport("shell32", CharSet = CharSet.Unicode)]
        private static extern int ExtractIconEx(string lpszFile, int nIconIndex, out IntPtr phiconLarge, IntPtr phiconSmall, int nIcons);

        [DllImport("shell32", CharSet = CharSet.Unicode)]
        private static extern int ExtractIconEx(string lpszFile, int nIconIndex, IntPtr phiconLarge, out IntPtr phiconSmall, int nIcons);
    }
}
