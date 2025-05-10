// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.Common.ListExtensions
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

#nullable enable
namespace Pathoschild.Stardew.Common;

internal static class ListExtensions
{
  public static bool IsInRange<T>(this IList<T> list, int index)
  {
    return index >= 0 && index < list.Count;
  }

  public static bool TryGetIndex<T>(this IList<T> list, int index, [NotNullWhen(true)] out T? value)
  {
    if (!list.IsInRange<T>(index))
    {
      value = default (T);
      return false;
    }
    value = list[index];
    return true;
  }
}
