// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.Fields.Models.RecipeEntry
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework.Fields.Models;

internal class RecipeEntry
{
  private readonly Lazy<string> UniqueKeyImpl;

  public string? Name { get; }

  public string Type { get; }

  public bool IsKnown { get; }

  public RecipeItemEntry[] Inputs { get; }

  public RecipeItemEntry Output { get; }

  public string? Conditions { get; }

  public string UniqueKey => this.UniqueKeyImpl.Value;

  public bool IsValid { get; }

  public RecipeEntry(
    string? name,
    string type,
    bool isKnown,
    RecipeItemEntry[] inputs,
    RecipeItemEntry output,
    string? conditions)
  {
    this.Name = name;
    this.Type = type;
    this.IsKnown = isKnown;
    this.Inputs = inputs;
    this.Output = output;
    this.Conditions = conditions;
    this.UniqueKeyImpl = new Lazy<string>((Func<string>) (() => RecipeEntry.GetUniqueKey(name, inputs, output)));
    this.IsValid = output.IsValid && ((IEnumerable<RecipeItemEntry>) inputs).All<RecipeItemEntry>((Func<RecipeItemEntry, bool>) (input => input.IsValid));
  }

  private static string GetUniqueKey(string? name, RecipeItemEntry[] inputs, RecipeItemEntry output)
  {
    // ISSUE: object of a compiler-generated type is created
    return string.Join(", ", ((IEnumerable<RecipeItemEntry>) inputs).Select<RecipeItemEntry, string>((Func<RecipeItemEntry, string>) (item => item.DisplayText)).OrderBy<string, string>((Func<string, string>) (item => item)).Concat<string>((IEnumerable<string>) new \u003C\u003Ez__ReadOnlyArray<string>(new string[2]
    {
      output.DisplayText,
      name
    })));
  }
}
