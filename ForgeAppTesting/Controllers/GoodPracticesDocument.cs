using ForgeAppTesting.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForgeAppTesting.Controllers
{
    public class GoodPracticesDocument
    {
        public string HubId { get; set; }
        public string ProjectId { get; set; }
        public int Version { get; set; }
        public ImportGoodPractices GoodPractices { get; set; }
    }
}
