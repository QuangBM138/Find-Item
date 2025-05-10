// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.Common.Items.ItemRepository
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Microsoft.Xna.Framework.Content;
using StardewValley;
using StardewValley.GameData;
using StardewValley.GameData.FishPonds;
using StardewValley.ItemTypeDefinitions;
using StardewValley.Objects;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace Pathoschild.Stardew.Common.Items;

internal class ItemRepository
{
  public IEnumerable<SearchableItem> GetAll(string? onlyType = null, bool includeVariants = true)
  {
    return GetAllRaw().Where<SearchableItem>((Func<SearchableItem, bool>) (item => item != null));

    IEnumerable<SearchableItem?> GetAllRaw()
    {
      foreach (IItemDataDefinition itemType1 in ItemRegistry.ItemTypes)
      {
        IItemDataDefinition itemType = itemType1;
        if (onlyType == null || !(itemType.Identifier != onlyType))
        {
          if (itemType.Identifier == "(O)")
          {
            ObjectDataDefinition objectDataDefinition = (ObjectDataDefinition) ItemRegistry.GetTypeDefinition("(O)");
            foreach (string allId in itemType.GetAllIds())
            {
              SearchableItem result = this.TryCreate(itemType.Identifier, allId, (Func<SearchableItem, Item>) (p => ItemRegistry.Create(itemType.Identifier + p.Id, 1, 0, false)));
              if (result?.Item is Ring)
                yield return result;
              else if (result?.QualifiedItemId == "(O)842")
              {
                foreach (SearchableItem secretNote in this.GetSecretNotes(itemType, true))
                  yield return secretNote;
              }
              else if (result?.QualifiedItemId == "(O)79")
              {
                foreach (SearchableItem secretNote in this.GetSecretNotes(itemType, false))
                  yield return secretNote;
              }
              else
              {
                switch (result?.QualifiedItemId)
                {
                  case "(O)340":
                    yield return this.TryCreate(itemType.Identifier, result.Id, (Func<SearchableItem, Item>) (_ => (Item) objectDataDefinition.CreateFlavoredHoney((Object) null)));
                    goto case "(O)DriedFruit";
                  case "(O)DriedFruit":
                  case "(O)DriedMushrooms":
                  case "(O)SmokedFish":
                  case "(O)SpecificBait":
                    if (includeVariants)
                    {
                      foreach (SearchableItem flavoredObjectVariant in this.GetFlavoredObjectVariants(objectDataDefinition, result?.Item as Object, itemType))
                        yield return flavoredObjectVariant;
                      break;
                    }
                    break;
                  default:
                    if (result != null)
                    {
                      yield return result;
                      goto case "(O)DriedFruit";
                    }
                    goto case "(O)DriedFruit";
                }
              }
              result = (SearchableItem) null;
            }
          }
          else
          {
            foreach (string allId in itemType.GetAllIds())
              yield return this.TryCreate(itemType.Identifier, allId, (Func<SearchableItem, Item>) (p => ItemRegistry.Create(itemType.Identifier + p.Id, 1, 0, false)));
          }
        }
      }
    }
  }

  private IEnumerable<SearchableItem?> GetSecretNotes(
    IItemDataDefinition itemType,
    bool isJournalScrap)
  {
    ItemRepository itemRepository = this;
    string baseId = isJournalScrap ? "842" : "79";
    foreach (int num in itemRepository.TryLoad<Dictionary<int, string>>((Func<Dictionary<int, string>>) (() => DataLoader.SecretNotes(Game1.content))).Keys.Where<int>(isJournalScrap ? (Func<int, bool>) (id => id >= GameLocation.JOURNAL_INDEX) : (Func<int, bool>) (id => id < GameLocation.JOURNAL_INDEX)).Select<int, int>(isJournalScrap ? (Func<int, int>) (id => id - GameLocation.JOURNAL_INDEX) : (Func<int, int>) (id => id)))
    {
      int id = num;
      yield return itemRepository.TryCreate(itemType.Identifier, $"{baseId}/{id}", (Func<SearchableItem, Item>) (_ =>
      {
        Item secretNotes = ItemRegistry.Create(itemType.Identifier + baseId, 1, 0, false);
        secretNotes.Name = $"{secretNotes.Name} #{id}";
        return secretNotes;
      }));
    }
  }

  private IEnumerable<SearchableItem?> GetFlavoredObjectVariants(
    ObjectDataDefinition objectDataDefinition,
    Object? item,
    IItemDataDefinition itemType)
  {
    if (item != null)
    {
      string id = ((Item) item).ItemId;
      switch (((Item) item).Category)
      {
        case -81:
          yield return this.TryCreate(itemType.Identifier, "342/" + id, (Func<SearchableItem, Item>) (_ => (Item) objectDataDefinition.CreateFlavoredPickle(item)));
          break;
        case -80:
          yield return this.TryCreate(itemType.Identifier, "340/" + id, (Func<SearchableItem, Item>) (_ => (Item) objectDataDefinition.CreateFlavoredHoney(item)));
          break;
        case -79:
          yield return this.TryCreate(itemType.Identifier, "348/" + id, (Func<SearchableItem, Item>) (_ => (Item) objectDataDefinition.CreateFlavoredWine(item)));
          yield return this.TryCreate(itemType.Identifier, "344/" + id, (Func<SearchableItem, Item>) (_ => (Item) objectDataDefinition.CreateFlavoredJelly(item)));
          if (((Item) item).QualifiedItemId != "(O)398")
          {
            yield return this.TryCreate(itemType.Identifier, "398/" + id, (Func<SearchableItem, Item>) (_ => (Item) objectDataDefinition.CreateFlavoredDriedFruit(item)));
            break;
          }
          break;
        case -75:
          yield return this.TryCreate(itemType.Identifier, "350/" + id, (Func<SearchableItem, Item>) (_ => (Item) objectDataDefinition.CreateFlavoredJuice(item)));
          yield return this.TryCreate(itemType.Identifier, "342/" + id, (Func<SearchableItem, Item>) (_ => (Item) objectDataDefinition.CreateFlavoredPickle(item)));
          break;
        case -23:
          if (((Item) item).QualifiedItemId == "(O)812")
          {
            List<List<string>> complexTags;
            HashSet<string> simpleTags;
            this.GetRoeContextTagLookups(out simpleTags, out complexTags);
            foreach (string key in (IEnumerable<string>) Game1.objectData.Keys)
            {
              Object input = this.TryCreate(itemType.Identifier, key, (Func<SearchableItem, Item>) (p => (Item) new Object(p.Id, 1, false, -1, 0)))?.Item as Object;
              if (input != null)
              {
                HashSet<string> contextTags = ((Item) input).GetContextTags();
                if (contextTags.Any<string>() && (contextTags.Any<string>((Func<string, bool>) (tag => simpleTags.Contains(tag))) || complexTags.Any<List<string>>((Func<List<string>, bool>) (set => set.All<string>((Func<string, bool>) (tag => ((Item) input).HasContextTag(tag)))))))
                {
                  SearchableItem roe = this.TryCreate(itemType.Identifier, "812/" + ((Item) input).ItemId, (Func<SearchableItem, Item>) (_ => (Item) objectDataDefinition.CreateFlavoredRoe(input)));
                  yield return roe;
                  Object roeObj = roe?.Item as Object;
                  if (roeObj != null && ((Item) input).QualifiedItemId != "(O)698")
                    yield return this.TryCreate(itemType.Identifier, "447/" + ((Item) input).ItemId, (Func<SearchableItem, Item>) (_ => (Item) objectDataDefinition.CreateFlavoredAgedRoe(roeObj)));
                  roe = (SearchableItem) null;
                }
              }
            }
            complexTags = (List<List<string>>) null;
            break;
          }
          break;
        case -4:
          yield return this.TryCreate(itemType.Identifier, "SmokedFish/" + id, (Func<SearchableItem, Item>) (_ => (Item) objectDataDefinition.CreateFlavoredSmokedFish(item)));
          yield return this.TryCreate(itemType.Identifier, "SpecificBait/" + id, (Func<SearchableItem, Item>) (_ => (Item) objectDataDefinition.CreateFlavoredBait(item)));
          break;
      }
      bool flag1 = ((Item) item).HasContextTag("preserves_pickle");
      if (flag1)
      {
        bool flag2;
        switch (((Item) item).Category)
        {
          case -81:
          case -75:
            flag2 = true;
            break;
          default:
            flag2 = false;
            break;
        }
        flag1 = !flag2;
      }
      if (flag1)
        yield return this.TryCreate(itemType.Identifier, "342/" + id, (Func<SearchableItem, Item>) (_ => (Item) objectDataDefinition.CreateFlavoredPickle(item)));
      if (((Item) item).HasContextTag("edible_mushroom"))
        yield return this.TryCreate(itemType.Identifier, "DriedMushrooms/" + id, (Func<SearchableItem, Item>) (_ => (Item) objectDataDefinition.CreateFlavoredDriedMushroom(item)));
    }
  }

  private void GetRoeContextTagLookups(
    out HashSet<string> simpleTags,
    out List<List<string>> complexTags)
  {
    simpleTags = new HashSet<string>();
    complexTags = new List<List<string>>();
    foreach (FishPondData fishPondData in this.TryLoad<List<FishPondData>>((Func<List<FishPondData>>) (() => DataLoader.FishPondData(Game1.content))))
    {
      if (!fishPondData.ProducedItems.All<FishPondReward>((Func<FishPondReward, bool>) (p =>
      {
        string itemId = ((GenericSpawnItemData) p).ItemId;
        return !(itemId == "812") && !(itemId == "(O)812");
      })))
      {
        if (fishPondData.RequiredTags.Count == 1 && !fishPondData.RequiredTags[0].StartsWith("!"))
          simpleTags.Add(fishPondData.RequiredTags[0]);
        else
          complexTags.Add(fishPondData.RequiredTags);
      }
    }
  }

  private TAsset TryLoad<TAsset>(Func<TAsset> load) where TAsset : new()
  {
    try
    {
      return load();
    }
    catch (ContentLoadException ex)
    {
      return new TAsset();
    }
  }

  private SearchableItem? TryCreate(string type, string key, Func<SearchableItem, Item> createItem)
  {
    try
    {
      SearchableItem searchableItem = new SearchableItem(type, key, createItem);
      searchableItem.Item.getDescription();
      string name = searchableItem.Item.Name;
      return name == null || name == "Error Item" ? (SearchableItem) null : searchableItem;
    }
    catch
    {
      return (SearchableItem) null;
    }
  }
}
