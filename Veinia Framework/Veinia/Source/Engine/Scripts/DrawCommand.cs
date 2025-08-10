using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VeiniaFramework
{
	public struct DrawCommand
	{
		public Texture2D Texture;
		public Rectangle Destination;
		public Rectangle? Source;
		public Color Color;
		public float Rotation;
		public Vector2 Origin;
		public SpriteEffects Effects;
		public float Z; // drawing order that works with multiple Begins()
		public Effect shader;
	}
}