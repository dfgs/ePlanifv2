using ePlanifViewModelsLib;
using System;
using System.Collections;
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

namespace ePlanifv2.Views
{
	/// <summary>
	/// Logique d'interaction pour GroupsView.xaml
	/// </summary>
	public partial class GroupsView : UserControl
	{
		private IGroupViewModel SelectedGroup
		{
			get { return treeView.SelectedItem as IGroupViewModel; }
		}



		public static readonly DependencyProperty PotentialMembersProperty = DependencyProperty.Register("PotentialMembers", typeof(IEnumerable), typeof(GroupsView));
		public IEnumerable PotentialMembers
		{
			get { return (IEnumerable)GetValue(PotentialMembersProperty); }
			set { SetValue(PotentialMembersProperty, value); }
		}


		public static readonly DependencyProperty GroupsProperty = DependencyProperty.Register("Groups", typeof(IEnumerable), typeof(GroupsView));
		public IEnumerable Groups
		{
			get { return (IEnumerable)GetValue(GroupsProperty); }
			set { SetValue(GroupsProperty, value); }
		}



		public GroupsView()
		{
			InitializeComponent();
		}

		private void AddMemberCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = SelectedGroup != null;
			e.Handled = true;
		}

		private async void AddMemberCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			MembersWindow window;

			window = new MembersWindow() { Owner = Application.Current.MainWindow, DataContext = PotentialMembers };
			if (window.ShowDialog() ?? false)
			{
				foreach (object item in window.SelectedItems)
				{
					await SelectedGroup.AddMemberAsync(item);
				}
			}
		}

	}
}
