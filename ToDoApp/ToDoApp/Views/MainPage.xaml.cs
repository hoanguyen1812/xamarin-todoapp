using System.ComponentModel;
using ToDoApp.Persistence;
using ToDoApp.ViewModels;
using Xamarin.Forms;

namespace ToDoApp.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            var itemStore = new SQLiteItemStore(DependencyService.Get<ISQLiteDb>());
            var pageService = new PageService();
            ViewModel = new MainPageViewModel(itemStore, pageService);

            InitializeComponent();
        }

        public MainPageViewModel ViewModel
        {
            get => BindingContext as MainPageViewModel;
            set => BindingContext = value;
        }

        protected override void OnAppearing()
        {
            ViewModel.LoadDataCommand.Execute(null);
            base.OnAppearing();
        }

        private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ViewModel.SelectItemCommand.Execute(e.SelectedItem);
        }
    }
}