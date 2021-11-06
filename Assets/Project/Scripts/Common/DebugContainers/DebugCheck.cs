using System;

namespace Common.DebugContainers
{
    public static class DebugCheck
    {
        public static void InfiniteCycle(ref int value, int maxValue)
        {
            value += 1;
            if (value > maxValue)
                throw new Exception("Infinite cycle");
        }
        
    }
}