// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.Common.Integrations.CustomBush.ICustomBushApi
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using StardewValley.TerrainFeatures;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

#nullable enable
namespace Pathoschild.Stardew.Common.Integrations.CustomBush;

public interface ICustomBushApi
{
  bool TryGetCustomBush(Bush bush, out ICustomBush? customBush);

  bool TryGetCustomBush(Bush bush, out ICustomBush? customBush, [NotNullWhen(true)] out string? id);

  bool TryGetDrops(string id, [NotNullWhen(true)] out IList<ICustomBushDrop>? drops);
}
