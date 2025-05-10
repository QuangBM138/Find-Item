// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.Common.Utilities.Cached`1
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using System;

#nullable enable
namespace Pathoschild.Stardew.Common.Utilities;

internal class Cached<TValue>
{
  private readonly Func<string> GetCacheKey;
  private readonly Func<TValue?, TValue> FetchNew;
  private string? LastCacheKey;
  private TValue? LastValue;

  public TValue Value
  {
    get
    {
      string str = this.GetCacheKey();
      if (str != this.LastCacheKey)
      {
        this.LastCacheKey = str;
        this.LastValue = this.FetchNew(this.LastValue);
      }
      return this.LastValue;
    }
  }

  public Cached(Func<string> getCacheKey, Func<TValue> fetchNew)
    : this(getCacheKey, (Func<TValue, TValue>) (_ => fetchNew()))
  {
  }

  public Cached(Func<string> getCacheKey, Func<TValue?, TValue> fetchNew)
  {
    this.GetCacheKey = getCacheKey;
    this.FetchNew = fetchNew;
  }
}
