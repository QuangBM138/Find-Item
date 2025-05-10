// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.Common.TileHelper
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Microsoft.Xna.Framework;
using StardewValley;
using System;
using System.Collections.Generic;
using xTile.Dimensions;
using xTile.Layers;

#nullable enable
namespace Pathoschild.Stardew.Common;

internal static class TileHelper
{
  public static IEnumerable<Vector2> GetTiles(this GameLocation? location)
  {
    if (location?.Map?.Layers == null)
      return (IEnumerable<Vector2>) Array.Empty<Vector2>();
    Layer layer = location.Map.Layers[0];
    return TileHelper.GetTiles(0, 0, layer.LayerWidth, layer.LayerHeight);
  }

  public static IEnumerable<Vector2> GetTiles(this Rectangle area)
  {
    return TileHelper.GetTiles(area.X, area.Y, area.Width, area.Height);
  }

  public static Rectangle Expand(this Rectangle area, int distance)
  {
    return new Rectangle(area.X - distance, area.Y - distance, area.Width + distance * 2, area.Height + distance * 2);
  }

  public static IEnumerable<Vector2> GetSurroundingTiles(this Vector2 tile)
  {
    return (IEnumerable<Vector2>) Utility.getSurroundingTileLocationsArray(tile);
  }

  public static IEnumerable<Vector2> GetSurroundingTiles(this Rectangle area)
  {
    for (int x = area.X - 1; x <= area.X + area.Width; ++x)
    {
      for (int y = area.Y - 1; y <= area.Y + area.Height; ++y)
      {
        if (!((Rectangle) ref area).Contains(x, y))
          yield return new Vector2((float) x, (float) y);
      }
    }
  }

  public static IEnumerable<Vector2> GetAdjacentTiles(this Vector2 tile)
  {
    return (IEnumerable<Vector2>) Utility.getAdjacentTileLocationsArray(tile);
  }

  public static IEnumerable<Vector2> GetTiles(int x, int y, int width, int height)
  {
    int curX = x;
    for (int maxX = x + width - 1; curX <= maxX; ++curX)
    {
      int curY = y;
      for (int maxY = y + height - 1; curY <= maxY; ++curY)
        yield return new Vector2((float) curX, (float) curY);
    }
  }

  public static IEnumerable<Vector2> GetVisibleTiles(int expand = 0)
  {
    return TileHelper.GetVisibleArea(expand).GetTiles();
  }

  public static Rectangle GetVisibleArea(int expand = 0)
  {
    return new Rectangle(((Rectangle) ref Game1.viewport).X / 64 /*0x40*/ - expand, ((Rectangle) ref Game1.viewport).Y / 64 /*0x40*/ - expand, (int) Math.Ceiling((Decimal) ((Rectangle) ref Game1.viewport).Width / 64M) + expand * 2, (int) Math.Ceiling((Decimal) ((Rectangle) ref Game1.viewport).Height / 64M) + expand * 2);
  }

  public static Vector2 GetTileFromCursor()
  {
    return TileHelper.GetTileFromScreenPosition((float) Game1.getMouseX(), (float) Game1.getMouseY());
  }

  public static Vector2 GetTileFromScreenPosition(float x, float y)
  {
    return new Vector2((float) (int) (((double) ((Rectangle) ref Game1.viewport).X + (double) x) / 64.0), (float) (int) (((double) ((Rectangle) ref Game1.viewport).Y + (double) y) / 64.0));
  }
}
