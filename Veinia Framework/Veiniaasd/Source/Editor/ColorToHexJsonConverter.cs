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
			writer.WriteValue(value.ToHexString());
		}

		public override Color ReadJson(JsonReader reader, Type objectType, Color existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			var color = (string)reader.Value;

			return ColorStorage.FromName(color).Value;
		}
	}
}