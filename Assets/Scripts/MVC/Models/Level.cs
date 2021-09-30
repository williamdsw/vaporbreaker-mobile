
namespace MVC.Models
{
    /// <summary>
    /// Level data
    /// </summary>
    public class Level
    {
        private long ID = 0;
        private string NAME = string.Empty;
        private long IS_UNLOCKED = 0;
        private long IS_COMPLETED = 0;
        private string LAYOUT = string.Empty;

        /// <summary>
        /// Database generated id
        /// </summary>
        public long Id { get => ID; set => ID = value; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get => NAME; set => NAME = value; }

        /// <summary>
        /// Is unlocked to play?
        /// </summary>
        public bool IsUnlocked { get => IS_UNLOCKED == 1; set => IS_UNLOCKED = (value ? 1 : 0); }

        /// <summary>
        /// Is completed?
        /// </summary>
        public bool IsCompleted { get => IS_COMPLETED == 1; set => IS_COMPLETED = (value ? 1 : 0); }

        /// <summary>
        /// Level's Layout
        /// </summary>
        public string Layout { get => LAYOUT; set => LAYOUT = value; }
    }
}
