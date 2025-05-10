// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.Lookups.TerrainFeatures.TreeSubject
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Netcode;
using Pathoschild.Stardew.Common;
using Pathoschild.Stardew.LookupAnything.Framework.DebugFields;
using Pathoschild.Stardew.LookupAnything.Framework.Fields;
using StardewModdingAPI;
using StardewValley;
using StardewValley.GameData.WildTrees;
using StardewValley.Network;
using StardewValley.TerrainFeatures;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework.Lookups.TerrainFeatures;

internal class TreeSubject : BaseSubject
{
  private readonly Tree Target;
  private readonly Vector2 Tile;
  private readonly ISubjectRegistry Codex;

  public TreeSubject(ISubjectRegistry codex, GameHelper gameHelper, Tree tree, Vector2 tile)
    : base(gameHelper, TreeSubject.GetName(tree), (string) null, I18n.Type_Tree())
  {
    this.Codex = codex;
    this.Target = tree;
    this.Tile = tile;
  }

  public override IEnumerable<ICustomField> GetData()
  {
    TreeSubject treeSubject = this;
    Tree tree = treeSubject.Target;
    WildTreeData data = tree.GetData();
    GameLocation location = ((TerrainFeature) tree).Location;
    bool isFertilized = ((NetFieldBase<bool, NetBool>) tree.fertilized).Value;
    IModInfo modFromStringId = treeSubject.GameHelper.TryGetModFromStringId(((NetFieldBase<string, NetString>) tree.treeType).Value);
    if (modFromStringId != null)
      yield return (ICustomField) new GenericField(I18n.AddedByMod(), I18n.AddedByMod_Summary((object) modFromStringId.Manifest.Name));
    WildTreeGrowthStage stage = (WildTreeGrowthStage) Math.Min(((NetFieldBase<int, NetInt>) tree.growthStage).Value, 5);
    bool isFullyGrown = stage == 5;
    yield return (ICustomField) new GenericField(I18n.Tree_Stage(), isFullyGrown ? I18n.Tree_Stage_Done() : I18n.Tree_Stage_Partial((object) I18n.For(stage), (object) (int) stage, (object) 5));
    if (!isFullyGrown)
    {
      string label1 = I18n.Tree_NextGrowth();
      if (!data.GrowsInWinter && location.GetSeason() == 3 && !location.SeedsIgnoreSeasonsHere() && !isFertilized)
        yield return (ICustomField) new GenericField(label1, I18n.Tree_NextGrowth_Winter());
      else if (stage == 4 && treeSubject.HasAdjacentTrees(treeSubject.Tile))
      {
        yield return (ICustomField) new GenericField(label1, I18n.Tree_NextGrowth_AdjacentTrees());
      }
      else
      {
        double chance = Math.Round((isFertilized ? (double) data.FertilizedGrowthChance : (double) data.GrowthChance) * 100.0, 2);
        string label2 = label1;
        object stage1 = (object) I18n.For((WildTreeGrowthStage) (stage + 1));
        string str = I18n.Tree_NextGrowth_Chance((object) chance, stage1);
        bool? hasValue = new bool?();
        yield return (ICustomField) new GenericField(label2, str, hasValue);
      }
    }
    if (!isFullyGrown)
    {
      if (!isFertilized)
      {
        yield return (ICustomField) new GenericField(I18n.Tree_IsFertilized(), treeSubject.Stringify((object) false));
      }
      else
      {
        Item obj = ItemRegistry.Create("(O)805", 1, 0, false);
        yield return (ICustomField) new ItemIconField(treeSubject.GameHelper, I18n.Tree_IsFertilized(), obj, treeSubject.Codex);
      }
    }
    if (isFullyGrown && !string.IsNullOrWhiteSpace(data.SeedItemId))
    {
      string objectName = GameI18n.GetObjectName(data.SeedItemId);
      if (((NetFieldBase<bool, NetBool>) tree.hasSeed).Value)
      {
        yield return (ICustomField) new ItemIconField(treeSubject.GameHelper, I18n.Tree_Seed(), ItemRegistry.Create(data.SeedItemId, 1, 0, false), treeSubject.Codex);
      }
      else
      {
        List<string> stringList = new List<string>(2);
        if ((double) data.SeedOnShakeChance > 0.0)
          stringList.Add(I18n.Tree_Seed_ProbabilityDaily((object) (float) ((double) data.SeedOnShakeChance * 100.0), (object) objectName));
        if ((double) data.SeedOnChopChance > 0.0)
          stringList.Add(I18n.Tree_Seed_ProbabilityOnChop((object) (float) ((double) data.SeedOnChopChance * 100.0), (object) objectName));
        if (stringList.Any<string>())
          yield return (ICustomField) new GenericField(I18n.Tree_Seed(), I18n.Tree_Seed_NotReady() + Environment.NewLine + string.Join(Environment.NewLine, (IEnumerable<string>) stringList));
      }
    }
  }

  public override IEnumerable<IDebugField> GetDebugFields()
  {
    TreeSubject treeSubject = this;
    Tree target = treeSubject.Target;
    yield return (IDebugField) new GenericDebugField("has seed", treeSubject.Stringify((object) ((NetFieldBase<bool, NetBool>) target.hasSeed).Value), pinned: true);
    yield return (IDebugField) new GenericDebugField("growth stage", ((NetFieldBase<int, NetInt>) target.growthStage).Value, pinned: true);
    yield return (IDebugField) new GenericDebugField("health", ((NetFieldBase<float, NetFloat>) target.health).Value, pinned: true);
    foreach (IDebugField debugField in treeSubject.GetDebugFieldsFrom((object) target))
      yield return debugField;
  }

  public override bool DrawPortrait(SpriteBatch spriteBatch, Vector2 position, Vector2 size)
  {
    ((TerrainFeature) this.Target).drawInMenu(spriteBatch, position, Vector2.Zero, 1f, 1f);
    return true;
  }

  private static string GetName(Tree tree)
  {
    string str = ((NetFieldBase<string, NetString>) tree.treeType).Value;
    string name;
    if (str != null)
    {
      switch (str.Length)
      {
        case 1:
          switch (str[0])
          {
            case '1':
              name = I18n.Tree_Name_Oak();
              goto label_20;
            case '2':
              name = I18n.Tree_Name_Maple();
              goto label_20;
            case '3':
              name = I18n.Tree_Name_Pine();
              goto label_20;
            case '6':
              name = I18n.Tree_Name_Palm();
              goto label_20;
            case '7':
              name = I18n.Tree_Name_BigMushroom();
              goto label_20;
            case '8':
              name = I18n.Tree_Name_Mahogany();
              goto label_20;
            case '9':
              name = I18n.Tree_Name_Palm();
              goto label_20;
          }
          break;
        case 2:
          switch (str[1])
          {
            case '0':
              if (str == "10")
              {
                name = I18n.Tree_Name_Mossy();
                goto label_20;
              }
              break;
            case '1':
              if (str == "11")
              {
                name = I18n.Tree_Name_Mossy();
                goto label_20;
              }
              break;
            case '2':
              if (str == "12")
              {
                name = I18n.Tree_Name_Mossy();
                goto label_20;
              }
              break;
            case '3':
              if (str == "13")
              {
                name = I18n.Tree_Name_Mystic();
                goto label_20;
              }
              break;
          }
          break;
      }
    }
    name = I18n.Tree_Name_Unknown();
label_20:
    return name;
  }

  private bool HasAdjacentTrees(Vector2 position)
  {
    GameLocation location = Game1.currentLocation;
    return ((IEnumerable<Vector2>) Utility.getSurroundingTileLocationsArray(position)).Select(adjacentTile => new
    {
      adjacentTile = adjacentTile,
      otherTree = ((NetDictionary<Vector2, TerrainFeature, NetRef<TerrainFeature>, SerializableDictionary<Vector2, TerrainFeature>, NetVector2Dictionary<TerrainFeature, NetRef<TerrainFeature>>>) location.terrainFeatures).ContainsKey(adjacentTile) ? ((NetDictionary<Vector2, TerrainFeature, NetRef<TerrainFeature>, SerializableDictionary<Vector2, TerrainFeature>, NetVector2Dictionary<TerrainFeature, NetRef<TerrainFeature>>>) location.terrainFeatures)[adjacentTile] as Tree : (Tree) null
    }).Select(_param1 => _param1.otherTree != null && ((NetFieldBase<int, NetInt>) _param1.otherTree.growthStage).Value >= 4).Any<bool>((Func<bool, bool>) (p => p));
  }
}
