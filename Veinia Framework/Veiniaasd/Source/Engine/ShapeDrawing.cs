using LibTessDotNet;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace VeiniaFramework
{
	public class ShapeDrawing
	{
		GraphicsDevice graphicsDevice;
		RasterizerState rasterizer;
		public BasicEffect basicEffect;


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

		/// <summary>
		/// Draws a shape out of specified triangles (no automatic triangulation)
		/// </summary>
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

		/// <summary>
		/// Auto-triangulates a shape from given outlineVertices
		/// </summary>
		public void ShapeTriangulated(Level level, Vector2[] outlineVertices, Transform transform = null, Color? color = null, Effect effect = null, float z = 0f, bool setWorldViewProjection = false)
		{
			var triangles = Triangulate(outlineVertices);

			var vertexPositions = new List<Vector2>();
			foreach (var t in triangles)
			{
				vertexPositions.Add(t.A);
				vertexPositions.Add(t.B);
				vertexPositions.Add(t.C);
			}

			Globals.shapeDrawing.ShapeVec2(level, vertexPositions.ToArray(), transform, color, effect, z, setWorldViewProjection);
		}


		public List<(Vector2 A, Vector2 B, Vector2 C)> Triangulate(Vector2[] points)
		{
			var tess = new Tess();

			var contour = new ContourVertex[points.Length];
			for (int i = 0; i < points.Length; i++)
				contour[i].Position = new Vec3(points[i].X, points[i].Y, 0);

			tess.AddContour(contour);

			tess.Tessellate(WindingRule.NonZero, ElementType.Polygons, 3);

			var triangles = new List<(Vector2, Vector2, Vector2)>();
			for (int i = 0; i < tess.ElementCount; i++)

			{
				int i0 = tess.Elements[i * 3 + 0];
				int i1 = tess.Elements[i * 3 + 1];
				int i2 = tess.Elements[i * 3 + 2];
				if (i0 == -1 || i1 == -1 || i2 == -1) continue;

				var v0 = new Vector2(tess.Vertices[i0].Position.X, tess.Vertices[i0].Position.Y);
				var v1 = new Vector2(tess.Vertices[i1].Position.X, tess.Vertices[i1].Position.Y);
				var v2 = new Vector2(tess.Vertices[i2].Position.X, tess.Vertices[i2].Position.Y);
				triangles.Add((v0, v1, v2));
			}

			return triangles;
		}
	}
}