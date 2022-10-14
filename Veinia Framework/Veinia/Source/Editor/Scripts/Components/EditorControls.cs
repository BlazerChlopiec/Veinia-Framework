using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Veinia.Editor
{
	public class EditorControls : Component
	{
		private bool drag;

		private Vector2 startMousePos;


		public override void Update()
		{
			if (Globals.myraDesktop.IsMouseOverGUI && drag == false && Globals.input.GetMouseButtonDown(0)) { return; }

			if (Globals.input.GetMouseButtonDown(0) && Globals.input.GetKey(Keys.LeftAlt))
			{
				startMousePos = Globals.input.GetMouseWorldPosition();
				drag = true;
			}

			if (!Globals.myraDesktop.IsMouseOverGUI)
			{
				Globals.camera.ZoomIn(Globals.input.deltaScroll);
				Globals.camera.Zoom = MathHelper.Clamp(Globals.camera.Zoom, .38f, 1.15f);
			}

			if (drag) { Globals.camera.SetPosition(startMousePos - (Globals.input.GetMouseWorldPosition() - Globals.camera.GetPosition())); }

			if (Globals.input.GetMouseButtonUp(0)) { drag = false; }
		}
	}
}
