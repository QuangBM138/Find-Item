// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.Fields.MovieTastesField
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Pathoschild.Stardew.LookupAnything.Framework.Constants;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework.Fields;

internal class MovieTastesField(
  string label,
  IDictionary<GiftTaste, string[]> giftTastes,
  GiftTaste showTaste) : GenericField(label, MovieTastesField.GetText(giftTastes, showTaste))
{
  private static string? GetText(IDictionary<GiftTaste, string[]> giftTastes, GiftTaste showTaste)
  {
    string[] source;
    return !giftTastes.TryGetValue(showTaste, out source) ? (string) null : I18n.List((IEnumerable<object>) ((IEnumerable<string>) source).OrderBy<string, string>((Func<string, string>) (p => p)).ToArray<string>());
  }
}
