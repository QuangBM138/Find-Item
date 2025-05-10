// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.Models.RecipeIngredientModel
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Netcode;
using StardewValley;
using System;
using System.Collections.Generic;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework.Models;

internal class RecipeIngredientModel
{
  public RecipeType RecipeType { get; }

  public string? InputId { get; }

  public string[] InputContextTags { get; }

  public int Count { get; }

  public Object.PreserveType? PreserveType { get; }

  public string? PreservedItemId { get; }

  public RecipeIngredientModel(
    RecipeType recipeType,
    string? inputId,
    int count,
    string[]? inputContextTags = null,
    Object.PreserveType? preserveType = null,
    string? preservedItemId = null)
  {
    this.RecipeType = recipeType;
    this.InputId = inputId;
    this.InputContextTags = inputContextTags ?? Array.Empty<string>();
    this.Count = count;
    this.PreserveType = preserveType;
    this.PreservedItemId = preservedItemId;
  }

  public bool Matches(Item? item)
  {
    if (item == null || this.InputId == null && this.InputContextTags.Length == 0 || this.InputId != null && !(this.InputId == item.Category.ToString()) && !(this.InputId == item.ItemId) && !(this.InputId == item.QualifiedItemId) && (this.RecipeType != RecipeType.Crafting || !CraftingRecipe.ItemMatchesForCrafting(item, this.InputId)) || this.InputContextTags.Length != 0 && !ItemContextTagManager.DoAllTagsMatch((IList<string>) this.InputContextTags, item.GetContextTags()))
      return false;
    Object.PreserveType? preserveType;
    if (this.PreservedItemId == null)
    {
      preserveType = this.PreserveType;
      if (!preserveType.HasValue)
        goto label_9;
    }
    if (!(item is Object @object) || this.PreservedItemId != null && this.PreservedItemId != ((NetFieldBase<string, NetString>) @object.preservedParentSheetIndex).Value)
      return false;
    preserveType = this.PreserveType;
    if (preserveType.HasValue)
    {
      preserveType = this.PreserveType;
      Object.PreserveType? nullable = ((NetFieldBase<Object.PreserveType?, NetNullableEnum<Object.PreserveType>>) @object.preserve).Value;
      if (!(preserveType.GetValueOrDefault() == nullable.GetValueOrDefault() & preserveType.HasValue == nullable.HasValue))
        return false;
    }
label_9:
    return true;
  }
}
