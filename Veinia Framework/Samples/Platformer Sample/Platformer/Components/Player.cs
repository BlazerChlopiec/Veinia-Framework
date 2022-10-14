using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;
using Veinia.Editor;

namespace Veinia.Platformer
{
	public class Player : Component, IDrawGizmos
	{
		private const float speed = 10;
		private const float jumpForce = 13;

		Physics physics;


		public override void Initialize() => physics = GetComponent<Physics>();

		float smoothZoom;
		public override void Update()
		{
			physics.velocity.X = Globals.input.horizontal * speed;

			if (Globals.input.GetKeyButtonDown(Keys.Space, Buttons.A) || Globals.input.GetKeyDown(Keys.W))
			{
				physics.velocity.Y = jumpForce;
			}
			if ((Globals.input.GetKeyButtonUp(Keys.Space, Buttons.A) || Globals.input.GetKeyUp(Keys.W)) && physics.velocity.Y > 0)
			{
				physics.velocity.Y *= .7f;
			}

			Globals.camera.LerpTo(transform.position, 10);

			smoothZoom = MathHelper.Lerp(smoothZoom, (MathF.Abs(Globals.input.horizontal) / 10), 1.5f * Time.deltaTime);
			Globals.camera.Zoom = 1 - smoothZoom;
		}

		public void DrawGizmos(SpriteBatch sb, EditorObject editorObject)
		{
			sb.DrawRectangle(editorObject.EditorPlacedSprite.rect.OffsetByHalf(), Color.Blue, 10, 1);
		}
	}
}