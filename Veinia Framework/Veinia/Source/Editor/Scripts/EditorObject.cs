using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace VeiniaFramework.Editor
{
	public class EditorObject
	{
		[JsonProperty("n")] public string PrefabName { get; set; }
		[JsonProperty("p")] public Vector2 Position { get { return position; } set { position = value; if (EditorPlacedSprite != null) EditorPlacedSprite.transform.position = value; } }
		Vector2 position;
		[JsonProperty("r")] public float Rotation { get { return rotation; } set { rotation = value; if (EditorPlacedSprite != null) EditorPlacedSprite.transform.rotation = -value; } }
		float rotation;
		[JsonIgnore] public Sprite EditorPlacedSprite { get; set; }
		[JsonIgnore] public IDrawGizmos gizmo { get; set; }
	}
}
