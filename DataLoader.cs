using System;
using System.IO;
using System.Collections.Generic;
using OfficeOpenXml;

namespace WYSAPlayerRanker
{
    public class ExcelPlayerDataLoader
    {
        private const int STARTING_ROW = 3;

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

        public static List<SeasonPlayerData> LoadPlayersFromExcel(string filePath)
        {
            var players = new List<SeasonPlayerData>();

            // Set EPPlus license context (required for EPPlus 5.0+)
            ExcelPackage.License.SetNonCommercialOrganization("Westford Youth Soccer Association");

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets[0]; // Load first sheet
                int rowCount = worksheet.Dimension?.Rows ?? 0;

                // Assuming first row contains headers
                for (int row = STARTING_ROW; row <= rowCount; row++)
                {
                    try
                    {
                        worksheet.Cells[3, 1].GetValue<string>();
                        var player = new SeasonPlayerData();
                        player.FirstName = worksheet.Cells[row, (int)Columns.FirstName].GetValue<string>();
                        player.LastName = worksheet.Cells[row, (int)Columns.LastName].GetValue<string>();
                        player.TeamName = worksheet.Cells[row, (int)Columns.Team].GetValue<string>();
                        player.Division = worksheet.Cells[row, (int)Columns.Div].GetValue<int>();
                        player.OrdinalRanking = worksheet.Cells[row, (int)Columns.OrdinalRanking].GetValue<int>();
                        player.Season = "Fall 2025";//GetCellValue(worksheet, row, "Season"),
                        player.PlacementRecommendation = ParsePlacementRecommendation(worksheet.Cells[row, (int)Columns.Placement].GetValue<string>());
                        player.TechnicalScore = worksheet.Cells[row, (int)Columns.Technical].GetValue<double>();
                        player.TacticalScore = worksheet.Cells[row, (int)Columns.Tactical].GetValue<double>();
                        player.MentalScore = worksheet.Cells[row, (int)Columns.Mental].GetValue<double>();
                        player.PhysicalScore = worksheet.Cells[row, (int)Columns.Physical].GetValue<double>();
                        player.AttendanceScore = worksheet.Cells[row, (int)Columns.Attendance].GetValue<double>();
                        player.GoalkeeperScore = worksheet.Cells[row, (int)Columns.GoalkeeperSkills].GetValue<double>();
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