// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.FormattedText
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework;

internal struct FormattedText(string? text, Microsoft.Xna.Framework.Color? color = null, bool bold = false) : 
  IFormattedText
{
  public string? Text { get; } = text;

  public Microsoft.Xna.Framework.Color? Color { get; } = color;

  public bool Bold { get; } = bold;
}
