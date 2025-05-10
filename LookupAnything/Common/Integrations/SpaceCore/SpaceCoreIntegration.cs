// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.Common.Integrations.SpaceCore.SpaceCoreIntegration
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using StardewModdingAPI;

#nullable enable
namespace Pathoschild.Stardew.Common.Integrations.SpaceCore;

internal class SpaceCoreIntegration(IModRegistry modRegistry, IMonitor monitor) : 
  BaseIntegration<ISpaceCoreApi>("SpaceCore", "spacechase0.SpaceCore", "1.25.0", modRegistry, monitor)
{
}
