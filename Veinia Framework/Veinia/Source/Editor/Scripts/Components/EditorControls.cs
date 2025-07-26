using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Myra.Graphics2D.UI;

namespace VeiniaFramework.Editor
{
	public class EditorControls : Component
	{
		public bool isDragging { get; private set; }
		private bool isHolding;

		private Vector2 startMousePos;

		private float zoomSensitivity = 1.2f;

		private const float DRAG_THRESHOLD = .3f; // world space

		public static bool disableDragMove;
		public static bool isTextBoxFocused => Globals.myraDesktop.FocusedKeyboardWidget is TextBox;

		public static bool isMouseOverGUIPreviousFrame;
		static bool isMouseOverGUIPreviousFrameOld;


		public override void Update()
		{
			EditorLabelManager.Add("MousePosition", new Label { Text = "Mouse Position - " + Globals.input.GetMouseWorldPosition().Round(3), VerticalAlignment = VerticalAlignment.Bottom, HorizontalAlignment = HorizontalAlignment.Right });
			EditorLabelManager.Add("CameraPosition", new Label { Text = "Camera Position - " + Globals.camera.GetPosition().Round(3), Top = -25, VerticalAlignment = VerticalAlignment.Bottom, HorizontalAlignment = HorizontalAlignment.Right }); ;

			isMouseOverGUIPreviousFrame = isMouseOverGUIPreviousFrameOld;
			isMouseOverGUIPreviousFrameOld = Globals.myraDesktop.IsMouseOverGUI;

			if (Globals.myraDesktop.IsMouseOverGUI && isDragging == false && Globals.input.GetMouseDown(0)) { return; }

			if (Globals.input.GetMouseDown(0))
			{
				startMousePos = Globals.input.GetMouseWorldPosition();
				if (!Globals.input.GetKey(Keys.LeftShift)) isHolding = true;
			}

			if (!Globals.myraDesktop.IsMouseOverGUI)
			{
				Globals.camera.Scale += Vector2.One * (Globals.input.deltaScroll * zoomSensitivity);
				Globals.camera.Scale = Vector2.Clamp(Globals.camera.Scale, Vector2.One * .28f, Vector2.One * 1.7f);
			}

			if (isHolding)
			{
				if (Vector2.Distance(startMousePos, Globals.input.GetMouseWorldPosition()) > DRAG_THRESHOLD)
				{
					if (!isDragging) startMousePos = Globals.input.GetMouseWorldPosition(); // reassing startMousePos to prevent this weird jump when crossing over threshold
					isDragging = true;
				}
			}

			if (isDragging && !disableDragMove) { Globals.camera.SetPosition(startMousePos - (Globals.input.GetMouseWorldPosition() - Globals.camera.GetPosition())); }

			if (Globals.input.GetMouseUp(0))
			{
				isHolding = false;

				// we release drag in next frame to not accidentally paint an object on drag release
				NextFrame.actions.Add(() => { isDragging = false; });
			}
		}
	}
}
