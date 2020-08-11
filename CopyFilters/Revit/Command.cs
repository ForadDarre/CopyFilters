#region Namespaces
using System;
using System.Diagnostics;
using System.Windows;

using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
#endregion

#region Info
/*
    Description: Copy filters and their settings from the active view to other views.

    Developer: Aleksandr Mozhaev
*/
#endregion

namespace CopyFilters
{
	[Transaction(TransactionMode.Manual)]
	[RegenerationAttribute(RegenerationOption.Manual)]
	public class Command : IExternalCommand
	{
		public Result Execute(
		  ExternalCommandData commandData,
		  ref string message,
		  ElementSet elements)
		{
			UIApplication uiapp = commandData.Application;
			UIDocument uidoc = uiapp.ActiveUIDocument;
			Autodesk.Revit.ApplicationServices.Application app = uiapp.Application;
			Document doc = uidoc.Document;

			try
			{
				if (doc.IsFamilyDocument || doc.IsReadOnly)
				{
					TaskDialog.Show("Error!", "The document has an incorrect format or is opened in readonly mode.");
				}

				else
				{
					if(doc.ActiveView.ViewType == ViewType.FloorPlan
					|| doc.ActiveView.ViewType == ViewType.CeilingPlan
					|| doc.ActiveView.ViewType == ViewType.Legend
					|| doc.ActiveView.ViewType == ViewType.DrawingSheet
					|| doc.ActiveView.ViewType == ViewType.DraftingView
					|| doc.ActiveView.ViewType == ViewType.Elevation
					|| doc.ActiveView.ViewType == ViewType.ThreeD
					|| doc.ActiveView.ViewType == ViewType.Section)
					{
						using (Transaction trans = new Transaction(doc, "Copy filters"))
						{
							Menu menuContext = new Menu(doc);
							Window menu = new Window
							{
								Title = "Copy filters",
								Content = menuContext,
								Width = 620,
								Height = 450,
								WindowStartupLocation = WindowStartupLocation.CenterScreen,
							};

							try
							{
								menu.ShowDialog();
							}

							catch (Exception ex)
							{
								TaskDialog.Show("Error!", ex.ToString());
								menu.Close();
								return Result.Cancelled;
							}
						}
					}
					
					else
					{
						TaskDialog.Show("Error!", "Incorrect active view. Open one of the following:\n" +
							"Floor plan\n" +
							"Ceiling plan\n" +
							"Legend\n" +
							"Sheet\n" +
							"Drafting view\n" +
							"Elevation\n" +
							"3D View\n" +
							"Section.");
						return Result.Cancelled;
					}
				}

			}

			catch (Autodesk.Revit.Exceptions.OperationCanceledException)
			{
				return Result.Cancelled;
			}

			catch (Exception ex)
			{
				TaskDialog.Show("Error!", ex.ToString());
				return Result.Cancelled;
			}

			return Result.Succeeded;
		}
	}
}
