using System;
using System.IO;
using System.Collections.Generic;
using OfficeOpenXml;
using WYSAPlayerRanker.DataStructures;
using System.Text;
using System.Linq;
using OfficeOpenXml.Style;
using System.Drawing;

namespace WYSAPlayerRanker
{
    public class ExcelPlayerDataLoader
    {
        static ExcelPlayerDataLoader()
        {
            ExcelPackage.License.SetNonCommercialOrganization("Westford Youth Soccer Association");
        }

        private const int COACH_EVAL_STARTING_ROW = 3;

        private static readonly Dictionary<Columns, int> ColumnIndexes2026 = new Dictionary<Columns, int>() {
            { Columns.FirstName, 2 },
            { Columns.LastName, 1 },
            { Columns.Grade, 3 },
            { Columns.Team, 4 },
            { Columns.Div, 5 },
            { Columns.OrdinalRanking, 6 },
            { Columns.Placement, 7 },
            { Columns.Technical, 8 },
            { Columns.Tactical, 9 },
            { Columns.Mental, 10 },
            { Columns.Physical, 11 },
            { Columns.Attendance, 12 },
            { Columns.GoalkeeperSkills, 14 },
            { Columns.AdditionalComments, 15 }
        };

        private static readonly Dictionary<Columns, int> ColumnIndexes2025 = new Dictionary<Columns, int>() {
            { Columns.FirstName, 2 },
            { Columns.LastName, 1 },
            { Columns.Grade, 100 },
            { Columns.Team, 3 },
            { Columns.Div, 4 },
            { Columns.OrdinalRanking, 5 },
            { Columns.Placement, 6 },
            { Columns.Technical, 7 },
            { Columns.Tactical, 8 },
            { Columns.Mental, 9 },
            { Columns.Physical, 10 },
            { Columns.Attendance, 11 },
            { Columns.GoalkeeperSkills, 12 },
            { Columns.AdditionalComments, 13 }
        };

        private enum Columns
        {
            FirstName,
            LastName,
            Grade,
            Team,
            Div,
            OrdinalRanking,
            Season,
            Placement,
            Technical,
            Tactical,
            Mental,
            Physical,
            Attendance,
            GoalkeeperSkills,
            AdditionalComments
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
                        player.Email = worksheet.Cells[row, 23].GetValue<string>();

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

            var columnMapping = filePath.Contains("S26") ? ColumnIndexes2026 : ColumnIndexes2025;
            String season = filePath.Contains("S26") ? "S26" : "F25";

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                ExcelWorksheet worksheet = null;

                foreach (var currentWorksheet in package.Workbook.Worksheets)
                {
                    if (currentWorksheet.Cells[1, 1]?.GetValue<string>() == "Player")
                    {
                        // Fall 2025 and beyond sheets start with "Player" in A1
                        worksheet = currentWorksheet;
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
                        player.FirstName = worksheet.Cells[row, columnMapping[Columns.FirstName]].GetValue<string>();
                        player.LastName = worksheet.Cells[row, columnMapping[Columns.LastName]].GetValue<string>();
                        player.TeamName = worksheet.Cells[row, columnMapping[Columns.Team]].GetValue<string>();
                        player.Division = worksheet.Cells[row, columnMapping[Columns.Div]].GetValue<int>();
                        player.OrdinalRanking = worksheet.Cells[row, columnMapping[Columns.OrdinalRanking]].GetValue<int>();
                        player.GradeLevel = worksheet.Cells[row, columnMapping[Columns.Grade]] != null ?
                            worksheet.Cells[row, columnMapping[Columns.Grade]].GetValue<int>() : -1;
                        player.Season = season;
                        player.PlacementRecommendation = ParsePlacementRecommendation(worksheet.Cells[row, columnMapping[Columns.Placement]].GetValue<string>());
                        player.TechnicalScore = worksheet.Cells[row, columnMapping[Columns.Technical]].GetValue<int>();
                        player.TacticalScore = worksheet.Cells[row, columnMapping[Columns.Tactical]].GetValue<int>();
                        player.MentalScore = worksheet.Cells[row, columnMapping[Columns.Mental]].GetValue<int>();
                        player.PhysicalScore = worksheet.Cells[row, columnMapping[Columns.Physical]].GetValue<int>();
                        player.AttendanceScore = worksheet.Cells[row, columnMapping[Columns.Attendance]].GetValue<int>();
                        player.GoalkeeperScore = worksheet.Cells[row, columnMapping[Columns.GoalkeeperSkills]].GetValue<int>();
                        player.Comments = worksheet.Cells[row, columnMapping[Columns.AdditionalComments]].GetValue<string>();

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

                    return LoadMasterListS26(currentWorksheet);
                    /*switch (currentWorksheet.Cells[1, 4]?.GetValue<string>())
                    {
                        case "Grade":
                            season = "F25";
                            return LoadMasterListF25(currentWorksheet);
                        case "Birth Date":
                        default:
                            season = "S26";
                            return LoadMasterListF26(currentWorksheet);
                    }*/
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

                    player.JerseyNumber = worksheet.Cells[row, 8].GetValue<int>();
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

        /// <summary>
        /// Loads a list of CoalescedPlayerData from last season's master player list
        /// with coach eval populated as "previous season score" and 
        /// </summary>
        public static List<CoalescedPlayerData> LoadMasterListS26(ExcelWorksheet worksheet)
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
                    player.PreviousTeam = worksheet.Cells[row, 7].GetValue<string>();
                    player.CurrentSeasonScore = worksheet.Cells[row, 9].GetValue<double>();
                    player.PreviousSeasonScore = worksheet.Cells[row, 10].GetValue<double>();
                    player.EvalScore = worksheet.Cells[row, 12].GetValue<double>();
                    player.CombinedScore = worksheet.Cells[row, 13].GetValue<double>();
                    //player.PreviousTeamDivision = worksheet.Cells[row, 8].GetValue<int>();

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
                    AddPlayers(worksheet, currentX + headerHeight, currentY, dataStore.GetTeam(teamKey));
                    
                    currentY += tableWidth;
                    teamCounter++;

                    if (teamCounter % 5 == 0)
                    {
                        currentY = 1;
                        currentX += tableHeight;
                    }
                }

                ExcelWorksheet exceptionSheet = package.Workbook.Worksheets.Add("Exceptions");
                SetExceptionHeaders(exceptionSheet);

                package.Save();
            }
        }

        private static void SetExceptionHeaders(ExcelWorksheet worksheet)
        {
            worksheet.Cells[1, 1].Value = "Name";
            worksheet.Cells[1, 2].Value = "Team data places them on";
            worksheet.Cells[1, 3].Value = "Team you are placing them on";
            worksheet.Cells[1, 4].Value = "Reason";
        }

        private static void AddPlayers(ExcelWorksheet worksheet, int x, int y, List<CoalescedPlayerData> players)
        {
            int currentRow = x;

            foreach (CoalescedPlayerData player in players)
            {
                worksheet.Cells[currentRow, y].Value = player.CombinedScore.ToString("F2");
                worksheet.Cells[currentRow, y].Style.Fill.PatternType = ExcelFillStyle.DarkGray;
                worksheet.Cells[currentRow, y].Style.Fill.BackgroundColor.SetColor(Color.Gray);
                worksheet.Cells[currentRow, y].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                worksheet.Cells[currentRow, y + 2].Value = player.FullName.Substring(player.FullName.IndexOf(' ') + 1);
                worksheet.Cells[currentRow, y + 2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                worksheet.Cells[currentRow, y + 3].Value = player.FullName.Split(' ')[0];
                worksheet.Cells[currentRow, y + 3].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                worksheet.Cells[currentRow, y + 4].Value = player.GradeLevel.ToString();
                worksheet.Cells[currentRow, y + 4].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                worksheet.Cells[currentRow, y + 5].Value = player.EvalScore.ToString("F2");
                worksheet.Cells[currentRow, y + 5].Style.Fill.PatternType = ExcelFillStyle.DarkGray;
                worksheet.Cells[currentRow, y + 5].Style.Fill.BackgroundColor.SetColor(Color.Gray);
                worksheet.Cells[currentRow, y + 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                currentRow++;
            }
        }

        private static void AddHeaders(ExcelWorksheet worksheet, String teamName, int x, int y)
        {
            // note: x is the row, y is the column
            worksheet.Cells[x, y].Value = "[Division Here]";
            worksheet.Cells[x, y].Style.Font.Bold = true;
            worksheet.Cells[x, y].Style.Fill.PatternType = ExcelFillStyle.DarkGray;
            worksheet.Cells[x, y].Style.Fill.BackgroundColor.SetColor(Color.Yellow);

            worksheet.Cells[x, y + 2].Value = "Coaches";
            worksheet.Cells[x, y + 2].Style.Font.Bold = true;

            worksheet.Cells[x + 1, y].Value = teamName;
            worksheet.Cells[x + 1, y].Style.Font.Bold = true;

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