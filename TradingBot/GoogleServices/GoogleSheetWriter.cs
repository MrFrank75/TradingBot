using GoogleSheetsHelper;
using System.Drawing;

namespace TradingBot.GoogleServices
{
    public class GoogleSheetWriter
    {
        public async Task WriteCsvRowsIntoSheet(string sheetId, List<string> csvRows) {
            var gsh = new GoogleSheetsHelper.GoogleSheetsHelper("security-details.json", sheetId);

            var row1 = new GoogleSheetRow();
            var row2 = new GoogleSheetRow();

            var cell1 = new GoogleSheetCell() { CellValue = "Header 1", IsBold = true, BackgroundColor = Color.DarkGoldenrod };
            var cell2 = new GoogleSheetCell() { CellValue = "Header 2", BackgroundColor = Color.Cyan };

            var cell3 = new GoogleSheetCell() { CellValue = "Value 1" };
            var cell4 = new GoogleSheetCell() { CellValue = "Value 2" };

            row1.Cells.AddRange(new List<GoogleSheetCell>() { cell1, cell2 });
            row2.Cells.AddRange(new List<GoogleSheetCell>() { cell3, cell4 });

            var rows = new List<GoogleSheetRow>() { row1, row2 };

            gsh.AddCells(new GoogleSheetParameters() { SheetName = "Sheet1", RangeColumnStart = 1, RangeRowStart = 1 }, rows);
        }
    }
}
