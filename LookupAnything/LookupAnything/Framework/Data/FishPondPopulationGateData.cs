// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.Data.FishPondPopulationGateData
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using System.Runtime.CompilerServices;
using System.Text;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework.Data;

internal record FishPondPopulationGateData(
  int RequiredPopulation,
  FishPondPopulationGateQuestItemData[] RequiredItems)
{
  public int NewPopulation => this.RequiredPopulation + 1;

  [CompilerGenerated]
  protected virtual bool PrintMembers(StringBuilder builder)
  {
    RuntimeHelpers.EnsureSufficientExecutionStack();
    builder.Append("RequiredPopulation = ");
    builder.Append(this.RequiredPopulation.ToString());
    builder.Append(", RequiredItems = ");
    builder.Append((object) this.RequiredItems);
    builder.Append(", NewPopulation = ");
    builder.Append(this.NewPopulation.ToString());
    return true;
  }
}
