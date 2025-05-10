// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.DataParser
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Microsoft.Xna.Framework;
using Netcode;
using Pathoschild.Stardew.Common;
using Pathoschild.Stardew.Common.Integrations.ExtraMachineConfig;
using Pathoschild.Stardew.LookupAnything.Framework;
using Pathoschild.Stardew.LookupAnything.Framework.Constants;
using Pathoschild.Stardew.LookupAnything.Framework.Data;
using Pathoschild.Stardew.LookupAnything.Framework.Models;
using Pathoschild.Stardew.LookupAnything.Framework.Models.FishData;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Buildings;
using StardewValley.Characters;
using StardewValley.Extensions;
using StardewValley.GameData;
using StardewValley.GameData.Buildings;
using StardewValley.GameData.FishPonds;
using StardewValley.GameData.Locations;
using StardewValley.GameData.Machines;
using StardewValley.Internal;
using StardewValley.ItemTypeDefinitions;
using StardewValley.Locations;
using StardewValley.Network;
using StardewValley.TokenizableStrings;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything;

internal class DataParser
{
  public const string ComplexRecipeId = "__COMPLEX_RECIPE__";

  public IEnumerable<BundleModel> GetBundles(IMonitor monitor)
  {
    foreach ((string key, string str) in ((NetFieldBase<NetWorldState, NetRef<NetWorldState>>) Game1.netWorldState).Value.BundleData)
    {
      if (str != null)
      {
        BundleModel bundle;
        try
        {
          string[] strArray1 = key.Split('/');
          string Area = ArgUtility.Get(strArray1, 0, (string) null, true);
          int ID = ArgUtility.GetInt(strArray1, 1, 0);
          string[] strArray2 = str.Split('/');
          string Name = ArgUtility.Get(strArray2, 0, (string) null, true);
          string RewardData = ArgUtility.Get(strArray2, 1, (string) null, true);
          string DisplayName = ArgUtility.Get(strArray2, 6, (string) null, true);
          List<BundleIngredientModel> bundleIngredientModelList = new List<BundleIngredientModel>();
          string[] strArray3 = ArgUtility.SplitBySpace(ArgUtility.Get(strArray2, 2, (string) null, true));
          for (int index = 0; index < strArray3.Length; index += 3)
          {
            int Index = index / 3;
            string ItemId = ArgUtility.Get(strArray3, index, (string) null, true);
            int Stack = ArgUtility.GetInt(strArray3, index + 1, 0);
            ItemQuality Quality = ArgUtility.GetEnum<ItemQuality>(strArray3, index + 2, ItemQuality.Normal);
            bundleIngredientModelList.Add(new BundleIngredientModel(Index, ItemId, Stack, Quality));
          }
          bundle = new BundleModel(ID, Name, DisplayName, Area, RewardData, bundleIngredientModelList.ToArray());
        }
        catch (Exception ex)
        {
          monitor.LogOnce($"Couldn't parse community center bundle '{key}' due to an invalid format.\nRecipe data: '{str}'\nError: {ex}", (LogLevel) 3);
          continue;
        }
        yield return bundle;
      }
    }
  }

  public IEnumerable<FishPondPopulationGateData> GetFishPondPopulationGates(FishPondData data)
  {
    if (data.PopulationGates != null)
    {
      foreach ((int num, List<string> source) in data.PopulationGates)
      {
        if (source != null)
        {
          FishPondPopulationGateQuestItemData[] array = source.Select<string, FishPondPopulationGateQuestItemData>((Func<string, FishPondPopulationGateQuestItemData>) (entry =>
          {
            string[] strArray = ArgUtility.SplitBySpace(entry);
            int length = strArray.Length;
            if (length < 1 || length > 3)
              return (FishPondPopulationGateQuestItemData) null;
            string ItemID = ArgUtility.Get(strArray, 0, (string) null, true);
            int val2_1 = ArgUtility.GetInt(strArray, 1, 1);
            int val2_2 = ArgUtility.GetInt(strArray, 2, 1);
            int MinCount = Math.Max(1, val2_1);
            int MaxCount = Math.Max(1, val2_2);
            if (MaxCount < MinCount)
              MaxCount = MinCount;
            return new FishPondPopulationGateQuestItemData(ItemID, MinCount, MaxCount);
          })).WhereNotNull<FishPondPopulationGateQuestItemData>().ToArray<FishPondPopulationGateQuestItemData>();
          yield return new FishPondPopulationGateData(num, array);
        }
      }
    }
  }

  public IEnumerable<FishPondDropData> GetFishPondDrops(FishPondData data)
  {
    if (data.ProducedItems != null)
    {
      foreach (FishPondReward producedItem in data.ProducedItems)
      {
        FishPondReward drop = producedItem;
        if (drop != null)
        {
          string[] itemIds = this.GetItemSpawnFieldIds(((GenericSpawnItemData) drop).RandomItemId, ((GenericSpawnItemData) drop).ItemId);
          string[] strArray = itemIds;
          for (int index = 0; index < strArray.Length; ++index)
            yield return new FishPondDropData(drop.RequiredPopulation, strArray[index], ((GenericSpawnItemData) drop).MinStack, ((GenericSpawnItemData) drop).MaxStack, drop.Chance * (1f / (float) itemIds.Length), ((GenericSpawnItemDataWithCondition) drop).Condition);
          strArray = (string[]) null;
          itemIds = (string[]) null;
          drop = (FishPondReward) null;
        }
      }
    }
  }

  public FishSpawnData GetFishSpawnRules(ParsedItemData fish, Metadata metadata)
  {
    List<FishSpawnLocationData> source1 = new List<FishSpawnLocationData>();
    bool IsLegendaryFamily = false;
    foreach ((string key, LocationData locationData1) in (IEnumerable<KeyValuePair<string, LocationData>>) Game1.locationData)
    {
      string locationId = key;
      LocationData locationData2 = locationData1;
      if (!metadata.IgnoreFishingLocations.Contains(locationId))
      {
        List<FishSpawnLocationData> source2 = new List<FishSpawnLocationData>();
        if (locationData2?.Fish != null)
        {
          foreach (SpawnFishData spawnFishData in locationData2.Fish)
          {
            if (spawnFishData != null)
            {
              ParsedItemData data = ItemRegistry.GetData(((GenericSpawnItemData) spawnFishData).ItemId);
              if (!(data?.ObjectType != "Fish") && !(data.QualifiedItemId != fish.QualifiedItemId))
              {
                Season? season1 = spawnFishData.Season;
                if (season1.HasValue)
                {
                  List<FishSpawnLocationData> spawnLocationDataList = source2;
                  string locationId1 = locationId;
                  string fishAreaId = spawnFishData.FishAreaId;
                  string[] seasons = new string[1];
                  season1 = spawnFishData.Season;
                  seasons[0] = season1.Value.ToString();
                  FishSpawnLocationData spawnLocationData = new FishSpawnLocationData(locationId1, fishAreaId, seasons);
                  spawnLocationDataList.Add(spawnLocationData);
                }
                else if (((GenericSpawnItemDataWithCondition) spawnFishData).Condition != null)
                {
                  foreach (GameStateQuery.ParsedGameStateQuery parsedGameStateQuery in GameStateQuery.Parse(((GenericSpawnItemDataWithCondition) spawnFishData).Condition))
                  {
                    if (parsedGameStateQuery.Query.Length != 0)
                    {
                      if (GameStateQuery.SeasonQueryKeys.Contains(parsedGameStateQuery.Query[0]))
                      {
                        List<string> stringList = new List<string>();
                        string[] strArray = new string[4]
                        {
                          "spring",
                          "summer",
                          "fall",
                          "winter"
                        };
                        foreach (string str in strArray)
                        {
                          string season = str;
                          if (!parsedGameStateQuery.Negated && ((IEnumerable<string>) parsedGameStateQuery.Query).Any<string>((Func<string, bool>) (word => word.Equals(season, StringComparison.OrdinalIgnoreCase))))
                            stringList.Add(season);
                        }
                        source2.Add(new FishSpawnLocationData(locationId, spawnFishData.FishAreaId, stringList.ToArray()));
                      }
                      else if (!IsLegendaryFamily && !parsedGameStateQuery.Negated)
                      {
                        string[] query = parsedGameStateQuery.Query;
                        if (query != null && query.Length == 3 && query[0] == "PLAYER_SPECIAL_ORDER_RULE_ACTIVE" && query[1] == "Current" && query[2] == "LEGENDARY_FAMILY")
                          IsLegendaryFamily = true;
                      }
                    }
                  }
                }
                else
                  source2.Add(new FishSpawnLocationData(locationId, spawnFishData.FishAreaId, new string[4]
                  {
                    "spring",
                    "summer",
                    "fall",
                    "winter"
                  }));
              }
            }
          }
        }
        if (source2.Count > 0)
          source1.AddRange(source2.GroupBy<FishSpawnLocationData, string>((Func<FishSpawnLocationData, string>) (p => p.Area)).Select(areaGroup => new
          {
            areaGroup = areaGroup,
            seasons = areaGroup.SelectMany<FishSpawnLocationData, string>((Func<FishSpawnLocationData, IEnumerable<string>>) (p => (IEnumerable<string>) p.Seasons)).Distinct<string>().ToArray<string>()
          }).Select(_param1 => new FishSpawnLocationData(locationId, _param1.areaGroup.Key, _param1.seasons)));
      }
    }
    List<FishSpawnTimeOfDayData> spawnTimeOfDayDataList = new List<FishSpawnTimeOfDayData>();
    FishSpawnWeather Weather = FishSpawnWeather.Both;
    int MinFishingLevel = 0;
    bool IsUnique = false;
    string str1;
    if (ItemExtensions.HasTypeObject((IHaveItemTypeId) fish) && source1.Any<FishSpawnLocationData>() && DataLoader.Fish(Game1.content).TryGetValue(fish.ItemId, out str1) && str1 != null)
    {
      string[] strArray1 = str1.Split('/');
      string[] strArray2 = ArgUtility.SplitBySpace(ArgUtility.Get(strArray1, 5, (string) null, true));
      int num = 0;
      for (int index = strArray2.Length + 1; num + 1 < index; num += 2)
      {
        int MinTime;
        int MaxTime;
        if (ArgUtility.TryGetInt(strArray2, num, ref MinTime, ref key, "int minTime") && ArgUtility.TryGetInt(strArray2, num + 1, ref MaxTime, ref key, "int maxTime"))
          spawnTimeOfDayDataList.Add(new FishSpawnTimeOfDayData(MinTime, MaxTime));
      }
      if (!ArgUtility.TryGetEnum<FishSpawnWeather>(strArray1, 7, ref Weather, ref key, "weather"))
        Weather = FishSpawnWeather.Both;
      if (!ArgUtility.TryGetInt(strArray1, 12, ref MinFishingLevel, ref key, "minFishingLevel"))
        MinFishingLevel = 0;
    }
    FishSpawnData fishSpawnData;
    if (metadata.CustomFishSpawnRules.TryGetValue(fish.QualifiedItemId, out fishSpawnData))
    {
      if (fishSpawnData.MinFishingLevel > MinFishingLevel)
        MinFishingLevel = fishSpawnData.MinFishingLevel;
      if (fishSpawnData.Weather != FishSpawnWeather.Unknown)
        Weather = fishSpawnData.Weather;
      IsUnique = IsUnique || fishSpawnData.IsUnique;
      if (fishSpawnData.TimesOfDay != null)
        spawnTimeOfDayDataList.AddRange((IEnumerable<FishSpawnTimeOfDayData>) fishSpawnData.TimesOfDay);
      if (fishSpawnData.Locations != null)
        source1.AddRange((IEnumerable<FishSpawnLocationData>) fishSpawnData.Locations);
    }
    return new FishSpawnData(fish, source1.ToArray(), spawnTimeOfDayDataList.ToArray(), Weather, MinFishingLevel, IsUnique, IsLegendaryFamily);
  }

  public IEnumerable<FishSpawnData> GetFishSpawnRules(
    GameLocation location,
    Vector2 tile,
    string fishAreaId,
    Metadata metadata)
  {
    HashSet<string> seenFishIds = new HashSet<string>();
    foreach (SpawnFishData spawnFishData in location.GetData().Fish)
    {
      if (((GenericSpawnItemData) spawnFishData).ItemId != null)
      {
        seenFishIds.Add(((GenericSpawnItemData) spawnFishData).ItemId);
        if (spawnFishData.FishAreaId == null || !(spawnFishData.FishAreaId != fishAreaId))
        {
          Rectangle? bobberPosition = spawnFishData.BobberPosition;
          ref Rectangle? local1 = ref bobberPosition;
          Rectangle valueOrDefault;
          bool? nullable1;
          if (!local1.HasValue)
          {
            nullable1 = new bool?();
          }
          else
          {
            valueOrDefault = local1.GetValueOrDefault();
            nullable1 = new bool?(((Rectangle) ref valueOrDefault).Contains(tile));
          }
          bool? nullable2 = nullable1;
          if (!nullable2.HasValue || nullable2.GetValueOrDefault())
          {
            Rectangle? playerPosition = spawnFishData.PlayerPosition;
            ref Rectangle? local2 = ref playerPosition;
            bool? nullable3;
            if (!local2.HasValue)
            {
              nullable3 = new bool?();
            }
            else
            {
              valueOrDefault = local2.GetValueOrDefault();
              nullable3 = new bool?(((Rectangle) ref valueOrDefault).Contains(((Character) Game1.player).TilePoint));
            }
            bool? nullable4 = nullable3;
            if (!nullable4.HasValue || nullable4.GetValueOrDefault())
            {
              ParsedItemData dataOrErrorItem = ItemRegistry.GetDataOrErrorItem(((GenericSpawnItemData) spawnFishData).ItemId);
              if (!(dataOrErrorItem.ObjectType != "Fish"))
                yield return this.GetFishSpawnRules(dataOrErrorItem, metadata);
            }
          }
        }
      }
    }
    foreach ((string key, FishSpawnData fishSpawnData) in metadata.CustomFishSpawnRules)
    {
      if (!seenFishIds.Contains(key) && fishSpawnData.MatchesLocation(location.Name))
        yield return this.GetFishSpawnRules(ItemRegistry.GetDataOrErrorItem(key), metadata);
    }
  }

  public FriendshipModel GetFriendshipForVillager(
    Farmer player,
    NPC npc,
    Friendship friendship,
    Metadata metadata)
  {
    return new FriendshipModel(player, npc, friendship, metadata.Constants);
  }

  public FriendshipModel GetFriendshipForPet(Farmer player, Pet pet)
  {
    return new FriendshipModel(((NetFieldBase<int, NetInt>) pet.friendshipTowardFarmer).Value, 100, 1000);
  }

  public FriendshipModel GetFriendshipForAnimal(
    Farmer player,
    FarmAnimal animal,
    Metadata metadata)
  {
    return new FriendshipModel(((NetFieldBase<int, NetInt>) animal.friendshipTowardFarmer).Value, metadata.Constants.AnimalFriendshipPointsPerLevel, metadata.Constants.AnimalFriendshipMaxPoints);
  }

  public string GetLocationDisplayName(FishSpawnLocationData fishSpawnData)
  {
    LocationData data;
    if (!Game1.locationData.TryGetValue(fishSpawnData.LocationId, out data))
      data = (LocationData) null;
    return this.GetLocationDisplayName(fishSpawnData.LocationId, data, fishSpawnData.Area);
  }

  public string GetLocationDisplayName(string id, LocationData? data)
  {
    string locationDisplayName = Translation.op_Implicit(I18n.GetByKey("location." + id).UsePlaceholder(false));
    if (!string.IsNullOrWhiteSpace(locationDisplayName))
      return locationDisplayName;
    if (data != null)
    {
      string text = TokenParser.ParseText(data.DisplayName, (Random) null, (TokenParserDelegate) null, (Farmer) null);
      if (!string.IsNullOrWhiteSpace(text))
        return text;
    }
    return id;
  }

  public string GetLocationDisplayName(string id, LocationData? data, string? fishAreaId)
  {
    int num;
    if (MineShaft.IsGeneratedLevel(id, ref num))
    {
      string level = fishAreaId ?? num.ToString();
      return string.IsNullOrWhiteSpace(level) ? this.GetLocationDisplayName(id, data) : I18n.Location_UndergroundMine_Level((object) level);
    }
    if (string.IsNullOrWhiteSpace(fishAreaId))
      return this.GetLocationDisplayName(id, data);
    string locationDisplayName1 = this.GetLocationDisplayName(id, data);
    string str;
    if (data == null)
    {
      str = (string) null;
    }
    else
    {
      Dictionary<string, FishAreaData> fishAreas = data.FishAreas;
      str = fishAreas != null ? fishAreas.GetValueOrDefault<string, FishAreaData>(fishAreaId)?.DisplayName : (string) null;
    }
    string text = TokenParser.ParseText(str, (Random) null, (TokenParserDelegate) null, (Farmer) null);
    string locationDisplayName2 = Translation.op_Implicit(I18n.GetByKey($"location.{id}.{fishAreaId}", (object) new
    {
      locationName = locationDisplayName1
    }).UsePlaceholder(false));
    if (string.IsNullOrWhiteSpace(locationDisplayName2))
      locationDisplayName2 = !string.IsNullOrWhiteSpace(text) ? I18n.Location_FishArea((object) locationDisplayName1, (object) text) : I18n.Location_UnknownFishArea((object) locationDisplayName1, (object) fishAreaId);
    return locationDisplayName2;
  }

  public IEnumerable<MonsterData> GetMonsters()
  {
    foreach ((string str1, string str2) in DataLoader.Monsters(Game1.content))
    {
      if (str2 != null)
      {
        string[] strArray1 = str2.Split('/');
        int Health = ArgUtility.GetInt(strArray1, 0, 0);
        int DamageToFarmer = ArgUtility.GetInt(strArray1, 1, 0);
        bool IsGlider = ArgUtility.GetBool(strArray1, 4, false);
        int Resilience = ArgUtility.GetInt(strArray1, 7, 0);
        double Jitteriness = (double) ArgUtility.GetFloat(strArray1, 8, 0.0f);
        int MoveTowardsPlayerThreshold = ArgUtility.GetInt(strArray1, 9, 0);
        int Speed = ArgUtility.GetInt(strArray1, 10, 0);
        double MissChance = (double) ArgUtility.GetFloat(strArray1, 11, 0.0f);
        bool IsMineMonster = ArgUtility.GetBool(strArray1, 12, false);
        List<ItemDropData> itemDropDataList1 = new List<ItemDropData>();
        string[] strArray2 = ArgUtility.SplitBySpace(ArgUtility.Get(strArray1, 6, (string) null, true));
        int num;
        for (int index = 0; index < strArray2.Length; index += 2)
        {
          string str3 = ArgUtility.Get(strArray2, index, (string) null, true);
          float Probability = ArgUtility.GetFloat(strArray2, index + 1, 0.0f);
          int MaxDrop = 1;
          int result;
          if (int.TryParse(str3, out result) && result < 0)
          {
            num = -result;
            str3 = num.ToString();
            MaxDrop = 3;
          }
          string str4 = str3;
          num = 0;
          string str5 = num.ToString();
          if (str4 == str5)
          {
            num = 378;
            str3 = num.ToString();
          }
          else
          {
            string str6 = str3;
            num = 2;
            string str7 = num.ToString();
            if (str6 == str7)
            {
              num = 380;
              str3 = num.ToString();
            }
            else
            {
              string str8 = str3;
              num = 4;
              string str9 = num.ToString();
              if (str8 == str9)
              {
                num = 382;
                str3 = num.ToString();
              }
              else
              {
                string str10 = str3;
                num = 6;
                string str11 = num.ToString();
                if (str10 == str11)
                {
                  num = 384;
                  str3 = num.ToString();
                }
                else
                {
                  string str12 = str3;
                  num = 8;
                  string str13 = num.ToString();
                  if (!(str12 == str13))
                  {
                    string str14 = str3;
                    num = 10;
                    string str15 = num.ToString();
                    if (str14 == str15)
                    {
                      num = 386;
                      str3 = num.ToString();
                    }
                    else
                    {
                      string str16 = str3;
                      num = 12;
                      string str17 = num.ToString();
                      if (str16 == str17)
                      {
                        num = 388;
                        str3 = num.ToString();
                      }
                      else
                      {
                        string str18 = str3;
                        num = 14;
                        string str19 = num.ToString();
                        if (str18 == str19)
                        {
                          num = 390;
                          str3 = num.ToString();
                        }
                      }
                    }
                  }
                  else
                    continue;
                }
              }
            }
          }
          itemDropDataList1.Add(new ItemDropData(str3, 1, MaxDrop, Probability));
        }
        if (IsMineMonster && Game1.player.timesReachedMineBottom >= 1)
        {
          List<ItemDropData> itemDropDataList2 = itemDropDataList1;
          num = 72;
          ItemDropData itemDropData1 = new ItemDropData(num.ToString(), 1, 1, 0.008f);
          itemDropDataList2.Add(itemDropData1);
          List<ItemDropData> itemDropDataList3 = itemDropDataList1;
          num = 74;
          ItemDropData itemDropData2 = new ItemDropData(num.ToString(), 1, 1, 0.008f);
          itemDropDataList3.Add(itemDropData2);
        }
        yield return new MonsterData(str1, Health, DamageToFarmer, IsGlider, Resilience, Jitteriness, MoveTowardsPlayerThreshold, Speed, MissChance, IsMineMonster, itemDropDataList1.ToArray());
      }
    }
  }

  public RecipeModel[] GetRecipes(
    Metadata metadata,
    IMonitor monitor,
    ExtraMachineConfigIntegration extraMachineConfig)
  {
    List<RecipeModel> recipeModelList = new List<RecipeModel>();
    foreach (var data in CraftingRecipe.cookingRecipes.Select(pair =>
    {
      KeyValuePair<string, string> keyValuePair = pair;
      string key = keyValuePair.Key;
      keyValuePair = pair;
      string str = keyValuePair.Value;
      return new
      {
        Key = key,
        Value = str,
        IsCookingRecipe = true
      };
    }).Concat(CraftingRecipe.craftingRecipes.Select(pair =>
    {
      KeyValuePair<string, string> keyValuePair = pair;
      string key = keyValuePair.Key;
      keyValuePair = pair;
      string str = keyValuePair.Value;
      return new
      {
        Key = key,
        Value = str,
        IsCookingRecipe = false
      };
    })))
    {
      if (data.Value != null)
      {
        try
        {
          CraftingRecipe recipe = new CraftingRecipe(data.Key, data.IsCookingRecipe);
          foreach (string itemId in recipe.itemToProduce)
          {
            string outputQualifiedItemId = RecipeModel.QualifyRecipeOutputId(recipe, itemId) ?? itemId;
            recipeModelList.Add(new RecipeModel(recipe, outputQualifiedItemId));
          }
        }
        catch (Exception ex)
        {
          monitor.Log($"Couldn't parse {(data.IsCookingRecipe ? "cooking" : "crafting")} recipe '{data.Key}' due to an invalid format.\nRecipe data: '{data.Value}'\nError: {ex}", (LogLevel) 3);
        }
      }
    }
    foreach ((string key, MachineData machineData) in DataLoader.Machines(Game1.content))
    {
      string str1 = key;
      MachineData machine = machineData;
      string qualifiedMachineId = str1;
      if (ItemRegistry.Exists(qualifiedMachineId))
      {
        int? count1 = machine?.OutputRules?.Count;
        if (count1.HasValue && count1.GetValueOrDefault() > 0)
        {
          List<MachineItemAdditionalConsumedItems> additionalConsumedItems = machine.AdditionalConsumedItems;
          RecipeIngredientModel[] collection = (additionalConsumedItems != null ? additionalConsumedItems.Select<MachineItemAdditionalConsumedItems, RecipeIngredientModel>((Func<MachineItemAdditionalConsumedItems, RecipeIngredientModel>) (item => new RecipeIngredientModel(RecipeType.MachineInput, item.ItemId, item.RequiredCount))).ToArray<RecipeIngredientModel>() : (RecipeIngredientModel[]) null) ?? Array.Empty<RecipeIngredientModel>();
          bool flag = false;
          foreach (MachineOutputRule outputRule1 in machine.OutputRules)
          {
            MachineOutputRule outputRule = outputRule1;
            count1 = outputRule?.Triggers?.Count;
            if (count1.HasValue && count1.GetValueOrDefault() > 0)
            {
              count1 = outputRule.OutputItem?.Count;
              if (count1.HasValue && count1.GetValueOrDefault() > 0)
              {
                foreach (MachineOutputTriggerRule trigger in outputRule.Triggers)
                {
                  if (trigger != null)
                  {
                    foreach (MachineItemOutput outputData in outputRule.OutputItem)
                    {
                      if (outputData != null)
                      {
                        MachineItemOutput[] machineItemOutputArray1;
                        if (extraMachineConfig.IsLoaded)
                        {
                          MachineItemOutput machineItemOutput1 = outputData;
                          IList<MachineItemOutput> extraOutputs = extraMachineConfig.ModApi.GetExtraOutputs(outputData, machine);
                          int index1 = 0;
                          MachineItemOutput[] machineItemOutputArray2 = new MachineItemOutput[1 + extraOutputs.Count];
                          machineItemOutputArray2[index1] = machineItemOutput1;
                          int index2 = index1 + 1;
                          foreach (MachineItemOutput machineItemOutput2 in (IEnumerable<MachineItemOutput>) extraOutputs)
                          {
                            machineItemOutputArray2[index2] = machineItemOutput2;
                            ++index2;
                          }
                          machineItemOutputArray1 = machineItemOutputArray2;
                        }
                        else
                          machineItemOutputArray1 = new MachineItemOutput[1]
                          {
                            outputData
                          };
                        foreach (MachineItemOutput machineItemOutput in machineItemOutputArray1)
                        {
                          MachineItemOutput outputItem = machineItemOutput;
                          List<string> conditions = (List<string>) null;
                          string str2 = (string) null;
                          if (!string.IsNullOrWhiteSpace(trigger.Condition))
                            str2 = trigger.Condition;
                          if (!string.IsNullOrWhiteSpace(((GenericSpawnItemDataWithCondition) outputData).Condition))
                            str2 = str2 != null ? $"{str2}, {((GenericSpawnItemDataWithCondition) outputData).Condition}" : ((GenericSpawnItemDataWithCondition) outputData).Condition;
                          if (!string.IsNullOrWhiteSpace(((GenericSpawnItemDataWithCondition) outputItem).Condition) && ((GenericSpawnItemDataWithCondition) outputItem).Condition != ((GenericSpawnItemDataWithCondition) outputData).Condition)
                            str2 = str2 != null ? $"{str2}, {((GenericSpawnItemDataWithCondition) outputItem).Condition}" : ((GenericSpawnItemDataWithCondition) outputItem).Condition;
                          if (str2 != null)
                            conditions = ((IEnumerable<string>) GameStateQuery.SplitRaw(str2)).Distinct<string>().ToList<string>();
                          string itemId;
                          string[] contextTags;
                          if (this.TryGetMostSpecificIngredientIds(trigger.RequiredItemId, trigger.RequiredTags, ref conditions, out itemId, out contextTags))
                          {
                            if (outputItem.OutputMethod != null)
                              flag = true;
                            List<RecipeIngredientModel> ingredients = new List<RecipeIngredientModel>(1)
                            {
                              new RecipeIngredientModel(RecipeType.MachineInput, itemId, trigger.RequiredCount, contextTags)
                            };
                            ingredients.AddRange((IEnumerable<RecipeIngredientModel>) collection);
                            if (extraMachineConfig.IsLoaded)
                            {
                              foreach ((string inputId, int count2) in (IEnumerable<(string, int)>) extraMachineConfig.ModApi.GetExtraRequirements(outputItem))
                                ingredients.Add(new RecipeIngredientModel(RecipeType.MachineInput, inputId, count2));
                              foreach ((string str3, int count3) in (IEnumerable<(string, int)>) extraMachineConfig.ModApi.GetExtraTagsRequirements(outputItem))
                                ingredients.Add(new RecipeIngredientModel(RecipeType.MachineInput, (string) null, count3, str3.Split(",")));
                            }
                            IList<ItemQueryResult> itemQueryResults;
                            if (((GenericSpawnItemData) outputItem).ItemId != null || ((GenericSpawnItemData) outputItem).RandomItemId != null)
                            {
                              ItemQueryContext itemQueryContext = new ItemQueryContext();
                              itemQueryResults = ItemQueryResolver.TryResolve((ISpawnItemData) outputItem, itemQueryContext, (ItemQuerySearchMode) 0, false, (HashSet<string>) null, (Func<string, string>) (id => id?.Replace("DROP_IN_ID", "0").Replace("DROP_IN_PRESERVE", "0").Replace("NEARBY_FLOWER_ID", "0")), (Action<string, string>) null, (Item) null);
                            }
                            else
                            {
                              itemQueryResults = (IList<ItemQueryResult>) new List<ItemQueryResult>();
                              flag = true;
                            }
                            recipeModelList.AddRange(itemQueryResults.Select<ItemQueryResult, RecipeModel>((Func<ItemQueryResult, RecipeModel>) (result =>
                            {
                              string displayName = ItemRegistry.GetDataOrErrorItem(qualifiedMachineId).DisplayName;
                              List<RecipeIngredientModel> ingredients1 = ingredients;
                              Func<Item, Item> func = (Func<Item, Item>) (_ => ItemRegistry.Create(result.Item.QualifiedItemId, 1, 0, false));
                              string machineId = qualifiedMachineId;
                              string qualifiedItemId = result.Item.QualifiedItemId;
                              int? minOutput = new int?(((GenericSpawnItemData) outputItem).MinStack > 0 ? ((GenericSpawnItemData) outputItem).MinStack : 1);
                              int? maxOutput = ((GenericSpawnItemData) outputItem).MaxStack > 0 ? new int?(((GenericSpawnItemData) outputItem).MaxStack) : new int?();
                              int? nullable = new int?(((GenericSpawnItemData) outputItem).Quality);
                              Decimal? outputChance = new Decimal?((Decimal) (100 / outputRule.OutputItem.Count / itemQueryResults.Count));
                              int? quality = nullable;
                              string[] array = conditions?.ToArray();
                              return new RecipeModel((string) null, RecipeType.MachineInput, displayName, (IEnumerable<RecipeIngredientModel>) ingredients1, 0, func, (Func<bool>) (() => true), machineId, outputQualifiedItemId: qualifiedItemId, minOutput: minOutput, maxOutput: maxOutput, outputChance: outputChance, quality: quality, conditions: array);
                            })));
                          }
                        }
                      }
                    }
                  }
                }
              }
            }
          }
          if (flag)
            recipeModelList.Add(new RecipeModel((string) null, RecipeType.MachineInput, ItemRegistry.GetDataOrErrorItem(qualifiedMachineId).DisplayName, (IEnumerable<RecipeIngredientModel>) Array.Empty<RecipeIngredientModel>(), 0, (Func<Item, Item>) (_ => ItemRegistry.Create("__COMPLEX_RECIPE__", 1, 0, false)), (Func<bool>) (() => true), qualifiedMachineId, outputQualifiedItemId: "__COMPLEX_RECIPE__"));
        }
      }
    }
    BuildingData buildingData2;
    foreach ((key, buildingData2) in (IEnumerable<KeyValuePair<string, BuildingData>>) Game1.buildingData)
    {
      string buildingType = key;
      BuildingData buildingData = buildingData2;
      BuildingData buildingData3 = buildingData;
      int? count;
      if ((buildingData3 != null ? (buildingData3.BuildCost > 0 ? 1 : 0) : 0) == 0)
      {
        BuildingData buildingData4 = buildingData;
        int num1;
        if (buildingData4 == null)
        {
          num1 = 0;
        }
        else
        {
          count = buildingData4.BuildMaterials?.Count;
          int num2 = 0;
          num1 = count.GetValueOrDefault() > num2 & count.HasValue ? 1 : 0;
        }
        if (num1 == 0)
          goto label_91;
      }
      RecipeIngredientModel[] ingredients2 = RecipeModel.ParseIngredients(buildingData);
      Building building;
      try
      {
        building = new Building(buildingType, Vector2.Zero);
      }
      catch
      {
        continue;
      }
      recipeModelList.Add(new RecipeModel(building, ingredients2, buildingData.BuildCost));
label_91:
      BuildingData buildingData5 = buildingData;
      int num3;
      if (buildingData5 == null)
      {
        num3 = 0;
      }
      else
      {
        count = buildingData5.ItemConversions?.Count;
        int num4 = 0;
        num3 = count.GetValueOrDefault() > num4 & count.HasValue ? 1 : 0;
      }
      if (num3 != 0)
      {
        foreach (BuildingItemConversion itemConversion in buildingData.ItemConversions)
        {
          count = itemConversion?.ProducedItems?.Count;
          if (count.HasValue && count.GetValueOrDefault() > 0)
          {
            count = itemConversion.RequiredTags?.Count;
            if (count.HasValue && count.GetValueOrDefault() > 0)
            {
              List<string> fromConditions = (List<string>) null;
              string itemId;
              string[] contextTags;
              if (this.TryGetMostSpecificIngredientIds((string) null, itemConversion.RequiredTags, ref fromConditions, out itemId, out contextTags))
              {
                RecipeIngredientModel[] ingredients = new RecipeIngredientModel[1]
                {
                  new RecipeIngredientModel(RecipeType.BuildingInput, itemId, itemConversion.RequiredCount, contextTags)
                };
                foreach (GenericSpawnItemDataWithCondition producedItem in itemConversion.ProducedItems)
                {
                  GenericSpawnItemDataWithCondition outputItem = producedItem;
                  if (outputItem != null)
                  {
                    IList<ItemQueryResult> itemQueryResults = ItemQueryResolver.TryResolve((ISpawnItemData) outputItem, new ItemQueryContext(), (ItemQuerySearchMode) 0, false, (HashSet<string>) null, (Func<string, string>) null, (Action<string, string>) null, (Item) null);
                    string[] conditions = !string.IsNullOrWhiteSpace(outputItem.Condition) ? ((IEnumerable<string>) GameStateQuery.SplitRaw(outputItem.Condition)).Distinct<string>().ToArray<string>() : (string[]) null;
                    recipeModelList.AddRange(itemQueryResults.Select<ItemQueryResult, RecipeModel>((Func<ItemQueryResult, RecipeModel>) (result =>
                    {
                      string displayType = TokenParser.ParseText(buildingData?.Name, (Random) null, (TokenParserDelegate) null, (Farmer) null) ?? buildingType;
                      RecipeIngredientModel[] ingredients3 = ingredients;
                      Func<Item, Item> func = (Func<Item, Item>) (_ => ItemRegistry.Create(result.Item.QualifiedItemId, 1, 0, false));
                      string machineId = buildingType;
                      string qualifiedItemId = result.Item.QualifiedItemId;
                      int? minOutput = new int?(((GenericSpawnItemData) outputItem).MinStack > 0 ? ((GenericSpawnItemData) outputItem).MinStack : 1);
                      int? maxOutput = ((GenericSpawnItemData) outputItem).MaxStack > 0 ? new int?(((GenericSpawnItemData) outputItem).MaxStack) : new int?();
                      int? nullable = new int?(((GenericSpawnItemData) outputItem).Quality);
                      Decimal? outputChance = new Decimal?((Decimal) (100 / itemQueryResults.Count));
                      int? quality = nullable;
                      string[] conditions1 = conditions;
                      return new RecipeModel((string) null, RecipeType.BuildingInput, displayType, (IEnumerable<RecipeIngredientModel>) ingredients3, 0, func, (Func<bool>) (() => true), machineId, outputQualifiedItemId: qualifiedItemId, minOutput: minOutput, maxOutput: maxOutput, outputChance: outputChance, quality: quality, conditions: conditions1);
                    })));
                  }
                }
              }
            }
          }
        }
      }
    }
    return recipeModelList.ToArray();
  }

  public string[] GetItemSpawnFieldIds(List<string?>? randomItemIds, string? itemId)
  {
    if (randomItemIds != null)
      return randomItemIds.Where<string>((Func<string, bool>) (id => id != null)).ToArray<string>();
    if (itemId == null)
      return Array.Empty<string>();
    return new string[1]{ itemId };
  }

  private bool TryGetMostSpecificIngredientIds(
    string? fromItemId,
    List<string?>? fromContextTags,
    ref List<string>? fromConditions,
    out string? itemId,
    out string[] contextTags)
  {
    contextTags = (fromContextTags != null ? fromContextTags.WhereNotNull<string>().ToArray<string>() : (string[]) null) ?? Array.Empty<string>();
    itemId = !string.IsNullOrWhiteSpace(fromItemId) ? fromItemId : (string) null;
    ParsedItemData data1;
    if (contextTags.Length == 1 && MachineDataHelper.TryGetUniqueItemFromContextTag(contextTags[0], out data1))
    {
      if (itemId != null && ItemRegistry.QualifyItemId(itemId) != data1.QualifiedItemId)
        return false;
      itemId = data1.QualifiedItemId;
      contextTags = Array.Empty<string>();
    }
    if (fromConditions != null)
    {
      for (int index = 0; index < fromConditions.Count; ++index)
      {
        ParsedItemData data2;
        if (MachineDataHelper.TryGetUniqueItemFromGameStateQuery(fromConditions[index], out data2))
        {
          if (itemId != null && data2.QualifiedItemId != ItemRegistry.QualifyItemId(itemId))
            return false;
          itemId = data2.QualifiedItemId;
          fromConditions.RemoveAt(index);
        }
      }
    }
    return itemId != null || contextTags.Length != 0;
  }
}
