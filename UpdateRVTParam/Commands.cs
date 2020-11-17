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

                data.Add(new KeyValuePair<string, int>("Avisos", doc.GetWarnings().Count));

                data.Add(new KeyValuePair<string, int>("Vistas", GetVistasValue(doc)));

                var muros = new FilteredElementCollector(doc)
                    .WhereElementIsNotElementType()
                    .OfCategory(BuiltInCategory.OST_Walls)
                    .Count();
                var suelos = new FilteredElementCollector(doc)
                    .WhereElementIsNotElementType()
                    .OfCategory(BuiltInCategory.OST_Floors)
                    .Count();
                var puertas = new FilteredElementCollector(doc)
                    .WhereElementIsNotElementType()
                    .OfCategory(BuiltInCategory.OST_Doors)
                    .Count();

                data.Add(new KeyValuePair<string, int>("muros", muros));
                data.Add(new KeyValuePair<string, int>("suelos", suelos));
                data.Add(new KeyValuePair<string, int>("puertas", puertas));

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
