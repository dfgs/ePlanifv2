using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ePlanifViewModelsLib.Commands
{
	public class CutPasteCommand<ViewModelType, ModelType> : Command
		where ViewModelType : WCFViewModel<ModelType>
	{

		private WCFViewModelCollection<ViewModelType, ModelType> collection;
		private ViewModelType cutItem;
		private ViewModelType newItem;

		public override string Description
		{
			get { return "Remove item"; }
		}


		public CutPasteCommand(WCFViewModelCollection<ViewModelType, ModelType> Collection, ViewModelType CutItem,ViewModelType NewItem)
		{
			this.collection = Collection;
			this.cutItem = CutItem;
			this.newItem = NewItem;
		}

		public override async Task<bool> ExecuteAsync()
		{
			return await collection.AddAsync(newItem) & await collection.RemoveAsync(cutItem);
		}

		public override async Task<bool> CancelAsync()
		{
			return await collection.AddAsync(cutItem, false) & await collection.RemoveAsync(newItem);
		}


	}
}
