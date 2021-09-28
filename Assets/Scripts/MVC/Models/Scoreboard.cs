
using System;
using Utilities;

namespace MVC.Models
{
    /// <summary>
    /// Scoreboard data
    /// </summary>
    public class Scoreboard
    {
        private long ID;
        private long LEVEL_ID;
        private long SCORE;
        private long TIME_SCORE;
        private long MOMENT;

        /// <summary>
        /// Database generated id
        /// </summary>
        public long Id { get => ID; set => ID = value; }

        /// <summary>
        /// Related Level Id
        /// </summary>
        public long LevelId { get => LEVEL_ID; set => LEVEL_ID = value; }

        /// <summary>
        /// Total Score
        /// </summary>
        public long Score { get => SCORE; set => SCORE = value; }

        /// <summary>
        /// Total Time Score
        /// </summary>
        public long TimeScore { get => TIME_SCORE; set => TIME_SCORE = value; }

        /// <summary>
        /// Played Moment
        /// </summary>
        public long Moment { get => MOMENT; set => MOMENT = value; }
    }
}