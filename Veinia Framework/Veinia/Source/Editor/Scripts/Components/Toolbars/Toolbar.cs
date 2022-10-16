using Microsoft.Xna.Framework.Input;
using Myra.Graphics2D.UI;

namespace Veinia.Editor
{
	public class Toolbar
	{
		public string toolbarName;
		public Keys shortcut;
		public Widget content;


		public Toolbar(string toolbarName) => this.toolbarName = toolbarName;

		public virtual void OnInitialize(GameObject gameObject) { }
		public virtual void OnFocus(GameObject gameObject) { }
		public virtual void OnLostFocus(GameObject gameObject) { }
	}
}
