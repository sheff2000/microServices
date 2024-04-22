using AuthService.Models;
using MongoDB.Driver;

namespace AuthService.Data
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration["MongoDB:ConnectionString"]);
            _database = client.GetDatabase(configuration["MongoDB:DatabaseName"]);
        }

        public IMongoCollection<User> Users => _database.GetCollection<User>("users");
        public IMongoCollection<Category> Categories => _database.GetCollection<Category>("categories");
        public IMongoCollection<Note> Notes => _database.GetCollection<Note>("notes");
    }
}
