using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Phanerozoic.Core.Entities;
using Phanerozoic.Core.Services.Interfaces;

namespace Phanerozoic.Core.Services.Googles
{
    public class GoogleSheetsService : IGoogleSheetsService
    {
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/sheets.googleapis.com-dotnet-quickstart.json
        private string[] _scopes = { SheetsService.Scope.Spreadsheets };

        private string _applicationName = "Phanerozoic.Core Library";
        private GoogleCredentialType _credentialType;
        private string _credentialsPath = "credentials.json";
        private UserCredential _userCredential;
        private GoogleCredential _serviceAccountCredential;

        public GoogleSheetsService(IServiceProvider serviceProvider)
        {
            IConfiguration configuration = serviceProvider.GetService<IConfiguration>();

            _credentialType = Enum.Parse<GoogleCredentialType>(configuration["Google:Credential:Type"]);
            _credentialsPath = configuration["Google:Credential:File"];

            Console.WriteLine($"Google API Credential Type: {_credentialType.ToString()}");
        }

        private ICredential GetCredential(GoogleCredentialType credentialType, string credentialsPaht)
        {
            if (credentialType == GoogleCredentialType.User)
            {
                return GetUserCredential(credentialsPaht);
            }
            else
            {
                return GetServiceCredential(credentialsPaht);
            }
        }

        private ICredential GetUserCredential(string credentialsPath)
        {
            if (_userCredential == null)
            {
                //// TODO 提醒使用者在瀏覽器頁面開啟權限

                using (var stream =
                    new FileStream(credentialsPath, FileMode.Open, FileAccess.Read))
                {
                    // The file token.json stores the user's access and refresh tokens, and is created
                    // automatically when the authorization flow completes for the first time.
                    string credPath = "token.json";
                    _userCredential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.Load(stream).Secrets,
                        _scopes,
                        "Phanerozoic",
                        CancellationToken.None,
                        new FileDataStore(credPath, true)).Result;
                    Console.WriteLine("Credential file saved to: " + credPath);
                }
            }

            return _userCredential;
        }

        private ICredential GetServiceCredential(string credentialsPath)
        {
            if (_serviceAccountCredential == null)
            {
                _serviceAccountCredential = GoogleCredential.FromFile(credentialsPath).CreateScoped(_scopes);
            }
            return _serviceAccountCredential;
        }

        private SheetsService GetSheets(ICredential credential)
        {
            // Create Google Sheets API service.
            return new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = _applicationName,
            });
        }

        public IList<IList<object>> GetValues(string spreadsheetId, string range)
        {
            var sheetsService = GetSheetsService();

            // Define request parameters.
            SpreadsheetsResource.ValuesResource.GetRequest request = sheetsService.Spreadsheets.Values.Get(spreadsheetId, range);
            request.ValueRenderOption = SpreadsheetsResource.ValuesResource.GetRequest.ValueRenderOptionEnum.FORMULA;

            // Prints the names and majors of students in a sample spreadsheet:
            ValueRange response = request.Execute();
            IList<IList<object>> values = response.Values;
            return values;
        }

        public void SetValue(string spreadsheetId, string range, IList<IList<object>> values)
        {
            var sheetsService = GetSheetsService();

            // TODO: Assign values to desired properties of `requestBody`. All existing
            // properties will be replaced:
            ValueRange requestBody = new ValueRange
            {
                Values = values,
            };

            SpreadsheetsResource.ValuesResource.UpdateRequest request = sheetsService.Spreadsheets.Values.Update(requestBody, spreadsheetId, range);
            request.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;

            // To execute asynchronously in an async method, replace `request.Execute()` as shown:
            UpdateValuesResponse response = request.Execute();
            // Data.UpdateValuesResponse response = await request.ExecuteAsync();

            // TODO: Change code below to process the `response` object:
            //Console.WriteLine(JsonSerializer.Serialize(response));
        }

        public void CreateSheet(string spreadsheetId, string sheetName)
        {
            var sheetService = GetSheetsService();

            // Add new Sheet
            var addSheetRequest = new AddSheetRequest();
            addSheetRequest.Properties = new SheetProperties();
            addSheetRequest.Properties.Title = sheetName;
            BatchUpdateSpreadsheetRequest batchUpdateSpreadsheetRequest = new BatchUpdateSpreadsheetRequest();
            batchUpdateSpreadsheetRequest.Requests = new List<Request>();
            batchUpdateSpreadsheetRequest.Requests.Add(new Request
            {
                AddSheet = addSheetRequest
            });

            var batchUpdateRequest = sheetService.Spreadsheets.BatchUpdate(batchUpdateSpreadsheetRequest, spreadsheetId);

            batchUpdateRequest.Execute();
        }

        private SheetsService GetSheetsService()
        {
            var credential = GetCredential(_credentialType, _credentialsPath);
            return GetSheets(credential);
        }
    }
}