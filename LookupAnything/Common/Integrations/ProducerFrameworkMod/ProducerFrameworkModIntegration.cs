// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.Common.Integrations.ProducerFrameworkMod.ProducerFrameworkModIntegration
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using StardewModdingAPI;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace Pathoschild.Stardew.Common.Integrations.ProducerFrameworkMod;

internal class ProducerFrameworkModIntegration(IModRegistry modRegistry, IMonitor monitor) : 
  BaseIntegration<IProducerFrameworkModApi>("Producer Framework Mod", "Digus.ProducerFrameworkMod", "1.9.3", modRegistry, monitor)
{
  private bool LoggedInvalidRecipeError;

  public IEnumerable<ProducerFrameworkRecipe> GetRecipes()
  {
    this.AssertLoaded();
    return this.ReadRecipes((IEnumerable<IDictionary<string, object>>) this.ModApi.GetRecipes());
  }

  public IEnumerable<ProducerFrameworkRecipe> GetRecipes(Object machine)
  {
    this.AssertLoaded();
    return this.ReadRecipes((IEnumerable<IDictionary<string, object>>) this.ModApi.GetRecipes(machine));
  }

  private IEnumerable<ProducerFrameworkRecipe> ReadRecipes(
    IEnumerable<IDictionary<string, object?>> raw)
  {
    return raw.Select<IDictionary<string, object>, ProducerFrameworkRecipe>(new Func<IDictionary<string, object>, ProducerFrameworkRecipe>(this.ReadRecipe)).WhereNotNull<ProducerFrameworkRecipe>();
  }

  private ProducerFrameworkRecipe? ReadRecipe(IDictionary<string, object?> raw)
  {
    try
    {
      string str1 = (string) raw["InputKey"];
      string str2 = (string) raw["MachineID"];
      ProducerFrameworkIngredient[] array1 = ((IEnumerable<Dictionary<string, object>>) raw["Ingredients"]).Select<Dictionary<string, object>, ProducerFrameworkIngredient>(new Func<Dictionary<string, object>, ProducerFrameworkIngredient>(this.ReadIngredient)).ToArray<ProducerFrameworkIngredient>();
      string[] array2 = ((IEnumerable<Dictionary<string, object>>) raw["ExceptIngredients"]).Select<Dictionary<string, object>, ProducerFrameworkIngredient>(new Func<Dictionary<string, object>, ProducerFrameworkIngredient>(this.ReadIngredient)).Select<ProducerFrameworkIngredient, string>((Func<ProducerFrameworkIngredient, string>) (p => p.InputId)).ToArray<string>();
      string str3 = (string) raw["Output"];
      int num1 = (int) raw["MinOutput"];
      int num2 = (int) raw["MaxOutput"];
      Object.PreserveType? nullable1 = (Object.PreserveType?) raw["PreserveType"];
      double num3 = (double) raw["OutputChance"];
      string inputId = str1;
      string machineId = str2;
      ProducerFrameworkIngredient[] ingredients = array1;
      string[] exceptIngredients = array2;
      string outputId = str3;
      int minOutput = num1;
      int maxOutput = num2;
      Object.PreserveType? nullable2 = nullable1;
      double outputChance = num3;
      Object.PreserveType? preserveType = nullable2;
      return new ProducerFrameworkRecipe(inputId, machineId, ingredients, exceptIngredients, outputId, minOutput, maxOutput, outputChance, preserveType);
    }
    catch (Exception ex)
    {
      if (!this.LoggedInvalidRecipeError)
      {
        this.LoggedInvalidRecipeError = true;
        this.Monitor.Log("Failed to load some recipes from Producer Framework Mod. Some custom machines may not appear in lookups.", (LogLevel) 3);
        this.Monitor.Log(ex.ToString(), (LogLevel) 0);
      }
      return (ProducerFrameworkRecipe) null;
    }
  }

  private ProducerFrameworkIngredient ReadIngredient(IDictionary<string, object?> raw)
  {
    string str = (string) raw["ID"];
    object obj;
    int num = !raw.TryGetValue("Count", out obj) || obj == null ? 1 : (int) obj;
    return new ProducerFrameworkIngredient()
    {
      InputId = str,
      Count = num
    };
  }
}
