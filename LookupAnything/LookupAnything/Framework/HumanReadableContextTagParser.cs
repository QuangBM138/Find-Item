// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.HumanReadableContextTagParser
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Pathoschild.Stardew.Common;
using StardewValley;
using StardewValley.ItemTypeDefinitions;
using System.Diagnostics.CodeAnalysis;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework;

internal static class HumanReadableContextTagParser
{
  public static string Format(string contextTag)
  {
    return HumanReadableContextTagParser.Format(contextTag, (string) null) ?? I18n.Condition_RawContextTag((object) contextTag);
  }

  [return: NotNullIfNotNull("defaultValue")]
  public static string? Format(string contextTag, string? defaultValue)
  {
    if (string.IsNullOrWhiteSpace(contextTag))
      return defaultValue;
    bool flag = contextTag.StartsWith('!');
    string str1;
    if (!flag)
    {
      str1 = contextTag;
    }
    else
    {
      string str2 = contextTag;
      str1 = str2.Substring(1, str2.Length - 1);
    }
    string tag = str1;
    string parsed;
    if (!HumanReadableContextTagParser.TryParseCategory(tag, out parsed) && !HumanReadableContextTagParser.TryParseItemId(tag, out parsed) && !HumanReadableContextTagParser.TryParsePreservedItemId(tag, out parsed) && !HumanReadableContextTagParser.TryParseSpecial(tag, out parsed))
      return defaultValue;
    return !flag ? parsed : I18n.ConditionOrContextTag_Negate((object) parsed);
  }

  private static bool TryParseCategory(string tag, [NotNullWhen(true)] out string? parsed)
  {
    int? nullable1;
    if (tag != null)
    {
      switch (tag.Length)
      {
        case 12:
          switch (tag[9])
          {
            case 'e':
              if (tag == "category_egg")
              {
                nullable1 = new int?(-5);
                goto label_85;
              }
              break;
            case 'g':
              if (tag == "category_gem")
              {
                nullable1 = new int?(-2);
                goto label_85;
              }
              break;
            case 'h':
              if (tag == "category_hat")
              {
                nullable1 = new int?(-95);
                goto label_85;
              }
              break;
          }
          break;
        case 13:
          switch (tag[9])
          {
            case 'b':
              if (tag == "category_bait")
              {
                nullable1 = new int?(-21);
                goto label_85;
              }
              break;
            case 'f':
              if (tag == "category_fish")
              {
                nullable1 = new int?(-4);
                goto label_85;
              }
              break;
            case 'j':
              if (tag == "category_junk")
              {
                nullable1 = new int?(-20);
                goto label_85;
              }
              break;
            case 'm':
              switch (tag)
              {
                case "category_meat":
                  nullable1 = new int?(-14);
                  goto label_85;
                case "category_milk":
                  nullable1 = new int?(-6);
                  goto label_85;
              }
              break;
            case 'r':
              if (tag == "category_ring")
              {
                nullable1 = new int?(-96);
                goto label_85;
              }
              break;
            case 't':
              if (tag == "category_tool")
              {
                nullable1 = new int?(-99);
                goto label_85;
              }
              break;
          }
          break;
        case 14:
          switch (tag[10])
          {
            case 'e':
              if (tag == "category_seeds")
              {
                nullable1 = new int?(-74);
                goto label_85;
              }
              break;
            case 'o':
              if (tag == "category_boots")
              {
                nullable1 = new int?(-97);
                goto label_85;
              }
              break;
            case 'y':
              if (tag == "category_syrup")
              {
                nullable1 = new int?(-27);
                goto label_85;
              }
              break;
          }
          break;
        case 15:
          switch (tag[9])
          {
            case 'f':
              if (tag == "category_fruits")
              {
                nullable1 = new int?(-79);
                goto label_85;
              }
              break;
            case 'g':
              if (tag == "category_greens")
              {
                nullable1 = new int?(-81);
                goto label_85;
              }
              break;
            case 'l':
              if (tag == "category_litter")
              {
                nullable1 = new int?(-999);
                goto label_85;
              }
              break;
            case 't':
              if (tag == "category_tackle")
              {
                nullable1 = new int?(-22);
                goto label_85;
              }
              break;
            case 'w':
              if (tag == "category_weapon")
              {
                nullable1 = new int?(-98);
                goto label_85;
              }
              break;
          }
          break;
        case 16 /*0x10*/:
          switch (tag[9])
          {
            case 'c':
              if (tag == "category_cooking")
              {
                nullable1 = new int?(-7);
                goto label_85;
              }
              break;
            case 'f':
              if (tag == "category_flowers")
              {
                nullable1 = new int?(-80);
                goto label_85;
              }
              break;
            case 't':
              if (tag == "category_trinket")
              {
                nullable1 = new int?(-101);
                goto label_85;
              }
              break;
          }
          break;
        case 17:
          switch (tag[10])
          {
            case 'i':
              if (tag == "category_minerals")
              {
                nullable1 = new int?(-12);
                goto label_85;
              }
              break;
            case 'l':
              if (tag == "category_clothing")
              {
                nullable1 = new int?(-100);
                goto label_85;
              }
              break;
            case 'r':
              if (tag == "category_crafting")
              {
                nullable1 = new int?(-8);
                goto label_85;
              }
              break;
          }
          break;
        case 18:
          switch (tag[9])
          {
            case 'e':
              if (tag == "category_equipment")
              {
                nullable1 = new int?(-29);
                goto label_85;
              }
              break;
            case 'f':
              if (tag == "category_furniture")
              {
                nullable1 = new int?(-24);
                goto label_85;
              }
              break;
            case 'v':
              if (tag == "category_vegetable")
              {
                nullable1 = new int?(-75);
                goto label_85;
              }
              break;
          }
          break;
        case 19:
          if (tag == "category_fertilizer")
          {
            nullable1 = new int?(-19);
            goto label_85;
          }
          break;
        case 20:
          if (tag == "category_ingredients")
          {
            nullable1 = new int?(-25);
            goto label_85;
          }
          break;
        case 21:
          if (tag == "category_monster_loot")
          {
            nullable1 = new int?(-28);
            goto label_85;
          }
          break;
        case 22:
          switch (tag[9])
          {
            case 'a':
              if (tag == "category_artisan_goods")
              {
                nullable1 = new int?(-26);
                goto label_85;
              }
              break;
            case 'b':
              if (tag == "category_big_craftable")
              {
                nullable1 = new int?(-9);
                goto label_85;
              }
              break;
          }
          break;
        case 24:
          switch (tag[9])
          {
            case 'm':
              if (tag == "category_metal_resources")
              {
                nullable1 = new int?(-15);
                goto label_85;
              }
              break;
            case 's':
              if (tag == "category_sell_at_pierres")
              {
                nullable1 = new int?(-17);
                goto label_85;
              }
              break;
          }
          break;
        case 26:
          if (tag == "category_sell_at_fish_shop")
          {
            nullable1 = new int?(-23);
            goto label_85;
          }
          break;
        case 27:
          if (tag == "category_building_resources")
          {
            nullable1 = new int?(-16);
            goto label_85;
          }
          break;
        case 36:
          if (tag == "category_sell_at_pierres_and_marnies")
          {
            nullable1 = new int?(-18);
            goto label_85;
          }
          break;
      }
    }
    nullable1 = new int?();
label_85:
    int? nullable2 = nullable1;
    if (nullable2.HasValue)
    {
      string categoryDisplayName = Object.GetCategoryDisplayName(nullable2.Value);
      if (!string.IsNullOrWhiteSpace(categoryDisplayName))
      {
        parsed = categoryDisplayName;
        return true;
      }
    }
    parsed = (string) null;
    return false;
  }

  private static bool TryParseItemId(string tag, [NotNullWhen(true)] out string? parsed)
  {
    ParsedItemData data;
    if (MachineDataHelper.TryGetUniqueItemFromContextTag(tag, out data))
    {
      string displayName = data.DisplayName;
      if (!string.IsNullOrWhiteSpace(displayName))
      {
        parsed = displayName;
        return true;
      }
    }
    parsed = (string) null;
    return false;
  }

  private static bool TryParsePreservedItemId(string tag, [NotNullWhen(true)] out string? parsed)
  {
    if (tag.StartsWith("preserve_sheet_index_"))
    {
      string str = tag;
      string displayName = ItemRegistry.GetData(str.Substring(21, str.Length - 21))?.DisplayName;
      if (!string.IsNullOrWhiteSpace(displayName))
      {
        parsed = I18n.ContextTag_PreservedItem((object) displayName);
        return true;
      }
    }
    parsed = (string) null;
    return false;
  }

  private static bool TryParseSpecial(string tag, [NotNullWhen(true)] out string? parsed)
  {
    string str;
    switch (tag)
    {
      case "bone_item":
        str = I18n.ContextTag_Bone();
        break;
      case "egg_item":
        str = I18n.ContextTag_Egg();
        break;
      case "large_egg_item":
        str = I18n.ContextTag_LargeEgg();
        break;
      default:
        str = (string) null;
        break;
    }
    parsed = str;
    return parsed != null;
  }
}
