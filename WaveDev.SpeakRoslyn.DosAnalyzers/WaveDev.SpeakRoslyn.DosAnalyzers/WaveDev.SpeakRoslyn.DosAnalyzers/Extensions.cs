using System;

namespace WaveDev.SpeakRoslyn.DosAnalyzers
{
    public static class Extensions
    {
        public static void Times(this int count, Action action)
        {
            for (int i = 0; i < count; i++)
            {
                action();
            }
        }
    }
}
