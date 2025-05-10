// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.Common.Integrations.ProducerFrameworkMod.ProducerFrameworkRecipe
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace Pathoschild.Stardew.Common.Integrations.ProducerFrameworkMod;

internal class ProducerFrameworkRecipe
{
  public string? InputId { get; }

  public string MachineId { get; }

  public ProducerFrameworkIngredient[] Ingredients { get; }

  public string?[] ExceptIngredients { get; }

  public string OutputId { get; }

  public int MinOutput { get; }

  public int MaxOutput { get; }

  public double OutputChance { get; }

  public Object.PreserveType? PreserveType { get; }

  public ProducerFrameworkRecipe(
    string? inputId,
    string machineId,
    ProducerFrameworkIngredient[] ingredients,
    string?[] exceptIngredients,
    string outputId,
    int minOutput,
    int maxOutput,
    double outputChance,
    Object.PreserveType? preserveType)
  {
    this.InputId = inputId;
    this.MachineId = machineId;
    this.Ingredients = ingredients;
    this.ExceptIngredients = exceptIngredients;
    this.OutputId = outputId;
    this.MinOutput = minOutput;
    this.MaxOutput = maxOutput;
    this.OutputChance = outputChance;
    this.PreserveType = preserveType;
  }

  public bool HasContextTags()
  {
    return this.InputId == null || ((IEnumerable<ProducerFrameworkIngredient>) this.Ingredients).Any<ProducerFrameworkIngredient>((Func<ProducerFrameworkIngredient, bool>) (p => p.InputId == null)) || ((IEnumerable<string>) this.ExceptIngredients).Any<string>((Func<string, bool>) (p => p == null));
  }
}
