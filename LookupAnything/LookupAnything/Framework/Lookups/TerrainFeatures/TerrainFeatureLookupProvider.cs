// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.Lookups.TerrainFeatures.TerrainFeatureLookupProvider
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Microsoft.Xna.Framework;
using Netcode;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Network;
using StardewValley.TerrainFeatures;
using System;
using System.Collections.Generic;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework.Lookups.TerrainFeatures;

internal class TerrainFeatureLookupProvider : BaseLookupProvider
{
  private readonly ISubjectRegistry Codex;

  public TerrainFeatureLookupProvider(
    IReflectionHelper reflection,
    GameHelper gameHelper,
    ISubjectRegistry codex)
    : base(reflection, gameHelper)
  {
    this.Codex = codex;
  }

  public override IEnumerable<ITarget> GetTargets(GameLocation location, Vector2 lookupTile)
  {
    TerrainFeatureLookupProvider featureLookupProvider1 = this;
    foreach (KeyValuePair<Vector2, TerrainFeature> pair in ((NetDictionary<Vector2, TerrainFeature, NetRef<TerrainFeature>, SerializableDictionary<Vector2, TerrainFeature>, NetVector2Dictionary<TerrainFeature, NetRef<TerrainFeature>>>) location.terrainFeatures).Pairs)
    {
      TerrainFeatureLookupProvider featureLookupProvider = featureLookupProvider1;
      (Vector2 vector2, TerrainFeature terrainFeature) = pair;
      if (featureLookupProvider1.GameHelper.CouldSpriteOccludeTile(vector2, lookupTile))
      {
        FruitTree tree1 = terrainFeature as FruitTree;
        if (tree1 == null)
        {
          Tree tree2 = terrainFeature as Tree;
          if (tree2 == null)
          {
            Bush bush = terrainFeature as Bush;
            if (bush != null)
              yield return (ITarget) new BushTarget(featureLookupProvider1.GameHelper, bush, (Func<ISubject>) (() => featureLookupProvider.BuildSubject(bush)));
          }
          else if ((double) tree2.alpha >= 0.800000011920929)
            yield return (ITarget) new TreeTarget(featureLookupProvider1.GameHelper, tree2, vector2, (Func<ISubject>) (() => featureLookupProvider.BuildSubject(tree2, vector2)));
        }
        else if ((double) tree1.alpha >= 0.800000011920929)
          yield return (ITarget) new FruitTreeTarget(featureLookupProvider1.GameHelper, tree1, vector2, (Func<ISubject>) (() => featureLookupProvider.BuildSubject(tree1, vector2)));
      }
    }
    foreach (LargeTerrainFeature largeTerrainFeature in location.largeTerrainFeatures)
    {
      Vector2 tile = ((TerrainFeature) largeTerrainFeature).Tile;
      if (featureLookupProvider1.GameHelper.CouldSpriteOccludeTile(tile, lookupTile))
      {
        TerrainFeatureLookupProvider featureLookupProvider = featureLookupProvider1;
        Bush bush = largeTerrainFeature as Bush;
        if (bush != null)
          yield return (ITarget) new BushTarget(featureLookupProvider1.GameHelper, bush, (Func<ISubject>) (() => featureLookupProvider.BuildSubject(bush)));
      }
    }
  }

  public override ISubject? GetSubjectFor(object entity, GameLocation? location)
  {
    return !(entity is Bush bush) ? (ISubject) null : this.BuildSubject(bush);
  }

  private ISubject BuildSubject(Bush bush)
  {
    return (ISubject) new BushSubject(this.GameHelper, this.Codex, bush);
  }

  private ISubject BuildSubject(FruitTree tree, Vector2 tile)
  {
    return (ISubject) new FruitTreeSubject(this.GameHelper, tree, tile);
  }

  private ISubject BuildSubject(Tree tree, Vector2 tile)
  {
    return (ISubject) new TreeSubject(this.Codex, this.GameHelper, tree, tile);
  }
}
