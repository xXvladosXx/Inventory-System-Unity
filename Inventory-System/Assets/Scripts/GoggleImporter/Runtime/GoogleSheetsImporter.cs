using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using UnityEngine;

namespace GoggleImporter.Runtime
{
    public class GoogleSheetsImporter
    {
        private readonly SheetsService _sheetsService;
        private readonly string _sheetID;

        public GoogleSheetsImporter(string credentialsPath, string sheetID)
        {
            _sheetID = sheetID;
            
            GoogleCredential credential;

            using (var stream = new FileStream(credentialsPath, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream).CreateScoped(SheetsService.Scope.Spreadsheets);
            }
            
            _sheetsService = new SheetsService(new Google.Apis.Services.BaseClientService.Initializer
            {
                HttpClientInitializer = credential
            });
        }
        
        public async Task DownloadAndParseSheetAsync(string sheetName, IGoogleSheetParser googleSheetParser, int rowIncrement = 1)
        {
            var range = $"{sheetName}!A1:Z";
            var request = _sheetsService.Spreadsheets.Values.Get(_sheetID, range);

            ValueRange response;

            try
            {
                response = await request.ExecuteAsync();
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to download sheet: {e.Message}");
                return;
            }
            
            if (response != null && response.Values != null)
            {
                var tableArray = response.Values;
                var rowsCount = tableArray.Count;

                for (int i = 0; i < rowsCount; i += rowIncrement)
                {
                    var headerRow = tableArray[i];
                    var headers = headerRow.Select(cell => cell.ToString()).ToList();

                    if (i + 1 < rowsCount)
                    {
                        var dataRow = tableArray[i + 1];

                        googleSheetParser.ParseSheet(headers, dataRow);
                    }
                }

                Debug.Log("Sheet downloaded and parsed successfully");
            }
            else
            {
                Debug.LogWarning("Sheet is empty");
            }
        }
    }
}