
namespace Utilities
{
    /// <summary>
    /// Information about this project
    /// </summary>
    public class ProjectInfo
    {
        /// <summary>
        /// Project identifier
        /// </summary>
        public static string Identifier => "com.retrogemn.vaporbreaker";

        /// <summary>
        /// Company Name
        /// </summary>
        public static string CompanyName => "Retrogemn";

        /// <summary>
        /// Product Name
        /// </summary>
        public static string ProductName => "VAPORBREAKER Mobile";

        /// <summary>
        /// iOS min version
        /// </summary>
        public static string iOSVersion => "11";

#if UNITY_ANDROID

        /// <summary>
        /// Project's version
        /// </summary>
        public static string Version => "3.0.0";

        /// <summary>
        /// Mac's build number
        /// </summary>
        public static int BundleVersion => 14;

#elif UNITY_IOS
        /// <summary>
        /// Project's version
        /// </summary>
        public static string Version => "3.0.0";

        /// <summary>
        /// Mac's build number
        /// </summary>
        public static int BundleVersion => 1;
#else
        /// <summary>
        /// Project's version
        /// </summary>
        public static string Version => string.Empty;

        /// <summary>
        /// Mac's build number
        /// </summary>
        public static int BundleVersion => 0;
#endif
    }
}