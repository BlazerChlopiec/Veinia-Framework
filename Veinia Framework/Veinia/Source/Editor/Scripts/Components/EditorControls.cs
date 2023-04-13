using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Myra.Graphics2D.UI;

namespace VeiniaFramework.Editor
{
	public class EditorControls : Component
	{
		public bool isDragging { get; private set; }

		private Vector2 startMousePos;

		private float zoomSensitivity = 1.2f;


		public override void Update()
		{
			EditorLabelManager.Add("MousePosition", new Label { Text = "Mouse Position - " + Globals.input.GetMouseWorldPosition().Round(3), VerticalAlignment = VerticalAlignment.Bottom, HorizontalAlignment = HorizontalAlignment.Right });
			EditorLabelManager.Add("CameraPosition", new Label { Text = "Camera Position - " + Globals.camera.GetPosition().Round(3), Top = -25, VerticalAlignment = VerticalAlignment.Bottom, HorizontalAlignment = HorizontalAlignment.Right });

			if (Globals.myraDesktop.IsMouseOverGUI && isDragging == false && Globals.input.GetMouseButtonDown(0)) { return; }

			if (Globals.input.GetMouseButtonDown(0) && Globals.input.GetKey(Keys.LeftAlt))
			{
				startMousePos = Globals.input.GetMouseWorldPosition();
				isDragging = true;
			}

			if (!Globals.myraDesktop.IsMouseOverGUI)
			{
				Globals.camera.ZoomIn(Globals.input.deltaScroll * zoomSensitivity);
				Globals.camera.Zoom = MathHelper.Clamp(Globals.camera.Zoom, .28f, 1.7f);
			}

			if (isDragging) { Globals.camera.SetPosition(startMousePos - (Globals.input.GetMouseWorldPosition() - Globals.camera.GetPosition())); }

			if (Globals.input.GetMouseButtonUp(0))
			{
				// we release drag in next frame to not accidentally paint an object on drag release
				NextFrame.actions.Add(() => { isDragging = false; });
			}
		}
	}
}
