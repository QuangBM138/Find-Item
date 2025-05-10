// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.Common.Integrations.PelicanFiber.PelicanFiberIntegration
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;

#nullable enable
namespace Pathoschild.Stardew.Common.Integrations.PelicanFiber;

internal class PelicanFiberIntegration : BaseIntegration
{
  private readonly string MenuTypeName = "PelicanFiber.Framework.ConstructionMenu";
  private readonly IReflectionHelper Reflection;

  public PelicanFiberIntegration(
    IModRegistry modRegistry,
    IReflectionHelper reflection,
    IMonitor monitor)
    : base("Pelican Fiber", "jwdred.PelicanFiber", "3.1.1-unofficial.7.1-pathoschild", modRegistry, monitor)
  {
    this.Reflection = reflection;
  }

  public bool IsBuildMenuOpen()
  {
    this.AssertLoaded();
    return Game1.activeClickableMenu?.GetType().FullName == this.MenuTypeName;
  }

  public CarpenterMenu.BlueprintEntry? GetBuildMenuBlueprint()
  {
    this.AssertLoaded();
    return !this.IsBuildMenuOpen() ? (CarpenterMenu.BlueprintEntry) null : this.Reflection.GetProperty<CarpenterMenu.BlueprintEntry>((object) Game1.activeClickableMenu, "Blueprint", true).GetValue();
  }
}
