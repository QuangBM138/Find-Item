// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.Lookups.Tiles.TileSubject
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pathoschild.Stardew.LookupAnything.Framework.DebugFields;
using Pathoschild.Stardew.LookupAnything.Framework.Fields;
using StardewValley;
using StardewValley.GameData.Locations;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using xTile.Layers;
using xTile.ObjectModel;
using xTile.Tiles;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework.Lookups.Tiles;

internal class TileSubject : BaseSubject
{
  protected readonly ModConfig Config;
  protected readonly GameLocation Location;
  protected readonly Vector2 Position;
  protected readonly bool ShowRawTileInfo;

  public TileSubject(
    GameHelper gameHelper,
    ModConfig config,
    GameLocation location,
    Vector2 position,
    bool showRawTileInfo)
    : base(gameHelper, I18n.Tile_Title((object) position.X, (object) position.Y), showRawTileInfo ? I18n.Tile_Description() : (string) null, (string) null)
  {
    this.Config = config;
    this.Location = location;
    this.Position = position;
    this.ShowRawTileInfo = showRawTileInfo;
  }

  public static bool TryCreate(
    GameHelper gameHelper,
    ModConfig config,
    GameLocation location,
    Vector2 position,
    bool showRawTileInfo,
    [NotNullWhen(true)] out TileSubject? tileSubject)
  {
    tileSubject = new TileSubject(gameHelper, config, location, position, showRawTileInfo);
    if (tileSubject.GetData().Any<ICustomField>())
      return true;
    tileSubject = (TileSubject) null;
    return false;
  }

  public override IEnumerable<ICustomField> GetData()
  {
    TileSubject tileSubject = this;
    StringBuilder summary;
    if (tileSubject.ShowRawTileInfo)
    {
      yield return (ICustomField) new GenericField(I18n.Tile_MapName(), tileSubject.Location.Name);
      if (((ICollection<KeyValuePair<string, PropertyValue>>) ((Component) tileSubject.Location.Map).Properties).Count > 0)
      {
        summary = new StringBuilder();
        foreach ((string str1, PropertyValue propertyValue) in (IEnumerable<KeyValuePair<string, PropertyValue>>) ((Component) tileSubject.Location.Map).Properties)
        {
          string str2 = PropertyValue.op_Implicit(propertyValue);
          summary.AppendLine(I18n.Tile_MapProperties_Value((object) str1, (object) str2));
        }
        GenericField genericField = new GenericField(I18n.Tile_MapProperties(), summary.ToString());
        if (summary.Length > 50)
          genericField.CollapseByDefault(I18n.Generic_ShowXResults((object) ((ICollection<KeyValuePair<string, PropertyValue>>) ((Component) tileSubject.Location.Map).Properties).Count));
        yield return (ICustomField) genericField;
        summary.Clear();
        summary = (StringBuilder) null;
      }
    }
    if (TileSubject.IsFishingArea(tileSubject.Location, tileSubject.Position))
    {
      string fishAreaId;
      FishAreaData fishAreaData;
      tileSubject.Location.TryGetFishAreaForTile(tileSubject.Position, ref fishAreaId, ref fishAreaData);
      FishSpawnRulesField fishSpawnRulesField = new FishSpawnRulesField(tileSubject.GameHelper, I18n.Item_FishSpawnRules(), tileSubject.Location, tileSubject.Position, fishAreaId, tileSubject.Config.ShowUncaughtFishSpawnRules);
      if (fishSpawnRulesField.HasValue)
        yield return (ICustomField) fishSpawnRulesField;
    }
    if (tileSubject.ShowRawTileInfo)
    {
      Tile[] array = tileSubject.GetTiles(tileSubject.Location, tileSubject.Position).ToArray<Tile>();
      if (!((IEnumerable<Tile>) array).Any<Tile>())
      {
        yield return (ICustomField) new GenericField(I18n.Tile_LayerTileNone(), I18n.Tile_LayerTile_NoneHere());
      }
      else
      {
        summary = new StringBuilder();
        Tile[] tileArray = array;
        for (int index = 0; index < tileArray.Length; ++index)
        {
          Tile tile = tileArray[index];
          summary.AppendLine(I18n.Tile_LayerTile_Appearance((object) tileSubject.Stringify((object) tile.TileIndex), (object) ((Component) tile.TileSheet).Id, (object) tile.TileSheet.ImageSource.Replace("\\", ": ").Replace("/", ": ")));
          summary.AppendLine();
          if (tile.BlendMode != null)
            summary.AppendLine(I18n.Tile_LayerTile_BlendMode((object) tileSubject.Stringify((object) tile.BlendMode)));
          foreach ((string str5, PropertyValue propertyValue) in (IEnumerable<KeyValuePair<string, PropertyValue>>) ((Component) tile).Properties)
          {
            string str4 = PropertyValue.op_Implicit(propertyValue);
            summary.AppendLine(I18n.Tile_LayerTile_TileProperty((object) str5, (object) str4));
          }
          foreach ((str5, propertyValue) in (IEnumerable<KeyValuePair<string, PropertyValue>>) tile.TileIndexProperties)
          {
            string str6 = PropertyValue.op_Implicit(propertyValue);
            summary.AppendLine(I18n.Tile_LayerTile_IndexProperty((object) str5, (object) str6));
          }
          yield return (ICustomField) new GenericField(I18n.Tile_LayerTile((object) ((Component) tile.Layer).Id), summary.ToString().TrimEnd());
          summary.Clear();
        }
        tileArray = (Tile[]) null;
        summary = (StringBuilder) null;
      }
    }
  }

  public override IEnumerable<IDebugField> GetDebugFields()
  {
    TileSubject tileSubject = this;
    string mapTileLabel = "map tile";
    string locationLabel = I18n.Tile_GameLocation();
    Tile[] tileArray = tileSubject.GetTiles(tileSubject.Location, tileSubject.Position).ToArray<Tile>();
    for (int index = 0; index < tileArray.Length; ++index)
    {
      Tile tile = tileArray[index];
      foreach (IDebugField debugField in tileSubject.GetDebugFieldsFrom((object) tile))
        yield return (IDebugField) new GenericDebugField($"{((Component) tile.Layer).Id}::{debugField.Label}", debugField.Value, new bool?(debugField.HasValue))
        {
          OverrideCategory = mapTileLabel
        };
      tile = (Tile) null;
    }
    tileArray = (Tile[]) null;
    foreach (IDebugField debugField in tileSubject.GetDebugFieldsFrom((object) tileSubject.Location))
      yield return (IDebugField) new GenericDebugField(debugField.Label, debugField.Value, new bool?(debugField.HasValue), debugField.IsPinned)
      {
        OverrideCategory = locationLabel
      };
  }

  public override bool DrawPortrait(SpriteBatch spriteBatch, Vector2 position, Vector2 size)
  {
    return false;
  }

  public static bool IsFishingArea(GameLocation location, Vector2 tile)
  {
    return location.isTileFishable((int) tile.X, (int) tile.Y);
  }

  private IEnumerable<Tile> GetTiles(GameLocation location, Vector2 position)
  {
    if ((double) position.X >= 0.0 && (double) position.Y >= 0.0)
    {
      foreach (Layer layer in location.map.Layers)
      {
        if ((double) position.X <= (double) layer.LayerWidth && (double) position.Y <= (double) layer.LayerHeight)
        {
          Tile tile = layer.Tiles[(int) position.X, (int) position.Y];
          if (tile != null)
            yield return tile;
        }
      }
    }
  }
}
