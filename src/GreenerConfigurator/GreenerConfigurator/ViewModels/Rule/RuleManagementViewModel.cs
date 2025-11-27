using Greener.Web.Definitions.API.Rule;
using GreenerConfigurator.Models;
using GreenerConfigurator.ClientCore.Models;
using GreenerConfigurator.ClientCore.Models.Rule;
using GreenerConfigurator.ClientCore.Services.Rule;
using GreenerConfigurator.ClientCore.Services;
using Microsoft.Extensions.DependencyInjection;
using GreenerConfigurator.ClientCore.Utilities.Enumerations;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GreenerConfigurator.ViewModels.Rule
{
    public class RuleManagementViewModel : ViewModelBase
    {

        #region [ Constructor(s) ]

        private readonly RuleService _ruleService;

        public RuleManagementViewModel()
        {
            _ruleService = App.ServiceProvider.GetRequiredService<RuleService>();
            OnAddRuleCommand = new AsyncRelayCommand(AddRuleCommandAsync);
            OnEditRuleCommand = new AsyncRelayCommand(EditRuleCommandAsync);
            OnDeleteRuleCommand = new AsyncRelayCommand(DeleteRuleCommandAsync);

            LoadRuleDataAsync();
        }

        #endregion

        #region [ Public Property(s) ]

        public ICommand OnAddRuleCommand { get; private set; }

        public ICommand OnEditRuleCommand { get; private set; }

        public ICommand OnDeleteRuleCommand { get; private set; }

        public ObservableCollection<RuleViewDto> RuleList
        {
            get => _RuleList;
            set
            {
                _RuleList = value;
                OnPropertyChanged(nameof(RuleList));
                if (_RuleList != null)
                    SelectedRule = _RuleList.FirstOrDefault();
            }
        }

        public RuleViewDto SelectedRule
        {
            get => _SelectedRule;
            set
            {
                _SelectedRule = value;
                OnPropertyChanged(nameof(SelectedRule));
            }
        }

        #endregion

        #region [ Private Field(s) ]

        private ObservableCollection<RuleViewDto> _RuleList = null;
        private RuleViewDto _SelectedRule = null;


        #endregion

        #region [ Private Method(s) ]

        private async Task LoadRuleDataAsync()
        {
            var temp = await _ruleService.GetAllRulesAsync();

            if (temp == null)
                temp = new List<RuleViewDto>();

            RuleList = new ObservableCollection<RuleViewDto>(temp);
        }

        private async Task AddRuleCommandAsync()
        {
            RuleEditModel tempModel = new RuleEditModel();
            tempModel.Id = Guid.NewGuid();

            var tempView = new RuleCreateUpdateViewModel(tempModel, PageStatus.Add);
            App.CurrentView(tempView);

            //var temp = Application.Current;
            //((MainWindow)temp.MainWindow).MainWindowModel.Navigator.CurrentViewModel 
        }

        private async Task EditRuleCommandAsync()
        {
            if (_SelectedRule != null)
            {
                RuleEditModel tempModel = new RuleEditModel();
                tempModel.Id = _SelectedRule.Id;

                var tempView = new RuleCreateUpdateViewModel(tempModel, PageStatus.Edit);
                App.CurrentView(tempView);

                //var temp = Application.Current;
                //((MainWindow)temp.MainWindow).MainWindowModel.Navigator.CurrentViewModel = new RuleCreateUpdateViewModel(tempModel, PageStatus.Edit);
            }
        }

        private async Task DeleteRuleCommandAsync()
        {
            if (_SelectedRule != null)
            {
                RuleEditModel tempModel = new RuleEditModel();
                tempModel.Id = _SelectedRule.Id;

                var tempView = new RuleCreateUpdateViewModel(tempModel, PageStatus.Delete);
                App.CurrentView(tempView);

                //var temp = Application.Current;
                //((MainWindow)temp.MainWindow).MainWindowModel.Navigator.CurrentViewModel = new RuleCreateUpdateViewModel(tempModel, PageStatus.Edit);
            }
        }

        #endregion

    }
}
