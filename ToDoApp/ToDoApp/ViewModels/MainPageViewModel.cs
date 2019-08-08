using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ToDoApp.Views;
using Xamarin.Forms;

namespace ToDoApp.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        private ItemViewModel _selectedItem;
        private IItemStore _itemStore;
        private IPageService _pageService;
        private bool _isDataLoaded;

        public ObservableCollection<ItemViewModel> Items { get; private set; } = new ObservableCollection<ItemViewModel>();

        public ItemViewModel SelectedItem
        {
            get => _selectedItem;
            set => SetValue(ref _selectedItem, value);
        }

        public ICommand LoadDataCommand { get; private set; }
        public ICommand AddItemCommand { get; private set; }
        public ICommand SelectItemCommand { get; private set; }
        public ICommand DeleteItemDataCommand { get; private set; }

        public MainPageViewModel(IItemStore itemStore, IPageService pageService)
        {
            _itemStore = itemStore;
            _pageService = pageService;

            LoadDataCommand = new Command(async () => await LoadData());
            SelectItemCommand = new Command<ItemViewModel>(async c => await SelectItem(c));
            AddItemCommand = new Command(async () => await AddItem());
        }

        private async Task LoadData()
        {
            if(_isDataLoaded)
                return;
            _isDataLoaded = true;

            var items = await _itemStore.GetItemsAsync();
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
    }
}
