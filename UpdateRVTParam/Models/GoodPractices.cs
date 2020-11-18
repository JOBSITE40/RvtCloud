using Autodesk.Revit.DB;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace UpdateRVTParam.Models
{
    public class GoodPractices
    {
        private Document _doc;
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

        public GoodPractices(Document doc)
        {
            this._doc = doc;
            this.nAvisos = doc.GetWarnings().Count;
            this.nVistas = CountInstancesFromDocument(BuiltInCategory.OST_Views);
            this.dwgT = CountTypesFromDocument(typeof(CADLinkType));
            this.familiesC = CountInstancesFromDocument(typeof(Family));
            this.groupsM = CountInstancesFromDocument(typeof(Group));
            this.genericModel = CountInstancesFromDocument(BuiltInCategory.OST_GenericModel);
            this.floorsT = CountInstancesFromDocument(BuiltInCategory.OST_Floors);
            this.wallsT = CountInstancesFromDocument(BuiltInCategory.OST_Walls);
            this.optionsT = CountInstancesFromDocument(BuiltInCategory.OST_DesignOptions);
            this.worksetsT = new FilteredWorksetCollector(doc).OfKind(WorksetKind.UserWorkset).Count();
            this.levelsT = CountInstancesFromDocument(typeof(Level));
            this.gridsT = CountInstancesFromDocument(typeof(Grid));
            this.roomsT = CountInstancesFromDocument(BuiltInCategory.OST_Rooms);
            this.spacesT = CountInstancesFromDocument(BuiltInCategory.OST_MEPSpaces);
            this.sheetsT = CountInstancesFromDocument(BuiltInCategory.OST_Sheets);
            this.templatesViewT = IntancesFromDocument(typeof(View)).Cast<View>().Where(x => x.IsTemplate == true).Count();
            this.schedulesT = CountInstancesFromDocument(typeof(ViewSchedule));
        }

        private int CountInstancesFromDocument(BuiltInCategory bic)
        {
            return new FilteredElementCollector(_doc)
                .WhereElementIsNotElementType()
                .OfCategory(bic)
                .Count();
        }

        private int CountInstancesFromDocument(Type clase)
        {
            return IntancesFromDocument(clase)
                .Count();
        }

        private FilteredElementCollector IntancesFromDocument(Type clase)
        {
            return new FilteredElementCollector(_doc)
                .WhereElementIsNotElementType()
                .OfClass(clase);
        }

        private int CountTypesFromDocument(Type clase)
        {
            return new FilteredElementCollector(_doc)
                .WhereElementIsElementType()
                .OfClass(clase)
                .Count();
        }
    }
}
