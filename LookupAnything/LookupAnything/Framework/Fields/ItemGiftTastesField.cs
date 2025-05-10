// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.Fields.ItemGiftTastesField
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Microsoft.Xna.Framework;
using Pathoschild.Stardew.LookupAnything.Framework.Constants;
using Pathoschild.Stardew.LookupAnything.Framework.Models;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework.Fields;

internal class ItemGiftTastesField(
  string label,
  IDictionary<GiftTaste, GiftTasteModel[]> giftTastes,
  GiftTaste showTaste,
  bool showUnknown,
  bool highlightUnrevealed) : GenericField(label, ItemGiftTastesField.GetText(giftTastes, showTaste, showUnknown, highlightUnrevealed))
{
  private static IEnumerable<IFormattedText> GetText(
    IDictionary<GiftTaste, GiftTasteModel[]> giftTastes,
    GiftTaste showTaste,
    bool showUnknown,
    bool highlightUnrevealed)
  {
    GiftTasteModel[] source;
    if (giftTastes.TryGetValue(showTaste, out source))
    {
      GiftTasteModel[] visibleEntries = ((IEnumerable<GiftTasteModel>) source).OrderBy<GiftTasteModel, string>((Func<GiftTasteModel, string>) (entry => ((Character) entry.Villager).displayName)).Where<GiftTasteModel>((Func<GiftTasteModel, bool>) (entry => showUnknown || entry.IsRevealed)).ToArray<GiftTasteModel>();
      int unrevealed = !showUnknown ? ((IEnumerable<GiftTasteModel>) giftTastes[showTaste]).Count<GiftTasteModel>((Func<GiftTasteModel, bool>) (p => !p.IsRevealed)) : 0;
      if (((IEnumerable<GiftTasteModel>) visibleEntries).Any<GiftTasteModel>())
      {
        int i = 0;
        for (int last = visibleEntries.Length - 1; i <= last; ++i)
        {
          GiftTasteModel giftTasteModel = visibleEntries[i];
          string text = ((Character) giftTasteModel.Villager).displayName + (i != last ? I18n.Generic_ListSeparator() : "");
          bool flag = highlightUnrevealed && !giftTasteModel.IsRevealed;
          Color? color = new Color?();
          int num = flag ? 1 : 0;
          yield return (IFormattedText) new FormattedText(text, color, num != 0);
        }
        if (unrevealed > 0)
          yield return (IFormattedText) new FormattedText(I18n.Item_UndiscoveredGiftTasteAppended((object) unrevealed), new Color?(Color.Gray));
      }
      else
        yield return (IFormattedText) new FormattedText(I18n.Item_UndiscoveredGiftTaste((object) unrevealed), new Color?(Color.Gray));
    }
  }
}
