using System;
using System.Windows.Controls;
using System.Collections.Generic;
using IronXL;
using System.Linq;
using Greener.Web.Definitions.Api.MasterData.Device;
using GreenerConfigurator.ClientCore.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using Greener.Web.Definitions.API.MasterData.Device.Manufacturer;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Greener.Web.Definitions.Enums;
using GreenerConfigurator.ClientCore.Models;

namespace GreenerConfigurator.Views.Sensor
{
    public partial class SensorImportView : UserControl
    {
        List<UnassignedPhysicalDeviceDto> KeyList = new List<UnassignedPhysicalDeviceDto>();
        private ObservableCollection<ManufacturerAndDeviceDetailDto> _manufacturerAndDeviceDetailList;

        public ObservableCollection<ManufacturerAndDeviceDetailDto> ManufacturerAndDeviceDetailList
        {
            get => _manufacturerAndDeviceDetailList;
            set
            {
                _manufacturerAndDeviceDetailList = value;
                OnPropertyChanged(nameof(ManufacturerAndDeviceDetailList));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private readonly UnassignedPhysicalDeviceService _unassignedPhysicalDeviceService;

        public SensorImportView()
        {
            _unassignedPhysicalDeviceService = App.ServiceProvider.GetRequiredService<UnassignedPhysicalDeviceService>();
            ManufacturerAndDeviceDetailList = new ObservableCollection<ManufacturerAndDeviceDetailDto>();
            InitializeComponent();
        }

        private void SelectFile_OnClick(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog();
            //dlg.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension

            Nullable<bool> result = openFileDialog.ShowDialog();
            KeyList.Clear();

            if (result == true)
            {
                lbFilename.Content = openFileDialog.FileName;
                WorkBook workbook = WorkBook.Load(openFileDialog.FileName);
                WorkSheet sheet = workbook.WorkSheets.First();

                for (int i = 3; i < sheet.RowCount; i++)
                {
                    var row = sheet.GetRow(i);
                    var item = new UnassignedPhysicalDeviceDto()
                    {
                        Barcode = row.Columns[0].Value.ToString(),
                        LoraWanDevEui = row.Columns[1].Value.ToString(),
                        LoraWanAppEui = row.Columns[2].Value.ToString(),
                        LoraWanJoinEui = row.Columns[3].Value.ToString()
                    };

                    KeyList.Add(item);
                }
            }
        }

        private async void Import_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                await _unassignedPhysicalDeviceService.ImportKeys(KeyList);
                MessageBox.Show("Successful importing keys");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void SelectOnClick(object sender, RoutedEventArgs e)
        {
            ManufacturerAndDeviceDetailList.Clear();
            var item = (e.Source as ComboBox).SelectedItem as EnumModel<DeviceConnectionType>;

            if (item != null)
            {
                var items = await _unassignedPhysicalDeviceService.GetManufacturerAndDeviceInfo(item.EnumItem);

                if (items != null)
                    ManufacturerAndDeviceDetailList = new ObservableCollection<ManufacturerAndDeviceDetailDto>(items.ManufacturerAndDeviceDetailDtos);
            }
            cmbImportManufacturer.ItemsSource = ManufacturerAndDeviceDetailList;
        }
    }
}