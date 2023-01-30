using GoogleSheetsHelper;
using System.Drawing;
using TradingBot.Models;

namespace TradingBot.GoogleServices
{
    public class GoogleSheetWriter
    {
        public async Task WriteCsvRowsIntoSheet(string sheetId, List<OrderBookCSVSnapshotEntry> csvEntry, int startingRow) {
            
            var gsh = new GoogleSheetsHelper.GoogleSheetsHelper("security-details.json", sheetId);
            var rows = new List<GoogleSheetRow>() {};

            foreach (var csvRow in csvEntry)
            {
                var row1 = new GoogleSheetRow();
                var cell1 = new GoogleSheetCell() { CellValue = csvRow.GenerationUtcDateTime };
                var cell2 = new GoogleSheetCell() { CellValue = csvRow.PriceLevel.ToString("0.00") };
                var cell3 = new GoogleSheetCell() { CellValue = csvRow.Quantity.ToString("0.00") };
                row1.Cells.AddRange(new List<GoogleSheetCell>() { cell1,cell2,cell3});
                rows.Add(row1);
            }

            gsh.AddCells(new GoogleSheetParameters() { SheetName = "RawData", RangeColumnStart = 1, RangeRowStart = startingRow }, rows);
        }
    }
}
