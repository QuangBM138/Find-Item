// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.ModConfigKeys
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using StardewModdingAPI;
using StardewModdingAPI.Utilities;
using System;
using System.Runtime.Serialization;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework;

internal class ModConfigKeys
{
  public KeybindList ToggleLookup { get; set; } = new KeybindList((SButton) 112 /*0x70*/);

  public KeybindList ToggleSearch { get; set; } = KeybindList.Parse($"{(SButton) 160 /*0xA0*/} + {(SButton) 112 /*0x70*/}");

  public KeybindList ScrollUp { get; set; } = new KeybindList((SButton) 38);

  public KeybindList ScrollDown { get; set; } = new KeybindList((SButton) 40);

  public KeybindList PageUp { get; set; } = new KeybindList((SButton) 33);

  public KeybindList PageDown { get; set; } = new KeybindList((SButton) 34);

  public KeybindList ToggleDebug { get; set; } = new KeybindList(Array.Empty<Keybind>());

  [System.Runtime.Serialization.OnDeserialized]
  public void OnDeserialized(StreamingContext context)
  {
    KeybindList keybindList;
    if (this.ToggleLookup == null)
      this.ToggleLookup = keybindList = new KeybindList(Array.Empty<Keybind>());
    if (this.ToggleSearch == null)
      this.ToggleSearch = keybindList = new KeybindList(Array.Empty<Keybind>());
    if (this.ScrollUp == null)
      this.ScrollUp = keybindList = new KeybindList(Array.Empty<Keybind>());
    if (this.ScrollDown == null)
      this.ScrollDown = keybindList = new KeybindList(Array.Empty<Keybind>());
    if (this.PageUp == null)
      this.PageUp = keybindList = new KeybindList(Array.Empty<Keybind>());
    if (this.PageDown == null)
      this.PageDown = keybindList = new KeybindList(Array.Empty<Keybind>());
    if (this.ToggleDebug != null)
      return;
    this.ToggleDebug = keybindList = new KeybindList(Array.Empty<Keybind>());
  }
}
