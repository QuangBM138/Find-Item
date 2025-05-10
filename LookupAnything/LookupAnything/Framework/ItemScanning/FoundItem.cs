// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.ItemScanning.FoundItem
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using StardewValley;
using System;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework.ItemScanning;

public class FoundItem
{
  public object? Parent { get; }

  public Item Item { get; }

  public bool IsInInventory { get; }

  public FoundItem(Item item, object? parent, bool isInInventory)
  {
    this.Item = item;
    this.Parent = parent;
    this.IsInInventory = isInInventory;
  }

  public int GetCount()
  {
    int count = Math.Max(1, this.Item.Stack);
    if (this.Parent is Fence && this.Item is Torch && count == 93)
      count = 1;
    return count;
  }
}
