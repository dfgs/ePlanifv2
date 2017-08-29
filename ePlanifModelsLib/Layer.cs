using DatabaseModelLib;
using ModelLib;


namespace ePlanifModelsLib
{
	public class Layer:ePlanifModel
    {

        public static readonly Column<Layer, int> LayerIDColumn = new Column<Layer, int>() {IsPrimaryKey=true,IsIdentity=true };
        public int? LayerID
        {
            get { return LayerIDColumn.GetValue(this); }
            set { LayerIDColumn.SetValue(this, value); }
        }
		
        public static readonly Column<Layer, Text> NameColumn = new Column<Layer, Text>() ;
        public Text? Name
        {
            get { return NameColumn.GetValue(this); }
            set { NameColumn.SetValue(this, value); }
        }

		[Revision(1)]
		public static readonly Column<Layer, Text> ColorColumn = new Column<Layer, Text>() {DefaultValue="LightGray" } ;
		public Text? Color
		{
			get { return ColorColumn.GetValue(this); }
			set { ColorColumn.SetValue(this, value); }
		}



		public static readonly Column<Layer, bool> IsDisabledColumn = new Column<Layer, bool>() { DefaultValue = false };
		public override bool? IsDisabled
		{
			get { return IsDisabledColumn.GetValue(this); }
			set { IsDisabledColumn.SetValue(this, value); }
		}

		



	}
}
