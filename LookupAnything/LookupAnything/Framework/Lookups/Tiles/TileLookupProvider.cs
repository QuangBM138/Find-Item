// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.Lookups.Tiles.TileLookupProvider
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Locations;
using StardewValley.Objects;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework.Lookups.Tiles;

internal class TileLookupProvider : BaseLookupProvider
{
  private readonly Func<ModConfig> Config;
  private readonly Func<bool> ShowRawTileInfo;

  public TileLookupProvider(
    IReflectionHelper reflection,
    GameHelper gameHelper,
    Func<ModConfig> config,
    Func<bool> showRawTileInfo)
    : base(reflection, gameHelper)
  {
    this.Config = config;
    this.ShowRawTileInfo = showRawTileInfo;
  }

  public override IEnumerable<ITarget> GetTargets(GameLocation location, Vector2 lookupTile)
  {
    TileLookupProvider tileLookupProvider = this;
    ISubject subject = tileLookupProvider.BuildSubject(location, lookupTile);
    if (subject != null)
      yield return (ITarget) new TileTarget(tileLookupProvider.GameHelper, lookupTile, (Func<ISubject>) (() => subject));
  }

  private ISubject? BuildSubject(GameLocation location, Vector2 tile)
  {
    bool showRawTileInfo = this.ShowRawTileInfo();
    ModConfig config = this.Config();
    int? crystalId;
    if (this.IsCrystalCavePuzzle(location, tile, out crystalId))
      return (ISubject) new CrystalCavePuzzleSubject(this.GameHelper, config, location, tile, showRawTileInfo, crystalId);
    if (this.GetIsIslandMermaidPuzzle(location, tile))
      return (ISubject) new IslandMermaidPuzzleSubject(this.GameHelper, config, location, tile, showRawTileInfo);
    if (this.IsIslandShrinePuzzle(location, tile))
      return (ISubject) new IslandShrinePuzzleSubject(this.GameHelper, config, location, tile, showRawTileInfo);
    TileSubject tileSubject;
    return TileSubject.TryCreate(this.GameHelper, config, location, tile, showRawTileInfo, out tileSubject) ? (ISubject) tileSubject : (ISubject) null;
  }

  private bool IsCrystalCavePuzzle(GameLocation location, Vector2 tile, out int? crystalId)
  {
    crystalId = new int?();
    if (location is IslandWestCave1)
    {
      string[] arguments;
      if (this.HasTileProperty(location, tile, "Action", "Buildings", out arguments) && ((IEnumerable<string>) arguments).Any<string>())
      {
        switch (arguments[0])
        {
          case "CrystalCaveActivate":
            return true;
          case "Crystal":
            int result;
            if (arguments.Length > 1 && int.TryParse(arguments[1], out result))
              crystalId = new int?(result);
            return true;
        }
      }
      else if (location.getTileIndexAt((int) tile.X, (int) tile.Y, "Buildings", (string) null) == 31 /*0x1F*/)
        return true;
    }
    return false;
  }

  private bool GetIsIslandMermaidPuzzle(GameLocation location, Vector2 tile)
  {
    if (location is IslandSouthEast islandSouthEast && islandSouthEast.MermaidIsHere())
    {
      float x = tile.X;
      if ((double) x >= 32.0 && (double) x <= 33.0)
      {
        float y = tile.Y;
        return (double) y >= 31.0 && (double) y <= 33.0;
      }
    }
    return false;
  }

  private bool IsIslandShrinePuzzle(GameLocation location, Vector2 tile)
  {
    if (!(location is IslandShrine))
      return false;
    float x = tile.X;
    if ((double) x >= 23.0 && (double) x <= 25.0)
    {
      float y = tile.Y;
      if ((double) y >= 20.0 && (double) y <= 22.0)
        return true;
    }
    Object @object;
    return location.objects.TryGetValue(tile, ref @object) && @object is ItemPedestal;
  }

  private bool HasTileProperty(
    GameLocation location,
    Vector2 tile,
    string name,
    string layer,
    out string[] arguments)
  {
    string str;
    bool flag = this.HasTileProperty(location, tile, name, layer, out str);
    arguments = (str != null ? ((IEnumerable<string>) str.Split(' ')).ToArray<string>() : (string[]) null) ?? Array.Empty<string>();
    return flag;
  }

  private bool HasTileProperty(
    GameLocation location,
    Vector2 tile,
    string name,
    string layer,
    [NotNullWhen(true)] out string? value)
  {
    value = location.doesTileHaveProperty((int) tile.X, (int) tile.Y, name, layer, false);
    return value != null;
  }
}
