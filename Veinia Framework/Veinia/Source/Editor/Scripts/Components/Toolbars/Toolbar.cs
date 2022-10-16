using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Myra.Graphics2D.UI;

namespace Veinia.Editor
{
	public class Toolbar
	{
		public string toolbarName;
		public ToolbarBehaviour toolbarBehaviour;

		public Keys shortcut;
		public Widget content;


		public Toolbar(string toolbarName, ToolbarBehaviour toolbarBehaviour)
		{
			this.toolbarName = toolbarName;
			this.toolbarBehaviour = toolbarBehaviour;
		}

		public virtual void OnInitialize(GameObject gameObject) { toolbarBehaviour.gameObject = gameObject; toolbarBehaviour.OnInitialize(); }
		public virtual void OnUpdate() { toolbarBehaviour.OnUpdate(); }
		public virtual void OnDraw(SpriteBatch sb) { toolbarBehaviour.OnDraw(sb); }
	}
}
