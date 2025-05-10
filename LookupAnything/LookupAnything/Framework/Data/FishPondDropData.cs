// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.Data.FishPondDropData
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using System;
using System.Runtime.CompilerServices;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework.Data;

internal record FishPondDropData : ItemDropData
{
  public int MinPopulation { get; }

  public FishPondDropData(
    int minPopulation,
    string itemID,
    int minDrop,
    int maxDrop,
    float probability,
    string? conditions)
    : base(itemID, minDrop, maxDrop, probability, conditions)
  {
    this.MinPopulation = Math.Max(minPopulation, 1);
  }

  [CompilerGenerated]
  public sealed override bool Equals(ItemDropData? other) => this.Equals((object) other);

  [CompilerGenerated]
  protected FishPondDropData(FishPondDropData original)
    : base((ItemDropData) original)
  {
    this.MinPopulation = original.MinPopulation;
  }
}
