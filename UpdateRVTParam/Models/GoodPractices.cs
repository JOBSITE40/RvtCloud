using Autodesk.Revit.DB;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UpdateRVTParam.Models
{
    public class GoodPractices
    {
        private Document _doc;

        #region Variables
        public int nAvisos { get; set; }
        public int nVistas { get; set; }
        public int dwgT { get; set; }
        public int rvtT { get; set; }
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
        public int imagesT { get; set; }
        public int areasT { get; set; }
        public int ceilingT { get; set; }
        public int railingT { get; set; }
        public int roofT { get; set; }
        public int foundationT { get; set; }
        public int stairsT { get; set; }
        public int rampT { get; set; }
        public int curtainWall { get; set; }
        public int ductsT { get; set; }
        public int pipesT { get; set; }
        public int linepatternT { get; set; }
        public int dimensionT { get; set; }
        public int columnsT { get; set; }
        public int framingT { get; set; }
        public int scopeBox { get; set; }
        public int legendsT { get; set; }
        public int draftingViewT { get; set; }
        public int textT { get; set; }
        public int assemblyT { get; set; }
        #endregion

        #region Listas
        public List<string> wallTypes { get; set; }
        public List<string> floorTypes { get; set; }

        public List<string> ceilingsTypes { get; set; }
        public List<string> roofTypes { get; set; }
        public List<string> railingTypes { get; set; }
        public List<string> worksetsNames { get; set; }
        public List<string> pipesystem { get; set; }
        public List<string> BrowserOrganization { get; set; }
        #endregion

        public GoodPractices(Document doc)
        {
            this._doc = doc;

            #region General Information
            //Recuentos de elementos
            this.nAvisos = doc.GetWarnings().Count;
            this.nVistas = CountInstancesFromDocument(BuiltInCategory.OST_Views);
            this.dwgT = CountTypesFromDocument(typeof(CADLinkType));
            this.groupsM = CountInstancesFromDocument(typeof(Group));
            this.optionsT = CountInstancesFromDocument(BuiltInCategory.OST_DesignOptions);
            this.worksetsT = new FilteredWorksetCollector(doc).OfKind(WorksetKind.UserWorkset).Count();
            this.levelsT = CountInstancesFromDocument(typeof(Level));
            this.gridsT = CountInstancesFromDocument(typeof(Grid));
            this.roomsT = CountInstancesFromDocument(BuiltInCategory.OST_Rooms);
            this.spacesT = CountInstancesFromDocument(BuiltInCategory.OST_MEPSpaces);
            this.sheetsT = CountInstancesFromDocument(BuiltInCategory.OST_Sheets);
            this.templatesViewT = IntancesFromDocument(typeof(View)).Cast<View>().Where(x => x.IsTemplate == true).Count();
            this.schedulesT = CountInstancesFromDocument(typeof(ViewSchedule));
            this.imagesT = CountInstancesFromDocument(typeof(ImageType)); //Revisar, no se exactamente que devuelve el Image Type
            this.rvtT = CountTypesFromDocument(typeof(RevitLinkInstance));
            this.areasT = CountTypesFromDocument(typeof(Area)); //Revisar que devuelve
            this.linepatternT = CountTypesFromDocument(typeof(LinePattern));
            this.dimensionT = CountTypesFromDocument(typeof(Dimension));
            this.scopeBox = CountInstancesFromDocument(BuiltInCategory.OST_VolumeOfInterest);
            this.legendsT = CountInstancesFromDocument(BuiltInCategory.OST_LegendComponents);
            this.draftingViewT = CountInstancesFromDocument(typeof(ViewDrafting));
            this.textT = CountInstancesFromDocument(BuiltInCategory.OST_TextNotes);
            this.assemblyT = CountInstancesFromDocument(typeof(AssemblyInstance));//Revisar si devuelve información correcta

            //Listas de elementos
            this.worksetsNames = new FilteredWorksetCollector(doc).OfKind(WorksetKind.UserWorkset).Select(wn => wn.Name).ToList();
            this.BrowserOrganization = new FilteredElementCollector(doc).OfClass(typeof(BrowserOrganization)).Select(bo => bo.Name).ToList();

            #endregion

            #region Architecture
            //Recuentos de elementos
            this.familiesC = CountInstancesFromDocument(typeof(Family));
            this.genericModel = CountInstancesFromDocument(BuiltInCategory.OST_GenericModel);
            this.floorsT = CountInstancesFromDocument(BuiltInCategory.OST_Floors);
            this.wallsT = CountInstancesFromDocument(BuiltInCategory.OST_Walls);
            this.ceilingT = CountInstancesFromDocument(BuiltInCategory.OST_Ceilings);
            this.railingT = CountInstancesFromDocument(BuiltInCategory.OST_Railings);
            this.roofT = CountInstancesFromDocument(BuiltInCategory.OST_Roofs);
            this.foundationT = CountInstancesFromDocument(BuiltInCategory.OST_StructuralFoundation);
            this.stairsT = CountInstancesFromDocument(BuiltInCategory.OST_Stairs);
            this.rampT = CountInstancesFromDocument(BuiltInCategory.OST_Ramps);
            this.curtainWall = CountInstancesFromDocument(BuiltInCategory.OST_CurtainWallPanels);
            
            //Listas de elementos
            this.wallTypes = new FilteredElementCollector(doc).WhereElementIsElementType().OfCategory(BuiltInCategory.OST_Walls).Select(mn => mn.Name).ToList();
            this.ceilingsTypes = new FilteredElementCollector(doc).WhereElementIsElementType().OfCategory(BuiltInCategory.OST_Ceilings).Select(cn => cn.Name).ToList();
            this.floorTypes = new FilteredElementCollector(doc).WhereElementIsElementType().OfCategory(BuiltInCategory.OST_Floors).Select(fn => fn.Name).ToList();
            this.railingTypes = new FilteredElementCollector(doc).WhereElementIsElementType().OfCategory(BuiltInCategory.OST_Railings).Select(fn => fn.Name).ToList();
            this.roofTypes = new FilteredElementCollector(doc).WhereElementIsElementType().OfCategory(BuiltInCategory.OST_Roofs).Select(fn => fn.Name).ToList();
            #endregion

            #region MEP
            //Recuentos de elementos
            this.ductsT = CountInstancesFromDocument(BuiltInCategory.OST_DuctCurves);
            this.pipesT = CountInstancesFromDocument(BuiltInCategory.OST_PipeSegments);
            //Listas de elementos
            #endregion

            #region Structure
            //Recuentos de elementos
            this.columnsT = CountInstancesFromDocument(BuiltInCategory.OST_StructuralColumns);
            this.framingT = CountInstancesFromDocument(BuiltInCategory.OST_StructuralFraming);

            //Listas de elementos

            #endregion



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
