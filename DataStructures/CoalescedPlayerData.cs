using System.ComponentModel;

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
        public double CombinedScore { get; set; }        
        public string PreviousTeam { get; set; }
        public int GradeLevel { get; set; }
        public bool HasRedFlag { get; set; }
        public bool HasAssociatedCoach { get; set; }
        public int PreviousTeamDivision { get; set; }

        

        [Browsable(false)]
        public string Key
        {
            get
            {
                return FullName.ToLower();
            }
        }
    }

    public static class CoalescedPlayerDataExtensions
    {
        public static void CalculateCombinedScore(this CoalescedPlayerData playerData, ApplicationSettings appSettings)
        {
            double divisionMultiplier = 1;
            if (playerData.PreviousTeamDivision > 0 && playerData.PreviousTeamDivision < appSettings.DivisionWeights.Count)
            {
                divisionMultiplier = appSettings.DivisionWeights[playerData.PreviousTeamDivision - 1];
            }

            // scenarios:
            // no data at all: just return a -1
            if (playerData.CurrentSeasonScore == 0 && playerData.PreviousSeasonScore == 0 && playerData.EvalScore == 0)
            {
                playerData.CombinedScore = -1;
            }
            // only eval scores: just return eval scores
            else if (playerData.CurrentSeasonScore == 0 && playerData.PreviousSeasonScore == 0 && playerData.EvalScore > 0)
            {
                playerData.CombinedScore = playerData.EvalScore;
            }
            // only previous season scores: just return previous season scores, with division weight
            else if (playerData.CurrentSeasonScore == 0 && playerData.PreviousSeasonScore > 0 && playerData.EvalScore == 0)
            {
                playerData.CombinedScore = playerData.PreviousSeasonScore * divisionMultiplier;
            }
            // only previous season and eval scores
            else if (playerData.CurrentSeasonScore == 0 && playerData.PreviousSeasonScore > 0 && playerData.EvalScore > 0)
            {
                // treat previous season score as weighted by both season weights
                double seasonWeight = appSettings.PreviousSeasonWeight + appSettings.CurrentSeasonWeight;
                playerData.CombinedScore = ((playerData.PreviousSeasonScore * seasonWeight) * divisionMultiplier +
                        (playerData.EvalScore * appSettings.AssessmentWeight));
            }
            // only current season scores: just  current season score, with division weight
            else if (playerData.CurrentSeasonScore > 0 && playerData.PreviousSeasonScore == 0 && playerData.EvalScore == 0)
            {
                playerData.CombinedScore = playerData.CurrentSeasonScore * divisionMultiplier;
            }
            // only current season and eval scores
            else if (playerData.CurrentSeasonScore > 0 && playerData.PreviousSeasonScore == 0 && playerData.EvalScore > 0)
            {
                double seasonWeight = appSettings.PreviousSeasonWeight + appSettings.CurrentSeasonWeight;
                playerData.CombinedScore = ((playerData.CurrentSeasonScore * seasonWeight) * divisionMultiplier +
                        (playerData.EvalScore * appSettings.AssessmentWeight));
            }
            // only current and previous season scores
            else if (playerData.CurrentSeasonScore > 0 && playerData.PreviousSeasonScore > 0 && playerData.EvalScore == 0)
            {
                double currentSeasonWeight = appSettings.CurrentSeasonWeight + appSettings.AssessmentWeight / 2.0;
                double previousSeasonWeight = appSettings.PreviousSeasonWeight + appSettings.AssessmentWeight / 2.0;
                playerData.CombinedScore = ((playerData.CurrentSeasonScore * currentSeasonWeight) * divisionMultiplier +
                        (playerData.PreviousSeasonScore * previousSeasonWeight) * divisionMultiplier);
            }
            // all three scores available: use complete formula
            else
            {
                playerData.CombinedScore = ((playerData.CurrentSeasonScore * appSettings.CurrentSeasonWeight) * divisionMultiplier +
                        (playerData.PreviousSeasonScore * appSettings.PreviousSeasonWeight) * divisionMultiplier +
                        (playerData.EvalScore * appSettings.AssessmentWeight));
            }

            // if no eval score is present, the combined score is further retracted
            if (playerData.EvalScore == 0)
            {
                playerData.CombinedScore *= (1 - appSettings.NoAssessmentPenalty);
            }

            playerData.HasRedFlag = playerData.CombinedScore < 0;
        }
    }
}
