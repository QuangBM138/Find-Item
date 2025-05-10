// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.Common.MachineDataHelper
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using StardewValley;
using StardewValley.Buildings;
using StardewValley.Extensions;
using StardewValley.GameData.Buildings;
using StardewValley.ItemTypeDefinitions;
using StardewValley.Objects;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

#nullable enable
namespace Pathoschild.Stardew.Common;

public static class MachineDataHelper
{
  public static void GetBuildingChestNames(
    BuildingData? data,
    ISet<string> inputChests,
    ISet<string> outputChests)
  {
    int? count = data?.ItemConversions?.Count;
    if (!count.HasValue || count.GetValueOrDefault() <= 0)
      return;
    foreach (BuildingItemConversion itemConversion in data.ItemConversions)
    {
      if (itemConversion?.SourceChest != null)
        inputChests.Add(itemConversion.SourceChest);
      if (itemConversion?.DestinationChest != null)
        outputChests.Add(itemConversion.DestinationChest);
    }
  }

  public static bool TryGetBuildingChestNames(
    BuildingData? data,
    out ISet<string> inputChests,
    out ISet<string> outputChests)
  {
    inputChests = (ISet<string>) new HashSet<string>();
    outputChests = (ISet<string>) new HashSet<string>();
    MachineDataHelper.GetBuildingChestNames(data, inputChests, outputChests);
    return inputChests.Count > 0 || outputChests.Count > 0;
  }

  public static IEnumerable<Chest> GetBuildingChests(Building building, ISet<string> chestNames)
  {
    foreach (Chest buildingChest in building.buildingChests)
    {
      if (chestNames.Contains(((Item) buildingChest).Name))
        yield return buildingChest;
    }
  }

  public static bool TryGetUniqueItemFromContextTag(string contextTag, [NotNullWhen(true)] out ParsedItemData? data)
  {
    if (contextTag.StartsWith("id_"))
    {
      string str1 = contextTag;
      string str2 = str1.Substring(3, str1.Length - 3);
      string str3 = (string) null;
      if (str2.StartsWith('('))
      {
        str3 = str2;
      }
      else
      {
        string[] strArray = str2.Split('_', 2);
        foreach (IItemDataDefinition itemType in ItemRegistry.ItemTypes)
        {
          if (string.Equals(strArray[0], itemType.StandardDescriptor, StringComparison.InvariantCultureIgnoreCase))
          {
            str3 = itemType.Identifier + strArray[1];
            break;
          }
        }
      }
      data = ItemRegistry.GetData(str3);
      return data != null;
    }
    data = (ParsedItemData) null;
    return false;
  }

  public static bool TryGetUniqueItemFromGameStateQuery(string query, [NotNullWhen(true)] out ParsedItemData? data)
  {
    data = (ParsedItemData) null;
    foreach (GameStateQuery.ParsedGameStateQuery parsedGameStateQuery in GameStateQuery.Parse(query))
    {
      if (parsedGameStateQuery.Error == null)
      {
        string str1 = ArgUtility.Get(parsedGameStateQuery.Query, 0, (string) null, true);
        if (StringExtensions.EqualsIgnoreCase(str1, "ITEM_ID"))
        {
          if (parsedGameStateQuery.Query.Length == 3 && StringExtensions.EqualsIgnoreCase(parsedGameStateQuery.Query[1], "Input"))
          {
            string str2 = ItemRegistry.QualifyItemId(parsedGameStateQuery.Query[2]);
            if (data == null)
              data = ItemRegistry.GetData(str2);
            else if (data.QualifiedItemId != str2)
            {
              data = (ParsedItemData) null;
              return false;
            }
          }
        }
        else
        {
          ParsedItemData data1;
          if (StringExtensions.EqualsIgnoreCase(str1, "ITEM_CONTEXT_TAG") && parsedGameStateQuery.Query.Length == 3 && StringExtensions.EqualsIgnoreCase(parsedGameStateQuery.Query[1], "Input") && MachineDataHelper.TryGetUniqueItemFromContextTag(parsedGameStateQuery.Query[2], out data1))
          {
            if (data == null)
              data = data1;
            else if (data.QualifiedItemId != data1.QualifiedItemId)
            {
              data = (ParsedItemData) null;
              return false;
            }
          }
        }
      }
    }
    return data != null;
  }
}
