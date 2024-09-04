using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using UnityEngine;

namespace GoggleImporter
{
    public class GoogleSheetsImporter
    {
        private readonly SheetsService _sheetsService;
        private readonly string _sheetID;
        private readonly List<string> _headers = new List<string>();

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
        
        public async Task DownloadAndParseSheetAsync(string sheetName, IGoogleSheetParser googleSheetParser)
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
                
                var firstRow = tableArray[0];
                foreach (var cell in firstRow)
                {
                    _headers.Add(cell.ToString());
                }
                
                var rowsCount = tableArray.Count;
                for (int i = 1; i < rowsCount; i++)
                {
                    var row = tableArray[i];
                    var rowLength = row.Count;
                    googleSheetParser.ParseSheet(_headers, row);

                    for (int j = 0; j < rowLength; j++)
                    {
                        var cell = row[j];
                        var header = _headers[j];
                        
                        
                        Debug.Log($"Header: {header}, Cell: {cell}");
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