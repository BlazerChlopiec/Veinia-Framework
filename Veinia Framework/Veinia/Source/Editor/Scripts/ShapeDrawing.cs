using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VeiniaFramework
{
	public class ShapeDrawing
	{
		GraphicsDevice graphicsDevice;
		BasicEffect basicEffect;
		RasterizerState rasterizer;


		public ShapeDrawing(GraphicsDevice device, CullMode cullMode = CullMode.None)
		{
			rasterizer = new RasterizerState()
			{
				CullMode = cullMode
			};

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

		public void Shape(Level level, VertexPositionColor[] vertices, Transform transform = null, Effect effect = null, float z = 0f, bool setWorldViewProjection = false)
		{
			if (effect == null) effect = basicEffect;

			for (int i = 0; i < vertices.Length; i++)
			{
				var pos = vertices[i].Position;

				if (transform != null)
				{
					Vector2 scaled = new Vector2(pos.X * transform.scale.X,
												 pos.Y * transform.scale.Y);

					pos = (transform.position + scaled).ToVector3(z: pos.Z);
				}

				// convert to screen
				vertices[i].Position = Transform.WorldToScreenPos(pos.ToVector2()).ToVector3(z: pos.Z);
			}

			level.drawCommands.Add(new DrawCommand
			{
				command = delegate
				{
					if (setWorldViewProjection) effect.Parameters["WorldViewProjection"].SetValue(basicEffect.View * basicEffect.Projection);

					graphicsDevice.RasterizerState = rasterizer;
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

		public void ShapeVec2(Level level, Vector2[] vertices, Transform transform = null, Color? color = null, Effect effect = null, float z = 0f, bool setWorldViewProjection = false)
		{
			if (vertices == null || vertices.Length == 0)
				return;

			Color finalColor = color ?? Color.White;

			VertexPositionColor[] vertexData = new VertexPositionColor[vertices.Length];
			for (int i = 0; i < vertices.Length; i++)
			{
				vertexData[i] = new VertexPositionColor(
					vertices[i].ToVector3(),
					finalColor
				);
			}

			Shape(level, vertexData, transform, effect, z, setWorldViewProjection);
		}
	}
}