// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Components.BaseMenu
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;
using System;
using xTile.Dimensions;

#nullable disable
namespace Pathoschild.Stardew.LookupAnything.Components;

internal class BaseMenu : IClickableMenu
{
  protected static bool UseSafeDimensions { get; set; }

  protected Point GetViewportSize()
  {
    Point viewportSize;
    // ISSUE: explicit constructor call
    ((Point) ref viewportSize).\u002Ector(((Rectangle) ref Game1.uiViewport).Width, ((Rectangle) ref Game1.uiViewport).Height);
    if (BaseMenu.UseSafeDimensions)
    {
      Viewport viewport1 = Game1.graphics.GraphicsDevice.Viewport;
      if (((Viewport) ref viewport1).Width < viewportSize.X)
      {
        ref Point local = ref viewportSize;
        int x = viewportSize.X;
        Viewport viewport2 = Game1.graphics.GraphicsDevice.Viewport;
        int width = ((Viewport) ref viewport2).Width;
        int num1 = Math.Min(x, width);
        int y = viewportSize.Y;
        viewport2 = Game1.graphics.GraphicsDevice.Viewport;
        int height = ((Viewport) ref viewport2).Height;
        int num2 = Math.Min(y, height);
        // ISSUE: explicit constructor call
        ((Point) ref local).\u002Ector(num1, num2);
      }
    }
    return viewportSize;
  }
}
