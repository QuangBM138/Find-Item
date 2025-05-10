// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.Common.Integrations.BaseIntegration`1
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using StardewModdingAPI;
using System;
using System.Diagnostics.CodeAnalysis;

#nullable enable
namespace Pathoschild.Stardew.Common.Integrations;

internal abstract class BaseIntegration<TApi> : BaseIntegration where TApi : class
{
  public TApi? ModApi { get; }

  [MemberNotNullWhen(true, "ModApi")]
  public override bool IsLoaded
  {
    [MemberNotNullWhen(true, "ModApi")] get => (object) this.ModApi != null;
  }

  protected BaseIntegration(
    string label,
    string modID,
    string minVersion,
    IModRegistry modRegistry,
    IMonitor monitor)
    : base(label, modID, minVersion, modRegistry, monitor)
  {
    if (!base.IsLoaded)
      return;
    this.ModApi = this.GetValidatedApi<TApi>();
  }

  [MemberNotNull("ModApi")]
  protected override void AssertLoaded()
  {
    if (!this.IsLoaded)
      throw new InvalidOperationException($"The {this.Label} integration isn't loaded.");
  }
}
