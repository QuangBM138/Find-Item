// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.HumanReadableConditionParser
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using StardewValley;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework;

internal static class HumanReadableConditionParser
{
  public static string Format(string condition)
  {
    return HumanReadableConditionParser.Format(condition, (string) null) ?? I18n.Condition_RawCondition((object) condition);
  }

  [return: NotNullIfNotNull("defaultValue")]
  public static string? Format(string condition, string? defaultValue)
  {
    GameStateQuery.ParsedGameStateQuery[] parsedGameStateQueryArray = GameStateQuery.Parse(condition);
    if (parsedGameStateQueryArray.Length != 1)
      return defaultValue;
    GameStateQuery.ParsedGameStateQuery parsedGameStateQuery = parsedGameStateQueryArray[0];
    if (parsedGameStateQuery.Error != null || parsedGameStateQuery.Query.Length == 0 || string.IsNullOrWhiteSpace(parsedGameStateQuery.Query[0]))
      return defaultValue;
    string str = (string) null;
    switch (parsedGameStateQuery.Query[0].ToUpperInvariant().Trim())
    {
      case "DAY_OF_MONTH":
        str = HumanReadableConditionParser.ParseDayOfMonth(parsedGameStateQuery.Query);
        break;
      case "ITEM_CONTEXT_TAG":
        str = HumanReadableConditionParser.ParseItemContextTag(parsedGameStateQuery.Query);
        break;
      case "ITEM_EDIBILITY":
        str = HumanReadableConditionParser.ParseItemEdibility(parsedGameStateQuery.Query);
        break;
    }
    if (str == null)
      return defaultValue;
    return !parsedGameStateQuery.Negated ? str : I18n.ConditionOrContextTag_Negate((object) str);
  }

  private static string ParseDayOfMonth(string[] query)
  {
    string[] array = ((IEnumerable<string>) query).Skip<string>(1).ToArray<string>();
    for (int index = 0; index < array.Length; ++index)
    {
      string a = array[index];
      if (string.Equals(a, "even", StringComparison.OrdinalIgnoreCase))
        array[index] = I18n.Condition_DayOfMonth_Even();
      else if (string.Equals(a, "odd", StringComparison.OrdinalIgnoreCase))
        array[index] = I18n.Condition_DayOfMonth_Odd();
    }
    return I18n.Condition_DayOfMonth((object) I18n.List((IEnumerable<object>) array));
  }

  private static string? ParseItemContextTag(string[] query)
  {
    string translated;
    if (!HumanReadableConditionParser.TryTranslateItemTarget(ArgUtility.Get(query, 1, (string) null, true), out translated))
      return (string) null;
    string[] subsetOf = ArgUtility.GetSubsetOf<string>(query, 2, -1);
    for (int index = 0; index < subsetOf.Length; ++index)
      subsetOf[index] = I18n.Condition_ItemContextTag_Value((object) subsetOf[index]);
    string tags = I18n.List((IEnumerable<object>) subsetOf);
    return query.Length <= 3 ? I18n.Condition_ItemContextTag((object) translated, (object) tags) : I18n.Condition_ItemContextTags((object) translated, (object) tags);
  }

  private static string? ParseItemEdibility(string[] query)
  {
    string translated;
    if (!HumanReadableConditionParser.TryTranslateItemTarget(ArgUtility.Get(query, 1, (string) null, true), out translated))
      return (string) null;
    if (!ArgUtility.HasIndex<string>(query, 2))
      return I18n.Condition_ItemEdibility_Edible((object) translated);
    int min = ArgUtility.GetInt(query, 2, 0);
    int max = ArgUtility.GetInt(query, 3, int.MaxValue);
    return max == int.MaxValue ? I18n.Condition_ItemEdibility_Min((object) translated, (object) min) : I18n.Condition_ItemEdibility_Range((object) translated, (object) min, (object) max);
  }

  private static bool TryTranslateItemTarget(string? itemType, [NotNullWhen(true)] out string? translated)
  {
    if (string.Equals(itemType, "Input", StringComparison.OrdinalIgnoreCase))
    {
      translated = I18n.Condition_ItemType_Input();
      return true;
    }
    if (string.Equals(itemType, "Target", StringComparison.OrdinalIgnoreCase))
    {
      translated = I18n.Condition_ItemType_Target();
      return true;
    }
    translated = (string) null;
    return false;
  }
}
