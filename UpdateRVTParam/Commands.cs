using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using DesignAutomationFramework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Autodesk.Forge.Sample.DesignAutomation.Revit
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Commands : IExternalDBApplication
    {
        //Path of the project(i.e)project where your Window family files are present
        string OUTPUT_FILE = "OutputFile.json";

        public ExternalDBApplicationResult OnStartup(ControlledApplication application)
        {
            DesignAutomationBridge.DesignAutomationReadyEvent += HandleDesignAutomationReadyEvent;
            return ExternalDBApplicationResult.Succeeded;
        }

        private void HandleDesignAutomationReadyEvent(object sender, DesignAutomationReadyEventArgs e)
        {
            LogTrace("Design Automation Ready event triggered...");
            e.Succeeded = true;
            ExtractInfo(e.DesignAutomationData);
        }

        private void ExtractInfo(DesignAutomationData _data)
        {
            var doc = _data.RevitDoc;
            LogTrace("Rvt File opened...");
            //InputParams inputParameters = JsonConvert.DeserializeObject<InputParams>(File.ReadAllText("params.json"));

            var data = new List<KeyValuePair<string, int>>();
            using (Transaction trans = new Transaction(doc))
            {
                trans.Start("Extract Good practices info");

                //Error y recuento
                data.Add(new KeyValuePair<string, int>("Avisos", doc.GetWarnings().Count));
                //Vistas y recuento
                data.Add(new KeyValuePair<string, int>("Vistas", GetVistasValue(doc)));

                //Parte Andrea
                //Información de Modelo 
                var fileSize = doc.PathName.Length;
                var dwgT = new FilteredElementCollector(doc).OfClass(typeof(CADLinkType)).WhereElementIsElementType().ToList().Count();

                //Familias de proyecto
                var familiesC = new FilteredElementCollector(doc).WhereElementIsNotElementType().OfClass(typeof(Family)).ToElements().Count();
                var groupsM = new FilteredElementCollector(doc).OfClass(typeof(Group)).ToElements().Count();
                var genericModel = new FilteredElementCollector(doc).WhereElementIsNotElementType().OfCategory(BuiltInCategory.OST_GenericModel).ToElements().Count();
                var floorsT = new FilteredElementCollector(doc).WhereElementIsNotElementType().OfCategory(BuiltInCategory.OST_Floors).ToElements().Count();
                var wallsT = new FilteredElementCollector(doc).WhereElementIsNotElementType().OfCategory(BuiltInCategory.OST_Walls).ToElements().Count();

                
                //Información general de proyecto
                var optionsT = new FilteredElementCollector(doc).WhereElementIsNotElementType().OfCategory(BuiltInCategory.OST_DesignOptions).ToList().Count();
                var worksetsT = new FilteredWorksetCollector(doc).OfKind(WorksetKind.UserWorkset).ToList().Count();
                var levelsT = new FilteredElementCollector(doc).WhereElementIsNotElementType().OfClass(typeof(Level)).ToElements().Count();
                var gridsT = new FilteredElementCollector(doc).WhereElementIsNotElementType().OfClass(typeof(Grid)).ToElements().Count();

                //Habitaciones y espacios
                var roomsT = new FilteredElementCollector(doc).WhereElementIsNotElementType().OfCategory(BuiltInCategory.OST_Rooms).ToElements().Count();
                var spacesT = new FilteredElementCollector(doc).WhereElementIsNotElementType().OfCategory(BuiltInCategory.OST_MEPSpaces).ToElements().Count();

                //Vistas de modelo
                var sheetsT = new FilteredElementCollector(doc).WhereElementIsNotElementType().OfCategory(BuiltInCategory.OST_Sheets).ToElements().Count();
                var templatesViewT = new FilteredElementCollector(doc).WhereElementIsNotElementType().OfClass(typeof(View)).Select(x => x as View).Where(x => x.IsTemplate == true).ToList().Count();
                var schedulesT = new FilteredElementCollector(doc).WhereElementIsNotElementType().OfClass(typeof(ViewSchedule)).ToElements().Count();


                trans.RollBack();
            }

            //Save the updated file by overwriting the existing file
            ModelPath ProjectModelPath = ModelPathUtils.ConvertUserVisiblePathToModelPath(OUTPUT_FILE);
            var _path = ModelPathUtils.ConvertModelPathToUserVisiblePath(ProjectModelPath);
            //Save the project file with updated window's parameters
            LogTrace("Saving file...");
            var json = JsonConvert.SerializeObject(data);
            File.WriteAllText(_path, json);
        }

        private int GetVistasValue(Document doc)
        {
            var col = new FilteredElementCollector(doc)
                .WhereElementIsNotElementType()
                .OfCategory(BuiltInCategory.OST_Views);

            return col.Count();
        }

        public ExternalDBApplicationResult OnShutdown(ControlledApplication application)
        {
            return ExternalDBApplicationResult.Succeeded;
        }

        private void SetElementParameter(FamilySymbol FamSym, BuiltInParameter paraMeter, double parameterValue)
        {
            FamSym.get_Parameter(paraMeter).Set(parameterValue);
        }

        public class InputParams
        {
            public double Width { get; set; }
            public double Height { get; set; }
        }

        /// <summary>
        /// This will appear on the Design Automation output
        /// </summary>
        private static void LogTrace(string format, params object[] args) { System.Console.WriteLine(format, args); }
    }
}
