// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.Fields.GenericField
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pathoschild.Stardew.Common;
using Pathoschild.Stardew.LookupAnything.Framework.Constants;
using Pathoschild.Stardew.LookupAnything.Framework.Lookups;
using Pathoschild.Stardew.LookupAnything.Framework.Models;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework.Fields;

internal class GenericField : ICustomField
{
  protected readonly List<LinkTextArea> LinkTextAreas = new List<LinkTextArea>();
  protected readonly int IconMargin = 5;

  public string Label { get; protected set; }

  public virtual bool MayHaveLinks => this.ExpandLink != null || this.LinkTextAreas.Count != 0;

  public LinkField? ExpandLink { get; protected set; }

  public IFormattedText[]? Value { get; protected set; }

  public bool HasValue { get; protected set; }

  public GenericField(string label, string? value, bool? hasValue = null)
  {
    this.Label = label;
    this.Value = this.FormatValue(value);
    bool? nullable = hasValue;
    int num;
    if (!nullable.HasValue)
    {
      IFormattedText[] source = this.Value;
      num = source != null ? (((IEnumerable<IFormattedText>) source).Any<IFormattedText>() ? 1 : 0) : 0;
    }
    else
      num = nullable.GetValueOrDefault() ? 1 : 0;
    this.HasValue = num != 0;
  }

  public GenericField(string label, IFormattedText value, bool? hasValue = null)
    : this(label, (IEnumerable<IFormattedText>) new \u003C\u003Ez__ReadOnlySingleElementList<IFormattedText>(value), hasValue)
  {
    // ISSUE: object of a compiler-generated type is created (out of statement scope)
  }

  public GenericField(string label, IEnumerable<IFormattedText> value, bool? hasValue = null)
  {
    this.Label = label;
    this.Value = value.ToArray<IFormattedText>();
    bool? nullable = hasValue;
    int num;
    if (!nullable.HasValue)
    {
      IFormattedText[] source = this.Value;
      num = source != null ? (((IEnumerable<IFormattedText>) source).Any<IFormattedText>() ? 1 : 0) : 0;
    }
    else
      num = nullable.GetValueOrDefault() ? 1 : 0;
    this.HasValue = num != 0;
  }

  public virtual Vector2? DrawValue(
    SpriteBatch spriteBatch,
    SpriteFont font,
    Vector2 position,
    float wrapWidth)
  {
    return new Vector2?();
  }

  public virtual bool TryGetLinkAt(int x, int y, [NotNullWhen(true)] out ISubject? subject)
  {
    if (this.ExpandLink != null)
    {
      this.ExpandLink = (LinkField) null;
      subject = (ISubject) null;
      return false;
    }
    foreach (LinkTextArea linkTextArea in this.LinkTextAreas)
    {
      Rectangle pixelArea = linkTextArea.PixelArea;
      if (((Rectangle) ref pixelArea).Contains(x, y))
      {
        subject = linkTextArea.Subject;
        return true;
      }
    }
    subject = (ISubject) null;
    return false;
  }

  public virtual void CollapseIfLengthExceeds(int minResultsForCollapse, int countForLabel)
  {
    IFormattedText[] formattedTextArray = this.Value;
    if ((formattedTextArray != null ? (formattedTextArray.Length >= minResultsForCollapse ? 1 : 0) : 0) == 0)
      return;
    this.CollapseByDefault(I18n.Generic_ShowXResults((object) countForLabel));
  }

  public void CollapseByDefault(string linkText)
  {
    this.ExpandLink = new LinkField(this.Label, linkText, (Func<ISubject>) (() => (ISubject) null));
  }

  protected GenericField(string label, bool hasValue = false)
    : this(label, (string) null, new bool?(hasValue))
  {
  }

  protected IFormattedText[] FormatValue(string? value)
  {
    if (string.IsNullOrWhiteSpace(value))
      return Array.Empty<IFormattedText>();
    return new IFormattedText[1]
    {
      (IFormattedText) new FormattedText(value)
    };
  }

  public static string? GetSaleValueString(int saleValue, int stackSize)
  {
    return GenericField.GetSaleValueString((IDictionary<ItemQuality, int>) new Dictionary<ItemQuality, int>()
    {
      [ItemQuality.Normal] = saleValue
    }, stackSize);
  }

  public static string? GetSaleValueString(IDictionary<ItemQuality, int>? saleValues, int stackSize)
  {
    if (saleValues == null || !saleValues.Any<KeyValuePair<ItemQuality, int>>() || saleValues.Values.All<int>((Func<int, bool>) (p => p == 0)))
      return (string) null;
    if (saleValues.Count == 1)
    {
      string saleValueString = I18n.Generic_Price((object) saleValues.First<KeyValuePair<ItemQuality, int>>().Value);
      if (stackSize > 1 && stackSize <= Constant.MaxStackSizeForPricing)
        saleValueString = $"{saleValueString} ({I18n.Generic_PriceForStack((object) (saleValues.First<KeyValuePair<ItemQuality, int>>().Value * stackSize), (object) stackSize)})";
      return saleValueString;
    }
    List<string> values = new List<string>();
    ItemQuality itemQuality = ItemQuality.Normal;
    while (true)
    {
      if (saleValues.ContainsKey(itemQuality))
        values.Add(itemQuality == ItemQuality.Normal ? I18n.Generic_Price((object) saleValues[itemQuality]) : I18n.Generic_PriceForQuality((object) saleValues[itemQuality], (object) I18n.For(itemQuality)));
      if (itemQuality.GetNext() != itemQuality)
        itemQuality = itemQuality.GetNext();
      else
        break;
    }
    return I18n.List((IEnumerable<object>) values);
  }

  protected Vector2 DrawIconText(
    SpriteBatch batch,
    SpriteFont font,
    Vector2 position,
    float absoluteWrapWidth,
    string text,
    Color textColor,
    SpriteInfo? icon = null,
    Vector2? iconSize = null,
    Color? iconColor = null,
    int? qualityIcon = null,
    bool probe = false)
  {
    int num1 = 0;
    if (icon != null && iconSize.HasValue)
    {
      if (!probe)
        batch.DrawSpriteWithin(icon, position.X, position.Y, iconSize.Value, iconColor);
      num1 = this.IconMargin;
    }
    else
      iconSize = new Vector2?(Vector2.Zero);
    int? nullable = qualityIcon;
    int num2 = 0;
    if (nullable.GetValueOrDefault() > num2 & nullable.HasValue && iconSize.HasValue)
    {
      Vector2 valueOrDefault = iconSize.GetValueOrDefault();
      if ((double) valueOrDefault.X > 0.0 && (double) valueOrDefault.Y > 0.0)
      {
        nullable = qualityIcon;
        int num3 = 4;
        Rectangle sprite = nullable.GetValueOrDefault() < num3 & nullable.HasValue ? new Rectangle(338 + (qualityIcon.Value - 1) * 8, 400, 8, 8) : new Rectangle(346, 392, 8, 8);
        Texture2D mouseCursors = Game1.mouseCursors;
        Vector2 size = Vector2.op_Division(iconSize.Value, 2f);
        Vector2 vector2;
        // ISSUE: explicit constructor call
        ((Vector2) ref vector2).\u002Ector(position.X + iconSize.Value.X - size.X, position.Y + iconSize.Value.Y - size.Y);
        batch.DrawSpriteWithin(mouseCursors, sprite, vector2.X, vector2.Y, size, iconColor);
      }
    }
    Vector2 vector2_1 = probe ? font.MeasureString(text) : batch.DrawTextBlock(font, text, Vector2.op_Addition(position, new Vector2(iconSize.Value.X + (float) num1, 0.0f)), absoluteWrapWidth - (float) num1, new Color?(textColor));
    return new Vector2(iconSize.Value.X + vector2_1.X, Math.Max(iconSize.Value.Y, vector2_1.Y));
  }
}
