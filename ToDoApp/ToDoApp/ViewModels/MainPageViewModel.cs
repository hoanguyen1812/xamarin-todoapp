using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ToDoApp.Infrastructure;
using ToDoApp.Models;
using ToDoApp.Views;
using Xamarin.Forms;
using Newtonsoft.Json;
using ToDoApp.Services;

namespace ToDoApp.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        private ItemViewModel _selectedItem;
        private IItemStore _itemStore;
        private IPageService _pageService;
        private bool _isDataLoaded;
        private  HttpClient _client = new HttpClient();

        public ObservableCollection<ItemViewModel> Items { get; private set; } = new ObservableCollection<ItemViewModel>();

        public ItemViewModel SelectedItem
        {
            get => _selectedItem;
            set => SetValue(ref _selectedItem, value);
        }

        private bool _isRefreshing = false;
        public bool IsRefreshing { 
            get { return _isRefreshing; }
            set {
                SetValue(ref _isRefreshing, value);
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }

        public ICommand LoadDataCommand { get; private set; }
        public ICommand AddItemCommand { get; private set; }
        public ICommand SelectItemCommand { get; private set; }
        public ICommand DeleteItemCommand { get; private set; }

        public ICommand RefreshCommand
        {
            get {
                return new Command(async () =>
                {
                    IsRefreshing = true;

                    _isDataLoaded = false;
                    await LoadData();

                    IsRefreshing = false;
                });
            }
        }

        public MainPageViewModel(IItemStore itemStore, IPageService pageService)
        {
            _itemStore = itemStore;
            _pageService = pageService;

            LoadDataCommand = new Command(async () => await LoadData());
            SelectItemCommand = new Command<ItemViewModel>(async c => await SelectItem(c));
            AddItemCommand = new Command(async () => await AddItem());
            DeleteItemCommand = new Command<ItemViewModel>(async c => await DeleteItem(c));
        }

        private async Task LoadData()
        {
            if(_isDataLoaded)
                return;
            _isDataLoaded = true;

            //var items = await _itemStore.GetItemsAsync();
            Items.Clear();
            var items = await new ItemService().GetItemsAsync();
            foreach (var item in items)
            {
                Items.Add(new ItemViewModel(item));
            }
        }

        private async Task SelectItem(ItemViewModel item)
        {
            if (item == null) return;

            SelectedItem = null;

            var viewModel = new ItemDetailsViewModel(item, _itemStore, _pageService);
            viewModel.ItemUpdated += (source, updatedItem) =>
            {
                item.Id = updatedItem.Id;
                item.Title = updatedItem.Title;
                item.Notes = updatedItem.Notes;
            };

            await _pageService.PushAsync(new ItemDetails(viewModel));
        }

        private async Task AddItem()
        {
            var viewModel = new ItemDetailsViewModel(new ItemViewModel(), _itemStore, _pageService);
            viewModel.ItemAdded += (source, item) => { Items.Add(new ItemViewModel(item)); };

            await _pageService.PushAsync(new ItemDetails(viewModel));
        }

        private async Task DeleteItem(ItemViewModel itemViewModel)
        {
            if (await _pageService.DisplayAlert("Warning", $"Are you sure you want to delete {itemViewModel.Title}?", "Yes", "No"))
            {
                Items.Remove(itemViewModel);
                //var item = await _itemStore.GetItem(itemViewModel.Id);
                //await _itemStore.DeleteItem(item);
                await _client.DeleteAsync($"{Constants.URL}/{itemViewModel.Id}");
            }

        }
    }
}
