// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.Common.Integrations.ExtraMachineConfig.IExtraMachineConfigApi
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using StardewValley.GameData.Machines;
using System.Collections.Generic;

#nullable enable
namespace Pathoschild.Stardew.Common.Integrations.ExtraMachineConfig;

public interface IExtraMachineConfigApi
{
  IList<(string, int)> GetExtraRequirements(MachineItemOutput outputData);

  IList<(string, int)> GetExtraTagsRequirements(MachineItemOutput outputData);

  IList<MachineItemOutput> GetExtraOutputs(MachineItemOutput outputData, MachineData? machine);
}
