using System.Threading;
using REghZy.Utils;

namespace AtominaCraftV4 {
    public static class Timing {
        /// <summary>
        /// Precision thread sleep timing
        /// </summary>
        /// <param name="delay">The exact number of milliseconds to sleep for</param>
        public static void SleepFor(long delay) {
            long nextTick = Time.GetSystemMillis() + delay;
            if (delay > 20) {
                // average windows thread-slice time == 15~ millis
                Thread.Sleep((int) (delay - 20));
            }

            // do this for the rest of the duration, for precise timing
            while (Time.GetSystemMillis() < nextTick) {

            }
        }
    }
}