// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.TargetFactory
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Microsoft.Xna.Framework;
using Pathoschild.Stardew.LookupAnything.Framework.Constants;
using Pathoschild.Stardew.LookupAnything.Framework.Lookups;
using Pathoschild.Stardew.LookupAnything.Framework.Lookups.Buildings;
using Pathoschild.Stardew.LookupAnything.Framework.Lookups.Characters;
using Pathoschild.Stardew.LookupAnything.Framework.Lookups.Items;
using Pathoschild.Stardew.LookupAnything.Framework.Lookups.TerrainFeatures;
using Pathoschild.Stardew.LookupAnything.Framework.Lookups.Tiles;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework;

internal class TargetFactory : ISubjectRegistry
{
  private const int SubjectCacheDuration = 300;
  private readonly GameHelper GameHelper;
  private readonly ILookupProvider[] LookupProviders;
  private readonly Dictionary<(object, GameLocation?), ISubject?> SubjectCache = new Dictionary<(object, GameLocation), ISubject>();
  private int SubjectCacheUntil;

  public TargetFactory(
    IReflectionHelper reflection,
    GameHelper gameHelper,
    Func<ModConfig> config,
    Func<bool> showRawTileInfo)
  {
    this.GameHelper = gameHelper;
    ISubjectRegistry codex = (ISubjectRegistry) this;
    this.LookupProviders = new ILookupProvider[5]
    {
      (ILookupProvider) new BuildingLookupProvider(reflection, gameHelper, config, codex),
      (ILookupProvider) new CharacterLookupProvider(reflection, gameHelper, config, codex),
      (ILookupProvider) new ItemLookupProvider(reflection, gameHelper, config, codex),
      (ILookupProvider) new TerrainFeatureLookupProvider(reflection, gameHelper, codex),
      (ILookupProvider) new TileLookupProvider(reflection, gameHelper, config, showRawTileInfo)
    };
  }

  public IEnumerable<ITarget> GetNearbyTargets(GameLocation location, Vector2 originTile)
  {
    foreach (ITarget nearbyTarget in ((IEnumerable<ILookupProvider>) this.LookupProviders).SelectMany<ILookupProvider, ITarget>((Func<ILookupProvider, IEnumerable<ITarget>>) (p => p.GetTargets(location, originTile))).WhereNotNull<ITarget>())
      yield return nearbyTarget;
  }

  public ITarget? GetTargetFromTile(GameLocation location, Vector2 tile)
  {
    return this.GetNearbyTargets(location, tile).Where<ITarget>((Func<ITarget, bool>) (target => Vector2.op_Equality(target.Tile, tile))).FirstOrDefault<ITarget>();
  }

  public ITarget? GetTargetFromScreenCoordinate(
    GameLocation location,
    Vector2 tile,
    Vector2 position)
  {
    Rectangle tileArea = this.GameHelper.GetScreenCoordinatesFromTile(tile);
    \u003C\u003Ef__AnonymousType22<ITarget, Rectangle, bool>[] array = this.GetNearbyTargets(location, tile).Select(target => new
    {
      target = target,
      spriteArea = target.GetWorldArea()
    }).Select(_param1 => new
    {
      \u003C\u003Eh__TransparentIdentifier0 = _param1,
      isAtTile = Vector2.op_Equality(_param1.target.Tile, tile)
    }).Where(_param1 =>
    {
      if (_param1.isAtTile)
        return true;
      Rectangle spriteArea = _param1.\u003C\u003Eh__TransparentIdentifier0.spriteArea;
      return ((Rectangle) ref spriteArea).Intersects(tileArea);
    }).OrderBy(_param1 => _param1.\u003C\u003Eh__TransparentIdentifier0.target.Precedence).ThenByDescending(_param1 => _param1.\u003C\u003Eh__TransparentIdentifier0.spriteArea.Y).ThenBy(_param1 => _param1.\u003C\u003Eh__TransparentIdentifier0.spriteArea.X).Select(_param1 => new
    {
      target = _param1.\u003C\u003Eh__TransparentIdentifier0.target,
      spriteArea = _param1.\u003C\u003Eh__TransparentIdentifier0.spriteArea,
      isAtTile = _param1.isAtTile
    }).ToArray();
    ITarget screenCoordinate = (ITarget) null;
    foreach (var data in array)
    {
      try
      {
        if (data.target.SpriteIntersectsPixel(tile, position, data.spriteArea))
          return data.target;
      }
      catch
      {
        if (screenCoordinate == null)
          screenCoordinate = data.target;
      }
    }
    foreach (var data in array)
    {
      if (data.isAtTile)
        return data.target;
    }
    return screenCoordinate;
  }

  public ISubject? GetSubjectFrom(Farmer player, GameLocation location, bool hasCursor)
  {
    ITarget target = hasCursor ? this.GetTargetFromScreenCoordinate(location, Game1.currentCursorTile, this.GameHelper.GetScreenCoordinatesFromCursor()) : this.GetTargetFromTile(location, this.GetFacingTile(player));
    return target == null ? (ISubject) null : target.GetSubject();
  }

  public ISubject? GetSubjectFrom(IClickableMenu menu, Vector2 cursorPos)
  {
    int cursorX = (int) cursorPos.X;
    int cursorY = (int) cursorPos.Y;
    return ((IEnumerable<ILookupProvider>) this.LookupProviders).Select<ILookupProvider, ISubject>((Func<ILookupProvider, ISubject>) (p => p.GetSubject(menu, cursorX, cursorY))).FirstOrDefault<ISubject>((Func<ISubject, bool>) (p => p != null));
  }

  public ISubject? GetByEntity(object entity, GameLocation? location)
  {
    (object, GameLocation) key1 = (entity, location);
    if (this.SubjectCacheUntil < Game1.ticks)
    {
      this.SubjectCache.Clear();
      this.SubjectCacheUntil = Game1.ticks + 300 - 1;
    }
    else
    {
      ISubject byEntity;
      if (this.SubjectCache.TryGetValue(key1, out byEntity))
        return byEntity;
    }
    Dictionary<(object, GameLocation), ISubject> subjectCache = this.SubjectCache;
    (object, GameLocation) key2 = key1;
    IEnumerable<ISubject> source = ((IEnumerable<ILookupProvider>) this.LookupProviders).Select<ILookupProvider, ISubject>((Func<ILookupProvider, ISubject>) (p => p.GetSubjectFor(entity, location)));
    ISubject subject;
    ISubject byEntity1 = subject = source.FirstOrDefault<ISubject>((Func<ISubject, bool>) (p => p != null));
    subjectCache[key2] = subject;
    return byEntity1;
  }

  public IEnumerable<ISubject> GetSearchSubjects()
  {
    return ((IEnumerable<ILookupProvider>) this.LookupProviders).SelectMany<ILookupProvider, ISubject>((Func<ILookupProvider, IEnumerable<ISubject>>) (p => p.GetSearchSubjects()));
  }

  private Vector2 GetFacingTile(Farmer player)
  {
    Vector2 tile = ((Character) player).Tile;
    FacingDirection facingDirection = (FacingDirection) ((Character) player).FacingDirection;
    switch (facingDirection)
    {
      case FacingDirection.Up:
        return Vector2.op_Addition(tile, new Vector2(0.0f, -1f));
      case FacingDirection.Right:
        return Vector2.op_Addition(tile, new Vector2(1f, 0.0f));
      case FacingDirection.Down:
        return Vector2.op_Addition(tile, new Vector2(0.0f, 1f));
      case FacingDirection.Left:
        return Vector2.op_Addition(tile, new Vector2(-1f, 0.0f));
      default:
        throw new NotSupportedException($"Unknown facing direction {facingDirection}");
    }
  }
}
