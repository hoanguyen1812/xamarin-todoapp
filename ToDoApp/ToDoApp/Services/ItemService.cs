using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ToDoApp.Infrastructure;
using ToDoApp.Models;

namespace ToDoApp.Services
{
    public class ItemService
    {
        private HttpClient _client = new HttpClient();

        public async Task<List<Item>> GetItemsAsync()
        {
            var response = await _client.GetAsync($"{Constants.URL}");

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Item>>(content);
        }
    }
}
