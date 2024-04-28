using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace CategoryService.Models
{
    public class Category
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("nameCategory")]
        public string? NameCategory { get; set; }

         [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; }

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        [BsonElement("__v")]
        public Int32 __v { get; set; }
    }
}
