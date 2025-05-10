// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.Models.FishData.FishSpawnWeather
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using System;

#nullable disable
namespace Pathoschild.Stardew.LookupAnything.Framework.Models.FishData;

[Flags]
internal enum FishSpawnWeather
{
  Unknown = 0,
  Sunny = 1,
  Rainy = 2,
  Both = Rainy | Sunny, // 0x00000003
}
