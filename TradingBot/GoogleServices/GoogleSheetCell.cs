namespace GoogleSheetsHelper
{
    public class GoogleSheetCell
    {
        public string CellValue { get; set; }
        public bool IsBold { get; set; }
        public System.Drawing.Color BackgroundColor { get; set; } = System.Drawing.Color.White;

        public String NumberFormatPattern { get; set; }
    }
}