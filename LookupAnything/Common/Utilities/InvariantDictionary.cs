// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.Common.Utilities.InvariantDictionary`1
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace Pathoschild.Stardew.Common.Utilities;

internal class InvariantDictionary<TValue> : Dictionary<string, TValue>
{
  public InvariantDictionary()
    : base((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase)
  {
  }

  public InvariantDictionary(IDictionary<string, TValue> values)
    : base(values, (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase)
  {
  }

  public InvariantDictionary(IEnumerable<KeyValuePair<string, TValue>> values)
    : base((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase)
  {
    foreach (KeyValuePair<string, TValue> keyValuePair in values)
      this.Add(keyValuePair.Key, keyValuePair.Value);
  }

  public InvariantDictionary<TValue> Clone(Func<TValue, TValue>? cloneValue = null)
  {
    return cloneValue == null ? new InvariantDictionary<TValue>((IDictionary<string, TValue>) this) : new InvariantDictionary<TValue>((IDictionary<string, TValue>) this.ToDictionary<KeyValuePair<string, TValue>, string, TValue>((Func<KeyValuePair<string, TValue>, string>) (p => p.Key), (Func<KeyValuePair<string, TValue>, TValue>) (p => cloneValue(p.Value))));
  }
}
