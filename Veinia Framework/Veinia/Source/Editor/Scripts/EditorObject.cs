using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System.ComponentModel;

namespace VeiniaFramework.Editor
{
	public class EditorObject
	{
		[JsonProperty("n")] public string PrefabName { get; set; }
		[JsonProperty("p")] public Vector2 Position { get { return position; } set { position = value; if (EditorPlacedSprite != null) EditorPlacedSprite.transform.position = value; } }
		Vector2 position;
		[JsonProperty("r", DefaultValueHandling = DefaultValueHandling.Ignore)] public float Rotation { get { return rotation; } set { rotation = value; if (EditorPlacedSprite != null) EditorPlacedSprite.transform.rotation = value; } }
		float rotation;
		[JsonProperty("d", DefaultValueHandling = DefaultValueHandling.Ignore)] public string customData;
		[Browsable(false)][JsonIgnore] public Sprite EditorPlacedSprite { get; set; }
		[Browsable(false)][JsonIgnore] public IDrawGizmos gizmo { get; set; }
	}
}
