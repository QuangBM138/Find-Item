// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.Fields.CheckboxListField
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pathoschild.Stardew.Common.UI;
using Pathoschild.Stardew.LookupAnything.Framework.Fields.Models;
using StardewValley;
using System;
using System.Collections.Generic;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework.Fields;

internal class CheckboxListField : GenericField
{
  protected CheckboxList[] CheckboxLists;
  protected readonly float CheckboxSize;
  protected readonly float LineHeight;

  public CheckboxListField(string label, params CheckboxList[] checkboxLists)
    : this(label)
  {
    this.CheckboxLists = checkboxLists;
  }

  public override Vector2? DrawValue(
    SpriteBatch spriteBatch,
    SpriteFont font,
    Vector2 position,
    float wrapWidth)
  {
    float num = 0.0f;
    foreach (CheckboxList checkboxList in this.CheckboxLists)
      num += this.DrawCheckboxList(checkboxList, spriteBatch, font, new Vector2(position.X, position.Y + num), wrapWidth).Y;
    return new Vector2?(new Vector2(wrapWidth, num - this.LineHeight));
  }

  protected CheckboxListField(string label)
    : base(label, true)
  {
    this.CheckboxLists = Array.Empty<CheckboxList>();
    this.CheckboxSize = (float) (CommonSprites.Icons.FilledCheckbox.Width * 2);
    this.LineHeight = Math.Max(this.CheckboxSize, Game1.smallFont.MeasureString("ABC").Y);
  }

  protected Vector2 DrawCheckboxList(
    CheckboxList checkboxList,
    SpriteBatch spriteBatch,
    SpriteFont font,
    Vector2 position,
    float wrapWidth)
  {
    float num1 = 0.0f;
    float checkboxSize = this.CheckboxSize;
    float num2 = 0.0f;
    float num3 = (float) (((double) this.LineHeight - (double) checkboxSize) / 2.0);
    if (checkboxList.Intro != (CheckboxList.IntroData) null)
    {
      num1 += this.DrawIconText(spriteBatch, font, new Vector2(position.X, position.Y + num1), wrapWidth, checkboxList.Intro.Text, Color.Black, checkboxList.Intro.Icon, new Vector2?(new Vector2(this.LineHeight))).Y;
      num2 = 14f;
    }
    foreach (Checkbox checkbox in checkboxList.Checkboxes)
    {
      spriteBatch.Draw(CommonSprites.Icons.Sheet, new Vector2(position.X + num2, position.Y + num1 + num3), new Rectangle?(checkbox.IsChecked ? CommonSprites.Icons.FilledCheckbox : CommonSprites.Icons.EmptyCheckbox), Color.White, 0.0f, Vector2.Zero, checkboxSize / (float) CommonSprites.Icons.FilledCheckbox.Width, (SpriteEffects) 0, 1f);
      Vector2 vector2 = spriteBatch.DrawTextBlock(Game1.smallFont, (IEnumerable<IFormattedText>) checkbox.Text, new Vector2((float) ((double) position.X + (double) num2 + (double) checkboxSize + 7.0), position.Y + num1), (float) ((double) wrapWidth - (double) checkboxSize - 7.0));
      num1 += Math.Max(checkboxSize, vector2.Y);
    }
    return new Vector2(position.X, num1 + this.LineHeight);
  }
}
