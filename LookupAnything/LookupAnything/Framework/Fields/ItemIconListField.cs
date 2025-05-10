// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.Fields.ItemIconListField
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pathoschild.Stardew.Common;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework.Fields;

internal class ItemIconListField : GenericField
{
  private readonly Tuple<Item, SpriteInfo?>[] Items;
  private readonly Func<Item, string?>? FormatItemName;
  private readonly bool ShowStackSize;

  public ItemIconListField(
    GameHelper gameHelper,
    string label,
    IEnumerable<Item?>? items,
    bool showStackSize,
    Func<Item, string?>? formatItemName = null)
    : base(label, items != null)
  {
    this.Items = (items != null ? items.WhereNotNull<Item>().Select<Item, Tuple<Item, SpriteInfo>>((Func<Item, Tuple<Item, SpriteInfo>>) (item => Tuple.Create<Item, SpriteInfo>(item, gameHelper.GetSprite(item)))).ToArray<Tuple<Item, SpriteInfo>>() : (Tuple<Item, SpriteInfo>[]) null) ?? Array.Empty<Tuple<Item, SpriteInfo>>();
    this.HasValue = ((IEnumerable<Tuple<Item, SpriteInfo>>) this.Items).Any<Tuple<Item, SpriteInfo>>();
    this.ShowStackSize = showStackSize;
    this.FormatItemName = formatItemName;
  }

  public override Vector2? DrawValue(
    SpriteBatch spriteBatch,
    SpriteFont font,
    Vector2 position,
    float wrapWidth)
  {
    float y = font.MeasureString("ABC").Y;
    Vector2 size;
    // ISSUE: explicit constructor call
    ((Vector2) ref size).\u002Ector(y);
    int num1 = 0;
    foreach (Tuple<Item, SpriteInfo> tuple in this.Items)
    {
      Item obj1;
      SpriteInfo spriteInfo;
      tuple.Deconstruct<Item, SpriteInfo>(out obj1, out spriteInfo);
      Item obj2 = obj1;
      SpriteInfo sprite = spriteInfo;
      spriteBatch.DrawSpriteWithin(sprite, position.X, position.Y + (float) num1, size);
      if (this.ShowStackSize && obj2.Stack > 1)
      {
        float num2 = 2f;
        Vector2 vector2 = Vector2.op_Addition(position, new Vector2(size.X - (float) Utility.getWidthOfTinyDigitString(obj2.Stack, num2), (float) ((double) size.Y + (double) num1 - 6.0 * (double) num2)));
        Utility.drawTinyDigits(obj2.Stack, spriteBatch, vector2, num2, 1f, Color.White);
      }
      Func<Item, string> formatItemName = this.FormatItemName;
      string text = (formatItemName != null ? formatItemName(obj2) : (string) null) ?? obj2.DisplayName;
      Vector2 vector2_1 = spriteBatch.DrawTextBlock(font, text, Vector2.op_Addition(position, new Vector2(size.X + 5f, (float) num1)), wrapWidth);
      num1 += (int) Math.Max(size.Y, vector2_1.Y) + 5;
    }
    return new Vector2?(new Vector2(wrapWidth, (float) (num1 + 5)));
  }
}
