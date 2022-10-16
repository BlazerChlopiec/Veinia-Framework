using Microsoft.Xna.Framework.Graphics;

namespace Veinia.Editor
{
	public class ToolbarBehaviour
	{
		public GameObject gameObject;

		public virtual void OnInitialize() { }
		public virtual void OnEnter() { }
		public virtual void OnExit() { }
		public virtual void OnUpdate() { }
		public virtual void OnDraw(SpriteBatch sb) { }
	}
}
