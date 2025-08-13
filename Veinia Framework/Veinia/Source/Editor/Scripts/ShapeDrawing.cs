using Microsoft.Xna.Framework.Graphics;

namespace VeiniaFramework
{
	public class ShapeDrawing
	{
		GraphicsDevice graphicsDevice;
		BasicEffect basicEffect;


		public ShapeDrawing(GraphicsDevice device)
		{
			graphicsDevice = device;
			basicEffect = new BasicEffect(device)
			{
				VertexColorEnabled = true,
			};
		}

		public void UpdateBasicEffect()
		{
			basicEffect.Projection = Globals.camera.GetProjection();
			basicEffect.View = Globals.camera.GetView();
		}

		public void Shape(Level level, VertexPositionColor[] vertices, Effect effect = null, float z = 0f)
		{
			if (effect == null) effect = basicEffect;

			level.drawCommands.Add(new DrawCommand
			{
				command = delegate
				{
					foreach (var pass in effect.CurrentTechnique.Passes)
					{
						pass.Apply();
						graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, vertices, 0, vertices.Length / 3);
					}
				},
				drawWithoutSpriteBatch = true,
				Z = z
			});
		}
	}
}