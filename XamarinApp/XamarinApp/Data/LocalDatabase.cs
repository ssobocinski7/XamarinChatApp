using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using XamarinApp.Models;
using XamarinApp.Models.SQL;


namespace XamarinApp.Data
{
    public class LocalDatabase
    {
        readonly SQLiteAsyncConnection database;
        
        public LocalDatabase(string path)
        {
            database = new SQLiteAsyncConnection(path);
            database.CreateTableAsync<Friend>().Wait();
            database.CreateTableAsync<ChatMessage>().Wait();
            database.CreateTableAsync<FriendRequest>().Wait();
        }
        public async Task ResetRequests()
        {
            await database.DropTableAsync<FriendRequest>();
            await database.CreateTableAsync<FriendRequest>();
        }
        public async Task ResetFriends()
        {
            await database.DropTableAsync<Friend>();
            await database.CreateTableAsync<Friend>();
        }
        public async Task RemoveRequest(string SenderID, string ReceiverID)
        {
            var result = await database.Table<FriendRequest>().Where(c => c.SenderID == SenderID && c.ReceiverID == ReceiverID).FirstOrDefaultAsync();
            await database.DeleteAsync(result);
        }
        public async Task<List<FriendRequest>> GetAllFriendRequests()
        {
            return await database.Table<FriendRequest>().ToListAsync();
        }
        public async Task<List<Friend>> GetAllFriends()
        {
            return await database.Table<Friend>().ToListAsync();
        }
        public async Task<string> GetFriendUsername(string friendID)
        {
            var result = await database.Table<Friend>().Where(c => c.UserID == friendID).FirstOrDefaultAsync();
            return result.UserName;
        }
        public async Task<List<ChatMessage>> GetLastMessagesWithFriend(string friendID)
        {
            int count = 100;
            List<ChatMessage> temp = await database
                        .Table<ChatMessage>()
                        .Where(c => c.ReceiverID == friendID || c.SenderID == friendID)
                        .OrderByDescending(c => c.ID)
                        .Take(count)
                        .ToListAsync();

            temp.Reverse();
            return temp;

       
                        
        }
        public async Task SaveItemAsync<T>(T item)
        {
            await database.InsertAsync(item);
        }
    
    }

}
