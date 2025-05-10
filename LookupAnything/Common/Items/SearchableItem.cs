// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.Common.Items.SearchableItem
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using StardewValley;
using System;

#nullable enable
namespace Pathoschild.Stardew.Common.Items;

internal class SearchableItem
{
  public string Type { get; }

  public Item Item { get; }

  public Func<Item> CreateItem { get; }

  public string Id { get; }

  public string QualifiedItemId { get; }

  public string Name => this.Item.Name;

  public string DisplayName => this.Item.DisplayName;

  public SearchableItem(string type, string id, Func<SearchableItem, Item> createItem)
  {
    SearchableItem searchableItem = this;
    this.Type = type;
    this.Id = id;
    this.QualifiedItemId = this.Type + this.Id;
    this.CreateItem = (Func<Item>) (() => createItem(searchableItem));
    this.Item = createItem(this);
  }

  public bool NameContains(string substring)
  {
    return this.Name.IndexOf(substring, StringComparison.OrdinalIgnoreCase) != -1 || this.DisplayName.IndexOf(substring, StringComparison.OrdinalIgnoreCase) != -1;
  }

  public bool NameEquivalentTo(string name)
  {
    return this.Name.Equals(name, StringComparison.OrdinalIgnoreCase) || this.DisplayName.Equals(name, StringComparison.OrdinalIgnoreCase);
  }
}
