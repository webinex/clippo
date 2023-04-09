using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Webinex.Clippo.AspNetCore.Controllers.Json
{
    internal class ClippoActionJsonConverter : JsonConverter<IClippoAction>
    {
        private static readonly string KIND_PROPERTY_NAME = "kind";

        private readonly IClippoAspNetCoreJsonSettings _settings;

        public ClippoActionJsonConverter(IClippoAspNetCoreJsonSettings settings)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        public override IClippoAction Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
                return null;

            var readerAtStart = reader;

            if (reader.TokenType != JsonTokenType.StartObject)
                throw new JsonException($"`{nameof(IClippoAction)}` might be object.");

            using var jsonDocument = JsonDocument.ParseValue(ref reader);
            var jsonObject = jsonDocument.RootElement;
            var actionType = ResolveActionType(jsonObject);

            return (IClippoAction) JsonSerializer.Deserialize(ref readerAtStart, actionType, options);
        }

        private Type ResolveActionType(JsonElement jsonObject)
        {
            var kindValue = jsonObject.EnumerateObject().FirstOrDefault(x =>
                    x.Name.Equals(KIND_PROPERTY_NAME, StringComparison.InvariantCultureIgnoreCase))
                .Value;

            if (kindValue.ValueKind != JsonValueKind.String)
                throw new JsonException($"{nameof(IClippoAction)}.{KIND_PROPERTY_NAME} might be string.");

            var kind = kindValue.GetString();
            return _settings.Actions[kind!];
        }

        public override void Write(Utf8JsonWriter writer, IClippoAction value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }
    }
}