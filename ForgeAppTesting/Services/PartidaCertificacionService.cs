using ForgeAppTesting.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace ForgeAppTesting.Services
{
    public class PartidaCertificacionService
    {
        private readonly IMongoCollection<PartidaCertificacion> _pcs;

        public PartidaCertificacionService(IBim5dDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _pcs = database.GetCollection<PartidaCertificacion>(settings.PartidaCertifiacionesCollectionName);
        }

        public List<PartidaCertificacion> Get() =>
            _pcs.Find(pc => true).ToList();

        public PartidaCertificacion Get(string id) =>
            _pcs.Find<PartidaCertificacion>(pc => pc.Id == id).FirstOrDefault();

        public PartidaCertificacion GetByHijo(string id) =>
            _pcs.Find<PartidaCertificacion>(pc => pc.HijoId == id).FirstOrDefault();

        public PartidaCertificacion Create(PartidaCertificacion pc)
        {
            _pcs.InsertOne(pc);
            return pc;
        }

        public void Update(string id, PartidaCertificacion pcIn) =>
            _pcs.ReplaceOne(pc => pc.Id == id, pcIn);
        public List<string> AddCertificaciones(string hijo, List<string> certificaciones)
        {
            var lista = new List<string>();
            var pcIn = GetByHijo(hijo);
            foreach(var certificacion in certificaciones)
            {
                var _certificacion = pcIn.Certificaciones.FirstOrDefault(x => x == certificacion);
                if(_certificacion == null)
                {
                    pcIn.Certificaciones.Add(certificacion);
                    lista.Add(certificacion);
                }
            }
            _pcs.ReplaceOne(pc => pc.HijoId == hijo, pcIn);
            return lista;
        }

        public void Remove(PartidaCertificacion pcIn) =>
            _pcs.DeleteOne(pc => pc.Id == pcIn.Id);

        public void Remove(string id) =>
            _pcs.DeleteOne(pc => pc.Id == id);
    }
}
