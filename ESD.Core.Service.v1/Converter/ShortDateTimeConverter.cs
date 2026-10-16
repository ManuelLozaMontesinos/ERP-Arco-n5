using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ESD.Core.Service.Converter
{
   public class ShortDateTimeConverter : JsonConverter<DateTime>
	{
		public override DateTime Read(
			 ref Utf8JsonReader reader,
			 Type typeToConvert,
			 JsonSerializerOptions options) =>
				  DateTime.Parse(reader.GetString(), CultureInfo.InvariantCulture);

		public override void Write(
			 Utf8JsonWriter writer,
			 DateTime value,
			 JsonSerializerOptions options) =>
				  writer.WriteStringValue(value.ToShortDateString());
	}
}
