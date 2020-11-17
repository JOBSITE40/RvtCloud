
namespace ForgeAppTesting.Models
{
    public class Bim5dDatabaseSettings : IBim5dDatabaseSettings
    {
        public string PartidaCertifiacionesCollectionName { get; set; }
        public string GoodPracticesCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IBim5dDatabaseSettings
    {
        string PartidaCertifiacionesCollectionName { get; set; }
        string GoodPracticesCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
