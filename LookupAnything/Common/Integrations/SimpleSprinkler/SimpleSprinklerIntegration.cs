// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.Common.Integrations.SimpleSprinkler.SimpleSprinklerIntegration
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Microsoft.Xna.Framework;
using StardewModdingAPI;
using System.Collections.Generic;

#nullable enable
namespace Pathoschild.Stardew.Common.Integrations.SimpleSprinkler;

internal class SimpleSprinklerIntegration(IModRegistry modRegistry, IMonitor monitor) : 
  BaseIntegration<ISimplerSprinklerApi>("Simple Sprinklers", "tZed.SimpleSprinkler", "1.6.0", modRegistry, monitor)
{
  public IDictionary<int, Vector2[]> GetNewSprinklerTiles()
  {
    this.AssertLoaded();
    return this.ModApi.GetNewSprinklerCoverage();
  }
}
