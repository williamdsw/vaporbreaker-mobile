
namespace MVC.Models
{
    /// <summary>
    /// Localization data
    /// </summary>
    public class Localization
    {
        private long ID;
        private string LANGUAGE;
        private string CONTENT;

        /// <summary>
        /// Database generated id
        /// </summary>
        public long Id { get => ID; set => ID = value; }

        /// <summary>
        /// Current Language: "English", "Portuguese", etc.
        /// </summary>
        public string Language { get => LANGUAGE; set => LANGUAGE = value; }

        /// <summary>
        /// JSON content
        /// </summary>
        public string Content { get => CONTENT; set => CONTENT = value; }
    }
}
