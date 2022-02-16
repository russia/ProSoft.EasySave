using System;

namespace ProSoft.EasySave.Infrastructure.Helpers
{

    public static class BytesHelpers
    {
        public static string FormatBytesSize(ulong bytes)
        {
            var longValue = bytes;
            ulong unit = 1024;
            if (bytes < unit)
                return $"{bytes} Bytes";

            var exp = (int)(Math.Log(longValue) / Math.Log(unit));
            return $"{bytes / Math.Pow(unit, exp):F2} {"KMGTPE"[exp - 1]}B";
        }
    }
}