// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.DrawTextHelper
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pathoschild.Stardew.Common;
using Pathoschild.Stardew.LookupAnything.Framework;
using StardewModdingAPI;
using StardewValley;
using System;
using System.Collections.Generic;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything;

internal static class DrawTextHelper
{
  private static string? LastLanguage;
  private static readonly HashSet<char> SoftBreakCharacters = new HashSet<char>();

  public static Vector2 DrawTextBlock(
    this SpriteBatch batch,
    SpriteFont font,
    string? text,
    Vector2 position,
    float wrapWidth,
    Color? color = null,
    bool bold = false,
    float scale = 1f)
  {
    // ISSUE: object of a compiler-generated type is created
    return batch.DrawTextBlock(font, (IEnumerable<IFormattedText>) new \u003C\u003Ez__ReadOnlySingleElementList<IFormattedText>((IFormattedText) new FormattedText(text, color, bold)), position, wrapWidth, scale);
  }

  public static Vector2 DrawTextBlock(
    this SpriteBatch batch,
    SpriteFont font,
    IEnumerable<IFormattedText?>? text,
    Vector2 position,
    float wrapWidth,
    float scale = 1f)
  {
    if (text == null)
      return new Vector2(0.0f, 0.0f);
    DrawTextHelper.InitIfNeeded();
    float num1 = 0.0f;
    float num2 = 0.0f;
    float num3 = font.MeasureString("ABC").Y * scale;
    float num4 = DrawHelper.GetSpaceWidth(font) * scale;
    float num5 = 0.0f;
    float num6 = num3;
    DrawTextHelper.InitIfNeeded();
    foreach (IFormattedText formattedText in text)
    {
      if (formattedText != null && formattedText.Text != null)
      {
        string[] strArray1 = formattedText.Text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (strArray1.Length != 0)
        {
          if (formattedText.Text.StartsWith(" "))
            strArray1[0] = " " + strArray1[0];
          if (formattedText.Text.EndsWith(" "))
          {
            string[] strArray2 = strArray1;
            // ISSUE: explicit reference operation
            ^ref strArray2[strArray2.Length - 1] += " ";
          }
        }
        bool flag1 = true;
        foreach (string str1 in strArray1)
        {
          string[] strArray3 = str1.Split('\n');
          if (strArray3.Length > 1)
          {
            for (int index = 0; index < strArray3.Length; ++index)
              strArray3[index] = strArray3[index].TrimEnd('\r');
          }
          for (int index = 0; index < strArray3.Length; ++index)
          {
            if (index > 0)
            {
              num1 = 0.0f;
              num2 += num3;
              num6 += num3;
              flag1 = true;
            }
            bool flag2 = true;
            foreach (string str2 in (IEnumerable<string>) DrawTextHelper.SplitWithinWordForLineWrapping(strArray3[index]))
            {
              float num7 = font.MeasureString(str2).X * scale;
              float num8 = !flag2 || flag1 ? 0.0f : num4;
              if (str2 == Environment.NewLine || (double) num7 + (double) num1 + (double) num8 > (double) wrapWidth && (int) num1 != 0)
              {
                num1 = 0.0f;
                num2 += num3;
                num6 += num3;
                flag1 = true;
              }
              if (!(str2 == Environment.NewLine))
              {
                Vector2 vector2;
                // ISSUE: explicit constructor call
                ((Vector2) ref vector2).\u002Ector(position.X + num1 + num8, position.Y + num2);
                if (formattedText.Bold)
                  Utility.drawBoldText(batch, str2, font, vector2, formattedText.Color ?? Color.Black, scale, -1f, 1);
                else
                  batch.DrawString(font, str2, vector2, formattedText.Color ?? Color.Black, 0.0f, Vector2.Zero, scale, (SpriteEffects) 0, 1f);
                if ((double) num1 + (double) num7 + (double) num8 > (double) num5)
                  num5 = num1 + num7 + num8;
                num1 += num7 + num8;
                flag1 = false;
                flag2 = false;
              }
            }
          }
        }
      }
    }
    return new Vector2(num5, num6);
  }

  public static void InitIfNeeded()
  {
    string currentLanguageString = LocalizedContentManager.CurrentLanguageString;
    if (!(DrawTextHelper.LastLanguage != currentLanguageString))
      return;
    string str = Translation.op_Implicit(I18n.GetByKey("generic.line-wrap-on").UsePlaceholder(false));
    DrawTextHelper.SoftBreakCharacters.Clear();
    if (!string.IsNullOrEmpty(str))
      StardewValley.Extensions.CollectionExtensions.AddRange<char>((ISet<char>) DrawTextHelper.SoftBreakCharacters, (IEnumerable<char>) str);
    DrawTextHelper.LastLanguage = currentLanguageString;
  }

  private static IList<string> SplitWithinWordForLineWrapping(string text)
  {
    HashSet<char> softBreakCharacters = DrawTextHelper.SoftBreakCharacters;
    string newLine = Environment.NewLine;
    List<string> stringList = new List<string>();
    int startIndex = 0;
    for (int index = 0; index < text.Length; ++index)
    {
      char ch = text[index];
      if ((int) ch == (int) newLine[0] && DrawTextHelper.IsNewlineAt(text, index))
      {
        if (index > startIndex)
          stringList.Add(text.Substring(startIndex, index - startIndex));
        stringList.Add(newLine);
        index += newLine.Length;
        startIndex = index;
      }
      else if (softBreakCharacters.Contains(ch))
      {
        stringList.Add(text.Substring(startIndex, index - startIndex + 1));
        startIndex = index + 1;
      }
    }
    if (startIndex == 0)
      stringList.Add(text);
    else if (startIndex < text.Length - 1)
      stringList.Add(text.Substring(startIndex));
    return (IList<string>) stringList;
  }

  private static bool IsNewlineAt(string text, int index)
  {
    string newLine = Environment.NewLine;
    int index1 = index;
    for (int index2 = 0; index1 < text.Length && index2 < newLine.Length; ++index2)
    {
      if ((int) text[index1] != (int) newLine[index2])
        return false;
      ++index1;
    }
    return true;
  }
}
