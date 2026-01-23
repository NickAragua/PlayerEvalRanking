using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json;
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

            if (!CoalescedPlayerDataByName.ContainsKey(player.Key))
            {
                coalescedPlayerData = new CoalescedPlayerData()
                {
                    FullName = player.FullName
                };

                CoalescedPlayerDataByName.Add(player.Key, coalescedPlayerData);
            } 
            else
            {
                coalescedPlayerData = CoalescedPlayerDataByName[player.Key];
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

            if (RegisteredPlayers.ContainsKey(player.Key))
            {
                coalescedPlayerData.GradeLevel = RegisteredPlayers[player.Key].GradeLevel;
            }

            coalescedPlayerData.PreviousTeam = player.TeamName;
            coalescedPlayerData.HasRedFlag |= player.RedFlag;

            return activityLog;
        }

        public void MovePlayerToTeam(CoalescedPlayerData playerData, string teamName)
        {
            CreateBackup();
            RemovePlayerFromCoalescedList(playerData);
            AddPlayerToTeam(teamName, playerData);
        }

        public void RemovePlayerFromTeam(CoalescedPlayerData playerData, string teamName)
        {
            CreateBackup();

            if (!CoalescedPlayerDataByName.ContainsKey(playerData.Key))
            {
                CoalescedPlayerDataByName.Add(playerData.Key, playerData);
            }

            RemovePlayerFromTeam(teamName, playerData);
        }

        public void RemovePlayerFromCoalescedList(CoalescedPlayerData playerData)
        {
            if(CoalescedPlayerDataByName.ContainsKey(playerData.Key))
            {
                CoalescedPlayerDataByName.Remove(playerData.Key);
            }
        }
        
        public void ProcessRegisteredPlayers(List<PlayerRegistrationData> registrants)
        {
            RegisteredPlayers.Clear();
            foreach (var registrant in registrants)
            {
                if (!RegisteredPlayers.ContainsKey(registrant.Key))
                {
                    RegisteredPlayers.Add(registrant.Key, registrant);
                }
            }
        }

        public bool PlayerIsRegistered(CoalescedPlayerData playerData)
        {
            return RegisteredPlayers.ContainsKey(playerData.Key);
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

        public void Serialize(string fileName)
        {
            string serializedData = JsonSerializer.Serialize(this);
            FileInfo destinationFile = new FileInfo(fileName);
            File.WriteAllText(destinationFile.FullName, serializedData);
        }

        public static PlayerRankingDataStore Deserialize(string fileName)
        {
            FileInfo sourceFile = new FileInfo(fileName);
            string serializedData = File.ReadAllText(sourceFile.FullName);
            PlayerRankingDataStore deserializedStore = JsonSerializer.Deserialize<PlayerRankingDataStore>(serializedData);

            return deserializedStore;
        }
    }
}
