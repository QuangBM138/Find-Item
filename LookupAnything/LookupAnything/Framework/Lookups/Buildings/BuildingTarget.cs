// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.Lookups.Buildings.BuildingTarget
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Microsoft.Xna.Framework;
using Netcode;
using StardewValley;
using StardewValley.Buildings;
using System;
using System.Collections.Generic;
using System.Linq;
using xTile.Dimensions;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework.Lookups.Buildings;

internal class BuildingTarget : GenericTarget<Building>
{
  private readonly Rectangle TileArea;
  private static readonly IDictionary<string, Rectangle[]> SpriteCollisionOverrides = (IDictionary<string, Rectangle[]>) new Dictionary<string, Rectangle[]>()
  {
    ["Barn"] = new Rectangle[1]
    {
      new Rectangle(48 /*0x30*/, 90, 32 /*0x20*/, 22)
    },
    ["Big Barn"] = new Rectangle[1]
    {
      new Rectangle(64 /*0x40*/, 90, 32 /*0x20*/, 22)
    },
    ["Deluxe Barn"] = new Rectangle[1]
    {
      new Rectangle(64 /*0x40*/, 90, 32 /*0x20*/, 22)
    },
    ["Coop"] = new Rectangle[1]
    {
      new Rectangle(33, 97, 14, 15)
    },
    ["Big Coop"] = new Rectangle[1]
    {
      new Rectangle(33, 97, 14, 15)
    },
    ["Deluxe Coop"] = new Rectangle[1]
    {
      new Rectangle(33, 97, 14, 15)
    },
    ["Fish Pond"] = new Rectangle[1]
    {
      new Rectangle(12, 12, 56, 56)
    }
  };

  public BuildingTarget(GameHelper gameHelper, Building value, Func<ISubject> getSubject)
    : base(gameHelper, SubjectType.Building, value, new Vector2((float) ((NetFieldBase<int, NetInt>) value.tileX).Value, (float) ((NetFieldBase<int, NetInt>) value.tileY).Value), getSubject)
  {
    this.TileArea = new Rectangle(((NetFieldBase<int, NetInt>) value.tileX).Value, ((NetFieldBase<int, NetInt>) value.tileY).Value, ((NetFieldBase<int, NetInt>) value.tilesWide).Value, ((NetFieldBase<int, NetInt>) value.tilesHigh).Value);
  }

  public override Rectangle GetSpritesheetArea()
  {
    return this.Value.getSourceRectForMenu() ?? this.Value.getSourceRect();
  }

  public override Rectangle GetWorldArea()
  {
    Rectangle spritesheetArea = this.GetSpritesheetArea();
    // ISSUE: explicit constructor call
    ((Rectangle) ref spritesheetArea).\u002Ector(spritesheetArea.X * 4, spritesheetArea.Y * 4, spritesheetArea.Width * 4, spritesheetArea.Height * 4);
    Rectangle rectangle;
    // ISSUE: explicit constructor call
    ((Rectangle) ref rectangle).\u002Ector(this.TileArea.X * 64 /*0x40*/, this.TileArea.Y * 64 /*0x40*/, this.TileArea.Width * 64 /*0x40*/, this.TileArea.Height * 64 /*0x40*/);
    return new Rectangle(rectangle.X - (spritesheetArea.Width - rectangle.Width + 1) - ((Rectangle) ref Game1.uiViewport).X, rectangle.Y - (spritesheetArea.Height - rectangle.Height + 1) - ((Rectangle) ref Game1.uiViewport).Y, Math.Max(rectangle.Width, spritesheetArea.Width), Math.Max(rectangle.Height, spritesheetArea.Height));
  }

  public override bool SpriteIntersectsPixel(Vector2 tile, Vector2 position, Rectangle spriteArea)
  {
    Rectangle spritesheetArea = this.GetSpritesheetArea();
    if (this.SpriteIntersectsPixel(tile, position, spriteArea, this.Value.texture.Value, spritesheetArea))
      return true;
    Rectangle[] source;
    if (!BuildingTarget.SpriteCollisionOverrides.TryGetValue(((NetFieldBase<string, NetString>) this.Value.buildingType).Value, out source))
      return false;
    Vector2 spriteSheetPosition = this.GameHelper.GetSpriteSheetCoordinates(position, spriteArea, spritesheetArea);
    return ((IEnumerable<Rectangle>) source).Any<Rectangle>((Func<Rectangle, bool>) (p => ((Rectangle) ref p).Contains((int) spriteSheetPosition.X, (int) spriteSheetPosition.Y)));
  }
}
