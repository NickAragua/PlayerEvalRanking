using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WYSAPlayerRanker.DataStructures;

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

        public Dictionary<string, List<CoalescedPlayerData>> Teams { get; set; } = new Dictionary<string, List<CoalescedPlayerData>>();

        public Dictionary<string, PlayerRegistrationData> RegisteredPlayers { get; set; } = new Dictionary<string, PlayerRegistrationData>();

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
                CoalescedPlayerData copiedRecord = new CoalescedPlayerData(kvp.Value);
                destination.Add(kvp.Key, copiedRecord);
            }
        }

        private void CopyRawData(Dictionary<string, SeasonPlayerData> source, Dictionary<string, SeasonPlayerData> destination)
        {
            destination.Clear();
            foreach (var kvp in source)
            {
                SeasonPlayerData copiedRecord = new SeasonPlayerData(kvp.Value);
                destination.Add(kvp.Key, copiedRecord);
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

            if (RegisteredPlayers.ContainsKey(player.FullName))
            {
                coalescedPlayerData.GradeLevel = RegisteredPlayers[player.FullName].GradeLevel;
            }

            coalescedPlayerData.PreviousTeam = player.TeamName;
            coalescedPlayerData.HasRedFlag |= player.RedFlag;

            return activityLog;
        }
        
        public void ProcessRegisteredPlayers(List<PlayerRegistrationData> registrants)
        {
            RegisteredPlayers.Clear();
            foreach (var registrant in registrants)
            {
                if (!RegisteredPlayers.ContainsKey(registrant.GetFullName()))
                {
                    RegisteredPlayers.Add(registrant.GetFullName(), registrant);
                }
            }
        }

        public void AddPlayerToTeam(string teamName, CoalescedPlayerData playerData)
        {
            if (!Teams.ContainsKey(teamName))
            {
                Teams[teamName] = new List<CoalescedPlayerData>();
            }

            if (!Teams[teamName].Contains(playerData))
            {
                Teams[teamName].Add(playerData);
            }
        }

        public void RemovePlayerFromTeam(string teamName, CoalescedPlayerData playerData)
        {
            if (Teams.ContainsKey(teamName))
            {
                Teams[teamName].Remove(playerData);
            }
        }

        public void AddNewTeam()
        {
            string teamName = $"Team {Teams.Count + 1}";
            Teams.Add(teamName, new List<CoalescedPlayerData>());
        }

        public void RemoveTeam(string teamName)
        {
            if (Teams.ContainsKey(teamName))
            {
                Teams.Remove(teamName);
            }
        }

        public List<CoalescedPlayerData> GetTeam(string teamName)
        {
            if (Teams.ContainsKey(teamName))
            {
                return Teams[teamName];
            }
            return new List<CoalescedPlayerData>();
        }

        public List<String> GetTeamNames()
        {
            return Teams.Keys.ToList();
        }
    }
}
