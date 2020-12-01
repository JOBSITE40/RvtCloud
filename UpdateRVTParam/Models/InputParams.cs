using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdateRVTParam.Models
{
    public class InputParams
    {
        public string HubId { get; set; }
        public string ProjectId { get; set; }
        public string Filename { get; set; }
        public int Version { get; set; }
    }
}
