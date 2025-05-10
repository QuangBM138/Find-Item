// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.Common.Utilities.HumanSortComparer
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using System;
using System.Collections.Generic;

#nullable enable
namespace Pathoschild.Stardew.Common.Utilities;

internal class HumanSortComparer : IComparer<string?>
{
  private readonly IComparer<string?> AlphaComparer;
  public static readonly HumanSortComparer DefaultIgnoreCase = new HumanSortComparer(true);

  public HumanSortComparer(bool ignoreCase = false)
  {
    this.AlphaComparer = ignoreCase ? (IComparer<string>) StringComparer.OrdinalIgnoreCase : (IComparer<string>) StringComparer.Ordinal;
  }

  public int Compare(string? a, string? b)
  {
    if (string.IsNullOrEmpty(a) || string.IsNullOrEmpty(b))
      return this.AlphaComparer.Compare(a, b);
    int position1 = 0;
    int position2 = 0;
    int num;
    do
    {
      string raw1;
      long? numeric1;
      this.GetNextPart(a, ref position1, out raw1, out numeric1);
      string raw2;
      long? numeric2;
      this.GetNextPart(b, ref position2, out raw2, out numeric2);
      bool flag1 = numeric1.HasValue && numeric2.HasValue;
      if (raw1 == null && raw2 == null)
        return 0;
      if (raw1 == null)
        return -1;
      if (raw2 == null)
        return 1;
      if (flag1)
      {
        for (int index = 0; index < raw1.Length && index < raw2.Length; ++index)
        {
          bool flag2 = raw1[index] == '0';
          bool flag3 = raw2[index] == '0';
          if (!(flag2 & flag3))
          {
            if (flag2)
              return -1;
            if (flag3)
              return 1;
            break;
          }
        }
      }
      num = flag1 ? numeric1.Value.CompareTo(numeric2.Value) : this.AlphaComparer.Compare(raw1, raw2);
      if (num < 0)
        return -1;
    }
    while (num <= 0);
    return 1;
  }

  private void GetNextPart(string str, ref int position, out string? raw, out long? numeric)
  {
    if (position >= str.Length)
    {
      raw = (string) null;
      numeric = new long?();
    }
    else
    {
      int num = position;
      bool flag = char.IsNumber(str[num]);
      ++position;
      while (position < str.Length && char.IsNumber(str[position]) == flag)
        ++position;
      raw = str.Substring(num, position - num);
      long result;
      numeric = !flag || !long.TryParse(raw, out result) ? new long?() : new long?(result);
    }
  }
}
