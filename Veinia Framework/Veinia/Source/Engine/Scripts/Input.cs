﻿using GeonBit.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace VeiniaFramework
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

		public float mouseX { get; private set; }
		public float mouseY { get; private set; }


		public void Update()
		{
			Title.Add(currentDevice, 2);

			//assign states
			oldKeyboard = keyboard;
			keyboard = Keyboard.GetState();

			oldGamepad = gamepad;
			gamepad = GamePad.GetState(0); //first player

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

			mouseX = (mouse.X - oldMouse.X);
			mouseY = -(mouse.Y - oldMouse.Y);

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
				axisVector.SafeNormalize();
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

		//keyboard
		public bool GetKey(Keys key)
		{
			return (keyboard.IsKeyDown(key));
		}
		public bool GetKeyDown(Keys key)
		{
			return keyboard.IsKeyDown(key) && !oldKeyboard.IsKeyDown(key);
		}
		public bool GetKeyUp(Keys key)
		{
			return keyboard.IsKeyUp(key) && !oldKeyboard.IsKeyUp(key);
		}
		//

		//gamepad
		public bool GetButtonUp(Buttons button)
		{
			return gamepad.IsButtonUp(button) && !oldGamepad.IsButtonUp(button);
		}
		public bool GetButton(Buttons button)
		{
			return (gamepad.IsButtonDown(button));
		}
		public bool GetButtonDown(Buttons button)
		{
			return gamepad.IsButtonDown(button) && !oldGamepad.IsButtonDown(button);
		}
		//

		//mouse
		public bool GetMouse(int buttonIndex)
		{
			if (buttonIndex == 0)
				return mouse.LeftButton == ButtonState.Pressed;
			if (buttonIndex == 1)
				return mouse.RightButton == ButtonState.Pressed;
			else { return false; }
		}
		public bool GetMouseDown(int buttonIndex)
		{
			if (UserInterface.Active.IsMouseInteracting) return false;

			if (buttonIndex == 0)
				return mouse.LeftButton == ButtonState.Pressed && oldMouse.LeftButton != ButtonState.Pressed;
			if (buttonIndex == 1)
				return mouse.RightButton == ButtonState.Pressed && oldMouse.RightButton != ButtonState.Pressed;
			else { return false; }
		}
		public bool GetMouseUp(int buttonIndex)
		{
			if (UserInterface.Active.IsMouseInteracting) return false;

			if (buttonIndex == 0)
				return mouse.LeftButton == ButtonState.Released && oldMouse.LeftButton != ButtonState.Released;
			if (buttonIndex == 1)
				return mouse.RightButton == ButtonState.Released && oldMouse.RightButton != ButtonState.Released;
			else { return false; }
		}
		//
		#endregion

		public Vector2 GetMouseScreenPosition() => Globals.camera.ScreenToWorld(mouse.Position.ToVector2());
		public Vector2 GetMouseWorldPosition() => Transform.ScreenToWorldPos(GetMouseScreenPosition());
	}
}