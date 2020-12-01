using ForgeAppTesting.Controllers;
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
        public GoodPractices(GoodPracticesDocument buenasPracticas)
        {
            this.Hub = buenasPracticas.HubId;
            this.Project = buenasPracticas.ProjectId;
            this.Version = buenasPracticas.Version;
            this.BuenasPracticas = buenasPracticas.GoodPractices;
        }
    }
}
