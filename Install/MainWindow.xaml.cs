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

namespace Install
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InstallVM vm = new InstallVM();
			DataContext = vm;

			this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

			if (vm.CloseAction == null)
				vm.CloseAction = new Action(() => Window.GetWindow(this).Close());

			InitializeComponent();
		}
	}
}
