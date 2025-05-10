// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.Common.Integrations.IconicFramework.IIconicFrameworkApi
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Microsoft.Xna.Framework;
using System;

#nullable enable
namespace Pathoschild.Stardew.Common.Integrations.IconicFramework;

public interface IIconicFrameworkApi
{
  void AddToolbarIcon(
    string texturePath,
    Rectangle? sourceRect,
    Func<string>? getTitle,
    Func<string>? getDescription,
    Action onClick,
    Action? onRightClick = null);
}
