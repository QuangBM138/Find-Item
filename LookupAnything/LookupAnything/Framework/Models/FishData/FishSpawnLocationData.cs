// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.Models.FishData.FishSpawnLocationData
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using System;
using System.Collections.Generic;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework.Models.FishData;

internal record FishSpawnLocationData(string LocationId, string? Area, HashSet<string> Seasons)
{
  internal FishSpawnLocationData(string locationId, int? area, string[] seasons)
  {
    string locationId1 = locationId;
    int? nullable = area;
    int num = 0;
    string area1 = nullable.GetValueOrDefault() >= num & nullable.HasValue ? area.ToString() : (string) null;
    string[] seasons1 = seasons;
    // ISSUE: explicit constructor call
    this.\u002Ector(locationId1, area1, seasons1);
  }

  internal FishSpawnLocationData(string locationId, string? area, string[] seasons)
    : this(locationId, area, new HashSet<string>((IEnumerable<string>) seasons, (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase))
  {
  }

  public bool MatchesLocation(string locationId)
  {
    return this.LocationId == "UndergroundMine" && !string.IsNullOrWhiteSpace(this.Area) ? locationId == this.LocationId + this.Area : locationId == this.LocationId;
  }
}
