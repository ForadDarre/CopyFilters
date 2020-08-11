using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Autodesk.Revit.DB;

namespace CopyFilters
{
	/// <summary>
	/// Логика взаимодействия для Menu.xaml
	/// </summary>
	public partial class Menu : UserControl
	{
		public Menu(Document doc)
		{
			MainVM vm = new MainVM(doc);
			DataContext = vm;

			if (vm.CloseAction == null)
				vm.CloseAction = new Action(() => Window.GetWindow(this).Close());

			InitializeComponent();
		}
	}
}
