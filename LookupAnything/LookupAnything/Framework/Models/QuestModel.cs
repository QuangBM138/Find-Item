// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.Models.QuestModel
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Netcode;
using StardewValley;
using StardewValley.Quests;
using StardewValley.SpecialOrders;
using StardewValley.SpecialOrders.Objectives;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework.Models;

internal class QuestModel
{
  private readonly Func<Item, bool> NeedsItemImpl;
  private static readonly DonateObjective DonateObjective = new DonateObjective();

  public string DisplayText { get; }

  public QuestModel(Quest quest)
  {
    QuestModel questModel = this;
    this.DisplayText = quest.GetName();
    this.NeedsItemImpl = (Func<Item, bool>) (item => questModel.NeedsItem(quest, item));
  }

  public QuestModel(SpecialOrder order)
  {
    QuestModel questModel = this;
    this.DisplayText = order.GetName();
    this.NeedsItemImpl = (Func<Item, bool>) (item => questModel.NeedsItem(order, item));
  }

  public bool NeedsItem(Object? obj) => obj != null && this.NeedsItemImpl((Item) obj);

  private bool NeedsItem(Quest quest, Item? item)
  {
    if (item == null)
      return false;
    switch (quest)
    {
      case CraftingQuest craftingQuest:
        return ((NetFieldBase<string, NetString>) craftingQuest.ItemId).Value == item.QualifiedItemId;
      case ItemDeliveryQuest itemDeliveryQuest:
        return ((NetFieldBase<string, NetString>) itemDeliveryQuest.ItemId).Value == item.QualifiedItemId;
      case ItemHarvestQuest itemHarvestQuest:
        return ((NetFieldBase<string, NetString>) itemHarvestQuest.ItemId).Value == item.QualifiedItemId;
      case LostItemQuest lostItemQuest:
        return ((NetFieldBase<string, NetString>) lostItemQuest.ItemId).Value == item.QualifiedItemId;
      case ResourceCollectionQuest resourceCollectionQuest:
        return ((NetFieldBase<string, NetString>) resourceCollectionQuest.ItemId).Value == item.QualifiedItemId;
      case SecretLostItemQuest secretLostItemQuest:
        return ((NetFieldBase<string, NetString>) secretLostItemQuest.ItemId).Value == item.QualifiedItemId;
      default:
        return false;
    }
  }

  private bool NeedsItem(SpecialOrder order, Item? item)
  {
    return ((IEnumerable<OrderObjective>) order.objectives).Any<OrderObjective>((Func<OrderObjective, bool>) (objective =>
    {
      switch (objective)
      {
        case CollectObjective collectObjective2:
          return this.IsMatch(item, collectObjective2.acceptableContextTagSets);
        case DeliverObjective deliverObjective2:
          return this.IsMatch(item, deliverObjective2.acceptableContextTagSets);
        case DonateObjective donateObjective2:
          return donateObjective2.IsValidItem(item);
        case FishObjective fishObjective2:
          return this.IsMatch(item, fishObjective2.acceptableContextTagSets);
        case GiftObjective giftObjective2:
          return this.IsMatch(item, giftObjective2.acceptableContextTagSets);
        case ShipObjective shipObjective2:
          return this.IsMatch(item, shipObjective2.acceptableContextTagSets);
        default:
          return false;
      }
    }));
  }

  private bool IsMatch(Item? item, NetStringList contextTags)
  {
    QuestModel.DonateObjective.acceptableContextTagSets = contextTags;
    return QuestModel.DonateObjective.IsValidItem(item);
  }
}
