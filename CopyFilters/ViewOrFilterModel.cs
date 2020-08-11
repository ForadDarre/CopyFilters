#region Namespaces
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Autodesk.Revit.DB;
#endregion

namespace CopyFilters
{
	public class ViewOrFilterModel : INotifyPropertyChanged
	{
		// Model for choosing listview items
		private Element filterOrView;
		private string name;
		private bool isSelected;

		public Element FilterOrView
		{
			get { return filterOrView; }

			set
			{
				filterOrView = value;
				OnPropertyChanged("FilterOrView");
			}
		}

		public string Name
		{
			get { return name; }

			set
			{
				name = value;
				OnPropertyChanged("Name");
			}
		}

		public bool IsSelected
		{
			get { return isSelected; }

			set
			{
				isSelected = value;
				OnPropertyChanged("IsSelected");
			}
		}


		public ViewOrFilterModel(Element element)
		{
			filterOrView = element;
			name = element.Name;
		}

		public ViewOrFilterModel(Element element, string _name)
		{
			filterOrView = element;
			name = _name;
		}


		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged([CallerMemberName]string prop = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
		}
	}
}
