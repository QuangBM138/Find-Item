// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.Common.Commands.GenericCommandHandler
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Pathoschild.Stardew.Common.Utilities;
using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace Pathoschild.Stardew.Common.Commands;

internal class GenericCommandHandler
{
  private readonly IMonitor Monitor;
  private readonly InvariantDictionary<ICommand> Commands;

  public string ModName { get; }

  public string RootName { get; }

  public GenericCommandHandler(
    string rootName,
    string modName,
    IEnumerable<ICommand> commands,
    IMonitor monitor)
  {
    this.RootName = rootName;
    this.ModName = modName;
    this.Monitor = monitor;
    this.Commands = new InvariantDictionary<ICommand>((IDictionary<string, ICommand>) commands.ToDictionary<ICommand, string>((Func<ICommand, string>) (p => p.Name)));
    this.Commands["help"] = (ICommand) new GenericHelpCommand(rootName, modName, monitor, (Func<InvariantDictionary<ICommand>>) (() => this.Commands));
  }

  public bool Handle(string[] args)
  {
    string key = ((IEnumerable<string>) args).FirstOrDefault<string>() ?? "help";
    string[] array = ((IEnumerable<string>) args).Skip<string>(1).ToArray<string>();
    ICommand command;
    if (this.Commands.TryGetValue(key, out command))
    {
      command.Handle(array);
      return true;
    }
    this.Monitor.Log($"The '{this.RootName} {args[0]}' command isn't valid. Type '{this.RootName} {"help"}' for a list of valid commands.", (LogLevel) 4);
    return false;
  }

  public void RegisterWith(ICommandHelper commandHelper)
  {
    commandHelper.Add(this.RootName, $"Starts a {this.ModName} command. Type '{this.RootName} {"help"}' for details.", (Action<string, string[]>) ((_, args) => this.Handle(args)));
  }
}
