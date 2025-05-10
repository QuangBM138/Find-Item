// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.Fields.CharacterGiftTastesField
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Microsoft.Xna.Framework;
using Pathoschild.Stardew.LookupAnything.Framework.Constants;
using Pathoschild.Stardew.LookupAnything.Framework.ItemScanning;
using Pathoschild.Stardew.LookupAnything.Framework.Models;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework.Fields;

internal class CharacterGiftTastesField : GenericField
{
  public int TotalItems { get; }

  public CharacterGiftTastesField(
    string label,
    IDictionary<GiftTaste, GiftTasteModel[]> giftTastes,
    GiftTaste showTaste,
    bool showUnknown,
    bool highlightUnrevealed,
    bool onlyOwned,
    IDictionary<string, bool> ownedItemsCache)
    : base(label)
  {
    CharacterGiftTastesField.ItemRecord[] giftTasteRecords = this.GetGiftTasteRecords(giftTastes, showTaste, ownedItemsCache);
    this.TotalItems = giftTasteRecords.Length;
    this.Value = this.GetText(giftTasteRecords, showUnknown, highlightUnrevealed, onlyOwned).ToArray<IFormattedText>();
    this.HasValue = this.Value.Length != 0;
  }

  public static IDictionary<string, bool> GetOwnedItemsCache(GameHelper gameHelper)
  {
    return (IDictionary<string, bool>) gameHelper.GetAllOwnedItems().GroupBy<FoundItem, string>((Func<FoundItem, string>) (entry => entry.Item.QualifiedItemId)).ToDictionary<IGrouping<string, FoundItem>, string, bool>((Func<IGrouping<string, FoundItem>, string>) (group => group.Key), (Func<IGrouping<string, FoundItem>, bool>) (group => group.Any<FoundItem>((Func<FoundItem, bool>) (p => p.IsInInventory))));
  }

  private CharacterGiftTastesField.ItemRecord[] GetGiftTasteRecords(
    IDictionary<GiftTaste, GiftTasteModel[]> giftTastes,
    GiftTaste showTaste,
    IDictionary<string, bool> ownedItemsCache)
  {
    GiftTasteModel[] source;
    bool flag;
    return !giftTastes.TryGetValue(showTaste, out source) ? Array.Empty<CharacterGiftTastesField.ItemRecord>() : ((IEnumerable<GiftTasteModel>) source).Select(entry => new
    {
      entry = entry,
      item = entry.Item
    }).Select(_param1 => new
    {
      \u003C\u003Eh__TransparentIdentifier0 = _param1,
      ownership = ownedItemsCache.TryGetValue(_param1.item.QualifiedItemId, out flag) ? new bool?(flag) : new bool?()
    }).Select(_param1 => new
    {
      \u003C\u003Eh__TransparentIdentifier1 = _param1,
      isOwned = _param1.ownership.HasValue
    }).Select(_param1 =>
    {
      var data = _param1;
      bool? ownership = _param1.\u003C\u003Eh__TransparentIdentifier1.ownership;
      int num = !ownership.HasValue ? 0 : (ownership.GetValueOrDefault() ? 1 : 0);
      return new
      {
        \u003C\u003Eh__TransparentIdentifier2 = data,
        inInventory = num != 0
      };
    }).OrderByDescending(_param1 => _param1.inInventory).ThenByDescending(_param1 => _param1.\u003C\u003Eh__TransparentIdentifier2.isOwned).ThenBy(_param1 => _param1.\u003C\u003Eh__TransparentIdentifier2.\u003C\u003Eh__TransparentIdentifier1.\u003C\u003Eh__TransparentIdentifier0.item.DisplayName).Select(_param1 => new CharacterGiftTastesField.ItemRecord(_param1.\u003C\u003Eh__TransparentIdentifier2.\u003C\u003Eh__TransparentIdentifier1.\u003C\u003Eh__TransparentIdentifier0.item, _param1.inInventory, _param1.\u003C\u003Eh__TransparentIdentifier2.isOwned, _param1.\u003C\u003Eh__TransparentIdentifier2.\u003C\u003Eh__TransparentIdentifier1.\u003C\u003Eh__TransparentIdentifier0.entry.IsRevealed)).ToArray<CharacterGiftTastesField.ItemRecord>();
  }

  private IEnumerable<IFormattedText> GetText(
    CharacterGiftTastesField.ItemRecord[] items,
    bool showUnknown,
    bool highlightUnrevealed,
    bool onlyOwned)
  {
    if (((IEnumerable<CharacterGiftTastesField.ItemRecord>) items).Any<CharacterGiftTastesField.ItemRecord>())
    {
      int unrevealed = 0;
      int unowned = 0;
      int i = 0;
      for (int last = items.Length - 1; i <= last; ++i)
      {
        CharacterGiftTastesField.ItemRecord itemRecord = items[i];
        if (!showUnknown && !itemRecord.IsRevealed)
          ++unrevealed;
        else if (onlyOwned && !itemRecord.IsOwned)
        {
          ++unowned;
        }
        else
        {
          string text = i != last ? itemRecord.Item.DisplayName + I18n.Generic_ListSeparator() : itemRecord.Item.DisplayName;
          bool bold = highlightUnrevealed && !itemRecord.IsRevealed;
          if (itemRecord.IsInventory)
            yield return (IFormattedText) new FormattedText(text, new Color?(Color.Green), bold);
          else if (itemRecord.IsOwned)
            yield return (IFormattedText) new FormattedText(text, new Color?(Color.Black), bold);
          else
            yield return (IFormattedText) new FormattedText(text, new Color?(Color.Gray), bold);
        }
      }
      if (unrevealed > 0)
        yield return (IFormattedText) new FormattedText(I18n.Npc_UndiscoveredGiftTaste((object) unrevealed), new Color?(Color.Gray));
      if (unowned > 0)
        yield return (IFormattedText) new FormattedText(I18n.Npc_UnownedGiftTaste((object) unowned), new Color?(Color.Gray));
    }
  }

  private record ItemRecord(Item Item, bool IsInventory, bool IsOwned, bool IsRevealed);
}
