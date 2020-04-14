using System;

namespace Orange.ApiTokenValidation.Shell
{
    internal class DebugInfoProvider
    {
        static readonly string[] SizeSuffixes =
            { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

        public static string GetMemoryUsage(int decimalPlaces = 1)
        {
            var value = GC.GetTotalMemory(false);
            return SizeSuffix(value, decimalPlaces);
        }

        public static string GetThreadMemoryUsage(int decimalPlaces = 1)
        {
            var value = GC.GetAllocatedBytesForCurrentThread();
            return SizeSuffix(value, decimalPlaces);
        }

        //TODO change it's very not optimized 
        static string SizeSuffix(long value, int decimalPlaces = 1)
        {
            if (value < 0) { return $"-{SizeSuffix(-value)}"; }

            int i = 0;
            var dValue = (double)value;
            while (Math.Round(dValue, decimalPlaces) >= 1000)
            {
                dValue /= 1024;
                i++;
            }

            var postfix = i < SizeSuffixes.Length ? SizeSuffixes[i] : "Unknown";

            return $"{dValue:n}{decimalPlaces} {postfix}";
            //return string.Format("{0:n" + decimalPlaces + "} {1}", dValue, SizeSuffixes[i]);
        }

        public static string GetGenerations()
        {
            return $"{GC.CollectionCount(0)}-{GC.CollectionCount(1)}-{GC.CollectionCount(2)}";
        }
    }
}
