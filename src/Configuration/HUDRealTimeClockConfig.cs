using System;
using System.Collections.Generic;
using Cairo;

namespace HUDRealTimeClock.Configuration
{
  public enum EnumPosition
  {
    LeftTop, RightTop, LeftBottom, RightBottom,
  }

  class RealTimeClockConfig
  {
    // You don't need to know what the hell is going on here :)
    public static string GetEnumValues(Type enumType)
    {
      List<string> strings = new();
      var values = Enum.GetValues(enumType);
      for (int i = 0; i < values.Length; i++)
      {
        object v = values.GetValue(i) ?? default;
        strings.Add($"{(int)v} = {v}");
      }
      return string.Join(", ", strings);
    }

    public string Format = "HH:mm:ss";
    public readonly string PositionComment = GetEnumValues(typeof(EnumPosition));
    public EnumPosition Position = EnumPosition.RightTop;
    public int FontSize = 32;
    public string FontName = "Lora";
    public readonly string FontSlantComment = GetEnumValues(typeof(FontSlant));
    public FontSlant FontSlant = FontSlant.Normal;
    public readonly string FontWeightComment = GetEnumValues(typeof(FontWeight));
    public FontWeight FontWeight = FontWeight.Normal;
    public string HexColor = "#FFFFFF";
    public string StrokeHexColor = "#000000";
    public double StrokeWidth = 2.0;

    public RealTimeClockConfig() { }

    public RealTimeClockConfig(RealTimeClockConfig previousConfig)
    {
      FontName = previousConfig.FontName;
      FontSize = previousConfig.FontSize;
      FontSlant = previousConfig.FontSlant;
      FontSlantComment = previousConfig.FontSlantComment;
      FontWeight = previousConfig.FontWeight;
      FontWeightComment = previousConfig.FontWeightComment;
      Format = previousConfig.Format;
      HexColor = previousConfig.HexColor;
      Position = previousConfig.Position;
      PositionComment = previousConfig.PositionComment;
      StrokeHexColor = previousConfig.StrokeHexColor;
      StrokeWidth = previousConfig.StrokeWidth;
    }
  }
}