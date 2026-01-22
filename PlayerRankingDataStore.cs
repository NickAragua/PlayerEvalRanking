using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WYSAPlayerRanker
{
    public enum PlayerOperationType
    {
        CurrentSeason,
        PreviousSeason,
        Eval
    }

    public class PlayerRankingDataStore
    {
        // raw list of all player evaluations
        public Dictionary<string, SeasonPlayerData> RawPlayerDatabase { get; set; } = new Dictionary<string, SeasonPlayerData>();

        public Dictionary<string, SeasonPlayerData> PreviousRawPlayerDatabase { get; set; } = new Dictionary<string, SeasonPlayerData>();

        public Dictionary<string, CoalescedPlayerData> CoalescedPlayerDataByName { get; set; } = new Dictionary<string, CoalescedPlayerData>();
        public Dictionary<string, CoalescedPlayerData> PreviousCoalescedData { get; set; } = new Dictionary<string, CoalescedPlayerData>();

        public void CreateBackup() 
        { 
            CopyCoalescedData(CoalescedPlayerDataByName, PreviousCoalescedData);
            CopyRawData(RawPlayerDatabase, PreviousRawPlayerDatabase);
        }
        
        public void RestoreFromBackup()
        {
            CopyCoalescedData(PreviousCoalescedData, CoalescedPlayerDataByName);
            CopyRawData(PreviousRawPlayerDatabase, RawPlayerDatabase);
        }

        private void CopyCoalescedData(Dictionary<string, CoalescedPlayerData> source, Dictionary<string, CoalescedPlayerData> destination)
        {
            destination.Clear();
            foreach (var kvp in source)
            {
                CoalescedPlayerData copiedData = new CoalescedPlayerData(kvp.Value);
                destination.Add(kvp.Key, copiedData);
            }
        }

        private void CopyRawData(Dictionary<string, SeasonPlayerData> source, Dictionary<string, SeasonPlayerData> destination)
        {
            destination.Clear();
            foreach (var kvp in source)
            {
                destination.Add(kvp.Key, kvp.Value);
            }
        }

        private void AddPlayerToRawDatabase(SeasonPlayerData player)
        {
            string key = $"{player.FullName}{player.SourceDataFile}";
            if (!RawPlayerDatabase.ContainsKey(key))
            {
                RawPlayerDatabase.Add(key, player);
            }
        }

        public string ProcessIndividualPlayer(SeasonPlayerData player, PlayerOperationType operationType)
        {
            AddPlayerToRawDatabase(player);

            String activityLog = String.Empty;
            CoalescedPlayerData coalescedPlayerData;

            if (!CoalescedPlayerDataByName.ContainsKey(player.FullName))
            {
                coalescedPlayerData = new CoalescedPlayerData()
                {
                    FullName = player.FullName
                };

                CoalescedPlayerDataByName.Add(player.FullName, coalescedPlayerData);
            } 
            else
            {
                coalescedPlayerData = CoalescedPlayerDataByName[player.FullName];
            }

            switch (operationType)
            {
                case PlayerOperationType.CurrentSeason:
                    coalescedPlayerData.CurrentSeasonScore = player.AverageScore;
                    break;
                case PlayerOperationType.PreviousSeason:
                    coalescedPlayerData.PreviousSeasonScore = player.AverageScore;
                    break;
                //case PlayerOperationType.Eval:
            }

            coalescedPlayerData.HasRedFlag |= player.RedFlag;

            return activityLog;
        }
    }
}
