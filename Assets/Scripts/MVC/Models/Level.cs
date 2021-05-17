
namespace MVC.Models
{
    public class Level
    {
        private long ID = 0;
        private string NAME = string.Empty;
        private long IS_UNLOCKED = 0;
        private long IS_COMPLETED = 0;

        public long Id { get => ID; set => ID = value; }
        public string Name { get => NAME; set => NAME = value; }
        public bool IsUnlocked { get => IS_UNLOCKED == 1; set => IS_UNLOCKED = (value ? 1 : 0); }
        public bool IsCompleted { get => IS_COMPLETED == 1; set => IS_COMPLETED = (value ? 1 : 0); }

        public Level() { }

        public Level(long id, string name, bool isUnlocked, bool isCompleted)
        {
            Id = id;
            Name = name;
            IsUnlocked = isUnlocked;
            IsCompleted = isCompleted;
        }

        public override string ToString() => string.Format("Id = {0}, Name = {1}, IsUnlocked = {2}, IsCompleted = {3}", Id, Name, IsUnlocked, IsCompleted);
    }
}
