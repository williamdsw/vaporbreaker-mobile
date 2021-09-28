
namespace MVC.Models
{
    /// <summary>
    /// Track data
    /// </summary>
    public class Track
    {
        private long ID;
        private string TITLE;
        private string ARTIST;
        private string COVER;
        private string FILENAME;

        /// <summary>
        /// Database generated id
        /// </summary>
        public long Id { get => ID; set => ID = value; }

        /// <summary>
        /// Track name
        /// </summary>
        public string Title { get => TITLE; set => TITLE = value; }

        /// <summary>
        /// Artist name
        /// </summary>
        public string Artist { get => ARTIST; set => ARTIST = value; }

        /// <summary>
        /// File name to be loaded
        /// </summary>
        public string FileName { get => FILENAME; set => FILENAME = value; }
    }
}
