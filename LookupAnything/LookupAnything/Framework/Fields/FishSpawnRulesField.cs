// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.Fields.FishSpawnRulesField
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Netcode;
using Pathoschild.Stardew.Common;
using Pathoschild.Stardew.LookupAnything.Framework.Fields.Models;
using Pathoschild.Stardew.LookupAnything.Framework.Models.FishData;
using StardewValley;
using StardewValley.ItemTypeDefinitions;
using StardewValley.SpecialOrders;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework.Fields;

internal class FishSpawnRulesField : CheckboxListField
{
  private static readonly string[] Seasons = new string[4]
  {
    "spring",
    "summer",
    "fall",
    "winter"
  };

  public FishSpawnRulesField(
    GameHelper gameHelper,
    string label,
    ParsedItemData fish,
    bool showUncaughtFishSpawnRules)
    : this(label, new CheckboxList(FishSpawnRulesField.GetConditions(gameHelper, fish), !showUncaughtFishSpawnRules && !FishSpawnRulesField.HasPlayerCaughtFish(fish)))
  {
  }

  public FishSpawnRulesField(
    GameHelper gameHelper,
    string label,
    GameLocation location,
    Vector2 tile,
    string fishAreaId,
    bool showUncaughtFishSpawnRules)
    : this(label, FishSpawnRulesField.GetConditions(gameHelper, location, tile, fishAreaId, showUncaughtFishSpawnRules).ToArray<CheckboxList>())
  {
  }

  public override Vector2? DrawValue(
    SpriteBatch spriteBatch,
    SpriteFont font,
    Vector2 position,
    float wrapWidth)
  {
    float num = 0.0f;
    int count = 0;
    foreach (CheckboxList checkboxList in this.CheckboxLists)
    {
      if (checkboxList.IsHidden)
        ++count;
      else
        num += this.DrawCheckboxList(checkboxList, spriteBatch, font, new Vector2(position.X, position.Y + num), wrapWidth).Y;
    }
    if (count > 0)
      num += this.LineHeight + this.DrawIconText(spriteBatch, font, new Vector2(position.X, position.Y + num), wrapWidth, I18n.Item_UncaughtFish((object) count), Color.Gray).Y;
    return new Vector2?(new Vector2(wrapWidth, num - this.LineHeight));
  }

  private FishSpawnRulesField(string label, params CheckboxList[] spawnConditions)
    : base(label)
  {
    this.CheckboxLists = spawnConditions;
    this.HasValue = ((IEnumerable<CheckboxList>) this.CheckboxLists).Any<CheckboxList>();
  }

  private static IEnumerable<CheckboxList> GetConditions(
    GameHelper gameHelper,
    GameLocation location,
    Vector2 tile,
    string fishAreaId,
    bool showUncaughtFishSpawnRules)
  {
    foreach (FishSpawnData fishSpawnRule in gameHelper.GetFishSpawnRules(location, tile, fishAreaId))
    {
      ParsedItemData dataOrErrorItem = ItemRegistry.GetDataOrErrorItem(fishSpawnRule.FishItem.QualifiedItemId);
      bool isHidden = !showUncaughtFishSpawnRules && !FishSpawnRulesField.HasPlayerCaughtFish(dataOrErrorItem);
      CheckboxList condition = new CheckboxList(FishSpawnRulesField.GetConditions(gameHelper, dataOrErrorItem), isHidden);
      condition.AddIntro(dataOrErrorItem.DisplayName, new SpriteInfo(dataOrErrorItem.GetTexture(), dataOrErrorItem.GetSourceRect(0, new int?())));
      yield return condition;
    }
  }

  private static IEnumerable<Checkbox> GetConditions(GameHelper gameHelper, ParsedItemData fish)
  {
    FishSpawnData spawnRules = gameHelper.GetFishSpawnRules(fish);
    FishSpawnLocationData[] locations = spawnRules.Locations;
    if ((locations != null ? (!((IEnumerable<FishSpawnLocationData>) locations).Any<FishSpawnLocationData>() ? 1 : 0) : 1) == 0)
    {
      if (spawnRules.IsUnique)
        yield return FishSpawnRulesField.GetCondition(I18n.Item_FishSpawnRules_NotCaughtYet(), !FishSpawnRulesField.HasPlayerCaughtFish(fish));
      if (spawnRules.MinFishingLevel > 0)
        yield return FishSpawnRulesField.GetCondition(I18n.Item_FishSpawnRules_MinFishingLevel((object) spawnRules.MinFishingLevel), Game1.player.FishingLevel >= spawnRules.MinFishingLevel);
      if (spawnRules.IsLegendaryFamily)
        yield return FishSpawnRulesField.GetCondition(I18n.Item_FishSpawnRules_ExtendedFamilyQuestActive(), Game1.player.team.SpecialOrderRuleActive("LEGENDARY_FAMILY", (SpecialOrder) null));
      if (spawnRules.Weather == FishSpawnWeather.Sunny)
        yield return FishSpawnRulesField.GetCondition(I18n.Item_FishSpawnRules_WeatherSunny(), !Game1.isRaining);
      else if (spawnRules.Weather == FishSpawnWeather.Rainy)
        yield return FishSpawnRulesField.GetCondition(I18n.Item_FishSpawnRules_WeatherRainy(), Game1.isRaining);
      FishSpawnTimeOfDayData[] timesOfDay = spawnRules.TimesOfDay;
      if ((timesOfDay != null ? (((IEnumerable<FishSpawnTimeOfDayData>) timesOfDay).Any<FishSpawnTimeOfDayData>() ? 1 : 0) : 0) != 0)
        yield return FishSpawnRulesField.GetCondition(I18n.Item_FishSpawnRules_Time((object) I18n.List((IEnumerable<object>) ((IEnumerable<FishSpawnTimeOfDayData>) spawnRules.TimesOfDay).Select<FishSpawnTimeOfDayData, string>((Func<FishSpawnTimeOfDayData, string>) (p => I18n.Generic_Range((object) CommonHelper.FormatTime(p.MinTime), (object) CommonHelper.FormatTime(p.MaxTime)).ToString())))), ((IEnumerable<FishSpawnTimeOfDayData>) spawnRules.TimesOfDay).Any<FishSpawnTimeOfDayData>((Func<FishSpawnTimeOfDayData, bool>) (p => Game1.timeOfDay >= p.MinTime && Game1.timeOfDay <= p.MaxTime)));
      if (FishSpawnRulesField.HaveSameSeasons((IEnumerable<FishSpawnLocationData>) spawnRules.Locations))
      {
        FishSpawnLocationData location = spawnRules.Locations[0];
        if (location.Seasons.Count == 4)
          yield return FishSpawnRulesField.GetCondition(I18n.Item_FishSpawnRules_SeasonAny(), true);
        else
          yield return FishSpawnRulesField.GetCondition(I18n.Item_FishSpawnRules_SeasonList((object) I18n.List((IEnumerable<object>) location.Seasons.Select<string, string>(new Func<string, string>(gameHelper.TranslateSeason)))), location.Seasons.Contains(Game1.currentSeason));
        yield return FishSpawnRulesField.GetCondition(I18n.Item_FishSpawnRules_Locations((object) I18n.List((IEnumerable<object>) ((IEnumerable<FishSpawnLocationData>) spawnRules.Locations).Select<FishSpawnLocationData, string>(new Func<FishSpawnLocationData, string>(gameHelper.GetLocationDisplayName)).OrderBy<string, string>((Func<string, string>) (p => p)))), spawnRules.MatchesLocation(Game1.currentLocation.Name));
      }
      else
      {
        IDictionary<string, string[]> dictionary = (IDictionary<string, string[]>) ((IEnumerable<FishSpawnLocationData>) spawnRules.Locations).SelectMany((Func<FishSpawnLocationData, IEnumerable<string>>) (location => (IEnumerable<string>) location.Seasons), (location, season) => new
        {
          Season = season,
          LocationName = gameHelper.GetLocationDisplayName(location)
        }).GroupBy(p => p.Season, p => p.LocationName, (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase).ToDictionary<IGrouping<string, string>, string, string[]>((Func<IGrouping<string, string>, string>) (p => p.Key), (Func<IGrouping<string, string>, string[]>) (p => p.ToArray<string>()), (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
        List<IFormattedText> label = new List<IFormattedText>()
        {
          (IFormattedText) new FormattedText(I18n.Item_FishSpawnRules_LocationsBySeason_Label())
        };
        foreach (string season in FishSpawnRulesField.Seasons)
        {
          string[] values;
          if (dictionary.TryGetValue(season, out values))
            label.Add((IFormattedText) new FormattedText(Environment.NewLine + I18n.Item_FishSpawnRules_LocationsBySeason_SeasonLocations((object) gameHelper.TranslateSeason(season), (object) I18n.List((IEnumerable<object>) values)), new Color?(season == Game1.currentSeason ? Color.Black : Color.Gray)));
        }
        bool isMet = ((IEnumerable<FishSpawnLocationData>) spawnRules.Locations).Any<FishSpawnLocationData>((Func<FishSpawnLocationData, bool>) (p => p.LocationId == Game1.currentLocation.Name && p.Seasons.Contains(Game1.currentSeason)));
        yield return FishSpawnRulesField.GetCondition((IEnumerable<IFormattedText>) label, isMet);
      }
    }
  }

  private static Checkbox GetCondition(string label, bool isMet) => new Checkbox(isMet, label);

  private static Checkbox GetCondition(IEnumerable<IFormattedText> label, bool isMet)
  {
    return new Checkbox(isMet, label.ToArray<IFormattedText>());
  }

  private static bool HaveSameSeasons(IEnumerable<FishSpawnLocationData> locations)
  {
    ISet<string> stringSet = (ISet<string>) null;
    foreach (FishSpawnLocationData location in locations)
    {
      if (stringSet == null)
        stringSet = (ISet<string>) location.Seasons;
      else if (stringSet.Count != location.Seasons.Count || !location.Seasons.All<string>(new Func<string, bool>(((ICollection<string>) stringSet).Contains)))
        return false;
    }
    return true;
  }

  private static bool HasPlayerCaughtFish(ParsedItemData fish)
  {
    return ((NetDictionary<string, int[], NetArray<int, NetInt>, SerializableDictionary<string, int[]>, NetStringIntArrayDictionary>) Game1.player.fishCaught).ContainsKey(fish.QualifiedItemId);
  }
}
