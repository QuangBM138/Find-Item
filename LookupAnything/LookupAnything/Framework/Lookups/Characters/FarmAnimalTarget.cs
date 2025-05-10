// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.Lookups.Characters.FarmAnimalTarget
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using System;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework.Lookups.Characters;

internal class FarmAnimalTarget(
  GameHelper gameHelper,
  FarmAnimal value,
  Vector2 tilePosition,
  Func<ISubject> getSubject) : GenericTarget<FarmAnimal>(gameHelper, SubjectType.FarmAnimal, value, tilePosition, getSubject)
{
  public override Rectangle GetSpritesheetArea() => ((Character) this.Value).Sprite.SourceRect;

  public override Rectangle GetWorldArea()
  {
    return this.GetSpriteArea(((Character) this.Value).GetBoundingBox(), this.GetSpritesheetArea());
  }

  public override bool SpriteIntersectsPixel(Vector2 tile, Vector2 position, Rectangle spriteArea)
  {
    SpriteEffects spriteEffects = ((Character) this.Value).flip ? (SpriteEffects) 1 : (SpriteEffects) 0;
    return this.SpriteIntersectsPixel(tile, position, spriteArea, ((Character) this.Value).Sprite.Texture, this.GetSpritesheetArea(), spriteEffects);
  }
}
