using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace VeiniaFramework.Editor
{
	public class SceneFile
	{
		public List<EditorObject> objects;

		[JsonProperty("l", DefaultValueHandling = DefaultValueHandling.Ignore)] // as in location
		public Vector2? editorCamPosition;
		[JsonProperty("z", DefaultValueHandling = DefaultValueHandling.Ignore)] // as in zoom
		public float? editorCamScale;
	}
}
