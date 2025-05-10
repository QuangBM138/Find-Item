// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.DebugFields.GenericDebugField
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework.DebugFields;

internal class GenericDebugField : IDebugField
{
  public string Label { get; protected set; }

  public string? Value { get; protected set; }

  [MemberNotNullWhen(true, "Value")]
  public bool HasValue { [MemberNotNullWhen(true, "Value")] get; [MemberNotNullWhen(true, "Value")] protected set; }

  public bool IsPinned { get; protected set; }

  public string? OverrideCategory { get; set; }

  public GenericDebugField(string label, string? value, bool? hasValue = null, bool pinned = false)
  {
    this.Label = label;
    this.Value = value;
    this.HasValue = ((int) hasValue ?? (!string.IsNullOrWhiteSpace(this.Value) ? 1 : 0)) != 0;
    this.IsPinned = pinned;
  }

  public GenericDebugField(string label, int value, bool? hasValue = null, bool pinned = false)
    : this(label, value.ToString((IFormatProvider) CultureInfo.InvariantCulture), hasValue, pinned)
  {
  }

  public GenericDebugField(string label, float value, bool? hasValue = null, bool pinned = false)
    : this(label, value.ToString((IFormatProvider) CultureInfo.InvariantCulture), hasValue, pinned)
  {
  }
}
