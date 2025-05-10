// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.Fields.PercentageBarField
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pathoschild.Stardew.LookupAnything.Components;
using System;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework.Fields;

internal class PercentageBarField : GenericField
{
  protected readonly int CurrentValue;
  protected readonly int MaxValue;
  protected readonly string? Text;
  protected readonly Color FilledColor;
  protected readonly Color EmptyColor;

  public PercentageBarField(
    string label,
    int currentValue,
    int maxValue,
    Color filledColor,
    Color emptyColor,
    string? text)
    : base(label, true)
  {
    this.CurrentValue = currentValue;
    this.MaxValue = maxValue;
    this.FilledColor = filledColor;
    this.EmptyColor = emptyColor;
    this.Text = text;
  }

  public override Vector2? DrawValue(
    SpriteBatch spriteBatch,
    SpriteFont font,
    Vector2 position,
    float wrapWidth)
  {
    Vector2 vector2_1 = this.DrawBar(spriteBatch, position, (float) this.CurrentValue / ((float) this.MaxValue * 1f), this.FilledColor, this.EmptyColor, wrapWidth);
    Vector2 vector2_2 = !string.IsNullOrWhiteSpace(this.Text) ? spriteBatch.DrawTextBlock(font, this.Text, new Vector2((float) ((double) position.X + (double) vector2_1.X + 3.0), position.Y), wrapWidth) : Vector2.Zero;
    return new Vector2?(new Vector2(vector2_1.X + 3f + vector2_2.X, Math.Max(vector2_1.Y, vector2_2.Y)));
  }

  protected Vector2 DrawBar(
    SpriteBatch spriteBatch,
    Vector2 position,
    float ratio,
    Color filledColor,
    Color emptyColor,
    float maxWidth = 100f)
  {
    int num1 = 22;
    ratio = Math.Min(1f, ratio);
    float num2 = Math.Min(100f, maxWidth);
    float num3 = num2 * ratio;
    float num4 = num2 - num3;
    if ((double) num3 > 0.0)
      spriteBatch.Draw(Sprites.Pixel, new Rectangle((int) position.X, (int) position.Y, (int) num3, num1), filledColor);
    if ((double) num4 > 0.0)
      spriteBatch.Draw(Sprites.Pixel, new Rectangle((int) ((double) position.X + (double) num3), (int) position.Y, (int) num4, num1), emptyColor);
    return new Vector2(num2, (float) num1);
  }
}
