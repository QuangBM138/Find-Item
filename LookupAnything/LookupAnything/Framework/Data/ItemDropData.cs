// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.Data.ItemDropData
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework.Data;

internal record ItemDropData(
  string ItemId,
  int MinDrop,
  int MaxDrop,
  float Probability,
  string? Conditions = null)
;
