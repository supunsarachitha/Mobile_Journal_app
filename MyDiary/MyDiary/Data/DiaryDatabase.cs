using MyDiary.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDiary.Data
{
    public class DiaryDatabase
    {

        static readonly Lazy<SQLiteAsyncConnection> lazyInitializer = new Lazy<SQLiteAsyncConnection>(() =>
        {
            return new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        });

        static SQLiteAsyncConnection Database => lazyInitializer.Value;
        static bool initialized = false;

        public DiaryDatabase()
        {
            InitializeAsync().SafeFireAndForget(false);
        }

        async Task InitializeAsync()
        {
            if (!initialized)
            {
                if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(PageItems).Name))
                {
                    await Database.CreateTablesAsync(CreateFlags.None, typeof(PageItems)).ConfigureAwait(false);
                }
                initialized = true;
            }
        }

        public Task<List<PageItems>> GetItemsAsync()
        {
            return Database.Table<PageItems>().OrderByDescending(x => x.DateTime).ToListAsync();
        }


        public Task<List<PageItems>> GetItemsNotDoneAsync()
        {
            return Database.QueryAsync<PageItems>("SELECT * FROM [TodoItem] WHERE [Done] = 0");
        }

        public Task<PageItems> GetItemAsync(int id)
        {
            return Database.Table<PageItems>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveItemAsync(PageItems item)
        {
            if (item.ID != 0)
            {
                return Database.UpdateAsync(item);
            }
            else
            {
                return Database.InsertAsync(item);
            }
        }

        public Task<int> DeleteItemAsync(PageItems item)
        {
            return Database.DeleteAsync(item);
        }

        public Task DeleteItemByIDAsync(int ID)
        {
            return Database.QueryAsync<PageItems>(("DELETE FROM [PageItems] WHERE [ID] =" + ID.ToString()));

        }
    }
}
