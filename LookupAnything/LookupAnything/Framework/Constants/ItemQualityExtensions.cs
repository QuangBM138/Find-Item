// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.Constants.ItemQualityExtensions
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using System;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework.Constants;

internal static class ItemQualityExtensions
{
  public static string GetName(this ItemQuality current) => current.ToString().ToLower();

  public static ItemQuality GetNext(this ItemQuality current)
  {
    switch (current)
    {
      case ItemQuality.Normal:
        return ItemQuality.Silver;
      case ItemQuality.Silver:
        return ItemQuality.Gold;
      case ItemQuality.Gold:
        return ItemQuality.Iridium;
      case ItemQuality.Iridium:
        return ItemQuality.Iridium;
      default:
        throw new NotSupportedException($"Unknown quality '{current}'.");
    }
  }
}
