// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.Common.Integrations.BaseIntegration
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using StardewModdingAPI;
using System;

#nullable enable
namespace Pathoschild.Stardew.Common.Integrations;

internal abstract class BaseIntegration : IModIntegration
{
  protected string ModID { get; }

  protected IModRegistry ModRegistry { get; }

  protected IMonitor Monitor { get; }

  public string Label { get; }

  public virtual bool IsLoaded { get; }

  protected BaseIntegration(
    string label,
    string modID,
    string minVersion,
    IModRegistry modRegistry,
    IMonitor monitor)
  {
    this.Label = label;
    this.ModID = modID;
    this.ModRegistry = modRegistry;
    this.Monitor = monitor;
    IManifest manifest = modRegistry.Get(this.ModID)?.Manifest;
    if (manifest == null)
      return;
    if (manifest.Version.IsOlderThan(minVersion))
      monitor.Log($"Detected {label} {manifest.Version}, but need {minVersion} or later. Disabled integration with this mod.", (LogLevel) 3);
    else
      this.IsLoaded = true;
  }

  protected TApi? GetValidatedApi<TApi>() where TApi : class
  {
    TApi api = this.ModRegistry.GetApi<TApi>(this.ModID);
    if ((object) api != null)
      return api;
    this.Monitor.Log($"Detected {this.Label}, but couldn't fetch its API. Disabled integration with this mod.", (LogLevel) 3);
    return default (TApi);
  }

  protected virtual void AssertLoaded()
  {
    if (!this.IsLoaded)
      throw new InvalidOperationException($"The {this.Label} integration isn't loaded.");
  }
}
