// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.Common.Integrations.TrainStation.TrainStationIntegration
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using StardewModdingAPI;
using System.Collections.Generic;

#nullable enable
namespace Pathoschild.Stardew.Common.Integrations.TrainStation;

internal class TrainStationIntegration(IModRegistry modRegistry, IMonitor monitor) : 
  BaseIntegration<ITrainStationApi>("Train Station", "Cherry.TrainStation", "2.2.0", modRegistry, monitor)
{
  public IEnumerable<ITrainStationStopModel> GetAvailableStops(bool isBoat)
  {
    this.AssertLoaded();
    return this.ModApi.GetAvailableStops(isBoat);
  }
}
