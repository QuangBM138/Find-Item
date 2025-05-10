// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.Common.DrawHelper
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using System;

#nullable enable
namespace Pathoschild.Stardew.Common;

internal static class DrawHelper
{
  public static float GetSpaceWidth(SpriteFont font) => CommonHelper.GetSpaceWidth(font);

  public static void DrawSprite(
    this SpriteBatch spriteBatch,
    Texture2D sheet,
    Rectangle sprite,
    float x,
    float y,
    Vector2 errorSize,
    Color? color = null,
    float scale = 1f)
  {
    try
    {
      spriteBatch.Draw(sheet, new Vector2(x, y), new Rectangle?(sprite), color ?? Color.White, 0.0f, Vector2.Zero, scale, (SpriteEffects) 0, 0.0f);
    }
    catch
    {
      Utility.DrawErrorTexture(spriteBatch, new Rectangle((int) x, (int) y, (int) errorSize.X, (int) errorSize.Y), 0.0f);
    }
  }

  public static void DrawSprite(
    this SpriteBatch spriteBatch,
    Texture2D sheet,
    Rectangle sprite,
    float x,
    float y,
    Point errorSize,
    Color? color = null,
    float scale = 1f)
  {
    try
    {
      spriteBatch.Draw(sheet, new Vector2(x, y), new Rectangle?(sprite), color ?? Color.White, 0.0f, Vector2.Zero, scale, (SpriteEffects) 0, 0.0f);
    }
    catch
    {
      Utility.DrawErrorTexture(spriteBatch, new Rectangle((int) x, (int) y, errorSize.X, errorSize.Y), 0.0f);
    }
  }

  public static void DrawSpriteWithin(
    this SpriteBatch spriteBatch,
    SpriteInfo? sprite,
    float x,
    float y,
    Vector2 size,
    Color? color = null)
  {
    try
    {
      sprite?.Draw(spriteBatch, (int) x, (int) y, size, color);
    }
    catch
    {
      Utility.DrawErrorTexture(spriteBatch, new Rectangle((int) x, (int) y, (int) size.X, (int) size.Y), 0.0f);
    }
  }

  public static void DrawSpriteWithin(
    this SpriteBatch spriteBatch,
    Texture2D sheet,
    Rectangle sprite,
    float x,
    float y,
    Vector2 size,
    Color? color = null)
  {
    float num1 = (float) Math.Max(sprite.Width, sprite.Height);
    float scale = size.X / num1;
    float num2 = Math.Max((float) (((double) size.X - (double) sprite.Width * (double) scale) / 2.0), 0.0f);
    float num3 = Math.Max((float) (((double) size.Y - (double) sprite.Height * (double) scale) / 2.0), 0.0f);
    spriteBatch.DrawSprite(sheet, sprite, x + num2, y + num3, size, new Color?(color ?? Color.White), scale);
  }

  public static void DrawLine(
    this SpriteBatch batch,
    float x,
    float y,
    Vector2 size,
    Color? color = null)
  {
    batch.Draw(CommonHelper.Pixel, new Rectangle((int) x, (int) y, (int) size.X, (int) size.Y), color ?? Color.White);
  }
}
