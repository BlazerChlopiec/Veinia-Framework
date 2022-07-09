using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Veinia.Source
{
	public class MouseDrag : Component
	{
		private bool drag;

		private Vector2 startMousePos;


		public override void Update()
		{
			if (Globals.input.GetMouseButtonDown(0) && Globals.input.GetKey(Keys.LeftAlt))
			{
				startMousePos = Globals.input.GetMouseWorldPosition();
				drag = true;
			}
			if (Globals.input.GetMouseButtonUp(0) && Globals.input.GetKey(Keys.LeftAlt)) { drag = false; }

			Globals.camera.ZoomIn(Globals.input.deltaScroll);
			Globals.camera.Zoom = MathHelper.Clamp(Globals.camera.Zoom, .1f, 1);

			if (drag) { Globals.camera.SetPosition(startMousePos - (Globals.input.GetMouseWorldPosition() - Globals.camera.GetPosition())); }
		}
	}
}
