using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CarbonConfigServer.Models
{
    public class AppConfig
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("AppName")]
        public string AppName { get; set; }
        
        [BsonElement("AuthToken")] //Probably be a 256 character unique string
        public string AuthToken { get; set; }

        public Dictionary<string, object> Config { get; set; }
        
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime LastSeen { get; set; }
    }
}