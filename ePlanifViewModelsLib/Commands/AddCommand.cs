using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePlanifViewModelsLib.Commands
{
	public class AddCommand<ViewModelType, ModelType> : Command
		where ViewModelType : WCFViewModel<ModelType>
	{

		private WCFViewModelCollection<ViewModelType, ModelType> collection;
		private ViewModelType item;
		private bool showWindow;

		public override string Description
		{
			get { return "Add item"; }
		}


		public AddCommand(WCFViewModelCollection<ViewModelType, ModelType> Collection, ViewModelType Item)
		{
			this.collection = Collection;
			this.item = Item;
			showWindow = true;
		}

		public override async Task<bool> ExecuteAsync()
		{
			bool result=await collection.AddAsync(item, showWindow);
			showWindow = false;
			return result;
		}

		public override async Task<bool> CancelAsync()
		{
			return await collection.RemoveAsync(item);
			//return await Task.FromResult(false);
		}


	}
}
