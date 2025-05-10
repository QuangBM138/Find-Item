// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.Common.CollectionExtensions
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace Pathoschild.Stardew.Common;

internal static class CollectionExtensions
{
  public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> values) where T : class
  {
    return values.Where<T>((Func<T, bool>) (p => (object) p != null));
  }

  public static HashSet<string> ToNonNullCaseInsensitive(this HashSet<string>? collection)
  {
    if (collection == null)
      return new HashSet<string>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
    return collection.Comparer != StringComparer.OrdinalIgnoreCase ? new HashSet<string>((IEnumerable<string>) collection, (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase) : collection;
  }

  public static Dictionary<string, TValue> ToNonNullCaseInsensitive<TValue>(
    this Dictionary<string, TValue>? collection)
  {
    if (collection == null)
      return new Dictionary<string, TValue>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
    return collection.Comparer != StringComparer.OrdinalIgnoreCase ? new Dictionary<string, TValue>((IDictionary<string, TValue>) collection, (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase) : collection;
  }
}
