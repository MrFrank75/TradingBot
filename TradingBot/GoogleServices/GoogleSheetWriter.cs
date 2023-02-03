using Google.Apis.Sheets.v4.Data;
using GoogleSheetsHelper;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Diagnostics;
using System.Drawing;
using TradingBot.OrderBook;

namespace TradingBot.GoogleServices
{

    public class GoogleSheetWriter : IGoogleSheetWriter
    {
        private readonly IPriceRangeQuantizer _priceRangeQuantizer;

        public GoogleSheetWriter(IPriceRangeQuantizer priceRangeQuantizer)
        {
            _priceRangeQuantizer = priceRangeQuantizer;
        }
        public void WriteCsvRowsIntoSheet(string sheetId, List<OrderBookCSVSnapshotEntry> csvEntry, int startingRow)
        {

            var gsh = new GoogleSheetsHelper.GoogleSheetsHelper("security-details.json", sheetId);
            var rows = new List<GoogleSheetRow>() { };
            var seriesId = 1;   //dummy value needed by Google Sheet to generate bubble charts

            foreach (var csvRow in csvEntry)
            {
                var row1 = new GoogleSheetRow();

                long dateTimeNumber;
                var cell1 = new GoogleSheetCell() { CellValue = csvRow.GenerationUtcDateTime };
                
                var conversionSuccesful = long.TryParse(csvRow.GenerationUtcDateTime, out dateTimeNumber);
                if (conversionSuccesful) { cell1.NumberValue= dateTimeNumber; };

                var cell2 = new GoogleSheetCell() { NumberValue = csvRow.PriceLevel, NumberFormatPattern = "" };
                var cell3 = new GoogleSheetCell() { NumberValue = csvRow.Quantity, NumberFormatPattern = "" };
                var cell4 = new GoogleSheetCell() { CellValue = seriesId.ToString()};
                row1.Cells.AddRange(new List<GoogleSheetCell>() { cell1, cell2, cell4 });
                rows.Add(row1);
            }

            gsh.AddCells(new GoogleSheetParameters() { SheetName = "RawData", RangeColumnStart = 1, RangeRowStart = startingRow }, rows);
        }

        public void WritePriceLevelsRangeWithQuantitiesIntoSheetByColumn(string sheetId, List<OrderBookCSVSnapshotEntry> priceLevelEntries, double lowPriceRange, double highPriceRange, double priceGranularity, int column)
        {

            var gsh = new GoogleSheetsHelper.GoogleSheetsHelper("security-details.json", sheetId);
            var priceLevelRows = new List<GoogleSheetRow>() { };

            Dictionary<int,double> priceLevelQuantities= new Dictionary<int, double>();
            var maxLevel = _priceRangeQuantizer.GetMaxLevel(lowPriceRange, highPriceRange, priceGranularity);
            foreach (var priceLevel in priceLevelEntries)
            {
                var rowNumber = _priceRangeQuantizer.QuantizePrice(priceLevel.PriceLevel, lowPriceRange, highPriceRange, priceGranularity);
                if (priceLevelQuantities.ContainsKey(rowNumber))
                    priceLevelQuantities[rowNumber]+=priceLevel.Quantity;
                else
                    priceLevelQuantities.Add(rowNumber, priceLevel.Quantity);
            }

            var headerCell = new GoogleSheetCell() { CellValue = priceLevelEntries.First().GenerationUtcDateTime};
            var headerRow = new GoogleSheetRow();
            headerRow.Cells.AddRange(new List<GoogleSheetCell>() { headerCell});
            priceLevelRows.Add(headerRow);

            for (int level = 0; level <= maxLevel; level++)
            {
                var singlePriceLevelQuantity = new GoogleSheetRow();
                var cell3 = new GoogleSheetCell() { NumberValue = 0, NumberFormatPattern = "" };
                if (priceLevelQuantities.ContainsKey(level))
                    cell3.NumberValue = priceLevelQuantities[level];
                singlePriceLevelQuantity.Cells.AddRange(new List<GoogleSheetCell>() { cell3 });
                priceLevelRows.Add(singlePriceLevelQuantity);
            }

            gsh.AddCells(new GoogleSheetParameters() { SheetName = "RawData", RangeColumnStart = column, RangeRowStart = 1 }, priceLevelRows);
        }


        public int WritePriceLevelsRangeWithQuantitiesIntoSheetByRow(string sheetId, List<OrderBookCSVSnapshotEntry> priceLevelEntries, double lowPriceRange, double highPriceRange, double priceGranularity, double currentAssetPrice, int startingRow)
        {
            if (priceLevelEntries.Count == 0)
                return 0;

            var cellDateTime = new GoogleSheetCell() { CellValue = priceLevelEntries.First().GenerationUtcDateTime };
            long dateTimeNumber;
            var conversionSuccessful = long.TryParse(priceLevelEntries.First().GenerationUtcDateTime, out dateTimeNumber);
            if (conversionSuccessful) { cellDateTime.NumberValue = dateTimeNumber; };


            var gsh = new GoogleSheetsHelper.GoogleSheetsHelper("security-details.json", sheetId);
            var priceLevelRows = new List<GoogleSheetRow>() { };
            var maxLevel = _priceRangeQuantizer.GetMaxLevel(lowPriceRange, highPriceRange, priceGranularity);
            double seriesIdForBuySellRanges = 1;    //dummy value required by Google Sheet
            double seriesIdForCurrentAssetPrice = 2;    //dummy value required by Google Sheet
            var fullVolume = priceLevelEntries.Sum(entry => entry.Quantity);

            //add 1 row with the actual price of the asset
            GoogleSheetRow currentAssetPriceRow = BuildGoogleSheetRow(cellDateTime, seriesIdForCurrentAssetPrice, currentAssetPrice, (fullVolume*0.01));
            priceLevelRows.Add(currentAssetPriceRow);

            for (int level = 0; level <= maxLevel; level++)
            {
                var priceLevel = lowPriceRange + (priceGranularity * level);
                var quantityForLevel = priceLevelEntries.
                    Where(entry => _priceRangeQuantizer.QuantizePrice(entry.PriceLevel, lowPriceRange, highPriceRange, priceGranularity) == level).
                    Aggregate(0.0, (myinitialQuantity, entry) => myinitialQuantity + entry.Quantity);

                GoogleSheetRow singlePriceLevelQuantity = BuildGoogleSheetRow(cellDateTime, seriesIdForBuySellRanges, priceLevel, quantityForLevel);
                priceLevelRows.Add(singlePriceLevelQuantity);
            }

            gsh.AddCells(new GoogleSheetParameters() { SheetName = "RawData", RangeColumnStart = 1, RangeRowStart = startingRow }, priceLevelRows);
            return priceLevelRows.Count;
        }

        private static GoogleSheetRow BuildGoogleSheetRow(GoogleSheetCell cellDateTime, double seriesIdForBuySellRanges, double priceLevel, double quantityForLevel)
        {
            var singlePriceLevelQuantity = new GoogleSheetRow();
            var cellPrice = new GoogleSheetCell() { NumberValue = priceLevel, NumberFormatPattern = "" };
            var cellQuantity = new GoogleSheetCell() { NumberValue = quantityForLevel, NumberFormatPattern = "" };
            var cellSeriesId = new GoogleSheetCell() { NumberValue = seriesIdForBuySellRanges, NumberFormatPattern = "" };
            singlePriceLevelQuantity.Cells.AddRange(new List<GoogleSheetCell>() { cellDateTime, cellPrice, cellSeriesId, cellQuantity });
            return singlePriceLevelQuantity;
        }

        public void WriteReferencePriceLevelRanges(string sheetId, double lowPriceRange, double highPriceRange, double priceGranularity)
        {
            var gsh = new GoogleSheetsHelper.GoogleSheetsHelper("security-details.json", sheetId);
            var priceLevelRows = new List<GoogleSheetRow>() { };
            var maxLevel = _priceRangeQuantizer.GetMaxLevel(lowPriceRange, highPriceRange, priceGranularity);

            var headerCell = new GoogleSheetCell() { CellValue = "PRICE LEVELS" };
            var headerRow = new GoogleSheetRow();
            headerRow.Cells.AddRange(new List<GoogleSheetCell>() { headerCell });
            priceLevelRows.Add(headerRow);

            for (int level = 0; level <=maxLevel; level++)
            {
                double priceLevel = lowPriceRange + (priceGranularity * level);
                var cell3 = new GoogleSheetCell() { NumberValue = priceLevel, NumberFormatPattern = "" };
                var singlePriceLevel = new GoogleSheetRow();
                singlePriceLevel.Cells.AddRange(new List<GoogleSheetCell>() { cell3 });
                priceLevelRows.Add(singlePriceLevel);
            }

            gsh.AddCells(new GoogleSheetParameters() { SheetName = "RawData", RangeColumnStart = 1, RangeRowStart = 1 }, priceLevelRows);
        }
    }
}
