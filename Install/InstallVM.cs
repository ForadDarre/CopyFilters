#region Namespaces
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
#endregion

namespace Install
{
	public class InstallVM : INotifyPropertyChanged
	{
		private int revitVersion;

		public int RevitVersion
		{
			get
			{
				return revitVersion;
			}

			set
			{
				revitVersion = value;
				OnPropertyChanged("RevitVersion");
			}
		}

		public Action CloseAction { get; set; }

		private void Close()
		{
			CloseAction();
		}

		private RelayCommand closeCommand;
		public RelayCommand CloseCommand
		{
			get
			{
				return closeCommand ??
					(closeCommand = new RelayCommand(param => Close()));
			}
		}

		private RelayCommand okCommand;
		public RelayCommand OKCommand
		{
			get
			{
				return okCommand ??
					(okCommand = new RelayCommand(param =>
					{
						CopyService.Copy(revitVersion);
						Close();
					}));
			}
		}

		public InstallVM()
		{
			revitVersion = 2021;
		}

		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged([CallerMemberName]string prop = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
		}
	}
}
