// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.Fields.Models.FishPondDrop
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Pathoschild.Stardew.Common;
using Pathoschild.Stardew.LookupAnything.Framework.Data;
using StardewValley;
using System.Runtime.CompilerServices;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework.Fields.Models;

internal record FishPondDrop : FishPondDropData
{
  public Item SampleItem { get; }

  public SpriteInfo? Sprite { get; }

  public bool IsUnlocked { get; }

  public FishPondDrop(FishPondDropData data, Item sampleItem, SpriteInfo? sprite, bool isUnlocked)
    : base(data.MinPopulation, data.ItemId, data.MinDrop, data.MaxDrop, data.Probability, data.Conditions)
  {
    this.SampleItem = sampleItem;
    this.Sprite = sprite;
    this.IsUnlocked = isUnlocked;
  }

  [CompilerGenerated]
  public sealed override bool Equals(FishPondDropData? other) => this.Equals((object) other);

  [CompilerGenerated]
  protected FishPondDrop(FishPondDrop original)
    : base((FishPondDropData) original)
  {
    this.SampleItem = original.SampleItem;
    this.Sprite = original.Sprite;
    this.IsUnlocked = original.IsUnlocked;
  }
}
