using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdateRVTParam.Models
{
    public class GoodPracticeDocument
    {
        public string HubId { get; set; }
        public string ProjectId { get; set; }
        public string Filename { get; set; }
        public int Version { get; set; }
        public GoodPractices GoodPractices { get; set; }
        public GoodPracticeDocument(string hubId, string projectId, string filename, int version, Document doc)
        {
            this.HubId = hubId;
            this.ProjectId = projectId;
            this.Filename = filename;
            this.Version = version;
            this.GoodPractices = new GoodPractices(doc);
        }
    }
}
