// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.Common.Utilities.InvariantSet
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable enable
namespace Pathoschild.Stardew.Common.Utilities;

internal class InvariantSet : 
  IInvariantSet,
  IReadOnlySet<string>,
  IEnumerable<string>,
  IEnumerable,
  IReadOnlyCollection<string>
{
  private static readonly HashSet<string> EmptyHashSet = new HashSet<string>();
  private readonly HashSet<string> Set;

  public static IInvariantSet Empty { get; } = (IInvariantSet) new InvariantSet();

  public int Count => this.Set.Count;

  public InvariantSet() => this.Set = InvariantSet.EmptyHashSet;

  public InvariantSet(IEnumerable<string> values)
  {
    HashSet<string> stringSet;
    switch (values)
    {
      case InvariantSet invariantSet:
        stringSet = invariantSet.Set;
        break;
      case HashSet<string> collection:
        stringSet = collection.ToNonNullCaseInsensitive();
        break;
      case ICollection<string> strings:
        if (strings.Count == 0)
        {
          stringSet = InvariantSet.EmptyHashSet;
          break;
        }
        goto default;
      default:
        stringSet = this.CreateSet(values);
        break;
    }
    this.Set = stringSet;
  }

  public InvariantSet(params string[] values)
  {
    this.Set = this.CreateSet((IEnumerable<string>) values);
  }

  public IEnumerator<string> GetEnumerator() => (IEnumerator<string>) this.Set.GetEnumerator();

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.Set.GetEnumerator();

  public bool Contains(string item) => this.Set.Contains(item);

  public bool IsProperSubsetOf(IEnumerable<string> other) => this.Set.IsProperSubsetOf(other);

  public bool IsProperSupersetOf(IEnumerable<string> other) => this.Set.IsProperSupersetOf(other);

  public bool IsSubsetOf(IEnumerable<string> other) => this.Set.IsSubsetOf(other);

  public bool IsSupersetOf(IEnumerable<string> other) => this.Set.IsSupersetOf(other);

  public bool Overlaps(IEnumerable<string> other) => this.Set.Overlaps(other);

  public bool SetEquals(IEnumerable<string> other) => this.Set.SetEquals(other);

  public IInvariantSet GetWith(string other)
  {
    if (this.Count == 0)
      return (IInvariantSet) new InvariantSet(new string[1]
      {
        other
      });
    if (this.Contains(other))
      return (IInvariantSet) this;
    HashSet<string> set = this.CreateSet((IEnumerable<string>) this.Set);
    set.Add(other);
    return (IInvariantSet) new InvariantSet((IEnumerable<string>) set);
  }

  public IInvariantSet GetWith(ICollection<string> other)
  {
    if (other.Count == 0)
      return (IInvariantSet) this;
    if (this.Count == 0)
      return (IInvariantSet) new InvariantSet((IEnumerable<string>) this.CreateSet((IEnumerable<string>) other));
    bool flag = false;
    HashSet<string> set = this.CreateSet((IEnumerable<string>) this.Set);
    foreach (string str in (IEnumerable<string>) other)
      flag |= set.Add(str);
    return !flag ? (IInvariantSet) this : (IInvariantSet) new InvariantSet((IEnumerable<string>) set);
  }

  public IInvariantSet GetWithout(string other)
  {
    if (!this.Contains(other))
      return (IInvariantSet) this;
    HashSet<string> set = this.CreateSet((IEnumerable<string>) this);
    set.Remove(other);
    return (IInvariantSet) new InvariantSet((IEnumerable<string>) set);
  }

  public IInvariantSet GetWithout(IEnumerable<string> other)
  {
    HashSet<string> values = (HashSet<string>) null;
    foreach (string str in other)
    {
      if (values == null)
      {
        if (this.Contains(str))
          values = this.CreateSet((IEnumerable<string>) this);
        else
          continue;
      }
      values.Remove(str);
    }
    return values == null ? (IInvariantSet) this : (IInvariantSet) new InvariantSet((IEnumerable<string>) values);
  }

  private HashSet<string> CreateSet(IEnumerable<string> values)
  {
    return new HashSet<string>(values, (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
  }
}
