// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.Common.Integrations.GenericModConfigMenu.IGenericModConfigMenuApi
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using StardewModdingAPI;
using StardewModdingAPI.Utilities;
using System;

#nullable enable
namespace Pathoschild.Stardew.Common.Integrations.GenericModConfigMenu;

public interface IGenericModConfigMenuApi
{
  void Register(IManifest mod, Action reset, Action save, bool titleScreenOnly = false);

  void AddSectionTitle(IManifest mod, Func<string> text, Func<string>? tooltip = null);

  void AddParagraph(IManifest mod, Func<string> text);

  void AddBoolOption(
    IManifest mod,
    Func<bool> getValue,
    Action<bool> setValue,
    Func<string> name,
    Func<string>? tooltip = null,
    string? fieldId = null);

  void AddNumberOption(
    IManifest mod,
    Func<int> getValue,
    Action<int> setValue,
    Func<string> name,
    Func<string>? tooltip = null,
    int? min = null,
    int? max = null,
    int? interval = null,
    Func<int, string>? formatValue = null,
    string? fieldId = null);

  void AddNumberOption(
    IManifest mod,
    Func<float> getValue,
    Action<float> setValue,
    Func<string> name,
    Func<string>? tooltip = null,
    float? min = null,
    float? max = null,
    float? interval = null,
    Func<float, string>? formatValue = null,
    string? fieldId = null);

  void AddTextOption(
    IManifest mod,
    Func<string> getValue,
    Action<string> setValue,
    Func<string> name,
    Func<string>? tooltip = null,
    string[]? allowedValues = null,
    Func<string, string>? formatAllowedValue = null,
    string? fieldId = null);

  void AddKeybindList(
    IManifest mod,
    Func<KeybindList> getValue,
    Action<KeybindList> setValue,
    Func<string> name,
    Func<string>? tooltip = null,
    string? fieldId = null);
}
