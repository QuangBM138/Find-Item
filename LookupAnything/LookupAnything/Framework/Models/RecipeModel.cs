// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.Models.RecipeModel
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Netcode;
using Pathoschild.Stardew.Common;
using Pathoschild.Stardew.LookupAnything.Framework.Fields.Models;
using StardewValley;
using StardewValley.Buildings;
using StardewValley.GameData.Buildings;
using StardewValley.Network;
using StardewValley.TokenizableStrings;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework.Models;

internal class RecipeModel
{
  private readonly Func<StardewValley.Item?, StardewValley.Item?>? Item;

  public string? Key { get; }

  public string? MachineId { get; }

  public RecipeType Type { get; }

  public string DisplayType { get; }

  public RecipeIngredientModel[] Ingredients { get; }

  public int GoldPrice { get; }

  public string? OutputQualifiedItemId { get; }

  public int MinOutput { get; }

  public int MaxOutput { get; }

  public Decimal OutputChance { get; set; }

  public RecipeIngredientModel[] ExceptIngredients { get; }

  public Func<bool> IsKnown { get; }

  public RecipeItemEntry? SpecialOutput { get; }

  public int? Quality { get; }

  public string[] Conditions { get; }

  public RecipeModel(
    string? key,
    RecipeType type,
    string displayType,
    IEnumerable<RecipeIngredientModel> ingredients,
    int goldPrice,
    Func<StardewValley.Item?, StardewValley.Item?>? item,
    Func<bool> isKnown,
    string? machineId,
    IEnumerable<RecipeIngredientModel>? exceptIngredients = null,
    string? outputQualifiedItemId = null,
    int? minOutput = null,
    int? maxOutput = null,
    Decimal? outputChance = null,
    int? quality = null,
    string[]? conditions = null)
  {
    if (!minOutput.HasValue && !maxOutput.HasValue)
    {
      minOutput = new int?(1);
      maxOutput = new int?(1);
    }
    else if (!minOutput.HasValue)
      minOutput = maxOutput;
    else if (!maxOutput.HasValue)
      maxOutput = minOutput;
    this.Key = key;
    this.Type = type;
    this.DisplayType = displayType;
    this.Ingredients = ingredients.ToArray<RecipeIngredientModel>();
    this.GoldPrice = goldPrice;
    this.MachineId = machineId;
    this.ExceptIngredients = (exceptIngredients != null ? exceptIngredients.ToArray<RecipeIngredientModel>() : (RecipeIngredientModel[]) null) ?? Array.Empty<RecipeIngredientModel>();
    this.Item = item;
    this.IsKnown = isKnown;
    this.OutputQualifiedItemId = outputQualifiedItemId;
    this.MinOutput = minOutput.Value;
    this.MaxOutput = maxOutput.Value;
    Decimal num;
    if (outputChance.HasValue)
    {
      Decimal valueOrDefault = outputChance.GetValueOrDefault();
      if (valueOrDefault > 0M && valueOrDefault < 100M)
      {
        num = outputChance.Value;
        goto label_10;
      }
    }
    num = 100M;
label_10:
    this.OutputChance = num;
    this.Quality = quality;
    this.Conditions = conditions ?? Array.Empty<string>();
  }

  public RecipeModel(
    CraftingRecipe recipe,
    string? outputQualifiedItemId,
    RecipeIngredientModel[]? ingredients = null)
  {
    string name = recipe.name;
    int type = recipe.isCookingRecipe ? 0 : 1;
    string displayType = recipe.isCookingRecipe ? I18n.RecipeType_Cooking() : I18n.RecipeType_Crafting();
    RecipeIngredientModel[] ingredients1 = ingredients ?? RecipeModel.ParseIngredients(recipe);
    Func<StardewValley.Item, StardewValley.Item> func = (Func<StardewValley.Item, StardewValley.Item>) (_ => recipe.createItem());
    Func<bool> isKnown = (Func<bool>) (() => recipe.name != null && Game1.player.knowsRecipe(recipe.name));
    int? nullable = new int?(recipe.numberProducedPerCraft);
    string outputQualifiedItemId1 = RecipeModel.QualifyRecipeOutputId(recipe, outputQualifiedItemId) ?? outputQualifiedItemId;
    int? minOutput = nullable;
    int? maxOutput = new int?();
    Decimal? outputChance = new Decimal?();
    int? quality = new int?();
    // ISSUE: explicit constructor call
    this.\u002Ector(name, (RecipeType) type, displayType, (IEnumerable<RecipeIngredientModel>) ingredients1, 0, func, isKnown, (string) null, outputQualifiedItemId: outputQualifiedItemId1, minOutput: minOutput, maxOutput: maxOutput, outputChance: outputChance, quality: quality);
  }

  public RecipeModel(Building building, RecipeIngredientModel[] ingredients, int goldPrice)
    : this(TokenParser.ParseText(building.GetData()?.Name, (Random) null, (TokenParserDelegate) null, (Farmer) null) ?? ((NetFieldBase<string, NetString>) building.buildingType).Value, RecipeType.BuildingBlueprint, I18n.Building_Construction(), (IEnumerable<RecipeIngredientModel>) ingredients, goldPrice, (Func<StardewValley.Item, StardewValley.Item>) (_ => (StardewValley.Item) null), (Func<bool>) (() => true), ((NetFieldBase<string, NetString>) building.buildingType).Value)
  {
    this.SpecialOutput = new RecipeItemEntry(new SpriteInfo(building.texture.Value, building.getSourceRectForMenu() ?? building.getSourceRect()), TokenParser.ParseText(building.GetData()?.Name, (Random) null, (TokenParserDelegate) null, (Farmer) null) ?? ((NetFieldBase<string, NetString>) building.buildingType).Value, new int?(), false, Entity: (object) building);
  }

  public RecipeModel(RecipeModel other)
  {
    string key = other.Key;
    int type = (int) other.Type;
    string displayType = other.DisplayType;
    RecipeIngredientModel[] ingredients = other.Ingredients;
    int goldPrice = other.GoldPrice;
    Func<StardewValley.Item, StardewValley.Item> func = other.Item;
    Func<bool> isKnown = other.IsKnown;
    IEnumerable<RecipeIngredientModel> exceptIngredients1 = (IEnumerable<RecipeIngredientModel>) other.ExceptIngredients;
    string outputQualifiedItemId1 = other.OutputQualifiedItemId;
    int? nullable = new int?(other.MinOutput);
    string machineId = other.MachineId;
    IEnumerable<RecipeIngredientModel> exceptIngredients2 = exceptIngredients1;
    string outputQualifiedItemId2 = outputQualifiedItemId1;
    int? minOutput = nullable;
    string[] conditions1 = other.Conditions;
    int? maxOutput = new int?();
    Decimal? outputChance = new Decimal?();
    int? quality = new int?();
    string[] conditions2 = conditions1;
    // ISSUE: explicit constructor call
    this.\u002Ector(key, (RecipeType) type, displayType, (IEnumerable<RecipeIngredientModel>) ingredients, goldPrice, func, isKnown, machineId, exceptIngredients2, outputQualifiedItemId2, minOutput, maxOutput, outputChance, quality, conditions2);
  }

  public static RecipeIngredientModel[] ParseIngredients(CraftingRecipe recipe)
  {
    RecipeType type = recipe.isCookingRecipe ? RecipeType.Cooking : RecipeType.Crafting;
    return recipe.recipeList.Select<KeyValuePair<string, int>, RecipeIngredientModel>((Func<KeyValuePair<string, int>, RecipeIngredientModel>) (p => new RecipeIngredientModel(type, p.Key, p.Value))).ToArray<RecipeIngredientModel>();
  }

  public static RecipeIngredientModel[] ParseIngredients(BuildingData? building)
  {
    int? count = building?.BuildMaterials?.Count;
    return !count.HasValue || count.GetValueOrDefault() <= 0 ? Array.Empty<RecipeIngredientModel>() : building.BuildMaterials.Select<BuildingMaterial, RecipeIngredientModel>((Func<BuildingMaterial, RecipeIngredientModel>) (ingredient => new RecipeIngredientModel(RecipeType.BuildingBlueprint, ingredient.ItemId, ingredient.Amount))).ToArray<RecipeIngredientModel>();
  }

  public bool IsForMachine(Building building)
  {
    return this.MachineId != null && this.MachineId == ((NetFieldBase<string, NetString>) building.buildingType).Value;
  }

  public bool IsForMachine(StardewValley.Item machine)
  {
    return this.MachineId != null && this.MachineId == machine.QualifiedItemId;
  }

  public StardewValley.Item? TryCreateItem(StardewValley.Item? ingredient)
  {
    if (this.Item == null)
      return (StardewValley.Item) null;
    try
    {
      return this.Item(ingredient);
    }
    catch
    {
      return (StardewValley.Item) null;
    }
  }

  public int GetTimesCrafted(Farmer player)
  {
    switch (this.Type)
    {
      case RecipeType.Cooking:
        string itemId = ItemRegistry.GetData(this.OutputQualifiedItemId)?.ItemId;
        int num1;
        return itemId != null && ((NetDictionary<string, int, NetInt, SerializableDictionary<string, int>, NetStringDictionary<int, NetInt>>) player.recipesCooked).TryGetValue(itemId, ref num1) ? num1 : 0;
      case RecipeType.Crafting:
        int num2;
        return !((NetDictionary<string, int, NetInt, SerializableDictionary<string, int>, NetStringDictionary<int, NetInt>>) player.craftingRecipes).TryGetValue(this.Key, ref num2) ? 0 : num2;
      default:
        return -1;
    }
  }

  public static string? QualifyRecipeOutputId(CraftingRecipe recipe, string? itemId)
  {
    return !recipe.bigCraftable ? ItemRegistry.QualifyItemId(itemId) : ItemRegistry.ManuallyQualifyItemId(itemId, "(BC)", false);
  }
}
