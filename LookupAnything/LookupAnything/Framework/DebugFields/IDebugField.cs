// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.DebugFields.IDebugField
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using System.Diagnostics.CodeAnalysis;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework.DebugFields;

internal interface IDebugField
{
  string Label { get; }

  string? Value { get; }

  [MemberNotNullWhen(true, "Value")]
  bool HasValue { [MemberNotNullWhen(true, "Value")] get; }

  bool IsPinned { get; }

  string? OverrideCategory { get; set; }
}
