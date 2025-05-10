// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.Common.SpriteInfo
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#nullable enable
namespace Pathoschild.Stardew.Common;

internal class SpriteInfo
{
  public Texture2D Spritesheet { get; }

  public Rectangle SourceRectangle { get; }

  public SpriteInfo(Texture2D spritesheet, Rectangle sourceRectangle)
  {
    this.Spritesheet = spritesheet;
    this.SourceRectangle = sourceRectangle;
  }

  public virtual void Draw(SpriteBatch spriteBatch, int x, int y, Vector2 size, Color? color = null)
  {
    spriteBatch.DrawSpriteWithin(this.Spritesheet, this.SourceRectangle, (float) x, (float) y, size, new Color?(color ?? Color.White));
  }
}
