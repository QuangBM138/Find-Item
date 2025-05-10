// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.IFormattedText
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework;

internal interface IFormattedText
{
  Microsoft.Xna.Framework.Color? Color { get; }

  string? Text { get; }

  bool Bold { get; }
}
