using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace AuthService.Models
{
    public class Note
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("title")]
        public string Title { get; set; }= string.Empty;

        [BsonElement("text")]
        public string Text { get; set; }= string.Empty;

        [BsonElement("category")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string CategoryId { get; set; }= string.Empty;

        [BsonElement("user")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }= string.Empty;

         [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; }

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        [BsonElement("__v")]
        public Int32 __v { get; set; }
        }
}
