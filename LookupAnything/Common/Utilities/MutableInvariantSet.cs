// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.Common.Utilities.MutableInvariantSet
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

#nullable enable
namespace Pathoschild.Stardew.Common.Utilities;

internal class MutableInvariantSet : 
  ISet<string>,
  ICollection<string>,
  IEnumerable<string>,
  IEnumerable
{
  private HashSet<string>? Set;
  private IInvariantSet? CachedImmutableSet;

  public int Count
  {
    get
    {
      HashSet<string> set = this.Set;
      return set == null ? 0 : __nonvirtual (set.Count);
    }
  }

  public bool IsReadOnly { get; private set; }

  [MemberNotNullWhen(false, "Set")]
  public bool IsEmpty
  {
    [MemberNotNullWhen(false, "Set")] get
    {
      int? count = this.Set?.Count;
      return !count.HasValue || count.GetValueOrDefault() == 0;
    }
  }

  public MutableInvariantSet()
  {
  }

  public MutableInvariantSet(IEnumerable<string> values) => this.Set = this.CreateSet(values);

  public IEnumerator<string> GetEnumerator()
  {
    return (IEnumerator<string>) this.Set?.GetEnumerator() ?? Enumerable.Empty<string>().GetEnumerator();
  }

  IEnumerator IEnumerable.GetEnumerator()
  {
    return (IEnumerator) this.Set?.GetEnumerator() ?? (IEnumerator) Enumerable.Empty<string>().GetEnumerator();
  }

  public bool IsProperSubsetOf(IEnumerable<string> other)
  {
    return this.IsEmpty && other is ICollection<string> strings ? strings.Count > 0 : this.EnsureSet().IsProperSubsetOf(other);
  }

  public bool IsProperSupersetOf(IEnumerable<string> other)
  {
    return !this.IsEmpty && this.Set.IsProperSupersetOf(other);
  }

  public bool IsSubsetOf(IEnumerable<string> other) => this.IsEmpty || this.Set.IsSubsetOf(other);

  public bool IsSupersetOf(IEnumerable<string> other)
  {
    return this.IsEmpty && other is ICollection<string> strings ? strings.Count == 0 : this.EnsureSet().IsSupersetOf(other);
  }

  public bool Overlaps(IEnumerable<string> other) => !this.IsEmpty && this.Set.Overlaps(other);

  public bool SetEquals(IEnumerable<string> other)
  {
    return this.IsEmpty && other is ICollection<string> strings ? strings.Count == 0 : this.EnsureSet().SetEquals(other);
  }

  public bool Contains(string item) => !this.IsEmpty && this.Set.Contains(item);

  public void CopyTo(string[] array, int arrayIndex)
  {
    if (this.IsEmpty)
      return;
    this.Set.CopyTo(array, arrayIndex);
  }

  void ICollection<string>.Add(string item)
  {
    this.AssertNotLocked();
    if (!this.EnsureSet().Add(item))
      return;
    this.ClearCache();
  }

  public bool Add(string item)
  {
    this.AssertNotLocked();
    if (!this.EnsureSet().Add(item))
      return false;
    this.ClearCache();
    return true;
  }

  public void Clear()
  {
    this.AssertNotLocked();
    if (this.IsEmpty)
      return;
    this.Set.Clear();
    this.ClearCache();
  }

  public bool Remove(string item)
  {
    this.AssertNotLocked();
    if (this.IsEmpty || !this.Set.Remove(item))
      return false;
    this.ClearCache();
    return true;
  }

  public void ExceptWith(IEnumerable<string> other)
  {
    this.AssertNotLocked();
    if (this.IsEmpty)
      return;
    int count = this.Count;
    this.EnsureSet().ExceptWith(other);
    this.ClearCacheUnlessCount(count);
  }

  public void IntersectWith(IEnumerable<string> other)
  {
    this.AssertNotLocked();
    if (this.IsEmpty)
      return;
    int count = this.Count;
    this.EnsureSet().IntersectWith(other);
    this.ClearCacheUnlessCount(count);
  }

  public void SymmetricExceptWith(IEnumerable<string> other)
  {
    this.AssertNotLocked();
    if (this.IsEmpty && other is ICollection<string> strings && strings.Count == 0)
      return;
    this.EnsureSet().SymmetricExceptWith(other);
    this.ClearCache();
  }

  public void UnionWith(IEnumerable<string> other)
  {
    this.AssertNotLocked();
    if (this.IsEmpty)
    {
      if (other is ICollection<string> strings && strings.Count == 0)
        return;
      this.Set = this.CreateSet(other);
      this.ClearCacheUnlessCount(0);
    }
    else
    {
      int count = this.Count;
      this.Set.UnionWith(other);
      this.ClearCacheUnlessCount(count);
    }
  }

  public IInvariantSet Lock()
  {
    this.IsReadOnly = true;
    return this.GetImmutable();
  }

  public IInvariantSet GetImmutable()
  {
    if (this.CachedImmutableSet == null)
    {
      int? count = this.Set?.Count;
      if (!count.HasValue || count.GetValueOrDefault() <= 0)
        this.CachedImmutableSet = InvariantSet.Empty;
      else if (this.IsReadOnly)
      {
        this.CachedImmutableSet = (IInvariantSet) new InvariantSet((IEnumerable<string>) this.Set);
      }
      else
      {
        HashSet<string> values = new HashSet<string>();
        foreach (string str in this.Set)
          values.Add(str);
        return (IInvariantSet) new InvariantSet((IEnumerable<string>) values);
      }
    }
    return this.CachedImmutableSet;
  }

  private void AssertNotLocked()
  {
    if (this.IsReadOnly)
      throw new NotSupportedException("This set is locked and doesn't allow further changes.");
  }

  [MemberNotNull("Set")]
  private HashSet<string> EnsureSet() => this.Set ?? (this.Set = this.CreateSet());

  private void ClearCache() => this.CachedImmutableSet = (IInvariantSet) null;

  private void ClearCacheUnlessCount(int count)
  {
    if (count == this.Count)
      return;
    this.CachedImmutableSet = (IInvariantSet) null;
  }

  private HashSet<string> CreateSet()
  {
    return new HashSet<string>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
  }

  private HashSet<string> CreateSet(IEnumerable<string> values)
  {
    return new HashSet<string>(values, (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
  }
}
