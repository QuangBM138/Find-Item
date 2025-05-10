// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.GameHelper
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Netcode;
using Pathoschild.Stardew.Common;
using Pathoschild.Stardew.Common.Integrations.BetterGameMenu;
using Pathoschild.Stardew.Common.Integrations.BushBloomMod;
using Pathoschild.Stardew.Common.Integrations.CustomBush;
using Pathoschild.Stardew.Common.Integrations.CustomFarmingRedux;
using Pathoschild.Stardew.Common.Integrations.ExtraMachineConfig;
using Pathoschild.Stardew.Common.Integrations.MultiFertilizer;
using Pathoschild.Stardew.Common.Integrations.ProducerFrameworkMod;
using Pathoschild.Stardew.Common.Integrations.SpaceCore;
using Pathoschild.Stardew.Common.Items;
using Pathoschild.Stardew.LookupAnything.Framework;
using Pathoschild.Stardew.LookupAnything.Framework.Constants;
using Pathoschild.Stardew.LookupAnything.Framework.Data;
using Pathoschild.Stardew.LookupAnything.Framework.ItemScanning;
using Pathoschild.Stardew.LookupAnything.Framework.Models;
using Pathoschild.Stardew.LookupAnything.Framework.Models.FishData;
using StardewModdingAPI;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Buildings;
using StardewValley.Characters;
using StardewValley.Extensions;
using StardewValley.GameData.Crafting;
using StardewValley.GameData.Crops;
using StardewValley.GameData.FishPonds;
using StardewValley.GameData.Locations;
using StardewValley.ItemTypeDefinitions;
using StardewValley.Locations;
using StardewValley.Menus;
using StardewValley.Network;
using StardewValley.Objects;
using StardewValley.Quests;
using StardewValley.SpecialOrders;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using xTile.Dimensions;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything;

internal class GameHelper
{
  private readonly CustomFarmingReduxIntegration CustomFarmingRedux;
  private readonly ProducerFrameworkModIntegration ProducerFrameworkMod;
  private readonly DataParser DataParser = new DataParser();
  private readonly WorldItemScanner WorldItemScanner;
  private readonly ItemRepository ItemRepository = new ItemRepository();
  private readonly IMonitor Monitor;
  private readonly IModRegistry ModRegistry;
  private Lazy<SearchableItem[]> Objects;
  private Lazy<RecipeModel[]> Recipes;

  public Metadata Metadata { get; }

  public BetterGameMenuIntegration BetterGameMenu { get; }

  public BushBloomModIntegration BushBloomMod { get; }

  public CustomBushIntegration CustomBush { get; }

  public ExtraMachineConfigIntegration ExtraMachineConfig { get; }

  public MultiFertilizerIntegration MultiFertilizer { get; }

  public SpaceCoreIntegration SpaceCore { get; }

  public GameHelper(
    Metadata metadata,
    IMonitor monitor,
    IModRegistry modRegistry,
    IReflectionHelper reflection)
  {
    this.Metadata = metadata;
    this.Monitor = monitor;
    this.ModRegistry = modRegistry;
    this.WorldItemScanner = new WorldItemScanner(reflection);
    this.BetterGameMenu = new BetterGameMenuIntegration(modRegistry, monitor);
    this.BushBloomMod = new BushBloomModIntegration(modRegistry, monitor);
    this.CustomBush = new CustomBushIntegration(modRegistry, monitor);
    this.CustomFarmingRedux = new CustomFarmingReduxIntegration(modRegistry, monitor);
    this.ExtraMachineConfig = new ExtraMachineConfigIntegration(modRegistry, monitor);
    this.MultiFertilizer = new MultiFertilizerIntegration(modRegistry, monitor);
    this.ProducerFrameworkMod = new ProducerFrameworkModIntegration(modRegistry, monitor);
    this.SpaceCore = new SpaceCoreIntegration(modRegistry, monitor);
    this.ResetCache(monitor);
  }

  [MemberNotNull(new string[] {"Objects", "Recipes"})]
  public void ResetCache(IMonitor monitor)
  {
    this.Objects = new Lazy<SearchableItem[]>((Func<SearchableItem[]>) (() => this.ItemRepository.GetAll("(O)").Where<SearchableItem>((Func<SearchableItem, bool>) (p => !(p.Item is Ring))).ToArray<SearchableItem>()));
    this.Recipes = new Lazy<RecipeModel[]>((Func<RecipeModel[]>) (() => ((IEnumerable<RecipeModel>) this.GetAllRecipes(monitor)).ToArray<RecipeModel>()));
  }

  public string TranslateSeason(string season)
  {
    int seasonNumber = Utility.getSeasonNumber(season);
    return seasonNumber == -1 ? season : Utility.getSeasonNameFromNumber(seasonNumber);
  }

  public bool TryGetDate(int day, string season, out SDate date)
  {
    return this.TryGetDate(day, season, Game1.year, out date);
  }

  public bool TryGetDate(int day, string season, int year, out SDate date)
  {
    try
    {
      date = new SDate(day, season, year);
      return true;
    }
    catch
    {
      date = SDate.Now();
      return false;
    }
  }

  public int GetShipped(string itemID)
  {
    return !((NetDictionary<string, int, NetInt, SerializableDictionary<string, int>, NetStringDictionary<int, NetInt>>) Game1.player.basicShipped).ContainsKey(itemID) ? 0 : ((NetDictionary<string, int, NetInt, SerializableDictionary<string, int>, NetStringDictionary<int, NetInt>>) Game1.player.basicShipped)[itemID];
  }

  public IEnumerable<KeyValuePair<string, bool>> GetFullShipmentAchievementItems()
  {
    return ((IEnumerable<SearchableItem>) this.Objects.Value).Select(entry => new
    {
      entry = entry,
      obj = (Object) entry.Item
    }).Where(_param1 => _param1.obj.Type != "Arch" && _param1.obj.Type != "Fish" && _param1.obj.Type != "Mineral" && _param1.obj.Type != "Cooking" && Object.isPotentialBasicShipped(((Item) _param1.obj).ItemId, ((Item) _param1.obj).Category, _param1.obj.Type)).Select(_param1 => new KeyValuePair<string, bool>(((Item) _param1.obj).QualifiedItemId, ((NetDictionary<string, int, NetInt, SerializableDictionary<string, int>, NetStringDictionary<int, NetInt>>) Game1.player.basicShipped).ContainsKey(((Item) _param1.obj).ItemId)));
  }

  public static CropData? GetCropDataByHarvestItem(string itemId)
  {
    foreach (CropData dataByHarvestItem in (IEnumerable<CropData>) Game1.cropData.Values)
    {
      if (dataByHarvestItem.HarvestItemId == itemId)
        return dataByHarvestItem;
    }
    return (CropData) null;
  }

  public IEnumerable<FoundItem> GetAllOwnedItems() => this.WorldItemScanner.GetAllOwnedItems();

  public IEnumerable<NPC> GetAllCharacters() => Utility.getAllCharacters().Distinct<NPC>();

  public int CountOwnedItems(Item item, bool flavorSpecific)
  {
    return this.GetAllOwnedItems().Select(found => new
    {
      found = found,
      foundItem = found.Item
    }).Where(_param1 => this.AreEquivalent(_param1.foundItem, item, flavorSpecific)).Select(_param1 => new
    {
      \u003C\u003Eh__TransparentIdentifier0 = _param1,
      canStack = _param1.foundItem.canStackWith((ISalable) _param1.foundItem)
    }).Select(_param1 => !_param1.canStack ? 1 : _param1.\u003C\u003Eh__TransparentIdentifier0.found.GetCount()).Sum();
  }

  public bool IsSocialVillager(NPC npc)
  {
    if (!((Character) npc).IsVillager)
      return false;
    bool flag;
    if (this.Metadata.Constants.ForceSocialVillagers.TryGetValue(((Character) npc).Name, out flag))
      return flag;
    return ((NetDictionary<string, Friendship, NetRef<Friendship>, SerializableDictionary<string, Friendship>, NetStringDictionary<Friendship, NetRef<Friendship>>>) Game1.player.friendshipData).ContainsKey(((Character) npc).Name) || npc.CanSocialize;
  }

  public IEnumerable<GiftTasteModel> GetGiftTastes(Item item)
  {
    if (item.canBeGivenAsGift())
    {
      foreach (NPC allCharacter in this.GetAllCharacters())
      {
        if (this.IsSocialVillager(allCharacter))
        {
          GiftTaste? giftTaste = this.GetGiftTaste(allCharacter, item);
          if (giftTaste.HasValue)
            yield return new GiftTasteModel(allCharacter, item, giftTaste.Value);
        }
      }
    }
  }

  public IEnumerable<GiftTasteModel> GetGiftTastes(NPC npc)
  {
    return !this.IsSocialVillager(npc) ? (IEnumerable<GiftTasteModel>) Array.Empty<GiftTasteModel>() : this.ItemRepository.GetAll("(O)", false).Where<SearchableItem>((Func<SearchableItem, bool>) (entry => !(entry.Item is Ring))).Select(entry => new
    {
      entry = entry,
      item = entry.CreateItem()
    }).Select(_param1 => new
    {
      \u003C\u003Eh__TransparentIdentifier0 = _param1,
      taste = this.GetGiftTaste(npc, _param1.item)
    }).Where(_param1 => _param1.taste.HasValue).Select(_param1 => new GiftTasteModel(npc, _param1.\u003C\u003Eh__TransparentIdentifier0.item, _param1.taste.Value));
  }

  public IEnumerable<KeyValuePair<NPC, GiftTaste?>> GetMovieTastes()
  {
    foreach (NPC allCharacter in this.GetAllCharacters())
    {
      if (this.IsSocialVillager(allCharacter))
      {
        string responseForMovie = MovieTheater.GetResponseForMovie(allCharacter);
        switch (responseForMovie)
        {
          case "love":
          case "like":
          case "dislike":
            yield return new KeyValuePair<NPC, GiftTaste?>(allCharacter, new GiftTaste?(Enum.Parse<GiftTaste>(responseForMovie, true)));
            continue;
          case "reject":
            yield return new KeyValuePair<NPC, GiftTaste?>(allCharacter, new GiftTaste?());
            continue;
          default:
            continue;
        }
      }
    }
  }

  public IEnumerable<FishPondPopulationGateData> GetFishPondPopulationGates(FishPondData data)
  {
    return this.DataParser.GetFishPondPopulationGates(data);
  }

  public IEnumerable<FishPondDropData> GetFishPondDrops(FishPondData data)
  {
    return this.DataParser.GetFishPondDrops(data);
  }

  public FishSpawnData GetFishSpawnRules(ParsedItemData fish)
  {
    return this.DataParser.GetFishSpawnRules(fish, this.Metadata);
  }

  public IEnumerable<FishSpawnData> GetFishSpawnRules(
    GameLocation location,
    Vector2 tile,
    string fishAreaId)
  {
    return this.DataParser.GetFishSpawnRules(location, tile, fishAreaId, this.Metadata);
  }

  public FriendshipModel GetFriendshipForVillager(Farmer player, NPC npc, Friendship friendship)
  {
    return this.DataParser.GetFriendshipForVillager(player, npc, friendship, this.Metadata);
  }

  public FriendshipModel GetFriendshipForPet(Farmer player, Pet pet)
  {
    return this.DataParser.GetFriendshipForPet(player, pet);
  }

  public FriendshipModel GetFriendshipForAnimal(Farmer player, FarmAnimal animal)
  {
    return this.DataParser.GetFriendshipForAnimal(player, animal, this.Metadata);
  }

  public string GetLocationDisplayName(FishSpawnLocationData fishSpawnData)
  {
    return this.DataParser.GetLocationDisplayName(fishSpawnData);
  }

  public string GetLocationDisplayName(GameLocation location, string? fishAreaId)
  {
    return this.DataParser.GetLocationDisplayName(location.Name, location.GetData(), fishAreaId);
  }

  public string GetLocationDisplayName(string id, LocationData? data)
  {
    return this.DataParser.GetLocationDisplayName(id, data);
  }

  public IEnumerable<MonsterData> GetMonsterData() => this.DataParser.GetMonsters();

  public IEnumerable<BundleModel> GetBundleData() => this.DataParser.GetBundles(this.Monitor);

  public IEnumerable<RecipeModel> GetRecipes() => (IEnumerable<RecipeModel>) this.Recipes.Value;

  public IEnumerable<RecipeModel> GetRecipesForIngredient(Item item)
  {
    if (item.TypeDefinitionId != "(O)")
      return (IEnumerable<RecipeModel>) Array.Empty<RecipeModel>();
    List<RecipeModel> recipes = this.GetRecipes().Where<RecipeModel>((Func<RecipeModel, bool>) (recipe => ((IEnumerable<RecipeIngredientModel>) recipe.Ingredients).Any<RecipeIngredientModel>((Func<RecipeIngredientModel, bool>) (p => p.Matches(item))) && !((IEnumerable<RecipeIngredientModel>) recipe.ExceptIngredients).Any<RecipeIngredientModel>((Func<RecipeIngredientModel, bool>) (p => p.Matches(item))))).ToList<RecipeModel>();
    int result;
    recipes.RemoveAll((Predicate<RecipeModel>) (recipe => recipe.Type == RecipeType.MachineInput && int.TryParse(((IEnumerable<RecipeIngredientModel>) recipe.Ingredients).FirstOrDefault<RecipeIngredientModel>()?.InputId, out result) && result < 0 && recipes.Any<RecipeModel>((Func<RecipeModel, bool>) (other => ((IEnumerable<RecipeIngredientModel>) other.Ingredients).FirstOrDefault<RecipeIngredientModel>()?.InputId == item.QualifiedItemId && other.DisplayType == recipe.DisplayType))));
    return (IEnumerable<RecipeModel>) recipes;
  }

  public IEnumerable<RecipeModel> GetRecipesForOutput(Item item)
  {
    return this.GetRecipes().Where<RecipeModel>((Func<RecipeModel, bool>) (recipe => this.AreEquivalent(item, recipe.TryCreateItem(item), false)));
  }

  public IEnumerable<RecipeModel> GetRecipesForMachine(Object? machine)
  {
    return machine == null ? (IEnumerable<RecipeModel>) Array.Empty<RecipeModel>() : (IEnumerable<RecipeModel>) this.GetRecipes().Where<RecipeModel>((Func<RecipeModel, bool>) (recipe => recipe.IsForMachine((Item) machine))).ToList<RecipeModel>();
  }

  public IEnumerable<RecipeModel> GetRecipesForBuilding(Building? building)
  {
    return building == null ? (IEnumerable<RecipeModel>) Array.Empty<RecipeModel>() : (IEnumerable<RecipeModel>) this.GetRecipes().Where<RecipeModel>((Func<RecipeModel, bool>) (recipe => recipe.IsForMachine(building))).ToList<RecipeModel>();
  }

  public IEnumerable<QuestModel> GetQuestsWhichNeedItem(Object item)
  {
    foreach (QuestModel questModel in ((IEnumerable<Quest>) Game1.player.questLog).Select<Quest, QuestModel>((Func<Quest, QuestModel>) (quest => new QuestModel(quest))).Concat<QuestModel>(((IEnumerable<SpecialOrder>) Game1.player.team.specialOrders).Select<SpecialOrder, QuestModel>((Func<SpecialOrder, QuestModel>) (order => new QuestModel(order)))))
    {
      if (!string.IsNullOrWhiteSpace(questModel.DisplayText) && questModel.NeedsItem(item))
        yield return questModel;
    }
  }

  public IEnumerable<Object> GetObjectsByCategory(int category)
  {
    foreach (SearchableItem searchableItem in ((IEnumerable<SearchableItem>) this.Objects.Value).Where<SearchableItem>((Func<SearchableItem, bool>) (obj => obj.Item.Category == category)))
      yield return (Object) searchableItem.CreateItem();
  }

  public bool CanHaveQuality(Item item)
  {
    if (((IEnumerable<string>) new string[9]
    {
      "Artifact",
      "Trash",
      "Crafting",
      "Seed",
      "Decor",
      "Resource",
      "Fertilizer",
      "Bait",
      "Fishing Tackle"
    }).Contains<string>(item.getCategoryName()))
      return false;
    return !((IEnumerable<string>) new string[3]
    {
      "Crafting",
      "asdf",
      "Quest"
    }).Contains<string>(item is Object @object ? @object.Type : (string) null);
  }

  public IModInfo? TryGetModFromStringId(string? id)
  {
    return CommonHelper.TryGetModFromStringId(this.ModRegistry, id);
  }

  public Vector2 GetScreenCoordinatesFromCursor()
  {
    return new Vector2((float) Game1.getMouseX(), (float) Game1.getMouseY());
  }

  public Vector2 GetScreenCoordinatesFromAbsolute(Vector2 coordinates)
  {
    return Vector2.op_Subtraction(coordinates, new Vector2((float) ((Rectangle) ref Game1.uiViewport).X, (float) ((Rectangle) ref Game1.uiViewport).Y));
  }

  public Rectangle GetScreenCoordinatesFromTile(Vector2 tile)
  {
    Vector2 coordinatesFromAbsolute = this.GetScreenCoordinatesFromAbsolute(Vector2.op_Multiply(tile, new Vector2(64f)));
    return new Rectangle((int) coordinatesFromAbsolute.X, (int) coordinatesFromAbsolute.Y, 64 /*0x40*/, 64 /*0x40*/);
  }

  public bool CouldSpriteOccludeTile(Vector2 spriteTile, Vector2 occludeTile, Vector2? spriteSize = null)
  {
    spriteSize.GetValueOrDefault();
    if (!spriteSize.HasValue)
      spriteSize = new Vector2?(Constant.MaxTargetSpriteSize);
    return (double) spriteTile.Y >= (double) occludeTile.Y && (double) Math.Abs(spriteTile.X - occludeTile.X) <= (double) spriteSize.Value.X && (double) Math.Abs(spriteTile.Y - occludeTile.Y) <= (double) spriteSize.Value.Y;
  }

  public Vector2 GetSpriteSheetCoordinates(
    Vector2 worldPosition,
    Rectangle worldRectangle,
    Rectangle spriteRectangle,
    SpriteEffects spriteEffects = 0)
  {
    float num1 = (float) (((double) worldPosition.X - (double) worldRectangle.X) / 4.0);
    float num2 = (float) (((double) worldPosition.Y - (double) worldRectangle.Y) / 4.0);
    if (((Enum) (object) spriteEffects).HasFlag((Enum) (object) (SpriteEffects) 1))
      num1 = (float) spriteRectangle.Width - num1;
    if (((Enum) (object) spriteEffects).HasFlag((Enum) (object) (SpriteEffects) 2))
      num2 = (float) spriteRectangle.Height - num2;
    return new Vector2(num1 + (float) spriteRectangle.X, num2 + (float) spriteRectangle.Y);
  }

  public TPixel GetSpriteSheetPixel<TPixel>(Texture2D spriteSheet, Vector2 position) where TPixel : struct
  {
    int x = (int) position.X;
    int index = (int) position.Y * spriteSheet.Width + x;
    TPixel[] pixelArray = new TPixel[spriteSheet.Width * spriteSheet.Height];
    spriteSheet.GetData<TPixel>(pixelArray);
    return pixelArray[index];
  }

  public SpriteInfo? GetSprite(Item? item, bool onlyCustom = false)
  {
    if (item is Object @object && this.CustomFarmingRedux.IsLoaded)
    {
      SpriteInfo sprite = this.CustomFarmingRedux.GetSprite(@object);
      if (sprite != null)
        return sprite;
    }
    if (onlyCustom || item == null)
      return (SpriteInfo) null;
    ParsedItemData dataOrErrorItem = ItemRegistry.GetDataOrErrorItem(item.QualifiedItemId);
    return new SpriteInfo(dataOrErrorItem.GetTexture(), dataOrErrorItem.GetSourceRect(0, new int?()));
  }

  public Vector2 DrawHoverBox(
    SpriteBatch spriteBatch,
    string label,
    Vector2 position,
    float wrapWidth)
  {
    return CommonHelper.DrawHoverBox(spriteBatch, label, in position, wrapWidth);
  }

  public void ShowErrorMessage(string message) => CommonHelper.ShowErrorMessage(message);

  public IClickableMenu? GetGameMenuPage(IClickableMenu menu)
  {
    return menu is GameMenu gameMenu ? gameMenu.GetCurrentPage() : this.BetterGameMenu.GetCurrentPage(menu);
  }

  private bool AreEquivalent(Item? a, Item? b, bool flavorSpecific)
  {
    if (a != null && b != null && a.QualifiedItemId == b.QualifiedItemId)
    {
      bool? nullable1 = a is Chest chest1 ? new bool?(((NetFieldBase<bool, NetBool>) chest1.fridge).Value) : new bool?();
      bool? nullable2 = b is Chest chest2 ? new bool?(((NetFieldBase<bool, NetBool>) chest2.fridge).Value) : new bool?();
      if (nullable1.GetValueOrDefault() == nullable2.GetValueOrDefault() & nullable1.HasValue == nullable2.HasValue)
        return !flavorSpecific || (a is Object object1 ? object1.GetPreservedItemId() : (string) null) == (b is Object object2 ? object2.GetPreservedItemId() : (string) null);
    }
    return false;
  }

  private RecipeModel[] GetAllRecipes(IMonitor monitor)
  {
    List<RecipeModel> list = ((IEnumerable<RecipeModel>) this.DataParser.GetRecipes(this.Metadata, monitor, this.ExtraMachineConfig)).ToList<RecipeModel>();
    if (this.ProducerFrameworkMod.IsLoaded)
    {
      List<RecipeModel> collection = new List<RecipeModel>();
      foreach (ProducerFrameworkRecipe recipe1 in this.ProducerFrameworkMod.GetRecipes())
      {
        ProducerFrameworkRecipe recipe = recipe1;
        if (recipe.InputId != null)
          list.RemoveAll((Predicate<RecipeModel>) (other => other.Type == RecipeType.MachineInput && other.MachineId == recipe.MachineId && other.Ingredients.Length != 0 && other.Ingredients[0].InputId == recipe.InputId));
        Object @object = ItemRegistry.Create<Object>(recipe.MachineId, 1, 0, true);
        if (@object != null && ItemExtensions.HasTypeBigCraftable((IHaveItemTypeId) @object))
        {
          List<RecipeModel> recipeModelList = collection;
          string displayName = ((Item) @object).DisplayName;
          IEnumerable<RecipeIngredientModel> ingredients = ((IEnumerable<ProducerFrameworkIngredient>) recipe.Ingredients).Select<ProducerFrameworkIngredient, RecipeIngredientModel>((Func<ProducerFrameworkIngredient, RecipeIngredientModel>) (p => new RecipeIngredientModel(RecipeType.MachineInput, p.InputId, p.Count)));
          Func<Item, Item> func = (Func<Item, Item>) (ingredient =>
          {
            Object allRecipes = ItemRegistry.Create<Object>(recipe.OutputId, 1, 0, false);
            if (ingredient != null)
            {
              int parentSheetIndex = ingredient.ParentSheetIndex;
              ((NetFieldBase<string, NetString>) allRecipes.preservedParentSheetIndex).Value = ingredient.ItemId;
              ((NetFieldBase<Object.PreserveType?, NetNullableEnum<Object.PreserveType>>) allRecipes.preserve).Value = recipe.PreserveType;
            }
            return (Item) allRecipes;
          });
          IEnumerable<RecipeIngredientModel> recipeIngredientModels = ((IEnumerable<string>) recipe.ExceptIngredients).Select<string, RecipeIngredientModel>((Func<string, RecipeIngredientModel>) (id => new RecipeIngredientModel(RecipeType.MachineInput, id, 1)));
          string outputId = recipe.OutputId;
          int? nullable1 = new int?(recipe.MinOutput);
          int? nullable2 = new int?(recipe.MaxOutput);
          Decimal? nullable3 = new Decimal?((Decimal) recipe.OutputChance);
          string machineId = ItemRegistry.ManuallyQualifyItemId(recipe.MachineId, "(BC)", false);
          IEnumerable<RecipeIngredientModel> exceptIngredients = recipeIngredientModels;
          string outputQualifiedItemId = outputId;
          int? minOutput = nullable1;
          int? maxOutput = nullable2;
          Decimal? outputChance = nullable3;
          int? quality = new int?();
          RecipeModel recipeModel = new RecipeModel((string) null, RecipeType.MachineInput, displayName, ingredients, 0, func, (Func<bool>) (() => true), machineId, exceptIngredients, outputQualifiedItemId, minOutput, maxOutput, outputChance, quality);
          recipeModelList.Add(recipeModel);
        }
      }
      list.AddRange((IEnumerable<RecipeModel>) collection);
    }
    list.AddRange(this.GetAllTailorRecipes());
    return list.ToArray();
  }

  private IEnumerable<RecipeModel> GetAllTailorRecipes()
  {
    Item[] objectList = ((IEnumerable<SearchableItem>) this.Objects.Value).Select<SearchableItem, Item>((Func<SearchableItem, Item>) (p => p.Item)).ToArray<Item>();
    Dictionary<string, Item[]> contextLookupCache = ((IEnumerable<Item>) objectList).SelectMany((Func<Item, IEnumerable<string>>) (item => (IEnumerable<string>) item.GetContextTags()), (item, tag) => new
    {
      item = item,
      tag = tag
    }).GroupBy(group => group.tag).ToDictionary<IGrouping<string, \u003C\u003Ef__AnonymousType27<Item, string>>, string, Item[]>(group => group.Key, group => group.Select(p => p.item).ToArray<Item>());
    HashSet<string> seenPermutation = new HashSet<string>();
    TailoringMenu tailor = new TailoringMenu();
    foreach (TailorItemRecipe recipe in tailor._tailoringRecipes)
    {
      Item[] clothItems = GetObjectsWithTags(recipe.FirstItemTags);
      Item[] spoolItems = GetObjectsWithTags(recipe.SecondItemTags);
      List<string> craftedItemIds = recipe.CraftedItemIds;
      string[] strArray1;
      if ((craftedItemIds != null ? (craftedItemIds.Any<string>() ? 1 : 0) : 0) != 0)
        strArray1 = recipe.CraftedItemIds.ToArray();
      else if (recipe.CraftedItemIdFeminine != null && ((Character) Game1.player).Gender == 1)
        strArray1 = new string[1]
        {
          recipe.CraftedItemIdFeminine
        };
      else
        strArray1 = new string[1]{ recipe.CraftedItemId };
      string[] strArray = strArray1;
      for (int index1 = 0; index1 < strArray.Length; ++index1)
      {
        string outputId = strArray[index1];
        int result;
        if (!int.TryParse(outputId, out result) || result >= 0)
        {
          Item[] objArray1 = clothItems;
          for (int index2 = 0; index2 < objArray1.Length; ++index2)
          {
            Item clothItem = objArray1[index2];
            Item[] objArray2 = spoolItems;
            for (int index3 = 0; index3 < objArray2.Length; ++index3)
            {
              Item spoolItem = objArray2[index3];
              if (seenPermutation.Add($"{clothItem.QualifiedItemId}|{spoolItem.QualifiedItemId}"))
              {
                Item output;
                try
                {
                  output = this.GetTailoredItem(outputId, tailor, spoolItem);
                }
                catch (Exception ex)
                {
                  this.Monitor.LogOnce($"Failed to get output #{outputId} for tailoring recipe [{string.Join(", ", (IEnumerable<string>) (recipe.FirstItemTags ?? new List<string>()))}] + [{string.Join(", ", (IEnumerable<string>) (recipe.SecondItemTags ?? new List<string>()))}]. Technical details:\n{ex}", (LogLevel) 3);
                  continue;
                }
                // ISSUE: object of a compiler-generated type is created
                yield return new RecipeModel((string) null, RecipeType.TailorInput, I18n.RecipeType_Tailoring(), (IEnumerable<RecipeIngredientModel>) new \u003C\u003Ez__ReadOnlyArray<RecipeIngredientModel>(new RecipeIngredientModel[2]
                {
                  new RecipeIngredientModel(RecipeType.TailorInput, clothItem.QualifiedItemId, 1),
                  new RecipeIngredientModel(RecipeType.TailorInput, spoolItem.QualifiedItemId, 1)
                }), 0, (Func<Item, Item>) (_ => output.getOne()), (Func<bool>) (() => Game1.player.HasTailoredThisItem(output)), (string) null, outputQualifiedItemId: ItemRegistry.QualifyItemId(recipe.CraftedItemId));
              }
            }
            objArray2 = (Item[]) null;
            clothItem = (Item) null;
          }
          objArray1 = (Item[]) null;
          outputId = (string) null;
        }
      }
      strArray = (string[]) null;
      clothItems = (Item[]) null;
      spoolItems = (Item[]) null;
    }

    Item[] GetObjectsWithTags(List<string>? contextTags)
    {
      if (contextTags == null)
        return Array.Empty<Item>();
      if (contextTags.Count == 1 && !contextTags[0].StartsWith("!"))
      {
        Item[] objArray;
        return !contextLookupCache.TryGetValue(contextTags[0], out objArray) ? Array.Empty<Item>() : objArray;
      }
      string key = string.Join("|", (IEnumerable<string>) contextTags.OrderBy<string, string>((Func<string, string>) (p => p)));
      Item[] array;
      if (!contextLookupCache.TryGetValue(key, out array))
        contextLookupCache[key] = array = ((IEnumerable<Item>) objectList).Where<Item>((Func<Item, bool>) (entry => contextTags.All<string>(new Func<string, bool>(entry.HasContextTag)))).ToArray<Item>();
      return array;
    }
  }

  private Item GetTailoredItem(string craftedItemId, TailoringMenu tailor, Item spoolItem)
  {
    Item tailoredItem = ItemRegistry.Create(craftedItemId, 1, 0, false);
    if (tailoredItem is Clothing clothing)
      tailor.DyeItems(clothing, spoolItem, 1f);
    return tailoredItem;
  }

  private GiftTaste? GetGiftTaste(NPC npc, Item item)
  {
    try
    {
      return new GiftTaste?((GiftTaste) npc.getGiftTasteForThisItem(item));
    }
    catch
    {
      return new GiftTaste?();
    }
  }
}
