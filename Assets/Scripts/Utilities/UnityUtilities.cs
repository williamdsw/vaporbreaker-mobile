using UnityEngine.Analytics;

namespace Utilities
{
    /// <summary>
    /// Utilities for Unity
    /// </summary>
    public class UnityUtilities
    {
        /// <summary>
        /// Disable analytics properties
        /// </summary>
        public static void DisableAnalytics()
        {
            Analytics.enabled = false;
            Analytics.deviceStatsEnabled = false;
            Analytics.initializeOnStartup = false;
            Analytics.limitUserTracking = false;
            PerformanceReporting.enabled = false;
        }
    }
}