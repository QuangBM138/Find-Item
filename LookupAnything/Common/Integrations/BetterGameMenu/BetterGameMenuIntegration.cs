// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.Common.Integrations.BetterGameMenu.BetterGameMenuIntegration
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using StardewModdingAPI;
using StardewValley.Menus;

#nullable enable
namespace Pathoschild.Stardew.Common.Integrations.BetterGameMenu;

internal class BetterGameMenuIntegration(IModRegistry modRegistry, IMonitor monitor) : 
  BaseIntegration<IBetterGameMenuApi>("BetterGameMenu", "leclair.bettergamemenu", "0.5.2", modRegistry, monitor)
{
  public IClickableMenu? GetCurrentPage(IClickableMenu? menu)
  {
    return this.IsLoaded && menu != null ? this.ModApi.GetCurrentPage(menu) : (IClickableMenu) null;
  }
}
