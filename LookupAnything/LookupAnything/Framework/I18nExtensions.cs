// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.I18n
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Microsoft.Xna.Framework;
using Netcode;
using Pathoschild.Stardew.LookupAnything.Framework.Constants;
using StardewModdingAPI;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.GameData.WildTrees;
using StardewValley.Mods;
using StardewValley.Network;
using StardewValley.Pathfinding;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework;

[GeneratedCode("TextTemplatingFileGenerator", "1.0.0")]
internal static class I18n
{
  private static ITranslationHelper? Translations;

  public static string List(IEnumerable<object> values)
  {
    return string.Join<object>(I18n.Generic_ListSeparator(), values);
  }

  public static string For(WildTreeGrowthStage stage)
  {
    return Translation.op_Implicit(I18n.GetByKey("tree.stages." + (stage == 4 ? "smallTree" : stage.ToString())));
  }

  public static string For(ItemQuality quality)
  {
    return Translation.op_Implicit(I18n.GetByKey("quality." + quality.GetName()));
  }

  public static string For(FriendshipStatus status, bool wasHousemate)
  {
    return wasHousemate && status == 4 ? I18n.FriendshipStatus_KickedOut() : Translation.op_Implicit(I18n.GetByKey("friendship-status." + status.ToString().ToLower()));
  }

  public static string For(ChildAge age)
  {
    return Translation.op_Implicit(I18n.GetByKey("npc.child.age." + age.ToString().ToLower()));
  }

  public static string ForMovieTasteLabel(string taste, string name)
  {
    return Translation.op_Implicit(I18n.GetByKey("item.movie-snack-preference." + taste, (object) new
    {
      name = name
    }));
  }

  public static string GetPlural(int count, string singleText, string pluralText)
  {
    return count != 1 ? pluralText : singleText;
  }

  public static string GetSeasonName(Season season)
  {
    return Utility.getSeasonNameFromNumber((int) season);
  }

  public static IEnumerable<string> GetSeasonNames(IEnumerable<Season> seasons)
  {
    foreach (Season season in seasons)
      yield return I18n.GetSeasonName(season);
  }

  public static string? Stringify(object? value)
  {
    object obj = value;
    switch (obj)
    {
      case null:
        return (string) null;
      case NetBool netBool:
        return I18n.Stringify((object) ((NetFieldBase<bool, NetBool>) netBool).Value);
      case NetByte netByte:
        return I18n.Stringify((object) ((NetFieldBase<byte, NetByte>) netByte).Value);
      case NetColor netColor:
        return I18n.Stringify((object) ((NetFieldBase<Color, NetColor>) netColor).Value);
      case NetDancePartner netDancePartner:
        return I18n.Stringify((object) netDancePartner.Value?.displayName);
      case NetDouble netDouble:
        return I18n.Stringify((object) ((NetFieldBase<double, NetDouble>) netDouble).Value);
      case NetFloat netFloat:
        return I18n.Stringify((object) ((NetFieldBase<float, NetFloat>) netFloat).Value);
      case NetGuid netGuid:
        return I18n.Stringify((object) ((NetFieldBase<Guid, NetGuid>) netGuid).Value);
      case NetInt netInt:
        return I18n.Stringify((object) ((NetFieldBase<int, NetInt>) netInt).Value);
      case NetLocationRef netLocationRef:
        return I18n.Stringify((object) netLocationRef.Value?.NameOrUniqueName);
      case NetLong netLong:
        return I18n.Stringify((object) ((NetFieldBase<long, NetLong>) netLong).Value);
      case NetPoint netPoint:
        return I18n.Stringify((object) ((NetFieldBase<Point, NetPoint>) netPoint).Value);
      case NetPosition netPosition:
        return I18n.Stringify((object) ((NetPausableField<Vector2, NetVector2, NetVector2>) netPosition).Value);
      case NetRectangle netRectangle:
        return I18n.Stringify((object) ((NetFieldBase<Rectangle, NetRectangle>) netRectangle).Value);
      case NetString netString:
        return I18n.Stringify((object) ((NetFieldBase<string, NetString>) netString).Value);
      case NetVector2 netVector2:
        return I18n.Stringify((object) ((NetFieldBase<Vector2, NetVector2>) netVector2).Value);
      case bool flag:
        return !flag ? I18n.Generic_No() : I18n.Generic_Yes();
      case Color _:
        Color color = (Color) obj;
        return $"(r:{((Color) ref color).R} g:{((Color) ref color).G} b:{((Color) ref color).B} a:{((Color) ref color).A})";
      case SDate sdate:
        return sdate.ToLocaleString(sdate.Year != Game1.year);
      case TimeSpan timeSpan:
        System.Collections.Generic.List<string> values = new System.Collections.Generic.List<string>();
        if (timeSpan.Days > 0)
          values.Add(I18n.Generic_Days((object) timeSpan.Days));
        if (timeSpan.Hours > 0)
          values.Add(I18n.Generic_Hours((object) timeSpan.Hours));
        if (timeSpan.Minutes > 0)
          values.Add(I18n.Generic_Minutes((object) timeSpan.Minutes));
        return I18n.List((IEnumerable<object>) values);
      case Vector2 _:
        Vector2 vector2 = (Vector2) obj;
        return $"({vector2.X}, {vector2.Y})";
      case Rectangle _:
        Rectangle rectangle = (Rectangle) obj;
        return $"(x:{rectangle.X}, y:{rectangle.Y}, width:{rectangle.Width}, height:{rectangle.Height})";
      case AnimatedSprite animatedSprite:
        return $"(textureName: {((NetFieldBase<string, NetString>) animatedSprite.textureName).Value}, currentFrame:{animatedSprite.currentFrame}, loop:{animatedSprite.loop}, sourceRect:{I18n.Stringify((object) animatedSprite.sourceRect)})";
      case MarriageDialogueReference dialogueReference:
        return $"(file: {dialogueReference.DialogueFile}, key: {dialogueReference.DialogueKey}, gendered: {dialogueReference.IsGendered}, substitutions: {I18n.Stringify((object) dialogueReference.Substitutions)})";
      case ModDataDictionary modDataDictionary:
        if (!((NetDictionary<string, string, NetString, SerializableDictionary<string, string>, NetStringDictionary<string, NetString>>) modDataDictionary).Any())
        {
          source = (IEnumerable) obj;
          goto label_51;
        }
        StringBuilder stringBuilder1 = new StringBuilder();
        stringBuilder1.AppendLine();
        foreach (KeyValuePair<string, string> keyValuePair in (IEnumerable<KeyValuePair<string, string>>) ((IEnumerable<KeyValuePair<string, string>>) ((NetDictionary<string, string, NetString, SerializableDictionary<string, string>, NetStringDictionary<string, NetString>>) modDataDictionary).Pairs).OrderBy<KeyValuePair<string, string>, string>((Func<KeyValuePair<string, string>, string>) (p => p.Key)))
        {
          StringBuilder stringBuilder2 = stringBuilder1;
          StringBuilder stringBuilder3 = stringBuilder2;
          StringBuilder.AppendInterpolatedStringHandler interpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(4, 2, stringBuilder2);
          interpolatedStringHandler.AppendLiteral("- ");
          interpolatedStringHandler.AppendFormatted(keyValuePair.Key);
          interpolatedStringHandler.AppendLiteral(": ");
          interpolatedStringHandler.AppendFormatted(keyValuePair.Value);
          ref StringBuilder.AppendInterpolatedStringHandler local = ref interpolatedStringHandler;
          stringBuilder3.AppendLine(ref local);
        }
        return stringBuilder1.ToString().TrimEnd();
      case SchedulePathDescription schedulePathDescription:
        return $"{schedulePathDescription.time / 100:00}:{schedulePathDescription.time % 100:00} {schedulePathDescription.targetLocationName} ({schedulePathDescription.targetTile.X}, {schedulePathDescription.targetTile.Y}) {schedulePathDescription.facingDirection} {schedulePathDescription.endOfRouteMessage}";
      case Stats stats:
        StringBuilder stringBuilder4 = new StringBuilder();
        stringBuilder4.AppendLine();
        foreach ((string key, uint num) in (Dictionary<string, uint>) stats.Values)
        {
          StringBuilder stringBuilder5 = stringBuilder4;
          StringBuilder stringBuilder6 = stringBuilder5;
          StringBuilder.AppendInterpolatedStringHandler interpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(4, 2, stringBuilder5);
          interpolatedStringHandler.AppendLiteral("- ");
          interpolatedStringHandler.AppendFormatted(key);
          interpolatedStringHandler.AppendLiteral(": ");
          interpolatedStringHandler.AppendFormatted(I18n.Stringify((object) num));
          ref StringBuilder.AppendInterpolatedStringHandler local = ref interpolatedStringHandler;
          stringBuilder6.AppendLine(ref local);
        }
        return stringBuilder4.ToString().TrimEnd();
      case Warp warp:
        return $"([{warp.X}, {warp.Y}] to {warp.TargetName}[{warp.TargetX}, {warp.TargetY}])";
      case IEnumerable source:
label_51:
        if (!(value is string))
          return $"({I18n.List((IEnumerable<object>) source.Cast<object>().Select<object, string>((Func<object, string>) (val => I18n.Stringify(val) ?? "<null>")).ToArray<string>())})";
        break;
    }
    Type type = value.GetType();
    if (type.IsGenericType)
    {
      Type genericTypeDefinition = type.GetGenericTypeDefinition();
      if (genericTypeDefinition == typeof (NetDictionary<,,,,>))
        return I18n.Stringify(type.GetProperty("FieldDict")?.GetValue(value));
      if (genericTypeDefinition == typeof (KeyValuePair<,>))
        return $"({I18n.Stringify(type.GetProperty("Key")?.GetValue(value))}: {I18n.Stringify(type.GetProperty("Value")?.GetValue(value))})";
    }
    return value.ToString();
  }

  public static void Init(ITranslationHelper translations) => I18n.Translations = translations;

  public static string Generic_ListSeparator()
  {
    return Translation.op_Implicit(I18n.GetByKey("generic.list-separator"));
  }

  public static string Generic_LineWrapOn()
  {
    return Translation.op_Implicit(I18n.GetByKey("generic.line-wrap-on"));
  }

  public static string Generic_Now() => Translation.op_Implicit(I18n.GetByKey("generic.now"));

  public static string Generic_Tomorrow()
  {
    return Translation.op_Implicit(I18n.GetByKey("generic.tomorrow"));
  }

  public static string Generic_Yesterday()
  {
    return Translation.op_Implicit(I18n.GetByKey("generic.yesterday"));
  }

  public static string Generic_Unknown()
  {
    return Translation.op_Implicit(I18n.GetByKey("generic.unknown"));
  }

  public static string Generic_Percent(object? percent)
  {
    return Translation.op_Implicit(I18n.GetByKey("generic.percent", (object) new
    {
      percent = percent
    }));
  }

  public static string Generic_PercentChanceOf(object? percent, object? label)
  {
    return Translation.op_Implicit(I18n.GetByKey("generic.percent-chance-of", (object) new
    {
      percent = percent,
      label = label
    }));
  }

  public static string Generic_PercentRatio(object? percent, object? value, object? max)
  {
    return Translation.op_Implicit(I18n.GetByKey("generic.percent-ratio", (object) new
    {
      percent = percent,
      value = value,
      max = max
    }));
  }

  public static string Generic_Ratio(object? value, object? max)
  {
    return Translation.op_Implicit(I18n.GetByKey("generic.ratio", (object) new
    {
      value = value,
      max = max
    }));
  }

  public static string Generic_Range(object? min, object? max)
  {
    return Translation.op_Implicit(I18n.GetByKey("generic.range", (object) new
    {
      min = min,
      max = max
    }));
  }

  public static string Generic_Yes() => Translation.op_Implicit(I18n.GetByKey("generic.yes"));

  public static string Generic_No() => Translation.op_Implicit(I18n.GetByKey("generic.no"));

  public static string Generic_Seconds(object? count)
  {
    return Translation.op_Implicit(I18n.GetByKey("generic.seconds", (object) new
    {
      count = count
    }));
  }

  public static string Generic_Minutes(object? count)
  {
    return Translation.op_Implicit(I18n.GetByKey("generic.minutes", (object) new
    {
      count = count
    }));
  }

  public static string Generic_Hours(object? count)
  {
    return Translation.op_Implicit(I18n.GetByKey("generic.hours", (object) new
    {
      count = count
    }));
  }

  public static string Generic_Days(object? count)
  {
    return Translation.op_Implicit(I18n.GetByKey("generic.days", (object) new
    {
      count = count
    }));
  }

  public static string Generic_InXDays(object? count)
  {
    return Translation.op_Implicit(I18n.GetByKey("generic.in-x-days", (object) new
    {
      count = count
    }));
  }

  public static string Generic_XDaysAgo(object? count)
  {
    return Translation.op_Implicit(I18n.GetByKey("generic.x-days-ago", (object) new
    {
      count = count
    }));
  }

  public static string Generic_Price(object? price)
  {
    return Translation.op_Implicit(I18n.GetByKey("generic.price", (object) new
    {
      price = price
    }));
  }

  public static string Generic_PriceForQuality(object? price, object? quality)
  {
    return Translation.op_Implicit(I18n.GetByKey("generic.price-for-quality", (object) new
    {
      price = price,
      quality = quality
    }));
  }

  public static string Generic_PriceForStack(object? price, object? count)
  {
    return Translation.op_Implicit(I18n.GetByKey("generic.price-for-stack", (object) new
    {
      price = price,
      count = count
    }));
  }

  public static string Generic_ShowXResults(object? count)
  {
    return Translation.op_Implicit(I18n.GetByKey("generic.show-x-results", (object) new
    {
      count = count
    }));
  }

  public static string Type_Building() => Translation.op_Implicit(I18n.GetByKey("type.building"));

  public static string Type_Bush() => Translation.op_Implicit(I18n.GetByKey("type.bush"));

  public static string Type_FruitTree()
  {
    return Translation.op_Implicit(I18n.GetByKey("type.fruit-tree"));
  }

  public static string Type_Monster() => Translation.op_Implicit(I18n.GetByKey("type.monster"));

  public static string Type_Player() => Translation.op_Implicit(I18n.GetByKey("type.player"));

  public static string Type_Tree() => Translation.op_Implicit(I18n.GetByKey("type.tree"));

  public static string Type_Villager() => Translation.op_Implicit(I18n.GetByKey("type.villager"));

  public static string Type_Other() => Translation.op_Implicit(I18n.GetByKey("type.other"));

  public static string Quality_Normal() => Translation.op_Implicit(I18n.GetByKey("quality.normal"));

  public static string Quality_Silver() => Translation.op_Implicit(I18n.GetByKey("quality.silver"));

  public static string Quality_Gold() => Translation.op_Implicit(I18n.GetByKey("quality.gold"));

  public static string Quality_Iridium()
  {
    return Translation.op_Implicit(I18n.GetByKey("quality.iridium"));
  }

  public static string Item_WildSeeds()
  {
    return Translation.op_Implicit(I18n.GetByKey("item.wild-seeds"));
  }

  public static string FriendshipStatus_Friendly()
  {
    return Translation.op_Implicit(I18n.GetByKey("friendship-status.friendly"));
  }

  public static string FriendshipStatus_Dating()
  {
    return Translation.op_Implicit(I18n.GetByKey("friendship-status.dating"));
  }

  public static string FriendshipStatus_Engaged()
  {
    return Translation.op_Implicit(I18n.GetByKey("friendship-status.engaged"));
  }

  public static string FriendshipStatus_Married()
  {
    return Translation.op_Implicit(I18n.GetByKey("friendship-status.married"));
  }

  public static string FriendshipStatus_Divorced()
  {
    return Translation.op_Implicit(I18n.GetByKey("friendship-status.divorced"));
  }

  public static string FriendshipStatus_KickedOut()
  {
    return Translation.op_Implicit(I18n.GetByKey("friendship-status.kicked-out"));
  }

  public static string AddedByMod() => Translation.op_Implicit(I18n.GetByKey("added-by-mod"));

  public static string AddedByMod_Summary(object? modName)
  {
    return Translation.op_Implicit(I18n.GetByKey("added-by-mod.summary", (object) new
    {
      modName = modName
    }));
  }

  public static string BundleArea_Pantry()
  {
    return Translation.op_Implicit(I18n.GetByKey("bundle-area.pantry"));
  }

  public static string BundleArea_CraftsRoom()
  {
    return Translation.op_Implicit(I18n.GetByKey("bundle-area.crafts-room"));
  }

  public static string BundleArea_FishTank()
  {
    return Translation.op_Implicit(I18n.GetByKey("bundle-area.fish-tank"));
  }

  public static string BundleArea_BoilerRoom()
  {
    return Translation.op_Implicit(I18n.GetByKey("bundle-area.boiler-room"));
  }

  public static string BundleArea_Vault()
  {
    return Translation.op_Implicit(I18n.GetByKey("bundle-area.vault"));
  }

  public static string BundleArea_BulletinBoard()
  {
    return Translation.op_Implicit(I18n.GetByKey("bundle-area.bulletin-board"));
  }

  public static string BundleArea_AbandonedJojaMart()
  {
    return Translation.op_Implicit(I18n.GetByKey("bundle-area.abandoned-joja-mart"));
  }

  public static string Shop_AdventureGuild()
  {
    return Translation.op_Implicit(I18n.GetByKey("shop.adventure-guild"));
  }

  public static string Shop_Clint() => Translation.op_Implicit(I18n.GetByKey("shop.clint"));

  public static string Shop_Marnie() => Translation.op_Implicit(I18n.GetByKey("shop.marnie"));

  public static string Shop_Pierre() => Translation.op_Implicit(I18n.GetByKey("shop.pierre"));

  public static string Shop_Robin() => Translation.op_Implicit(I18n.GetByKey("shop.robin"));

  public static string Shop_Volcano() => Translation.op_Implicit(I18n.GetByKey("shop.volcano"));

  public static string Shop_Willy() => Translation.op_Implicit(I18n.GetByKey("shop.willy"));

  public static string RecipeType_Cooking()
  {
    return Translation.op_Implicit(I18n.GetByKey("recipe-type.cooking"));
  }

  public static string RecipeType_Crafting()
  {
    return Translation.op_Implicit(I18n.GetByKey("recipe-type.crafting"));
  }

  public static string RecipeType_Tailoring()
  {
    return Translation.op_Implicit(I18n.GetByKey("recipe-type.tailoring"));
  }

  public static string Data_Npc_Pet_Description()
  {
    return Translation.op_Implicit(I18n.GetByKey("data.npc.pet.description"));
  }

  public static string Data_Npc_Horse_Description()
  {
    return Translation.op_Implicit(I18n.GetByKey("data.npc.horse.description"));
  }

  public static string Data_Npc_Junimo_Description()
  {
    return Translation.op_Implicit(I18n.GetByKey("data.npc.junimo.description"));
  }

  public static string Data_Npc_TrashBear_Description()
  {
    return Translation.op_Implicit(I18n.GetByKey("data.npc.trash-bear.description"));
  }

  public static string Data_Type_Container()
  {
    return Translation.op_Implicit(I18n.GetByKey("data.type.container"));
  }

  public static string Data_Item_EggIncubator_Description()
  {
    return Translation.op_Implicit(I18n.GetByKey("data.item.egg-incubator.description"));
  }

  public static string Data_Item_Barrel_Name()
  {
    return Translation.op_Implicit(I18n.GetByKey("data.item.barrel.name"));
  }

  public static string Data_Item_Barrel_Description()
  {
    return Translation.op_Implicit(I18n.GetByKey("data.item.barrel.description"));
  }

  public static string Data_Item_Box_Name()
  {
    return Translation.op_Implicit(I18n.GetByKey("data.item.box.name"));
  }

  public static string Data_Item_Box_Description()
  {
    return Translation.op_Implicit(I18n.GetByKey("data.item.box.description"));
  }

  public static string Location_Beach_EastPier(object? locationName)
  {
    return Translation.op_Implicit(I18n.GetByKey("location.beach.east-pier", (object) new
    {
      locationName = locationName
    }));
  }

  public static string Location_BugLand()
  {
    return Translation.op_Implicit(I18n.GetByKey("location.bugLand"));
  }

  public static string Location_Desert_TopPond(object? locationName)
  {
    return Translation.op_Implicit(I18n.GetByKey("location.desert.topPond", (object) new
    {
      locationName = locationName
    }));
  }

  public static string Location_FarmCave()
  {
    return Translation.op_Implicit(I18n.GetByKey("location.farmCave"));
  }

  public static string Location_FarmHouse()
  {
    return Translation.op_Implicit(I18n.GetByKey("location.farmHouse"));
  }

  public static string Location_Forest_IslandTip(object? locationName)
  {
    return Translation.op_Implicit(I18n.GetByKey("location.forest.island-tip", (object) new
    {
      locationName = locationName
    }));
  }

  public static string Location_Forest_Lake(object? locationName)
  {
    return Translation.op_Implicit(I18n.GetByKey("location.forest.lake", (object) new
    {
      locationName = locationName
    }));
  }

  public static string Location_Forest_River(object? locationName)
  {
    return Translation.op_Implicit(I18n.GetByKey("location.forest.river", (object) new
    {
      locationName = locationName
    }));
  }

  public static string Location_HarveyRoom()
  {
    return Translation.op_Implicit(I18n.GetByKey("location.HarveyRoom"));
  }

  public static string Location_LeoTreeHouse()
  {
    return Translation.op_Implicit(I18n.GetByKey("location.LeoTreeHouse"));
  }

  public static string Location_SandyHouse()
  {
    return Translation.op_Implicit(I18n.GetByKey("location.SandyHouse"));
  }

  public static string Location_SebastianRoom()
  {
    return Translation.op_Implicit(I18n.GetByKey("location.SebastianRoom"));
  }

  public static string Location_Submarine()
  {
    return Translation.op_Implicit(I18n.GetByKey("location.submarine"));
  }

  public static string Location_Town_NorthmostBridge(object? locationName)
  {
    return Translation.op_Implicit(I18n.GetByKey("location.town.northmost-bridge", (object) new
    {
      locationName = locationName
    }));
  }

  public static string Location_UndergroundMine()
  {
    return Translation.op_Implicit(I18n.GetByKey("location.undergroundMine"));
  }

  public static string Location_UndergroundMine_Level(object? level)
  {
    return Translation.op_Implicit(I18n.GetByKey("location.undergroundMine.level", (object) new
    {
      level = level
    }));
  }

  public static string Location_WitchSwamp()
  {
    return Translation.op_Implicit(I18n.GetByKey("location.witchSwamp"));
  }

  public static string Location_Caldera()
  {
    return Translation.op_Implicit(I18n.GetByKey("location.caldera"));
  }

  public static string Location_IslandEast()
  {
    return Translation.op_Implicit(I18n.GetByKey("location.islandEast"));
  }

  public static string Location_IslandNorth()
  {
    return Translation.op_Implicit(I18n.GetByKey("location.islandNorth"));
  }

  public static string Location_IslandSouth()
  {
    return Translation.op_Implicit(I18n.GetByKey("location.islandSouth"));
  }

  public static string Location_IslandSouthEast()
  {
    return Translation.op_Implicit(I18n.GetByKey("location.islandSouthEast"));
  }

  public static string Location_IslandSouthEastCave()
  {
    return Translation.op_Implicit(I18n.GetByKey("location.islandSouthEastCave"));
  }

  public static string Location_IslandWest()
  {
    return Translation.op_Implicit(I18n.GetByKey("location.islandWest"));
  }

  public static string Location_IslandWest_Freshwater()
  {
    return Translation.op_Implicit(I18n.GetByKey("location.islandWest.freshwater"));
  }

  public static string Location_IslandWest_Ocean()
  {
    return Translation.op_Implicit(I18n.GetByKey("location.islandWest.ocean"));
  }

  public static string Location_FishArea(object? locationName, object? areaName)
  {
    return Translation.op_Implicit(I18n.GetByKey("location.fish-area", (object) new
    {
      locationName = locationName,
      areaName = areaName
    }));
  }

  public static string Location_UnknownFishArea(object? locationName, object? id)
  {
    return Translation.op_Implicit(I18n.GetByKey("location.unknown-fish-area", (object) new
    {
      locationName = locationName,
      id = id
    }));
  }

  public static string ConditionsSummary(object? conditions)
  {
    return Translation.op_Implicit(I18n.GetByKey("conditions-summary", (object) new
    {
      conditions = conditions
    }));
  }

  public static string ConditionOrContextTag_Negate(object? value)
  {
    return Translation.op_Implicit(I18n.GetByKey("condition-or-context-tag.negate", (object) new
    {
      value = value
    }));
  }

  public static string Condition_DayOfMonth(object? days)
  {
    return Translation.op_Implicit(I18n.GetByKey("condition.day-of-month", (object) new
    {
      days = days
    }));
  }

  public static string Condition_DayOfMonth_Even()
  {
    return Translation.op_Implicit(I18n.GetByKey("condition.day-of-month.even"));
  }

  public static string Condition_DayOfMonth_Odd()
  {
    return Translation.op_Implicit(I18n.GetByKey("condition.day-of-month.odd"));
  }

  public static string Condition_ItemType_Input()
  {
    return Translation.op_Implicit(I18n.GetByKey("condition.item-type.input"));
  }

  public static string Condition_ItemType_Target()
  {
    return Translation.op_Implicit(I18n.GetByKey("condition.item-type.target"));
  }

  public static string Condition_RawContextTag(object? tag)
  {
    return Translation.op_Implicit(I18n.GetByKey("condition.raw-context-tag", (object) new
    {
      tag = tag
    }));
  }

  public static string Condition_RawCondition(object? tag)
  {
    return Translation.op_Implicit(I18n.GetByKey("condition.raw-condition", (object) new
    {
      tag = tag
    }));
  }

  public static string Condition_ItemContextTag(object? item, object? tags)
  {
    return Translation.op_Implicit(I18n.GetByKey("condition.item-context-tag", (object) new
    {
      item = item,
      tags = tags
    }));
  }

  public static string Condition_ItemContextTags(object? item, object? tags)
  {
    return Translation.op_Implicit(I18n.GetByKey("condition.item-context-tags", (object) new
    {
      item = item,
      tags = tags
    }));
  }

  public static string Condition_ItemContextTag_Value(object? tag)
  {
    return Translation.op_Implicit(I18n.GetByKey("condition.item-context-tag.value", (object) new
    {
      tag = tag
    }));
  }

  public static string Condition_ItemEdibility_Edible(object? item)
  {
    return Translation.op_Implicit(I18n.GetByKey("condition.item-edibility.edible", (object) new
    {
      item = item
    }));
  }

  public static string Condition_ItemEdibility_Min(object? item, object? min)
  {
    return Translation.op_Implicit(I18n.GetByKey("condition.item-edibility.min", (object) new
    {
      item = item,
      min = min
    }));
  }

  public static string Condition_ItemEdibility_Range(object? item, object? min, object? max)
  {
    return Translation.op_Implicit(I18n.GetByKey("condition.item-edibility.range", (object) new
    {
      item = item,
      min = min,
      max = max
    }));
  }

  public static string ContextTag_Bone()
  {
    return Translation.op_Implicit(I18n.GetByKey("context-tag.bone"));
  }

  public static string ContextTag_Egg()
  {
    return Translation.op_Implicit(I18n.GetByKey("context-tag.egg"));
  }

  public static string ContextTag_LargeEgg()
  {
    return Translation.op_Implicit(I18n.GetByKey("context-tag.large-egg"));
  }

  public static string ContextTag_PreservedItem(object? name)
  {
    return Translation.op_Implicit(I18n.GetByKey("context-tag.preserved-item", (object) new
    {
      name = name
    }));
  }

  public static string Animal_Love() => Translation.op_Implicit(I18n.GetByKey("animal.love"));

  public static string Animal_Happiness()
  {
    return Translation.op_Implicit(I18n.GetByKey("animal.happiness"));
  }

  public static string Animal_Mood() => Translation.op_Implicit(I18n.GetByKey("animal.mood"));

  public static string Animal_Complaints()
  {
    return Translation.op_Implicit(I18n.GetByKey("animal.complaints"));
  }

  public static string Animal_ProduceReady()
  {
    return Translation.op_Implicit(I18n.GetByKey("animal.produce-ready"));
  }

  public static string Animal_Growth() => Translation.op_Implicit(I18n.GetByKey("animal.growth"));

  public static string Animal_SellsFor()
  {
    return Translation.op_Implicit(I18n.GetByKey("animal.sells-for"));
  }

  public static string Animal_Complaints_Hungry()
  {
    return Translation.op_Implicit(I18n.GetByKey("animal.complaints.hungry"));
  }

  public static string Animal_Complaints_LeftOut()
  {
    return Translation.op_Implicit(I18n.GetByKey("animal.complaints.left-out"));
  }

  public static string Animal_Complaints_NewHome()
  {
    return Translation.op_Implicit(I18n.GetByKey("animal.complaints.new-home"));
  }

  public static string Animal_Complaints_NoHeater()
  {
    return Translation.op_Implicit(I18n.GetByKey("animal.complaints.no-heater"));
  }

  public static string Animal_Complaints_NotPetted()
  {
    return Translation.op_Implicit(I18n.GetByKey("animal.complaints.not-petted"));
  }

  public static string Animal_Complaints_WildAnimalAttack()
  {
    return Translation.op_Implicit(I18n.GetByKey("animal.complaints.wild-animal-attack"));
  }

  public static string Building_Animals()
  {
    return Translation.op_Implicit(I18n.GetByKey("building.animals"));
  }

  public static string Building_Construction()
  {
    return Translation.op_Implicit(I18n.GetByKey("building.construction"));
  }

  public static string Building_ConstructionCosts()
  {
    return Translation.op_Implicit(I18n.GetByKey("building.construction-costs"));
  }

  public static string Building_FeedTrough()
  {
    return Translation.op_Implicit(I18n.GetByKey("building.feed-trough"));
  }

  public static string Building_Horse() => Translation.op_Implicit(I18n.GetByKey("building.horse"));

  public static string Building_HorseLocation()
  {
    return Translation.op_Implicit(I18n.GetByKey("building.horse-location"));
  }

  public static string Building_Owner() => Translation.op_Implicit(I18n.GetByKey("building.owner"));

  public static string Building_Slimes()
  {
    return Translation.op_Implicit(I18n.GetByKey("building.slimes"));
  }

  public static string Building_StoredHay()
  {
    return Translation.op_Implicit(I18n.GetByKey("building.stored-hay"));
  }

  public static string Building_Upgrades()
  {
    return Translation.op_Implicit(I18n.GetByKey("building.upgrades"));
  }

  public static string Building_WaterTrough()
  {
    return Translation.op_Implicit(I18n.GetByKey("building.water-trough"));
  }

  public static string Building_Animals_Summary(object? count, object? max)
  {
    return Translation.op_Implicit(I18n.GetByKey("building.animals.summary", (object) new
    {
      count = count,
      max = max
    }));
  }

  public static string Building_Construction_Summary(object? date)
  {
    return Translation.op_Implicit(I18n.GetByKey("building.construction.summary", (object) new
    {
      date = date
    }));
  }

  public static string Building_FeedTrough_Automated()
  {
    return Translation.op_Implicit(I18n.GetByKey("building.feed-trough.automated"));
  }

  public static string Building_FeedTrough_Summary(object? filled, object? max)
  {
    return Translation.op_Implicit(I18n.GetByKey("building.feed-trough.summary", (object) new
    {
      filled = filled,
      max = max
    }));
  }

  public static string Building_HorseLocation_Summary(object? location, object? x, object? y)
  {
    return Translation.op_Implicit(I18n.GetByKey("building.horse-location.summary", (object) new
    {
      location = location,
      x = x,
      y = y
    }));
  }

  public static string Building_JunimoHarvestingEnabled()
  {
    return Translation.op_Implicit(I18n.GetByKey("building.junimo-harvesting-enabled"));
  }

  public static string Building_Owner_None()
  {
    return Translation.op_Implicit(I18n.GetByKey("building.owner.none"));
  }

  public static string Building_OutputProcessing()
  {
    return Translation.op_Implicit(I18n.GetByKey("building.output-processing"));
  }

  public static string Building_OutputReady()
  {
    return Translation.op_Implicit(I18n.GetByKey("building.output-ready"));
  }

  public static string Building_Slimes_Summary(object? count, object? max)
  {
    return Translation.op_Implicit(I18n.GetByKey("building.slimes.summary", (object) new
    {
      count = count,
      max = max
    }));
  }

  public static string Building_StoredHay_Summary(
    object? hayCount,
    object? maxHayInLocation,
    object? maxHayInBuilding)
  {
    return Translation.op_Implicit(I18n.GetByKey("building.stored-hay.summary", (object) new
    {
      hayCount = hayCount,
      maxHayInLocation = maxHayInLocation,
      maxHayInBuilding = maxHayInBuilding
    }));
  }

  public static string Building_Upgrades_Barn_0()
  {
    return Translation.op_Implicit(I18n.GetByKey("building.upgrades.barn.0"));
  }

  public static string Building_Upgrades_Barn_1()
  {
    return Translation.op_Implicit(I18n.GetByKey("building.upgrades.barn.1"));
  }

  public static string Building_Upgrades_Barn_2()
  {
    return Translation.op_Implicit(I18n.GetByKey("building.upgrades.barn.2"));
  }

  public static string Building_Upgrades_Cabin_0()
  {
    return Translation.op_Implicit(I18n.GetByKey("building.upgrades.cabin.0"));
  }

  public static string Building_Upgrades_Cabin_1()
  {
    return Translation.op_Implicit(I18n.GetByKey("building.upgrades.cabin.1"));
  }

  public static string Building_Upgrades_Cabin_2()
  {
    return Translation.op_Implicit(I18n.GetByKey("building.upgrades.cabin.2"));
  }

  public static string Building_Upgrades_Coop_0()
  {
    return Translation.op_Implicit(I18n.GetByKey("building.upgrades.coop.0"));
  }

  public static string Building_Upgrades_Coop_1()
  {
    return Translation.op_Implicit(I18n.GetByKey("building.upgrades.coop.1"));
  }

  public static string Building_Upgrades_Coop_2()
  {
    return Translation.op_Implicit(I18n.GetByKey("building.upgrades.coop.2"));
  }

  public static string Building_WaterTrough_Summary(object? filled, object? max)
  {
    return Translation.op_Implicit(I18n.GetByKey("building.water-trough.summary", (object) new
    {
      filled = filled,
      max = max
    }));
  }

  public static string Building_FishPond_Population()
  {
    return Translation.op_Implicit(I18n.GetByKey("building.fish-pond.population"));
  }

  public static string Building_FishPond_Drops()
  {
    return Translation.op_Implicit(I18n.GetByKey("building.fish-pond.drops"));
  }

  public static string Building_FishPond_Quests()
  {
    return Translation.op_Implicit(I18n.GetByKey("building.fish-pond.quests"));
  }

  public static string Building_FishPond_Population_Empty()
  {
    return Translation.op_Implicit(I18n.GetByKey("building.fish-pond.population.empty"));
  }

  public static string Building_FishPond_Population_NextSpawn(object? relativeDate)
  {
    return Translation.op_Implicit(I18n.GetByKey("building.fish-pond.population.next-spawn", (object) new
    {
      relativeDate = relativeDate
    }));
  }

  public static string Building_FishPond_Drops_Preface(object? chance)
  {
    return Translation.op_Implicit(I18n.GetByKey("building.fish-pond.drops.preface", (object) new
    {
      chance = chance
    }));
  }

  public static string Building_FishPond_Drops_MinFish(object? count)
  {
    return Translation.op_Implicit(I18n.GetByKey("building.fish-pond.drops.min-fish", (object) new
    {
      count = count
    }));
  }

  public static string Building_FishPond_Quests_Done(object? count)
  {
    return Translation.op_Implicit(I18n.GetByKey("building.fish-pond.quests.done", (object) new
    {
      count = count
    }));
  }

  public static string Building_FishPond_Quests_IncompleteOne(object? count, object? itemName)
  {
    return Translation.op_Implicit(I18n.GetByKey("building.fish-pond.quests.incomplete-one", (object) new
    {
      count = count,
      itemName = itemName
    }));
  }

  public static string Building_FishPond_Quests_IncompleteRandom(object? count, object? itemList)
  {
    return Translation.op_Implicit(I18n.GetByKey("building.fish-pond.quests.incomplete-random", (object) new
    {
      count = count,
      itemList = itemList
    }));
  }

  public static string Building_FishPond_Quests_Available(object? relativeDate)
  {
    return Translation.op_Implicit(I18n.GetByKey("building.fish-pond.quests.available", (object) new
    {
      relativeDate = relativeDate
    }));
  }

  public static string Bush_Name_Berry()
  {
    return Translation.op_Implicit(I18n.GetByKey("bush.name.berry"));
  }

  public static string Bush_Name_Plain()
  {
    return Translation.op_Implicit(I18n.GetByKey("bush.name.plain"));
  }

  public static string Bush_Name_Tea() => Translation.op_Implicit(I18n.GetByKey("bush.name.tea"));

  public static string Bush_Description_Berry()
  {
    return Translation.op_Implicit(I18n.GetByKey("bush.description.berry"));
  }

  public static string Bush_Description_Plain()
  {
    return Translation.op_Implicit(I18n.GetByKey("bush.description.plain"));
  }

  public static string Bush_Description_Tea()
  {
    return Translation.op_Implicit(I18n.GetByKey("bush.description.Tea"));
  }

  public static string Bush_DatePlanted()
  {
    return Translation.op_Implicit(I18n.GetByKey("bush.date-planted"));
  }

  public static string Bush_Growth() => Translation.op_Implicit(I18n.GetByKey("bush.growth"));

  public static string Bush_NextHarvest()
  {
    return Translation.op_Implicit(I18n.GetByKey("bush.next-harvest"));
  }

  public static string Bush_Growth_Summary(object? date)
  {
    return Translation.op_Implicit(I18n.GetByKey("bush.growth.summary", (object) new
    {
      date = date
    }));
  }

  public static string Bush_Schedule_Berry()
  {
    return Translation.op_Implicit(I18n.GetByKey("bush.schedule.berry"));
  }

  public static string Bush_Schedule_Tea()
  {
    return Translation.op_Implicit(I18n.GetByKey("bush.schedule.tea"));
  }

  public static string FruitTree_Complaints()
  {
    return Translation.op_Implicit(I18n.GetByKey("fruit-tree.complaints"));
  }

  public static string FruitTree_Name(object? fruitName)
  {
    return Translation.op_Implicit(I18n.GetByKey("fruit-tree.name", (object) new
    {
      fruitName = fruitName
    }));
  }

  public static string FruitTree_Growth()
  {
    return Translation.op_Implicit(I18n.GetByKey("fruit-tree.growth"));
  }

  public static string FruitTree_NextFruit()
  {
    return Translation.op_Implicit(I18n.GetByKey("fruit-tree.next-fruit"));
  }

  public static string FruitTree_Season()
  {
    return Translation.op_Implicit(I18n.GetByKey("fruit-tree.season"));
  }

  public static string FruitTree_Quality()
  {
    return Translation.op_Implicit(I18n.GetByKey("fruit-tree.quality"));
  }

  public static string FruitTree_Complaints_AdjacentObjects()
  {
    return Translation.op_Implicit(I18n.GetByKey("fruit-tree.complaints.adjacent-objects"));
  }

  public static string FruitTree_Growth_Summary(object? date)
  {
    return Translation.op_Implicit(I18n.GetByKey("fruit-tree.growth.summary", (object) new
    {
      date = date
    }));
  }

  public static string FruitTree_NextFruit_MaxFruit()
  {
    return Translation.op_Implicit(I18n.GetByKey("fruit-tree.next-fruit.max-fruit"));
  }

  public static string FruitTree_NextFruit_OutOfSeason()
  {
    return Translation.op_Implicit(I18n.GetByKey("fruit-tree.next-fruit.out-of-season"));
  }

  public static string FruitTree_NextFruit_StruckByLightning(object? count)
  {
    return Translation.op_Implicit(I18n.GetByKey("fruit-tree.next-fruit.struck-by-lightning", (object) new
    {
      count = count
    }));
  }

  public static string FruitTree_NextFruit_TooYoung()
  {
    return Translation.op_Implicit(I18n.GetByKey("fruit-tree.next-fruit.too-young"));
  }

  public static string FruitTree_Quality_Now(object? quality)
  {
    return Translation.op_Implicit(I18n.GetByKey("fruit-tree.quality.now", (object) new
    {
      quality = quality
    }));
  }

  public static string FruitTree_Quality_OnDate(object? quality, object? date)
  {
    return Translation.op_Implicit(I18n.GetByKey("fruit-tree.quality.on-date", (object) new
    {
      quality = quality,
      date = date
    }));
  }

  public static string FruitTree_Quality_OnDateNextYear(object? quality, object? date)
  {
    return Translation.op_Implicit(I18n.GetByKey("fruit-tree.quality.on-date-next-year", (object) new
    {
      quality = quality,
      date = date
    }));
  }

  public static string FruitTree_Season_Summary(object? season)
  {
    return Translation.op_Implicit(I18n.GetByKey("fruit-tree.season.summary", (object) new
    {
      season = season
    }));
  }

  public static string Crop_Summary() => Translation.op_Implicit(I18n.GetByKey("crop.summary"));

  public static string Crop_Harvest() => Translation.op_Implicit(I18n.GetByKey("crop.harvest"));

  public static string Crop_Fertilized()
  {
    return Translation.op_Implicit(I18n.GetByKey("crop.fertilized"));
  }

  public static string Crop_Watered() => Translation.op_Implicit(I18n.GetByKey("crop.watered"));

  public static string Crop_Summary_Dead()
  {
    return Translation.op_Implicit(I18n.GetByKey("crop.summary.dead"));
  }

  public static string Crop_Summary_DropsX(object? count)
  {
    return Translation.op_Implicit(I18n.GetByKey("crop.summary.drops-X", (object) new
    {
      count = count
    }));
  }

  public static string Crop_Summary_DropsXToY(object? min, object? max, object? percent)
  {
    return Translation.op_Implicit(I18n.GetByKey("crop.summary.drops-X-to-Y", (object) new
    {
      min = min,
      max = max,
      percent = percent
    }));
  }

  public static string Crop_Summary_HarvestOnce(object? daysToFirstHarvest)
  {
    return Translation.op_Implicit(I18n.GetByKey("crop.summary.harvest-once", (object) new
    {
      daysToFirstHarvest = daysToFirstHarvest
    }));
  }

  public static string Crop_Summary_HarvestMulti(
    object? daysToFirstHarvest,
    object? daysToNextHarvests)
  {
    return Translation.op_Implicit(I18n.GetByKey("crop.summary.harvest-multi", (object) new
    {
      daysToFirstHarvest = daysToFirstHarvest,
      daysToNextHarvests = daysToNextHarvests
    }));
  }

  public static string Crop_Summary_FarmingXp(object? amount)
  {
    return Translation.op_Implicit(I18n.GetByKey("crop.summary.farming-xp", (object) new
    {
      amount = amount
    }));
  }

  public static string Crop_Summary_ForagingXp(object? amount)
  {
    return Translation.op_Implicit(I18n.GetByKey("crop.summary.foraging-xp", (object) new
    {
      amount = amount
    }));
  }

  public static string Crop_Summary_Seasons(object? seasons)
  {
    return Translation.op_Implicit(I18n.GetByKey("crop.summary.seasons", (object) new
    {
      seasons = seasons
    }));
  }

  public static string Crop_Summary_SellsFor(object? price)
  {
    return Translation.op_Implicit(I18n.GetByKey("crop.summary.sells-for", (object) new
    {
      price = price
    }));
  }

  public static string Crop_Harvest_TooLate(object? date)
  {
    return Translation.op_Implicit(I18n.GetByKey("crop.harvest.too-late", (object) new
    {
      date = date
    }));
  }

  public static string Item_Contents() => Translation.op_Implicit(I18n.GetByKey("item.contents"));

  public static string Item_LovesThis()
  {
    return Translation.op_Implicit(I18n.GetByKey("item.loves-this"));
  }

  public static string Item_LikesThis()
  {
    return Translation.op_Implicit(I18n.GetByKey("item.likes-this"));
  }

  public static string Item_NeutralAboutThis()
  {
    return Translation.op_Implicit(I18n.GetByKey("item.neutral-about-this"));
  }

  public static string Item_DislikesThis()
  {
    return Translation.op_Implicit(I18n.GetByKey("item.dislikes-this"));
  }

  public static string Item_HatesThis()
  {
    return Translation.op_Implicit(I18n.GetByKey("item.hates-this"));
  }

  public static string Item_NeededFor()
  {
    return Translation.op_Implicit(I18n.GetByKey("item.needed-for"));
  }

  public static string Item_NumberOwned()
  {
    return Translation.op_Implicit(I18n.GetByKey("item.number-owned"));
  }

  public static string Item_NumberCooked()
  {
    return Translation.op_Implicit(I18n.GetByKey("item.number-cooked"));
  }

  public static string Item_NumberCrafted()
  {
    return Translation.op_Implicit(I18n.GetByKey("item.number-crafted"));
  }

  public static string Item_Recipes() => Translation.op_Implicit(I18n.GetByKey("item.recipes"));

  public static string Item_SeeAlso() => Translation.op_Implicit(I18n.GetByKey("item.see-also"));

  public static string Item_SellsFor() => Translation.op_Implicit(I18n.GetByKey("item.sells-for"));

  public static string Item_SellsTo() => Translation.op_Implicit(I18n.GetByKey("item.sells-to"));

  public static string Item_CanBeDyed()
  {
    return Translation.op_Implicit(I18n.GetByKey("item.can-be-dyed"));
  }

  public static string Item_ProducesDye()
  {
    return Translation.op_Implicit(I18n.GetByKey("item.produces-dye"));
  }

  public static string Item_Contents_Placed(object? name)
  {
    return Translation.op_Implicit(I18n.GetByKey("item.contents.placed", (object) new
    {
      name = name
    }));
  }

  public static string Item_Contents_Ready(object? name)
  {
    return Translation.op_Implicit(I18n.GetByKey("item.contents.ready", (object) new
    {
      name = name
    }));
  }

  public static string Item_Contents_Partial(object? name, object? time)
  {
    return Translation.op_Implicit(I18n.GetByKey("item.contents.partial", (object) new
    {
      name = name,
      time = time
    }));
  }

  public static string Item_UndiscoveredGiftTaste(object? count)
  {
    return Translation.op_Implicit(I18n.GetByKey("item.undiscovered-gift-taste", (object) new
    {
      count = count
    }));
  }

  public static string Item_UndiscoveredGiftTasteAppended(object? count)
  {
    return Translation.op_Implicit(I18n.GetByKey("item.undiscovered-gift-taste-appended", (object) new
    {
      count = count
    }));
  }

  public static string Item_NeededFor_CommunityCenter(object? bundles)
  {
    return Translation.op_Implicit(I18n.GetByKey("item.needed-for.community-center", (object) new
    {
      bundles = bundles
    }));
  }

  public static string Item_NeededFor_FullShipment()
  {
    return Translation.op_Implicit(I18n.GetByKey("item.needed-for.full-shipment"));
  }

  public static string Item_NeededFor_Monoculture(object? count)
  {
    return Translation.op_Implicit(I18n.GetByKey("item.needed-for.monoculture", (object) new
    {
      count = count
    }));
  }

  public static string Item_NeededFor_Polyculture(object? count)
  {
    return Translation.op_Implicit(I18n.GetByKey("item.needed-for.polyculture", (object) new
    {
      count = count
    }));
  }

  public static string Item_NeededFor_FullCollection()
  {
    return Translation.op_Implicit(I18n.GetByKey("item.needed-for.full-collection"));
  }

  public static string Item_NeededFor_GourmetChef(object? recipes)
  {
    return Translation.op_Implicit(I18n.GetByKey("item.needed-for.gourmet-chef", (object) new
    {
      recipes = recipes
    }));
  }

  public static string Item_NeededFor_CraftMaster(object? recipes)
  {
    return Translation.op_Implicit(I18n.GetByKey("item.needed-for.craft-master", (object) new
    {
      recipes = recipes
    }));
  }

  public static string Item_NeededFor_Quests(object? quests)
  {
    return Translation.op_Implicit(I18n.GetByKey("item.needed-for.quests", (object) new
    {
      quests = quests
    }));
  }

  public static string Item_UnknownRecipes(object? count)
  {
    return Translation.op_Implicit(I18n.GetByKey("item.unknown-recipes", (object) new
    {
      count = count
    }));
  }

  public static string Item_SellsTo_ShippingBox()
  {
    return Translation.op_Implicit(I18n.GetByKey("item.sells-to.shipping-box"));
  }

  public static string Item_RecipesForIngredient_Entry(object? name, object? count)
  {
    return Translation.op_Implicit(I18n.GetByKey("item.recipes-for-ingredient.entry", (object) new
    {
      name = name,
      count = count
    }));
  }

  public static string Item_RecipesForMachine_MultipleItems(object? name, object? count)
  {
    return Translation.op_Implicit(I18n.GetByKey("item.recipes-for-machine.multiple-items", (object) new
    {
      name = name,
      count = count
    }));
  }

  public static string Item_RecipesForMachine_SameAsInput()
  {
    return Translation.op_Implicit(I18n.GetByKey("item.recipes-for-machine.same-as-input"));
  }

  public static string Item_RecipesForMachine_TooComplex()
  {
    return Translation.op_Implicit(I18n.GetByKey("item.recipes-for-machine.too-complex"));
  }

  public static string Item_NumberOwned_Summary(object? count)
  {
    return Translation.op_Implicit(I18n.GetByKey("item.number-owned.summary", (object) new
    {
      count = count
    }));
  }

  public static string Item_NumberOwnedFlavored_Summary(
    object? count,
    object? name,
    object? baseCount,
    object? baseName)
  {
    return Translation.op_Implicit(I18n.GetByKey("item.number-owned-flavored.summary", (object) new
    {
      count = count,
      name = name,
      baseCount = baseCount,
      baseName = baseName
    }));
  }

  public static string Item_NumberCrafted_Summary(object? count)
  {
    return Translation.op_Implicit(I18n.GetByKey("item.number-crafted.summary", (object) new
    {
      count = count
    }));
  }

  public static string Item_CaskContents()
  {
    return Translation.op_Implicit(I18n.GetByKey("item.cask-contents"));
  }

  public static string Item_CaskSchedule()
  {
    return Translation.op_Implicit(I18n.GetByKey("item.cask-schedule"));
  }

  public static string Item_CaskSchedule_Now(object? quality)
  {
    return Translation.op_Implicit(I18n.GetByKey("item.cask-schedule.now", (object) new
    {
      quality = quality
    }));
  }

  public static string Item_CaskSchedule_NowPartial(object? quality)
  {
    return Translation.op_Implicit(I18n.GetByKey("item.cask-schedule.now-partial", (object) new
    {
      quality = quality
    }));
  }

  public static string Item_CaskSchedule_Tomorrow(object? quality)
  {
    return Translation.op_Implicit(I18n.GetByKey("item.cask-schedule.tomorrow", (object) new
    {
      quality = quality
    }));
  }

  public static string Item_CaskSchedule_InXDays(object? quality, object? count, object? date)
  {
    return Translation.op_Implicit(I18n.GetByKey("item.cask-schedule.in-x-days", (object) new
    {
      quality = quality,
      count = count,
      date = date
    }));
  }

  public static string Item_CrabpotBait()
  {
    return Translation.op_Implicit(I18n.GetByKey("item.crabpot-bait"));
  }

  public static string Item_CrabpotBaitNeeded()
  {
    return Translation.op_Implicit(I18n.GetByKey("item.crabpot-bait-needed"));
  }

  public static string Item_CrabpotBaitNotNeeded()
  {
    return Translation.op_Implicit(I18n.GetByKey("item.crabpot-bait-not-needed"));
  }

  public static string Item_FenceHealth()
  {
    return Translation.op_Implicit(I18n.GetByKey("item.fence-health"));
  }

  public static string Item_FenceHealth_GoldClock()
  {
    return Translation.op_Implicit(I18n.GetByKey("item.fence-health.gold-clock"));
  }

  public static string Item_FenceHealth_Summary(object? percent, object? count)
  {
    return Translation.op_Implicit(I18n.GetByKey("item.fence-health.summary", (object) new
    {
      percent = percent,
      count = count
    }));
  }

  public static string Item_FishPondDrops()
  {
    return Translation.op_Implicit(I18n.GetByKey("item.fish-pond-drops"));
  }

  public static string Item_FishSpawnRules()
  {
    return Translation.op_Implicit(I18n.GetByKey("item.fish-spawn-rules"));
  }

  public static string Item_FishSpawnRules_MinFishingLevel(object? level)
  {
    return Translation.op_Implicit(I18n.GetByKey("item.fish-spawn-rules.min-fishing-level", (object) new
    {
      level = level
    }));
  }

  public static string Item_FishSpawnRules_NotCaughtYet()
  {
    return Translation.op_Implicit(I18n.GetByKey("item.fish-spawn-rules.not-caught-yet"));
  }

  public static string Item_FishSpawnRules_ExtendedFamilyQuestActive()
  {
    return Translation.op_Implicit(I18n.GetByKey("item.fish-spawn-rules.extended-family-quest-active"));
  }

  public static string Item_FishSpawnRules_Locations(object? locations)
  {
    return Translation.op_Implicit(I18n.GetByKey("item.fish-spawn-rules.locations", (object) new
    {
      locations = locations
    }));
  }

  public static string Item_FishSpawnRules_LocationsBySeason_Label()
  {
    return Translation.op_Implicit(I18n.GetByKey("item.fish-spawn-rules.locations-by-season.label"));
  }

  public static string Item_FishSpawnRules_LocationsBySeason_SeasonLocations(
    object? season,
    object? locations)
  {
    return Translation.op_Implicit(I18n.GetByKey("item.fish-spawn-rules.locations-by-season.season-locations", (object) new
    {
      season = season,
      locations = locations
    }));
  }

  public static string Item_FishSpawnRules_SeasonAny()
  {
    return Translation.op_Implicit(I18n.GetByKey("item.fish-spawn-rules.season-any"));
  }

  public static string Item_FishSpawnRules_SeasonList(object? seasons)
  {
    return Translation.op_Implicit(I18n.GetByKey("item.fish-spawn-rules.season-list", (object) new
    {
      seasons = seasons
    }));
  }

  public static string Item_FishSpawnRules_Time(object? times)
  {
    return Translation.op_Implicit(I18n.GetByKey("item.fish-spawn-rules.time", (object) new
    {
      times = times
    }));
  }

  public static string Item_FishSpawnRules_WeatherSunny()
  {
    return Translation.op_Implicit(I18n.GetByKey("item.fish-spawn-rules.weather-sunny"));
  }

  public static string Item_FishSpawnRules_WeatherRainy()
  {
    return Translation.op_Implicit(I18n.GetByKey("item.fish-spawn-rules.weather-rainy"));
  }

  public static string Item_UncaughtFish(object? count)
  {
    return Translation.op_Implicit(I18n.GetByKey("item.uncaught-fish", (object) new
    {
      count = count
    }));
  }

  public static string Item_MeleeWeapon_Accuracy()
  {
    return Translation.op_Implicit(I18n.GetByKey("item.melee-weapon.accuracy"));
  }

  public static string Item_MeleeWeapon_CriticalChance()
  {
    return Translation.op_Implicit(I18n.GetByKey("item.melee-weapon.critical-chance"));
  }

  public static string Item_MeleeWeapon_CriticalDamage()
  {
    return Translation.op_Implicit(I18n.GetByKey("item.melee-weapon.critical-damage"));
  }

  public static string Item_MeleeWeapon_Damage()
  {
    return Translation.op_Implicit(I18n.GetByKey("item.melee-weapon.damage"));
  }

  public static string Item_MeleeWeapon_Defense()
  {
    return Translation.op_Implicit(I18n.GetByKey("item.melee-weapon.defense"));
  }

  public static string Item_MeleeWeapon_Knockback()
  {
    return Translation.op_Implicit(I18n.GetByKey("item.melee-weapon.knockback"));
  }

  public static string Item_MeleeWeapon_Reach()
  {
    return Translation.op_Implicit(I18n.GetByKey("item.melee-weapon.reach"));
  }

  public static string Item_MeleeWeapon_Speed()
  {
    return Translation.op_Implicit(I18n.GetByKey("item.melee-weapon.speed"));
  }

  public static string Item_MeleeWeapon_CriticalDamage_Label(object? multiplier)
  {
    return Translation.op_Implicit(I18n.GetByKey("item.melee-weapon.critical-damage.label", (object) new
    {
      multiplier = multiplier
    }));
  }

  public static string Item_MeleeWeapon_Defense_Label(object? amount)
  {
    return Translation.op_Implicit(I18n.GetByKey("item.melee-weapon.defense.label", (object) new
    {
      amount = amount
    }));
  }

  public static string Item_MeleeWeapon_Knockback_Label(object? amount, object? multiplier)
  {
    return Translation.op_Implicit(I18n.GetByKey("item.melee-weapon.knockback.label", (object) new
    {
      amount = amount,
      multiplier = multiplier
    }));
  }

  public static string Item_MeleeWeapon_Reach_Label(object? amount)
  {
    return Translation.op_Implicit(I18n.GetByKey("item.melee-weapon.reach.label", (object) new
    {
      amount = amount
    }));
  }

  public static string Item_MeleeWeapon_Speed_ShownVsActual(
    object? shownSpeed,
    object? lineBreak,
    object? actualSpeed)
  {
    return Translation.op_Implicit(I18n.GetByKey("item.melee-weapon.speed.shown-vs-actual", (object) new
    {
      shownSpeed = shownSpeed,
      lineBreak = lineBreak,
      actualSpeed = actualSpeed
    }));
  }

  public static string Item_MeleeWeapon_Speed_Summary(object? speed, object? milliseconds)
  {
    return Translation.op_Implicit(I18n.GetByKey("item.melee-weapon.speed.summary", (object) new
    {
      speed = speed,
      milliseconds = milliseconds
    }));
  }

  public static string Item_MovieSnackPreference()
  {
    return Translation.op_Implicit(I18n.GetByKey("item.movie-snack-preference"));
  }

  public static string Item_MovieTicket_MovieThisWeek()
  {
    return Translation.op_Implicit(I18n.GetByKey("item.movie-ticket.movie-this-week"));
  }

  public static string Item_MovieTicket_LovesMovie()
  {
    return Translation.op_Implicit(I18n.GetByKey("item.movie-ticket.loves-movie"));
  }

  public static string Item_MovieTicket_LikesMovie()
  {
    return Translation.op_Implicit(I18n.GetByKey("item.movie-ticket.likes-movie"));
  }

  public static string Item_MovieTicket_DislikesMovie()
  {
    return Translation.op_Implicit(I18n.GetByKey("item.movie-ticket.dislikes-movie"));
  }

  public static string Item_MovieTicket_RejectsMovie()
  {
    return Translation.op_Implicit(I18n.GetByKey("item.movie-ticket.rejects-movie"));
  }

  public static string Item_MovieSnackPreference_Love(object? name)
  {
    return Translation.op_Implicit(I18n.GetByKey("item.movie-snack-preference.love", (object) new
    {
      name = name
    }));
  }

  public static string Item_MovieSnackPreference_Like(object? name)
  {
    return Translation.op_Implicit(I18n.GetByKey("item.movie-snack-preference.like", (object) new
    {
      name = name
    }));
  }

  public static string Item_MovieSnackPreference_Dislike(object? name)
  {
    return Translation.op_Implicit(I18n.GetByKey("item.movie-snack-preference.dislike", (object) new
    {
      name = name
    }));
  }

  public static string Item_MovieTicket_MovieThisWeek_None()
  {
    return Translation.op_Implicit(I18n.GetByKey("item.movie-ticket.movie-this-week.none"));
  }

  public static string Item_MusicBlock_Pitch()
  {
    return Translation.op_Implicit(I18n.GetByKey("item.music-block.pitch"));
  }

  public static string Item_MusicBlock_DrumType()
  {
    return Translation.op_Implicit(I18n.GetByKey("item.music-block.drum-type"));
  }

  public static string Monster_Invincible()
  {
    return Translation.op_Implicit(I18n.GetByKey("monster.invincible"));
  }

  public static string Monster_Health() => Translation.op_Implicit(I18n.GetByKey("monster.health"));

  public static string Monster_Drops() => Translation.op_Implicit(I18n.GetByKey("monster.drops"));

  public static string Monster_Experience()
  {
    return Translation.op_Implicit(I18n.GetByKey("monster.experience"));
  }

  public static string Monster_Defense()
  {
    return Translation.op_Implicit(I18n.GetByKey("monster.defense"));
  }

  public static string Monster_Attack() => Translation.op_Implicit(I18n.GetByKey("monster.attack"));

  public static string Monster_AdventureGuild()
  {
    return Translation.op_Implicit(I18n.GetByKey("monster.adventure-guild"));
  }

  public static string Monster_Drops_Nothing()
  {
    return Translation.op_Implicit(I18n.GetByKey("monster.drops.nothing"));
  }

  public static string Monster_AdventureGuild_EradicationGoal(
    object? name,
    object? count,
    object? requiredCount)
  {
    return Translation.op_Implicit(I18n.GetByKey("monster.adventure-guild.eradication-goal", (object) new
    {
      name = name,
      count = count,
      requiredCount = requiredCount
    }));
  }

  public static string Npc_Birthday() => Translation.op_Implicit(I18n.GetByKey("npc.birthday"));

  public static string Npc_CanRomance()
  {
    return Translation.op_Implicit(I18n.GetByKey("npc.can-romance"));
  }

  public static string Npc_Friendship() => Translation.op_Implicit(I18n.GetByKey("npc.friendship"));

  public static string Npc_TalkedToday()
  {
    return Translation.op_Implicit(I18n.GetByKey("npc.talked-today"));
  }

  public static string Npc_GiftedToday()
  {
    return Translation.op_Implicit(I18n.GetByKey("npc.gifted-today"));
  }

  public static string Npc_GiftedThisWeek()
  {
    return Translation.op_Implicit(I18n.GetByKey("npc.gifted-this-week"));
  }

  public static string Npc_KissedToday()
  {
    return Translation.op_Implicit(I18n.GetByKey("npc.kissed-today"));
  }

  public static string Npc_HuggedToday()
  {
    return Translation.op_Implicit(I18n.GetByKey("npc.hugged-today"));
  }

  public static string Npc_LovesGifts()
  {
    return Translation.op_Implicit(I18n.GetByKey("npc.loves-gifts"));
  }

  public static string Npc_LikesGifts()
  {
    return Translation.op_Implicit(I18n.GetByKey("npc.likes-gifts"));
  }

  public static string Npc_NeutralGifts()
  {
    return Translation.op_Implicit(I18n.GetByKey("npc.neutral-gifts"));
  }

  public static string Npc_DislikesGifts()
  {
    return Translation.op_Implicit(I18n.GetByKey("npc.dislikes-gifts"));
  }

  public static string Npc_HatesGifts()
  {
    return Translation.op_Implicit(I18n.GetByKey("npc.hates-gifts"));
  }

  public static string Npc_Schedule() => Translation.op_Implicit(I18n.GetByKey("npc.schedule"));

  public static string Npc_CanRomance_Married()
  {
    return Translation.op_Implicit(I18n.GetByKey("npc.can-romance.married"));
  }

  public static string Npc_CanRomance_Housemate()
  {
    return Translation.op_Implicit(I18n.GetByKey("npc.can-romance.housemate"));
  }

  public static string Npc_Friendship_NotMet()
  {
    return Translation.op_Implicit(I18n.GetByKey("npc.friendship.not-met"));
  }

  public static string Npc_Friendship_NeedBouquet()
  {
    return Translation.op_Implicit(I18n.GetByKey("npc.friendship.need-bouquet"));
  }

  public static string Npc_Friendship_NeedPoints(object? count)
  {
    return Translation.op_Implicit(I18n.GetByKey("npc.friendship.need-points", (object) new
    {
      count = count
    }));
  }

  public static string Npc_UndiscoveredGiftTaste(object? count)
  {
    return Translation.op_Implicit(I18n.GetByKey("npc.undiscovered-gift-taste", (object) new
    {
      count = count
    }));
  }

  public static string Npc_UnownedGiftTaste(object? count)
  {
    return Translation.op_Implicit(I18n.GetByKey("npc.unowned-gift-taste", (object) new
    {
      count = count
    }));
  }

  public static string Npc_Schedule_CurrentPosition(object? locationName, object? x, object? y)
  {
    return Translation.op_Implicit(I18n.GetByKey("npc.schedule.current-position", (object) new
    {
      locationName = locationName,
      x = x,
      y = y
    }));
  }

  public static string Npc_Schedule_NoEntries()
  {
    return Translation.op_Implicit(I18n.GetByKey("npc.schedule.no-entries"));
  }

  public static string Npc_Schedule_NotFollowingSchedule()
  {
    return Translation.op_Implicit(I18n.GetByKey("npc.schedule.not-following-schedule"));
  }

  public static string Npc_Schedule_Entry(object? time, object? locationName, object? x, object? y)
  {
    return Translation.op_Implicit(I18n.GetByKey("npc.schedule.entry", (object) new
    {
      time = time,
      locationName = locationName,
      x = x,
      y = y
    }));
  }

  public static string Npc_Schedule_Farmhand_UnknownPosition()
  {
    return Translation.op_Implicit(I18n.GetByKey("npc.schedule.farmhand.unknown-position"));
  }

  public static string Npc_Schedule_Farmhand_UnknownSchedule()
  {
    return Translation.op_Implicit(I18n.GetByKey("npc.schedule.farmhand.unknown-schedule"));
  }

  public static string Npc_Child_Age() => Translation.op_Implicit(I18n.GetByKey("npc.child.age"));

  public static string Npc_Child_Age_DescriptionPartial(
    object? label,
    object? count,
    object? nextLabel)
  {
    return Translation.op_Implicit(I18n.GetByKey("npc.child.age.description-partial", (object) new
    {
      label = label,
      count = count,
      nextLabel = nextLabel
    }));
  }

  public static string Npc_Child_Age_DescriptionGrown(object? label)
  {
    return Translation.op_Implicit(I18n.GetByKey("npc.child.age.description-grown", (object) new
    {
      label = label
    }));
  }

  public static string Npc_Child_Age_Newborn()
  {
    return Translation.op_Implicit(I18n.GetByKey("npc.child.age.newborn"));
  }

  public static string Npc_Child_Age_Baby()
  {
    return Translation.op_Implicit(I18n.GetByKey("npc.child.age.baby"));
  }

  public static string Npc_Child_Age_Crawler()
  {
    return Translation.op_Implicit(I18n.GetByKey("npc.child.age.crawler"));
  }

  public static string Npc_Child_Age_Toddler()
  {
    return Translation.op_Implicit(I18n.GetByKey("npc.child.age.toddler"));
  }

  public static string Pet_Love() => Translation.op_Implicit(I18n.GetByKey("pet.love"));

  public static string Pet_PettedToday()
  {
    return Translation.op_Implicit(I18n.GetByKey("pet.petted-today"));
  }

  public static string Pet_LastPetted()
  {
    return Translation.op_Implicit(I18n.GetByKey("pet.last-petted"));
  }

  public static string Pet_WaterBowl() => Translation.op_Implicit(I18n.GetByKey("pet.water-bowl"));

  public static string Pet_LastPetted_Yes()
  {
    return Translation.op_Implicit(I18n.GetByKey("pet.last-petted.yes"));
  }

  public static string Pet_LastPetted_DaysAgo(object? days)
  {
    return Translation.op_Implicit(I18n.GetByKey("pet.last-petted.days-ago", (object) new
    {
      days = days
    }));
  }

  public static string Pet_LastPetted_Never()
  {
    return Translation.op_Implicit(I18n.GetByKey("pet.last-petted.never"));
  }

  public static string Pet_WaterBowl_Empty()
  {
    return Translation.op_Implicit(I18n.GetByKey("pet.water-bowl.empty"));
  }

  public static string Pet_WaterBowl_Filled()
  {
    return Translation.op_Implicit(I18n.GetByKey("pet.water-bowl.filled"));
  }

  public static string Player_FarmName()
  {
    return Translation.op_Implicit(I18n.GetByKey("player.farm-name"));
  }

  public static string Player_FarmMap()
  {
    return Translation.op_Implicit(I18n.GetByKey("player.farm-map"));
  }

  public static string Player_FavoriteThing()
  {
    return Translation.op_Implicit(I18n.GetByKey("player.favorite-thing"));
  }

  public static string Player_Gender() => Translation.op_Implicit(I18n.GetByKey("player.gender"));

  public static string Player_Housemate()
  {
    return Translation.op_Implicit(I18n.GetByKey("player.housemate"));
  }

  public static string Player_Spouse() => Translation.op_Implicit(I18n.GetByKey("player.spouse"));

  public static string Player_WatchedMovieThisWeek()
  {
    return Translation.op_Implicit(I18n.GetByKey("player.watched-movie-this-week"));
  }

  public static string Player_CombatSkill()
  {
    return Translation.op_Implicit(I18n.GetByKey("player.combat-skill"));
  }

  public static string Player_FarmingSkill()
  {
    return Translation.op_Implicit(I18n.GetByKey("player.farming-skill"));
  }

  public static string Player_FishingSkill()
  {
    return Translation.op_Implicit(I18n.GetByKey("player.fishing-skill"));
  }

  public static string Player_ForagingSkill()
  {
    return Translation.op_Implicit(I18n.GetByKey("player.foraging-skill"));
  }

  public static string Player_MiningSkill()
  {
    return Translation.op_Implicit(I18n.GetByKey("player.mining-skill"));
  }

  public static string Player_Luck() => Translation.op_Implicit(I18n.GetByKey("player.luck"));

  public static string Player_SaveFormat()
  {
    return Translation.op_Implicit(I18n.GetByKey("player.save-format"));
  }

  public static string Player_FarmMap_Custom()
  {
    return Translation.op_Implicit(I18n.GetByKey("player.farm-map.custom"));
  }

  public static string Player_Gender_Male()
  {
    return Translation.op_Implicit(I18n.GetByKey("player.gender.male"));
  }

  public static string Player_Gender_Female()
  {
    return Translation.op_Implicit(I18n.GetByKey("player.gender.female"));
  }

  public static string Player_Luck_Summary(object? percent)
  {
    return Translation.op_Implicit(I18n.GetByKey("player.luck.summary", (object) new
    {
      percent = percent
    }));
  }

  public static string Player_Skill_Progress(object? level, object? expNeeded)
  {
    return Translation.op_Implicit(I18n.GetByKey("player.skill.progress", (object) new
    {
      level = level,
      expNeeded = expNeeded
    }));
  }

  public static string Player_Skill_ProgressLast(object? level)
  {
    return Translation.op_Implicit(I18n.GetByKey("player.skill.progress-last", (object) new
    {
      level = level
    }));
  }

  public static string Puzzle_Solution()
  {
    return Translation.op_Implicit(I18n.GetByKey("puzzle.solution"));
  }

  public static string Puzzle_Solution_Solved()
  {
    return Translation.op_Implicit(I18n.GetByKey("puzzle.solution.solved"));
  }

  public static string Puzzle_Solution_Hidden()
  {
    return Translation.op_Implicit(I18n.GetByKey("puzzle.solution.hidden"));
  }

  public static string Puzzle_IslandCrystalCave_Title()
  {
    return Translation.op_Implicit(I18n.GetByKey("puzzle.island-crystal-cave.title"));
  }

  public static string Puzzle_IslandCrystalCave_CrystalId()
  {
    return Translation.op_Implicit(I18n.GetByKey("puzzle.island-crystal-cave.crystal-id"));
  }

  public static string Puzzle_IslandCrystalCave_Solution_NotActivated()
  {
    return Translation.op_Implicit(I18n.GetByKey("puzzle.island-crystal-cave.solution.not-activated"));
  }

  public static string Puzzle_IslandCrystalCave_Solution_Waiting()
  {
    return Translation.op_Implicit(I18n.GetByKey("puzzle.island-crystal-cave.solution.waiting"));
  }

  public static string Puzzle_IslandCrystalCave_Solution_Activated()
  {
    return Translation.op_Implicit(I18n.GetByKey("puzzle.island-crystal-cave.solution.activated"));
  }

  public static string Puzzle_IslandMermaid_Title()
  {
    return Translation.op_Implicit(I18n.GetByKey("puzzle.island-mermaid.title"));
  }

  public static string Puzzle_IslandMermaid_Solution_Intro()
  {
    return Translation.op_Implicit(I18n.GetByKey("puzzle.island-mermaid.solution.intro"));
  }

  public static string Puzzle_IslandShrine_Title()
  {
    return Translation.op_Implicit(I18n.GetByKey("puzzle.island-shrine.title"));
  }

  public static string Puzzle_IslandShrine_Solution()
  {
    return Translation.op_Implicit(I18n.GetByKey("puzzle.island-shrine.solution"));
  }

  public static string Puzzle_IslandShrine_Solution_East(object? item)
  {
    return Translation.op_Implicit(I18n.GetByKey("puzzle.island-shrine.solution.east", (object) new
    {
      item = item
    }));
  }

  public static string Puzzle_IslandShrine_Solution_North(object? item)
  {
    return Translation.op_Implicit(I18n.GetByKey("puzzle.island-shrine.solution.north", (object) new
    {
      item = item
    }));
  }

  public static string Puzzle_IslandShrine_Solution_South(object? item)
  {
    return Translation.op_Implicit(I18n.GetByKey("puzzle.island-shrine.solution.south", (object) new
    {
      item = item
    }));
  }

  public static string Puzzle_IslandShrine_Solution_West(object? item)
  {
    return Translation.op_Implicit(I18n.GetByKey("puzzle.island-shrine.solution.west", (object) new
    {
      item = item
    }));
  }

  public static string Tile_Title(object? x, object? y)
  {
    return Translation.op_Implicit(I18n.GetByKey("tile.title", (object) new
    {
      x = x,
      y = y
    }));
  }

  public static string Tile_Description()
  {
    return Translation.op_Implicit(I18n.GetByKey("tile.description"));
  }

  public static string Tile_GameLocation()
  {
    return Translation.op_Implicit(I18n.GetByKey("tile.game-location"));
  }

  public static string Tile_MapName() => Translation.op_Implicit(I18n.GetByKey("tile.map-name"));

  public static string Tile_MapProperties()
  {
    return Translation.op_Implicit(I18n.GetByKey("tile.map-properties"));
  }

  public static string Tile_LayerTile(object? layer)
  {
    return Translation.op_Implicit(I18n.GetByKey("tile.layer-tile", (object) new
    {
      layer = layer
    }));
  }

  public static string Tile_LayerTileNone()
  {
    return Translation.op_Implicit(I18n.GetByKey("tile.layer-tile-none"));
  }

  public static string Tile_MapProperties_Value(object? name, object? value)
  {
    return Translation.op_Implicit(I18n.GetByKey("tile.map-properties.value", (object) new
    {
      name = name,
      value = value
    }));
  }

  public static string Tile_LayerTile_Appearance(
    object? index,
    object? tilesheetId,
    object? tilesheetPath)
  {
    return Translation.op_Implicit(I18n.GetByKey("tile.layer-tile.appearance", (object) new
    {
      index = index,
      tilesheetId = tilesheetId,
      tilesheetPath = tilesheetPath
    }));
  }

  public static string Tile_LayerTile_BlendMode(object? value)
  {
    return Translation.op_Implicit(I18n.GetByKey("tile.layer-tile.blend-mode", (object) new
    {
      value = value
    }));
  }

  public static string Tile_LayerTile_IndexProperty(object? name, object? value)
  {
    return Translation.op_Implicit(I18n.GetByKey("tile.layer-tile.index-property", (object) new
    {
      name = name,
      value = value
    }));
  }

  public static string Tile_LayerTile_TileProperty(object? name, object? value)
  {
    return Translation.op_Implicit(I18n.GetByKey("tile.layer-tile.tile-property", (object) new
    {
      name = name,
      value = value
    }));
  }

  public static string Tile_LayerTile_NoneHere()
  {
    return Translation.op_Implicit(I18n.GetByKey("tile.layer-tile.none-here"));
  }

  public static string TrashBearOrGourmand_ItemWanted()
  {
    return Translation.op_Implicit(I18n.GetByKey("trash-bear-or-gourmand.item-wanted"));
  }

  public static string TrashBearOrGourmand_QuestProgress()
  {
    return Translation.op_Implicit(I18n.GetByKey("trash-bear-or-gourmand.quest-progress"));
  }

  public static string Tree_Stage() => Translation.op_Implicit(I18n.GetByKey("tree.stage"));

  public static string Tree_NextGrowth()
  {
    return Translation.op_Implicit(I18n.GetByKey("tree.next-growth"));
  }

  public static string Tree_Seed() => Translation.op_Implicit(I18n.GetByKey("tree.seed"));

  public static string Tree_IsFertilized()
  {
    return Translation.op_Implicit(I18n.GetByKey("tree.is-fertilized"));
  }

  public static string Tree_Name_BigMushroom()
  {
    return Translation.op_Implicit(I18n.GetByKey("tree.name.big-mushroom"));
  }

  public static string Tree_Name_Mahogany()
  {
    return Translation.op_Implicit(I18n.GetByKey("tree.name.mahogany"));
  }

  public static string Tree_Name_Maple()
  {
    return Translation.op_Implicit(I18n.GetByKey("tree.name.maple"));
  }

  public static string Tree_Name_Oak() => Translation.op_Implicit(I18n.GetByKey("tree.name.oak"));

  public static string Tree_Name_Palm() => Translation.op_Implicit(I18n.GetByKey("tree.name.palm"));

  public static string Tree_Name_Pine() => Translation.op_Implicit(I18n.GetByKey("tree.name.pine"));

  public static string Tree_Name_Mossy()
  {
    return Translation.op_Implicit(I18n.GetByKey("tree.name.mossy"));
  }

  public static string Tree_Name_Mystic()
  {
    return Translation.op_Implicit(I18n.GetByKey("tree.name.mystic"));
  }

  public static string Tree_Name_Unknown()
  {
    return Translation.op_Implicit(I18n.GetByKey("tree.name.unknown"));
  }

  public static string Tree_Stage_Done()
  {
    return Translation.op_Implicit(I18n.GetByKey("tree.stage.done"));
  }

  public static string Tree_Stage_Partial(object? stageName, object? step, object? max)
  {
    return Translation.op_Implicit(I18n.GetByKey("tree.stage.partial", (object) new
    {
      stageName = stageName,
      step = step,
      max = max
    }));
  }

  public static string Tree_NextGrowth_Winter()
  {
    return Translation.op_Implicit(I18n.GetByKey("tree.next-growth.winter"));
  }

  public static string Tree_NextGrowth_AdjacentTrees()
  {
    return Translation.op_Implicit(I18n.GetByKey("tree.next-growth.adjacent-trees"));
  }

  public static string Tree_NextGrowth_Chance(object? chance, object? stage)
  {
    return Translation.op_Implicit(I18n.GetByKey("tree.next-growth.chance", (object) new
    {
      chance = chance,
      stage = stage
    }));
  }

  public static string Tree_Seed_NotReady()
  {
    return Translation.op_Implicit(I18n.GetByKey("tree.seed.not-ready"));
  }

  public static string Tree_Seed_ProbabilityDaily(object? chance, object? itemName)
  {
    return Translation.op_Implicit(I18n.GetByKey("tree.seed.probability-daily", (object) new
    {
      chance = chance,
      itemName = itemName
    }));
  }

  public static string Tree_Seed_ProbabilityOnChop(object? chance, object? itemName)
  {
    return Translation.op_Implicit(I18n.GetByKey("tree.seed.probability-on-chop", (object) new
    {
      chance = chance,
      itemName = itemName
    }));
  }

  public static string Tree_Stages_Seed()
  {
    return Translation.op_Implicit(I18n.GetByKey("tree.stages.seed"));
  }

  public static string Tree_Stages_Sprout()
  {
    return Translation.op_Implicit(I18n.GetByKey("tree.stages.sprout"));
  }

  public static string Tree_Stages_Sapling()
  {
    return Translation.op_Implicit(I18n.GetByKey("tree.stages.sapling"));
  }

  public static string Tree_Stages_Bush()
  {
    return Translation.op_Implicit(I18n.GetByKey("tree.stages.bush"));
  }

  public static string Tree_Stages_SmallTree()
  {
    return Translation.op_Implicit(I18n.GetByKey("tree.stages.smallTree"));
  }

  public static string Tree_Stages_Tree()
  {
    return Translation.op_Implicit(I18n.GetByKey("tree.stages.tree"));
  }

  public static string Config_Title_MainOptions()
  {
    return Translation.op_Implicit(I18n.GetByKey("config.title.main-options"));
  }

  public static string Config_ForceFullScreen_Name()
  {
    return Translation.op_Implicit(I18n.GetByKey("config.force-full-screen.name"));
  }

  public static string Config_ForceFullScreen_Desc()
  {
    return Translation.op_Implicit(I18n.GetByKey("config.force-full-screen.desc"));
  }

  public static string Config_Title_Progression()
  {
    return Translation.op_Implicit(I18n.GetByKey("config.title.progression"));
  }

  public static string Config_Progression_ShowUncaughtFishSpawnRules_Name()
  {
    return Translation.op_Implicit(I18n.GetByKey("config.progression.show-uncaught-fish-spawn-rules.name"));
  }

  public static string Config_Progression_ShowUncaughtFishSpawnRules_Desc()
  {
    return Translation.op_Implicit(I18n.GetByKey("config.progression.show-uncaught-fish-spawn-rules.desc"));
  }

  public static string Config_Progression_ShowUnknownGiftTastes_Name()
  {
    return Translation.op_Implicit(I18n.GetByKey("config.progression.show-unknown-gift-tastes.name"));
  }

  public static string Config_Progression_ShowUnknownGiftTastes_Desc()
  {
    return Translation.op_Implicit(I18n.GetByKey("config.progression.show-unknown-gift-tastes.desc"));
  }

  public static string Config_Progression_ShowUnknownRecipes_Name()
  {
    return Translation.op_Implicit(I18n.GetByKey("config.progression.show-unknown-recipes.name"));
  }

  public static string Config_Progression_ShowUnknownRecipes_Desc()
  {
    return Translation.op_Implicit(I18n.GetByKey("config.progression.show-unknown-recipes.desc"));
  }

  public static string Config_Progression_ShowPuzzleSolutions_Name()
  {
    return Translation.op_Implicit(I18n.GetByKey("config.progression.show-puzzle-solutions.name"));
  }

  public static string Config_Progression_ShowPuzzleSolutions_Desc()
  {
    return Translation.op_Implicit(I18n.GetByKey("config.progression.show-puzzle-solutions.desc"));
  }

  public static string Config_Title_GiftTastes()
  {
    return Translation.op_Implicit(I18n.GetByKey("config.title.gift-tastes"));
  }

  public static string Config_ShowGiftTastes_Loved_Name()
  {
    return Translation.op_Implicit(I18n.GetByKey("config.show-gift-tastes.loved.name"));
  }

  public static string Config_ShowGiftTastes_Loved_Desc()
  {
    return Translation.op_Implicit(I18n.GetByKey("config.show-gift-tastes.loved.desc"));
  }

  public static string Config_ShowGiftTastes_Liked_Name()
  {
    return Translation.op_Implicit(I18n.GetByKey("config.show-gift-tastes.liked.name"));
  }

  public static string Config_ShowGiftTastes_Liked_Desc()
  {
    return Translation.op_Implicit(I18n.GetByKey("config.show-gift-tastes.liked.desc"));
  }

  public static string Config_ShowGiftTastes_Neutral_Name()
  {
    return Translation.op_Implicit(I18n.GetByKey("config.show-gift-tastes.neutral.name"));
  }

  public static string Config_ShowGiftTastes_Neutral_Desc()
  {
    return Translation.op_Implicit(I18n.GetByKey("config.show-gift-tastes.neutral.desc"));
  }

  public static string Config_ShowGiftTastes_Disliked_Name()
  {
    return Translation.op_Implicit(I18n.GetByKey("config.show-gift-tastes.disliked.name"));
  }

  public static string Config_ShowGiftTastes_Disliked_Desc()
  {
    return Translation.op_Implicit(I18n.GetByKey("config.show-gift-tastes.disliked.desc"));
  }

  public static string Config_ShowGiftTastes_Hated_Name()
  {
    return Translation.op_Implicit(I18n.GetByKey("config.show-gift-tastes.hated.name"));
  }

  public static string Config_ShowGiftTastes_Hated_Desc()
  {
    return Translation.op_Implicit(I18n.GetByKey("config.show-gift-tastes.hated.desc"));
  }

  public static string Config_ShowUnownedGifts_Name()
  {
    return Translation.op_Implicit(I18n.GetByKey("config.show-unowned-gifts.name"));
  }

  public static string Config_ShowUnownedGifts_Desc()
  {
    return Translation.op_Implicit(I18n.GetByKey("config.show-unowned-gifts.desc"));
  }

  public static string Config_HighlightUnrevealedGiftTastes_Name()
  {
    return Translation.op_Implicit(I18n.GetByKey("config.highlight-unrevealed-gift-tastes.name"));
  }

  public static string Config_HighlightUnrevealedGiftTastes_Desc()
  {
    return Translation.op_Implicit(I18n.GetByKey("config.highlight-unrevealed-gift-tastes.desc"));
  }

  public static string Config_Title_CollapseFields()
  {
    return Translation.op_Implicit(I18n.GetByKey("config.title.collapse-fields"));
  }

  public static string Config_CollapseFields_Enabled_Name()
  {
    return Translation.op_Implicit(I18n.GetByKey("config.collapse-fields.enabled.name"));
  }

  public static string Config_CollapseFields_Enabled_Desc()
  {
    return Translation.op_Implicit(I18n.GetByKey("config.collapse-fields.enabled.desc"));
  }

  public static string Config_CollapseFields_BuildingRecipes_Name()
  {
    return Translation.op_Implicit(I18n.GetByKey("config.collapse-fields.building-recipes.name"));
  }

  public static string Config_CollapseFields_ItemRecipes_Name()
  {
    return Translation.op_Implicit(I18n.GetByKey("config.collapse-fields.item-recipes.name"));
  }

  public static string Config_CollapseFields_NpcGiftTastes_Name()
  {
    return Translation.op_Implicit(I18n.GetByKey("config.collapse-fields.npc-gift-tastes.name"));
  }

  public static string Config_CollapseFields_Any_Desc()
  {
    return Translation.op_Implicit(I18n.GetByKey("config.collapse-fields.any.desc"));
  }

  public static string Config_Title_AdvancedOptions()
  {
    return Translation.op_Implicit(I18n.GetByKey("config.title.advanced-options"));
  }

  public static string Config_TileLookups_Name()
  {
    return Translation.op_Implicit(I18n.GetByKey("config.tile-lookups.name"));
  }

  public static string Config_TileLookups_Desc()
  {
    return Translation.op_Implicit(I18n.GetByKey("config.tile-lookups.desc"));
  }

  public static string Config_DataMiningFields_Name()
  {
    return Translation.op_Implicit(I18n.GetByKey("config.data-mining-fields.name"));
  }

  public static string Config_DataMiningFields_Desc()
  {
    return Translation.op_Implicit(I18n.GetByKey("config.data-mining-fields.desc"));
  }

  public static string Config_ShowInvalidRecipes_Name()
  {
    return Translation.op_Implicit(I18n.GetByKey("config.show-invalid-recipes.name"));
  }

  public static string Config_ShowInvalidRecipes_Desc()
  {
    return Translation.op_Implicit(I18n.GetByKey("config.show-invalid-recipes.desc"));
  }

  public static string Config_TargetRedirection_Name()
  {
    return Translation.op_Implicit(I18n.GetByKey("config.target-redirection.name"));
  }

  public static string Config_TargetRedirection_Desc()
  {
    return Translation.op_Implicit(I18n.GetByKey("config.target-redirection.desc"));
  }

  public static string Config_ScrollAmount_Name()
  {
    return Translation.op_Implicit(I18n.GetByKey("config.scroll-amount.name"));
  }

  public static string Config_ScrollAmount_Desc()
  {
    return Translation.op_Implicit(I18n.GetByKey("config.scroll-amount.desc"));
  }

  public static string Config_Title_Controls()
  {
    return Translation.op_Implicit(I18n.GetByKey("config.title.controls"));
  }

  public static string Config_HideOnKeyUp_Name()
  {
    return Translation.op_Implicit(I18n.GetByKey("config.hide-on-key-up.name"));
  }

  public static string Config_HideOnKeyUp_Desc()
  {
    return Translation.op_Implicit(I18n.GetByKey("config.hide-on-key-up.desc"));
  }

  public static string Config_ToggleLookup_Name()
  {
    return Translation.op_Implicit(I18n.GetByKey("config.toggle-lookup.name"));
  }

  public static string Config_ToggleLookup_Desc()
  {
    return Translation.op_Implicit(I18n.GetByKey("config.toggle-lookup.desc"));
  }

  public static string Config_ToggleSearch_Name()
  {
    return Translation.op_Implicit(I18n.GetByKey("config.toggle-search.name"));
  }

  public static string Config_ToggleSearch_Desc()
  {
    return Translation.op_Implicit(I18n.GetByKey("config.toggle-search.desc"));
  }

  public static string Config_ScrollUp_Name()
  {
    return Translation.op_Implicit(I18n.GetByKey("config.scroll-up.name"));
  }

  public static string Config_ScrollUp_Desc()
  {
    return Translation.op_Implicit(I18n.GetByKey("config.scroll-up.desc"));
  }

  public static string Config_ScrollDown_Name()
  {
    return Translation.op_Implicit(I18n.GetByKey("config.scroll-down.name"));
  }

  public static string Config_ScrollDown_Desc()
  {
    return Translation.op_Implicit(I18n.GetByKey("config.scroll-down.desc"));
  }

  public static string Config_PageUp_Name()
  {
    return Translation.op_Implicit(I18n.GetByKey("config.page-up.name"));
  }

  public static string Config_PageUp_Desc()
  {
    return Translation.op_Implicit(I18n.GetByKey("config.page-up.desc"));
  }

  public static string Config_PageDown_Name()
  {
    return Translation.op_Implicit(I18n.GetByKey("config.page-down.name"));
  }

  public static string Config_PageDown_Desc()
  {
    return Translation.op_Implicit(I18n.GetByKey("config.page-down.desc"));
  }

  public static string Config_ToggleDebug_Name()
  {
    return Translation.op_Implicit(I18n.GetByKey("config.toggle-debug.name"));
  }

  public static string Config_ToggleDebug_Desc()
  {
    return Translation.op_Implicit(I18n.GetByKey("config.toggle-debug.desc"));
  }

  public static string Icon_ToggleSearch_Name()
  {
    return Translation.op_Implicit(I18n.GetByKey("icon.toggle-search.name"));
  }

  public static string Icon_ToggleSearch_Desc()
  {
    return Translation.op_Implicit(I18n.GetByKey("icon.toggle-search.desc"));
  }

  public static Translation GetByKey(string key, object? tokens = null)
  {
    if (I18n.Translations == null)
      throw new InvalidOperationException("You must call I18n.Init from the mod's entry method before reading translations.");
    return I18n.Translations.Get(key, tokens);
  }

  public static class Keys
  {
    public const string Generic_ListSeparator = "generic.list-separator";
    public const string Generic_LineWrapOn = "generic.line-wrap-on";
    public const string Generic_Now = "generic.now";
    public const string Generic_Tomorrow = "generic.tomorrow";
    public const string Generic_Yesterday = "generic.yesterday";
    public const string Generic_Unknown = "generic.unknown";
    public const string Generic_Percent = "generic.percent";
    public const string Generic_PercentChanceOf = "generic.percent-chance-of";
    public const string Generic_PercentRatio = "generic.percent-ratio";
    public const string Generic_Ratio = "generic.ratio";
    public const string Generic_Range = "generic.range";
    public const string Generic_Yes = "generic.yes";
    public const string Generic_No = "generic.no";
    public const string Generic_Seconds = "generic.seconds";
    public const string Generic_Minutes = "generic.minutes";
    public const string Generic_Hours = "generic.hours";
    public const string Generic_Days = "generic.days";
    public const string Generic_InXDays = "generic.in-x-days";
    public const string Generic_XDaysAgo = "generic.x-days-ago";
    public const string Generic_Price = "generic.price";
    public const string Generic_PriceForQuality = "generic.price-for-quality";
    public const string Generic_PriceForStack = "generic.price-for-stack";
    public const string Generic_ShowXResults = "generic.show-x-results";
    public const string Type_Building = "type.building";
    public const string Type_Bush = "type.bush";
    public const string Type_FruitTree = "type.fruit-tree";
    public const string Type_Monster = "type.monster";
    public const string Type_Player = "type.player";
    public const string Type_Tree = "type.tree";
    public const string Type_Villager = "type.villager";
    public const string Type_Other = "type.other";
    public const string Quality_Normal = "quality.normal";
    public const string Quality_Silver = "quality.silver";
    public const string Quality_Gold = "quality.gold";
    public const string Quality_Iridium = "quality.iridium";
    public const string Item_WildSeeds = "item.wild-seeds";
    public const string FriendshipStatus_Friendly = "friendship-status.friendly";
    public const string FriendshipStatus_Dating = "friendship-status.dating";
    public const string FriendshipStatus_Engaged = "friendship-status.engaged";
    public const string FriendshipStatus_Married = "friendship-status.married";
    public const string FriendshipStatus_Divorced = "friendship-status.divorced";
    public const string FriendshipStatus_KickedOut = "friendship-status.kicked-out";
    public const string AddedByMod = "added-by-mod";
    public const string AddedByMod_Summary = "added-by-mod.summary";
    public const string BundleArea_Pantry = "bundle-area.pantry";
    public const string BundleArea_CraftsRoom = "bundle-area.crafts-room";
    public const string BundleArea_FishTank = "bundle-area.fish-tank";
    public const string BundleArea_BoilerRoom = "bundle-area.boiler-room";
    public const string BundleArea_Vault = "bundle-area.vault";
    public const string BundleArea_BulletinBoard = "bundle-area.bulletin-board";
    public const string BundleArea_AbandonedJojaMart = "bundle-area.abandoned-joja-mart";
    public const string Shop_AdventureGuild = "shop.adventure-guild";
    public const string Shop_Clint = "shop.clint";
    public const string Shop_Marnie = "shop.marnie";
    public const string Shop_Pierre = "shop.pierre";
    public const string Shop_Robin = "shop.robin";
    public const string Shop_Volcano = "shop.volcano";
    public const string Shop_Willy = "shop.willy";
    public const string RecipeType_Cooking = "recipe-type.cooking";
    public const string RecipeType_Crafting = "recipe-type.crafting";
    public const string RecipeType_Tailoring = "recipe-type.tailoring";
    public const string Data_Npc_Pet_Description = "data.npc.pet.description";
    public const string Data_Npc_Horse_Description = "data.npc.horse.description";
    public const string Data_Npc_Junimo_Description = "data.npc.junimo.description";
    public const string Data_Npc_TrashBear_Description = "data.npc.trash-bear.description";
    public const string Data_Type_Container = "data.type.container";
    public const string Data_Item_EggIncubator_Description = "data.item.egg-incubator.description";
    public const string Data_Item_Barrel_Name = "data.item.barrel.name";
    public const string Data_Item_Barrel_Description = "data.item.barrel.description";
    public const string Data_Item_Box_Name = "data.item.box.name";
    public const string Data_Item_Box_Description = "data.item.box.description";
    public const string Location_Beach_EastPier = "location.beach.east-pier";
    public const string Location_BugLand = "location.bugLand";
    public const string Location_Desert_TopPond = "location.desert.topPond";
    public const string Location_FarmCave = "location.farmCave";
    public const string Location_FarmHouse = "location.farmHouse";
    public const string Location_Forest_IslandTip = "location.forest.island-tip";
    public const string Location_Forest_Lake = "location.forest.lake";
    public const string Location_Forest_River = "location.forest.river";
    public const string Location_HarveyRoom = "location.HarveyRoom";
    public const string Location_LeoTreeHouse = "location.LeoTreeHouse";
    public const string Location_SandyHouse = "location.SandyHouse";
    public const string Location_SebastianRoom = "location.SebastianRoom";
    public const string Location_Submarine = "location.submarine";
    public const string Location_Town_NorthmostBridge = "location.town.northmost-bridge";
    public const string Location_UndergroundMine = "location.undergroundMine";
    public const string Location_UndergroundMine_Level = "location.undergroundMine.level";
    public const string Location_WitchSwamp = "location.witchSwamp";
    public const string Location_Caldera = "location.caldera";
    public const string Location_IslandEast = "location.islandEast";
    public const string Location_IslandNorth = "location.islandNorth";
    public const string Location_IslandSouth = "location.islandSouth";
    public const string Location_IslandSouthEast = "location.islandSouthEast";
    public const string Location_IslandSouthEastCave = "location.islandSouthEastCave";
    public const string Location_IslandWest = "location.islandWest";
    public const string Location_IslandWest_Freshwater = "location.islandWest.freshwater";
    public const string Location_IslandWest_Ocean = "location.islandWest.ocean";
    public const string Location_FishArea = "location.fish-area";
    public const string Location_UnknownFishArea = "location.unknown-fish-area";
    public const string ConditionsSummary = "conditions-summary";
    public const string ConditionOrContextTag_Negate = "condition-or-context-tag.negate";
    public const string Condition_DayOfMonth = "condition.day-of-month";
    public const string Condition_DayOfMonth_Even = "condition.day-of-month.even";
    public const string Condition_DayOfMonth_Odd = "condition.day-of-month.odd";
    public const string Condition_ItemType_Input = "condition.item-type.input";
    public const string Condition_ItemType_Target = "condition.item-type.target";
    public const string Condition_RawContextTag = "condition.raw-context-tag";
    public const string Condition_RawCondition = "condition.raw-condition";
    public const string Condition_ItemContextTag = "condition.item-context-tag";
    public const string Condition_ItemContextTags = "condition.item-context-tags";
    public const string Condition_ItemContextTag_Value = "condition.item-context-tag.value";
    public const string Condition_ItemEdibility_Edible = "condition.item-edibility.edible";
    public const string Condition_ItemEdibility_Min = "condition.item-edibility.min";
    public const string Condition_ItemEdibility_Range = "condition.item-edibility.range";
    public const string ContextTag_Bone = "context-tag.bone";
    public const string ContextTag_Egg = "context-tag.egg";
    public const string ContextTag_LargeEgg = "context-tag.large-egg";
    public const string ContextTag_PreservedItem = "context-tag.preserved-item";
    public const string Animal_Love = "animal.love";
    public const string Animal_Happiness = "animal.happiness";
    public const string Animal_Mood = "animal.mood";
    public const string Animal_Complaints = "animal.complaints";
    public const string Animal_ProduceReady = "animal.produce-ready";
    public const string Animal_Growth = "animal.growth";
    public const string Animal_SellsFor = "animal.sells-for";
    public const string Animal_Complaints_Hungry = "animal.complaints.hungry";
    public const string Animal_Complaints_LeftOut = "animal.complaints.left-out";
    public const string Animal_Complaints_NewHome = "animal.complaints.new-home";
    public const string Animal_Complaints_NoHeater = "animal.complaints.no-heater";
    public const string Animal_Complaints_NotPetted = "animal.complaints.not-petted";
    public const string Animal_Complaints_WildAnimalAttack = "animal.complaints.wild-animal-attack";
    public const string Building_Animals = "building.animals";
    public const string Building_Construction = "building.construction";
    public const string Building_ConstructionCosts = "building.construction-costs";
    public const string Building_FeedTrough = "building.feed-trough";
    public const string Building_Horse = "building.horse";
    public const string Building_HorseLocation = "building.horse-location";
    public const string Building_Owner = "building.owner";
    public const string Building_Slimes = "building.slimes";
    public const string Building_StoredHay = "building.stored-hay";
    public const string Building_Upgrades = "building.upgrades";
    public const string Building_WaterTrough = "building.water-trough";
    public const string Building_Animals_Summary = "building.animals.summary";
    public const string Building_Construction_Summary = "building.construction.summary";
    public const string Building_FeedTrough_Automated = "building.feed-trough.automated";
    public const string Building_FeedTrough_Summary = "building.feed-trough.summary";
    public const string Building_HorseLocation_Summary = "building.horse-location.summary";
    public const string Building_JunimoHarvestingEnabled = "building.junimo-harvesting-enabled";
    public const string Building_Owner_None = "building.owner.none";
    public const string Building_OutputProcessing = "building.output-processing";
    public const string Building_OutputReady = "building.output-ready";
    public const string Building_Slimes_Summary = "building.slimes.summary";
    public const string Building_StoredHay_Summary = "building.stored-hay.summary";
    public const string Building_Upgrades_Barn_0 = "building.upgrades.barn.0";
    public const string Building_Upgrades_Barn_1 = "building.upgrades.barn.1";
    public const string Building_Upgrades_Barn_2 = "building.upgrades.barn.2";
    public const string Building_Upgrades_Cabin_0 = "building.upgrades.cabin.0";
    public const string Building_Upgrades_Cabin_1 = "building.upgrades.cabin.1";
    public const string Building_Upgrades_Cabin_2 = "building.upgrades.cabin.2";
    public const string Building_Upgrades_Coop_0 = "building.upgrades.coop.0";
    public const string Building_Upgrades_Coop_1 = "building.upgrades.coop.1";
    public const string Building_Upgrades_Coop_2 = "building.upgrades.coop.2";
    public const string Building_WaterTrough_Summary = "building.water-trough.summary";
    public const string Building_FishPond_Population = "building.fish-pond.population";
    public const string Building_FishPond_Drops = "building.fish-pond.drops";
    public const string Building_FishPond_Quests = "building.fish-pond.quests";
    public const string Building_FishPond_Population_Empty = "building.fish-pond.population.empty";
    public const string Building_FishPond_Population_NextSpawn = "building.fish-pond.population.next-spawn";
    public const string Building_FishPond_Drops_Preface = "building.fish-pond.drops.preface";
    public const string Building_FishPond_Drops_MinFish = "building.fish-pond.drops.min-fish";
    public const string Building_FishPond_Quests_Done = "building.fish-pond.quests.done";
    public const string Building_FishPond_Quests_IncompleteOne = "building.fish-pond.quests.incomplete-one";
    public const string Building_FishPond_Quests_IncompleteRandom = "building.fish-pond.quests.incomplete-random";
    public const string Building_FishPond_Quests_Available = "building.fish-pond.quests.available";
    public const string Bush_Name_Berry = "bush.name.berry";
    public const string Bush_Name_Plain = "bush.name.plain";
    public const string Bush_Name_Tea = "bush.name.tea";
    public const string Bush_Description_Berry = "bush.description.berry";
    public const string Bush_Description_Plain = "bush.description.plain";
    public const string Bush_Description_Tea = "bush.description.Tea";
    public const string Bush_DatePlanted = "bush.date-planted";
    public const string Bush_Growth = "bush.growth";
    public const string Bush_NextHarvest = "bush.next-harvest";
    public const string Bush_Growth_Summary = "bush.growth.summary";
    public const string Bush_Schedule_Berry = "bush.schedule.berry";
    public const string Bush_Schedule_Tea = "bush.schedule.tea";
    public const string FruitTree_Complaints = "fruit-tree.complaints";
    public const string FruitTree_Name = "fruit-tree.name";
    public const string FruitTree_Growth = "fruit-tree.growth";
    public const string FruitTree_NextFruit = "fruit-tree.next-fruit";
    public const string FruitTree_Season = "fruit-tree.season";
    public const string FruitTree_Quality = "fruit-tree.quality";
    public const string FruitTree_Complaints_AdjacentObjects = "fruit-tree.complaints.adjacent-objects";
    public const string FruitTree_Growth_Summary = "fruit-tree.growth.summary";
    public const string FruitTree_NextFruit_MaxFruit = "fruit-tree.next-fruit.max-fruit";
    public const string FruitTree_NextFruit_OutOfSeason = "fruit-tree.next-fruit.out-of-season";
    public const string FruitTree_NextFruit_StruckByLightning = "fruit-tree.next-fruit.struck-by-lightning";
    public const string FruitTree_NextFruit_TooYoung = "fruit-tree.next-fruit.too-young";
    public const string FruitTree_Quality_Now = "fruit-tree.quality.now";
    public const string FruitTree_Quality_OnDate = "fruit-tree.quality.on-date";
    public const string FruitTree_Quality_OnDateNextYear = "fruit-tree.quality.on-date-next-year";
    public const string FruitTree_Season_Summary = "fruit-tree.season.summary";
    public const string Crop_Summary = "crop.summary";
    public const string Crop_Harvest = "crop.harvest";
    public const string Crop_Fertilized = "crop.fertilized";
    public const string Crop_Watered = "crop.watered";
    public const string Crop_Summary_Dead = "crop.summary.dead";
    public const string Crop_Summary_DropsX = "crop.summary.drops-X";
    public const string Crop_Summary_DropsXToY = "crop.summary.drops-X-to-Y";
    public const string Crop_Summary_HarvestOnce = "crop.summary.harvest-once";
    public const string Crop_Summary_HarvestMulti = "crop.summary.harvest-multi";
    public const string Crop_Summary_FarmingXp = "crop.summary.farming-xp";
    public const string Crop_Summary_ForagingXp = "crop.summary.foraging-xp";
    public const string Crop_Summary_Seasons = "crop.summary.seasons";
    public const string Crop_Summary_SellsFor = "crop.summary.sells-for";
    public const string Crop_Harvest_TooLate = "crop.harvest.too-late";
    public const string Item_Contents = "item.contents";
    public const string Item_LovesThis = "item.loves-this";
    public const string Item_LikesThis = "item.likes-this";
    public const string Item_NeutralAboutThis = "item.neutral-about-this";
    public const string Item_DislikesThis = "item.dislikes-this";
    public const string Item_HatesThis = "item.hates-this";
    public const string Item_NeededFor = "item.needed-for";
    public const string Item_NumberOwned = "item.number-owned";
    public const string Item_NumberCooked = "item.number-cooked";
    public const string Item_NumberCrafted = "item.number-crafted";
    public const string Item_Recipes = "item.recipes";
    public const string Item_SeeAlso = "item.see-also";
    public const string Item_SellsFor = "item.sells-for";
    public const string Item_SellsTo = "item.sells-to";
    public const string Item_CanBeDyed = "item.can-be-dyed";
    public const string Item_ProducesDye = "item.produces-dye";
    public const string Item_Contents_Placed = "item.contents.placed";
    public const string Item_Contents_Ready = "item.contents.ready";
    public const string Item_Contents_Partial = "item.contents.partial";
    public const string Item_UndiscoveredGiftTaste = "item.undiscovered-gift-taste";
    public const string Item_UndiscoveredGiftTasteAppended = "item.undiscovered-gift-taste-appended";
    public const string Item_NeededFor_CommunityCenter = "item.needed-for.community-center";
    public const string Item_NeededFor_FullShipment = "item.needed-for.full-shipment";
    public const string Item_NeededFor_Monoculture = "item.needed-for.monoculture";
    public const string Item_NeededFor_Polyculture = "item.needed-for.polyculture";
    public const string Item_NeededFor_FullCollection = "item.needed-for.full-collection";
    public const string Item_NeededFor_GourmetChef = "item.needed-for.gourmet-chef";
    public const string Item_NeededFor_CraftMaster = "item.needed-for.craft-master";
    public const string Item_NeededFor_Quests = "item.needed-for.quests";
    public const string Item_UnknownRecipes = "item.unknown-recipes";
    public const string Item_SellsTo_ShippingBox = "item.sells-to.shipping-box";
    public const string Item_RecipesForIngredient_Entry = "item.recipes-for-ingredient.entry";
    public const string Item_RecipesForMachine_MultipleItems = "item.recipes-for-machine.multiple-items";
    public const string Item_RecipesForMachine_SameAsInput = "item.recipes-for-machine.same-as-input";
    public const string Item_RecipesForMachine_TooComplex = "item.recipes-for-machine.too-complex";
    public const string Item_NumberOwned_Summary = "item.number-owned.summary";
    public const string Item_NumberOwnedFlavored_Summary = "item.number-owned-flavored.summary";
    public const string Item_NumberCrafted_Summary = "item.number-crafted.summary";
    public const string Item_CaskContents = "item.cask-contents";
    public const string Item_CaskSchedule = "item.cask-schedule";
    public const string Item_CaskSchedule_Now = "item.cask-schedule.now";
    public const string Item_CaskSchedule_NowPartial = "item.cask-schedule.now-partial";
    public const string Item_CaskSchedule_Tomorrow = "item.cask-schedule.tomorrow";
    public const string Item_CaskSchedule_InXDays = "item.cask-schedule.in-x-days";
    public const string Item_CrabpotBait = "item.crabpot-bait";
    public const string Item_CrabpotBaitNeeded = "item.crabpot-bait-needed";
    public const string Item_CrabpotBaitNotNeeded = "item.crabpot-bait-not-needed";
    public const string Item_FenceHealth = "item.fence-health";
    public const string Item_FenceHealth_GoldClock = "item.fence-health.gold-clock";
    public const string Item_FenceHealth_Summary = "item.fence-health.summary";
    public const string Item_FishPondDrops = "item.fish-pond-drops";
    public const string Item_FishSpawnRules = "item.fish-spawn-rules";
    public const string Item_FishSpawnRules_MinFishingLevel = "item.fish-spawn-rules.min-fishing-level";
    public const string Item_FishSpawnRules_NotCaughtYet = "item.fish-spawn-rules.not-caught-yet";
    public const string Item_FishSpawnRules_ExtendedFamilyQuestActive = "item.fish-spawn-rules.extended-family-quest-active";
    public const string Item_FishSpawnRules_Locations = "item.fish-spawn-rules.locations";
    public const string Item_FishSpawnRules_LocationsBySeason_Label = "item.fish-spawn-rules.locations-by-season.label";
    public const string Item_FishSpawnRules_LocationsBySeason_SeasonLocations = "item.fish-spawn-rules.locations-by-season.season-locations";
    public const string Item_FishSpawnRules_SeasonAny = "item.fish-spawn-rules.season-any";
    public const string Item_FishSpawnRules_SeasonList = "item.fish-spawn-rules.season-list";
    public const string Item_FishSpawnRules_Time = "item.fish-spawn-rules.time";
    public const string Item_FishSpawnRules_WeatherSunny = "item.fish-spawn-rules.weather-sunny";
    public const string Item_FishSpawnRules_WeatherRainy = "item.fish-spawn-rules.weather-rainy";
    public const string Item_UncaughtFish = "item.uncaught-fish";
    public const string Item_MeleeWeapon_Accuracy = "item.melee-weapon.accuracy";
    public const string Item_MeleeWeapon_CriticalChance = "item.melee-weapon.critical-chance";
    public const string Item_MeleeWeapon_CriticalDamage = "item.melee-weapon.critical-damage";
    public const string Item_MeleeWeapon_Damage = "item.melee-weapon.damage";
    public const string Item_MeleeWeapon_Defense = "item.melee-weapon.defense";
    public const string Item_MeleeWeapon_Knockback = "item.melee-weapon.knockback";
    public const string Item_MeleeWeapon_Reach = "item.melee-weapon.reach";
    public const string Item_MeleeWeapon_Speed = "item.melee-weapon.speed";
    public const string Item_MeleeWeapon_CriticalDamage_Label = "item.melee-weapon.critical-damage.label";
    public const string Item_MeleeWeapon_Defense_Label = "item.melee-weapon.defense.label";
    public const string Item_MeleeWeapon_Knockback_Label = "item.melee-weapon.knockback.label";
    public const string Item_MeleeWeapon_Reach_Label = "item.melee-weapon.reach.label";
    public const string Item_MeleeWeapon_Speed_ShownVsActual = "item.melee-weapon.speed.shown-vs-actual";
    public const string Item_MeleeWeapon_Speed_Summary = "item.melee-weapon.speed.summary";
    public const string Item_MovieSnackPreference = "item.movie-snack-preference";
    public const string Item_MovieTicket_MovieThisWeek = "item.movie-ticket.movie-this-week";
    public const string Item_MovieTicket_LovesMovie = "item.movie-ticket.loves-movie";
    public const string Item_MovieTicket_LikesMovie = "item.movie-ticket.likes-movie";
    public const string Item_MovieTicket_DislikesMovie = "item.movie-ticket.dislikes-movie";
    public const string Item_MovieTicket_RejectsMovie = "item.movie-ticket.rejects-movie";
    public const string Item_MovieSnackPreference_Love = "item.movie-snack-preference.love";
    public const string Item_MovieSnackPreference_Like = "item.movie-snack-preference.like";
    public const string Item_MovieSnackPreference_Dislike = "item.movie-snack-preference.dislike";
    public const string Item_MovieTicket_MovieThisWeek_None = "item.movie-ticket.movie-this-week.none";
    public const string Item_MusicBlock_Pitch = "item.music-block.pitch";
    public const string Item_MusicBlock_DrumType = "item.music-block.drum-type";
    public const string Monster_Invincible = "monster.invincible";
    public const string Monster_Health = "monster.health";
    public const string Monster_Drops = "monster.drops";
    public const string Monster_Experience = "monster.experience";
    public const string Monster_Defense = "monster.defense";
    public const string Monster_Attack = "monster.attack";
    public const string Monster_AdventureGuild = "monster.adventure-guild";
    public const string Monster_Drops_Nothing = "monster.drops.nothing";
    public const string Monster_AdventureGuild_EradicationGoal = "monster.adventure-guild.eradication-goal";
    public const string Npc_Birthday = "npc.birthday";
    public const string Npc_CanRomance = "npc.can-romance";
    public const string Npc_Friendship = "npc.friendship";
    public const string Npc_TalkedToday = "npc.talked-today";
    public const string Npc_GiftedToday = "npc.gifted-today";
    public const string Npc_GiftedThisWeek = "npc.gifted-this-week";
    public const string Npc_KissedToday = "npc.kissed-today";
    public const string Npc_HuggedToday = "npc.hugged-today";
    public const string Npc_LovesGifts = "npc.loves-gifts";
    public const string Npc_LikesGifts = "npc.likes-gifts";
    public const string Npc_NeutralGifts = "npc.neutral-gifts";
    public const string Npc_DislikesGifts = "npc.dislikes-gifts";
    public const string Npc_HatesGifts = "npc.hates-gifts";
    public const string Npc_Schedule = "npc.schedule";
    public const string Npc_CanRomance_Married = "npc.can-romance.married";
    public const string Npc_CanRomance_Housemate = "npc.can-romance.housemate";
    public const string Npc_Friendship_NotMet = "npc.friendship.not-met";
    public const string Npc_Friendship_NeedBouquet = "npc.friendship.need-bouquet";
    public const string Npc_Friendship_NeedPoints = "npc.friendship.need-points";
    public const string Npc_UndiscoveredGiftTaste = "npc.undiscovered-gift-taste";
    public const string Npc_UnownedGiftTaste = "npc.unowned-gift-taste";
    public const string Npc_Schedule_CurrentPosition = "npc.schedule.current-position";
    public const string Npc_Schedule_NoEntries = "npc.schedule.no-entries";
    public const string Npc_Schedule_NotFollowingSchedule = "npc.schedule.not-following-schedule";
    public const string Npc_Schedule_Entry = "npc.schedule.entry";
    public const string Npc_Schedule_Farmhand_UnknownPosition = "npc.schedule.farmhand.unknown-position";
    public const string Npc_Schedule_Farmhand_UnknownSchedule = "npc.schedule.farmhand.unknown-schedule";
    public const string Npc_Child_Age = "npc.child.age";
    public const string Npc_Child_Age_DescriptionPartial = "npc.child.age.description-partial";
    public const string Npc_Child_Age_DescriptionGrown = "npc.child.age.description-grown";
    public const string Npc_Child_Age_Newborn = "npc.child.age.newborn";
    public const string Npc_Child_Age_Baby = "npc.child.age.baby";
    public const string Npc_Child_Age_Crawler = "npc.child.age.crawler";
    public const string Npc_Child_Age_Toddler = "npc.child.age.toddler";
    public const string Pet_Love = "pet.love";
    public const string Pet_PettedToday = "pet.petted-today";
    public const string Pet_LastPetted = "pet.last-petted";
    public const string Pet_WaterBowl = "pet.water-bowl";
    public const string Pet_LastPetted_Yes = "pet.last-petted.yes";
    public const string Pet_LastPetted_DaysAgo = "pet.last-petted.days-ago";
    public const string Pet_LastPetted_Never = "pet.last-petted.never";
    public const string Pet_WaterBowl_Empty = "pet.water-bowl.empty";
    public const string Pet_WaterBowl_Filled = "pet.water-bowl.filled";
    public const string Player_FarmName = "player.farm-name";
    public const string Player_FarmMap = "player.farm-map";
    public const string Player_FavoriteThing = "player.favorite-thing";
    public const string Player_Gender = "player.gender";
    public const string Player_Housemate = "player.housemate";
    public const string Player_Spouse = "player.spouse";
    public const string Player_WatchedMovieThisWeek = "player.watched-movie-this-week";
    public const string Player_CombatSkill = "player.combat-skill";
    public const string Player_FarmingSkill = "player.farming-skill";
    public const string Player_FishingSkill = "player.fishing-skill";
    public const string Player_ForagingSkill = "player.foraging-skill";
    public const string Player_MiningSkill = "player.mining-skill";
    public const string Player_Luck = "player.luck";
    public const string Player_SaveFormat = "player.save-format";
    public const string Player_FarmMap_Custom = "player.farm-map.custom";
    public const string Player_Gender_Male = "player.gender.male";
    public const string Player_Gender_Female = "player.gender.female";
    public const string Player_Luck_Summary = "player.luck.summary";
    public const string Player_Skill_Progress = "player.skill.progress";
    public const string Player_Skill_ProgressLast = "player.skill.progress-last";
    public const string Puzzle_Solution = "puzzle.solution";
    public const string Puzzle_Solution_Solved = "puzzle.solution.solved";
    public const string Puzzle_Solution_Hidden = "puzzle.solution.hidden";
    public const string Puzzle_IslandCrystalCave_Title = "puzzle.island-crystal-cave.title";
    public const string Puzzle_IslandCrystalCave_CrystalId = "puzzle.island-crystal-cave.crystal-id";
    public const string Puzzle_IslandCrystalCave_Solution_NotActivated = "puzzle.island-crystal-cave.solution.not-activated";
    public const string Puzzle_IslandCrystalCave_Solution_Waiting = "puzzle.island-crystal-cave.solution.waiting";
    public const string Puzzle_IslandCrystalCave_Solution_Activated = "puzzle.island-crystal-cave.solution.activated";
    public const string Puzzle_IslandMermaid_Title = "puzzle.island-mermaid.title";
    public const string Puzzle_IslandMermaid_Solution_Intro = "puzzle.island-mermaid.solution.intro";
    public const string Puzzle_IslandShrine_Title = "puzzle.island-shrine.title";
    public const string Puzzle_IslandShrine_Solution = "puzzle.island-shrine.solution";
    public const string Puzzle_IslandShrine_Solution_East = "puzzle.island-shrine.solution.east";
    public const string Puzzle_IslandShrine_Solution_North = "puzzle.island-shrine.solution.north";
    public const string Puzzle_IslandShrine_Solution_South = "puzzle.island-shrine.solution.south";
    public const string Puzzle_IslandShrine_Solution_West = "puzzle.island-shrine.solution.west";
    public const string Tile_Title = "tile.title";
    public const string Tile_Description = "tile.description";
    public const string Tile_GameLocation = "tile.game-location";
    public const string Tile_MapName = "tile.map-name";
    public const string Tile_MapProperties = "tile.map-properties";
    public const string Tile_LayerTile = "tile.layer-tile";
    public const string Tile_LayerTileNone = "tile.layer-tile-none";
    public const string Tile_MapProperties_Value = "tile.map-properties.value";
    public const string Tile_LayerTile_Appearance = "tile.layer-tile.appearance";
    public const string Tile_LayerTile_BlendMode = "tile.layer-tile.blend-mode";
    public const string Tile_LayerTile_IndexProperty = "tile.layer-tile.index-property";
    public const string Tile_LayerTile_TileProperty = "tile.layer-tile.tile-property";
    public const string Tile_LayerTile_NoneHere = "tile.layer-tile.none-here";
    public const string TrashBearOrGourmand_ItemWanted = "trash-bear-or-gourmand.item-wanted";
    public const string TrashBearOrGourmand_QuestProgress = "trash-bear-or-gourmand.quest-progress";
    public const string Tree_Stage = "tree.stage";
    public const string Tree_NextGrowth = "tree.next-growth";
    public const string Tree_Seed = "tree.seed";
    public const string Tree_IsFertilized = "tree.is-fertilized";
    public const string Tree_Name_BigMushroom = "tree.name.big-mushroom";
    public const string Tree_Name_Mahogany = "tree.name.mahogany";
    public const string Tree_Name_Maple = "tree.name.maple";
    public const string Tree_Name_Oak = "tree.name.oak";
    public const string Tree_Name_Palm = "tree.name.palm";
    public const string Tree_Name_Pine = "tree.name.pine";
    public const string Tree_Name_Mossy = "tree.name.mossy";
    public const string Tree_Name_Mystic = "tree.name.mystic";
    public const string Tree_Name_Unknown = "tree.name.unknown";
    public const string Tree_Stage_Done = "tree.stage.done";
    public const string Tree_Stage_Partial = "tree.stage.partial";
    public const string Tree_NextGrowth_Winter = "tree.next-growth.winter";
    public const string Tree_NextGrowth_AdjacentTrees = "tree.next-growth.adjacent-trees";
    public const string Tree_NextGrowth_Chance = "tree.next-growth.chance";
    public const string Tree_Seed_NotReady = "tree.seed.not-ready";
    public const string Tree_Seed_ProbabilityDaily = "tree.seed.probability-daily";
    public const string Tree_Seed_ProbabilityOnChop = "tree.seed.probability-on-chop";
    public const string Tree_Stages_Seed = "tree.stages.seed";
    public const string Tree_Stages_Sprout = "tree.stages.sprout";
    public const string Tree_Stages_Sapling = "tree.stages.sapling";
    public const string Tree_Stages_Bush = "tree.stages.bush";
    public const string Tree_Stages_SmallTree = "tree.stages.smallTree";
    public const string Tree_Stages_Tree = "tree.stages.tree";
    public const string Config_Title_MainOptions = "config.title.main-options";
    public const string Config_ForceFullScreen_Name = "config.force-full-screen.name";
    public const string Config_ForceFullScreen_Desc = "config.force-full-screen.desc";
    public const string Config_Title_Progression = "config.title.progression";
    public const string Config_Progression_ShowUncaughtFishSpawnRules_Name = "config.progression.show-uncaught-fish-spawn-rules.name";
    public const string Config_Progression_ShowUncaughtFishSpawnRules_Desc = "config.progression.show-uncaught-fish-spawn-rules.desc";
    public const string Config_Progression_ShowUnknownGiftTastes_Name = "config.progression.show-unknown-gift-tastes.name";
    public const string Config_Progression_ShowUnknownGiftTastes_Desc = "config.progression.show-unknown-gift-tastes.desc";
    public const string Config_Progression_ShowUnknownRecipes_Name = "config.progression.show-unknown-recipes.name";
    public const string Config_Progression_ShowUnknownRecipes_Desc = "config.progression.show-unknown-recipes.desc";
    public const string Config_Progression_ShowPuzzleSolutions_Name = "config.progression.show-puzzle-solutions.name";
    public const string Config_Progression_ShowPuzzleSolutions_Desc = "config.progression.show-puzzle-solutions.desc";
    public const string Config_Title_GiftTastes = "config.title.gift-tastes";
    public const string Config_ShowGiftTastes_Loved_Name = "config.show-gift-tastes.loved.name";
    public const string Config_ShowGiftTastes_Loved_Desc = "config.show-gift-tastes.loved.desc";
    public const string Config_ShowGiftTastes_Liked_Name = "config.show-gift-tastes.liked.name";
    public const string Config_ShowGiftTastes_Liked_Desc = "config.show-gift-tastes.liked.desc";
    public const string Config_ShowGiftTastes_Neutral_Name = "config.show-gift-tastes.neutral.name";
    public const string Config_ShowGiftTastes_Neutral_Desc = "config.show-gift-tastes.neutral.desc";
    public const string Config_ShowGiftTastes_Disliked_Name = "config.show-gift-tastes.disliked.name";
    public const string Config_ShowGiftTastes_Disliked_Desc = "config.show-gift-tastes.disliked.desc";
    public const string Config_ShowGiftTastes_Hated_Name = "config.show-gift-tastes.hated.name";
    public const string Config_ShowGiftTastes_Hated_Desc = "config.show-gift-tastes.hated.desc";
    public const string Config_ShowUnownedGifts_Name = "config.show-unowned-gifts.name";
    public const string Config_ShowUnownedGifts_Desc = "config.show-unowned-gifts.desc";
    public const string Config_HighlightUnrevealedGiftTastes_Name = "config.highlight-unrevealed-gift-tastes.name";
    public const string Config_HighlightUnrevealedGiftTastes_Desc = "config.highlight-unrevealed-gift-tastes.desc";
    public const string Config_Title_CollapseFields = "config.title.collapse-fields";
    public const string Config_CollapseFields_Enabled_Name = "config.collapse-fields.enabled.name";
    public const string Config_CollapseFields_Enabled_Desc = "config.collapse-fields.enabled.desc";
    public const string Config_CollapseFields_BuildingRecipes_Name = "config.collapse-fields.building-recipes.name";
    public const string Config_CollapseFields_ItemRecipes_Name = "config.collapse-fields.item-recipes.name";
    public const string Config_CollapseFields_NpcGiftTastes_Name = "config.collapse-fields.npc-gift-tastes.name";
    public const string Config_CollapseFields_Any_Desc = "config.collapse-fields.any.desc";
    public const string Config_Title_AdvancedOptions = "config.title.advanced-options";
    public const string Config_TileLookups_Name = "config.tile-lookups.name";
    public const string Config_TileLookups_Desc = "config.tile-lookups.desc";
    public const string Config_DataMiningFields_Name = "config.data-mining-fields.name";
    public const string Config_DataMiningFields_Desc = "config.data-mining-fields.desc";
    public const string Config_ShowInvalidRecipes_Name = "config.show-invalid-recipes.name";
    public const string Config_ShowInvalidRecipes_Desc = "config.show-invalid-recipes.desc";
    public const string Config_TargetRedirection_Name = "config.target-redirection.name";
    public const string Config_TargetRedirection_Desc = "config.target-redirection.desc";
    public const string Config_ScrollAmount_Name = "config.scroll-amount.name";
    public const string Config_ScrollAmount_Desc = "config.scroll-amount.desc";
    public const string Config_Title_Controls = "config.title.controls";
    public const string Config_HideOnKeyUp_Name = "config.hide-on-key-up.name";
    public const string Config_HideOnKeyUp_Desc = "config.hide-on-key-up.desc";
    public const string Config_ToggleLookup_Name = "config.toggle-lookup.name";
    public const string Config_ToggleLookup_Desc = "config.toggle-lookup.desc";
    public const string Config_ToggleSearch_Name = "config.toggle-search.name";
    public const string Config_ToggleSearch_Desc = "config.toggle-search.desc";
    public const string Config_ScrollUp_Name = "config.scroll-up.name";
    public const string Config_ScrollUp_Desc = "config.scroll-up.desc";
    public const string Config_ScrollDown_Name = "config.scroll-down.name";
    public const string Config_ScrollDown_Desc = "config.scroll-down.desc";
    public const string Config_PageUp_Name = "config.page-up.name";
    public const string Config_PageUp_Desc = "config.page-up.desc";
    public const string Config_PageDown_Name = "config.page-down.name";
    public const string Config_PageDown_Desc = "config.page-down.desc";
    public const string Config_ToggleDebug_Name = "config.toggle-debug.name";
    public const string Config_ToggleDebug_Desc = "config.toggle-debug.desc";
    public const string Icon_ToggleSearch_Name = "icon.toggle-search.name";
    public const string Icon_ToggleSearch_Desc = "icon.toggle-search.desc";
  }
}
