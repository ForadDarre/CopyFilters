#region Namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using System.Windows.Data;

using System.Windows;

using Autodesk.Revit.DB;

using CopyFilters.Revit;
#endregion

namespace CopyFilters
{
	public class MainVM : INotifyPropertyChanged
	{
		#region Variables
		private Document doc;
		private ObservableCollection<ViewOrFilterModel> filters;
		private ObservableCollection<ViewOrFilterModel> views;

		public ObservableCollection<ViewOrFilterModel> Filters
		{
			get { return filters; }

			set
			{
				filters = value;
				OnPropertyChanged("Filters");
			}
		}

		public ObservableCollection<ViewOrFilterModel> Views
		{
			get { return views; }

			set
			{
				views = value;
				OnPropertyChanged("Views");
			}
		}
		#endregion

		#region Collection supporting stuff
		private void Filters_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.OldItems != null)
				foreach (var item in e.OldItems)
					((ViewOrFilterModel)item).PropertyChanged -= FiltersChanged;

			if (e.NewItems != null)
				foreach (var item in e.NewItems)
					((ViewOrFilterModel)item).PropertyChanged += FiltersChanged;
		}

		private void FiltersChanged(object sender, PropertyChangedEventArgs e)
		{
			SelectedFilters.Refresh();
		}

		public CollectionViewSource _sourceForSelectedFilters;    // is necessary, because garbage collector deletes SelectedItems
		public ICollectionView SelectedFilters
		{
			get { return _sourceForSelectedFilters.View; }
		}



		private void Views_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.OldItems != null)
				foreach (var item in e.OldItems)
					((ViewOrFilterModel)item).PropertyChanged -= ViewsChanged;

			if (e.NewItems != null)
				foreach (var item in e.NewItems)
					((ViewOrFilterModel)item).PropertyChanged += ViewsChanged;
		}

		private void ViewsChanged(object sender, PropertyChangedEventArgs e)
		{
			SelectedViews.Refresh();
		}

		public CollectionViewSource _sourceForSelectedViews;    // is necessary, because garbage collector deletes SelectedItems
		public ICollectionView SelectedViews
		{
			get { return _sourceForSelectedViews.View; }
		}
		#endregion

		#region Commands
		private RelayCommand okCommand;
		public RelayCommand OKCommand
		{
			get
			{
				return okCommand ??
					(okCommand = new RelayCommand(param =>
					{
						using (Transaction trans = new Transaction(doc, "Copy filters"))
						{
							trans.Start();

							AddFiltersService.SetFilters(filters.Where(x => x.IsSelected).Select(x => x.FilterOrView).ToList(), 
								views.Where(x => x.IsSelected).Select(x => x.FilterOrView).ToList(), doc);

							trans.Commit();
						}
						Close();
					}));
			}
		}

		private RelayCommand cancelCommand;
		public RelayCommand CancelCommand
		{
			get
			{
				return cancelCommand ??
					(cancelCommand = new RelayCommand(param => Close()));
			}
		}

		public Action CloseAction { get; set; }

		private void Close()
		{
			CloseAction();
		}
		#endregion


		public MainVM(Document document)
		{
			doc = document;

			Filters = new ObservableCollection<ViewOrFilterModel>();
			Filters.CollectionChanged += Filters_CollectionChanged;
			Filters = LoadDataService.GetFilters(doc);

			_sourceForSelectedFilters = new CollectionViewSource()
			{
				Source = Filters,
				IsLiveFilteringRequested = true,
				LiveFilteringProperties = { "FilterProperty" }
			};

			SelectedFilters.Filter = o => ((ViewOrFilterModel)o).IsSelected;


			Views = new ObservableCollection<ViewOrFilterModel>();
			Views.CollectionChanged += Views_CollectionChanged;
			Views = LoadDataService.GetViews(doc);

			_sourceForSelectedViews = new CollectionViewSource()
			{
				Source = Views,
				IsLiveFilteringRequested = true,
				LiveFilteringProperties = { "FilterProperty" }
			};

			SelectedViews.Filter = o => ((ViewOrFilterModel)o).IsSelected;
		}

		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged([CallerMemberName]string prop = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
		}
	}
}
