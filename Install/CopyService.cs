using System;
using System.Windows;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace Install
{
	public static class CopyService
	{
		static readonly string pluginName = "CopyFilters";
		static readonly string addinName = pluginName + ".addin";

		public static void Copy(int revitVersion)
		{
			string currentDirectory = Directory.GetCurrentDirectory();

			if (Directory.Exists(@"C:\ProgramData\Autodesk\Revit\Addins\" + revitVersion))
			{
				if (File.Exists(currentDirectory + @"\" + addinName)
				&& Directory.Exists(currentDirectory + @"\" + pluginName)
				&& File.Exists(currentDirectory + @"\" + pluginName + @"\" + pluginName + ".dll")
				&& File.Exists(currentDirectory + @"\" + pluginName + @"\" + pluginName + ".pdb"))
				{
					if (File.Exists(@"C:\ProgramData\Autodesk\Revit\Addins\" + revitVersion + @"\" + addinName)
						|| Directory.Exists(@"C:\ProgramData\Autodesk\Revit\Addins\Plugins\" + pluginName))
					{
						System.Windows.Forms.DialogResult dialogResult = System.Windows.Forms.MessageBox.Show("The plugin is already installed. " +
							"Do you want to reinstall it? Note: if you are not sure, check out the readme file.", "Warning", MessageBoxButtons.YesNo);
						if (dialogResult == System.Windows.Forms.DialogResult.Yes)
						{
							CopyAll(currentDirectory, @"C:\ProgramData\Autodesk\Revit\Addins\Plugins\" + pluginName, revitVersion);
						}
						else if (dialogResult == System.Windows.Forms.DialogResult.No)
						{
						}
					}
					else
					{
						CopyAll(currentDirectory, @"C:\ProgramData\Autodesk\Revit\Addins\Plugins\" + pluginName, revitVersion);
					}
				}

				else
				{
					System.Windows.MessageBox.Show("Missing source files. Redownload the archive.", "Error");
				}
			}

			else
			{
				System.Windows.MessageBox.Show("Can't find the folder with the entered Revit version. Try to copy the files manually using the instructions in readme file.", "Error");
			}
		}

		private static void CopyAll(string currentDirectory, string targetDirectory, int revitVersion)
		{
			string sourceDirectory = currentDirectory + @"\" + pluginName;

			File.Copy(currentDirectory + @"\" + addinName, @"C:\ProgramData\Autodesk\Revit\Addins\" + revitVersion + @"\" + addinName, true);

			DirectoryInfo diSource = new DirectoryInfo(sourceDirectory);
			DirectoryInfo diTarget = new DirectoryInfo(targetDirectory);

			CopyPlugin(diSource, diTarget);

			System.Windows.Forms.MessageBox.Show("The installation completed succesfully", "Success");
		}

		private static void CopyPlugin(DirectoryInfo source, DirectoryInfo target)
		{
			Directory.CreateDirectory(target.FullName);

			// Copy each file into the new directory.
			foreach (FileInfo fi in source.GetFiles())
			{
				fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
			}
		}
	}
}
