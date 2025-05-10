// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.Models.FishData.FishSpawnData
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using StardewValley.ItemTypeDefinitions;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework.Models.FishData;

internal record FishSpawnData(
  ParsedItemData FishItem,
  FishSpawnLocationData[]? Locations,
  FishSpawnTimeOfDayData[]? TimesOfDay,
  FishSpawnWeather Weather,
  int MinFishingLevel,
  bool IsUnique,
  bool IsLegendaryFamily)
{
  public bool MatchesLocation(string locationName)
  {
    FishSpawnLocationData[] locations = this.Locations;
    return (locations != null ? new bool?(((IEnumerable<FishSpawnLocationData>) locations).Any<FishSpawnLocationData>((Func<FishSpawnLocationData, bool>) (p => p.MatchesLocation(locationName)))) : new bool?()) ?? false;
  }
}
