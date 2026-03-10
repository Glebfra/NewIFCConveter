using Newtonsoft.Json;
using Start.API;
using Start.Attributes;
using Start.Converters;
using Start.Interfaces;
using Start.StartProperties;

namespace Start.Entities.Fittings
{
    [StartElement(StartElementTypeEnum.VALVE)]
    public class StartValveEntity : StartAbstractFittingEntity, 
        IStartClippingEntity
    {
        [JsonProperty(StartPropertyName.Diameter)]
        [JsonConverter(typeof(JsonStartConverter<LengthValueProperty<double>>))]
        public LengthValueProperty<double> OutsideDiameter { get; set; } = new LengthValueProperty<double>();

        [JsonProperty(StartPropertyName.Length)]
        [JsonConverter(typeof(JsonStartConverter<LengthValueProperty<double>>))]
        public LengthValueProperty<double> Length { get; set; } = new LengthValueProperty<double>();

        [JsonProperty(StartPropertyName.LeakageCheck)]
        [JsonConverter(typeof(JsonStartConverter<EnumProperty<StartLeakageCheckEnum>>))]
        public EnumProperty<StartLeakageCheckEnum> LeakageCheckEnum { get; set; } =
            new EnumProperty<StartLeakageCheckEnum>();

        [JsonProperty(StartPropertyName.GasketEffectiveDiameter)]
        [JsonConverter(typeof(JsonStartConverter<LengthValueProperty<double>>))]
        public LengthValueProperty<double> GasketEffectiveDiameter { get; set; } = new LengthValueProperty<double>();

        [JsonProperty(StartPropertyName.NominalPressure)]
        [JsonConverter(typeof(JsonStartConverter<PressureValueProperty<double>>))]
        public PressureValueProperty<double> NominalPressure { get; set; } = new PressureValueProperty<double>();

        public void ClipEntity(IStartClippableEntity clippable)
        {
            clippable.Clip(Position, Length.SIProperty / 2);
        }
    }
}