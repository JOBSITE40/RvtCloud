using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForgeAppTesting.Models
{
    public class ImportGoodPractices
    {
        public int nAvisos { get; set; }
        public int nVistas { get; set; }
        public int dwgT { get; set; }
        public int familiesC { get; set; }
        public int groupsM { get; set; }
        public int genericModel { get; set; }
        public int floorsT { get; set; }
        public int wallsT { get; set; }
        public int optionsT { get; set; }
        public int worksetsT { get; set; }
        public int levelsT { get; set; }
        public int gridsT { get; set; }
        public int roomsT { get; set; }
        public int spacesT { get; set; }
        public int sheetsT { get; set; }
        public int templatesViewT { get; set; }
        public int schedulesT { get; set; }

        public ImportGoodPractices(
            int nAvisos,
            int nVistas,
            int dwgT,
            int familiesC,
            int groupsM,
            int genericModel,
            int floorsT,
            int wallsT,
            int optionsT,
            int worksetsT,
            int levelsT,
            int gridsT,
            int roomsT,
            int spacesT,
            int sheetsT,
            int templatesViewT,
            int schedulesT)
        {
            this.nAvisos = nAvisos;
            this.nVistas = nVistas;
            this.dwgT = dwgT;
            this.familiesC = familiesC;
            this.groupsM = groupsM;
            this.genericModel = genericModel;
            this.floorsT = floorsT;
            this.wallsT = wallsT;
            this.optionsT = optionsT;
            this.worksetsT = worksetsT;
            this.levelsT = levelsT;
            this.gridsT = gridsT;
            this.roomsT = roomsT;
            this.spacesT = spacesT;
            this.sheetsT = sheetsT;
            this.templatesViewT = templatesViewT;
            this.schedulesT = schedulesT;
        }
    }
}
