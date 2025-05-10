// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.Lookups.Characters.CharacterTarget
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Monsters;
using System;
using xTile.Dimensions;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework.Lookups.Characters;

internal class CharacterTarget(
  GameHelper gameHelper,
  SubjectType type,
  NPC value,
  Vector2 tilePosition,
  Func<ISubject> getSubject) : GenericTarget<NPC>(gameHelper, type, value, tilePosition, getSubject)
{
  public override Rectangle GetSpritesheetArea() => ((Character) this.Value).Sprite.SourceRect;

  public override Rectangle GetWorldArea()
  {
    NPC npc = this.Value;
    AnimatedSprite sprite = ((Character) npc).Sprite;
    Rectangle boundingBox = ((Character) npc).GetBoundingBox();
    float num1;
    switch (npc)
    {
      case DustSpirit _:
        num1 = (float) ((Rectangle) ref boundingBox).Bottom;
        break;
      case Bat _:
        num1 = (float) ((Rectangle) ref boundingBox).Center.Y;
        break;
      case Bug _:
        num1 = (float) (((Rectangle) ref boundingBox).Top - sprite.SpriteHeight * 4) + (float) (Math.Sin((double) Game1.currentGameTime.TotalGameTime.Milliseconds / 1000.0 * (2.0 * Math.PI)) * 10.0);
        break;
      case SquidKid squidKid:
        num1 = (float) (((Rectangle) ref boundingBox).Bottom - sprite.SpriteHeight * 4 + squidKid.yOffset);
        break;
      default:
        num1 = (float) ((Rectangle) ref boundingBox).Top;
        break;
    }
    int num2 = sprite.SpriteHeight * 4;
    int num3 = sprite.SpriteWidth * 4;
    float num4 = (float) (((Rectangle) ref boundingBox).Center.X - num3 / 2);
    float num5 = num1 + (float) boundingBox.Height - (float) num2 + (float) (((Character) npc).yJumpOffset * 2);
    return new Rectangle((int) ((double) num4 - (double) ((Rectangle) ref Game1.uiViewport).X), (int) ((double) num5 - (double) ((Rectangle) ref Game1.uiViewport).Y), num3, num2);
  }

  public override bool SpriteIntersectsPixel(Vector2 tile, Vector2 position, Rectangle spriteArea)
  {
    NPC npc = this.Value;
    AnimatedSprite sprite = ((Character) npc).Sprite;
    if (npc is Monster)
      return ((Rectangle) ref spriteArea).Contains((int) position.X, (int) position.Y);
    SpriteEffects spriteEffects = ((Character) npc).flip ? (SpriteEffects) 1 : (SpriteEffects) 0;
    return this.SpriteIntersectsPixel(tile, position, spriteArea, sprite.Texture, sprite.sourceRect, spriteEffects);
  }
}
