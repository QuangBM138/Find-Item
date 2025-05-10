// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.Lookups.Items.ItemSubject
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Netcode;
using Pathoschild.Stardew.Common;
using Pathoschild.Stardew.Common.DataParsers;
using Pathoschild.Stardew.Common.Utilities;
using Pathoschild.Stardew.LookupAnything.Framework.Constants;
using Pathoschild.Stardew.LookupAnything.Framework.Data;
using Pathoschild.Stardew.LookupAnything.Framework.DebugFields;
using Pathoschild.Stardew.LookupAnything.Framework.Fields;
using Pathoschild.Stardew.LookupAnything.Framework.Models;
using StardewModdingAPI;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Buildings;
using StardewValley.Extensions;
using StardewValley.GameData.Crops;
using StardewValley.GameData.FishPonds;
using StardewValley.GameData.Movies;
using StardewValley.ItemTypeDefinitions;
using StardewValley.Locations;
using StardewValley.Network;
using StardewValley.Objects;
using StardewValley.TerrainFeatures;
using StardewValley.TokenizableStrings;
using StardewValley.Tools;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework.Lookups.Items;

internal class ItemSubject : BaseSubject
{
  private readonly Item Target;
  private readonly Crop? FromCrop;
  private readonly HoeDirt? FromDirt;
  private readonly Crop? SeedForCrop;
  private readonly ObjectContext Context;
  private readonly bool KnownQuality;
  private readonly GameLocation? Location;
  public bool ShowUncaughtFishSpawnRules;
  private readonly bool ShowUnknownGiftTastes;
  private readonly bool HighlightUnrevealedGiftTastes;
  private readonly ModGiftTasteConfig ShowGiftTastes;
  private readonly bool ShowUnknownRecipes;
  private readonly bool ShowInvalidRecipes;
  private readonly ModCollapseLargeFieldsConfig CollapseFieldsConfig;
  private readonly ISubjectRegistry Codex;
  private readonly Func<Crop, ObjectContext, HoeDirt?, ISubject> GetCropSubject;

  public ItemSubject(
    ISubjectRegistry codex,
    GameHelper gameHelper,
    bool showUncaughtFishSpawnRules,
    bool showUnknownGiftTastes,
    bool highlightUnrevealedGiftTastes,
    ModGiftTasteConfig showGiftTastes,
    bool showUnknownRecipes,
    bool showInvalidRecipes,
    ModCollapseLargeFieldsConfig collapseFieldsConfig,
    Item item,
    ObjectContext context,
    bool knownQuality,
    GameLocation? location,
    Func<Crop, ObjectContext, HoeDirt?, ISubject> getCropSubject,
    Crop? fromCrop = null,
    HoeDirt? fromDirt = null)
    : base(gameHelper)
  {
    this.Codex = codex;
    this.ShowUncaughtFishSpawnRules = showUncaughtFishSpawnRules;
    this.ShowUnknownGiftTastes = showUnknownGiftTastes;
    this.HighlightUnrevealedGiftTastes = highlightUnrevealedGiftTastes;
    this.ShowGiftTastes = showGiftTastes;
    this.ShowUnknownRecipes = showUnknownRecipes;
    this.ShowInvalidRecipes = showInvalidRecipes;
    this.CollapseFieldsConfig = collapseFieldsConfig;
    this.Target = item;
    this.FromCrop = fromCrop ?? fromDirt?.crop;
    this.FromDirt = fromDirt;
    this.Context = context;
    this.Location = location;
    this.KnownQuality = knownQuality;
    this.GetCropSubject = getCropSubject;
    this.SeedForCrop = item.QualifiedItemId != "(O)433" || this.FromCrop == null ? this.TryGetCropForSeed(item, location) : (Crop) null;
    this.Initialize(this.Target.DisplayName, this.GetDescription(this.Target), this.GetTypeValue(this.Target));
  }

  public override IEnumerable<ICustomField> GetData()
  {
    ItemSubject itemSubject = this;
    Item item = itemSubject.Target;
    ParsedItemData itemData = ItemRegistry.GetDataOrErrorItem(item.QualifiedItemId);
    Object obj = item as Object;
    bool isCrop = itemSubject.FromCrop != null;
    bool isSeed = itemSubject.SeedForCrop != null;
    Crop fromCrop = itemSubject.FromCrop;
    bool isDeadCrop = fromCrop != null && ((NetFieldBase<bool, NetBool>) fromCrop.dead).Value;
    bool canSell = item.canBeShipped() || ((IEnumerable<ShopData>) itemSubject.Metadata.Shops).Any<ShopData>((Func<ShopData, bool>) (shop => ((IEnumerable<int>) shop.BuysCategories).Contains<int>(item.Category)));
    bool isMovieTicket = item.QualifiedItemId == "(O)809";
    Object @object = obj;
    bool showInventoryFields = @object == null || !@object.IsBreakableStone();
    ItemData itemData1 = itemSubject.Metadata.GetObject(item, itemSubject.Context);
    bool? nullable;
    if (itemData1 != (ItemData) null)
    {
      // ISSUE: explicit non-virtual call
      itemSubject.Name = itemData1.NameKey != null ? Translation.op_Implicit(I18n.GetByKey(itemData1.NameKey)) : __nonvirtual (itemSubject.Name);
      // ISSUE: explicit non-virtual call
      itemSubject.Description = itemData1.DescriptionKey != null ? Translation.op_Implicit(I18n.GetByKey(itemData1.DescriptionKey)) : __nonvirtual (itemSubject.Description);
      // ISSUE: explicit non-virtual call
      itemSubject.Type = itemData1.TypeKey != null ? Translation.op_Implicit(I18n.GetByKey(itemData1.TypeKey)) : __nonvirtual (itemSubject.Type);
      nullable = itemData1.ShowInventoryFields;
      showInventoryFields = nullable ?? showInventoryFields;
    }
    IModInfo modFromStringId = itemSubject.GameHelper.TryGetModFromStringId(item.ItemId);
    if (modFromStringId != null)
    {
      string label = I18n.AddedByMod();
      string str = I18n.AddedByMod_Summary((object) modFromStringId.Manifest.Name);
      nullable = new bool?();
      bool? hasValue = nullable;
      yield return (ICustomField) new GenericField(label, str, hasValue);
    }
    if (isDeadCrop)
    {
      string label = I18n.Crop_Summary();
      string str = I18n.Crop_Summary_Dead();
      nullable = new bool?();
      bool? hasValue = nullable;
      yield return (ICustomField) new GenericField(label, str, hasValue);
    }
    else
    {
      foreach (ICustomField cropField in itemSubject.GetCropFields(itemSubject.FromDirt, itemSubject.FromCrop ?? itemSubject.SeedForCrop, obj, isSeed))
        yield return cropField;
      IndoorPot pot = item as IndoorPot;
      if (pot != null)
      {
        Crop potCrop = ((NetFieldBase<HoeDirt, NetRef<HoeDirt>>) pot.hoeDirt).Value.crop;
        Bush potBush = ((NetFieldBase<Bush, NetRef<Bush>>) pot.bush).Value;
        if (potCrop != null)
        {
          string displayName = ItemRegistry.GetDataOrErrorItem(((NetFieldBase<string, NetString>) potCrop.indexOfHarvest).Value).DisplayName;
          yield return (ICustomField) new LinkField(I18n.Item_Contents(), displayName, (Func<ISubject>) (() => this.GetCropSubject(potCrop, ObjectContext.World, ((NetFieldBase<HoeDirt, NetRef<HoeDirt>>) pot.hoeDirt).Value)));
        }
        if (potBush != null)
        {
          ISubject subject = itemSubject.Codex.GetByEntity((object) potBush, itemSubject.Location ?? ((TerrainFeature) potBush).Location);
          if (subject != null)
            yield return (ICustomField) new LinkField(I18n.Item_Contents(), subject.Name, (Func<ISubject>) (() => subject));
        }
        potBush = (Bush) null;
      }
      foreach (ICustomField machineOutputField in itemSubject.GetMachineOutputFields(obj))
        yield return machineOutputField;
      if (((Item) obj)?.Name == "Flute Block")
      {
        string label = I18n.Item_MusicBlock_Pitch();
        string str = I18n.Generic_Ratio((object) ((NetFieldBase<string, NetString>) obj.preservedParentSheetIndex).Value, (object) 2300);
        nullable = new bool?();
        bool? hasValue = nullable;
        yield return (ICustomField) new GenericField(label, str, hasValue);
      }
      else if (((Item) obj)?.Name == "Drum Block")
      {
        string label = I18n.Item_MusicBlock_DrumType();
        string str = I18n.Generic_Ratio((object) ((NetFieldBase<string, NetString>) obj.preservedParentSheetIndex).Value, (object) 6);
        nullable = new bool?();
        bool? hasValue = nullable;
        yield return (ICustomField) new GenericField(label, str, hasValue);
      }
      if (showInventoryFields)
      {
        foreach (ICustomField neededForField in itemSubject.GetNeededForFields(obj))
          yield return neededForField;
        if (canSell && !isCrop)
        {
          string saleValueString = GenericField.GetSaleValueString(itemSubject.GetSaleValue(item, itemSubject.KnownQuality), item.Stack);
          string label = I18n.Item_SellsFor();
          string str = saleValueString;
          nullable = new bool?();
          bool? hasValue = nullable;
          yield return (ICustomField) new GenericField(label, str, hasValue);
          List<string> values = new List<string>();
          if (item.canBeShipped())
            values.Add(I18n.Item_SellsTo_ShippingBox());
          values.AddRange(((IEnumerable<ShopData>) itemSubject.Metadata.Shops).Where<ShopData>((Func<ShopData, bool>) (shop => ((IEnumerable<int>) shop.BuysCategories).Contains<int>(item.Category))).Select(shop => new
          {
            shop = shop,
            name = I18n.GetByKey(shop.DisplayKey).ToString()
          }).OrderBy(_param1 => _param1.name).Select(_param1 => _param1.name));
          yield return (ICustomField) new GenericField(I18n.Item_SellsTo(), I18n.List((IEnumerable<object>) values));
        }
        if (item is Clothing clothing)
        {
          string label = I18n.Item_CanBeDyed();
          string str = itemSubject.Stringify((object) ((NetFieldBase<bool, NetBool>) clothing.dyeable).Value);
          nullable = new bool?();
          bool? hasValue = nullable;
          yield return (ICustomField) new GenericField(label, str, hasValue);
        }
        if (!isMovieTicket)
        {
          IDictionary<GiftTaste, GiftTasteModel[]> giftTastes = itemSubject.GetGiftTastes(item);
          if (itemSubject.ShowGiftTastes.Loved)
            yield return (ICustomField) new ItemGiftTastesField(I18n.Item_LovesThis(), giftTastes, GiftTaste.Love, itemSubject.ShowUnknownGiftTastes, itemSubject.HighlightUnrevealedGiftTastes);
          if (itemSubject.ShowGiftTastes.Liked)
            yield return (ICustomField) new ItemGiftTastesField(I18n.Item_LikesThis(), giftTastes, GiftTaste.Like, itemSubject.ShowUnknownGiftTastes, itemSubject.HighlightUnrevealedGiftTastes);
          if (itemSubject.ShowGiftTastes.Neutral)
            yield return (ICustomField) new ItemGiftTastesField(I18n.Item_NeutralAboutThis(), giftTastes, GiftTaste.Neutral, itemSubject.ShowUnknownGiftTastes, itemSubject.HighlightUnrevealedGiftTastes);
          if (itemSubject.ShowGiftTastes.Disliked)
            yield return (ICustomField) new ItemGiftTastesField(I18n.Item_DislikesThis(), giftTastes, GiftTaste.Dislike, itemSubject.ShowUnknownGiftTastes, itemSubject.HighlightUnrevealedGiftTastes);
          if (itemSubject.ShowGiftTastes.Hated)
            yield return (ICustomField) new ItemGiftTastesField(I18n.Item_HatesThis(), giftTastes, GiftTaste.Hate, itemSubject.ShowUnknownGiftTastes, itemSubject.HighlightUnrevealedGiftTastes);
          giftTastes = (IDictionary<GiftTaste, GiftTasteModel[]>) null;
        }
      }
      MeleeWeapon weapon = item as MeleeWeapon;
      if (weapon != null && !((Tool) weapon).isScythe())
      {
        int accuracy = ((NetFieldBase<int, NetInt>) weapon.addedPrecision).Value;
        float critChance = ((NetFieldBase<float, NetFloat>) weapon.critChance).Value;
        float critMultiplier = ((NetFieldBase<float, NetFloat>) weapon.critMultiplier).Value;
        int min = ((NetFieldBase<int, NetInt>) weapon.minDamage).Value;
        int max = ((NetFieldBase<int, NetInt>) weapon.maxDamage).Value;
        int defense = ((NetFieldBase<int, NetInt>) weapon.addedDefense).Value;
        float knockback = ((NetFieldBase<float, NetFloat>) weapon.knockback).Value;
        int speed = ((NetFieldBase<int, NetInt>) weapon.speed).Value;
        int reach = ((NetFieldBase<int, NetInt>) weapon.addedAreaOfEffect).Value;
        int shownKnockback = (int) Math.Ceiling((double) Math.Abs(knockback - weapon.defaultKnockBackForThisType(((NetFieldBase<int, NetInt>) weapon.type).Value)) * 10.0);
        int shownSpeed = (speed - (((NetFieldBase<int, NetInt>) weapon.type).Value == 2 ? -8 : 0)) / 2;
        string label = I18n.Item_MeleeWeapon_Damage();
        string str1 = min != max ? I18n.Generic_Range((object) min, (object) max) : min.ToString();
        nullable = new bool?();
        bool? hasValue = nullable;
        yield return (ICustomField) new GenericField(label, str1, hasValue);
        yield return (ICustomField) new GenericField(I18n.Item_MeleeWeapon_CriticalChance(), I18n.Generic_Percent((object) (float) ((double) critChance * 100.0)));
        yield return (ICustomField) new GenericField(I18n.Item_MeleeWeapon_CriticalDamage(), I18n.Item_MeleeWeapon_CriticalDamage_Label((object) critMultiplier));
        yield return (ICustomField) new GenericField(I18n.Item_MeleeWeapon_Defense(), defense == 0 ? "0" : I18n.Item_MeleeWeapon_Defense_Label((object) AddSign((float) ((NetFieldBase<int, NetInt>) weapon.addedDefense).Value)));
        if (speed == 0)
        {
          yield return (ICustomField) new GenericField(I18n.Item_MeleeWeapon_Speed(), "0");
        }
        else
        {
          string str2 = I18n.Item_MeleeWeapon_Speed_Summary((object) AddSign((float) speed), (object) AddSign((float) (-speed * 40)));
          if (speed != shownSpeed)
          {
            string shownSpeed1 = AddSign((float) shownSpeed);
            object obj1 = (object) str2;
            string newLine = Environment.NewLine;
            object actualSpeed = obj1;
            str2 = I18n.Item_MeleeWeapon_Speed_ShownVsActual((object) shownSpeed1, (object) newLine, actualSpeed);
          }
          yield return (ICustomField) new GenericField(I18n.Item_MeleeWeapon_Speed(), str2);
        }
        yield return (ICustomField) new GenericField(I18n.Item_MeleeWeapon_Knockback(), (double) knockback > 1.0 ? I18n.Item_MeleeWeapon_Knockback_Label((object) AddSign((float) shownKnockback), (object) knockback) : "0");
        yield return (ICustomField) new GenericField(I18n.Item_MeleeWeapon_Reach(), reach > 0 ? I18n.Item_MeleeWeapon_Reach_Label((object) AddSign((float) reach)) : "0");
        yield return (ICustomField) new GenericField(I18n.Item_MeleeWeapon_Accuracy(), AddSign((float) accuracy));
      }
      if (showInventoryFields)
      {
        RecipeModel[] array = itemSubject.GameHelper.GetRecipesForIngredient(item).Concat<RecipeModel>(itemSubject.GameHelper.GetRecipesForOutput(item)).Concat<RecipeModel>(itemSubject.GameHelper.GetRecipesForMachine(item as Object)).ToArray<RecipeModel>();
        if (array.Length != 0)
        {
          ItemRecipesField itemRecipesField = new ItemRecipesField(itemSubject.GameHelper, itemSubject.Codex, I18n.Item_Recipes(), item, array, itemSubject.ShowUnknownRecipes, itemSubject.ShowInvalidRecipes);
          if (itemSubject.CollapseFieldsConfig.Enabled)
            itemRecipesField.CollapseIfLengthExceeds(itemSubject.CollapseFieldsConfig.ItemRecipes, array.Length);
          yield return (ICustomField) itemRecipesField;
        }
      }
      yield return (ICustomField) new FishSpawnRulesField(itemSubject.GameHelper, I18n.Item_FishSpawnRules(), itemData, itemSubject.ShowUncaughtFishSpawnRules);
      bool flag1 = ItemExtensions.HasTypeObject((IHaveItemTypeId) item);
      if (flag1)
      {
        bool flag2 = item.Category == -4;
        if (!flag2)
        {
          string qualifiedItemId = item.QualifiedItemId;
          flag2 = qualifiedItemId == "(O)393" || qualifiedItemId == "(O)397";
        }
        flag1 = flag2;
      }
      if (flag1)
      {
        FishPondData rawData = FishPond.GetRawData(item.ItemId);
        if (rawData != null)
        {
          string preface = I18n.Building_FishPond_Drops_Preface((object) I18n.Generic_Range((object) (int) Math.Round((double) Utility.Lerp(0.15f, 0.95f, 0.1f) * 100.0), (object) (int) Math.Round((double) Utility.Lerp(0.15f, 0.95f, 1f) * 100.0)));
          yield return (ICustomField) new FishPondDropsField(itemSubject.GameHelper, itemSubject.Codex, I18n.Item_FishPondDrops(), -1, rawData, obj, preface);
        }
      }
      if (item is Fence fence)
      {
        string label = I18n.Item_FenceHealth();
        if (((GameLocation) Game1.getFarm()).isBuildingConstructed(Constant.BuildingNames.GoldClock))
        {
          yield return (ICustomField) new GenericField(label, I18n.Item_FenceHealth_GoldClock());
        }
        else
        {
          float maxValue = ((NetFieldBase<bool, NetBool>) fence.isGate).Value ? ((NetFieldBase<float, NetFloat>) fence.maxHealth).Value * 2f : ((NetFieldBase<float, NetFloat>) fence.maxHealth).Value;
          float num = ((NetFieldBase<float, NetFloat>) fence.health).Value / maxValue;
          double count = Math.Round((double) ((NetFieldBase<float, NetFloat>) fence.health).Value * (double) itemSubject.Constants.FenceDecayRate / 60.0 / 24.0);
          double percent = Math.Round((double) num * 100.0);
          yield return (ICustomField) new PercentageBarField(label, (int) ((NetFieldBase<float, NetFloat>) fence.health).Value, (int) maxValue, Color.Green, Color.Red, I18n.Item_FenceHealth_Summary((object) (int) percent, (object) (int) count));
        }
      }
      if (isMovieTicket)
      {
        MovieData movieForDate = MovieTheater.GetMovieForDate(Game1.Date);
        if (movieForDate == null)
        {
          yield return (ICustomField) new GenericField(I18n.Item_MovieTicket_MovieThisWeek(), I18n.Item_MovieTicket_MovieThisWeek_None());
        }
        else
        {
          // ISSUE: object of a compiler-generated type is created
          yield return (ICustomField) new GenericField(I18n.Item_MovieTicket_MovieThisWeek(), (IEnumerable<IFormattedText>) new \u003C\u003Ez__ReadOnlyArray<IFormattedText>(new IFormattedText[3]
          {
            (IFormattedText) new FormattedText(TokenParser.ParseText(movieForDate.Title, (Random) null, (TokenParserDelegate) null, (Farmer) null), bold: true),
            (IFormattedText) new FormattedText(Environment.NewLine),
            (IFormattedText) new FormattedText(TokenParser.ParseText(movieForDate.Description, (Random) null, (TokenParserDelegate) null, (Farmer) null))
          }));
          IDictionary<GiftTaste, string[]> tastes = (IDictionary<GiftTaste, string[]>) itemSubject.GameHelper.GetMovieTastes().GroupBy<KeyValuePair<NPC, GiftTaste?>, GiftTaste>((Func<KeyValuePair<NPC, GiftTaste?>, GiftTaste>) (entry => entry.Value ?? ~GiftTaste.Love)).ToDictionary<IGrouping<GiftTaste, KeyValuePair<NPC, GiftTaste?>>, GiftTaste, string[]>((Func<IGrouping<GiftTaste, KeyValuePair<NPC, GiftTaste?>>, GiftTaste>) (group => group.Key), (Func<IGrouping<GiftTaste, KeyValuePair<NPC, GiftTaste?>>, string[]>) (group => group.Select<KeyValuePair<NPC, GiftTaste?>, string>((Func<KeyValuePair<NPC, GiftTaste?>, string>) (p => ((Character) p.Key).displayName)).OrderBy<string, string>((Func<string, string>) (p => p)).ToArray<string>()));
          yield return (ICustomField) new MovieTastesField(I18n.Item_MovieTicket_LovesMovie(), tastes, GiftTaste.Love);
          yield return (ICustomField) new MovieTastesField(I18n.Item_MovieTicket_LikesMovie(), tastes, GiftTaste.Like);
          yield return (ICustomField) new MovieTastesField(I18n.Item_MovieTicket_DislikesMovie(), tastes, GiftTaste.Dislike);
          yield return (ICustomField) new MovieTastesField(I18n.Item_MovieTicket_RejectsMovie(), tastes, ~GiftTaste.Love);
          tastes = (IDictionary<GiftTaste, string[]>) null;
        }
      }
      if (showInventoryFields)
        yield return (ICustomField) new ColorField(I18n.Item_ProducesDye(), item);
      if (showInventoryFields && !isCrop)
      {
        yield return (ICustomField) new GenericField(I18n.Item_NumberOwned(), itemSubject.GetNumberOwnedText(item));
        RecipeModel[] array = itemSubject.GameHelper.GetRecipes().Where<RecipeModel>((Func<RecipeModel, bool>) (recipe => recipe.OutputQualifiedItemId == this.Target.QualifiedItemId)).ToArray<RecipeModel>();
        if (((IEnumerable<RecipeModel>) array).Any<RecipeModel>())
        {
          string label = ((IEnumerable<RecipeModel>) array).First<RecipeModel>().Type == RecipeType.Cooking ? I18n.Item_NumberCooked() : I18n.Item_NumberCrafted();
          int count = ((IEnumerable<RecipeModel>) array).Sum<RecipeModel>((Func<RecipeModel, int>) (recipe => recipe.GetTimesCrafted(Game1.player)));
          if (count >= 0)
            yield return (ICustomField) new GenericField(label, I18n.Item_NumberCrafted_Summary((object) count));
        }
      }
      bool flag3 = isSeed && item.ItemId != ((NetFieldBase<string, NetString>) itemSubject.SeedForCrop.indexOfHarvest).Value;
      if (flag3)
      {
        string itemId = item.ItemId;
        flag3 = !(itemId == "495") && !(itemId == "496") && !(itemId == "497");
      }
      if (flag3 && item.ItemId != "770")
      {
        string displayName = ItemRegistry.GetDataOrErrorItem(((NetFieldBase<string, NetString>) itemSubject.SeedForCrop.indexOfHarvest).Value).DisplayName;
        yield return (ICustomField) new LinkField(I18n.Item_SeeAlso(), displayName, (Func<ISubject>) (() => this.GetCropSubject(this.SeedForCrop, ObjectContext.Inventory, (HoeDirt) null)));
      }
    }

    static string AddSign(float value) => ((double) value > 0.0 ? "+" : "") + value.ToString();
  }

  public override IEnumerable<IDebugField> GetDebugFields()
  {
    ItemSubject itemSubject = this;
    Item target = itemSubject.Target;
    Object obj = target as Object;
    Crop crop = itemSubject.FromCrop ?? itemSubject.SeedForCrop;
    yield return (IDebugField) new GenericDebugField("item ID", target.QualifiedItemId, pinned: true);
    yield return (IDebugField) new GenericDebugField("sprite index", target.ParentSheetIndex, pinned: true);
    yield return (IDebugField) new GenericDebugField("category", $"{target.Category} ({target.getCategoryName()})", pinned: true);
    if (obj != null)
    {
      yield return (IDebugField) new GenericDebugField("edibility", obj.Edibility, pinned: true);
      yield return (IDebugField) new GenericDebugField("item type", obj.Type, pinned: true);
    }
    if (crop != null)
    {
      yield return (IDebugField) new GenericDebugField("crop fully grown", itemSubject.Stringify((object) ((NetFieldBase<bool, NetBool>) crop.fullyGrown).Value), pinned: true);
      yield return (IDebugField) new GenericDebugField("crop phase", $"{crop.currentPhase} (day {crop.dayOfCurrentPhase} in phase)", pinned: true);
    }
    yield return (IDebugField) new GenericDebugField("context tags", I18n.List((IEnumerable<object>) target.GetContextTags().OrderBy<string, string>((Func<string, string>) (p => p), (IComparer<string>) new HumanSortComparer())), pinned: true);
    foreach (IDebugField debugField in itemSubject.GetDebugFieldsFrom((object) target))
      yield return debugField;
    if (crop != null)
    {
      foreach (IDebugField debugField in itemSubject.GetDebugFieldsFrom((object) crop))
        yield return (IDebugField) new GenericDebugField("crop::" + debugField.Label, debugField.Value, new bool?(debugField.HasValue), debugField.IsPinned);
    }
  }

  public override bool DrawPortrait(SpriteBatch spriteBatch, Vector2 position, Vector2 size)
  {
    this.Target.drawInMenu(spriteBatch, position, 1f, 1f, 1f, (StackDrawType) 0, Color.White, false);
    return true;
  }

  private string? GetDescription(Item item)
  {
    try
    {
      string displayName = item.DisplayName;
      return !(item is MeleeWeapon meleeWeapon) || ((Tool) meleeWeapon).isScythe() ? item.getDescription() : ((Tool) meleeWeapon).Description;
    }
    catch (KeyNotFoundException ex)
    {
      return (string) null;
    }
  }

  private string GetTypeValue(Item item)
  {
    string categoryName = item.getCategoryName();
    return string.IsNullOrWhiteSpace(categoryName) ? I18n.Type_Other() : categoryName;
  }

  private Crop? TryGetCropForSeed(Item seed, GameLocation? location)
  {
    if (!ItemExtensions.HasTypeId((IHaveItemTypeId) seed, "(O)"))
      return (Crop) null;
    try
    {
      CropData cropData;
      return Crop.TryGetData(seed.ItemId, ref cropData) ? new Crop(seed.ItemId, 0, 0, (GameLocation) ((object) location ?? (object) Game1.getFarm())) : (Crop) null;
    }
    catch
    {
      return (Crop) null;
    }
  }

  private IEnumerable<ICustomField> GetCropFields(
    HoeDirt? dirt,
    Crop? crop,
    Object? producedItem,
    bool isSeed)
  {
    ItemSubject itemSubject = this;
    CropDataParser data = new CropDataParser(crop, !isSeed);
    if (data.CropData != null && crop != null)
    {
      bool isForage = CommonHelper.IsItemId(((NetFieldBase<string, NetString>) crop.whichForageCrop).Value, false) && ((NetFieldBase<bool, NetBool>) crop.fullyGrown).Value;
      if (!isSeed && !isForage)
      {
        SDate nextHarvest = data.GetNextHarvest();
        string str = !data.CanHarvestNow ? (Game1.currentLocation.SeedsIgnoreSeasonsHere() || ((IEnumerable<Season>) data.Seasons).Contains<Season>(nextHarvest.Season) ? $"{itemSubject.Stringify((object) nextHarvest)} ({itemSubject.GetRelativeDateStr(nextHarvest)})" : I18n.Crop_Harvest_TooLate((object) itemSubject.Stringify((object) nextHarvest))) : I18n.Generic_Now();
        yield return (ICustomField) new GenericField(I18n.Crop_Harvest(), str);
      }
      if (!isForage)
      {
        List<string> values = new List<string>();
        if (!((NetFieldBase<bool, NetBool>) crop.forageCrop).Value)
          values.Add(data.HasMultipleHarvests ? I18n.Crop_Summary_HarvestMulti((object) data.DaysToFirstHarvest, (object) data.DaysToSubsequentHarvest) : I18n.Crop_Summary_HarvestOnce((object) data.DaysToFirstHarvest));
        values.Add(I18n.Crop_Summary_Seasons((object) I18n.List((IEnumerable<object>) I18n.GetSeasonNames((IEnumerable<Season>) data.Seasons))));
        if (data.CropData != null)
        {
          int harvestMinStack = data.CropData.HarvestMinStack;
          int harvestMaxStack = data.CropData.HarvestMaxStack;
          double extraHarvestChance = data.CropData.ExtraHarvestChance;
          if (harvestMinStack != harvestMaxStack)
            values.Add(I18n.Crop_Summary_DropsXToY((object) harvestMinStack, (object) harvestMaxStack, (object) (int) Math.Round(extraHarvestChance * 100.0, 2)));
          else if (harvestMinStack > 1)
            values.Add(I18n.Crop_Summary_DropsX((object) harvestMinStack));
        }
        else
          values.Add(I18n.Crop_Summary_DropsX((object) 1));
        if (((NetFieldBase<bool, NetBool>) crop.forageCrop).Value)
        {
          values.Add(I18n.Crop_Summary_ForagingXp((object) 3));
        }
        else
        {
          Object @object = producedItem;
          int amount = (int) Math.Round(16.0 * Math.Log(0.018 * (@object != null ? (double) @object.Price : 0.0) + 1.0, Math.E));
          values.Add(I18n.Crop_Summary_FarmingXp((object) amount));
        }
        Item sampleDrop = data.GetSampleDrop();
        values.Add(I18n.Crop_Summary_SellsFor((object) GenericField.GetSaleValueString(itemSubject.GetSaleValue(sampleDrop, false), 1)));
        yield return (ICustomField) new GenericField(I18n.Crop_Summary(), "-" + string.Join(Environment.NewLine + "-", (IEnumerable<string>) values));
      }
      if (dirt != null && !isForage)
      {
        yield return (ICustomField) new GenericField(I18n.Crop_Watered(), itemSubject.Stringify((object) (((NetFieldBase<int, NetInt>) dirt.state).Value == 1)));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        string[] array = itemSubject.GetAppliedFertilizers(dirt).Select<string, string>(ItemSubject.\u003C\u003EO.\u003C0\u003E__GetObjectName ?? (ItemSubject.\u003C\u003EO.\u003C0\u003E__GetObjectName = new Func<string, string>(GameI18n.GetObjectName))).Distinct<string>().DefaultIfEmpty<string>(itemSubject.Stringify((object) false)).OrderBy<string, string>((Func<string, string>) (p => p)).ToArray<string>();
        yield return (ICustomField) new GenericField(I18n.Crop_Fertilized(), I18n.List((IEnumerable<object>) array));
      }
    }
  }

  private IEnumerable<string> GetAppliedFertilizers(HoeDirt dirt)
  {
    if (this.GameHelper.MultiFertilizer.IsLoaded)
      return this.GameHelper.MultiFertilizer.GetAppliedFertilizers(dirt);
    // ISSUE: object of a compiler-generated type is created
    return ItemRegistry.QualifyItemId(((NetFieldBase<string, NetString>) dirt.fertilizer).Value) != null ? (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(((NetFieldBase<string, NetString>) dirt.fertilizer).Value) : (IEnumerable<string>) Array.Empty<string>();
  }

  private IEnumerable<ICustomField> GetMachineOutputFields(Object? machine)
  {
    ItemSubject itemSubject = this;
    if (machine != null)
    {
      Object heldObj = ((NetFieldBase<Object, NetRef<Object>>) machine.heldObject).Value;
      int minutesLeft = machine.MinutesUntilReady;
      Cask cask = machine as Cask;
      if (cask != null)
      {
        if (heldObj != null)
        {
          ItemQuality curQuality = (ItemQuality) ((Item) heldObj).Quality;
          float effectiveAge = (float) itemSubject.Constants.CaskAgeSchedule.Values.Max() - ((NetFieldBase<float, NetFloat>) cask.daysToMature).Value;
          \u003C\u003Ef__AnonymousType14<ItemQuality, int, SDate>[] schedule = itemSubject.Constants.CaskAgeSchedule.Select(entry => new
          {
            entry = entry,
            quality = entry.Key
          }).Select(_param1 => new
          {
            \u003C\u003Eh__TransparentIdentifier0 = _param1,
            baseDays = _param1.entry.Value
          }).Where(_param1 => (double) _param1.baseDays > (double) effectiveAge).OrderBy(_param1 => _param1.baseDays).Select(_param1 => new
          {
            \u003C\u003Eh__TransparentIdentifier1 = _param1,
            daysLeft = (int) Math.Ceiling(((double) _param1.baseDays - (double) effectiveAge) / (double) ((NetFieldBase<float, NetFloat>) cask.agingRate).Value)
          }).Select(_param1 => new
          {
            Quality = _param1.\u003C\u003Eh__TransparentIdentifier1.\u003C\u003Eh__TransparentIdentifier0.quality,
            DaysLeft = _param1.daysLeft,
            HarvestDate = SDate.Now().AddDays(_param1.daysLeft)
          }).ToArray();
          yield return (ICustomField) new ItemIconField(itemSubject.GameHelper, I18n.Item_Contents(), (Item) heldObj, itemSubject.Codex);
          if (minutesLeft <= 0 || !schedule.Any())
          {
            yield return (ICustomField) new GenericField(I18n.Item_CaskSchedule(), I18n.Item_CaskSchedule_Now((object) I18n.For(curQuality)));
          }
          else
          {
            string str = string.Join(Environment.NewLine, schedule.Select(entry => new
            {
              entry = entry,
              str = I18n.GetPlural(entry.DaysLeft, I18n.Item_CaskSchedule_Tomorrow((object) I18n.For(entry.Quality)), I18n.Item_CaskSchedule_InXDays((object) I18n.For(entry.Quality), (object) entry.DaysLeft, (object) this.Stringify((object) entry.HarvestDate)))
            }).Select(_param1 => "-" + _param1.str));
            yield return (ICustomField) new GenericField(I18n.Item_CaskSchedule(), I18n.Item_CaskSchedule_NowPartial((object) I18n.For(curQuality)) + Environment.NewLine + str);
          }
          schedule = null;
        }
      }
      else
      {
        switch (machine)
        {
          case CrabPot crabPot:
            if (heldObj == null)
            {
              if (((NetFieldBase<Object, NetRef<Object>>) crabPot.bait).Value != null)
                yield return (ICustomField) new ItemIconField(itemSubject.GameHelper, I18n.Item_CrabpotBait(), (Item) ((NetFieldBase<Object, NetRef<Object>>) crabPot.bait).Value, itemSubject.Codex);
              else if (((NetHashSet<int>) Game1.player.professions).Contains(11))
                yield return (ICustomField) new GenericField(I18n.Item_CrabpotBait(), I18n.Item_CrabpotBaitNotNeeded());
              else
                yield return (ICustomField) new GenericField(I18n.Item_CrabpotBait(), I18n.Item_CrabpotBaitNeeded());
            }
            if (heldObj == null)
              break;
            string text1 = I18n.Item_Contents_Ready((object) ((Item) heldObj).DisplayName);
            yield return (ICustomField) new ItemIconField(itemSubject.GameHelper, I18n.Item_Contents(), (Item) heldObj, itemSubject.Codex, text1);
            break;
          case Furniture _:
            if (heldObj == null)
              break;
            string text2 = I18n.Item_Contents_Placed((object) ((Item) heldObj).DisplayName);
            yield return (ICustomField) new ItemIconField(itemSubject.GameHelper, I18n.Item_Contents(), (Item) heldObj, itemSubject.Codex, text2);
            break;
          default:
            if (((Item) machine).QualifiedItemId == $"{"(BC)"}{Constant.ObjectIndexes.AutoGrabber}")
            {
              string str = I18n.Stringify((object) (bool) (!(heldObj is Chest chest) ? 0 : (((IEnumerable<Item>) chest.GetItemsForPlayer(Game1.player.UniqueMultiplayerID)).Any<Item>() ? 1 : 0)));
              yield return (ICustomField) new GenericField(I18n.Item_Contents(), str);
              break;
            }
            if (heldObj == null)
              break;
            string text3 = minutesLeft <= 0 ? I18n.Item_Contents_Ready((object) ((Item) heldObj).DisplayName) : I18n.Item_Contents_Partial((object) ((Item) heldObj).DisplayName, (object) itemSubject.Stringify((object) TimeSpan.FromMinutes((double) minutesLeft)));
            yield return (ICustomField) new ItemIconField(itemSubject.GameHelper, I18n.Item_Contents(), (Item) heldObj, itemSubject.Codex, text3);
            break;
        }
      }
    }
  }

  private IEnumerable<ICustomField> GetNeededForFields(Object? obj)
  {
    ItemSubject itemSubject = this;
    if (obj != null && !(((Item) obj).TypeDefinitionId != "(O)"))
    {
      List<string> stringList = new List<string>();
      string[] array1 = itemSubject.GetUnfinishedBundles(obj).OrderBy<BundleModel, string>((Func<BundleModel, string>) (bundle => bundle.Area)).ThenBy<BundleModel, string>((Func<BundleModel, string>) (bundle => bundle.DisplayName)).Select(bundle => new
      {
        bundle = bundle,
        countNeeded = this.GetIngredientCountNeeded(bundle, obj)
      }).Select(_param1 =>
      {
        if (_param1.countNeeded <= 1)
          return $"{this.GetTranslatedBundleArea(_param1.bundle)}: {_param1.bundle.DisplayName}";
        return $"{this.GetTranslatedBundleArea(_param1.bundle)}: {_param1.bundle.DisplayName} x {_param1.countNeeded}";
      }).ToArray<string>();
      if (((IEnumerable<string>) array1).Any<string>())
        stringList.Add(I18n.Item_NeededFor_CommunityCenter((object) I18n.List((IEnumerable<object>) array1)));
      CropData cropData = itemSubject.FromCrop != null ? itemSubject.FromCrop.GetData() : GameHelper.GetCropDataByHarvestItem(((Item) obj).ItemId);
      if (cropData != null)
      {
        if (cropData.CountForPolyculture && !((NetHashSet<int>) Game1.player.achievements).Contains(31 /*0x1F*/))
        {
          int count = itemSubject.Constants.PolycultureCount - itemSubject.GameHelper.GetShipped(((Item) obj).ItemId);
          if (count > 0)
            stringList.Add(I18n.Item_NeededFor_Polyculture((object) count));
        }
        if (cropData.CountForMonoculture && !((NetHashSet<int>) Game1.player.achievements).Contains(32 /*0x20*/))
        {
          int count = itemSubject.Constants.MonocultureCount - itemSubject.GameHelper.GetShipped(((Item) obj).ItemId);
          if (count > 0)
            stringList.Add(I18n.Item_NeededFor_Monoculture((object) count));
        }
      }
      if (itemSubject.GameHelper.GetFullShipmentAchievementItems().Any<KeyValuePair<string, bool>>((Func<KeyValuePair<string, bool>, bool>) (p => p.Key == ((Item) obj).QualifiedItemId && !p.Value)))
        stringList.Add(I18n.Item_NeededFor_FullShipment());
      if (obj.needsToBeDonated())
        stringList.Add(I18n.Item_NeededFor_FullCollection());
      ItemSubject.RecipeData[] array2 = itemSubject.GameHelper.GetRecipesForIngredient(itemSubject.Target).Select(recipe => new
      {
        recipe = recipe,
        item = recipe.TryCreateItem(this.Target)
      }).Select(_param1 => new
      {
        \u003C\u003Eh__TransparentIdentifier0 = _param1,
        timesCrafted = _param1.recipe.GetTimesCrafted(Game1.player)
      }).Where(_param1 => _param1.\u003C\u003Eh__TransparentIdentifier0.item != null && _param1.timesCrafted <= 0).OrderBy(_param1 => _param1.\u003C\u003Eh__TransparentIdentifier0.item.DisplayName).Select(_param1 => new ItemSubject.RecipeData(_param1.\u003C\u003Eh__TransparentIdentifier0.recipe.Type, _param1.\u003C\u003Eh__TransparentIdentifier0.item.DisplayName, _param1.timesCrafted, _param1.\u003C\u003Eh__TransparentIdentifier0.recipe.IsKnown())).ToArray<ItemSubject.RecipeData>();
      ItemSubject.RecipeData[] array3 = ((IEnumerable<ItemSubject.RecipeData>) array2).Where<ItemSubject.RecipeData>((Func<ItemSubject.RecipeData, bool>) (recipe => recipe.Type == RecipeType.Cooking)).ToArray<ItemSubject.RecipeData>();
      if (array3.Length != 0)
      {
        string neededForRecipeText = itemSubject.GetNeededForRecipeText(array3);
        stringList.Add(I18n.Item_NeededFor_GourmetChef((object) neededForRecipeText));
      }
      ItemSubject.RecipeData[] array4 = ((IEnumerable<ItemSubject.RecipeData>) array2).Where<ItemSubject.RecipeData>((Func<ItemSubject.RecipeData, bool>) (recipe => recipe.Type == RecipeType.Crafting)).ToArray<ItemSubject.RecipeData>();
      if (array4.Length != 0)
      {
        string neededForRecipeText = itemSubject.GetNeededForRecipeText(array4);
        stringList.Add(I18n.Item_NeededFor_CraftMaster((object) neededForRecipeText));
      }
      string[] array5 = itemSubject.GameHelper.GetQuestsWhichNeedItem(obj).Select<QuestModel, string>((Func<QuestModel, string>) (p => p.DisplayText)).OrderBy<string, string>((Func<string, string>) (p => p)).ToArray<string>();
      if (((IEnumerable<string>) array5).Any<string>())
        stringList.Add(I18n.Item_NeededFor_Quests((object) I18n.List((IEnumerable<object>) array5)));
      if (stringList.Any<string>())
        yield return (ICustomField) new GenericField(I18n.Item_NeededFor(), I18n.List((IEnumerable<object>) stringList));
    }
  }

  private string GetNeededForRecipeText(ItemSubject.RecipeData[] missingRecipes)
  {
    if (this.ShowUnknownRecipes)
      return I18n.List((IEnumerable<object>) ((IEnumerable<ItemSubject.RecipeData>) missingRecipes).Select<ItemSubject.RecipeData, string>((Func<ItemSubject.RecipeData, string>) (recipe => recipe.DisplayName)).ToArray<string>());
    ItemSubject.RecipeData[] array = ((IEnumerable<ItemSubject.RecipeData>) missingRecipes).Where<ItemSubject.RecipeData>((Func<ItemSubject.RecipeData, bool>) (recipe => recipe.IsKnown)).ToArray<ItemSubject.RecipeData>();
    int count = missingRecipes.Length - array.Length;
    return ((IEnumerable<ItemSubject.RecipeData>) array).Any<ItemSubject.RecipeData>() ? I18n.List((IEnumerable<object>) ((IEnumerable<string>) ((IEnumerable<ItemSubject.RecipeData>) array).Select<ItemSubject.RecipeData, string>((Func<ItemSubject.RecipeData, string>) (recipe => recipe.DisplayName)).ToArray<string>()).Append<string>(I18n.Item_UnknownRecipes((object) count))) : I18n.Item_UnknownRecipes((object) count);
  }

  private IEnumerable<BundleModel> GetUnfinishedBundles(Object item)
  {
    ItemSubject itemSubject = this;
    if (!Game1.player.hasOrWillReceiveMail("JojaMember"))
    {
      bool flag = ((NetFieldBase<bool, NetBool>) item.bigCraftable).Value;
      if (!flag)
        flag = item is Cask || item is Fence || item is Furniture || item is IndoorPot || item is Sign || item is Torch || item is Wallpaper;
      if (!flag)
      {
        CommunityCenter communityCenter = Game1.locations.OfType<CommunityCenter>().First<CommunityCenter>();
        if (!communityCenter.areAllAreasComplete() || IsBundleOpen(36))
        {
          foreach (BundleModel bundleModel in itemSubject.GameHelper.GetBundleData())
          {
            BundleModel bundle = bundleModel;
            if (IsBundleOpen(bundle.ID) && itemSubject.GetIngredientsFromBundle(bundle, item).Any<BundleIngredientModel>((Func<BundleIngredientModel, bool>) (p => this.IsIngredientNeeded(bundle, p))))
              yield return bundle;
          }
        }

        bool IsBundleOpen(int id)
        {
          try
          {
            return !communityCenter.isBundleComplete(id);
          }
          catch
          {
            return false;
          }
        }
      }
    }
  }

  private string GetNumberOwnedText(Item item)
  {
    int num = this.GameHelper.CountOwnedItems(item, false);
    int count1 = item is Object ? this.GameHelper.CountOwnedItems(item, true) : num;
    if (num != count1)
    {
      ParsedItemData data = ItemRegistry.GetData(item.QualifiedItemId);
      if (data != null)
      {
        object name1 = (object) item.Name;
        // ISSUE: variable of a boxed type
        __Boxed<int> count2 = (ValueType) count1;
        object name2 = name1;
        object displayName = (object) data.DisplayName;
        // ISSUE: variable of a boxed type
        __Boxed<int> baseCount = (ValueType) num;
        object baseName = displayName;
        return I18n.Item_NumberOwnedFlavored_Summary((object) count2, name2, (object) baseCount, baseName);
      }
    }
    return I18n.Item_NumberOwned_Summary((object) count1);
  }

  private string GetTranslatedBundleArea(BundleModel bundle)
  {
    string area = bundle.Area;
    string translatedBundleArea;
    if (area != null)
    {
      switch (area.Length)
      {
        case 5:
          if (area == "Vault")
          {
            translatedBundleArea = I18n.BundleArea_Vault();
            goto label_18;
          }
          break;
        case 6:
          if (area == "Pantry")
          {
            translatedBundleArea = I18n.BundleArea_Pantry();
            goto label_18;
          }
          break;
        case 9:
          if (area == "Fish Tank")
          {
            translatedBundleArea = I18n.BundleArea_FishTank();
            goto label_18;
          }
          break;
        case 11:
          switch (area[0])
          {
            case 'B':
              if (area == "Boiler Room")
              {
                translatedBundleArea = I18n.BundleArea_BoilerRoom();
                goto label_18;
              }
              break;
            case 'C':
              if (area == "Crafts Room")
              {
                translatedBundleArea = I18n.BundleArea_CraftsRoom();
                goto label_18;
              }
              break;
          }
          break;
        case 14:
          if (area == "Bulletin Board")
          {
            translatedBundleArea = I18n.BundleArea_BulletinBoard();
            goto label_18;
          }
          break;
        case 19:
          if (area == "Abandoned Joja Mart")
          {
            translatedBundleArea = I18n.BundleArea_AbandonedJojaMart();
            goto label_18;
          }
          break;
      }
    }
    translatedBundleArea = bundle.Area;
label_18:
    return translatedBundleArea;
  }

  private IDictionary<ItemQuality, int> GetSaleValue(Item item, bool qualityIsKnown)
  {
    if (((!(item.getOne() is Object one1) ? 1 : (!this.GameHelper.CanHaveQuality(item) ? 1 : 0)) | (qualityIsKnown ? 1 : 0)) != 0)
    {
      ItemQuality itemQuality = !qualityIsKnown || one1 == null ? ItemQuality.Normal : (ItemQuality) ((Item) one1).Quality;
      return (IDictionary<ItemQuality, int>) new Dictionary<ItemQuality, int>()
      {
        [itemQuality] = this.GetRawSalePrice(item)
      };
    }
    string[] withIridiumQuality = this.Constants.ItemsWithIridiumQuality;
    Dictionary<ItemQuality, int> saleValue = new Dictionary<ItemQuality, int>();
    Object one2 = (Object) item.getOne();
    foreach (ItemQuality enumValue in CommonHelper.GetEnumValues<ItemQuality>())
    {
      if (enumValue != ItemQuality.Iridium || ((IEnumerable<string>) withIridiumQuality).Contains<string>(item.QualifiedItemId) || ((IEnumerable<string>) withIridiumQuality).Contains<string>(item.Category.ToString()))
      {
        ((Item) one2).Quality = (int) enumValue;
        saleValue[enumValue] = this.GetRawSalePrice((Item) one2);
      }
    }
    return (IDictionary<ItemQuality, int>) saleValue;
  }

  private int GetRawSalePrice(Item item)
  {
    int num = item is Object @object ? ((Item) @object).sellToStorePrice(-1L) : item.salePrice(false) / 2;
    return num <= 0 ? 0 : num;
  }

  private IDictionary<GiftTaste, GiftTasteModel[]> GetGiftTastes(Item item)
  {
    return (IDictionary<GiftTaste, GiftTasteModel[]>) this.GameHelper.GetGiftTastes(item).GroupBy<GiftTasteModel, GiftTaste>((Func<GiftTasteModel, GiftTaste>) (p => p.Taste)).ToDictionary<IGrouping<GiftTaste, GiftTasteModel>, GiftTaste, GiftTasteModel[]>((Func<IGrouping<GiftTaste, GiftTasteModel>, GiftTaste>) (p => p.Key), (Func<IGrouping<GiftTaste, GiftTasteModel>, GiftTasteModel[]>) (p => p.Distinct<GiftTasteModel>().ToArray<GiftTasteModel>()));
  }

  private IEnumerable<BundleIngredientModel> GetIngredientsFromBundle(
    BundleModel bundle,
    Object item)
  {
    return ((IEnumerable<BundleIngredientModel>) bundle.Ingredients).Where<BundleIngredientModel>((Func<BundleIngredientModel, bool>) (required =>
    {
      string itemId = required.ItemId;
      return itemId != null && !(itemId == "-1") && (ItemRegistry.HasItemId((Item) item, required.ItemId) || !(required.ItemId != ((Item) item).Category.ToString())) && (ItemQuality) ((Item) item).Quality >= required.Quality;
    }));
  }

  private bool IsIngredientNeeded(BundleModel bundle, BundleIngredientModel ingredient)
  {
    bool[] flagArray;
    return !((NetDictionary<int, bool[], NetArray<bool, NetBool>, SerializableDictionary<int, bool[]>, NetBundles>) Game1.locations.OfType<CommunityCenter>().First<CommunityCenter>().bundles).TryGetValue(bundle.ID, ref flagArray) || ingredient.Index >= flagArray.Length || !flagArray[ingredient.Index];
  }

  private int GetIngredientCountNeeded(BundleModel bundle, Object item)
  {
    return this.GetIngredientsFromBundle(bundle, item).Where<BundleIngredientModel>((Func<BundleIngredientModel, bool>) (p => this.IsIngredientNeeded(bundle, p))).Sum<BundleIngredientModel>((Func<BundleIngredientModel, int>) (p => p.Stack));
  }

  public record RecipeData(RecipeType Type, string DisplayName, int TimesCrafted, bool IsKnown);
}
