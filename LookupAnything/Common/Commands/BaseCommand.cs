// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.Common.Commands.BaseCommand
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using StardewModdingAPI;

#nullable enable
namespace Pathoschild.Stardew.Common.Commands;

internal abstract class BaseCommand : ICommand
{
  protected readonly IMonitor Monitor;

  public string Name { get; }

  public string Description => this.GetDescription();

  public abstract string GetDescription();

  public abstract void Handle(string[] args);

  protected BaseCommand(IMonitor monitor, string name)
  {
    this.Monitor = monitor;
    this.Name = name;
  }
}
