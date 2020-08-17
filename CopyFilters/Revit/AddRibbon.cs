#region Namespaces
using System;
using System.Windows.Media.Imaging;
using System.IO;
using System.Reflection;

using Autodesk.Revit.UI;
#endregion

namespace CopyFilters
{
	public class AddRibbon : IExternalApplication
	{
		public Result OnStartup(UIControlledApplication application)
		{
			RibbonPanel panel = application.CreateRibbonPanel("‎Filters‎"); // don't forget: there's an invisible symbol the the beginning and in the end (to avoid reiteration)
			AddPushButtonCopyFilters(panel);

			return Result.Succeeded;
		}

		public Result OnShutdown(UIControlledApplication application)
		{
			// nothing to clean up in this simple case
			return Result.Succeeded;
		}

		private void AddPushButtonCopyFilters(RibbonPanel rp)
		{
			string path = Assembly.GetExecutingAssembly().Location;

			PushButtonData buttonDataCopyFilters = new PushButtonData("CopyFilters", "Copy Filters", path, "CopyFilters.Command");

			BitmapSource bitmap32 = GetEmbeddedImage("CopyFilters.logo.png"); // https://www.flaticon.com/free-icon/funnel_929427

			buttonDataCopyFilters.LargeImage = bitmap32;

			PushButton pushButtonCopyFilters = rp.AddItem(buttonDataCopyFilters) as PushButton;
			pushButtonCopyFilters.ToolTip = "Copy filters and their settings from the active view to the other views.";

			ContextualHelp help = new ContextualHelp(ContextualHelpType.Url, "https://apps.autodesk.com/en/Publisher/PublisherHomepage?ID=T64TQNF5ELQ5");
			pushButtonCopyFilters.SetContextualHelp(help);
		}

		static BitmapSource GetEmbeddedImage(string name)
		{
			try
			{
				Assembly a = Assembly.GetExecutingAssembly();
				Stream s = a.GetManifestResourceStream(name);
				return BitmapFrame.Create(s);
			}
			catch
			{
				return null;
			}
		}
	}
}
