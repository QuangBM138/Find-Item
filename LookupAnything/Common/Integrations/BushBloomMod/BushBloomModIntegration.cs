// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.Common.Integrations.BushBloomMod.BushBloomModIntegration
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using StardewModdingAPI;

#nullable enable
namespace Pathoschild.Stardew.Common.Integrations.BushBloomMod;

internal class BushBloomModIntegration(IModRegistry modRegistry, IMonitor monitor) : 
  BaseIntegration<IBushBloomModApi>("CustomBush", "NCarigon.BushBloomMod", "1.2.4", modRegistry, monitor)
{
}
