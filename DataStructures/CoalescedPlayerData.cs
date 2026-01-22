using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WYSAPlayerRanker
{
    public class CoalescedPlayerData
    {
        public CoalescedPlayerData()
        {

        }
        public CoalescedPlayerData(CoalescedPlayerData other)
        {
            FullName = other.FullName;
            CurrentSeasonScore = other.CurrentSeasonScore;
            PreviousSeasonScore = other.PreviousSeasonScore;
            EvalScore = other.EvalScore;
            HasAssociatedCoach = other.HasAssociatedCoach;
            HasRedFlag = other.HasRedFlag;
        }

        public string FullName { get; set; }
        public double CurrentSeasonScore { get; set; }
        public double PreviousSeasonScore { get; set; }
        public double EvalScore { get; set; }
        public string PreviousTeam { get; set; }
        public int GradeLevel { get; set; }
        public bool HasRedFlag { get; set; }
        public bool HasAssociatedCoach { get; set; }

        public double GetCombinedScore()
        {
            // Example weighted average calculation
            return (CurrentSeasonScore * 0.4) + (PreviousSeasonScore * 0.3) + (EvalScore * 0.3);
        }
    }
}
