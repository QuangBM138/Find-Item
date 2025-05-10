// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.Lookups.TerrainFeatures.BushTarget
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Netcode;
using StardewValley.TerrainFeatures;
using System;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework.Lookups.TerrainFeatures;

internal class BushTarget(GameHelper gameHelper, Bush value, Func<ISubject> getSubject) : 
  GenericTarget<Bush>(gameHelper, SubjectType.Bush, value, ((TerrainFeature) value).Tile, getSubject)
{
  public override Rectangle GetSpritesheetArea()
  {
    return ((NetFieldBase<Rectangle, NetRectangle>) this.Value.sourceRect).Value;
  }

  public override Rectangle GetWorldArea()
  {
    return this.GetSpriteArea(((TerrainFeature) this.Value).getBoundingBox(), this.GetSpritesheetArea());
  }

  public override bool SpriteIntersectsPixel(Vector2 tile, Vector2 position, Rectangle spriteArea)
  {
    SpriteEffects spriteEffects = ((NetFieldBase<bool, NetBool>) this.Value.flipped).Value ? (SpriteEffects) 1 : (SpriteEffects) 0;
    return this.SpriteIntersectsPixel(tile, position, spriteArea, Bush.texture.Value, this.GetSpritesheetArea(), spriteEffects);
  }
}
