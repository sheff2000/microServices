using AuthService.Models;
using MongoDB.Driver;

namespace AuthService.Data
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IMongoClient mongoClient)
        {
            _database = mongoClient.GetDatabase("test");
        }

        // Пример метода для получения коллекции из базы данных
        public IMongoCollection<TDocument> GetCollection<TDocument>(string name)
        {
            return _database.GetCollection<TDocument>(name);
        }


        public IMongoCollection<User> Users => _database.GetCollection<User>("user_servers");
        public IMongoCollection<Category> Categories => _database.GetCollection<Category>("category_servers");
        public IMongoCollection<Note> Notes => _database.GetCollection<Note>("note_servers");
    }
}
