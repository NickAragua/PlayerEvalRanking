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
        public SeasonPlayerData()
        {
        }

        public SeasonPlayerData(SeasonPlayerData other)
        {
            SourceDataFile = other.SourceDataFile;
            FirstName = other.FirstName;
            LastName = other.LastName;
            TeamName = other.TeamName;
            EvaluatingCoach = other.EvaluatingCoach;
            Division = other.Division;
            OrdinalRanking = other.OrdinalRanking;
            Season = other.Season;
            PlacementRecommendation = other.PlacementRecommendation;
            TechnicalScore = other.TechnicalScore;
            TacticalScore = other.TacticalScore;
            MentalScore = other.MentalScore;
            PhysicalScore = other.PhysicalScore;
            AttendanceScore = other.AttendanceScore;
            GoalkeeperScore = other.GoalkeeperScore;
            Comments = other.Comments;
            RedFlag = other.RedFlag;
        }

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

        public string FullName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }

        public string Key
        {
            get
            {
                return FullName.ToLower();
            }
        }
    }
}
