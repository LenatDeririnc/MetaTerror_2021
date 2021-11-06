using System;

namespace Common.DebugContainers
{
    public static class DebugCheck
    {
        public static void InfiniteCycle(int value, int maxValue)
        {
            if (value > maxValue)
                throw new Exception("Infinite cycle");
        }
        
    }
}