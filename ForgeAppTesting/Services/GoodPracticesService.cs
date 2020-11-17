using ForgeAppTesting.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace ForgeAppTesting.Services
{
    public class GoodPracticesService
    {
        private readonly IMongoCollection<GoodPractices> _gps;

        public GoodPracticesService(IBim5dDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _gps = database.GetCollection<GoodPractices>(settings.GoodPracticesCollectionName);
        }

        public List<GoodPractices> Get() =>
            _gps.Find(gp => true).ToList();

        public GoodPractices Get(string id) =>
            _gps.Find<GoodPractices>(gp => gp.Id == id).FirstOrDefault();

        public GoodPractices Create(GoodPractices gp)
        {
            _gps.InsertOne(gp);
            return gp;
        }

        public void Update(string id, GoodPractices gpIn) =>
            _gps.ReplaceOne(gp => gp.Id == id, gpIn);

        public void Remove(GoodPractices gpIn) =>
            _gps.DeleteOne(gp => gp.Id == gpIn.Id);

        public void Remove(string id) =>
            _gps.DeleteOne(gp => gp.Id == id);
    }
}
