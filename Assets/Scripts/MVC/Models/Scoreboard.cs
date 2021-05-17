
using System;
using Utilities;

namespace MVC.Models
{
    public class Scoreboard
    {
        private long ID;
        private long LEVEL_ID;
        private long SCORE;
        private long TIME_SCORE;
        private long MOMENT;

        public long Id { get => ID; set => ID = value; }
        public long LevelId { get => LEVEL_ID; set => LEVEL_ID = value; }
        public long Score { get => SCORE; set => SCORE = value; }
        public long TimeScore { get => TIME_SCORE; set => TIME_SCORE = value; }
        public long Moment { get => MOMENT; set => MOMENT = value; }

        public Scoreboard() { }

        public Scoreboard(long id, long levelId, long score, long timeScore, long moment)
        {
            Id = id;
            LevelId = levelId;
            Score = score;
            TimeScore = timeScore;
            Moment = moment;
        }

        public override string ToString()
        {
            string s = "Id = {0}, Level Id = {1}, Score = {2}, Time Score = {3}, Moment = {4}";
            return string.Format(s, Id, LevelId, Formatter.FormatToCurrency(Score), Formatter.FormatToCurrency(TimeScore), DateTimeOffset.FromUnixTimeSeconds(Moment).ToString("yyyy-MM-dd HH:mm:ss"));
        }
    }
}