using ePlanifViewModelsLib;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
	/// Logique d'interaction pour EmployeesAdminView.xaml
	/// </summary>
	public partial class EmployeesAdminView : UserControl
	{
		public static readonly DependencyProperty OwnerWindowProperty = DependencyProperty.Register("OwnerWindow", typeof(Window), typeof(EmployeesAdminView));
		public Window OwnerWindow
		{
			get { return (Window)GetValue(OwnerWindowProperty); }
			set { SetValue(OwnerWindowProperty, value); }
		}

		public EmployeesAdminView()
		{
			InitializeComponent();
		}

		private void DeleteCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			EmployeeViewModel employee;
			employee = listBox?.SelectedItem as EmployeeViewModel;
			e.CanExecute = (employee!= null) && (employee.Photo!=null);
			e.Handled = true;
		}

		private async void DeleteCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			EmployeeViewModel employee;

			employee = listBox.SelectedItem as EmployeeViewModel;
			if (employee == null) return;

			await employee.DeletePhotoAsync();

		}


		private void UploadCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = listBox?.SelectedItem != null;
			e.Handled = true;
		}

		private async void UploadCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			OpenFileDialog dialog;
			EmployeeViewModel employee;

			employee = listBox.SelectedItem as EmployeeViewModel;
			if (employee == null) return;

			dialog = new OpenFileDialog();
			dialog.Title = "Select a photo to upload";
			dialog.FileName = "*.*";
			dialog.Filter = "All files|*.*";
			if (dialog.ShowDialog(OwnerWindow) ?? false)
			{
				await UploadPhotoAsync(employee, dialog.FileName);
			}
		}

		private byte[] GetImageData(BitmapSource Source)
		{
			MemoryStream memStream = new MemoryStream();
			PngBitmapEncoder encoder = new PngBitmapEncoder();
			encoder.Frames.Add(BitmapFrame.Create(Source));
			encoder.Save(memStream);
			return memStream.ToArray();
		}

		private async Task UploadPhotoAsync(EmployeeViewModel Employee, string Path)
		{
			BitmapImage bitmapImage;
			byte[] data;

			try
			{
				using (FileStream stream = new FileStream(Path, FileMode.Open))
				{
					bitmapImage = new BitmapImage();
					bitmapImage.BeginInit();
					bitmapImage.DecodePixelWidth = 64;
					bitmapImage.DecodePixelHeight = 64;
					bitmapImage.StreamSource = stream;
					bitmapImage.EndInit();

					data = GetImageData(bitmapImage);
				}
				await Employee.UploadPhotoAsync(data);

			}
			catch (Exception ex)
			{
				MessageBox.Show(OwnerWindow, ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}

		}


	}
}
