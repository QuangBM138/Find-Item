// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.Common.DataParsers.CropDataParser
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Netcode;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.GameData.Crops;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace Pathoschild.Stardew.Common.DataParsers;

internal class CropDataParser
{
  public Crop? Crop { get; }

  public CropData? CropData { get; }

  public Season[] Seasons { get; }

  public int HarvestablePhase { get; }

  public int DaysToFirstHarvest { get; }

  public int DaysToSubsequentHarvest { get; }

  public bool HasMultipleHarvests { get; }

  public bool CanHarvestNow { get; }

  public CropDataParser(Crop? crop, bool isPlanted)
  {
    this.Crop = crop;
    this.CropData = crop?.GetData();
    CropData cropData = this.CropData;
    if (cropData != null)
    {
      this.Seasons = cropData.Seasons.ToArray();
      this.HasMultipleHarvests = crop.RegrowsAfterHarvest();
      this.HarvestablePhase = ((NetList<int, NetInt>) crop.phaseDays).Count - 1;
      this.CanHarvestNow = ((NetFieldBase<int, NetInt>) crop.currentPhase).Value >= this.HarvestablePhase && (!((NetFieldBase<bool, NetBool>) crop.fullyGrown).Value || ((NetFieldBase<int, NetInt>) crop.dayOfCurrentPhase).Value <= 0);
      this.DaysToFirstHarvest = ((IEnumerable<int>) crop.phaseDays).Take<int>(((NetList<int, NetInt>) crop.phaseDays).Count - 1).Sum();
      this.DaysToSubsequentHarvest = cropData.RegrowDays;
      if (isPlanted || !((NetHashSet<int>) Game1.player.professions).Contains(5))
        return;
      this.DaysToFirstHarvest = (int) ((double) this.DaysToFirstHarvest * 0.9);
    }
    else
      this.Seasons = Array.Empty<Season>();
  }

  public SDate GetNextHarvest()
  {
    Crop crop = this.Crop;
    if (crop == null)
      throw new InvalidOperationException("Can't get the harvest date because there's no crop.");
    CropData cropData = this.CropData;
    if (cropData == null)
      throw new InvalidOperationException("Can't get the harvest date because the crop has no data.");
    if (this.CanHarvestNow)
      return SDate.Now();
    if (!((NetFieldBase<bool, NetBool>) crop.fullyGrown).Value)
    {
      int num = this.DaysToFirstHarvest - ((NetFieldBase<int, NetInt>) crop.dayOfCurrentPhase).Value - ((IEnumerable<int>) crop.phaseDays).Take<int>(((NetFieldBase<int, NetInt>) crop.currentPhase).Value).Sum();
      return SDate.Now().AddDays(num);
    }
    return ((NetFieldBase<int, NetInt>) crop.dayOfCurrentPhase).Value >= cropData.RegrowDays ? SDate.Now().AddDays(cropData.RegrowDays) : SDate.Now().AddDays(((NetFieldBase<int, NetInt>) crop.dayOfCurrentPhase).Value);
  }

  public Item GetSampleDrop()
  {
    if (this.Crop == null)
      throw new InvalidOperationException("Can't get a sample drop because there's no crop.");
    return ItemRegistry.Create(((NetFieldBase<string, NetString>) this.Crop.indexOfHarvest).Value, 1, 0, false);
  }
}
