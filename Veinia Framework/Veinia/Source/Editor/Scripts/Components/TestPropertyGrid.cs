using Myra.Graphics2D.UI;
using Myra.Graphics2D.UI.Properties;

namespace Veinia.Editor
{
	public class TestPropertyGrid : Component
	{
		public override void Initialize()
		{
			var propertyGrid = new PropertyGrid
			{
				Object = new Properties { Boolean = true, Float = 10 },
				Width = 350
			};

			var window = new Window
			{
				Title = "Properties",
				Content = propertyGrid
			};

			window.Show(Globals.desktop);
		}
	}

	public class Properties
	{
		public float Float { get; set; }
		public bool Boolean { get; set; }

		public Options options;
		public enum Options
		{
			OptionA,
			OptionB,
			OptionC
		}
	}
}
