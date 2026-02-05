using System;
using System.IO;
using System.Collections.Generic;
using OfficeOpenXml;
using WYSAPlayerRanker.DataStructures;
using System.Text;

namespace WYSAPlayerRanker
{
    public class ExcelPlayerDataLoader
    {
        static ExcelPlayerDataLoader()
        {
            ExcelPackage.License.SetNonCommercialOrganization("Westford Youth Soccer Association");
        }

        private const int COACH_EVAL_STARTING_ROW = 3;

        private enum Columns
        {
            FirstName = 2,
            LastName = 1,
            Team = 3,
            Div = 4,
            OrdinalRanking = 5,
            Season = 99,
            Placement = 6,
            Technical = 7,
            Tactical = 8,
            Mental = 9,
            Physical = 10,
            Attendance = 11,
            GoalkeeperSkills = 12,
            AdditionalComments = 13
        }

        /// <summary>
        /// Given an exported xlsx from sportsconnect "age group report", loads a list of registered players
        /// </summary
        public static List<PlayerRegistrationData> LoadRegisteredPlayers(string filePath)
        {
            var registeredPlayers = new List<PlayerRegistrationData>();

            ExcelPackage.License.SetNonCommercialOrganization("Westford Youth Soccer Association");

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                ExcelWorksheet worksheet = null;

                foreach (var currentWorksheet in package.Workbook.Worksheets)
                {
                    if (currentWorksheet.Cells[1, 1]?.GetValue<string>() != "Program Name")
                    {
                        continue;
                    }

                    worksheet = currentWorksheet;
                    break;
                }

                int rowCount = worksheet?.Dimension?.Rows ?? 0;

                // Assuming first row contains headers
                for (int row = 2; row <= rowCount; row++)
                {
                    try
                    {
                        var player = new PlayerRegistrationData();
                        player.FirstName = worksheet.Cells[row, 5].GetValue<string>();
                        player.LastName = worksheet.Cells[row, 4].GetValue<string>();
                        player.GradeLevel = Int32.Parse(worksheet.Cells[row, 6].GetValue<string>().Substring(0, 1));
                        player.PreviousTeam = worksheet.Cells[row, 10].GetValue<string>().Replace("Westford ", "");

                        if (string.IsNullOrWhiteSpace(player.FirstName) && string.IsNullOrWhiteSpace(player.LastName))
                        {
                            // Skip empty rows
                            continue;
                        }

                        registeredPlayers.Add(player);
                    }
                    catch (Exception ex)
                    {
                        // Log or handle parsing errors for individual rows
                        System.Diagnostics.Debug.WriteLine($"Error parsing row {row}: {ex.Message}");
                    }
                }
            }

            return registeredPlayers;
        }

        /// <summary>
        /// Given a coach eval xlsx file path, loads a list of SeasonPlayerData
        /// </summary>
        public static List<SeasonPlayerData> LoadPlayersFromExcel(string filePath, out string errorLog)
        {
            var players = new List<SeasonPlayerData>();
            StringBuilder sb = new StringBuilder();

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                ExcelWorksheet worksheet = null;
                int startingRow;

                foreach (var currentWorksheet in package.Workbook.Worksheets)
                {
                    if (currentWorksheet.Cells[1, 1]?.GetValue<string>() == "Player")
                    {
                        // Fall 2025 and beyond sheets start with "Player" in A1
                        worksheet = currentWorksheet;
                        startingRow = 3;
                        break;
                    }
                    else if (currentWorksheet.Cells[1, 1]?.GetValue<string>() == "Last Name")
                    {
                        // spring 2025 sheets start with "Last Name" in A1
                        worksheet = currentWorksheet;
                        startingRow = 2;
                        break;
                    }
                }

                if (worksheet == null)
                {
                    sb.Append("No valid worksheet found in the provided Excel file.");
                }

                int rowCount = worksheet?.Dimension?.Rows ?? 0;

                // Assuming first row contains headers
                for (int row = COACH_EVAL_STARTING_ROW; row <= rowCount; row++)
                {
                    try
                    {
                        var player = new SeasonPlayerData();
                        player.SourceDataFile = Path.GetFileName(filePath);
                        player.FirstName = worksheet.Cells[row, (int)Columns.FirstName].GetValue<string>();
                        player.LastName = worksheet.Cells[row, (int)Columns.LastName].GetValue<string>();
                        player.TeamName = worksheet.Cells[row, (int)Columns.Team].GetValue<string>();
                        player.Division = worksheet.Cells[row, (int)Columns.Div].GetValue<int>();
                        player.OrdinalRanking = worksheet.Cells[row, (int)Columns.OrdinalRanking].GetValue<int>();
                        player.Season = "Fall 2025";//GetCellValue(worksheet, row, "Season"),
                        player.PlacementRecommendation = ParsePlacementRecommendation(worksheet.Cells[row, (int)Columns.Placement].GetValue<string>());
                        player.TechnicalScore = worksheet.Cells[row, (int)Columns.Technical].GetValue<int>();
                        player.TacticalScore = worksheet.Cells[row, (int)Columns.Tactical].GetValue<int>();
                        player.MentalScore = worksheet.Cells[row, (int)Columns.Mental].GetValue<int>();
                        player.PhysicalScore = worksheet.Cells[row, (int)Columns.Physical].GetValue<int>();
                        player.AttendanceScore = worksheet.Cells[row, (int)Columns.Attendance].GetValue<int>();
                        player.GoalkeeperScore = worksheet.Cells[row, (int)Columns.GoalkeeperSkills].GetValue<int>();
                        player.Comments = worksheet.Cells[row, (int)Columns.AdditionalComments].GetValue<string>();

                        if (string.IsNullOrWhiteSpace(player.FirstName) && string.IsNullOrWhiteSpace(player.LastName))
                        {
                            // Skip empty rows
                            continue;
                        }

                        players.Add(player);
                    }
                    catch (Exception ex)
                    {
                        // Log or handle parsing errors for individual rows
                        sb.Append($"Error parsing row {row}: {ex.Message}");
                    }
                }
            }

            errorLog = sb.ToString();
            return players;
        }

        public static List<CoalescedPlayerData> LoadMasterList(string filePath, out string season)
        {
            var players = new List<CoalescedPlayerData>();
            season = "S26"; // default for now

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                foreach (var currentWorksheet in package.Workbook.Worksheets)
                {
                    if (currentWorksheet.Cells[1, 1]?.GetValue<string>() != "Program")
                    {
                        continue;
                    }

                    switch (currentWorksheet.Cells[1, 4]?.GetValue<string>())
                    {
                        case "Grade":
                            season = "F25";
                            return LoadMasterListF25(currentWorksheet);
                        case "Birth Date":
                        default:
                            season = "S26";
                            return LoadMasterListF26(currentWorksheet);
                    }
                }
            }

            return players;
        }

        /// <summary>
        /// Loads a list of CoalescedPlayerData from last season's master player list
        /// with coach eval populated as "previous season score" and 
        /// </summary>
        public static List<CoalescedPlayerData> LoadMasterListF26(ExcelWorksheet worksheet)
        {
            var players = new List<CoalescedPlayerData>();

            int rowCount = worksheet?.Dimension?.Rows ?? 0;
            // Assuming first row contains headers
            for (int row = 2; row <= rowCount; row++)
            {
                try
                {
                    var player = new CoalescedPlayerData();
                    player.FullName = worksheet.Cells[row, 3].GetValue<string>() + " " +
                        worksheet.Cells[row, 2].GetValue<string>();

                    player.EvalScore = worksheet.Cells[row, 9].GetValue<double>();
                    player.PreviousTeam = worksheet.Cells[row, 7].GetValue<string>();

                    if (!String.IsNullOrEmpty(player.PreviousTeam))
                    {
                        player.PreviousTeam = player.PreviousTeam.Replace("Westford ", "");
                    }

                    player.CurrentSeasonScore = worksheet.Cells[row, 10].GetValue<double>();
                    player.PreviousSeasonScore = worksheet.Cells[row, 12].GetValue<double>();
                    player.CombinedScore = worksheet.Cells[row, 16].GetValue<double>();

                    if (string.IsNullOrWhiteSpace(player.FullName))
                    {
                        // Skip empty rows
                        continue;
                    }

                    players.Add(player);
                }
                catch (Exception ex)
                {
                    // Log or handle parsing errors for individual rows
                    System.Diagnostics.Debug.WriteLine($"Error parsing row {row}: {ex.Message}");
                }
            }

            return players;
        }

        /// <summary>
        /// Loads a list of CoalescedPlayerData from last season's master player list
        /// with coach eval populated as "previous season score" and 
        /// </summary>
        public static List<CoalescedPlayerData> LoadMasterListF25(ExcelWorksheet worksheet)
        {
            var players = new List<CoalescedPlayerData>();
                        
            int rowCount = worksheet?.Dimension?.Rows ?? 0;
            // Assuming first row contains headers
            for (int row = 2; row <= rowCount; row++)
            {
                try
                {
                    var player = new CoalescedPlayerData();
                    player.FullName = worksheet.Cells[row, 3].GetValue<string>() + " " +
                        worksheet.Cells[row, 2].GetValue<string>();
                    player.PreviousSeasonScore = worksheet.Cells[row, 12].GetValue<double>();
                    player.EvalScore = worksheet.Cells[row, 13].GetValue<double>();
                    player.PreviousTeamDivision = worksheet.Cells[row, 8].GetValue<int>();

                    if (string.IsNullOrWhiteSpace(player.FullName))
                    {
                        // Skip empty rows
                        continue;
                    }

                    players.Add(player);
                }
                catch (Exception ex)
                {
                    // Log or handle parsing errors for individual rows
                    System.Diagnostics.Debug.WriteLine($"Error parsing row {row}: {ex.Message}");
                }
            }

            return players;
        }

        public static void ExportTeams(string destinationFile, PlayerRankingDataStore dataStore)
        {
            if(File.Exists(destinationFile)) 
            {                 
                File.Delete(destinationFile);
            }

            using (var package = new ExcelPackage(new FileInfo(destinationFile)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Teams");

                int tableWidth = 7;
                int tableHeight = 21;
                int headerHeight = 3;

                int currentX = 1;
                int currentY = 1;
                int teamCounter = 0;

                foreach (string teamKey in dataStore.Teams.Keys)
                {
                    AddHeaders(worksheet, teamKey, currentX, currentY);
                    AddPlayers(worksheet, currentX + headerHeight, currentY, dataStore.Teams[teamKey]);
                    
                    currentY += tableWidth;
                    teamCounter++;

                    if (teamCounter % 5 == 0)
                    {
                        currentY = 1;
                        currentX += tableHeight;
                    }
                }

                package.Save();
            }
        }

        private static void AddPlayers(ExcelWorksheet worksheet, int x, int y, List<CoalescedPlayerData> players)
        {
            int currentRow = x;

            foreach (CoalescedPlayerData player in players)
            {
                worksheet.Cells[currentRow, y].Value = player.CombinedScore.ToString("F2");
                worksheet.Cells[currentRow, y + 2].Value = player.FullName.Split(' ')[0];
                worksheet.Cells[currentRow, y + 3].Value = player.FullName.Split(' ')[1];
                worksheet.Cells[currentRow, y + 4].Value = player.GradeLevel.ToString();
                worksheet.Cells[currentRow, y + 5].Value = player.EvalScore.ToString("F2");
                currentRow++;
            }
        }

        private static void AddHeaders(ExcelWorksheet worksheet, String teamName, int x, int y)
        {
            // note: x is the row, y is the column
            worksheet.Cells[x, y].Value = "[Division Here]";
            worksheet.Cells[x, y + 2].Value = "Coaches";
            worksheet.Cells[x + 1, y].Value = teamName;
            worksheet.Cells[x + 1, y + 2].Value = "[Coach List Here]";
            worksheet.Cells[x + 2, y].Value = "Overall";
            worksheet.Cells[x + 2, y + 1].Value = "#";
            worksheet.Cells[x + 2, y + 2].Value = "Last Name";
            worksheet.Cells[x + 2, y + 3].Value = "First Name";
            worksheet.Cells[x + 2, y + 4].Value = "Grade";
            worksheet.Cells[x + 2, y + 5].Value = "Assess";
        }

        private static PlacementRecommendation ParsePlacementRecommendation(string value)
        {
            switch (value)
            {
                case "MOVE UP":
                    return PlacementRecommendation.MoveUp;
                case "MOVE DOWN":
                    return PlacementRecommendation.MoveDown;
                default:
                    return PlacementRecommendation.Stay;
            }
        }
    }
}