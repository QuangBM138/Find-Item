// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.Fields.Models.RecipeByTypeGroup
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework.Fields.Models;

internal record RecipeByTypeGroup(string Type, RecipeEntry[] Recipes, float[] ColumnWidths)
{
  public string Type { get; init; } = Type;

  public RecipeEntry[] Recipes { get; init; } = Recipes;

  public float[] ColumnWidths { get; init; } = ColumnWidths;

  public float TotalColumnWidth { get; } = ((IEnumerable<float>) ColumnWidths).Sum<float>((Func<float, float>) (p => p));

  [CompilerGenerated]
  public void Deconstruct(out string Type, out RecipeEntry[] Recipes, out float[] ColumnWidths)
  {
    Type = this.Type;
    Recipes = this.Recipes;
    ColumnWidths = this.ColumnWidths;
  }
}
