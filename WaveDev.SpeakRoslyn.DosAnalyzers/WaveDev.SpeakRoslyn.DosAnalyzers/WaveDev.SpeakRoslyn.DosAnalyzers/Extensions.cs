using System;

namespace WaveDev.SpeakRoslyn.DosAnalyzers
{
    /// <summary>
    /// This class provides extensions methods that are helpful in the context of this Code Analyzer and Code Fix implementations.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// This method performs an operation n times.
        /// </summary>
        /// <param name="n">Indicates how often the operation will be performed.</param>
        /// <param name="action">The operation that must be performed n times.</param>
        public static void Times(this int n, Action action)
        {
            for (int i = 0; i < n; i++)
            {
                action();
            }
        }
    }
}
