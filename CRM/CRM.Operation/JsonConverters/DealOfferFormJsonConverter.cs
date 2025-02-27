using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using CRM.Data.Enums;
using CRM.Operation.Helpers;
using CRM.Operation.Models.DealOfferModels;

namespace CRM.Operation.JsonConverters
{
    public class DealOfferFormJsonConverter : JsonConverter<IDealOfferFormModel>
    {
        public override bool CanConvert(Type type)
        {
            return typeof(IDealOfferFormModel).IsAssignableFrom(type);
        }

        public override IDealOfferFormModel Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            if (!reader.Read()
                || reader.TokenType != JsonTokenType.PropertyName
                || reader.GetString() != "TypeDiscriminator")
            {
                throw new JsonException();
            }

            if (!reader.Read() || reader.TokenType != JsonTokenType.Number)
            {
                throw new JsonException();
            }

            IDealOfferFormModel baseClass;
            OfferTypeEnum typeDiscriminator = (OfferTypeEnum) reader.GetInt32();
            if (!reader.Read() || reader.GetString() != "TypeValue")
            {
                throw new JsonException();
            }

            if (!reader.Read() || reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            var offerModelType = OfferTypeMappingHelper.OfferEnumToModelType[typeDiscriminator];

            if (offerModelType is null)
                throw new NotSupportedException();
            
            baseClass = (IDealOfferFormModel) JsonSerializer.Deserialize(ref reader, offerModelType);

            if (!reader.Read() || reader.TokenType != JsonTokenType.EndObject)
            {
                throw new JsonException();
            }

            return baseClass;
        }

        public override void Write(
            Utf8JsonWriter writer,
            IDealOfferFormModel value,
            JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            var offerType =
                OfferTypeMappingHelper.OfferEnumToModelType.FirstOrDefault(x => x.Value == value.GetType());
            
            writer.WriteNumber("TypeDiscriminator", (int) offerType.Key);
            writer.WritePropertyName("TypeValue");
            JsonSerializer.Serialize(writer, value as object);

            writer.WriteEndObject();
        }
    }
}