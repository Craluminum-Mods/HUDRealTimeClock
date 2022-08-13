using System;
using Vintagestory.API.Client;
using Vintagestory.API.MathTools;
using Cairo;
using Newtonsoft.Json;

namespace HUDRealTimeClock
{
  internal static class ConversionExtensions
  {
    public static T ToEnum<T>(this string source) => JsonConvert.DeserializeObject<T>($"\"{source}\"");
  }

  internal class HudElementRealTimeClock : HudElement
  {
    private long id;
    public override string ToggleKeyCombinationCode => "realtimeclock";
    public double StrokeWidth => capi.World.Config.GetDouble("realtimeclock_strokewidth");
    public int FontSize => capi.World.Config.GetInt("realtimeclock_fontsize");
    public string FontName => capi.World.Config.GetString("realtimeclock_fontname");
    public string Format => capi.World.Config.GetString("realtimeclock_format");
    public string HexColor => capi.World.Config.GetString("realtimeclock_hexcolor");
    public string StrokeHexColor => capi.World.Config.GetString("realtimeclock_strokehexcolor");
    public double FixedHeight => DialogArea.ToString() is "LeftBottom" or "RightBottom" ? 48.0 : 256.0;
    public double FixedWidth => DialogArea.ToString() is "LeftBottom" or "RightBottom" ? 130.0 : 400.0;
    public FontWeight FontWeight => capi.World.Config.GetString("realtimeclock_fontweight") is "Bold" ? FontWeight.Bold : FontWeight.Normal;
    public FontSlant FontSlant => capi.World.Config.GetString("realtimeclock_fontslant").ToEnum<FontSlant>();
    public EnumDialogArea DialogArea => capi.World.Config.GetString("realtimeclock_position").ToEnum<EnumDialogArea>();

    public HudElementRealTimeClock(ICoreClientAPI capi) : base(capi) { base.capi = capi; }

    public override void OnOwnPlayerDataReceived()
    {
      base.OnOwnPlayerDataReceived();
      ElementBounds elementBounds = ElementBounds.Fixed(DialogArea, 0, 0, FixedWidth, FixedHeight);

      SingleComposer = capi
        .Gui
        .CreateCompo("realtimeclock", elementBounds)
        .AddDynamicText(
          "",
          CairoFont
            .WhiteMediumText()
            .WithFont(FontName)
            .WithFontSize(FontSize)
            .WithWeight(FontWeight)
            .WithSlant(FontSlant)
            .WithColor(ColorUtil.Hex2Doubles(HexColor))
            .WithStroke(ColorUtil.Hex2Doubles(StrokeHexColor), StrokeWidth)
            .WithOrientation(EnumTextOrientation.Justify),
          elementBounds.ForkChild(),
          "realtimeclock").Compose();

      id = capi.World.RegisterGameTickListener(delegate
      {
        UpdateText();
      }, 200);
    }

    public void UpdateText() => SingleComposer.GetDynamicText("realtimeclock").SetNewText(DateTime.Now.ToString(Format));

    public override bool TryOpen()
    {
      if (base.TryOpen())
      {
        OnOwnPlayerDataReceived();
        capi.Settings.Bool["realTimeClockGui"] = true;
        return true;
      }
      return false;
    }

    public override bool TryClose()
    {
      if (base.TryClose())
      {
        capi.Settings.Bool["realTimeClockGui"] = false;
        Dispose();
        return true;
      }
      return false;
    }

    public override void Dispose()
    {
      base.Dispose();
      capi.World.UnregisterGameTickListener(id);
    }
  }
}