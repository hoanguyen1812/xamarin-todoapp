using System.IO;
using SQLite;
using ToDoApp.Droid.Persistence;
using ToDoApp.Persistence;
using Xamarin.Forms;
using Environment = System.Environment;

[assembly:Dependency(typeof(SQLiteDb))]

namespace ToDoApp.Droid.Persistence
{
    public class SQLiteDb: ISQLiteDb
    {
        public SQLiteAsyncConnection GetConnection()
        {
            var documentPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var path = Path.Combine(documentPath, "MySQLite.db3");
            return new SQLiteAsyncConnection(path);
        }
    }
}