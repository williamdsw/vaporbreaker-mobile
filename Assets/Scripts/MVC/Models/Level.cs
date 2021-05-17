
namespace MVC.Models
{
    public class Level
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public Level() { }

        public Level(long id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
