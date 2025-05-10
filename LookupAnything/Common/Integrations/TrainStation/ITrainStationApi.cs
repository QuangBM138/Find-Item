// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.Common.Integrations.TrainStation.ITrainStationApi
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using System.Collections.Generic;

#nullable enable
namespace Pathoschild.Stardew.Common.Integrations.TrainStation;

public interface ITrainStationApi
{
  IEnumerable<ITrainStationStopModel> GetAvailableStops(bool isBoat);
}
