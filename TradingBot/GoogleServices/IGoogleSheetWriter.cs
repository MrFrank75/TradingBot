﻿using TradingBot.OrderBook;

namespace TradingBot.GoogleServices
{
    public interface IGoogleSheetWriter
    {
        void WriteCsvRowsIntoSheet(string sheetId, List<OrderBookCSVSnapshotEntry> csvEntry, int startingRow);
        void WritePriceLevelsRangeWithQuantitiesIntoSheetByColumn(string sheetId, List<OrderBookCSVSnapshotEntry> priceLevelEntries, double lowPriceRange, double highPriceRange, double priceGranularity, int startingColumn);
        int  WritePriceLevelsRangeWithQuantitiesIntoSheetByRow(string sheetId, List<OrderBookCSVSnapshotEntry> priceLevelEntries, double lowPriceRange, double highPriceRange, double priceGranularity, int percVolumeToFilterOut, double currentAssetPrice, int startingRow);
        void WriteReferencePriceLevelRanges(string sheetId, double lowPriceRange, double highPriceRange, double priceGranularity);
    }
}