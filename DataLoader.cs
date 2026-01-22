using System;
using System.IO;
using System.Collections.Generic;
using OfficeOpenXml;
using WYSAPlayerRanker.DataStructures;

namespace WYSAPlayerRanker
{
    public class ExcelPlayerDataLoader
    {
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

        public static List<SeasonPlayerData> LoadPlayersFromExcel(string filePath)
        {
            var players = new List<SeasonPlayerData>();

            // Set EPPlus license context (required for EPPlus 5.0+)
            ExcelPackage.License.SetNonCommercialOrganization("Westford Youth Soccer Association");

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                ExcelWorksheet worksheet = null;

                foreach (var currentWorksheet in package.Workbook.Worksheets)
                {
                    if (currentWorksheet.Cells[1, 1]?.GetValue<string>() != "Player")
                    {
                        continue;
                    }

                    worksheet = currentWorksheet;
                    break;
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
                        System.Diagnostics.Debug.WriteLine($"Error parsing row {row}: {ex.Message}");
                    }
                }
            }

            return players;
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