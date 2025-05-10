// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.Lookups.TerrainFeatures.TreeTarget
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Netcode;
using StardewValley.GameData.WildTrees;
using StardewValley.TerrainFeatures;
using System;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework.Lookups.TerrainFeatures;

internal class TreeTarget(
  GameHelper gameHelper,
  Tree value,
  Vector2 tilePosition,
  Func<ISubject> getSubject) : GenericTarget<Tree>(gameHelper, SubjectType.WildTree, value, tilePosition, getSubject)
{
  public override Rectangle GetSpritesheetArea()
  {
    Tree tree = this.Value;
    if (((NetFieldBase<bool, NetBool>) tree.stump).Value)
      return Tree.stumpSourceRect;
    if (((NetFieldBase<int, NetInt>) tree.growthStage).Value >= 5)
      return Tree.treeTopSourceRect;
    Rectangle spritesheetArea;
    switch ((int) (WildTreeGrowthStage) ((NetFieldBase<int, NetInt>) tree.growthStage).Value)
    {
      case 0:
        spritesheetArea = new Rectangle(32 /*0x20*/, 128 /*0x80*/, 16 /*0x10*/, 16 /*0x10*/);
        break;
      case 1:
        spritesheetArea = new Rectangle(0, 128 /*0x80*/, 16 /*0x10*/, 16 /*0x10*/);
        break;
      case 2:
        spritesheetArea = new Rectangle(16 /*0x10*/, 128 /*0x80*/, 16 /*0x10*/, 16 /*0x10*/);
        break;
      default:
        spritesheetArea = new Rectangle(0, 96 /*0x60*/, 16 /*0x10*/, 32 /*0x20*/);
        break;
    }
    return spritesheetArea;
  }

  public override Rectangle GetWorldArea()
  {
    return this.GetSpriteArea(((TerrainFeature) this.Value).getBoundingBox(), this.GetSpritesheetArea());
  }

  public override bool SpriteIntersectsPixel(Vector2 tile, Vector2 position, Rectangle spriteArea)
  {
    Tree tree = this.Value;
    WildTreeGrowthStage wildTreeGrowthStage = (WildTreeGrowthStage) ((NetFieldBase<int, NetInt>) tree.growthStage).Value;
    Texture2D spriteSheet = tree.texture.Value;
    SpriteEffects spriteEffects = ((NetFieldBase<bool, NetBool>) tree.flipped).Value ? (SpriteEffects) 1 : (SpriteEffects) 0;
    if (this.SpriteIntersectsPixel(tile, position, spriteArea, spriteSheet, this.GetSpritesheetArea(), spriteEffects))
      return true;
    if (wildTreeGrowthStage == 5)
    {
      Rectangle spriteArea1;
      // ISSUE: explicit constructor call
      ((Rectangle) ref spriteArea1).\u002Ector(((Rectangle) ref spriteArea).Center.X - Tree.stumpSourceRect.Width / 2 * 4, spriteArea.Y + spriteArea.Height - Tree.stumpSourceRect.Height * 4, Tree.stumpSourceRect.Width * 4, Tree.stumpSourceRect.Height * 4);
      if (((Rectangle) ref spriteArea1).Contains((int) position.X, (int) position.Y) && this.SpriteIntersectsPixel(tile, position, spriteArea1, spriteSheet, Tree.stumpSourceRect, spriteEffects))
        return true;
    }
    return false;
  }
}
