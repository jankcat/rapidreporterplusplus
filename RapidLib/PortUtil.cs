using System;

namespace RapidLib
{
    public static class PortUtil
    {
        public static bool IsRunningOnMono()
        {
            return Type.GetType("Mono.Runtime") != null;
        }
    }
}
