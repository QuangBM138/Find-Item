// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.Common.Integrations.BetterSprinklersPlus.BetterSprinklersPlusIntegration
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Microsoft.Xna.Framework;
using StardewModdingAPI;
using System.Collections.Generic;

#nullable enable
namespace Pathoschild.Stardew.Common.Integrations.BetterSprinklersPlus;

internal class BetterSprinklersPlusIntegration : BaseIntegration<IBetterSprinklersPlusApi>
{
  public int MaxRadius { get; }

  public BetterSprinklersPlusIntegration(IModRegistry modRegistry, IMonitor monitor)
    : base("Better Sprinklers Plus", "com.CodesThings.BetterSprinklersPlus", "2.6.0", modRegistry, monitor)
  {
    if (!this.IsLoaded)
      return;
    this.MaxRadius = this.ModApi.GetMaxGridSize();
  }

  public IDictionary<int, Vector2[]> GetSprinklerTiles()
  {
    this.AssertLoaded();
    return this.ModApi.GetSprinklerCoverage();
  }
}
