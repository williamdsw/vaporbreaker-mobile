
namespace MVC.Models
{
    public class Localization
    {
        private long ID;
        private string LANGUAGE;
        private string CONTENT;

        public long Id { get => ID; set => ID = value; }
        public string Language { get => LANGUAGE; set => LANGUAGE = value; }
        public string Content { get => CONTENT; set => CONTENT = value; }

        public override string ToString() => string.Format("Id: {0}; Language: {1}; Content: {2}", Id, Language, Content);
        
    }
}
