// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.Lookups.Items.CropTarget
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Netcode;
using StardewValley;
using StardewValley.TerrainFeatures;
using System;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework.Lookups.Items;

internal class CropTarget : GenericTarget<HoeDirt>
{
  private readonly Texture2D? Texture;
  private readonly Rectangle SourceRect;

  public CropTarget(
    GameHelper gameHelper,
    HoeDirt value,
    Vector2 tilePosition,
    Func<ISubject> getSubject)
    : base(gameHelper, SubjectType.Crop, value, tilePosition, getSubject)
  {
    this.GetSpriteSheet(value.crop, out this.Texture, out this.SourceRect);
  }

  public override Rectangle GetSpritesheetArea() => this.SourceRect;

  public override Rectangle GetWorldArea()
  {
    return this.GetSpriteArea(((TerrainFeature) this.Value).getBoundingBox(), this.GetSpritesheetArea());
  }

  public override bool SpriteIntersectsPixel(Vector2 tile, Vector2 position, Rectangle spriteArea)
  {
    Crop crop = this.Value.crop;
    SpriteEffects spriteEffects = ((NetFieldBase<bool, NetBool>) crop.flip).Value ? (SpriteEffects) 1 : (SpriteEffects) 0;
    if (this.SpriteIntersectsPixel(tile, position, spriteArea, this.Texture, this.GetSpritesheetArea(), spriteEffects))
      return true;
    return Color.op_Inequality(((NetFieldBase<Color, NetColor>) crop.tintColor).Value, Color.White) && ((NetFieldBase<int, NetInt>) crop.currentPhase).Value == ((NetList<int, NetInt>) crop.phaseDays).Count - 1 && !((NetFieldBase<bool, NetBool>) crop.dead).Value && this.SpriteIntersectsPixel(tile, position, spriteArea, this.Texture, this.SourceRect, spriteEffects);
  }

  private void GetSpriteSheet(Crop target, out Texture2D? texture, out Rectangle sourceRect)
  {
    texture = target.DrawnCropTexture;
    sourceRect = target.sourceRect;
    if (!((NetFieldBase<bool, NetBool>) target.forageCrop).Value || !(((NetFieldBase<string, NetString>) target.whichForageCrop).Value == 2.ToString()))
      return;
    sourceRect = new Rectangle(128 /*0x80*/ + (int) ((Game1.currentGameTime.TotalGameTime.TotalMilliseconds + ((double) this.Tile.X * 111.0 + (double) this.Tile.Y * 77.0)) % 800.0 / 200.0) * 16 /*0x10*/, 128 /*0x80*/, 16 /*0x10*/, 16 /*0x10*/);
  }
}
