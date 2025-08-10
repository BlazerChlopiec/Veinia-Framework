using Microsoft.Xna.Framework.Graphics;
using System;

namespace VeiniaFramework
{
	public struct DrawCommand
	{
		public Action command;
		public float Z; // drawing order that works with multiple Begins()
		public Effect shader;
	}
}