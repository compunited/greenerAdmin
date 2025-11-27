using Greener.Web.Definitions.Enums;
using GreenerConfigurator.ClientCore.Models;

using System;
using System.Collections.Generic;
using System.Linq;

namespace GreenerConfigurator.ViewModels.Sensor
{
    public class SensorImportViewModel : ViewModelBase
    {
        private List<EnumModel<DeviceConnectionType>> _DeviceConnectionTypeItemList;

        public List<EnumModel<DeviceConnectionType>> DeviceConnectionTypeItemList
        {
            get => _DeviceConnectionTypeItemList;
            set
            {
                _DeviceConnectionTypeItemList = value;
                OnPropertyChanged(nameof(DeviceConnectionTypeItemList));
            }
        }

        public SensorImportViewModel()
        {
            var values = Enum.GetValues(typeof(DeviceConnectionType)).Cast<DeviceConnectionType>();
            DeviceConnectionTypeItemList = new List<EnumModel<DeviceConnectionType>>();

            foreach (var item in values)
            {
                DeviceConnectionTypeItemList.Add(new EnumModel<DeviceConnectionType>()
                {
                    EnumItem = item
                });
            }                       
        }
    }
}
