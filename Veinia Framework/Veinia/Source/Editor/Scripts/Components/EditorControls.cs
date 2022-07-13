using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Veinia.Editor
{
	public class EditorControls : Component
	{
		Toolbox toolbox;

		private bool drag;

		private Vector2 startMousePos;


		public override void Initialize()
		{
			toolbox = FindComponentOfType<Toolbox>();
		}

		public override void Update()
		{
			if (toolbox.hoveringOver) { drag = false; return; }

			if (Globals.input.GetMouseButtonDown(0) && Globals.input.GetKey(Keys.LeftAlt))
			{
				startMousePos = Globals.input.GetMouseWorldPosition();
				drag = true;
			}
			if (Globals.input.GetMouseButtonUp(0)) { drag = false; }

			Globals.camera.ZoomIn(Globals.input.deltaScroll);
			Globals.camera.Zoom = MathHelper.Clamp(Globals.camera.Zoom, .35f, 1.2f);

			if (drag) { Globals.camera.SetPosition(startMousePos - (Globals.input.GetMouseWorldPosition() - Globals.camera.GetPosition())); }
		}
	}
}
