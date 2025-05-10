// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.Common.Integrations.TrainStation.ITrainStationStopModel
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

#nullable enable
namespace Pathoschild.Stardew.Common.Integrations.TrainStation;

public interface ITrainStationStopModel
{
  string Id { get; }

  string DisplayName { get; }

  string TargetMapName { get; }

  int TargetX { get; }

  int TargetY { get; }

  int FacingDirectionAfterWarp { get; }

  int Cost { get; }

  bool IsBoat { get; }

  string[] Conditions { get; }
}
