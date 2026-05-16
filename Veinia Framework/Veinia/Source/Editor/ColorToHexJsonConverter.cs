using FontStashSharp;
using FontStashSharp.RichText;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;

namespace VeiniaFramework.Editor
{
	public class ColorToHexJsonConverter : JsonConverter<Color>
	{
		public override void WriteJson(JsonWriter writer, Color value, JsonSerializer serializer)
		{
			var fscol = new FSColor(value.R, value.G, value.B);
			writer.WriteValue(fscol.ToHexString());
		}

		public override Color ReadJson(JsonReader reader, Type objectType, Color existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			var color = (string)reader.Value;
			var fscol = ColorStorage.FromName(color).Value;
			return new Color(fscol.R, fscol.G, fscol.B);
		}
	}
}