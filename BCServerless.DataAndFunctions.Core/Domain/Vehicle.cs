using Newtonsoft.Json;

namespace BCServerlessDemo.DataAndFunctions.Core.Domain
{
    public class Vehicle : BaseModel
    {
        [JsonProperty(PropertyName = "symboling")] public string Symboling { get; set; }
        [JsonProperty(PropertyName = "normalizedLosses")] public string NormalizedLosses { get; set; }
        [JsonProperty(PropertyName = "make")] public string Make { get; set; }
        [JsonProperty(PropertyName = "fuelType")] public string FuelType { get; set; }
        [JsonProperty(PropertyName = "aspiration")] public string Aspiration { get; set; }
        [JsonProperty(PropertyName = "numOfDoors")] public string NumOfDoors { get; set; }
        [JsonProperty(PropertyName = "bodyStyle")] public string BodyStyle { get; set; }
        [JsonProperty(PropertyName = "driveWheels")] public string DriveWheels { get; set; }
        [JsonProperty(PropertyName = "engineLocation")] public string EngineLocation { get; set; }
        [JsonProperty(PropertyName = "wheelBase")] public string WheelBase { get; set; }
        [JsonProperty(PropertyName = "length")] public string Length { get; set; }
        [JsonProperty(PropertyName = "width")] public string Width { get; set; }
        [JsonProperty(PropertyName = "height")] public string Height { get; set; }
        [JsonProperty(PropertyName = "curbWeight")] public string CurbWeight { get; set; }
        [JsonProperty(PropertyName = "engineType")] public string EngineType { get; set; }
        [JsonProperty(PropertyName = "numOfCylinders")] public string NumOfCylinders { get; set; }
        [JsonProperty(PropertyName = "engineSize")] public string EngineSize { get; set; }
        [JsonProperty(PropertyName = "fuelSystem")] public string FuelSystem { get; set; }
        [JsonProperty(PropertyName = "bore")] public string Bore { get; set; }
        [JsonProperty(PropertyName = "stroke")] public string Stroke { get; set; }
        [JsonProperty(PropertyName = "compressionRatio")] public string CompressionRatio { get; set; }
        [JsonProperty(PropertyName = "horsePower")] public string Horsepower { get; set; }
        [JsonProperty(PropertyName = "peakRPM")] public string PeakRPM { get; set; }
        [JsonProperty(PropertyName = "cityMPG")] public string CityMPG { get; set; }
        [JsonProperty(PropertyName = "highwayMPG")] public string HighwayMPG { get; set; }
        [JsonProperty(PropertyName = "price")] public string Price { get; set; }

    }
}
