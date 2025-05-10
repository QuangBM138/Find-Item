// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.Common.Integrations.IconicFramework.IconicFrameworkIntegration
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Microsoft.Xna.Framework;
using StardewModdingAPI;
using System;

#nullable enable
namespace Pathoschild.Stardew.Common.Integrations.IconicFramework;

internal class IconicFrameworkIntegration(IModRegistry modRegistry, IMonitor monitor) : 
  BaseIntegration<IIconicFrameworkApi>("IconicFramework", "furyx639.ToolbarIcons", "3.1.0-beta.1", modRegistry, monitor)
{
  public void AddToolbarIcon(
    string texturePath,
    Rectangle? sourceRect,
    Func<string>? getTitle,
    Func<string>? getDescription,
    Action onClick,
    Action? onRightClick = null)
  {
    this.AssertLoaded();
    this.ModApi.AddToolbarIcon(texturePath, sourceRect, getTitle, getDescription, onClick, onRightClick);
  }
}
