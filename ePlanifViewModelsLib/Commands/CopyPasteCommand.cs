using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePlanifViewModelsLib.Commands
{
	public class CopyPasteCommand<ViewModelType, ModelType> : Command
		where ViewModelType : WCFViewModel<ModelType>
	{

		private WCFViewModelCollection<ViewModelType, ModelType> collection;
		private ViewModelType item;

		public override string Description
		{
			get { return "Copy item"; }
		}


		public CopyPasteCommand(WCFViewModelCollection<ViewModelType, ModelType> Collection, ViewModelType Item)
		{
			this.collection = Collection;
			this.item = Item;
		}

		public override async Task<bool> ExecuteAsync()
		{
			bool result = await collection.AddAsync(item, false);
			return result;
		}

		public override async Task<bool> CancelAsync()
		{
			return await collection.RemoveAsync(item);
			//return await Task.FromResult(false);
		}


	}
}
