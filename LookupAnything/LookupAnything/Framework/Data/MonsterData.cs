// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.Data.MonsterData
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework.Data;

internal record MonsterData(
  string Name,
  int Health,
  int DamageToFarmer,
  bool IsGlider,
  int Resilience,
  double Jitteriness,
  int MoveTowardsPlayerThreshold,
  int Speed,
  double MissChance,
  bool IsMineMonster,
  ItemDropData[] Drops)
;
