using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;
using ToDoApp.Infrastructure;
using ToDoApp.Models;
using Xamarin.Forms;

namespace ToDoApp.ViewModels
{
    public class ItemDetailsViewModel
    {
        private IItemStore _itemStore;
        private IPageService _pageService;
        private HttpClient _client = new HttpClient();

        public EventHandler<Item> ItemAdded;
        public EventHandler<Item> ItemUpdated;

        public Item Item { get; private set; }
        public ICommand SaveCommand { get; private set; }

        public ItemDetailsViewModel(ItemViewModel viewModel, IItemStore itemStore, IPageService pageService)
        {
            if (viewModel == null) 
                throw new ArgumentNullException(nameof(viewModel));

            _pageService = pageService;
            _itemStore = itemStore;

            SaveCommand = new Command(async ()=> await Save());

            Item = new Item
            {
                Id = viewModel.Id,
                Title = viewModel.Title,
                Notes = viewModel.Notes,
            };
        }

        private async Task Save()
        {
            if(string.IsNullOrWhiteSpace(Item.Title))
            {
                await _pageService.DisplayAlert("Error", "Please enter the title", "OK");
                return;
            }

            if (Item.Id == 0)
            {
                //await _itemStore.AddItem(Item);
                var content = JsonConvert.SerializeObject(Item);
                await _client.PostAsync(Constants.URL, new StringContent(content));
                ItemAdded?.Invoke(this, Item);
            }
            else
            {
                //await _itemStore.UpdateItem(Item);
                var content = JsonConvert.SerializeObject(Item);
                await _client.PutAsync($"{Constants.URL}/{Item.Id}", new StringContent(content));
                ItemUpdated?.Invoke(this, Item);
            }

            await _pageService.PopAsync();
        }
    }
}
