using ePlanifModelsLib;
using ModelLib;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using ViewModelLib;
using ViewModelLib.Attributes;

namespace ePlanifViewModelsLib
{
	public class PhotoViewModel : WCFViewModel<Photo>
    {

        public int? PhotoID
        {
            get { return Model.PhotoID; }
            set { Model.PhotoID = value; OnPropertyChanged(); }
        }
		
		[IntListProperty(Header = "Employee", IsMandatory = false, IsReadOnly = false, DisplayMemberPath = "FullName", SelectedValuePath = "EmployeeID", SourcePath = "Service.Employees")]
		public int? EmployeeID
		{
			get { return Model.EmployeeID; }
			set { Model.EmployeeID = value; OnPropertyChanged(); }
		}

		private BitmapImage image;
		public BitmapImage Image
		{ 
			get { return image; }
		}


		public PhotoViewModel(ePlanifServiceViewModel Service)
			:base(Service)
		{
			
		}
		
		protected override Task<Photo> OnLoadModelAsync()
        {
            return Task.FromResult(Model);
        }

		public void RefreshImage()
		{
			try
			{
				image = new BitmapImage();
				image.BeginInit();
				image.StreamSource = new MemoryStream(Model.Data);
				image.EndInit();
			}
			catch (Exception ex)
			{
				ViewModel.Log(ex);
				image = null;
			}
		}
		protected override async Task OnLoadedAsync()
		{
			await base.OnLoadedAsync();
			if ((image==null) && (Model.Data!=null)) RefreshImage();
		}



	}
}
