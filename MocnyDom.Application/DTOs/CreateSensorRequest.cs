namespace MocnyDom.Application.DTOs
{
    public class CreateSensorRequest
    {
        public string Name { get; set; }
        public string Type { get; set; } // string -> parsed to SensorType
    }
}
