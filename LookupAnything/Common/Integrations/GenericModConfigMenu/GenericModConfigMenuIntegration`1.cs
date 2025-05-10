// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.Common.Integrations.GenericModConfigMenu.GenericModConfigMenuIntegration`1
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using StardewModdingAPI;
using StardewModdingAPI.Utilities;
using System;

#nullable enable
namespace Pathoschild.Stardew.Common.Integrations.GenericModConfigMenu;

internal class GenericModConfigMenuIntegration<TConfig> : BaseIntegration<IGenericModConfigMenuApi> where TConfig : new()
{
  private readonly IManifest ConsumerManifest;
  private readonly Func<TConfig> GetConfig;
  private readonly Action Reset;
  private readonly Action SaveAndApply;

  public GenericModConfigMenuIntegration(
    IModRegistry modRegistry,
    IMonitor monitor,
    IManifest manifest,
    Func<TConfig> getConfig,
    Action reset,
    Action saveAndApply)
    : base("Generic Mod Config Menu", "spacechase0.GenericModConfigMenu", "1.9.6", modRegistry, monitor)
  {
    this.ConsumerManifest = manifest;
    this.GetConfig = getConfig;
    this.Reset = reset;
    this.SaveAndApply = saveAndApply;
  }

  public GenericModConfigMenuIntegration<TConfig> Register(bool titleScreenOnly = false)
  {
    this.AssertLoaded();
    this.ModApi.Register(this.ConsumerManifest, this.Reset, this.SaveAndApply, titleScreenOnly);
    return this;
  }

  public GenericModConfigMenuIntegration<TConfig> AddSectionTitle(
    Func<string> text,
    Func<string>? tooltip = null)
  {
    this.AssertLoaded();
    this.ModApi.AddSectionTitle(this.ConsumerManifest, text, tooltip);
    return this;
  }

  public GenericModConfigMenuIntegration<TConfig> AddParagraph(Func<string> text)
  {
    this.AssertLoaded();
    this.ModApi.AddParagraph(this.ConsumerManifest, text);
    return this;
  }

  public GenericModConfigMenuIntegration<TConfig> AddCheckbox(
    Func<string> name,
    Func<string> tooltip,
    Func<TConfig, bool> get,
    Action<TConfig, bool> set,
    bool enable = true)
  {
    this.AssertLoaded();
    if (enable)
      this.ModApi.AddBoolOption(this.ConsumerManifest, (Func<bool>) (() => get(this.GetConfig())), (Action<bool>) (val => set(this.GetConfig(), val)), name, tooltip);
    return this;
  }

  public GenericModConfigMenuIntegration<TConfig> AddDropdown(
    Func<string> name,
    Func<string> tooltip,
    Func<TConfig, string> get,
    Action<TConfig, string> set,
    string[] allowedValues,
    Func<string, string> formatAllowedValue,
    bool enable = true)
  {
    this.AssertLoaded();
    if (enable)
      this.ModApi.AddTextOption(this.ConsumerManifest, (Func<string>) (() => get(this.GetConfig())), (Action<string>) (val => set(this.GetConfig(), val)), name, tooltip, allowedValues, formatAllowedValue);
    return this;
  }

  public GenericModConfigMenuIntegration<TConfig> AddTextbox(
    Func<string> name,
    Func<string> tooltip,
    Func<TConfig, string> get,
    Action<TConfig, string> set,
    bool enable = true)
  {
    this.AssertLoaded();
    if (enable)
      this.ModApi.AddTextOption(this.ConsumerManifest, (Func<string>) (() => get(this.GetConfig())), (Action<string>) (val => set(this.GetConfig(), val)), name, tooltip);
    return this;
  }

  public GenericModConfigMenuIntegration<TConfig> AddNumberField(
    Func<string> name,
    Func<string> tooltip,
    Func<TConfig, int> get,
    Action<TConfig, int> set,
    int min,
    int max,
    bool enable = true)
  {
    this.AssertLoaded();
    if (enable)
      this.ModApi.AddNumberOption(this.ConsumerManifest, (Func<int>) (() => get(this.GetConfig())), (Action<int>) (val => set(this.GetConfig(), val)), name, tooltip, new int?(min), new int?(max));
    return this;
  }

  public GenericModConfigMenuIntegration<TConfig> AddNumberField(
    Func<string> name,
    Func<string> tooltip,
    Func<TConfig, float> get,
    Action<TConfig, float> set,
    float min,
    float max,
    bool enable = true,
    float interval = 0.1f)
  {
    this.AssertLoaded();
    if (enable)
      this.ModApi.AddNumberOption(this.ConsumerManifest, (Func<float>) (() => get(this.GetConfig())), (Action<float>) (val => set(this.GetConfig(), val)), name, tooltip, new float?(min), new float?(max), new float?(interval));
    return this;
  }

  public GenericModConfigMenuIntegration<TConfig> AddKeyBinding(
    Func<string> name,
    Func<string> tooltip,
    Func<TConfig, KeybindList> get,
    Action<TConfig, KeybindList> set,
    bool enable = true)
  {
    this.AssertLoaded();
    if (enable)
      this.ModApi.AddKeybindList(this.ConsumerManifest, (Func<KeybindList>) (() => get(this.GetConfig())), (Action<KeybindList>) (val => set(this.GetConfig(), val)), name, tooltip);
    return this;
  }
}
