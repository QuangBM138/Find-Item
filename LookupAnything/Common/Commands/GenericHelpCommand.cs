// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.Common.Commands.GenericHelpCommand
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Pathoschild.Stardew.Common.Utilities;
using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#nullable enable
namespace Pathoschild.Stardew.Common.Commands;

internal class GenericHelpCommand : BaseCommand
{
  private readonly string ModName;
  private readonly string RootName;
  private readonly Func<InvariantDictionary<ICommand>> GetCommands;
  internal const string CommandName = "help";

  public GenericHelpCommand(
    string rootName,
    string modName,
    IMonitor monitor,
    Func<InvariantDictionary<ICommand>> getCommands)
    : base(monitor, "help")
  {
    this.ModName = modName;
    this.RootName = rootName;
    this.GetCommands = getCommands;
  }

  public override string GetDescription()
  {
    return $"\r\n                {this.RootName} {"help"}\r\n                   Usage: {this.RootName} {"help"}\r\n                   Lists all available {this.RootName} commands.\r\n\r\n                   Usage: {this.RootName} {"help"} <cmd>\r\n                   Provides information for a specific {this.RootName} command.\r\n                   - cmd: The {this.RootName} command name.\r\n            ";
  }

  public override void Handle(string[] args)
  {
    InvariantDictionary<ICommand> source = this.GetCommands();
    StringBuilder stringBuilder1 = new StringBuilder();
    if (!((IEnumerable<string>) args).Any<string>())
    {
      StringBuilder stringBuilder2 = stringBuilder1;
      StringBuilder stringBuilder3 = stringBuilder2;
      StringBuilder.AppendInterpolatedStringHandler interpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(150, 5, stringBuilder2);
      interpolatedStringHandler.AppendLiteral("The '");
      interpolatedStringHandler.AppendFormatted(this.RootName);
      interpolatedStringHandler.AppendLiteral("' command is the entry point for ");
      interpolatedStringHandler.AppendFormatted(this.ModName);
      interpolatedStringHandler.AppendLiteral(" commands. You use it by specifying a more ");
      interpolatedStringHandler.AppendLiteral("specific command (like '");
      interpolatedStringHandler.AppendFormatted("help");
      interpolatedStringHandler.AppendLiteral("' in '");
      interpolatedStringHandler.AppendFormatted(this.RootName);
      interpolatedStringHandler.AppendLiteral(" ");
      interpolatedStringHandler.AppendFormatted("help");
      interpolatedStringHandler.AppendLiteral("'). Here are the available commands:\n\n");
      ref StringBuilder.AppendInterpolatedStringHandler local = ref interpolatedStringHandler;
      stringBuilder3.AppendLine(ref local);
      foreach (KeyValuePair<string, ICommand> keyValuePair in (IEnumerable<KeyValuePair<string, ICommand>>) source.OrderBy<KeyValuePair<string, ICommand>, string>((Func<KeyValuePair<string, ICommand>, string>) (p => p.Key), (IComparer<string>) HumanSortComparer.DefaultIgnoreCase))
      {
        stringBuilder1.AppendLine(keyValuePair.Value.Description);
        stringBuilder1.AppendLine();
        stringBuilder1.AppendLine();
      }
    }
    else
    {
      ICommand command;
      if (source.TryGetValue(args[0], out command))
      {
        stringBuilder1.AppendLine(command.Description);
      }
      else
      {
        StringBuilder stringBuilder4 = stringBuilder1;
        StringBuilder stringBuilder5 = stringBuilder4;
        StringBuilder.AppendInterpolatedStringHandler interpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(53, 4, stringBuilder4);
        interpolatedStringHandler.AppendLiteral("Unknown command '");
        interpolatedStringHandler.AppendFormatted(this.RootName);
        interpolatedStringHandler.AppendLiteral(" ");
        interpolatedStringHandler.AppendFormatted(args[0]);
        interpolatedStringHandler.AppendLiteral("'. Type '");
        interpolatedStringHandler.AppendFormatted(this.RootName);
        interpolatedStringHandler.AppendLiteral(" ");
        interpolatedStringHandler.AppendFormatted("help");
        interpolatedStringHandler.AppendLiteral("' for available commands.");
        ref StringBuilder.AppendInterpolatedStringHandler local = ref interpolatedStringHandler;
        stringBuilder5.AppendLine(ref local);
      }
    }
    this.Monitor.Log(stringBuilder1.ToString().Trim(), (LogLevel) 2);
  }
}
