const APP_SCRIPT_VERSION = "V0.5.2";

function doPost(e) {
    const logger = new LocalLogger();
    logger.init();

    logger.log("App Script Version: " + APP_SCRIPT_VERSION, LocalLogger.Severity.INFO);
    // #region Request Validation

    if (!e || !e.postData || !e.postData.contents) {
        logger.log("e or postData is missing" , LocalLogger.Severity.ERROR);
        return ContentService.createTextOutput("Invalid request");
    }

    const data = JSON.parse(e.postData.contents);
    logger.log("DATA: " + JSON.stringify(data), LocalLogger.Severity.DEBUG);

    // check API key
    const validKey = "Lalalal1sa";
    if (data.apiKey !== validKey) {
        logger.log("Invalid API Key: " + data.apiKey, LocalLogger.Severity.ERROR);
        return ContentService.createTextOutput("Unauthorized");
    }
    // #end regrion

    // #region Data Validation

    // Select sheet
    let sheet = null;
    if (data.isCollectAdvancedData === true) {
        sheet = SpreadsheetApp.getActiveSpreadsheet().getSheetByName("AdvanceData");
    } else {
        sheet = SpreadsheetApp.getActiveSpreadsheet().getSheetByName("NormalData");
    }
    if (!sheet) {
        logger.log("Sheet not found: " + (data.isCollectAdvancedData ? "AdvanceData" : "NormalData"), LocalLogger.Severity.ERROR);
        return ContentService.createTextOutput("Sheet not found");
    }

    // #endregion

    // #region Setup Headers
    const summaryHeaders = [
        "Timestamp", "Version", "UniqueID", "UserName", "Gender", "Age", "Topic", "Difficulty", "UserType",
        "Score", "MaxScore", "Accuracy", "Correct", "Wrong", "Stars", "TimeUsed"
    ];

    let currentHeaders = [];
    const lastCol = sheet.getLastColumn();
    if (lastCol >= 1) {
    currentHeaders = sheet.getRange(1, 1, 1, lastCol).getValues()[0];
    }

    // Only set headers if the first cell is empty (sheet is new) or headers are missing
    let shouldSetHeaders = currentHeaders.length === 0 || currentHeaders[0] === "";
    if (shouldSetHeaders) {
    for (let i = 0; i < summaryHeaders.length; i++) {
        sheet.getRange(1, i + 1).setValue(summaryHeaders[i]);
    }
    }
    // #endregion

    // #region Prepare Data Row
    const baseRow = [
        new Date(),
        data.appversion ?? "",
        data.uniqueId ?? "", 
        data.userName ?? "",
        data.gender ?? "",
        data.age ?? "",
        data.topic ?? "",
        data.difficulty ?? "",
        data.userType ?? "",
        data.score ?? "",
        data.maxScore ?? "",
        data.accuracy ?? "",
        data.correct ?? "",
        data.wrong ?? "",
        data.stars ?? "",
        data.timeTaken ?? ""
    ];

    // Prepare advanced answer columns and headers
    let answerColumns = [];
    let answerHeaders = [];

    if (data.isCollectAdvancedData && Array.isArray(data.answerDetails)) {
        for (let i = 0; i < data.answerDetails.length; i++) {
            const a = data.answerDetails[i];
            answerColumns.push(
                a.questionText ?? "",
                a.selectedAnswer ?? "",
                a.correctAnswer ?? "",
                a.timeToAnswer ?? "",
                a.wasCorrect ?? false
            );
            answerHeaders.push(
                `Q${i + 1}_Text`,
                `Q${i + 1}_Answer`,
                `Q${i + 1}_Correct`,
                `Q${i + 1}_Time`,
                `Q${i + 1}_WasCorrect`
            );
        }

        // Only check/set headers if needed
        let needSetAdvancedHeaders = false;
        for (let j = 0; j < answerHeaders.length; j++) {
            const headerCell = sheet.getRange(1, summaryHeaders.length + j + 1).getValue();
            if (headerCell !== answerHeaders[j]) {
                needSetAdvancedHeaders = true;
                break;
            }
        }
        if (needSetAdvancedHeaders) {
            for (let j = 0; j < answerHeaders.length; j++) {
                sheet.getRange(1, summaryHeaders.length + j + 1).setValue(answerHeaders[j]);
            }
        }
    }

    // Combine all columns
    const fullRow = baseRow.concat(answerColumns);

    // Ensure enough columns exist
    const totalColumns = fullRow.length;
    const existingColumnCount = sheet.getLastColumn();
    if (totalColumns > existingColumnCount) {
        sheet.insertColumnsAfter(existingColumnCount, totalColumns - existingColumnCount);
    }
    // Append the row
    sheet.appendRow(fullRow);
    logger.log("Data row appended: " + JSON.stringify(fullRow), LocalLogger.Severity.INFO);
    const lastRow = sheet.getLastRow();
    sheet.getRange(lastRow, 1, 1, fullRow.length).setHorizontalAlignment("center");

    return ContentService.createTextOutput("Success");
}
