using sapo_clone.Common.Attributes;

namespace sapo_clone.API.Settings
{
    [ApplicationSetting]
    public class CorsSetting
    {
        public CorsPolicy[] Policies { get; init; } = new CorsPolicy[] { };
    }

    public class CorsPolicy
    {
        public required string Name { get; init; }
        public string[] AllowedOrigins { get; init; } = Array.Empty<string>();
        public string[] AllowedMethods { get; init; } = Array.Empty<string>();
        public string[] AllowedHeaders { get; init; } = Array.Empty<string>();
        public bool AllowCredentials { get; init; }
    }
}
