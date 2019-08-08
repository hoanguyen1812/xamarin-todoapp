using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ToDoApp.Models;

namespace ToDoApp.ViewModels
{
    public interface IItemStore
    {
        Task<IEnumerable<Item>> GetItemsAsync();
        Task<Item> GetItem(int id);
        Task AddItem(Item item);
        Task UpdateItem(Item item);
        Task DeleteItem(Item item);
    }
}
