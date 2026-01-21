using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WYSAPlayerRanker
{
    public enum PlacementRecommendation
    {
        MoveUp,
        MoveDown,
        Stay
    }

    public class SeasonPlayerData
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TeamName { get; set; }
        public int Division { get; set; }

        public int OrdinalRanking   { get; set; }
        public string Season { get; set; }
        public PlacementRecommendation PlacementRecommendation { get; set; }

        public double TechnicalScore { get; set; }
        public double TacticalScore { get; set; }
        public double MentalScore { get; set; }
        public double PhysicalScore { get; set; }
        public double AttendanceScore { get; set; }

        public double AverageScore
        {
            get
            {
                return (TechnicalScore + TacticalScore + MentalScore + PhysicalScore + AttendanceScore) / 5.0;
            }
        }

        public double GoalkeeperScore { get; set; }

        public string Comments { get; set; }
    }
}
