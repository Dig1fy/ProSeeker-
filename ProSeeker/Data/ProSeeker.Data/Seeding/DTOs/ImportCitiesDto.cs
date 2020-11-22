namespace ProSeeker.Data.Seeding.DTOs
{
    using Newtonsoft.Json;

    public class ImportCitiesDto
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
