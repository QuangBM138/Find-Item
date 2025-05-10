// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.ItemScanning.WorldItemScanner
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Netcode;
using Pathoschild.Stardew.Common;
using Pathoschild.Stardew.Common.Utilities;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Buildings;
using StardewValley.Characters;
using StardewValley.Locations;
using StardewValley.Objects;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework.ItemScanning;

public class WorldItemScanner
{
  private readonly IReflectionHelper Reflection;

  public WorldItemScanner(IReflectionHelper reflection) => this.Reflection = reflection;

  public IEnumerable<FoundItem> GetAllOwnedItems()
  {
    List<FoundItem> tracked = new List<FoundItem>();
    ISet<Item> itemsSeen = (ISet<Item>) new HashSet<Item>((IEqualityComparer<Item>) new ObjectReferenceComparer<Item>());
    foreach (GameLocation location in CommonHelper.GetLocations())
    {
      foreach (Furniture root in location.furniture)
        this.ScanAndTrack(tracked, itemsSeen, (Item) root, (object) location, isRootInWorld: true);
      Chest chest;
      switch (location)
      {
        case FarmHouse farmHouse:
          chest = ((NetFieldBase<Chest, NetRef<Chest>>) farmHouse.fridge).Value;
          break;
        case IslandFarmHouse islandFarmHouse:
          chest = ((NetFieldBase<Chest, NetRef<Chest>>) islandFarmHouse.fridge).Value;
          break;
        default:
          chest = (Chest) null;
          break;
      }
      Chest root1 = chest;
      this.ScanAndTrack(tracked, itemsSeen, (Item) root1, (object) location, includeRoot: false);
      foreach (NPC character in location.characters)
      {
        Hat root2 = (character is Child child ? ((NetFieldBase<Hat, NetRef<Hat>>) child.hat).Value : (Hat) null) ?? (character is Horse horse ? ((NetFieldBase<Hat, NetRef<Hat>>) horse.hat).Value : (Hat) null);
        this.ScanAndTrack(tracked, itemsSeen, (Item) root2, (object) character);
      }
      foreach (Building building in location.buildings)
      {
        if (building is JunimoHut parent)
          this.ScanAndTrack(tracked, itemsSeen, (Item) parent.GetOutputChest(), (object) parent, includeRoot: false);
        foreach (Chest buildingChest in building.buildingChests)
          this.ScanAndTrack(tracked, itemsSeen, (Item) buildingChest, (object) building, includeRoot: false);
      }
      foreach (Object root3 in location.objects.Values)
      {
        if (root3 is Chest || !this.IsSpawnedWorldItem((Item) root3))
          this.ScanAndTrack(tracked, itemsSeen, (Item) root3, (object) location, isRootInWorld: true);
      }
    }
    this.ScanAndTrack(tracked, itemsSeen, (IEnumerable<Item>) Game1.player.Items, (object) Game1.player, true);
    // ISSUE: object of a compiler-generated type is created
    this.ScanAndTrack(tracked, itemsSeen, (IEnumerable<Item>) new \u003C\u003Ez__ReadOnlyArray<Item>(new Item[6]
    {
      (Item) ((NetFieldBase<Clothing, NetRef<Clothing>>) Game1.player.shirtItem).Value,
      (Item) ((NetFieldBase<Clothing, NetRef<Clothing>>) Game1.player.pantsItem).Value,
      (Item) ((NetFieldBase<Boots, NetRef<Boots>>) Game1.player.boots).Value,
      (Item) ((NetFieldBase<Hat, NetRef<Hat>>) Game1.player.hat).Value,
      (Item) ((NetFieldBase<Ring, NetRef<Ring>>) Game1.player.leftRing).Value,
      (Item) ((NetFieldBase<Ring, NetRef<Ring>>) Game1.player.rightRing).Value
    }), (object) Game1.player, true);
    Farm farm = Game1.getFarm();
    int val1 = farm != null ? ((NetFieldBase<int, NetInt>) ((GameLocation) farm).piecesOfHay).Value : 0;
    while (val1 > 0)
    {
      Item root = ItemRegistry.Create("(O)178", 1, 0, false);
      root.Stack = Math.Min(val1, root.maximumStackSize());
      val1 -= root.Stack;
      this.ScanAndTrack(tracked, itemsSeen, root, (object) farm);
    }
    return (IEnumerable<FoundItem>) tracked;
  }

  private bool IsSpawnedWorldItem(Item item)
  {
    if (!(item is Object @object))
      return false;
    return @object.IsSpawnedObject || @object.isForage() || ((Item) @object).Category == -999;
  }

  private void ScanAndTrack(
    List<FoundItem> tracked,
    ISet<Item> itemsSeen,
    Item? root,
    object? parent,
    bool isInInventory = false,
    bool isRootInWorld = false,
    bool includeRoot = true)
  {
    foreach (FoundItem foundItem in this.Scan(itemsSeen, root, parent, isInInventory, isRootInWorld, includeRoot))
      tracked.Add(foundItem);
  }

  private void ScanAndTrack(
    List<FoundItem> tracked,
    ISet<Item> itemsSeen,
    IEnumerable<Item> roots,
    object parent,
    bool isInInventory = false,
    bool isRootInWorld = false,
    bool includeRoots = true)
  {
    foreach (FoundItem foundItem in roots.SelectMany<Item, FoundItem>((Func<Item, IEnumerable<FoundItem>>) (root => this.Scan(itemsSeen, root, parent, isInInventory, isRootInWorld, includeRoots))))
      tracked.Add(foundItem);
  }

  private IEnumerable<FoundItem> Scan(
    ISet<Item> itemsSeen,
    Item? root,
    object? parent,
    bool isInInventory,
    bool isRootInWorld,
    bool includeRoot = true)
  {
    if (root != null && itemsSeen.Add(root))
    {
      yield return new FoundItem(root, parent, isInInventory);
      foreach (FoundItem foundItem in this.GetDirectContents(root, isRootInWorld).SelectMany<Item, FoundItem>((Func<Item, IEnumerable<FoundItem>>) (p => this.Scan(itemsSeen, p, (object) root, isInInventory, false))))
        yield return foundItem;
    }
  }

  private IEnumerable<Item> GetDirectContents(Item? root, bool isRootInWorld)
  {
    if (root != null)
    {
      if (root is Object @object)
      {
        if (@object.MinutesUntilReady <= 0 || @object is Cask)
          yield return (Item) ((NetFieldBase<Object, NetRef<Object>>) @object.heldObject).Value;
      }
      else if (this.IsCustomItemClass(root))
      {
        Item directContent = this.Reflection.GetField<Item>((object) root, "heldObject", false)?.GetValue() ?? this.Reflection.GetProperty<Item>((object) root, "heldObject", false)?.GetValue();
        if (directContent != null)
          yield return directContent;
      }
      switch (root)
      {
        case StorageFurniture storageFurniture:
          foreach (Item heldItem in (NetList<Item, NetRef<Item>>) storageFurniture.heldItems)
            yield return heldItem;
          break;
        case Chest chest:
          if (isRootInWorld && !((NetFieldBase<bool, NetBool>) chest.playerChest).Value)
            break;
          foreach (Item directContent in (IEnumerable<Item>) chest.GetItemsForPlayer(Game1.player.UniqueMultiplayerID))
            yield return directContent;
          break;
        case Tool tool:
          foreach (Item attachment in (NetArray<Object, NetRef<Object>>) tool.attachments)
            yield return attachment;
          break;
      }
    }
  }

  private bool IsCustomItemClass(Item item)
  {
    string str = item.GetType().Namespace ?? "";
    return str != "StardewValley" && !str.StartsWith("StardewValley.");
  }
}
