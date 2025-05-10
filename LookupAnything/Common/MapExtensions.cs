// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.Common.MapExtensions
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using System;
using xTile;
using xTile.Dimensions;
using xTile.Layers;

#nullable enable
namespace Pathoschild.Stardew.Common;

internal static class MapExtensions
{
  public static Size GetSizeInTiles(this Map map)
  {
    int val1_1 = 1;
    int val1_2 = 1;
    foreach (Layer layer in map.Layers)
    {
      val1_1 = Math.Max(val1_1, layer.LayerWidth);
      val1_2 = Math.Max(val1_2, layer.LayerHeight);
    }
    return new Size(val1_1, val1_2);
  }
}
