using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Veinia
{
	public sealed class Input
	{
		KeyboardState keyboard;
		KeyboardState oldKeyboard;

		GamePadState gamepad;
		GamePadState oldGamepad;

		MouseState mouse;
		MouseState oldMouse;

		GamePadThumbSticks sticks;

		public enum Device
		{
			noDevice, // used for not making x360 the default | its only assigned in the start 
			x360,
			keyboard,
		}
		public Device currentDevice = Device.noDevice;

		public bool normalizeControllerAxis = false;
		public bool normalizeKeyboardAxis = false;

		float deadzone = 0.2f;

		public float horizontal { get; private set; }
		public float vertical { get; private set; }

		public float deltaScroll { get; private set; }
		float currentScroll;


		public void Update()
		{
			Title.Add(currentDevice, 4);

			//assign states
			if (keyboard != null)
				oldKeyboard = keyboard;
			keyboard = Keyboard.GetState();

			if (gamepad != null)
				oldGamepad = gamepad;
			gamepad = GamePad.GetState(0); //first player

			if (mouse != null)
				oldMouse = mouse;
			mouse = Mouse.GetState();

			sticks = gamepad.ThumbSticks;
			//

			//scroll delta
			deltaScroll = mouse.ScrollWheelValue - currentScroll;
			currentScroll += deltaScroll;
			deltaScroll /= 2000;

			//detect which device are you using
			if (GetPressedButton(gamepad) != 0 && keyboard.GetPressedKeys().Length < 1) { currentDevice = Device.x360; }
			if (keyboard.GetPressedKeys().Length >= 1 && GetPressedButton(gamepad) == 0) { currentDevice = Device.keyboard; }

			//Debug.WriteLine(currentDevice);

			MakeAxis(); // create horizontal & vertical
		}

		private void MakeAxis()
		{
			//keyboard
			if (currentDevice == Device.keyboard)
			{
				horizontal = 0; // reset to zero each frame 
				if (GetKey(Keys.A) || GetKey(Keys.Left)) { horizontal -= 1; }
				if (GetKey(Keys.D) || GetKey(Keys.Right)) { horizontal += 1; }

				vertical = 0; // reset to zero each frame 
				if (GetKey(Keys.W) || GetKey(Keys.Up)) { vertical += 1; }
				if (GetKey(Keys.S) || GetKey(Keys.Down)) { vertical -= 1; }
			}
			//

			//controller
			if (currentDevice == Device.x360)
			{
				if (sticks.Left.Length() > deadzone) // if the magnitude is more than deadzone assign horizontal & vertical
				{
					horizontal = sticks.Left.X;
					vertical = sticks.Left.Y;
				}
				else { horizontal = 0; vertical = 0; }
			}
			//

			Vector2 axisVector = new Vector2(horizontal, vertical);

			if (currentDevice == Device.keyboard && normalizeKeyboardAxis || normalizeControllerAxis) // always normalize the input when its a keyboard
			{                                                                                         // or when 'normalizeControllerAxis' is true
				Utils.SafeNormalize(axisVector);
			}


			horizontal = axisVector.X;
			vertical = axisVector.Y;
		}

		public void SetDeadzone(float value) => deadzone = value;

		public Buttons GetPressedButton(GamePadState gamepad)
		{
			GamePadButtons buttons;
			buttons = gamepad.Buttons;

			if (buttons.Start == ButtonState.Pressed) { return Buttons.Start; }
			if (buttons.Back == ButtonState.Pressed) { return Buttons.Back; }
			if (buttons.A == ButtonState.Pressed) { return Buttons.A; }
			if (buttons.B == ButtonState.Pressed) { return Buttons.B; }
			if (buttons.X == ButtonState.Pressed) { return Buttons.X; }
			if (buttons.Y == ButtonState.Pressed) { return Buttons.Y; }
			if (buttons.RightShoulder == ButtonState.Pressed) { return Buttons.RightShoulder; }
			if (buttons.LeftShoulder == ButtonState.Pressed) { return Buttons.LeftShoulder; }
			if (buttons.RightStick == ButtonState.Pressed) { return Buttons.RightStick; }
			if (buttons.LeftStick == ButtonState.Pressed) { return Buttons.LeftStick; }

			if (gamepad.DPad.Down == ButtonState.Pressed) { return Buttons.DPadDown; }
			if (gamepad.DPad.Left == ButtonState.Pressed) { return Buttons.DPadLeft; }
			if (gamepad.DPad.Right == ButtonState.Pressed) { return Buttons.DPadRight; }
			if (gamepad.DPad.Up == ButtonState.Pressed) { return Buttons.DPadUp; }
			if (gamepad.ThumbSticks.Left.Length() >= deadzone) { return Buttons.LeftStick; }
			if (gamepad.ThumbSticks.Right.Length() >= deadzone) { return Buttons.RightStick; }
			if (gamepad.IsButtonDown(Buttons.LeftTrigger)) { return Buttons.LeftTrigger; }
			if (gamepad.IsButtonDown(Buttons.RightTrigger)) { return Buttons.RightTrigger; }

			//return new Buttons(); // return 0 when nothing is pressed
			return default; // return 0 when nothing is pressed
		}


		#region Get input methods

		public bool GetKeyButton(Keys key, Buttons button)
		{
			return (keyboard.IsKeyDown(key) || gamepad.IsButtonDown(button));
		}
		public bool GetKey(Keys key)
		{
			return (keyboard.IsKeyDown(key));
		}
		public bool GetButton(Buttons button)
		{
			return (gamepad.IsButtonDown(button));
		}
		public bool GetMouseButton(int buttonIndex)
		{
			if (buttonIndex == 0)
				return mouse.LeftButton == ButtonState.Pressed;
			if (buttonIndex == 1)
				return mouse.RightButton == ButtonState.Pressed;
			else { return false; }
		}
		public bool GetMouseGamepadButton(int buttonIndex, Buttons button)
		{
			if (buttonIndex == 0)
				return mouse.LeftButton == ButtonState.Pressed || gamepad.IsButtonDown(button);
			if (buttonIndex == 1)
				return mouse.RightButton == ButtonState.Pressed || gamepad.IsButtonDown(button);
			else { return false; }
		}

		public bool GetKeyButtonDown(Keys key, Buttons button)
		{
			return keyboard.IsKeyDown(key) && !oldKeyboard.IsKeyDown(key) || gamepad.IsButtonDown(button) && !oldGamepad.IsButtonDown(button);
		}
		public bool GetKeyDown(Keys key)
		{
			return keyboard.IsKeyDown(key) && !oldKeyboard.IsKeyDown(key);
		}
		public bool GetButtonDown(Buttons button)
		{
			return gamepad.IsButtonDown(button) && !oldGamepad.IsButtonDown(button);
		}
		public bool GetMouseButtonDown(int buttonIndex)
		{
			if (buttonIndex == 0)
				return mouse.LeftButton == ButtonState.Pressed && oldMouse.LeftButton != ButtonState.Pressed;
			if (buttonIndex == 1)
				return mouse.RightButton == ButtonState.Pressed && oldMouse.RightButton != ButtonState.Pressed;
			else { return false; }
		}
		public bool GetMouseGamepadButtonDown(int buttonIndex, Buttons button)
		{
			if (buttonIndex == 0)
				return mouse.LeftButton == ButtonState.Pressed && oldMouse.LeftButton != ButtonState.Pressed
										|| gamepad.IsButtonDown(button) && !oldGamepad.IsButtonDown(button);
			if (buttonIndex == 1)
				return mouse.RightButton == ButtonState.Pressed && oldMouse.RightButton != ButtonState.Pressed
										 || gamepad.IsButtonDown(button) && !oldGamepad.IsButtonDown(button);
			else { return false; }
		}

		public bool GetKeyButtonUp(Keys key, Buttons button)
		{
			return keyboard.IsKeyUp(key) && !oldKeyboard.IsKeyUp(key) || gamepad.IsButtonUp(button) && !oldGamepad.IsButtonUp(button);
		}
		public bool GetKeyUp(Keys key)
		{
			return keyboard.IsKeyUp(key) && !oldKeyboard.IsKeyUp(key);
		}
		public bool GetButtonUp(Buttons button)
		{
			return gamepad.IsButtonUp(button) && !oldGamepad.IsButtonUp(button);
		}
		public bool GetMouseButtonUp(int buttonIndex)
		{
			if (buttonIndex == 0)
				return mouse.LeftButton == ButtonState.Released && oldMouse.LeftButton != ButtonState.Released;
			if (buttonIndex == 1)
				return mouse.RightButton == ButtonState.Released && oldMouse.RightButton != ButtonState.Released;
			else { return false; }
		}
		public bool GetMouseGamepadButtonUp(int buttonIndex, Buttons button)
		{
			if (buttonIndex == 0)
				return mouse.LeftButton == ButtonState.Released && oldMouse.LeftButton != ButtonState.Released
										|| gamepad.IsButtonUp(button) && !oldGamepad.IsButtonUp(button);
			if (buttonIndex == 1)
				return mouse.RightButton == ButtonState.Released && oldMouse.RightButton != ButtonState.Released
										 || gamepad.IsButtonUp(button) && !oldGamepad.IsButtonUp(button);
			else { return false; }
		}

		#endregion

		public Vector2 GetMouseScreenPosition() => Globals.camera.ScreenToWorld(mouse.Position.ToVector2());
		public Vector2 GetMouseWorldPosition() => Transform.ScreenToWorldPos(Globals.camera.ScreenToWorld(mouse.Position.ToVector2()));
		public Vector2 GetMouseWorldPositionNonCameraRelative() => Transform.ScreenToWorldPos(mouse.Position.ToVector2());
	}
}