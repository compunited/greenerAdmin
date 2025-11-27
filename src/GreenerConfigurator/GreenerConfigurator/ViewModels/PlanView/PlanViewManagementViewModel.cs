using Greener.Web.Definitions.API.Plans;
using Greener.Web.Definitions.API.Plans.Configurator.Overview;
using Greener.Web.Definitions.API.Plans.Configurator.ViewDtos;
using Greener.Web.Definitions.API.Plans.Frontend.RequestParameter;
using GreenerConfigurator.ClientCore.Models;
using GreenerConfigurator.ClientCore.Services;
using Microsoft.Extensions.DependencyInjection;
using GreenerConfigurator.ClientCore.Services.Location;
using GreenerConfigurator.ClientCore.Utilities;
using GreenerConfigurator.ClientCore.Utilities.Enumerations;
using GreenerConfigurator.Utilities;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using PlanElement = Greener.Web.Definitions.API.Plans.PlanElement;

namespace GreenerConfigurator.ViewModels.PlanView
{
    public class PlanViewManagementViewModel : ViewModelBase
    {
        #region [ Constructor(s) ]
        private readonly LocationService _locationService;
        private readonly PlanViewService _planViewService;

        public PlanViewManagementViewModel(LocationModel selectedLocation = null)
        {
            _locationService = App.ServiceProvider.GetRequiredService<LocationService>();
            _planViewService = App.ServiceProvider.GetRequiredService<PlanViewService>();

            OnAddPlanViewCommand = new AsyncRelayCommand(AddPlanViewCommandAsync);
            OnEditPlanViewCommand = new AsyncRelayCommand(EditPlanViewCommandAsync);
            OnDeletePlanViewCommand = new AsyncRelayCommand(DeletePlanViewCommandAsync);

            GetAllLocations(selectedLocation);

        }

        #endregion

        #region [ Public Property(s) ]

        public ICommand OnAddPlanViewCommand { get; private set; }

        public ICommand OnEditPlanViewCommand { get; private set; }

        public ICommand OnDeletePlanViewCommand { get; private set; }

        public ObservableCollection<LocationModel> LocationList
        {
            get => _LocationList;
            set
            {
                _LocationList = value;
                OnPropertyChanged(nameof(LocationList));
            }
        }

        public ObservableCollection<PlanItem> PlanItemList
        {
            get => _PlanItemList;
            set
            {
                _PlanItemList = value;
                OnPropertyChanged(nameof(PlanItemList));
            }
        }

        public LocationModel SelectedLocation
        {
            get => _SelectedLocation;
            set
            {
                _SelectedLocation = value;
                OnPropertyChanged(nameof(SelectedLocation));
                ManageLoadPlanViewAsync();
            }
        }

        public PlanItem SelectedPlanItem
        {
            get => _SelectedPlanItem;
            set
            {
                _SelectedPlanItem = value;
                OnPropertyChanged(nameof(SelectedPlanItem));
            }
        }

        public bool IsPlanViewEditEnable
        {
            get => _IsPlanViewEditEnable;
            set
            {
                _IsPlanViewEditEnable = value;
                OnPropertyChanged(nameof(IsPlanViewEditEnable));
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
                if(_ErrorMessageVisibility== Visibility.Collapsed)
                {
                    ErrorMessage = string.Empty;
                }

                OnPropertyChanged(nameof(ErrorMessageVisibility));
            }
        }

        #endregion

        #region [ Private Field(s) ]

        private ObservableCollection<LocationModel> _LocationList = null;
        private ObservableCollection<PlanItem> _PlanItemList = null;
        private LocationModel _SelectedLocation = null;
        private PlanItemResultDto planView = null;
        private PlanImageDto planImage = null;
        private bool _IsPlanViewEditEnable = false;
        private PlanItem _SelectedPlanItem = null;
        private string _ErrorMessage = string.Empty;
        private Visibility _ErrorMessageVisibility = Visibility.Collapsed;

        #endregion

        #region [ Private Method(s) ]

        private async Task GetAllLocations(LocationModel selectedLocation = null)
        {
            var tempLocations = await _locationService.GetAllLocation();

            if (tempLocations == null)
                tempLocations = new List<LocationModel>();

            LocationList = new ObservableCollection<LocationModel>(tempLocations);
            if (selectedLocation != null)
            {
                SelectedLocation = tempLocations.Where(w => w.LocationId == selectedLocation.LocationId).FirstOrDefault();
            }
            else
            {
                SelectedLocation = tempLocations.FirstOrDefault();
            }
        }

        private async Task ManageLoadPlanViewAsync()
        {
            IsPlanViewEditEnable = false;

            GetPlanViewsByLocationIdParameter requestParameter = new GetPlanViewsByLocationIdParameter();
            requestParameter.LocationId = SelectedLocation.LocationId;
            var tempPlanView = await _planViewService.GetPlanViewByLocationIdAsync(requestParameter);
            if (tempPlanView != null && tempPlanView.Count != 0)
            {
                IsPlanViewEditEnable = true;
                PlanItemList = new ObservableCollection<PlanItem>(tempPlanView);
                SelectedPlanItem = PlanItemList.FirstOrDefault();
            }
            else
            {
                PlanItemList = null;
                SelectedPlanItem = null;
            }
        }
        private async Task AddPlanViewCommandAsync()
        {
            ErrorMessageVisibility = Visibility.Collapsed;

            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "All Images|*.jpg;*.png;*.jpeg";

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                string filename = dialog.FileName;
                var mimeType = ImageHelper.GetImageMimeType(dialog.FileName);

                // Überprüfen, ob das Bild ein unterstütztes Format hat
                if (!string.IsNullOrEmpty(mimeType) && (mimeType == "image/jpeg" || mimeType == "image/png"))
                {
                    try
                    {
                        planView = new PlanItemResultDto();
                        planView.PlanImageDto = new PlanImageDto();
                        planView.PlanViewDto = new PlanViewDto();
                        planView.PlanViewDto.ElementDtos = new List<PlanElement>();
                        planView.PlanViewDto.LocationId = SelectedLocation.LocationId;

                        planImage = new PlanImageDto();
                        byte[] imageArray = File.ReadAllBytes(filename);
                        planView.PlanImageDto.ImageAsBase64 = Convert.ToBase64String(imageArray);
                        planView.PlanImageDto.ImageMimeType = mimeType;

                        // Alternative Methode zur BitmapImage-Erstellung
                        var image = new BitmapImage();
                        using (var ms = new MemoryStream(imageArray))
                        {
                            ms.Position = 0;
                            image.BeginInit();
                            image.CacheOption = BitmapCacheOption.OnLoad;
                            image.StreamSource = ms;
                            image.EndInit();
                            image.Freeze(); // Damit es außerhalb des UI-Threads sicher verwendet werden kann
                        }

                        planView.PlanImageDto.ImageSizeX = (int)image.Width;
                        planView.PlanImageDto.ImageSizeY = (int)image.Height;

                        PlanViewViewModel viewModel = new PlanViewViewModel(SelectedLocation, planView, PageStatus.Add);

                        var temp = Application.Current;
                        ((MainWindow)temp.MainWindow).MainWindowModel.Navigator.CurrentViewModel = viewModel;
                    }
                    catch (Exception ex)
                    {
                        // Fehlerbehandlung, wenn das Bild nicht dekodiert werden kann
                        ErrorMessage = $"The image could not be decoded: {ex.Message}";
                        ErrorMessageVisibility = Visibility.Visible;
                    }
                }
                else
                {
                    // Zeige Fehlermeldung, wenn das Bildformat nicht unterstützt wird
                    ErrorMessage = "The selected image type is not supported!";
                    ErrorMessageVisibility = Visibility.Visible;
                }
            }
        }

        private async Task EditPlanViewCommandAsync()
        {
            if (SelectedPlanItem != null)
            {
                GetPlanViewByPlanViewRefIdParameter requestParameter = new GetPlanViewByPlanViewRefIdParameter();
                requestParameter.PlanViewRefId = SelectedPlanItem.PlanViewRefId;
                var tempResult = await _planViewService.GetPlanViewByRefId(requestParameter);
                if (tempResult != null)
                {
                    planView = new PlanItemResultDto();
                    planView.PlanImageDto = tempResult.PlanImageDto;
                    planView.PlanViewDto = new PlanViewDto();
                    planView.PlanViewDto.LocationId = SelectedLocation.LocationId;

                    planView.PlanViewDto = tempResult.PlanViewDto;

                    PlanViewViewModel viewModel = new PlanViewViewModel(SelectedLocation, planView, PageStatus.Edit);

                    var temp = Application.Current;
                    ((MainWindow)temp.MainWindow).MainWindowModel.Navigator.CurrentViewModel = viewModel;
                }
            }
        }

        private async Task DeletePlanViewCommandAsync()
        {
        }

        #endregion
    }
}
