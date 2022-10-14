using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace Veinia.Editor
{
	public class EditorObject
	{
		public string PrefabName { get; set; }
		public Vector2 Position { get; set; }
		[JsonIgnore] public Sprite EditorPlacedSprite { get; set; }
		[JsonIgnore] public IDrawGizmos gizmo { get; set; }
	}
}
