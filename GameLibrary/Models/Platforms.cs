using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary.Models
{
    public static class Platforms
    {
        /// <summary>
        /// Returns the enum of the supported platform
        /// </summary>
        /// <returns></returns>
        public static Platform DeterminePlatform()
        {
            PlatformID platform = Environment.OSVersion.Platform;
            switch (platform)
            {
                case PlatformID.Win32NT:
                case PlatformID.Win32S:
                case PlatformID.Win32Windows:
                case PlatformID.WinCE:
                    return Platform.WindowsDesktop;
                case PlatformID.Unix:
                    return Platform.Android;
                case PlatformID.MacOSX:
                    return Platform.iOS;
                default:
                    return Platform.unknown;
            }
        }

        /// <summary>
        /// Enum that holds all supported platforms
        /// </summary>
        public enum Platform
        {
            WindowsDesktop,
            Android,
            iOS,
            unknown
        }
    }
}
