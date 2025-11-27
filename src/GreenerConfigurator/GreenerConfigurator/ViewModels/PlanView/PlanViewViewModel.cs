using Greener.Web.Definitions.API.Plans;
using Greener.Web.Definitions.API.Plans.Configurator.CreateDtos;
using Greener.Web.Definitions.API.Plans.Configurator.UpdateDtos;
using Greener.Web.Definitions.API.Plans.Configurator.ViewDtos;
using GreenerConfigurator.ClientCore.Models;
using GreenerConfigurator.ClientCore.Services;
using Microsoft.Extensions.DependencyInjection;
using global::GreenerConfigurator.ClientCore.Utilities;
using GreenerConfigurator.ClientCore.Utilities.Enumerations;
using GreenerConfigurator.Utilities;
using GreenerConfigurator.ClientCore.Utilities.MUI;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;

using BitmapSource = System.Windows.Media.Imaging.BitmapSource;

namespace GreenerConfigurator.ViewModels.PlanView
{
    public class PlanViewViewModel : ViewModelBase
    {
        #region [ Constructor(s) ]

        private readonly PlanViewService _planViewService;
        private readonly DeviceStateService _deviceStateService;
        private readonly LoraWanDataRowsService _loraWanDataRowsService;

        public PlanViewViewModel(LocationModel selectedLocation, PlanItemResultDto planView, PageStatus pageStatus)
        {
            _planViewService = App.ServiceProvider.GetRequiredService<PlanViewService>();
            _deviceStateService = App.ServiceProvider.GetRequiredService<DeviceStateService>();
            _loraWanDataRowsService = App.ServiceProvider.GetRequiredService<LoraWanDataRowsService>();
            OnCancelPlanViewCommand = new AsyncRelayCommand(CancelPlanViewCommandAsync);
            OnSavePlanViewCommand = new AsyncRelayCommand(SavePlanViewCommandAsync);
            OnDeleteElementCommand = new AsyncRelayCommand(DeleteElementCommandAsync);

            PlanViewPageStatus = pageStatus;

            SelectedLocation = selectedLocation;

            planItemResultDto = planView;
            PlanView = planView.PlanViewDto;
            PlanImage = planView.PlanImageDto;

            Init();
        }

        #endregion

        #region [ Public Property(s) ]

        public ICommand OnSavePlanViewCommand { get; private set; }

        public ICommand OnCancelPlanViewCommand { get; private set; }

        public ICommand OnDeleteElementCommand { get; private set; }

        public LocationModel SelectedLocation
        {
            get => _SelectedLocation;
            set
            {
                _SelectedLocation = value;
                OnPropertyChanged(nameof(SelectedLocation));
            }
        }

        public bool IsPlanViewEditEnable
        {
            get => _IsPlanViewEditEnable;
            set
            {
                _IsPlanViewEditEnable = value;
                OnPropertyChanged(nameof(_IsPlanViewEditEnable));
            }
        }

        public bool IsImageEnable
        {
            get => _IsImageEnable;
            set
            {
                _IsImageEnable = value;
                OnPropertyChanged(nameof(IsImageEnable));
            }
        }

        public PageStatus PlanViewPageStatus
        {
            get => _PlanViewPageStatus;
            set
            {
                _PlanViewPageStatus = value;
                switch (_PlanViewPageStatus)
                {
                    case PageStatus.Add:
                        {
                        }
                        break;
                    case PageStatus.Edit:
                        { }
                        break;
                    case PageStatus.Delete:
                        {
                            ButtonOkText = Language.Delete;
                            IsImageEnable = false;
                        }
                        break;
                }
                OnPropertyChanged(nameof(PlanViewPageStatus));
            }
        }

        public string ButtonOkText
        {
            get => _ButtonOkText;
            set
            {
                _ButtonOkText = value;
                OnPropertyChanged(nameof(ButtonOkText));
            }
        }

        public PlanViewDto PlanView
        {
            get => _PlanView;
            set
            {
                _PlanView = value;
                OnPropertyChanged(nameof(PlanView));
            }
        }

        public PlanImageDto PlanImage
        {
            get => _PlanImage;
            set
            {
                _PlanImage = value;
                OnPropertyChanged(nameof(PlanImage));
            }
        }

        public BitmapSource PlanBitMap
        {
            get => _PlanBitMap;
            set
            {
                _PlanBitMap = value;
                OnPropertyChanged(nameof(PlanBitMap));
            }
        }

        public ObservableCollection<PlanElementModel> PlanElements
        {
            get => _PlanElements;
            set
            {
                _PlanElements = value;
                OnPropertyChanged(nameof(PlanElements));
            }
        }

        public PlanElementModel SelectedPlanElement
        {
            get => _SelectedPlanElement;
            set
            {
                _SelectedPlanElement = value;
                OnPropertyChanged(nameof(SelectedPlanElement));
            }
        }

        public ObservableCollection<PhysicalDeviceModel> PhysicalDevices
        {
            get => _PhysicalDevices;
            set
            {
                _PhysicalDevices = value;
                OnPropertyChanged(nameof(PhysicalDevices));
            }
        }

        public PhysicalDeviceModel SelectedPhysicalDevice
        {
            get => _SelectedPhysicalDevice;
            set
            {
                _SelectedPhysicalDevice = value;
                ManageLoadLoraWanDataAsync();
                OnPropertyChanged(nameof(SelectedPhysicalDevice));
            }

        }

        public ObservableCollection<LoraWanDataRowsModel> LoraWanList
        {
            get => _LoraWanList;
            set
            {
                _LoraWanList = value;
                OnPropertyChanged(nameof(LoraWanList));
            }
        }

        public LoraWanDataRowsModel SelectedLoraWan
        {
            get => _SelectedLoraWan;
            set
            {
                _SelectedLoraWan = value;
                OnPropertyChanged(nameof(SelectedLoraWan));
            }
        }

        public string ErrorMessage
        {
            get => _ErrorMessage;
            set
            {
                _ErrorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public Visibility ErrorMessageVisibility
        {
            get => _ErrorMessageVisibility;
            set
            {
                _ErrorMessageVisibility = value;
                if (_ErrorMessageVisibility == Visibility.Collapsed)
                {
                    ErrorMessage = string.Empty;
                }

                OnPropertyChanged(nameof(ErrorMessageVisibility));
            }
        }

        #endregion

        #region [ Public Method(s) ]

        public void ManageMouseClickOnImage(float x, float y)
        {
            var bitMap = ImageHelper.BitmapFromSource(PlanBitMap);
            ImageHelper.AddDataOnImage(bitMap, $"{SelectedPhysicalDevice.PhysicalDeviceName}_{SelectedLoraWan.DataDetailNumber}", x, y);

            PlanElements.Add(new PlanElementModel()
            {
                PlanElementId = Guid.NewGuid(),
                DataDetailNumber = SelectedLoraWan.DataDetailNumber,
                DeviceName = SelectedPhysicalDevice.PhysicalDeviceName,
                Label = $"{SelectedPhysicalDevice.PhysicalDeviceName}_{SelectedLoraWan.DataDetailNumber}",
                PositionX = x,
                PositionY = y,
                PhysicalDeviceId = SelectedPhysicalDevice.PhysicalDeviceId,
            });

            SelectedPlanElement = PlanElements.Last();

            PlanBitMap = ImageHelper.BitmapSourceBitMap(bitMap);

            OnPropertyChanged(nameof(PlanBitMap));
        }

        private const bool V = false;

        #endregion

        #region [ Private Field(s) ]

        private PlanItemResultDto planItemResultDto = null;
        private LocationModel _SelectedLocation = null;

        private bool _IsPlanViewEditEnable = false;
        private bool _IsImageEnable = true;
        private PageStatus _PlanViewPageStatus = PageStatus.Idle;
        private string _ButtonOkText = Language.Save;
        private PlanViewDto _PlanView = null;
        private PlanImageDto _PlanImage = null;
        private BitmapSource _PlanBitMap = null;
        private ObservableCollection<PlanElementModel> _PlanElements = null;
        private List<LoraWanDataRowsModel> loraWanDataRowsModels = null;
        private ObservableCollection<PhysicalDeviceModel> _PhysicalDevices = null;
        private PhysicalDeviceModel _SelectedPhysicalDevice = null;
        private ObservableCollection<LoraWanDataRowsModel> _LoraWanList = null;
        private LoraWanDataRowsModel _SelectedLoraWan = null;
        private PlanElementModel _SelectedPlanElement = null;
        private string _ErrorMessage = string.Empty;
        private Visibility _ErrorMessageVisibility = Visibility.Collapsed;

        #endregion

        #region [ Private Method(s) ]

        private void Init()
        {
            bool loadOnlyImage = true;
            if (PlanViewPageStatus != PageStatus.Add)
            {
                loadOnlyImage = false;
            }

            GetListOfDevicesAsync();

            List<PlanElementModel> tempPlanElements = new List<PlanElementModel>();
            _PlanView.ElementDtos.ForEach(element =>
            {
                tempPlanElements.Add(new PlanElementModel(element));
            });

            PlanElements = new ObservableCollection<PlanElementModel>(tempPlanElements);
            ProcessImageAsync(loadOnlyImage);
        }

        private async Task AddPlanViewCommandAsync()
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "All Images|*.jpg;*.png"; // Filter files by extension

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                string filename = dialog.FileName;

                PlanView = new PlanViewDto();
                PlanView.LocationId = SelectedLocation.LocationId;
                PlanView.ElementDtos = new List<PlanElement>();
                _PlanImage = new PlanImageDto();

                byte[] imageArray = System.IO.File.ReadAllBytes(filename);
                //byte[] imageArray = System.IO.File.ReadAllBytes(Directory.GetCurrentDirectory() + "/Assets/images.jpg");

                _PlanImage.ImageAsBase64 = Convert.ToBase64String(imageArray);

                byte[] imageAsBytes = Convert.FromBase64String(_PlanImage.ImageAsBase64);
                MemoryStream memoryStream = new MemoryStream(imageArray);
                memoryStream.Position = 0;
                var bitMap = (Bitmap)Bitmap.FromStream(memoryStream);

                PlanBitMap = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                                                                bitMap.GetHbitmap(),
                                                                IntPtr.Zero,
                                                                System.Windows.Int32Rect.Empty,
                                                                BitmapSizeOptions.FromWidthAndHeight(bitMap.Width, bitMap.Height));

                PlanViewPageStatus = PageStatus.Add;
            }
        }

        private async Task SavePlanViewCommandAsync()
        {
            try
            {
                ErrorMessageVisibility = Visibility.Collapsed;
                var result = false;
                if (PlanViewPageStatus == PageStatus.Add)
                {
                    result = await ManageCreatePlanViewAsync();
                }
                else
                {
                    result = await ManageUpdatePlanViewAsync();
                }

                if (result)
                {
                    var temp = Application.Current;
                    ((MainWindow)temp.MainWindow).MainWindowModel.Navigator.CurrentViewModel = new PlanViewManagementViewModel(SelectedLocation);
                }
                else
                {
                    //TODO: temporary solution to show error on this page
                    ErrorMessage = "Process was not sucessfull, please try it again";
                    ErrorMessageVisibility = Visibility.Visible;
                }
            }
            catch (Exception exp)
            {
                //TODO: temporary solution to show error on this page
                ErrorMessage = exp.Message;
                ErrorMessageVisibility = Visibility.Visible;
            }
        }

        private async Task<bool> ManageCreatePlanViewAsync()
        {
            bool result = false;

            PlanViewForCreateDto createDto = new PlanViewForCreateDto();
            createDto.Name = PlanView.Name;
            createDto.Description = PlanView.Description;
            createDto.LocationId = SelectedLocation.LocationId;
            createDto.PlanImageDto = _PlanImage;
            createDto.ElementDtos = PlanElements.ToList<PlanElement>();
            if (createDto.PlanImageDto.ImageMimeType == "image/jpg")
            {
                createDto.PlanImageDto.ImageMimeType = "image/jpeg";
            }
            createDto.Validate();

            result = await _planViewService.CreatePlanViewAsync(createDto);

            return result;
        }

        private async Task<bool> ManageUpdatePlanViewAsync()
        {
            bool result = false;

            PlanViewUpdateDto updateDto = new PlanViewUpdateDto();
            updateDto.Name = PlanView.Name;
            updateDto.Description = PlanView.Description;
            updateDto.LocationId = SelectedLocation.LocationId;
            updateDto.ElementDtos = PlanElements.ToList<PlanElement>();
            updateDto.PlanViewRefId = PlanView.PlanViewRefId;
            updateDto.Validate();

            result = await _planViewService.UpdatePlanViewAsync(updateDto);

            return result;
        }

        private async Task CancelPlanViewCommandAsync()
        {
            var temp = Application.Current;
            ((MainWindow)temp.MainWindow).MainWindowModel.Navigator.CurrentViewModel = new PlanViewManagementViewModel(SelectedLocation);
        }

        private async Task DeleteElementCommandAsync()
        {
            if (_SelectedPlanElement != null)
            {
                PlanElements.Remove(_SelectedPlanElement);

                // Check if there are any remaining elements before selecting the next one
                if (PlanElements.Any())
                {
                    SelectedPlanElement = PlanElements.First();
                    await ProcessImageAsync(false);
                }
                else
                {
                    // If no elements are left, set SelectedPlanElement to null and display a message
                    SelectedPlanElement = null;
                    ErrorMessage = "No elements remaining.";
                    ErrorMessageVisibility = Visibility.Visible;
                }
            }
            else
            {
                // Display an error message if no element is selected for deletion
                ErrorMessage = "No element selected for deletion.";
                ErrorMessageVisibility = Visibility.Visible;
            }
        }


        private async Task GetDeviceStatesByLocationIdAsync()
        {
            var tempDevices = await _deviceStateService.GetDeviceStateByLocationIdAsync(SelectedLocation.LocationId);

        }

        private Task ProcessImageAsync(bool loadOnlyImage = true)
        {
            return Task.Run(() =>
            {
                try
                {
                    PlanBitMap = ImageHelper.BitmapSourceFromBase64(_PlanImage.ImageAsBase64);

                    if (!loadOnlyImage)
                    {
                        var tempList = PlanElements.Select(s => new ImageHelper.ImageExtraData()
                        {
                            Label = s.Label,
                            PositionX = s.PositionX,
                            PositionY = s.PositionY,
                        }).ToList();

                        var bitMap = ImageHelper.BitmapFromBase64(_PlanImage.ImageAsBase64);
                        ImageHelper.AddListOfDataOnImage(bitMap, tempList);
                        PlanBitMap = ImageHelper.BitmapSourceBitMap(bitMap);
                    }
                }
                catch (Exception exp)
                {
                    // Handle the exception or log it as needed
                    throw;
                }
            });
        }


        private async Task GetListOfDevicesAsync()
        {
            try
            {
                loraWanDataRowsModels = await _loraWanDataRowsService.GetLoraWanDataRowsLatestByLocationId(SelectedLocation.LocationId);

                if (loraWanDataRowsModels != null)
                {
                    List<PhysicalDeviceModel> tempPhysicalDeviceList = new List<PhysicalDeviceModel>();

                    var group = loraWanDataRowsModels.GroupBy(g => g.PhysicalDeviceId).ToList();
                    foreach (var item in group)
                    {
                        tempPhysicalDeviceList.Add(new PhysicalDeviceModel()
                        {
                            PhysicalDeviceId = item.Key,
                            PhysicalDeviceName = item.ToList().First().PhysicalDeviceName
                        });
                    }

                    PhysicalDevices = new ObservableCollection<PhysicalDeviceModel>(tempPhysicalDeviceList);

                    SelectedPhysicalDevice = PhysicalDevices.FirstOrDefault();
                }
            }
            catch (Exception exp)
            {
                throw;
            }
        }

        private void ManageLoadLoraWanDataAsync()
        {
            if (_SelectedPhysicalDevice != null)
            {
                var temp = loraWanDataRowsModels.Where(w => w.PhysicalDeviceId == _SelectedPhysicalDevice.PhysicalDeviceId).ToList();
                LoraWanList = new ObservableCollection<LoraWanDataRowsModel>(temp);
                SelectedLoraWan = temp.FirstOrDefault();
            }
        }

        private void AddDataOnImage(Bitmap bitmap, string text, float x, float y)
        {
            var bitMap = BitmapFromSource(PlanBitMap);
            PointF location = new PointF((float)x, (float)y);
            using (Graphics graphics = Graphics.FromImage(bitMap))
            {
                using (Font arialFont = new Font("Arial", 10))
                {
                    graphics.DrawString(text, arialFont, Brushes.White, location);
                }
            }
        }

        private void AddListOfDataOnImage(Bitmap bitMap, List<PlanElementModel> planElementModels)
        {
            using (Graphics graphics = Graphics.FromImage(bitMap))
            {
                using (Font arialFont = new Font("Arial", 10))
                {
                    foreach (var item in planElementModels)
                    {
                        PointF location = new PointF(item.PositionX, item.PositionY);
                        graphics.DrawString(item.Label, arialFont, Brushes.White, location);
                    }
                }
            }
        }

        private Bitmap BitmapFromSource(BitmapSource bitmapsource)
        {
            Bitmap bitmap;
            using (var outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapsource));
                enc.Save(outStream);
                bitmap = new Bitmap(outStream);
            }
            return bitmap;
        }

        #endregion
    }
}
