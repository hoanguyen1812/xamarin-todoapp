using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace ToDoApp.Persistence
{
    public interface ISQLiteDb
    {
        SQLiteAsyncConnection GetConnection();
    }
}
