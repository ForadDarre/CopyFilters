#region Namespaces
using System;
using System.Diagnostics;
using System.Collections.Generic;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
#endregion

namespace CopyFilters.Revit
{
	public static class AddFiltersService
	{
		public static void SetFilters(List<Element> filters, List<Element> views, Document doc)
		{
			foreach (Element filter in filters)
			{
				OverrideGraphicSettings settings = doc.ActiveView.GetFilterOverrides(filter.Id);
				bool filterVisibility = doc.ActiveView.GetFilterVisibility(filter.Id);

				foreach (View view in views)
				{
					try
					{
						if (!view.IsFilterApplied(filter.Id))
						{
							view.AddFilter(filter.Id);
						}

						view.SetFilterOverrides(filter.Id, settings);
						view.SetFilterVisibility(filter.Id, filterVisibility);
					}

					catch (Exception)
					{
						TaskDialog.Show("Error!", "Error in adding filters to the view '" + view.Title + "'.");
					}
				}
			}
		}
	}
}
