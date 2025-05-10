// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.Common.CommonHelper
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pathoschild.Stardew.Common.UI;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Locations;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

#nullable enable
namespace Pathoschild.Stardew.Common;

internal static class CommonHelper
{
  private static readonly Lazy<Texture2D> LazyPixel = new Lazy<Texture2D>((Func<Texture2D>) (() =>
  {
    Texture2D texture2D = new Texture2D(Game1.graphics.GraphicsDevice, 1, 1);
    texture2D.SetData<Color>(new Color[1]{ Color.White });
    return texture2D;
  }));
  public const int ButtonBorderWidth = 16 /*0x10*/;
  public static readonly Vector2 ScrollEdgeSize = new Vector2((float) (CommonSprites.Scroll.TopLeft.Width * 4), (float) (CommonSprites.Scroll.TopLeft.Height * 4));

  public static Texture2D Pixel => CommonHelper.LazyPixel.Value;

  public static IEnumerable<TValue> GetEnumValues<TValue>() where TValue : struct
  {
    return Enum.GetValues(typeof (TValue)).Cast<TValue>();
  }

  public static IEnumerable<GameLocation> GetLocations(bool includeTempLevels = false)
  {
    IEnumerable<GameLocation> first = Game1.locations.Concat<GameLocation>(Game1.locations.SelectMany<GameLocation, GameLocation, GameLocation>((Func<GameLocation, IEnumerable<GameLocation>>) (location => location.GetInstancedBuildingInteriors()), (Func<GameLocation, GameLocation, GameLocation>) ((location, indoors) => indoors)));
    if (includeTempLevels)
      first = first.Concat<GameLocation>((IEnumerable<GameLocation>) MineShaft.activeMines).Concat<GameLocation>((IEnumerable<GameLocation>) VolcanoDungeon.activeLevels);
    return first;
  }

  public static Vector2 GetPlayerTile(Farmer? player)
  {
    Vector2 vector2 = player != null ? ((Character) player).Position : Vector2.Zero;
    return new Vector2((float) (int) ((double) vector2.X / 64.0), (float) (int) ((double) vector2.Y / 64.0));
  }

  public static bool IsItemId(string itemId, bool allowZero = true)
  {
    if (string.IsNullOrWhiteSpace(itemId))
      return false;
    int result;
    return !int.TryParse(itemId, out result) || result >= (!allowZero ? 1 : 0);
  }

  public static string FormatTime(int time)
  {
    string timeOfDayString = Game1.getTimeOfDayString(time);
    if (LocalizedContentManager.CurrentLanguageCode - 9 <= 1)
    {
      string str = Game1.content.LoadString(time < 1200 || time >= 2400 ? "Strings\\StringsFromCSFiles:DayTimeMoneyBox.cs.10370" : "Strings\\StringsFromCSFiles:DayTimeMoneyBox.cs.10371");
      timeOfDayString += LocalizedContentManager.CurrentLanguageCode == 9 ? str : " " + str;
    }
    return timeOfDayString;
  }

  public static float GetSpaceWidth(SpriteFont font)
  {
    return font.MeasureString("A B").X - font.MeasureString("AB").X;
  }

  public static IModInfo? TryGetModFromStringId(
    IModRegistry modRegistry,
    string? id,
    bool allowModOnlyId = false)
  {
    if (id == null)
      return (IModInfo) null;
    IModInfo modFromStringId = (IModInfo) null;
    if (allowModOnlyId)
    {
      modFromStringId = modRegistry.Get(id);
      if (modFromStringId != null)
        return modFromStringId;
    }
    string[] strArray = id.Split('_');
    if (strArray.Length == 1)
      return (IModInfo) null;
    string str = strArray[0];
    int num = strArray.Length - 1;
    for (int index = 0; index < num; ++index)
    {
      if (index != 0)
        str = $"{str}_{strArray[index]}";
      modFromStringId = modRegistry.Get(str) ?? modFromStringId;
    }
    return modFromStringId;
  }

  public static void Draw(
    this SpriteBatch batch,
    Texture2D sheet,
    Rectangle sprite,
    int x,
    int y,
    int width,
    int height,
    Color? color = null)
  {
    batch.Draw(sheet, new Rectangle(x, y, width, height), new Rectangle?(sprite), color ?? Color.White);
  }

  public static Vector2 DrawHoverBox(
    SpriteBatch spriteBatch,
    string label,
    in Vector2 position,
    float wrapWidth)
  {
    SpriteBatch batch1 = spriteBatch;
    SpriteFont smallFont1 = Game1.smallFont;
    string text1 = label;
    // ISSUE: explicit reference operation
    ref Vector2 local1 = @Vector2.op_Addition(position, new Vector2(20f));
    double wrapWidth1 = (double) wrapWidth;
    Color? nullable = new Color?();
    ref Color? local2 = ref nullable;
    Vector2 vector2 = batch1.DrawTextBlock(smallFont1, text1, in local1, (float) wrapWidth1, in local2);
    IClickableMenu.drawTextureBox(spriteBatch, Game1.menuTexture, new Rectangle(0, 256 /*0x0100*/, 60, 60), (int) position.X, (int) position.Y, (int) vector2.X + 27 + 20, (int) vector2.Y + 27, Color.White, 1f, true, -1f);
    SpriteBatch batch2 = spriteBatch;
    SpriteFont smallFont2 = Game1.smallFont;
    string text2 = label;
    // ISSUE: explicit reference operation
    ref Vector2 local3 = @Vector2.op_Addition(position, new Vector2(20f));
    double wrapWidth2 = (double) wrapWidth;
    nullable = new Color?();
    ref Color? local4 = ref nullable;
    batch2.DrawTextBlock(smallFont2, text2, in local3, (float) wrapWidth2, in local4);
    return Vector2.op_Addition(vector2, new Vector2(27f));
  }

  public static void DrawTab(
    SpriteBatch spriteBatch,
    int x,
    int y,
    int innerWidth,
    int innerHeight,
    out Vector2 innerDrawPosition,
    int align = 0,
    float alpha = 1f,
    bool forIcon = false,
    bool drawShadow = true)
  {
    int num1 = innerWidth + 32 /*0x20*/;
    int num2 = innerHeight + 21;
    int num3;
    switch (align)
    {
      case 1:
        num3 = -num1 / 2;
        break;
      case 2:
        num3 = -num1;
        break;
      default:
        num3 = 0;
        break;
    }
    int num4 = num3;
    int num5 = forIcon ? -4 : 0;
    int num6 = forIcon ? -8 : 0;
    innerDrawPosition = new Vector2((float) (x + 16 /*0x10*/ + num4 + num5), (float) (y + 16 /*0x10*/ + num6));
    IClickableMenu.drawTextureBox(spriteBatch, Game1.menuTexture, new Rectangle(0, 256 /*0x0100*/, 60, 60), x + num4, y, num1, num2 + 4, Color.op_Multiply(Color.White, alpha), 1f, drawShadow, -1f);
  }

  public static void DrawButton(
    SpriteBatch spriteBatch,
    in Vector2 position,
    in Vector2 contentSize,
    out Vector2 contentPos,
    out Rectangle bounds,
    int padding = 0)
  {
    CommonHelper.DrawContentBox(spriteBatch, CommonSprites.Button.Sheet, in CommonSprites.Button.Background, in CommonSprites.Button.Top, in CommonSprites.Button.Right, in CommonSprites.Button.Bottom, in CommonSprites.Button.Left, in CommonSprites.Button.TopLeft, in CommonSprites.Button.TopRight, in CommonSprites.Button.BottomRight, in CommonSprites.Button.BottomLeft, in position, in contentSize, out contentPos, out bounds, padding);
  }

  public static void DrawScroll(
    SpriteBatch spriteBatch,
    in Vector2 position,
    in Vector2 contentSize,
    out Vector2 contentPos,
    out Rectangle bounds,
    int padding = 5)
  {
    CommonHelper.DrawContentBox(spriteBatch, CommonSprites.Scroll.Sheet, in CommonSprites.Scroll.Background, in CommonSprites.Scroll.Top, in CommonSprites.Scroll.Right, in CommonSprites.Scroll.Bottom, in CommonSprites.Scroll.Left, in CommonSprites.Scroll.TopLeft, in CommonSprites.Scroll.TopRight, in CommonSprites.Scroll.BottomRight, in CommonSprites.Scroll.BottomLeft, in position, in contentSize, out contentPos, out bounds, padding);
  }

  public static void DrawContentBox(
    SpriteBatch spriteBatch,
    Texture2D texture,
    in Rectangle background,
    in Rectangle top,
    in Rectangle right,
    in Rectangle bottom,
    in Rectangle left,
    in Rectangle topLeft,
    in Rectangle topRight,
    in Rectangle bottomRight,
    in Rectangle bottomLeft,
    in Vector2 position,
    in Vector2 contentSize,
    out Vector2 contentPos,
    out Rectangle bounds,
    int padding)
  {
    int innerWidth;
    int innerHeight;
    int outerWidth;
    int outerHeight;
    int borderWidth;
    int borderHeight;
    CommonHelper.GetContentBoxDimensions(topLeft, contentSize, padding, out innerWidth, out innerHeight, out outerWidth, out outerHeight, out borderWidth, out borderHeight);
    int x = (int) position.X;
    int y = (int) position.Y;
    spriteBatch.Draw(texture, new Rectangle(x + borderWidth, y + borderHeight, innerWidth, innerHeight), new Rectangle?(background), Color.White);
    spriteBatch.Draw(texture, new Rectangle(x + borderWidth, y, innerWidth, borderHeight), new Rectangle?(top), Color.White);
    spriteBatch.Draw(texture, new Rectangle(x + borderWidth, y + borderHeight + innerHeight, innerWidth, borderHeight), new Rectangle?(bottom), Color.White);
    spriteBatch.Draw(texture, new Rectangle(x, y + borderHeight, borderWidth, innerHeight), new Rectangle?(left), Color.White);
    spriteBatch.Draw(texture, new Rectangle(x + borderWidth + innerWidth, y + borderHeight, borderWidth, innerHeight), new Rectangle?(right), Color.White);
    spriteBatch.Draw(texture, new Rectangle(x, y, borderWidth, borderHeight), new Rectangle?(topLeft), Color.White);
    spriteBatch.Draw(texture, new Rectangle(x, y + borderHeight + innerHeight, borderWidth, borderHeight), new Rectangle?(bottomLeft), Color.White);
    spriteBatch.Draw(texture, new Rectangle(x + borderWidth + innerWidth, y, borderWidth, borderHeight), new Rectangle?(topRight), Color.White);
    spriteBatch.Draw(texture, new Rectangle(x + borderWidth + innerWidth, y + borderHeight + innerHeight, borderWidth, borderHeight), new Rectangle?(bottomRight), Color.White);
    contentPos = new Vector2((float) (x + borderWidth + padding), (float) (y + borderHeight + padding));
    bounds = new Rectangle(x, y, outerWidth, outerHeight);
  }

  public static void ShowInfoMessage(string message, int? duration = null)
  {
    Game1.addHUDMessage(new HUDMessage(message, 3)
    {
      noIcon = true,
      timeLeft = (float) ((double) duration ?? 3500.0)
    });
  }

  public static void ShowErrorMessage(string message)
  {
    Game1.addHUDMessage(new HUDMessage(message, 3));
  }

  public static void GetScrollDimensions(
    Vector2 contentSize,
    int padding,
    out int innerWidth,
    out int innerHeight,
    out int labelOuterWidth,
    out int outerHeight,
    out int borderWidth,
    out int borderHeight)
  {
    CommonHelper.GetContentBoxDimensions(CommonSprites.Scroll.TopLeft, contentSize, padding, out innerWidth, out innerHeight, out labelOuterWidth, out outerHeight, out borderWidth, out borderHeight);
  }

  public static void GetContentBoxDimensions(
    Rectangle topLeft,
    Vector2 contentSize,
    int padding,
    out int innerWidth,
    out int innerHeight,
    out int outerWidth,
    out int outerHeight,
    out int borderWidth,
    out int borderHeight)
  {
    borderWidth = topLeft.Width * 4;
    borderHeight = topLeft.Height * 4;
    innerWidth = (int) ((double) contentSize.X + (double) (padding * 2));
    innerHeight = (int) ((double) contentSize.Y + (double) (padding * 2));
    outerWidth = innerWidth + borderWidth * 2;
    outerHeight = innerHeight + borderHeight * 2;
  }

  public static void DrawLine(
    this SpriteBatch batch,
    float x,
    float y,
    in Vector2 size,
    in Color? color = null)
  {
    batch.Draw(CommonHelper.Pixel, new Rectangle((int) x, (int) y, (int) size.X, (int) size.Y), color ?? Color.White);
  }

  public static Vector2 DrawTextBlock(
    this SpriteBatch batch,
    SpriteFont font,
    string? text,
    in Vector2 position,
    float wrapWidth,
    in Color? color = null,
    bool bold = false,
    float scale = 1f)
  {
    if (text == null)
      return new Vector2(0.0f, 0.0f);
    List<string> stringList = new List<string>();
    foreach (string str in text.Split(' ', StringSplitOptions.RemoveEmptyEntries))
    {
      int length;
      while ((length = str.IndexOf(Environment.NewLine, StringComparison.Ordinal)) >= 0)
      {
        if (length == 0)
        {
          stringList.Add(Environment.NewLine);
          str = str.Substring(Environment.NewLine.Length);
        }
        else if (length > 0)
        {
          stringList.Add(str.Substring(0, length));
          stringList.Add(Environment.NewLine);
          str = str.Substring(length + Environment.NewLine.Length);
        }
      }
      if (str.Length > 0)
        stringList.Add(str);
    }
    float num1 = 0.0f;
    float num2 = 0.0f;
    float num3 = font.MeasureString("ABC").Y * scale;
    float num4 = CommonHelper.GetSpaceWidth(font) * scale;
    float num5 = 0.0f;
    float num6 = num3;
    foreach (string str1 in stringList)
    {
      float num7 = font.MeasureString(str1).X * scale;
      if (str1 == Environment.NewLine || (double) num7 + (double) num1 > (double) wrapWidth && (int) num1 != 0)
      {
        num1 = 0.0f;
        num2 += num3;
        num6 += num3;
      }
      if (!(str1 == Environment.NewLine))
      {
        Vector2 vector2_1;
        // ISSUE: explicit constructor call
        ((Vector2) ref vector2_1).\u002Ector(position.X + num1, position.Y + num2);
        Color? nullable;
        if (bold)
        {
          SpriteBatch spriteBatch = batch;
          string str2 = str1;
          SpriteFont spriteFont = font;
          Vector2 vector2_2 = vector2_1;
          nullable = color;
          Color color1 = nullable ?? Color.Black;
          double num8 = (double) scale;
          Utility.drawBoldText(spriteBatch, str2, spriteFont, vector2_2, color1, (float) num8, -1f, 1);
        }
        else
        {
          SpriteBatch spriteBatch = batch;
          SpriteFont spriteFont = font;
          string str3 = str1;
          Vector2 vector2_3 = vector2_1;
          nullable = color;
          Color color2 = nullable ?? Color.Black;
          Vector2 zero = Vector2.Zero;
          double num9 = (double) scale;
          spriteBatch.DrawString(spriteFont, str3, vector2_3, color2, 0.0f, zero, (float) num9, (SpriteEffects) 0, 1f);
        }
        if ((double) num1 + (double) num7 > (double) num5)
          num5 = num1 + num7;
        num1 += num7 + num4;
      }
    }
    return new Vector2(num5, num6);
  }

  public static void InterceptErrors(
    this IMonitor monitor,
    string verb,
    Action action,
    Action<Exception>? onError = null)
  {
    monitor.InterceptErrors(verb, (string) null, action, onError);
  }

  public static void InterceptErrors(
    this IMonitor monitor,
    string verb,
    string? detailedVerb,
    Action action,
    Action<Exception>? onError = null)
  {
    try
    {
      action();
    }
    catch (Exception ex)
    {
      monitor.InterceptError(ex, verb, detailedVerb);
      if (onError == null)
        return;
      onError(ex);
    }
  }

  public static void InterceptError(
    this IMonitor monitor,
    Exception ex,
    string verb,
    string? detailedVerb = null)
  {
    if (detailedVerb == null)
      detailedVerb = verb;
    monitor.Log($"Something went wrong {detailedVerb}:\n{ex}", (LogLevel) 4);
    CommonHelper.ShowErrorMessage($"Huh. Something went wrong {verb}. The error log has the technical details.");
  }

  public static void RemoveObsoleteFiles(IMod mod, params string[] relativePaths)
  {
    string directoryPath = mod.Helper.DirectoryPath;
    foreach (string relativePath in relativePaths)
    {
      string path = Path.Combine(directoryPath, relativePath);
      if (File.Exists(path))
      {
        try
        {
          File.Delete(path);
          mod.Monitor.Log($"Removed obsolete file '{relativePath}'.", (LogLevel) 0);
        }
        catch (Exception ex)
        {
          mod.Monitor.Log($"Failed deleting obsolete file '{relativePath}':\n{ex}", (LogLevel) 0);
        }
      }
    }
  }

  public static string GetFileHash(string absolutePath)
  {
    using (FileStream inputStream = File.OpenRead(absolutePath))
    {
      using (MD5 md5 = MD5.Create())
        return BitConverter.ToString(md5.ComputeHash((Stream) inputStream)).Replace("-", "").ToLowerInvariant();
    }
  }
}
