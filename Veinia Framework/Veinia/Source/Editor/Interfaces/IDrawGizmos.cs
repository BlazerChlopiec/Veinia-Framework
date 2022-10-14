using Microsoft.Xna.Framework.Graphics;

namespace Veinia.Editor
{
	public interface IDrawGizmos
	{
		public void DrawGizmos(SpriteBatch sb, EditorObject editorObject);
	}
}
