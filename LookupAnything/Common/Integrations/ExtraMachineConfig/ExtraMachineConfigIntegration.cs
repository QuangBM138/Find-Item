// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.Common.Integrations.ExtraMachineConfig.ExtraMachineConfigIntegration
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using StardewModdingAPI;

#nullable enable
namespace Pathoschild.Stardew.Common.Integrations.ExtraMachineConfig;

internal class ExtraMachineConfigIntegration(IModRegistry modRegistry, IMonitor monitor) : 
  BaseIntegration<IExtraMachineConfigApi>("ExtraMachineConfig", "selph.ExtraMachineConfig", "1.4.0", modRegistry, monitor)
{
}
