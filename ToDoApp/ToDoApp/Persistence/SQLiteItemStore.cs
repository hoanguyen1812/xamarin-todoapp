using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using ToDoApp.Models;
using ToDoApp.ViewModels;

namespace ToDoApp.Persistence
{
    public class SQLiteItemStore : IItemStore
    {
        private SQLiteAsyncConnection _connection;

        public SQLiteItemStore(ISQLiteDb db)
        {
            _connection= db.GetConnection();
            _connection.CreateTableAsync<Item>();
        }

        public async Task<IEnumerable<Item>> GetItemsAsync()
        {
            return await _connection.Table<Item>().ToListAsync();
        }

        public async Task DeleteItem(Item item)
        {
            await _connection.DeleteAsync(item);
        }

        public async Task UpdateItem(Item item)
        {
            await _connection.UpdateAsync(item);
        }

        public async Task AddItem(Item item)
        {
            await _connection.InsertAsync(item);
        }

        public async Task<Item> GetItem(int id)
        {
            return await _connection.FindAsync<Item>(id);
        }
    }
}
