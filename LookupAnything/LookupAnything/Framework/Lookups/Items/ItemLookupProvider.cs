// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.Lookups.Items.ItemLookupProvider
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Microsoft.Xna.Framework;
using Netcode;
using Pathoschild.Stardew.Common;
using Pathoschild.Stardew.Common.Items;
using Pathoschild.Stardew.LookupAnything.Framework.Data;
using StardewModdingAPI;
using StardewValley;
using StardewValley.GameData.FloorsAndPaths;
using StardewValley.Locations;
using StardewValley.Menus;
using StardewValley.Network;
using StardewValley.Objects;
using StardewValley.TerrainFeatures;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework.Lookups.Items;

internal class ItemLookupProvider : BaseLookupProvider
{
  private readonly ItemRepository ItemRepository = new ItemRepository();
  private readonly Func<ModConfig> Config;
  private readonly ISubjectRegistry Codex;

  public ItemLookupProvider(
    IReflectionHelper reflection,
    GameHelper gameHelper,
    Func<ModConfig> config,
    ISubjectRegistry codex)
    : base(reflection, gameHelper)
  {
    this.Config = config;
    this.Codex = codex;
  }

  public override IEnumerable<ITarget> GetTargets(GameLocation location, Vector2 lookupTile)
  {
    ItemLookupProvider itemLookupProvider = this;
    foreach ((Vector2 key, Object @object) in location.objects.Pairs)
    {
      Vector2 vector2 = key;
      Object obj = @object;
      if ((!(location is IslandShrine) || !(obj is ItemPedestal)) && itemLookupProvider.GameHelper.CouldSpriteOccludeTile(vector2, lookupTile))
        yield return (ITarget) new ObjectTarget(itemLookupProvider.GameHelper, obj, vector2, (Func<ISubject>) (() => this.BuildSubject((Item) obj, ObjectContext.World, location, false)));
    }
    foreach (Furniture furniture1 in location.furniture)
    {
      Furniture furniture = furniture1;
      Vector2 tileLocation = ((Object) furniture).TileLocation;
      if (itemLookupProvider.GameHelper.CouldSpriteOccludeTile(tileLocation, lookupTile))
        yield return (ITarget) new ObjectTarget(itemLookupProvider.GameHelper, (Object) furniture, tileLocation, (Func<ISubject>) (() => this.BuildSubject((Item) furniture, ObjectContext.Inventory, location)));
    }
    TerrainFeature terrainFeature3;
    foreach ((key, terrainFeature3) in ((NetDictionary<Vector2, TerrainFeature, NetRef<TerrainFeature>, SerializableDictionary<Vector2, TerrainFeature>, NetVector2Dictionary<TerrainFeature, NetRef<TerrainFeature>>>) location.terrainFeatures).Pairs)
    {
      Vector2 vector2 = key;
      HoeDirt dirt = terrainFeature3 as HoeDirt;
      if (dirt != null && dirt.crop != null && itemLookupProvider.GameHelper.CouldSpriteOccludeTile(vector2, lookupTile))
        yield return (ITarget) new CropTarget(itemLookupProvider.GameHelper, dirt, vector2, (Func<ISubject>) (() => this.BuildSubject(dirt.crop, ObjectContext.World, dirt)));
    }
    foreach ((key, terrainFeature3) in ((NetDictionary<Vector2, TerrainFeature, NetRef<TerrainFeature>, SerializableDictionary<Vector2, TerrainFeature>, NetVector2Dictionary<TerrainFeature, NetRef<TerrainFeature>>>) location.terrainFeatures).Pairs)
    {
      Vector2 vector2 = key;
      TerrainFeature terrainFeature4 = terrainFeature3;
      if (itemLookupProvider.GameHelper.CouldSpriteOccludeTile(vector2, lookupTile) && terrainFeature4 is Flooring flooring)
      {
        FloorPathData data = flooring.GetData();
        if (data != null)
        {
          Item item = ItemRegistry.Create(data.ItemId, 1, 0, false);
          yield return (ITarget) new FlooringTarget(itemLookupProvider.GameHelper, flooring, vector2, (Func<ISubject>) (() => this.BuildSubject(item, ObjectContext.World, location)));
        }
      }
    }
  }

  public override ISubject? GetSubject(IClickableMenu menu, int cursorX, int cursorY)
  {
    IClickableMenu iclickableMenu1 = this.GameHelper.GetGameMenuPage(menu) ?? menu;
    IClickableMenu iclickableMenu2 = iclickableMenu1;
    switch (iclickableMenu2)
    {
      case MenuWithInventory menuWithInventory:
        bool flag1;
        switch (menu)
        {
          case FieldOfficeMenu _:
          case TailoringMenu _:
            flag1 = true;
            break;
          default:
            flag1 = false;
            break;
        }
        if (flag1)
        {
          switch (iclickableMenu2)
          {
            case TailoringMenu tailoringMenu:
              ClickableTextureComponent[] textureComponentArray = new ClickableTextureComponent[3]
              {
                tailoringMenu.leftIngredientSpot,
                tailoringMenu.rightIngredientSpot,
                tailoringMenu.craftResultDisplay
              };
              foreach (ClickableTextureComponent textureComponent in textureComponentArray)
              {
                if (((ClickableComponent) textureComponent).containsPoint(cursorX, cursorY) && ((ClickableComponent) textureComponent).item != null)
                  return this.BuildSubject(((ClickableComponent) textureComponent).item, ObjectContext.Inventory, (GameLocation) null);
              }
              return this.GetSubject((IClickableMenu) ((MenuWithInventory) tailoringMenu).inventory, cursorX, cursorY);
            case FieldOfficeMenu fieldOfficeMenu:
              ClickableComponent clickableComponent1 = fieldOfficeMenu.pieceHolders.FirstOrDefault<ClickableComponent>((Func<ClickableComponent, bool>) (p => p.containsPoint(cursorX, cursorY)));
              if (clickableComponent1 != null)
              {
                if (clickableComponent1.item != null)
                  return this.BuildSubject(clickableComponent1.item, ObjectContext.Inventory, (GameLocation) null, false);
                if (CommonHelper.IsItemId(clickableComponent1.label))
                  return this.BuildSubject(ItemRegistry.Create(clickableComponent1.label, 1, 0, false), ObjectContext.Inventory, (GameLocation) null, false);
                break;
              }
              break;
            default:
              goto label_77;
          }
        }
        else
        {
          Item target = Game1.player.CursorSlotItem ?? menuWithInventory.heldItem ?? menuWithInventory.hoveredItem;
          if (target != null)
            return this.BuildSubject(target, ObjectContext.Inventory, (GameLocation) null);
          break;
        }
        break;
      case InventoryPage inventoryPage:
        Item target1 = Game1.player.CursorSlotItem ?? inventoryPage.hoveredItem;
        if (target1 != null)
          return this.BuildSubject(target1, ObjectContext.Inventory, (GameLocation) null);
        break;
      case ShopMenu shopMenu:
        switch (shopMenu.hoveredItem)
        {
          case Item target2:
            return this.BuildSubject(target2, ObjectContext.Inventory, (GameLocation) null);
          case MovieConcession movieConcession:
            return (ISubject) new MovieSnackSubject(this.GameHelper, movieConcession);
          default:
            return this.GetSubject((IClickableMenu) shopMenu.inventory, cursorX, cursorY);
        }
      case Toolbar toolbar:
        ClickableComponent clickableComponent2 = toolbar.buttons.FirstOrDefault<ClickableComponent>((Func<ClickableComponent, bool>) (slot => slot.containsPoint(cursorX, cursorY)));
        if (clickableComponent2 == null)
          return (ISubject) null;
        int num = toolbar.buttons.IndexOf(clickableComponent2);
        if (num < 0 || num > Game1.player.Items.Count - 1)
          return (ISubject) null;
        Item target3 = Game1.player.Items[num];
        if (target3 != null)
          return this.BuildSubject(target3, ObjectContext.Inventory, (GameLocation) null);
        break;
      case InventoryMenu inventoryMenu:
        using (List<ClickableComponent>.Enumerator enumerator = inventoryMenu.inventory.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            ClickableComponent current = enumerator.Current;
            if (current.containsPoint(cursorX, cursorY))
            {
              int result;
              if (int.TryParse(current.name, out result))
              {
                Item target4;
                if (inventoryMenu.actualInventory.TryGetIndex<Item>(result, out target4))
                {
                  if (target4 != null)
                    return this.BuildSubject(target4, ObjectContext.Inventory, (GameLocation) null);
                  break;
                }
                break;
              }
              break;
            }
          }
          break;
        }
      case CollectionsPage collectionsPage:
        int currentTab = collectionsPage.currentTab;
        bool flag2;
        switch (currentTab)
        {
          case 5:
          case 6:
          case 7:
            flag2 = true;
            break;
          default:
            flag2 = false;
            break;
        }
        if (!flag2)
        {
          int currentPage = collectionsPage.currentPage;
          using (List<ClickableTextureComponent>.Enumerator enumerator = collectionsPage.collections[currentTab][currentPage].GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              ClickableTextureComponent current = enumerator.Current;
              if (((ClickableComponent) current).containsPoint(cursorX, cursorY))
                return this.BuildSubject(ItemRegistry.Create(((ClickableComponent) current).name.Split(' ')[0], 1, 0, false), ObjectContext.Inventory, (GameLocation) null, false);
            }
            break;
          }
        }
        break;
      case CraftingPage craftingPage:
        Item hoverItem = craftingPage.hoverItem;
        if (hoverItem != null)
          return this.BuildSubject(hoverItem, ObjectContext.Inventory, (GameLocation) null);
        CraftingRecipe hoverRecipe = craftingPage.hoverRecipe;
        if (hoverRecipe != null)
          return this.BuildSubject(hoverRecipe.createItem(), ObjectContext.Inventory, (GameLocation) null);
        Dictionary<ClickableTextureComponent, CraftingRecipe> dictionary;
        if (craftingPage.pagesOfCraftingRecipes.TryGetIndex<Dictionary<ClickableTextureComponent, CraftingRecipe>>(craftingPage.currentCraftingPage, out dictionary))
        {
          using (Dictionary<ClickableTextureComponent, CraftingRecipe>.Enumerator enumerator = dictionary.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              (ClickableTextureComponent key, CraftingRecipe craftingRecipe) = enumerator.Current;
              if (((ClickableComponent) key).containsPoint(cursorX, cursorY))
              {
                if (craftingRecipe?.createItem() != null)
                  return this.BuildSubject(craftingRecipe.createItem(), ObjectContext.Inventory, (GameLocation) null);
                break;
              }
            }
            break;
          }
        }
        break;
      case ProfileMenu profileMenu:
        Item hoveredItem1 = profileMenu.hoveredItem;
        if (hoveredItem1 != null)
          return this.BuildSubject(hoveredItem1, ObjectContext.Inventory, (GameLocation) null);
        break;
      case JunimoNoteMenu junimoNoteMenu:
        Item hoveredItem2 = junimoNoteMenu.hoveredItem;
        if (hoveredItem2 != null)
          return this.BuildSubject(hoveredItem2, ObjectContext.Inventory, (GameLocation) null);
        for (int index = 0; index < junimoNoteMenu.ingredientList.Count; ++index)
        {
          if (((ClickableComponent) junimoNoteMenu.ingredientList[index]).containsPoint(cursorX, cursorY))
          {
            BundleIngredientDescription ingredient = junimoNoteMenu.currentPageBundle.ingredients[index];
            Item target5 = ItemRegistry.Create(ingredient.id, ingredient.stack, 0, false);
            target5.Quality = ingredient.quality;
            return this.BuildSubject(target5, ObjectContext.Inventory, (GameLocation) null);
          }
        }
        using (List<ClickableTextureComponent>.Enumerator enumerator = junimoNoteMenu.ingredientSlots.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            ClickableTextureComponent current = enumerator.Current;
            if (((ClickableComponent) current).item != null && ((ClickableComponent) current).containsPoint(cursorX, cursorY))
              return this.BuildSubject(((ClickableComponent) current).item, ObjectContext.Inventory, (GameLocation) null);
          }
          break;
        }
      default:
label_77:
        Item target6 = this.Reflection.GetField<Item>((object) iclickableMenu1, "hoveredItem", false)?.GetValue() ?? this.Reflection.GetField<Item>((object) iclickableMenu1, "HoveredItem", false)?.GetValue();
        if (target6 != null)
          return this.BuildSubject(target6, ObjectContext.Inventory, (GameLocation) null);
        break;
    }
    return (ISubject) null;
  }

  public override IEnumerable<ISubject> GetSearchSubjects()
  {
    foreach (SearchableItem searchableItem in this.ItemRepository.GetAll())
      yield return this.BuildSubject(searchableItem.Item, ObjectContext.World, (GameLocation) null, false);
  }

  public override ISubject? GetSubjectFor(object entity, GameLocation? location)
  {
    return !(entity is Item target) ? (ISubject) null : this.BuildSubject(target, ObjectContext.Any, location, false);
  }

  private ISubject BuildSubject(
    Item target,
    ObjectContext context,
    GameLocation? location,
    bool knownQuality = true)
  {
    ModConfig modConfig = this.Config();
    ISubjectRegistry codex = this.Codex;
    GameHelper gameHelper = this.GameHelper;
    int num1 = modConfig.ShowUncaughtFishSpawnRules ? 1 : 0;
    int num2 = modConfig.ShowUnknownGiftTastes ? 1 : 0;
    bool showUnknownRecipes = modConfig.ShowUnknownRecipes;
    bool showInvalidRecipes = modConfig.ShowInvalidRecipes;
    int num3 = modConfig.HighlightUnrevealedGiftTastes ? 1 : 0;
    ModGiftTasteConfig showGiftTastes = modConfig.ShowGiftTastes;
    int num4 = showUnknownRecipes ? 1 : 0;
    int num5 = showInvalidRecipes ? 1 : 0;
    ModCollapseLargeFieldsConfig collapseLargeFields = modConfig.CollapseLargeFields;
    Item obj = target;
    int context1 = (int) context;
    int num6 = knownQuality ? 1 : 0;
    GameLocation location1 = location;
    Func<Crop, ObjectContext, HoeDirt, ISubject> getCropSubject = new Func<Crop, ObjectContext, HoeDirt, ISubject>(this.BuildSubject);
    return (ISubject) new ItemSubject(codex, gameHelper, num1 != 0, num2 != 0, num3 != 0, showGiftTastes, num4 != 0, num5 != 0, collapseLargeFields, obj, (ObjectContext) context1, num6 != 0, location1, getCropSubject);
  }

  private ISubject BuildSubject(Crop target, ObjectContext context, HoeDirt? dirt)
  {
    string itemId = ((NetFieldBase<string, NetString>) target.indexOfHarvest).Value;
    if (!CommonHelper.IsItemId(itemId, false) && ((NetFieldBase<bool, NetBool>) target.forageCrop).Value)
    {
      if (((NetFieldBase<string, NetString>) target.whichForageCrop).Value == 2.ToString())
        itemId = "829";
      else if (((NetFieldBase<string, NetString>) target.whichForageCrop).Value == 1.ToString())
        itemId = "399";
    }
    ModConfig modConfig = this.Config();
    ISubjectRegistry codex = this.Codex;
    GameHelper gameHelper = this.GameHelper;
    int num1 = modConfig.ShowUncaughtFishSpawnRules ? 1 : 0;
    int num2 = modConfig.ShowUnknownGiftTastes ? 1 : 0;
    bool showUnknownRecipes = modConfig.ShowUnknownRecipes;
    bool showInvalidRecipes = modConfig.ShowInvalidRecipes;
    int num3 = modConfig.HighlightUnrevealedGiftTastes ? 1 : 0;
    ModGiftTasteConfig showGiftTastes = modConfig.ShowGiftTastes;
    int num4 = showUnknownRecipes ? 1 : 0;
    int num5 = showInvalidRecipes ? 1 : 0;
    ModCollapseLargeFieldsConfig collapseLargeFields = modConfig.CollapseLargeFields;
    Item obj = ItemRegistry.Create(itemId, 1, 0, false);
    int context1 = (int) context;
    GameLocation location = ((TerrainFeature) dirt)?.Location;
    Func<Crop, ObjectContext, HoeDirt, ISubject> getCropSubject = new Func<Crop, ObjectContext, HoeDirt, ISubject>(this.BuildSubject);
    HoeDirt fromDirt = dirt;
    return (ISubject) new ItemSubject(codex, gameHelper, num1 != 0, num2 != 0, num3 != 0, showGiftTastes, num4 != 0, num5 != 0, collapseLargeFields, obj, (ObjectContext) context1, false, location, getCropSubject, fromDirt: fromDirt);
  }
}
