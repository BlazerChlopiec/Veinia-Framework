using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Myra.Graphics2D.UI;

namespace VeiniaFramework.Editor
{
	public class Toolbar
	{
		public string toolbarName;
		public ToolbarBehaviour toolbarBehaviour;

		public Keys shortcut;
		public Widget displayedToolbarContent;


		public Toolbar(string toolbarName, ToolbarBehaviour toolbarBehaviour)
		{
			this.toolbarName = toolbarName;
			this.toolbarBehaviour = toolbarBehaviour;
		}

		public virtual void OnInitialize(GameObject gameObject) { }
		public virtual void OnUpdate() { }
		public virtual void OnDraw(SpriteBatch sb) { }
	}
}
