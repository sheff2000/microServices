using CategoryService.Models;
using MongoDB.Driver;

namespace CategoryService.Data
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IMongoClient mongoClient)
        {
            _database = mongoClient.GetDatabase("test");
        }
        public IMongoCollection<TDocument> GetCollection<TDocument>(string name)
        {
            return _database.GetCollection<TDocument>(name);
        }
        public IMongoCollection<Category> Categories => _database.GetCollection<Category>("category_servers");
    }
}
