// Decompiled with JetBrains decompiler
// Type: Item_Locator.FindContainers
// Assembly: Item Locator, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BFE121E-49FA-41A1-80AC-34270D5A3C38
// Assembly location: D:\game indi\Item Locator\Item Locator.dll

using Microsoft.Xna.Framework;
using Netcode;
using StardewValley;
using StardewValley.Buildings;
using StardewValley.Objects;
using System;
using System.Collections.Generic;

#nullable enable
namespace Item_Locator;

public class FindContainers
{
  public static List<Vector2> get_container_locs(GameLocation location, string i)
  {
    FindContainers.getJunimoHutTiles(location, i);
    List<Vector2> containerLocs = new List<Vector2>();
    Vector2? houseFridgeTile = FindContainers.getHouseFridgeTile(location, i);
    List<Vector2> junimoHutTiles = FindContainers.getJunimoHutTiles(location, i);
    if (houseFridgeTile.HasValue)
      containerLocs.Add(houseFridgeTile.Value);
    if (junimoHutTiles != null)
      containerLocs.AddRange((IEnumerable<Vector2>) junimoHutTiles);
    for (int index1 = 0; index1 < location.map.Layers[0].LayerWidth; ++index1)
    {
      for (int index2 = 0; index2 < location.map.Layers[0].LayerHeight; ++index2)
      {
        Chest chest;
        int num;
        if (location.objects.ContainsKey(new Vector2((float) index1, (float) index2)))
        {
          chest = location.Objects[new Vector2((float) index1, (float) index2)] as Chest;
          num = chest != null ? 1 : 0;
        }
        else
          num = 0;
        if (num != 0)
        {
          if (((Object) chest).name == "Junimo Chest")
          {
            foreach (Item obj in Game1.player.team.GetOrCreateGlobalInventory("JunimoChests"))
            {
              if (i.Equals(obj.Name, StringComparison.OrdinalIgnoreCase))
              {
                containerLocs.Add(new Vector2((float) index1, (float) index2));
                break;
              }
            }
          }
          else
          {
            foreach (Item obj in chest.Items)
            {
              if (i.Equals(obj.Name, StringComparison.OrdinalIgnoreCase))
              {
                containerLocs.Add(new Vector2((float) index1, (float) index2));
                break;
              }
            }
          }
        }
      }
    }
    return containerLocs;
  }

  private static Vector2? getHouseFridgeTile(GameLocation playerloc, string i)
  {
    GameLocation locationFromName1 = Game1.getLocationFromName("FarmHouse");
    GameLocation locationFromName2 = Game1.getLocationFromName("IslandFarmHouse");
    if ((playerloc == locationFromName1 || playerloc == locationFromName2) && playerloc.GetFridgePosition().HasValue)
    {
      foreach (Item obj in playerloc.GetFridge(true).Items)
      {
        if (i.Equals(obj.Name, StringComparison.OrdinalIgnoreCase))
          return new Vector2?(new Vector2((float) playerloc.GetFridgePosition().Value.X, (float) playerloc.GetFridgePosition().Value.Y));
      }
    }
    return new Vector2?();
  }

  private static List<Vector2> getJunimoHutTiles(GameLocation playerloc, string i)
  {
    List<Vector2> junimoHutTiles = new List<Vector2>();
    GameLocation locationFromName = Game1.getLocationFromName("Farm");
    Farm farm = Game1.getFarm();
    if (playerloc == locationFromName)
    {
      foreach (Building building in ((GameLocation) farm).buildings)
      {
        if (building is JunimoHut junimoHut)
        {
          foreach (Item obj in junimoHut.GetOutputChest().Items)
          {
            if (i.Equals(obj.Name, StringComparison.OrdinalIgnoreCase))
            {
              junimoHutTiles.Add(new Vector2((float) (((NetFieldBase<int, NetInt>) ((Building) junimoHut).tileX).Value + 1), (float) (((NetFieldBase<int, NetInt>) ((Building) junimoHut).tileY).Value + 1)));
              break;
            }
          }
        }
      }
    }
    return junimoHutTiles;
  }
}
