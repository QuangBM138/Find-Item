// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.Lookups.Characters.FarmerTarget
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Microsoft.Xna.Framework;
using StardewValley;
using System;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework.Lookups.Characters;

internal class FarmerTarget(GameHelper gameHelper, Farmer value, Func<ISubject> getSubject) : 
  GenericTarget<Farmer>(gameHelper, SubjectType.Farmer, value, ((Character) value).Tile, getSubject)
{
  public override Rectangle GetSpritesheetArea()
  {
    return ((AnimatedSprite) this.Value.FarmerSprite).SourceRect;
  }

  public override Rectangle GetWorldArea()
  {
    return this.GetSpriteArea(((Character) this.Value).GetBoundingBox(), this.GetSpritesheetArea());
  }

  public override bool SpriteIntersectsPixel(Vector2 tile, Vector2 position, Rectangle spriteArea)
  {
    return ((Rectangle) ref spriteArea).Contains((int) position.X, (int) position.Y);
  }
}
