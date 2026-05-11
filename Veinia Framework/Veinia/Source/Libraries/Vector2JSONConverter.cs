using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Globalization;

public class Vector2JSONConverter : JsonConverter
{
	public override void WriteJson(
		JsonWriter writer,
		object value,
		JsonSerializer serializer)
	{
		Vector2 vector = (Vector2)value;

		writer.WriteStartObject();

		writer.WritePropertyName("x");
		writer.WriteValue(vector.X);

		writer.WritePropertyName("y");
		writer.WriteValue(vector.Y);

		writer.WriteEndObject();
	}

	public override object ReadJson(
		JsonReader reader,
		Type objectType,
		object existingValue,
		JsonSerializer serializer)
	{
		// OBJECT FORMAT
		if (reader.TokenType == JsonToken.StartObject)
		{
			JObject obj = JObject.Load(reader);

			return new Vector2(
				obj["x"]!.Value<float>(),
				obj["y"]!.Value<float>()
			);
		}

		// STRING FORMAT
		if (reader.TokenType == JsonToken.String)
		{
			string s = ((string)reader.Value).Trim();

			string[] parts = s.Split(' ');

			if (parts.Length == 2)
			{
				return new Vector2(
					float.Parse(parts[0], CultureInfo.InvariantCulture),
					float.Parse(parts[1], CultureInfo.InvariantCulture)
				);
			}

			if (parts.Length == 1)
			{
				float v = float.Parse(parts[0], CultureInfo.InvariantCulture);

				return new Vector2(v);
			}
		}

		throw new InvalidOperationException("Invalid Vector2");
	}

	public override bool CanConvert(Type objectType)
	{
		return objectType == typeof(Vector2);
	}
}