// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.Lookups.TerrainFeatures.FruitTreeTarget
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Netcode;
using StardewValley;
using StardewValley.TerrainFeatures;
using System;
using xTile.Dimensions;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework.Lookups.TerrainFeatures;

internal class FruitTreeTarget : GenericTarget<FruitTree>
{
  private readonly Texture2D? Texture;
  private readonly Rectangle SourceRect;

  public FruitTreeTarget(
    GameHelper gameHelper,
    FruitTree value,
    Vector2 tilePosition,
    Func<ISubject> getSubject)
    : base(gameHelper, SubjectType.FruitTree, value, tilePosition, getSubject)
  {
    this.GetSpriteSheet(value, out this.Texture, out this.SourceRect);
  }

  public override Rectangle GetSpritesheetArea()
  {
    FruitTree fruitTree = this.Value;
    if (((NetFieldBase<bool, NetBool>) fruitTree.stump).Value)
      return new Rectangle(this.SourceRect.X + 384, this.SourceRect.Y + 48 /*0x30*/, 48 /*0x30*/, 32 /*0x20*/);
    if (((NetFieldBase<int, NetInt>) fruitTree.growthStage).Value >= 4)
      return new Rectangle(this.SourceRect.X + (12 + (fruitTree.IgnoresSeasonsHere() ? 1 : Game1.seasonIndex) * 3) * 16 /*0x10*/, this.SourceRect.Y, 48 /*0x30*/, 80 /*0x50*/);
    switch (((NetFieldBase<int, NetInt>) fruitTree.growthStage).Value)
    {
      case 0:
      case 1:
      case 2:
        return new Rectangle(this.SourceRect.X + ((NetFieldBase<int, NetInt>) fruitTree.growthStage).Value * 48 /*0x30*/, this.SourceRect.Y, 48 /*0x30*/, 80 /*0x50*/);
      default:
        return new Rectangle(this.SourceRect.X + 144 /*0x90*/, this.SourceRect.Y, 48 /*0x30*/, 80 /*0x50*/);
    }
  }

  public override Rectangle GetWorldArea()
  {
    FruitTree fruitTree = this.Value;
    Rectangle spritesheetArea = this.GetSpritesheetArea();
    int num1 = spritesheetArea.Width * 4;
    int num2 = spritesheetArea.Height * 4;
    int num3;
    int num4;
    if (((NetFieldBase<int, NetInt>) fruitTree.growthStage).Value < 4)
    {
      Vector2 tile = this.Tile;
      Vector2 vector2_1;
      // ISSUE: explicit constructor call
      ((Vector2) ref vector2_1).\u002Ector((float) Math.Max(-8.0, Math.Min(64.0, Math.Sin((double) tile.X * 200.0 / (2.0 * Math.PI)) * -16.0)), (float) Math.Max(-8.0, Math.Min(64.0, Math.Sin((double) tile.X * 200.0 / (2.0 * Math.PI)) * -16.0)));
      Vector2 vector2_2 = Vector2.op_Subtraction(new Vector2((float) ((double) tile.X * 64.0 + 32.0) + vector2_1.X, (float) ((double) tile.Y * 64.0 - (double) spritesheetArea.Height + 128.0) + vector2_1.Y), new Vector2((float) ((Rectangle) ref Game1.uiViewport).X, (float) ((Rectangle) ref Game1.uiViewport).Y));
      num3 = (int) vector2_2.X - num1 / 2;
      num4 = (int) vector2_2.Y - num2;
    }
    else
    {
      Rectangle worldArea = base.GetWorldArea();
      num3 = ((Rectangle) ref worldArea).Center.X - num1 / 2;
      num4 = ((Rectangle) ref worldArea).Bottom - num2;
    }
    return new Rectangle(num3, num4, num1, num2);
  }

  public override bool SpriteIntersectsPixel(Vector2 tile, Vector2 position, Rectangle spriteArea)
  {
    SpriteEffects spriteEffects = ((NetFieldBase<bool, NetBool>) this.Value.flipped).Value ? (SpriteEffects) 1 : (SpriteEffects) 0;
    return this.SpriteIntersectsPixel(tile, position, spriteArea, this.Texture, this.GetSpritesheetArea(), spriteEffects);
  }

  public void GetSpriteSheet(FruitTree target, out Texture2D? texture, out Rectangle sourceRect)
  {
    texture = target.texture;
    sourceRect = new Rectangle(0, target.GetSpriteRowNumber() * 5 * 16 /*0x10*/, 432, 80 /*0x50*/);
  }
}
