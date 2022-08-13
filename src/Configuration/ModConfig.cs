using Vintagestory.API.Client;

namespace HUDRealTimeClock.Configuration
{
  static class ModConfig
  {
    private const string jsonConfig = "RealTimeClockConfig.json";
    private static RealTimeClockConfig config;

    public static void ReadConfig(ICoreClientAPI api)
    {
      try
      {
        config = LoadConfig(api);

        if (config == null)
        {
          GenerateConfig(api);
          config = LoadConfig(api);
        }
        else
        {
          GenerateConfig(api, config);
        }
      }
      catch
      {
        GenerateConfig(api);
        config = LoadConfig(api);
      }

      api.World.Config.SetDouble("realtimeclock_strokewidth", config.StrokeWidth);
      api.World.Config.SetInt("realtimeclock_fontsize", config.FontSize);
      api.World.Config.SetString("realtimeclock_fontname", config.FontName);
      api.World.Config.SetString("realtimeclock_fontslant", config.FontSlant.ToString());
      api.World.Config.SetString("realtimeclock_fontweight", config.FontWeight.ToString());
      api.World.Config.SetString("realtimeclock_format", config.Format);
      api.World.Config.SetString("realtimeclock_hexcolor", config.HexColor);
      api.World.Config.SetString("realtimeclock_position", config.Position.ToString());
      api.World.Config.SetString("realtimeclock_strokehexcolor", config.StrokeHexColor);
    }

    private static RealTimeClockConfig LoadConfig(ICoreClientAPI api) =>
      api.LoadModConfig<RealTimeClockConfig>(jsonConfig);

    private static void GenerateConfig(ICoreClientAPI api) =>
      api.StoreModConfig(new RealTimeClockConfig(), jsonConfig);

    private static void GenerateConfig(ICoreClientAPI api, RealTimeClockConfig previousConfig) =>
      api.StoreModConfig(new RealTimeClockConfig(previousConfig), jsonConfig);
  }
}