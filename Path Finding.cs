// Decompiled with JetBrains decompiler
// Type: Item_Locator.Path_Finding
// Assembly: Item Locator, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BFE121E-49FA-41A1-80AC-34270D5A3C38
// Assembly location: D:\game indi\Item Locator\Item Locator.dll

using Microsoft.Xna.Framework;
using Netcode;
using StardewValley;
using System;
using System.Collections.Generic;

#nullable enable
namespace Item_Locator;

public class Path_Finding
{
  public static bool invalidPlayerTile;

  public static List<Vector2> Find_Empty_Tiles(GameLocation location)
  {
    List<Vector2> emptyTiles = new List<Vector2>();
    for (int index1 = 0; index1 < location.map.Layers[0].LayerWidth; ++index1)
    {
      for (int index2 = 0; index2 < location.map.Layers[0].LayerHeight; ++index2)
      {
        if (Path_Finding.isEmptyTile(location, new Vector2((float) index1, (float) index2)))
          emptyTiles.Add(new Vector2((float) index1, (float) index2));
      }
    }
    return emptyTiles;
  }

  public static Dictionary<Vector2, List<Vector2>> genAdjList(List<Vector2> targets)
  {
    GameLocation currentLocation = ((Character) Game1.player).currentLocation;
    Dictionary<Vector2, List<Vector2>> dictionary = new Dictionary<Vector2, List<Vector2>>();
    List<Vector2> emptyTiles = Path_Finding.Find_Empty_Tiles(currentLocation);
    foreach (Vector2 target in targets)
      emptyTiles.Add(target);
    foreach (Vector2 key in emptyTiles)
    {
      List<Vector2> vector2List = new List<Vector2>();
      foreach (Vector2 vector2 in emptyTiles)
      {
        if (Vector2.op_Equality(new Vector2(key.X - 1f, key.Y), vector2))
          vector2List.Add(vector2);
        if (Vector2.op_Equality(new Vector2(key.X + 1f, key.Y), vector2))
          vector2List.Add(vector2);
        if (Vector2.op_Equality(new Vector2(key.X, key.Y + 1f), vector2))
          vector2List.Add(vector2);
        if (Vector2.op_Equality(new Vector2(key.X, key.Y - 1f), vector2))
          vector2List.Add(vector2);
        if (vector2List.Count == 4)
          break;
      }
      dictionary.Add(key, vector2List);
    }
    return dictionary;
  }

  public static void GetPaths()
  {
    ModEntry.paths.Clear();
    Random random = new Random();
    GameLocation currentLocation = ((Character) Game1.player).currentLocation;
    Vector2 tile = ((Character) Game1.player).Tile;
    List<Vector2> containerLocs = FindContainers.get_container_locs(currentLocation, CustomItemMenu.SearchedItem);
    if (containerLocs.Count > 0)
    {
      ModEntry.paths = Path_Finding.FindPathsBFS(Path_Finding.genAdjList(containerLocs), containerLocs, tile);
      foreach (List<Vector2> path in ModEntry.paths)
        ModEntry.pathColors[path] = new Color(random.Next(125, 256 /*0x0100*/), random.Next(125, 256 /*0x0100*/), random.Next(125, 256 /*0x0100*/));
      ModEntry.shouldDraw = true;
    }
    else
    {
      ModEntry.paths.Clear();
      ModEntry.pathColors.Clear();
      ModEntry.shouldDraw = false;
    }
  }

  private static List<List<Vector2>> FindPathsBFS(
    Dictionary<Vector2, List<Vector2>> adjlist,
    List<Vector2> targets,
    Vector2 playerLocation)
  {
    Vector2 vector2 = playerLocation;
    List<List<Vector2>> pathsBfs = new List<List<Vector2>>();
    if (!Path_Finding.isEmptyTile(((Character) Game1.player).currentLocation, vector2))
    {
      Path_Finding.invalidPlayerTile = true;
      return pathsBfs;
    }
    Path_Finding.invalidPlayerTile = false;
    foreach (Vector2 target in targets)
    {
      Dictionary<Vector2, Vector2?> prev = Path_Finding.solve(vector2, target, adjlist);
      List<Vector2> vector2List = Path_Finding.reconstructPath(vector2, target, prev);
      if (vector2List.Count > 0)
        pathsBfs.Add(vector2List);
    }
    return pathsBfs;
  }

  private static Dictionary<Vector2, Vector2?> solve(
    Vector2 start,
    Vector2 target,
    Dictionary<Vector2, List<Vector2>> adjlist)
  {
    Queue<Vector2> vector2Queue = new Queue<Vector2>();
    vector2Queue.Enqueue(start);
    Dictionary<Vector2, bool> dictionary1 = new Dictionary<Vector2, bool>();
    Dictionary<Vector2, Vector2?> dictionary2 = new Dictionary<Vector2, Vector2?>();
    foreach (Vector2 key in adjlist.Keys)
    {
      if (Vector2.op_Equality(key, start))
        dictionary1[start] = true;
      else
        dictionary1[key] = false;
      dictionary2[key] = new Vector2?();
    }
    dictionary2[target] = new Vector2?();
    while (vector2Queue.Count > 0)
    {
      Vector2 key1 = vector2Queue.Dequeue();
      foreach (Vector2 key2 in adjlist[key1])
      {
        if (!dictionary1[key2])
        {
          vector2Queue.Enqueue(key2);
          dictionary1[key2] = true;
          dictionary2[key2] = new Vector2?(key1);
        }
      }
    }
    return dictionary2;
  }

  private static List<Vector2> reconstructPath(
    Vector2 start,
    Vector2 end,
    Dictionary<Vector2, Vector2?> prev)
  {
    List<Vector2> vector2List = new List<Vector2>();
    for (Vector2? nullable = new Vector2?(end); nullable.HasValue && nullable.HasValue; nullable = prev[nullable.Value] ?? new Vector2?())
      vector2List.Add(nullable.Value);
    vector2List.Reverse();
    return Vector2.op_Equality(vector2List[0], start) ? vector2List : new List<Vector2>();
  }

  private static bool isEmptyTile(GameLocation location, Vector2 tile)
  {
    Object @object;
    if (location.objects.TryGetValue(tile, ref @object) && @object is Fence fence && ((NetFieldBase<bool, NetBool>) fence.isGate).Value && (NetInt.op_Implicit(fence.gatePosition) == 88 || NetInt.op_Implicit(fence.gatePosition) == 0))
      return true;
    return location.isTileOnMap(tile) && !location.IsTileBlockedBy(tile, (CollisionMask) (int) byte.MaxValue, (CollisionMask) (int) byte.MaxValue, true);
  }
}
