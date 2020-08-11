#region Namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;

using Autodesk.Revit.DB;
#endregion

namespace CopyFilters.Revit
{
	public static class LoadDataService
	{
		public static ObservableCollection<ViewOrFilterModel> GetFilters(Document document)
		{
			IEnumerable<ElementId> filtersCollection = document.ActiveView.GetFilters();
			return new ObservableCollection<ViewOrFilterModel>(filtersCollection.Select(x => new ViewOrFilterModel(document.GetElement(x))));
		}

		public static ObservableCollection<ViewOrFilterModel> GetViews(Document document)
		{
			IEnumerable<View> viewsCollection = new FilteredElementCollector(document)
				.OfClass(typeof(View))
				.WhereElementIsNotElementType()
				.Cast<View>()
				.Where(view => view.ViewType == ViewType.FloorPlan
					|| view.ViewType == ViewType.CeilingPlan
					|| view.ViewType == ViewType.Legend
					|| view.ViewType == ViewType.DrawingSheet
					|| view.ViewType == ViewType.DraftingView
					|| view.ViewType == ViewType.Elevation
					|| view.ViewType == ViewType.ThreeD
					|| view.ViewType == ViewType.Section);

			return new ObservableCollection<ViewOrFilterModel>(viewsCollection.Select(x => new ViewOrFilterModel(x, x.Title)));
		}
	}
}
