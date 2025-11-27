using BitMiracle.LibTiff.Classic;
using CommunityToolkit.Mvvm.Input;
using Greener.Web.Definitions.API.Network;
using global::GreenerConfigurator.ClientCore.Services;
using GreenerConfigurator.ClientCore.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.OleDb;
using System.DirectoryServices.ActiveDirectory;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml;

namespace GreenerConfigurator.ViewModels.NetworkDevice
{
    public class UnassignedNetworkDeviceManagementViewModel : ViewModelBase
    {
        #region [ Constructor(s) ]

        private readonly NetworkDeviceService _networkDeviceService;

        public UnassignedNetworkDeviceManagementViewModel()
        {
            _networkDeviceService = App.ServiceProvider.GetRequiredService<NetworkDeviceService>();
            OnAddUnassignedNetworkDeviceCommand = new AsyncRelayCommand(AddUnassignedNetworkDeviceCommandAsync);

            LoadUnassignedNetworkDeviceAsync();
        }

        #endregion

        #region [ Public Property(s) ]

        public ICommand OnAddUnassignedNetworkDeviceCommand { get; private set; }

        public ObservableCollection<UnassignedNetworkDeviceDto> UnassignedNetworkDeviceList
        {
            get => _UnassignedNetworkDeviceList;
            set
            {
                _UnassignedNetworkDeviceList = value;
                OnPropertyChanged(nameof(UnassignedNetworkDeviceList));
            }
        }

        public string ImportUnassignedListError
        {
            get => _ImportUnassignedListError;
            set
            {
                _ImportUnassignedListError = value;
                OnPropertyChanged(nameof(ImportUnassignedListError));
            }
        }

        #endregion

        #region [ Private Field(s) ]

        private ObservableCollection<UnassignedNetworkDeviceDto> _UnassignedNetworkDeviceList;
        private string _ImportUnassignedListError = string.Empty;

        #endregion

        #region [ Private Metho(s) ]

        private async Task LoadUnassignedNetworkDeviceAsync()
        {
            var result = await _networkDeviceService.GetUnassignedNetworkDeviceAsync();
            UnassignedNetworkDeviceList = new ObservableCollection<UnassignedNetworkDeviceDto>(result);
        }

        private async Task AddUnassignedNetworkDeviceCommandAsync()
        {
            ImportUnassignedListError = string.Empty;

            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";
                openFileDialog.Title = "Select a Excel File";

                var resultDialog = openFileDialog.ShowDialog();
                if (resultDialog.HasValue && resultDialog.Value)
                {
                    string filePath = openFileDialog.FileName;
                    var tempJson = ReadExcelToJson(filePath);
                    var tempUnassignedList = JsonConvert.DeserializeObject<List<UnassignedNetworkDeviceDto>>(tempJson);

                    var tempValidateResult = ValidateImportedData(tempUnassignedList);
                    if (tempValidateResult)
                    {
                        var result = await _networkDeviceService.AddUnassignedNetworkDeviceAsync(tempUnassignedList);
                        if (result != null && result.Count > 0)
                        {
                            var tempNotAddedItems = result.Where(w => !w.IsAdded).ToList();
                            if (tempNotAddedItems.Count() > 0)
                            {
                                ImportUnassignedListError = "Following Serial Numbers are not added to the list : " + string.Join(", ", tempNotAddedItems.Select(s => s.SerialNumber));
                            }

                            LoadUnassignedNetworkDeviceAsync();
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                LogHelper.LogError(exp.ToString());
                ImportUnassignedListError = "Something goes wrong while adding the list. Please try again.";
            }
        }

        private bool ValidateImportedData(List<UnassignedNetworkDeviceDto> unassignedList)
        {
            bool result = true;

            unassignedList.GroupBy(g => g.SerialNumber).ToList().ForEach(g =>
            {
                if (g.Count() > 1)
                {
                    result = false;
                    ImportUnassignedListError += $" {g.Key}, ";
                    return;
                }
            });

            if (!result)
            {
                ImportUnassignedListError = "Duplicate Serial Number found in the list :" + ImportUnassignedListError;
            }

            var tempDuplicateSerialNumbers = FindDuplicateSerialNumbers(unassignedList);
            if (tempDuplicateSerialNumbers.Count > 0)
            {
                ImportUnassignedListError += "\r\n" + "Following Serial Numbers are already in the DB : " + string.Join(", ", tempDuplicateSerialNumbers);
                result = false;
            }

            return result;
        }

        private string ReadExcelToJson(string filePath)
        {
            string connString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={filePath};Extended Properties='Excel 12.0;HDR=YES;IMEX=1';";

            using (OleDbConnection conn = new OleDbConnection(connString))
            {
                conn.Open();
                DataTable dtSheet = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                string firstSheet = dtSheet.Rows[0]["TABLE_NAME"].ToString(); // Get first sheet name

                string query = $"SELECT * FROM [{firstSheet}]";
                using (OleDbDataAdapter adapter = new OleDbDataAdapter(query, conn))
                {
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    // Convert DataTable to List of Dictionaries
                    List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                    foreach (DataRow row in dataTable.Rows)
                    {
                        string serialNumber = row[nameof(UnassignedNetworkDeviceDto.SerialNumber)]?.ToString()?.Trim(); // Check Serial #
                        if (string.IsNullOrEmpty(serialNumber))
                        {
                            continue; // Skip this row if Serial # is empty or null
                        }

                        Dictionary<string, object> rowData = new Dictionary<string, object>();
                        foreach (DataColumn column in dataTable.Columns)
                        {
                            if (column.ColumnName == nameof(UnassignedNetworkDeviceDto.MAC))
                            {
                                rowData[nameof(UnassignedNetworkDeviceDto.MacRange)] = row[column].ToString().Trim().ToUpper();
                                var tempMac = row[column].ToString().ToUpper().Split('-').FirstOrDefault();
                                rowData[column.ColumnName] =tempMac.Trim() ?? ""; // Handle NULL values
                            }
                            else
                            {
                                rowData[column.ColumnName] = row[column].ToString().ToUpper() ?? ""; // Handle NULL values
                            }
                        }

                        rows.Add(rowData);
                    }

                    return JsonConvert.SerializeObject(rows); // JsonSerializer.Serialize(rows, new JsonSerializerOptions { WriteIndented = true });
                }
            }
        }

        private List<string> FindDuplicateSerialNumbers(List<UnassignedNetworkDeviceDto> importedDevices)
        {
            HashSet<string> existingSerialNumbers = new HashSet<string>(_UnassignedNetworkDeviceList.Select(d => d.SerialNumber));

            return importedDevices
                .Where(d => existingSerialNumbers.Contains(d.SerialNumber))
                .Select(d => d.SerialNumber)
                .Distinct()
                .ToList();
        }

        #endregion
    }
}
