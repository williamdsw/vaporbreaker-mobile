
namespace MVC.Models
{
    public class Scoreboard
    {
        public long Id { get; set; }
        public long LevelId { get; set; }
        public long Score { get; set; }
        public long TimeScore { get; set; }
        public long Moment { get; set; }

        public Scoreboard() { }

        public Scoreboard(long id, long levelId, long score, long timeScore, long moment)
        {
            Id = id;
            LevelId = levelId;
            Score = score;
            TimeScore = timeScore;
            Moment = moment;
        }
    }
}