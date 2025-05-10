// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.Common.Integrations.BushBloomMod.IBushBloomModApi
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Microsoft.Xna.Framework;
using StardewValley;

#nullable enable
namespace Pathoschild.Stardew.Common.Integrations.BushBloomMod;

public interface IBushBloomModApi
{
  (string, WorldDate, WorldDate)[] GetActiveSchedules(
    string season,
    int dayofMonth,
    int? year = null,
    GameLocation? location = null,
    Vector2? tile = null);

  bool IsReady();
}
