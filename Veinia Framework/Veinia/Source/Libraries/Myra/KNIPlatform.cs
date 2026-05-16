using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Myra.Graphics2D.UI;
using Myra.Platform;
using System.Collections.Generic;

public class KNIPlatform : IMyraPlatform
{
	private Game game;
	private KNIRenderer renderer;

	public KNIPlatform(Game game, KNIRenderer renderer)
	{
		this.game = game;
		this.renderer = renderer;
	}

	public System.Drawing.Point ViewSize => new System.Drawing.Point(
		game.GraphicsDevice.PresentationParameters.BackBufferWidth,
		game.GraphicsDevice.PresentationParameters.BackBufferHeight
	);

	public IMyraRenderer Renderer => renderer;

	public MouseInfo GetMouseInfo()
	{
		var state = Mouse.GetState();
		var info = new MouseInfo
		{
			Position = new System.Drawing.Point(state.X, state.Y),
			IsLeftButtonDown = state.LeftButton == ButtonState.Pressed,
			IsMiddleButtonDown = state.MiddleButton == ButtonState.Pressed,
			IsRightButtonDown = state.RightButton == ButtonState.Pressed,
			Wheel = state.ScrollWheelValue
		};
		return info;
	}

	public Myra.Platform.TouchCollection GetTouchState()
	{
		var state = TouchPanel.GetState();
		var touches = new List<Myra.Platform.TouchLocation>();

		foreach (var touch in state)
		{
			var pos = new System.Numerics.Vector2(touch.Position.X, touch.Position.Y);
			touches.Add(new Myra.Platform.TouchLocation { Position = pos });
		}
		var collection = new Myra.Platform.TouchCollection
		{
			IsConnected = state.IsConnected,
			Touches = touches
		};
		return collection;
	}

	public void SetKeysDown(bool[] keys)
	{
		if (keys == null) return;

		var state = Keyboard.GetState();
		for (int i = 0; i < keys.Length; i++)
		{
			keys[i] = state.IsKeyDown((Microsoft.Xna.Framework.Input.Keys)i);
		}
	}

	public void SetMouseCursorType(MouseCursorType mouseCursorType)
	{
		switch (mouseCursorType)
		{
			case MouseCursorType.IBeam:
				Mouse.SetCursor(MouseCursor.IBeam);
				break;
			case MouseCursorType.Hand:
				Mouse.SetCursor(MouseCursor.Hand);
				break;
			case MouseCursorType.Wait:
				Mouse.SetCursor(MouseCursor.Wait);
				break;
			case MouseCursorType.Arrow:
			default:
				Mouse.SetCursor(MouseCursor.Arrow);
				break;
		}
	}
}