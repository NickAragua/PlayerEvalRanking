using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
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
        public Dictionary<string, List<SeasonPlayerData>> RawPlayerDatabase { get; set; } = new Dictionary<string, List<SeasonPlayerData>>();

        public Dictionary<string, List<SeasonPlayerData>> PreviousRawPlayerDatabase { get; set; } = new Dictionary<string, List<SeasonPlayerData>>();

        public Dictionary<string, CoalescedPlayerData> CoalescedPlayerDataByName { get; set; } = new Dictionary<string, CoalescedPlayerData>();
        
        public Dictionary<string, CoalescedPlayerData> PreviousCoalescedData { get; set; } = new Dictionary<string, CoalescedPlayerData>();

        public Dictionary<string, List<CoalescedPlayerData>> Teams { get; set; } = new Dictionary<string, List<CoalescedPlayerData>>();

        public Dictionary<string, PlayerRegistrationData> RegisteredPlayers { get; set; } = new Dictionary<string, PlayerRegistrationData>();

        public HashSet<string> ImportedEvals { get; set; } = new HashSet<string>();

        public ApplicationSettings ApplicationSettings { get; set; } = new ApplicationSettings();

        public void CreateBackup() 
        { 
            CopyCoalescedData(CoalescedPlayerDataByName, PreviousCoalescedData);
            CopyRawData(RawPlayerDatabase, PreviousRawPlayerDatabase);

            if (ApplicationSettings.AutoSave) 
            { 
                Serialize("autosave_player_rankings.json");
            }
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

        private void CopyRawData(Dictionary<string, List<SeasonPlayerData>> source, Dictionary<string, List<SeasonPlayerData>> destination)
        {
            destination.Clear();
            foreach (var kvp in source)
            {
                destination.Add(kvp.Key, new List<SeasonPlayerData>());
                foreach (var playerRecord in kvp.Value)
                {
                    SeasonPlayerData copiedRecord = new SeasonPlayerData(playerRecord);
                    destination[kvp.Key].Add(copiedRecord);
                }
            }
        }

        private void AddPlayerToRawDatabase(SeasonPlayerData player)
        {
            string key = player.Key;
            if (!RawPlayerDatabase.ContainsKey(key))
            {
                RawPlayerDatabase.Add(key, new List<SeasonPlayerData>());
            }

            if (!RawPlayerDatabase[key].Contains(player))
            {
                RawPlayerDatabase[key].Add(player);
            }
        }

        public void ImportFromLastSeasonMasterList(List<CoalescedPlayerData> lastSeasonPlayers)
        {
            CreateBackup();

            foreach (CoalescedPlayerData player in lastSeasonPlayers)
            {
                if (!CoalescedPlayerDataByName.ContainsKey(player.Key))
                {
                    CoalescedPlayerDataByName.Add(player.Key, player);
                }
                else
                {
                    CoalescedPlayerDataByName[player.Key].PreviousSeasonScore = player.PreviousSeasonScore;
                    CoalescedPlayerDataByName[player.Key].EvalScore = player.EvalScore;
                }

                CoalescedPlayerDataByName[player.Key].CalculateCombinedScore(ApplicationSettings);
            }
        }

        private double CalculateAveragePlayerScore(SeasonPlayerData player)
        {
            if (RawPlayerDatabase.ContainsKey(player.Key))
            {
                double totalScore = 0.0;
                int scoreCount = 0;

                foreach (SeasonPlayerData playerData in RawPlayerDatabase[player.Key])
                {
                    scoreCount++;
                    totalScore += playerData.AverageScore;
                }

                return totalScore / scoreCount;
            }

            return 0.0;
        }

        public string ProcessIndividualPlayer(SeasonPlayerData player, PlayerOperationType operationType)
        {
            AddPlayerToRawDatabase(player);

            ImportedEvals.Add(player.SourceDataFile);

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

            coalescedPlayerData.CurrentSeasonScore = CalculateAveragePlayerScore(player);

            if (RegisteredPlayers.ContainsKey(player.Key))
            {
                coalescedPlayerData.GradeLevel = RegisteredPlayers[player.Key].GradeLevel;
            }

            if (String.IsNullOrEmpty(coalescedPlayerData.PreviousTeam)) 
            {
                coalescedPlayerData.PreviousTeam = player.TeamName;
            }

            if (coalescedPlayerData.PreviousTeamDivision == 0)
            {
                coalescedPlayerData.PreviousTeamDivision = player.Division;
            }

            coalescedPlayerData.CalculateCombinedScore(ApplicationSettings);

            coalescedPlayerData.HasRedFlag |= player.RedFlag;

            return activityLog;
        }

        public void RecalculatePlayerScores()
        {
            foreach (var kvp in CoalescedPlayerDataByName)
            {
                kvp.Value.CalculateCombinedScore(ApplicationSettings);
            }
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

        /// <summary>
        /// Given a list of registrants, adds them to the registrant dictionary; updates grade levels in coalesced data
        /// </summary>
        public void ProcessRegisteredPlayers(List<PlayerRegistrationData> registrants)
        {
            RegisteredPlayers.Clear();
            foreach (var registrant in registrants)
            {
                if (!RegisteredPlayers.ContainsKey(registrant.Key))
                {
                    RegisteredPlayers.Add(registrant.Key, registrant);
                }

                if (CoalescedPlayerDataByName.ContainsKey(registrant.Key))
                {
                    CoalescedPlayerDataByName[registrant.Key].GradeLevel = registrant.GradeLevel;
                }
            }
        }

        public bool PlayerIsRegistered(CoalescedPlayerData playerData)
        {
            return RegisteredPlayers.ContainsKey(playerData.Key);
        }

        private void AddPlayerToTeam(string teamName, CoalescedPlayerData playerData)
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
