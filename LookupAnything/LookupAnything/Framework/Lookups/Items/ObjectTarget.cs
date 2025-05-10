// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.Lookups.Items.ObjectTarget
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Netcode;
using Pathoschild.Stardew.Common;
using StardewValley;
using StardewValley.Objects;
using System;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework.Lookups.Items;

internal class ObjectTarget : GenericTarget<Object>
{
  private readonly SpriteInfo? CustomSprite;

  public ObjectTarget(
    GameHelper gameHelper,
    Object value,
    Vector2 tilePosition,
    Func<ISubject> getSubject)
    : base(gameHelper, SubjectType.Object, value, tilePosition, getSubject)
  {
    this.CustomSprite = gameHelper.GetSprite((Item) value, true);
  }

  public override Rectangle GetSpritesheetArea()
  {
    if (this.CustomSprite != null)
      return this.CustomSprite.SourceRectangle;
    Object @object = this.Value;
    Rectangle spritesheetArea;
    switch (@object)
    {
      case Fence fence:
        spritesheetArea = this.GetSpritesheetArea(fence, Game1.currentLocation);
        break;
      case Furniture furniture:
        spritesheetArea = ((NetFieldBase<Rectangle, NetRectangle>) furniture.sourceRect).Value;
        break;
      default:
        spritesheetArea = ItemRegistry.GetDataOrErrorItem(((Item) @object).QualifiedItemId).GetSourceRect(0, new int?());
        break;
    }
    return spritesheetArea;
  }

  public override Rectangle GetWorldArea()
  {
    Rectangle boundingBox = this.Value.GetBoundingBox();
    if (this.CustomSprite == null)
      return this.GetSpriteArea(boundingBox, this.GetSpritesheetArea());
    Rectangle spriteArea = this.GetSpriteArea(boundingBox, this.CustomSprite.SourceRectangle);
    return new Rectangle(spriteArea.X, spriteArea.Y - spriteArea.Height / 2, spriteArea.Width, spriteArea.Height);
  }

  public override bool SpriteIntersectsPixel(Vector2 tile, Vector2 position, Rectangle spriteArea)
  {
    Object @object = this.Value;
    Texture2D spriteSheet = this.CustomSprite == null ? (!(@object is Fence fence) ? ItemRegistry.GetDataOrErrorItem(((Item) @object).QualifiedItemId).GetTexture() : fence.fenceTexture.Value) : this.CustomSprite.Spritesheet;
    Rectangle spritesheetArea = this.GetSpritesheetArea();
    SpriteEffects spriteEffects = @object.Flipped ? (SpriteEffects) 1 : (SpriteEffects) 0;
    return this.SpriteIntersectsPixel(tile, position, spriteArea, spriteSheet, spritesheetArea, spriteEffects);
  }

  private Rectangle GetSpritesheetArea(Fence fence, GameLocation location)
  {
    int num = 1;
    if ((double) ((NetFieldBase<float, NetFloat>) fence.health).Value > 1.0)
    {
      int key = 0;
      Vector2 tileLocation = ((Object) fence).TileLocation;
      ++tileLocation.X;
      if (location.objects.ContainsKey(tileLocation) && location.objects[tileLocation] is Fence && ((Fence) location.objects[tileLocation]).countsForDrawing(((Item) fence).ItemId))
        key += 100;
      tileLocation.X -= 2f;
      if (location.objects.ContainsKey(tileLocation) && location.objects[tileLocation] is Fence && ((Fence) location.objects[tileLocation]).countsForDrawing(((Item) fence).ItemId))
        key += 10;
      ++tileLocation.X;
      ++tileLocation.Y;
      if (location.objects.ContainsKey(tileLocation) && location.objects[tileLocation] is Fence && ((Fence) location.objects[tileLocation]).countsForDrawing(((Item) fence).ItemId))
        key += 500;
      tileLocation.Y -= 2f;
      if (location.objects.ContainsKey(tileLocation) && location.objects[tileLocation] is Fence && ((Fence) location.objects[tileLocation]).countsForDrawing(((Item) fence).ItemId))
        key += 1000;
      if (((NetFieldBase<bool, NetBool>) fence.isGate).Value)
      {
        switch (key)
        {
          case 110:
            return new Rectangle(((NetFieldBase<int, NetInt>) fence.gatePosition).Value == 88 ? 24 : 0, 128 /*0x80*/, 24, 32 /*0x20*/);
          case 1500:
            return new Rectangle(((NetFieldBase<int, NetInt>) fence.gatePosition).Value == 0 ? 16 /*0x10*/ : 0, 160 /*0xA0*/, 16 /*0x10*/, 16 /*0x10*/);
          default:
            num = 17;
            break;
        }
      }
      else
        num = Fence.fenceDrawGuide[key];
    }
    Texture2D texture2D = fence.fenceTexture.Value;
    return new Rectangle(num * Fence.fencePieceWidth % texture2D.Bounds.Width, num * Fence.fencePieceWidth / texture2D.Bounds.Width * Fence.fencePieceHeight, Fence.fencePieceWidth, Fence.fencePieceHeight);
  }
}
