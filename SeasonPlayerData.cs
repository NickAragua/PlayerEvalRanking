using System.ComponentModel;

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
        public string SourceDataFile { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TeamName { get; set; }
        [Browsable(false)]
        public string EvaluatingCoach { get; set; }
        public int Division { get; set; }
        public int OrdinalRanking   { get; set; }
        public string Season { get; set; }
        public PlacementRecommendation PlacementRecommendation { get; set; }
        public int TechnicalScore { get; set; }
        public int TacticalScore { get; set; }
        public int MentalScore { get; set; }
        public int PhysicalScore { get; set; }
        public int AttendanceScore { get; set; }

        public double AverageScore
        {
            get
            {
                return (TechnicalScore + TacticalScore + MentalScore + PhysicalScore + AttendanceScore) / 5.0;
            }
        }

        public int GoalkeeperScore { get; set; }
        public string Comments { get; set; }
        public bool RedFlag { get; set; }

        // used as a key in the data store as well
        public string FullName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }
    }
}
