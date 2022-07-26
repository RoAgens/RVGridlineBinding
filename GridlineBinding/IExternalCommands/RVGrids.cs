using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GridlineBinding.IExternalCommands
{
    public class RVGrids
    {
        private static Document _doc;

        public RVGrids(Document doc) => _doc = doc;

        /// <summary>
        /// Превод осей из 2D в 3D и обратно
        /// </summary>
        /// <param name="grids"></param>
        /// <param name="end"></param>
        /// <param name="view"></param>
        /// <param name="type"></param>
        public void Set2D3D(DatumEnds end, View view, DatumExtentType type)
        {
            GetGrids().ForEach(y => y.SetDatumExtentType(end, view, type));
        }

        /// <summary>
        /// Поиск всех осей в проекте
        /// </summary>
        /// <returns></returns>
        private List<Grid> GetGrids()
        {
            return new FilteredElementCollector(_doc)
                               .WhereElementIsNotElementType().OfCategory(BuiltInCategory.OST_Grids)
                               .WhereElementIsNotElementType()
                               .ToElements()
                               .Cast<Grid>()
                               .ToList();
        }
    }
}
