CREATE PROCEDURE TechnicalIndicatorsBarChart_Procedure
AS
BEGIN
    BEGIN TRY
        INSERT INTO TechnicalIndicatorsBarChart_Final
        (
            RecordId,
            Symbol,
            FullName,
            Industry,
            OneMonthMovingAverage,
            FiftyDayMovingAverage,
            TwoHundredDayMovingAverage,
            FiveDayRelationalStrength,
            FiftyDayRelationalStrength,
            OneHundredDayRelationalStrength,
            PriceToCashFlow,
            ReturnOnAssets,
            PriceToEarningsGrowth,
            EPSBasicLastQtr,
            MeanTargetPrice,
            HighTargetPrice,
            TwentyDayPercent,
            ThreeMonthPercent,
            FiftyDayPercent,
            OneHundredDayPercent,
            ThreeMonthMovingAverage,
            StandardDeviation,
            SixtyMonthBeta,
            FiftyTwoWeekChange,
            DataSource, 
            DateOfEntry
        )
        SELECT 
            NEWID() as RecordId,
            Symbol,
            FullName,
            Industry,
            ROUND(OneMonthMovingAverage, 2),
            ROUND(FiftyDayMovingAverage, 2),
            ROUND(TwoHundredDayMovingAverage, 2),
            ROUND(FiveDayRelationalStrength, 2),
            ROUND(FiftyDayRelationalStrength, 2),
            ROUND(OneHundredDayRelationalStrength, 2),
            ROUND(PriceToCashFlow, 2),
            ROUND(ReturnOnAssets, 2),
            ROUND(PriceToEarningsGrowth, 2),
            ROUND(EPSBasicLastQtr, 2),
            ROUND(MeanTargetPrice, 2),
            ROUND(HighTargetPrice, 2),
            ROUND(TwentyDayPercent, 2),
            ROUND(ThreeMonthPercent, 2),
            ROUND(FiftyDayPercent, 2),
            ROUND(OneHundredDayPercent, 2),
            ROUND(ThreeMonthMovingAverage, 2),
            ROUND(StandardDeviation, 2),
            ROUND(SixtyMonthBeta, 2),
            ROUND(FiftyTwoWeekChange, 2),
           'BarChart' AS DataSource, 
            GETDATE() AS DateOfEntry
       FROM TechnicalIndicatorsBarChart_Staging
       WHERE 
           (Symbol IS NOT NULL AND 
           FullName IS NOT NULL AND
           Industry IS NOT NULL AND
           OneMonthMovingAverage IS NOT NULL AND
           FiftyDayMovingAverage IS NOT NULL AND
           TwoHundredDayMovingAverage IS NOT NULL AND
           FiveDayRelationalStrength IS NOT NULL AND
           FiftyDayRelationalStrength IS NOT NULL AND
           OneHundredDayRelationalStrength IS NOT NULL AND
           PriceToCashFlow IS NOT NULL AND 
           ReturnOnAssets IS NOT NULL AND
           MeanTargetPrice IS NOT NULL AND
           HighTargetPrice IS NOT NULL AND
           TwentyDayPercent IS NOT NULL AND
           ThreeMonthPercent IS NOT NULL AND
           FiftyDayPercent IS NOT NULL AND
           OneHundredDayPercent IS NOT NULL AND
           ThreeMonthMovingAverage IS NOT NULL AND
           FiftyTwoWeekChange IS NOT NULL);
       DELETE FROM TechnicalIndicatorsBarChart_Staging
    END TRY
    BEGIN CATCH
        SELECT 
           ERROR_NUMBER() AS ErrorNumber,
           ERROR_LINE() AS ErrorLine,
           ERROR_MESSAGE() AS ErrorMessage;
    END CATCH
END;





