// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.Common.GameI18n
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using StardewValley;
using StardewValley.GameData.Buildings;
using StardewValley.TokenizableStrings;
using System;

#nullable enable
namespace Pathoschild.Stardew.Common;

internal static class GameI18n
{
  public static string GetBuildingName(string id)
  {
    if (Game1.buildingData == null)
      return "(missing translation: game hasn't loaded object data yet)";
    try
    {
      BuildingData buildingData;
      return Game1.buildingData.TryGetValue(id, out buildingData) ? TokenParser.ParseText(buildingData?.Name, (Random) null, (TokenParserDelegate) null, (Farmer) null) ?? id : $"(missing translation: no building with ID '{id}')";
    }
    catch
    {
      return $"(missing translation: building with ID '{id}' has an invalid format)";
    }
  }

  public static string GetObjectName(string id)
  {
    if (Game1.objectData == null)
      return "(missing translation: game hasn't loaded object data yet)";
    try
    {
      return ItemRegistry.GetData(ItemRegistry.ManuallyQualifyItemId(id, "(O)", false))?.DisplayName ?? $"(missing translation: no object with ID '{id}')";
    }
    catch
    {
      return $"(missing translation: object with ID '{id}' has an invalid format)";
    }
  }

  public static string GetString(string key, params object[] substitutions)
  {
    return Game1.content.LoadString(key, substitutions);
  }
}
