using HUDRealTimeClock.Configuration;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;

[assembly: ModInfo("HUD Real Time Clock",
  Authors = new[] { "Craluminum2413" })]

namespace HUDRealTimeClock
{
  class HUDRealTimeClock : ModSystem
  {
    private long id;

    public override void StartClientSide(ICoreClientAPI capi)
    {
      ModConfig.ReadConfig(capi);
      var realTimeClock = new HudElementRealTimeClock(capi);
      capi.Input.RegisterHotKey("realtimeclock", Lang.Get("realtimeclock:Show/Hide 'Real Time Clock'"), GlKeys.O, HotkeyType.HelpAndOverlays);
      id = capi.Event.RegisterGameTickListener(delegate
      {
        if (capi.Settings.Bool["realTimeClockGui"]) realTimeClock.TryOpen();
        capi.Event.UnregisterGameTickListener(id);
      }, 1000);
      capi.Input.SetHotKeyHandler("realtimeclock", delegate
      {
        realTimeClock.Toggle();
        return true;
      });
    }
  }
}