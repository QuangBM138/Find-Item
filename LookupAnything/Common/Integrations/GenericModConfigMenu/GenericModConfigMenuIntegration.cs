// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.Common.Integrations.GenericModConfigMenu.GenericModConfigMenuIntegration
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using StardewModdingAPI;
using System;

#nullable enable
namespace Pathoschild.Stardew.Common.Integrations.GenericModConfigMenu;

internal static class GenericModConfigMenuIntegration
{
  public static void AddGenericModConfigMenu<TConfig>(
    this IMod mod,
    IGenericModConfigMenuIntegrationFor<TConfig> configMenu,
    Func<TConfig> get,
    Action<TConfig> set,
    Action? onSaved = null)
    where TConfig : class, new()
  {
    GenericModConfigMenuIntegration<TConfig> menu = new GenericModConfigMenuIntegration<TConfig>(mod.Helper.ModRegistry, mod.Monitor, mod.ModManifest, get, new Action(Reset), new Action(SaveAndApply));
    if (!menu.IsLoaded)
      return;
    configMenu.Register(menu, mod.Monitor);

    void Reset()
    {
      set(new TConfig());
      mod.Helper.WriteConfig<TConfig>(get());
    }

    void SaveAndApply()
    {
      mod.Helper.WriteConfig<TConfig>(get());
      Action action = onSaved;
      if (action == null)
        return;
      action();
    }
  }
}
