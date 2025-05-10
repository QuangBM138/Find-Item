// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.Lookups.GenericTarget`1
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using System;
using xTile.Dimensions;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework.Lookups;

internal abstract class GenericTarget<TValue> : ITarget
{
  protected const int PrecedenceForFlooring = 999;
  protected const int PrecedenceForTile = 1000;

  protected GameHelper GameHelper { get; }

  public SubjectType Type { get; protected set; }

  public Vector2 Tile { get; protected set; }

  public TValue Value { get; }

  public Func<ISubject> GetSubject { get; protected set; }

  public int Precedence { get; protected set; }

  public abstract Rectangle GetSpritesheetArea();

  public virtual Rectangle GetWorldArea()
  {
    return this.GameHelper.GetScreenCoordinatesFromTile(this.Tile);
  }

  public virtual bool SpriteIntersectsPixel(Vector2 tile, Vector2 position, Rectangle spriteArea)
  {
    return Vector2.op_Equality(this.Tile, tile);
  }

  protected GenericTarget(
    GameHelper gameHelper,
    SubjectType type,
    TValue value,
    Vector2 tilePosition,
    Func<ISubject> getSubject)
  {
    this.GameHelper = gameHelper;
    this.Type = type;
    this.Value = value;
    this.Tile = tilePosition;
    this.GetSubject = getSubject;
  }

  protected Rectangle GetSpriteArea(Rectangle boundingBox, Rectangle sourceRectangle)
  {
    int num1 = sourceRectangle.Height * 4;
    int num2 = sourceRectangle.Width * 4;
    int num3 = ((Rectangle) ref boundingBox).Center.X - num2 / 2;
    int num4 = boundingBox.Y + boundingBox.Height - num1;
    return new Rectangle(num3 - ((Rectangle) ref Game1.uiViewport).X, num4 - ((Rectangle) ref Game1.uiViewport).Y, num2, num1);
  }

  protected bool SpriteIntersectsPixel(
    Vector2 tile,
    Vector2 position,
    Rectangle spriteArea,
    Texture2D? spriteSheet,
    Rectangle spriteSourceRectangle,
    SpriteEffects spriteEffects = 0)
  {
    if (spriteSheet == null)
      return false;
    Vector2 sheetCoordinates = this.GameHelper.GetSpriteSheetCoordinates(position, spriteArea, spriteSourceRectangle, spriteEffects);
    if (!((Rectangle) ref spriteSourceRectangle).Contains((int) sheetCoordinates.X, (int) sheetCoordinates.Y))
      return false;
    Color spriteSheetPixel = this.GameHelper.GetSpriteSheetPixel<Color>(spriteSheet, sheetCoordinates);
    return ((Color) ref spriteSheetPixel).A > (byte) 0;
  }
}
