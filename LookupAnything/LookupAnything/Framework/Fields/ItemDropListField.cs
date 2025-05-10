// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.Fields.ItemDropListField
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pathoschild.Stardew.Common;
using Pathoschild.Stardew.LookupAnything.Framework.Data;
using Pathoschild.Stardew.LookupAnything.Framework.Lookups;
using Pathoschild.Stardew.LookupAnything.Framework.Models;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework.Fields;

internal class ItemDropListField : GenericField
{
  protected GameHelper GameHelper;
  private readonly ISubjectRegistry Codex;
  private readonly Tuple<ItemDropData, Item, SpriteInfo?>[] Drops;
  private readonly string? Preface;
  private readonly string? DefaultText;
  private readonly bool FadeNonGuaranteed;
  private readonly bool CrossOutNonGuaranteed;

  public ItemDropListField(
    GameHelper gameHelper,
    ISubjectRegistry codex,
    string label,
    IEnumerable<ItemDropData> drops,
    bool sort = true,
    bool fadeNonGuaranteed = false,
    bool crossOutNonGuaranteed = false,
    string? defaultText = null,
    string? preface = null)
    : base(label)
  {
    this.GameHelper = gameHelper;
    this.Codex = codex;
    this.Drops = this.GetEntries(drops, gameHelper).ToArray<Tuple<ItemDropData, Item, SpriteInfo>>();
    if (sort)
      this.Drops = ((IEnumerable<Tuple<ItemDropData, Item, SpriteInfo>>) this.Drops).OrderByDescending<Tuple<ItemDropData, Item, SpriteInfo>, float>((Func<Tuple<ItemDropData, Item, SpriteInfo>, float>) (p => p.Item1.Probability)).ThenBy<Tuple<ItemDropData, Item, SpriteInfo>, string>((Func<Tuple<ItemDropData, Item, SpriteInfo>, string>) (p => p.Item2.DisplayName)).ToArray<Tuple<ItemDropData, Item, SpriteInfo>>();
    this.HasValue = defaultText != null || ((IEnumerable<Tuple<ItemDropData, Item, SpriteInfo>>) this.Drops).Any<Tuple<ItemDropData, Item, SpriteInfo>>();
    this.FadeNonGuaranteed = fadeNonGuaranteed;
    this.CrossOutNonGuaranteed = crossOutNonGuaranteed;
    this.Preface = preface;
    this.DefaultText = defaultText;
  }

  public override Vector2? DrawValue(
    SpriteBatch spriteBatch,
    SpriteFont font,
    Vector2 position,
    float wrapWidth)
  {
    if (!((IEnumerable<Tuple<ItemDropData, Item, SpriteInfo>>) this.Drops).Any<Tuple<ItemDropData, Item, SpriteInfo>>())
      return new Vector2?(spriteBatch.DrawTextBlock(font, this.DefaultText, position, wrapWidth));
    this.LinkTextAreas.Clear();
    float num = 0.0f;
    if (!string.IsNullOrWhiteSpace(this.Preface))
    {
      Vector2 vector2 = spriteBatch.DrawTextBlock(font, this.Preface, position, wrapWidth);
      num += (float) (int) vector2.Y;
    }
    Vector2 size;
    // ISSUE: explicit constructor call
    ((Vector2) ref size).\u002Ector(font.MeasureString("ABC").Y);
    foreach (Tuple<ItemDropData, Item, SpriteInfo> drop in this.Drops)
    {
      ItemDropData itemDropData1;
      Item obj;
      SpriteInfo spriteInfo;
      drop.Deconstruct<ItemDropData, Item, SpriteInfo>(out itemDropData1, out obj, out spriteInfo);
      ItemDropData itemDropData2 = itemDropData1;
      Item entity = obj;
      SpriteInfo sprite = spriteInfo;
      bool flag1 = (double) itemDropData2.Probability > 0.99000000953674316;
      bool flag2 = this.FadeNonGuaranteed && !flag1;
      bool flag3 = this.CrossOutNonGuaranteed && !flag1;
      ISubject byEntity = this.Codex.GetByEntity((object) entity, (GameLocation) null);
      Color color = Color.op_Multiply(byEntity != null ? Color.Blue : Color.Black, flag2 ? 0.75f : 1f);
      spriteBatch.DrawSpriteWithin(sprite, position.X, position.Y + num, size, new Color?(flag2 ? Color.op_Multiply(Color.White, 0.5f) : Color.White));
      string text1 = flag1 ? entity.DisplayName : I18n.Generic_PercentChanceOf((object) (Decimal) (Math.Round((double) itemDropData2.Probability, 4) * 100.0), (object) entity.DisplayName);
      if (itemDropData2.MinDrop != itemDropData2.MaxDrop)
        text1 = $"{text1} ({I18n.Generic_Range((object) itemDropData2.MinDrop, (object) itemDropData2.MaxDrop)})";
      else if (itemDropData2.MinDrop > 1)
        text1 += $" ({itemDropData2.MinDrop})";
      Vector2 vector2 = spriteBatch.DrawTextBlock(font, text1, Vector2.op_Addition(position, new Vector2(size.X + 5f, num + 5f)), wrapWidth, new Color?(color));
      if (byEntity != null)
      {
        Rectangle PixelArea;
        // ISSUE: explicit constructor call
        ((Rectangle) ref PixelArea).\u002Ector((int) ((double) position.X + (double) size.X + 5.0), (int) ((double) (int) position.Y + (double) num), (int) vector2.X, (int) vector2.Y);
        this.LinkTextAreas.Add(new LinkTextArea(byEntity, PixelArea));
      }
      if (flag3)
        DrawHelper.DrawLine(spriteBatch, (float) ((double) position.X + (double) size.X + 5.0), (float) ((double) position.Y + (double) num + (double) size.Y / 2.0), new Vector2(vector2.X, 1f), new Color?(this.FadeNonGuaranteed ? Color.Gray : Color.Black));
      if (itemDropData2.Conditions != null)
      {
        string text2 = I18n.ConditionsSummary((object) HumanReadableConditionParser.Format(itemDropData2.Conditions));
        num += vector2.Y + 5f;
        vector2 = spriteBatch.DrawTextBlock(font, text2, Vector2.op_Addition(position, new Vector2(size.X + 5f, num + 5f)), wrapWidth);
        if (flag3)
          DrawHelper.DrawLine(spriteBatch, (float) ((double) position.X + (double) size.X + 5.0), (float) ((double) position.Y + (double) num + (double) size.Y / 2.0), new Vector2(vector2.X, 1f), new Color?(this.FadeNonGuaranteed ? Color.Gray : Color.Black));
      }
      num += vector2.Y + 5f;
    }
    return new Vector2?(new Vector2(wrapWidth, num));
  }

  private IEnumerable<Tuple<ItemDropData, Item, SpriteInfo?>> GetEntries(
    IEnumerable<ItemDropData> drops,
    GameHelper gameHelper)
  {
    foreach (ItemDropData drop in drops)
    {
      Item obj = ItemRegistry.Create(drop.ItemId, 1, 0, false);
      SpriteInfo sprite = gameHelper.GetSprite(obj);
      yield return Tuple.Create<ItemDropData, Item, SpriteInfo>(drop, obj, sprite);
    }
  }
}
