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
        public string Hub { get; set; }
        public string Project { get; set; }
        public int Version { get; set; }
        public ImportGoodPractices BuenasPracticas { get; set; }
        public GoodPractices(ImportGoodPractices buenasPracticas)
        {
            this.Hub = "test";
            this.Project = "test";
            this.Version = -1;
            this.BuenasPracticas = buenasPracticas;
        }
    }
}
