// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.Common.Commands.ICommand
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

#nullable enable
namespace Pathoschild.Stardew.Common.Commands;

internal interface ICommand
{
  string Name { get; }

  string Description { get; }

  void Handle(string[] args);
}
