// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.Models.GiftTasteModel
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Pathoschild.Stardew.LookupAnything.Framework.Constants;
using StardewValley;
using System.Runtime.CompilerServices;
using System.Text;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework.Models;

internal record GiftTasteModel(NPC Villager, Item Item, GiftTaste Taste)
{
  public bool IsRevealed => Game1.player.hasGiftTasteBeenRevealed(this.Villager, this.Item.ItemId);

  [CompilerGenerated]
  protected virtual bool PrintMembers(StringBuilder builder)
  {
    RuntimeHelpers.EnsureSufficientExecutionStack();
    builder.Append("Villager = ");
    builder.Append((object) this.Villager);
    builder.Append(", Item = ");
    builder.Append((object) this.Item);
    builder.Append(", Taste = ");
    builder.Append(this.Taste.ToString());
    builder.Append(", IsRevealed = ");
    builder.Append(this.IsRevealed.ToString());
    return true;
  }
}
