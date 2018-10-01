using System.Collections.Generic;

namespace RpiProcesses.FFmpeg
{
    public static class Resolution
    {
        private static readonly Dictionary<EResolution, string> Resolutions = new Dictionary<EResolution, string>()
        {
            { EResolution.Res320x180, "320x180" },
            { EResolution.Res640x480, "640x480" },
            { EResolution.Res720x480, "720x480" }
        };

        public static string GetResolution(EResolution resolution)
        {
            return Resolutions[resolution];
        }
    }
}
