// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.Lookups.Buildings.BuildingSubject
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Netcode;
using Pathoschild.Stardew.Common;
using Pathoschild.Stardew.LookupAnything.Framework.Data;
using Pathoschild.Stardew.LookupAnything.Framework.DebugFields;
using Pathoschild.Stardew.LookupAnything.Framework.Fields;
using Pathoschild.Stardew.LookupAnything.Framework.Fields.Models;
using Pathoschild.Stardew.LookupAnything.Framework.Models;
using StardewModdingAPI;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Buildings;
using StardewValley.Characters;
using StardewValley.GameData.Buildings;
using StardewValley.GameData.FishPonds;
using StardewValley.Locations;
using StardewValley.Monsters;
using StardewValley.Objects;
using StardewValley.TokenizableStrings;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using xTile;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework.Lookups.Buildings;

internal class BuildingSubject : BaseSubject
{
  private readonly Building Target;
  private readonly Rectangle SourceRectangle;
  private readonly ISubjectRegistry Codex;
  private readonly ModCollapseLargeFieldsConfig CollapseFieldsConfig;
  private readonly bool ShowInvalidRecipes;

  public BuildingSubject(
    ISubjectRegistry codex,
    GameHelper gameHelper,
    Building building,
    Rectangle sourceRectangle,
    ModCollapseLargeFieldsConfig collapseFieldsConfig,
    bool showInvalidRecipes)
    : base(gameHelper, ((NetFieldBase<string, NetString>) building.buildingType).Value, (string) null, I18n.Type_Building())
  {
    this.Codex = codex;
    this.Target = building;
    this.SourceRectangle = sourceRectangle;
    this.CollapseFieldsConfig = collapseFieldsConfig;
    this.ShowInvalidRecipes = showInvalidRecipes;
    BuildingData data = building.GetData();
    this.Name = TokenParser.ParseText(data?.Name, (Random) null, (TokenParserDelegate) null, (Farmer) null) ?? this.Name;
    this.Description = TokenParser.ParseText(data?.Description, (Random) null, (TokenParserDelegate) null, (Farmer) null) ?? this.Description;
  }

  public override IEnumerable<ICustomField> GetData()
  {
    BuildingSubject buildingSubject = this;
    Building building = buildingSubject.Target;
    BuildingData data = building.GetData();
    bool built = !building.isUnderConstruction(true);
    int? upgradeLevel = buildingSubject.GetUpgradeLevel(building);
    IModInfo modFromStringId = buildingSubject.GameHelper.TryGetModFromStringId(((NetFieldBase<string, NetString>) building.buildingType).Value);
    if (modFromStringId != null)
      yield return (ICustomField) new GenericField(I18n.AddedByMod(), I18n.AddedByMod_Summary((object) modFromStringId.Manifest.Name));
    if (!built || ((NetFieldBase<int, NetInt>) building.daysUntilUpgrade).Value > 0)
    {
      int num = building.isUnderConstruction(true) ? ((NetFieldBase<int, NetInt>) building.daysOfConstructionLeft).Value : ((NetFieldBase<int, NetInt>) building.daysUntilUpgrade).Value;
      SDate sdate = SDate.Now().AddDays(num);
      yield return (ICustomField) new GenericField(I18n.Building_Construction(), I18n.Building_Construction_Summary((object) buildingSubject.Stringify((object) sdate)));
    }
    Farmer owner = buildingSubject.GetOwner();
    if (owner != null)
      yield return (ICustomField) new LinkField(I18n.Building_Owner(), ((Character) owner).Name, (Func<ISubject>) (() => this.Codex.GetByEntity((object) owner, ((Character) owner).currentLocation)));
    else if (building.GetIndoors() is Cabin)
      yield return (ICustomField) new GenericField(I18n.Building_Owner(), I18n.Building_Owner_None());
    if (built && building is Stable stable)
    {
      Horse horse = Utility.findHorse(stable.HorseId);
      if (horse != null)
      {
        yield return (ICustomField) new LinkField(I18n.Building_Horse(), ((Character) horse).Name, (Func<ISubject>) (() => this.Codex.GetByEntity((object) horse, ((Character) horse).currentLocation)));
        yield return (ICustomField) new GenericField(I18n.Building_HorseLocation(), I18n.Building_HorseLocation_Summary((object) ((Character) horse).currentLocation.Name, (object) ((Character) horse).TilePoint.X, (object) ((Character) horse).TilePoint.Y));
      }
    }
    if (built && building.GetIndoors() is AnimalHouse animalHouse)
    {
      yield return (ICustomField) new GenericField(I18n.Building_Animals(), I18n.Building_Animals_Summary((object) ((NetList<long, NetLong>) animalHouse.animalsThatLiveHere).Count, (object) ((NetFieldBase<int, NetInt>) animalHouse.animalLimit).Value));
      int? nullable = upgradeLevel;
      int num = 2;
      if (nullable.GetValueOrDefault() >= num & nullable.HasValue && (buildingSubject.IsBarn(building) || buildingSubject.IsCoop(building)))
      {
        yield return (ICustomField) new GenericField(I18n.Building_FeedTrough(), I18n.Building_FeedTrough_Automated());
      }
      else
      {
        int total;
        int filled;
        buildingSubject.GetFeedMetrics(animalHouse, out total, out filled);
        yield return (ICustomField) new GenericField(I18n.Building_FeedTrough(), I18n.Building_FeedTrough_Summary((object) filled, (object) total));
      }
    }
    if (built && building.GetIndoors() is SlimeHutch slimeHutch)
    {
      int count = ((IEnumerable) ((GameLocation) slimeHutch).characters).OfType<GreenSlime>().Count<GreenSlime>();
      yield return (ICustomField) new GenericField(I18n.Building_Slimes(), I18n.Building_Slimes_Summary((object) count, (object) 20));
      yield return (ICustomField) new GenericField(I18n.Building_WaterTrough(), I18n.Building_WaterTrough_Summary((object) ((IEnumerable<bool>) slimeHutch.waterSpots).Count<bool>((Func<bool, bool>) (p => p)), (object) slimeHutch.waterSpots.Count));
    }
    if (built)
    {
      Checkbox[] array = buildingSubject.GetUpgradeLevelSummary(building, upgradeLevel).ToArray<Checkbox>();
      if (((IEnumerable<Checkbox>) array).Any<Checkbox>())
        yield return (ICustomField) new CheckboxListField(I18n.Building_Upgrades(), new CheckboxList[1]
        {
          new CheckboxList(array)
        });
    }
    if (built)
    {
      FishPond pond = building as FishPond;
      if (pond == null)
      {
        if (building is JunimoHut hut)
        {
          yield return (ICustomField) new GenericField(I18n.Building_JunimoHarvestingEnabled(), I18n.Stringify((object) !((NetFieldBase<bool, NetBool>) hut.noHarvest).Value));
          yield return (ICustomField) new ItemIconListField(buildingSubject.GameHelper, I18n.Building_OutputReady(), (IEnumerable<Item>) hut.GetOutputChest()?.GetItemsForPlayer(Game1.player.UniqueMultiplayerID), true);
        }
        else
        {
          RecipeModel[] array = buildingSubject.GameHelper.GetRecipesForBuilding(building).ToArray<RecipeModel>();
          if (array.Length != 0)
          {
            ItemRecipesField itemRecipesField = new ItemRecipesField(buildingSubject.GameHelper, buildingSubject.Codex, I18n.Item_Recipes(), (Item) null, array, true, buildingSubject.ShowInvalidRecipes);
            if (buildingSubject.CollapseFieldsConfig.Enabled)
              itemRecipesField.CollapseIfLengthExceeds(buildingSubject.CollapseFieldsConfig.BuildingRecipes, array.Length);
            yield return (ICustomField) itemRecipesField;
            ISet<string> inputChests;
            ISet<string> outputChests;
            if (MachineDataHelper.TryGetBuildingChestNames(data, out inputChests, out outputChests))
            {
              IEnumerable<Item> items = MachineDataHelper.GetBuildingChests(building, inputChests).SelectMany<Chest, Item>((Func<Chest, IEnumerable<Item>>) (p => (IEnumerable<Item>) p.GetItemsForPlayer()));
              IEnumerable<Item> outputItems = MachineDataHelper.GetBuildingChests(building, outputChests).SelectMany<Chest, Item>((Func<Chest, IEnumerable<Item>>) (p => (IEnumerable<Item>) p.GetItemsForPlayer()));
              yield return (ICustomField) new ItemIconListField(buildingSubject.GameHelper, I18n.Building_OutputProcessing(), items, true);
              yield return (ICustomField) new ItemIconListField(buildingSubject.GameHelper, I18n.Building_OutputReady(), outputItems, true);
              outputItems = (IEnumerable<Item>) null;
            }
          }
        }
      }
      else if (!CommonHelper.IsItemId(((NetFieldBase<string, NetString>) pond.fishType).Value))
      {
        yield return (ICustomField) new GenericField(I18n.Building_FishPond_Population(), I18n.Building_FishPond_Population_Empty());
      }
      else
      {
        Object fish = pond.GetFishObject();
        ((Item) fish).Stack = pond.FishCount;
        FishPondData pondData = pond.GetFishPondData();
        string text = $"{((Item) fish).DisplayName} ({I18n.Generic_Ratio((object) pond.FishCount, (object) ((NetFieldBase<int, NetInt>) ((Building) pond).maxOccupants).Value)})";
        if (pond.FishCount < ((NetFieldBase<int, NetInt>) ((Building) pond).maxOccupants).Value)
        {
          SDate date = SDate.Now().AddDays(pondData.SpawnTime - ((NetFieldBase<int, NetInt>) pond.daysSinceSpawn).Value);
          text = text + Environment.NewLine + I18n.Building_FishPond_Population_NextSpawn((object) buildingSubject.GetRelativeDateStr(date));
        }
        yield return (ICustomField) new ItemIconField(buildingSubject.GameHelper, I18n.Building_FishPond_Population(), (Item) fish, buildingSubject.Codex, text);
        yield return (ICustomField) new ItemIconField(buildingSubject.GameHelper, I18n.Building_OutputReady(), ((NetFieldBase<Item, NetRef<Item>>) pond.output).Value, buildingSubject.Codex);
        int num = (int) Math.Round((double) Utility.Lerp(0.15f, 0.95f, (float) ((NetFieldBase<int, NetInt>) ((Building) pond).currentOccupants).Value / 10f) * 100.0);
        yield return (ICustomField) new FishPondDropsField(buildingSubject.GameHelper, buildingSubject.Codex, I18n.Building_FishPond_Drops(), ((NetFieldBase<int, NetInt>) ((Building) pond).currentOccupants).Value, pondData, fish, I18n.Building_FishPond_Drops_Preface((object) num.ToString()));
        Dictionary<int, List<string>> populationGates = pondData.PopulationGates;
        if ((populationGates != null ? (populationGates.Any<KeyValuePair<int, List<string>>>((Func<KeyValuePair<int, List<string>>, bool>) (gate => gate.Key > ((NetFieldBase<int, NetInt>) pond.lastUnlockedPopulationGate).Value)) ? 1 : 0) : 0) != 0)
          yield return (ICustomField) new CheckboxListField(I18n.Building_FishPond_Quests(), new CheckboxList[1]
          {
            new CheckboxList(buildingSubject.GetPopulationGates(pond, pondData))
          });
        fish = (Object) null;
        pondData = (FishPondData) null;
      }
      hut = (JunimoHut) null;
      if (((NetFieldBase<int, NetInt>) building.hayCapacity).Value > 0)
      {
        Farm farm = Game1.getFarm();
        int hayCount = ((NetFieldBase<int, NetInt>) ((GameLocation) farm).piecesOfHay).Value;
        int maxHayInLocation = Math.Max(((NetFieldBase<int, NetInt>) ((GameLocation) farm).piecesOfHay).Value, ((GameLocation) farm).GetHayCapacity());
        yield return (ICustomField) new GenericField(I18n.Building_StoredHay(), I18n.Building_StoredHay_Summary((object) hayCount, (object) maxHayInLocation, (object) ((NetFieldBase<int, NetInt>) building.hayCapacity).Value));
      }
    }
    RecipeModel[] array1 = buildingSubject.GameHelper.GetRecipes().Where<RecipeModel>((Func<RecipeModel, bool>) (recipe => recipe.Type == RecipeType.BuildingBlueprint && recipe.MachineId == ((NetFieldBase<string, NetString>) building.buildingType).Value)).ToArray<RecipeModel>();
    if (array1.Length != 0)
    {
      ItemRecipesField itemRecipesField = new ItemRecipesField(buildingSubject.GameHelper, buildingSubject.Codex, I18n.Building_ConstructionCosts(), (Item) null, array1, true, buildingSubject.ShowInvalidRecipes, false, false);
      if (buildingSubject.CollapseFieldsConfig.Enabled)
        itemRecipesField.CollapseIfLengthExceeds(buildingSubject.CollapseFieldsConfig.BuildingRecipes, array1.Length);
      yield return (ICustomField) itemRecipesField;
    }
  }

  public override IEnumerable<IDebugField> GetDebugFields()
  {
    BuildingSubject buildingSubject = this;
    Building target = buildingSubject.Target;
    yield return (IDebugField) new GenericDebugField("building type", ((NetFieldBase<string, NetString>) target.buildingType).Value, pinned: true);
    yield return (IDebugField) new GenericDebugField("days of construction left", ((NetFieldBase<int, NetInt>) target.daysOfConstructionLeft).Value, pinned: true);
    yield return (IDebugField) new GenericDebugField("indoors name", target.GetIndoorsName(), pinned: true);
    yield return (IDebugField) new GenericDebugField("indoors type", target.GetIndoorsType().ToString(), pinned: true);
    foreach (IDebugField debugField in buildingSubject.GetDebugFieldsFrom((object) target))
      yield return debugField;
  }

  public override bool DrawPortrait(SpriteBatch spriteBatch, Vector2 position, Vector2 size)
  {
    Building target = this.Target;
    spriteBatch.Draw(target.texture.Value, position, new Rectangle?(this.SourceRectangle), target.color, 0.0f, Vector2.Zero, size.X / (float) this.SourceRectangle.Width, (SpriteEffects) 0, 0.89f);
    return true;
  }

  private bool IsBarn(Building? building)
  {
    string str = ((NetFieldBase<string, NetString>) building?.buildingType).Value;
    return str == "Barn" || str == "Big Barn" || str == "Deluxe Barn";
  }

  private bool IsCoop(Building? building)
  {
    string str = ((NetFieldBase<string, NetString>) building?.buildingType).Value;
    return str == "Coop" || str == "Big Coop" || str == "Deluxe Coop";
  }

  private Farmer? GetOwner()
  {
    if (this.Target is Stable target)
      return Game1.GetPlayer(((NetFieldBase<long, NetLong>) ((Building) target).owner).Value, false);
    return this.Target.GetIndoors() is Cabin indoors ? ((FarmHouse) indoors).owner : (Farmer) null;
  }

  private int? GetUpgradeLevel(Building building)
  {
    int result1;
    if (this.IsBarn(building) && int.TryParse(((NetFieldBase<string, NetString>) building.GetIndoors()?.mapPath).Value?.Substring("Maps\\Barn".Length), out result1))
      return new int?(result1 - 1);
    if (building.GetIndoors() is Cabin indoors)
      return new int?(((FarmHouse) indoors).upgradeLevel);
    int result2;
    return this.IsCoop(building) && int.TryParse(((NetFieldBase<string, NetString>) building.GetIndoors()?.mapPath).Value?.Substring("Maps\\Coop".Length), out result2) ? new int?(result2 - 1) : new int?();
  }

  private void GetFeedMetrics(AnimalHouse building, out int total, out int filled)
  {
    Map map = ((GameLocation) building).Map;
    total = 0;
    filled = 0;
    for (int index1 = 0; index1 < map.Layers[0].LayerWidth; ++index1)
    {
      for (int index2 = 0; index2 < map.Layers[0].LayerHeight; ++index2)
      {
        if (((GameLocation) building).doesTileHaveProperty(index1, index2, "Trough", "Back", false) != null)
        {
          ++total;
          Object @object;
          if (((GameLocation) building).objects.TryGetValue(new Vector2((float) index1, (float) index2), ref @object) && ((Item) @object).QualifiedItemId == "(O)178")
            ++filled;
        }
      }
    }
  }

  private IEnumerable<Checkbox> GetUpgradeLevelSummary(Building building, int? upgradeLevel)
  {
    if (this.IsBarn(building))
    {
      yield return new Checkbox(true, I18n.Building_Upgrades_Barn_0());
      string text1 = I18n.Building_Upgrades_Barn_1();
      int? nullable1 = upgradeLevel;
      int num1 = 1;
      yield return new Checkbox(nullable1.GetValueOrDefault() >= num1 & nullable1.HasValue, text1);
      string text2 = I18n.Building_Upgrades_Barn_2();
      int? nullable2 = upgradeLevel;
      int num2 = 2;
      yield return new Checkbox(nullable2.GetValueOrDefault() >= num2 & nullable2.HasValue, text2);
    }
    else if (building.GetIndoors() is Cabin)
    {
      yield return new Checkbox(true, I18n.Building_Upgrades_Cabin_0());
      string text3 = I18n.Building_Upgrades_Cabin_1();
      int? nullable3 = upgradeLevel;
      int num3 = 1;
      yield return new Checkbox(nullable3.GetValueOrDefault() >= num3 & nullable3.HasValue, text3);
      string text4 = I18n.Building_Upgrades_Cabin_2();
      int? nullable4 = upgradeLevel;
      int num4 = 2;
      yield return new Checkbox(nullable4.GetValueOrDefault() >= num4 & nullable4.HasValue, text4);
    }
    else if (this.IsCoop(building))
    {
      yield return new Checkbox(true, I18n.Building_Upgrades_Coop_0());
      string text5 = I18n.Building_Upgrades_Coop_1();
      int? nullable5 = upgradeLevel;
      int num5 = 1;
      yield return new Checkbox(nullable5.GetValueOrDefault() >= num5 & nullable5.HasValue, text5);
      string text6 = I18n.Building_Upgrades_Coop_2();
      int? nullable6 = upgradeLevel;
      int num6 = 2;
      yield return new Checkbox(nullable6.GetValueOrDefault() >= num6 & nullable6.HasValue, text6);
    }
  }

  private IEnumerable<Checkbox> GetPopulationGates(FishPond pond, FishPondData data)
  {
    BuildingSubject buildingSubject = this;
    bool foundNextQuest = false;
    foreach (FishPondPopulationGateData pondPopulationGate in buildingSubject.GameHelper.GetFishPondPopulationGates(data))
    {
      int newPopulation = pondPopulationGate.NewPopulation;
      if (((NetFieldBase<int, NetInt>) pond.lastUnlockedPopulationGate).Value >= pondPopulationGate.RequiredPopulation)
      {
        yield return new Checkbox(true, I18n.Building_FishPond_Quests_Done((object) newPopulation));
      }
      else
      {
        string[] array = ((IEnumerable<FishPondPopulationGateQuestItemData>) pondPopulationGate.RequiredItems).Select<FishPondPopulationGateQuestItemData, string>((Func<FishPondPopulationGateQuestItemData, string>) (drop =>
        {
          string populationGates = ItemRegistry.GetDataOrErrorItem(drop.ItemID).DisplayName;
          if (drop.MinCount != drop.MaxCount)
            populationGates = $"{populationGates} ({I18n.Generic_Range((object) drop.MinCount, (object) drop.MaxCount)})";
          else if (drop.MinCount > 1)
            populationGates += $" ({drop.MinCount})";
          return populationGates;
        })).ToArray<string>();
        string text = array.Length > 1 ? I18n.Building_FishPond_Quests_IncompleteRandom((object) newPopulation, (object) I18n.List((IEnumerable<object>) array)) : I18n.Building_FishPond_Quests_IncompleteOne((object) newPopulation, (object) array[0]);
        if (!foundNextQuest)
        {
          foundNextQuest = true;
          int days = data.SpawnTime + data.SpawnTime * (((NetFieldBase<int, NetInt>) ((Building) pond).maxOccupants).Value - ((NetFieldBase<int, NetInt>) ((Building) pond).currentOccupants).Value) - ((NetFieldBase<int, NetInt>) pond.daysSinceSpawn).Value;
          text = $"{text}; {I18n.Building_FishPond_Quests_Available((object) buildingSubject.GetRelativeDateStr(days))}";
        }
        yield return new Checkbox(false, text);
      }
    }
  }
}
