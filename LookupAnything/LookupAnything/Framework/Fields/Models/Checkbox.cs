// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.Fields.Models.Checkbox
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework.Fields.Models;

internal record Checkbox(bool IsChecked, params IFormattedText[] Text)
{
  public Checkbox(bool isChecked, string text)
    : this((isChecked ? 1 : 0) != 0, (IFormattedText) new FormattedText(text))
  {
  }
}
