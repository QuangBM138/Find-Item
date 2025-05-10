// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.Lookups.TerrainFeatures.FruitTreeSubject
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Netcode;
using Pathoschild.Stardew.LookupAnything.Framework.Constants;
using Pathoschild.Stardew.LookupAnything.Framework.DebugFields;
using Pathoschild.Stardew.LookupAnything.Framework.Fields;
using StardewModdingAPI;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.GameData.FruitTrees;
using StardewValley.TerrainFeatures;
using StardewValley.TokenizableStrings;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework.Lookups.TerrainFeatures;

internal class FruitTreeSubject : BaseSubject
{
  private readonly FruitTree Target;
  private readonly Vector2 Tile;

  public FruitTreeSubject(GameHelper gameHelper, FruitTree tree, Vector2 tile)
    : base(gameHelper, I18n.FruitTree_Name((object) FruitTreeSubject.GetDisplayName(tree)), (string) null, I18n.Type_FruitTree())
  {
    this.Target = tree;
    this.Tile = tile;
  }

  public override IEnumerable<ICustomField> GetData()
  {
    FruitTreeSubject fruitTreeSubject = this;
    FruitTree tree = fruitTreeSubject.Target;
    bool isMature = ((NetFieldBase<int, NetInt>) tree.daysUntilMature).Value <= 0;
    bool isDead = ((NetFieldBase<bool, NetBool>) tree.stump).Value;
    bool isStruckByLightning = ((NetFieldBase<int, NetInt>) tree.struckByLightningCountdown).Value > 0;
    IModInfo modFromStringId = fruitTreeSubject.GameHelper.TryGetModFromStringId(((NetFieldBase<string, NetString>) tree.treeId).Value);
    if (modFromStringId != null)
      yield return (ICustomField) new GenericField(I18n.AddedByMod(), I18n.AddedByMod_Summary((object) modFromStringId.Manifest.Name));
    if (isMature && !isDead)
    {
      SDate sdate = SDate.Now().AddDays(1);
      string label = I18n.FruitTree_NextFruit();
      if (isStruckByLightning)
        yield return (ICustomField) new GenericField(label, I18n.FruitTree_NextFruit_StruckByLightning((object) ((NetFieldBase<int, NetInt>) tree.struckByLightningCountdown).Value));
      else if (!fruitTreeSubject.IsInSeason(tree, sdate.Season))
        yield return (ICustomField) new GenericField(label, I18n.FruitTree_NextFruit_OutOfSeason());
      else if (tree.fruit.Count >= 3)
        yield return (ICustomField) new GenericField(label, I18n.FruitTree_NextFruit_MaxFruit());
      else
        yield return (ICustomField) new GenericField(label, I18n.Generic_Tomorrow());
    }
    if (!isMature)
    {
      SDate dayOfMaturity = SDate.Now().AddDays(((NetFieldBase<int, NetInt>) tree.daysUntilMature).Value);
      string grownOnDateText = I18n.FruitTree_Growth_Summary((object) fruitTreeSubject.Stringify((object) dayOfMaturity));
      yield return (ICustomField) new GenericField(I18n.FruitTree_NextFruit(), I18n.FruitTree_NextFruit_TooYoung());
      yield return (ICustomField) new GenericField(I18n.FruitTree_Growth(), $"{grownOnDateText} ({fruitTreeSubject.GetRelativeDateStr(dayOfMaturity)})");
      if (FruitTree.IsGrowthBlocked(fruitTreeSubject.Tile, ((TerrainFeature) tree).Location))
        yield return (ICustomField) new GenericField(I18n.FruitTree_Complaints(), I18n.FruitTree_Complaints_AdjacentObjects());
      dayOfMaturity = (SDate) null;
      grownOnDateText = (string) null;
    }
    else
    {
      ItemQuality currentQuality = fruitTreeSubject.GetCurrentQuality(tree, fruitTreeSubject.Constants.FruitTreeQualityGrowthTime);
      if (currentQuality == ItemQuality.Iridium)
      {
        yield return (ICustomField) new GenericField(I18n.FruitTree_Quality(), I18n.FruitTree_Quality_Now((object) I18n.For(currentQuality)));
      }
      else
      {
        // ISSUE: reference to a compiler-generated method
        string[] array = fruitTreeSubject.GetQualitySchedule(tree, currentQuality, fruitTreeSubject.Constants.FruitTreeQualityGrowthTime).Select<KeyValuePair<ItemQuality, int>, string>(new Func<KeyValuePair<ItemQuality, int>, string>(fruitTreeSubject.\u003CGetData\u003Eb__3_0)).ToArray<string>();
        yield return (ICustomField) new GenericField(I18n.FruitTree_Quality(), string.Join(Environment.NewLine, array));
      }
    }
    FruitTreeData data = tree.GetData();
    IEnumerable<string> strings;
    if (data == null)
    {
      strings = (IEnumerable<string>) null;
    }
    else
    {
      List<Season> seasons = data.Seasons;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      strings = seasons != null ? seasons.Select<Season, string>(FruitTreeSubject.\u003C\u003EO.\u003C0\u003E__GetSeasonName ?? (FruitTreeSubject.\u003C\u003EO.\u003C0\u003E__GetSeasonName = new Func<Season, string>(I18n.GetSeasonName))) : (IEnumerable<string>) null;
    }
    IEnumerable<string> values = strings;
    if (values != null)
      yield return (ICustomField) new GenericField(I18n.FruitTree_Season(), I18n.FruitTree_Season_Summary((object) I18n.List((IEnumerable<object>) values)));
  }

  public override IEnumerable<IDebugField> GetDebugFields()
  {
    FruitTreeSubject fruitTreeSubject = this;
    FruitTree target = fruitTreeSubject.Target;
    yield return (IDebugField) new GenericDebugField("mature in", $"{target.daysUntilMature} days", pinned: true);
    yield return (IDebugField) new GenericDebugField("growth stage", ((NetFieldBase<int, NetInt>) target.growthStage).Value, pinned: true);
    yield return (IDebugField) new GenericDebugField("health", ((NetFieldBase<float, NetFloat>) target.health).Value, pinned: true);
    foreach (IDebugField debugField in fruitTreeSubject.GetDebugFieldsFrom((object) target))
      yield return debugField;
  }

  public override bool DrawPortrait(SpriteBatch spriteBatch, Vector2 position, Vector2 size)
  {
    ((TerrainFeature) this.Target).drawInMenu(spriteBatch, position, Vector2.Zero, 1f, 1f);
    return true;
  }

  private static string GetDisplayName(FruitTree tree)
  {
    FruitTreeData data = tree.GetData();
    string text = TokenParser.ParseText(data?.DisplayName, (Random) null, (TokenParserDelegate) null, (Farmer) null);
    return !string.IsNullOrWhiteSpace(data?.DisplayName) ? text : "???";
  }

  private ItemQuality GetCurrentQuality(FruitTree tree, int daysPerQuality)
  {
    int num = Math.Max(0, Math.Min(3, -((NetFieldBase<int, NetInt>) tree.daysUntilMature).Value / daysPerQuality));
    switch (num)
    {
      case 0:
        return ItemQuality.Normal;
      case 1:
        return ItemQuality.Silver;
      case 2:
        return ItemQuality.Gold;
      case 3:
        return ItemQuality.Iridium;
      default:
        throw new NotSupportedException($"Unexpected quality level {num}.");
    }
  }

  private IEnumerable<KeyValuePair<ItemQuality, int>> GetQualitySchedule(
    FruitTree tree,
    ItemQuality currentQuality,
    int daysPerQuality)
  {
    if (((NetFieldBase<int, NetInt>) tree.daysUntilMature).Value <= 0)
    {
      yield return new KeyValuePair<ItemQuality, int>(currentQuality, 0);
      int dayOffset = daysPerQuality - Math.Abs(((NetFieldBase<int, NetInt>) tree.daysUntilMature).Value % daysPerQuality);
      ItemQuality[] itemQualityArray = new ItemQuality[3]
      {
        ItemQuality.Silver,
        ItemQuality.Gold,
        ItemQuality.Iridium
      };
      for (int index = 0; index < itemQualityArray.Length; ++index)
      {
        ItemQuality key = itemQualityArray[index];
        if (currentQuality < key)
        {
          yield return new KeyValuePair<ItemQuality, int>(key, dayOffset);
          dayOffset += daysPerQuality;
        }
      }
      itemQualityArray = (ItemQuality[]) null;
    }
  }

  private bool IsInSeason(FruitTree tree, Season season)
  {
    if (((TerrainFeature) tree).Location.SeedsIgnoreSeasonsHere())
      return true;
    List<Season> seasons = tree.GetData()?.Seasons;
    if (seasons != null)
    {
      foreach (Season season1 in seasons)
      {
        if (season == season1)
          return true;
      }
    }
    return false;
  }
}
