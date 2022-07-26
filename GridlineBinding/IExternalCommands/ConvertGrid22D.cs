using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using GridlineBinding.Services;
using GridlineBinding.Views;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GridlineBinding.IExternalCommands
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    class ConvertGrid22D : IExternalCommand
    {
        private static Document _doc;

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            RevitManager.CommandData = commandData;
            _doc = RevitManager.Document;

            try
            {
                DoJob();
                ShowReport();
            }
            catch (Exception e)
            {
                var report = $"Ошибка: {e.Message}";
                var extra = $"Ошибка: {e}";
                var reportWindow = new ReportWindow(report, extra);
                reportWindow.ShowDialog();
            }

            return Result.Succeeded;
        }

        public void ShowReport()
        {
            var report = "Выполнено";
            var reportWindow = new ReportWindow(report);
            reportWindow.ShowDialog();
        }
        public void DoJob()
        {
            using var t = new Transaction(_doc);

            t.Start(App.Title);

            RVGrids rvGrids = new RVGrids(_doc);
            rvGrids.Set2D3D(DatumEnds.End0, _doc.ActiveView, DatumExtentType.ViewSpecific);
            rvGrids.Set2D3D(DatumEnds.End1, _doc.ActiveView, DatumExtentType.ViewSpecific);

            t.Commit();
        }
    }
}