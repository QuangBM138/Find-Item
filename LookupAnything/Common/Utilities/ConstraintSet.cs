// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.Common.Utilities.ConstraintSet`1
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using System.Collections.Generic;

#nullable enable
namespace Pathoschild.Stardew.Common.Utilities;

internal class ConstraintSet<T>
{
  public HashSet<T> RestrictToValues { get; }

  public HashSet<T> ExcludeValues { get; }

  public bool IsBounded => this.RestrictToValues.Count != 0;

  public bool IsInfinite => !this.IsBounded;

  public bool IsConstrained => this.RestrictToValues.Count != 0 || this.ExcludeValues.Count != 0;

  public ConstraintSet()
    : this((IEqualityComparer<T>) EqualityComparer<T>.Default)
  {
  }

  public ConstraintSet(IEqualityComparer<T> comparer)
  {
    this.RestrictToValues = new HashSet<T>(comparer);
    this.ExcludeValues = new HashSet<T>(comparer);
  }

  public bool AddBound(T value) => this.RestrictToValues.Add(value);

  public bool AddBound(IEnumerable<T> values)
  {
    return StardewValley.Extensions.CollectionExtensions.AddRange<T>((ISet<T>) this.RestrictToValues, values) > 0;
  }

  public bool Exclude(T value) => this.ExcludeValues.Add(value);

  public bool Exclude(IEnumerable<T> values)
  {
    return StardewValley.Extensions.CollectionExtensions.AddRange<T>((ISet<T>) this.ExcludeValues, values) > 0;
  }

  public bool Intersects(ConstraintSet<T> other)
  {
    if (this.IsInfinite && other.IsInfinite)
      return true;
    if (this.IsBounded)
    {
      foreach (T restrictToValue in this.RestrictToValues)
      {
        if (this.Allows(restrictToValue) && other.Allows(restrictToValue))
          return true;
      }
    }
    if (other.IsBounded)
    {
      foreach (T restrictToValue in other.RestrictToValues)
      {
        if (other.Allows(restrictToValue) && this.Allows(restrictToValue))
          return true;
      }
    }
    return false;
  }

  public bool Allows(T value)
  {
    if (this.ExcludeValues.Contains(value))
      return false;
    return this.IsInfinite || this.RestrictToValues.Contains(value);
  }
}
