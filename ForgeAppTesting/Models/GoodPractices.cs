using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;

namespace ForgeAppTesting.Models
{
    public class GoodPractices
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public List<KeyValuePair<string, int>> BuenasPracticas { get; set; }
        public GoodPractices(List<KeyValuePair<string, int>> buenasPracticas)
        {
            this.BuenasPracticas = buenasPracticas;
        }
    }
}
