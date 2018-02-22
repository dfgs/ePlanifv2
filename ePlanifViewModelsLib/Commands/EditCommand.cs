using DatabaseModelLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModelLib;

namespace ePlanifViewModelsLib.Commands
{
	public class EditCommand<ViewModelType, ModelType> : Command
		where ViewModelType : WCFViewModel<ModelType>
		where ModelType:new()
	{

		private WCFViewModelCollection<ViewModelType, ModelType> collection;
		private ViewModelSchema schema;
		private bool showWindow;

		public override string Description
		{
			get { return "Edit items"; }
		}


		public EditCommand(WCFViewModelCollection<ViewModelType, ModelType> Collection, ViewModelType[] Items)
		{
			this.collection = Collection;
			schema = new ViewModelSchema(Items, typeof(ViewModelType));
			showWindow = true;
		}

		public override async Task<bool> ExecuteAsync()
		{
			if (!showWindow) schema.Commit();
			bool result= await collection.EditAsync(schema, showWindow);
			showWindow = false;

			return result;
		}

		public override async Task<bool> CancelAsync()
		{
			schema.Revert();
			bool result = await collection.EditAsync(schema, false);

			return result;
		}


	}
}
