// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.Lookups.Buildings.BuildingLookupProvider
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Microsoft.Xna.Framework;
using Netcode;
using Pathoschild.Stardew.LookupAnything.Framework.Constants;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Buildings;
using System;
using System.Collections.Generic;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework.Lookups.Buildings;

internal class BuildingLookupProvider : BaseLookupProvider
{
  private readonly Func<ModConfig> Config;
  private readonly ISubjectRegistry Codex;

  public BuildingLookupProvider(
    IReflectionHelper reflection,
    GameHelper gameHelper,
    Func<ModConfig> config,
    ISubjectRegistry codex)
    : base(reflection, gameHelper)
  {
    this.Config = config;
    this.Codex = codex;
  }

  public override IEnumerable<ITarget> GetTargets(GameLocation location, Vector2 lookupTile)
  {
    BuildingLookupProvider buildingLookupProvider1 = this;
    foreach (Building building1 in location.buildings)
    {
      BuildingLookupProvider buildingLookupProvider = buildingLookupProvider1;
      Building building = building1;
      Vector2 spriteTile;
      // ISSUE: explicit constructor call
      ((Vector2) ref spriteTile).\u002Ector((float) ((NetFieldBase<int, NetInt>) building.tileX).Value, (float) (((NetFieldBase<int, NetInt>) building.tileY).Value + ((NetFieldBase<int, NetInt>) building.tilesHigh).Value));
      if (buildingLookupProvider1.GameHelper.CouldSpriteOccludeTile(spriteTile, lookupTile, new Vector2?(Constant.MaxBuildingTargetSpriteSize)))
        yield return (ITarget) new BuildingTarget(buildingLookupProvider1.GameHelper, building, (Func<ISubject>) (() => buildingLookupProvider.BuildSubject(building)));
    }
  }

  public override IEnumerable<ISubject> GetSearchSubjects()
  {
    foreach (string key in (IEnumerable<string>) Game1.buildingData.Keys)
    {
      Building building;
      try
      {
        building = new Building(key, Vector2.Zero);
      }
      catch
      {
        continue;
      }
      yield return this.BuildSubject(building);
    }
  }

  public override ISubject? GetSubjectFor(object entity, GameLocation? location)
  {
    return !(entity is Building building) ? (ISubject) null : this.BuildSubject(building);
  }

  private ISubject BuildSubject(Building building)
  {
    ModConfig modConfig = this.Config();
    return (ISubject) new BuildingSubject(this.Codex, this.GameHelper, building, building.getSourceRectForMenu() ?? building.getSourceRect(), modConfig.CollapseLargeFields, modConfig.ShowInvalidRecipes);
  }
}
