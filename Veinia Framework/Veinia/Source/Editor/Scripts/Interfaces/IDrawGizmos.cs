﻿using Microsoft.Xna.Framework.Graphics;

namespace VeiniaFramework.Editor
{
	public interface IDrawGizmos
	{
		public void DrawGizmos(SpriteBatch sb, EditorObject editorObject);
	}
}
