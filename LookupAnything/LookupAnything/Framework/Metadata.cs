// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.Metadata
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Pathoschild.Stardew.LookupAnything.Framework.Data;
using Pathoschild.Stardew.LookupAnything.Framework.Models.FishData;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework;

internal record Metadata(
  ConstantData Constants,
  ItemData[] Items,
  CharacterData[] Characters,
  ShopData[] Shops,
  Dictionary<string, FishSpawnData> CustomFishSpawnRules,
  HashSet<string> IgnoreFishingLocations,
  PuzzleSolutionsData PuzzleSolutions)
{
  public bool LooksValid()
  {
    return ((IEnumerable<object>) new object[7]
    {
      (object) this.Constants,
      (object) this.Items,
      (object) this.Characters,
      (object) this.Shops,
      (object) this.CustomFishSpawnRules,
      (object) this.IgnoreFishingLocations,
      (object) this.PuzzleSolutions
    }).All<object>((Func<object, bool>) (p => p != null));
  }

  public ItemData? GetObject(Item item, ObjectContext context)
  {
    return ((IEnumerable<ItemData>) this.Items).FirstOrDefault<ItemData>((Func<ItemData, bool>) (p => p.QualifiedId.Contains(item.QualifiedItemId) && p.Context.HasFlag((Enum) context)));
  }

  public CharacterData? GetCharacter(NPC character, SubjectType type)
  {
    CharacterData[] characters1 = this.Characters;
    CharacterData character1 = characters1 != null ? ((IEnumerable<CharacterData>) characters1).FirstOrDefault<CharacterData>((Func<CharacterData, bool>) (p => p.ID == $"{type}::{((Character) character).Name}")) : (CharacterData) null;
    if ((object) character1 != null)
      return character1;
    CharacterData[] characters2 = this.Characters;
    return characters2 == null ? (CharacterData) null : ((IEnumerable<CharacterData>) characters2).FirstOrDefault<CharacterData>((Func<CharacterData, bool>) (p => p.ID == type.ToString()));
  }
}
