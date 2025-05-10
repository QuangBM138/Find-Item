// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.Common.Integrations.Automate.AutomateIntegration
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using System.Collections.Generic;

#nullable enable
namespace Pathoschild.Stardew.Common.Integrations.Automate;

internal class AutomateIntegration(IModRegistry modRegistry, IMonitor monitor) : 
  BaseIntegration<IAutomateApi>("Automate", "Pathoschild.Automate", "1.11.0", modRegistry, monitor)
{
  public IDictionary<Vector2, int> GetMachineStates(GameLocation location, Rectangle tileArea)
  {
    this.AssertLoaded();
    return this.ModApi.GetMachineStates(location, tileArea);
  }
}
