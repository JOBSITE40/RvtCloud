using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;

namespace ForgeAppTesting.Models
{
    public class PartidaCertificacion
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string HijoId { get; set; }

        public List<string> Certificaciones { get; set; }
        public PartidaCertificacion(string hijoId, List<string> certificaciones)
        {
            this.HijoId = hijoId;
            this.Certificaciones = certificaciones;
        }
    }
}
