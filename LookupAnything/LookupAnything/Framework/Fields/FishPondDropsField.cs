// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.Fields.FishPondDropsField
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pathoschild.Stardew.Common;
using Pathoschild.Stardew.Common.UI;
using Pathoschild.Stardew.LookupAnything.Framework.Data;
using Pathoschild.Stardew.LookupAnything.Framework.Fields.Models;
using Pathoschild.Stardew.LookupAnything.Framework.Lookups;
using Pathoschild.Stardew.LookupAnything.Framework.Models;
using StardewValley;
using StardewValley.GameData.FishPonds;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework.Fields;

internal class FishPondDropsField : GenericField
{
  protected GameHelper GameHelper;
  private readonly ISubjectRegistry Codex;
  private readonly FishPondDrop[] Drops;
  private readonly string Preface;

  public FishPondDropsField(
    GameHelper gameHelper,
    ISubjectRegistry codex,
    string label,
    int currentPopulation,
    FishPondData data,
    Object? fish,
    string preface)
    : base(label)
  {
    this.GameHelper = gameHelper;
    this.Codex = codex;
    this.Drops = this.GetEntries(currentPopulation, data, fish, gameHelper).ToArray<FishPondDrop>();
    this.HasValue = ((IEnumerable<FishPondDrop>) this.Drops).Any<FishPondDrop>();
    this.Preface = preface;
  }

  public override Vector2? DrawValue(
    SpriteBatch spriteBatch,
    SpriteFont font,
    Vector2 position,
    float wrapWidth)
  {
    this.LinkTextAreas.Clear();
    float num1 = 0.0f;
    if (!string.IsNullOrWhiteSpace(this.Preface))
    {
      Vector2 vector2 = spriteBatch.DrawTextBlock(font, this.Preface, position, wrapWidth);
      num1 += (float) (int) vector2.Y;
    }
    float val1 = (float) (CommonSprites.Icons.FilledCheckbox.Width * 2);
    float num2 = (float) (((double) Math.Max(val1, Game1.smallFont.MeasureString("ABC").Y) - (double) val1) / 2.0);
    float num3 = val1 + 7f;
    float num4 = num3 * 2f;
    Vector2 size;
    // ISSUE: explicit constructor call
    ((Vector2) ref size).\u002Ector(font.MeasureString("ABC").Y);
    int num5 = -1;
    bool flag1 = false;
    foreach (FishPondDrop drop in this.Drops)
    {
      bool flag2 = !drop.IsUnlocked | flag1;
      if (num5 != drop.MinPopulation)
      {
        num5 = drop.MinPopulation;
        spriteBatch.Draw(CommonSprites.Icons.Sheet, new Vector2(position.X + num3, position.Y + num1 + num2), new Rectangle?(drop.IsUnlocked ? CommonSprites.Icons.FilledCheckbox : CommonSprites.Icons.EmptyCheckbox), Color.op_Multiply(Color.White, flag2 ? 0.5f : 1f), 0.0f, Vector2.Zero, val1 / (float) CommonSprites.Icons.FilledCheckbox.Width, (SpriteEffects) 0, 1f);
        Vector2 vector2 = spriteBatch.DrawTextBlock(Game1.smallFont, I18n.Building_FishPond_Drops_MinFish((object) drop.MinPopulation), new Vector2((float) ((double) position.X + (double) num3 + (double) val1 + 7.0), position.Y + num1), (float) ((double) wrapWidth - (double) val1 - 7.0), new Color?(flag2 ? Color.Gray : Color.Black));
        if (flag1)
          DrawHelper.DrawLine(spriteBatch, (float) ((double) position.X + (double) num3 + (double) val1 + 7.0), (float) ((double) position.Y + (double) num1 + (double) size.Y / 2.0), new Vector2(vector2.X, 1f), new Color?(Color.Gray));
        num1 += Math.Max(val1, vector2.Y);
      }
      bool flag3 = (double) drop.Probability > 0.99000000953674316;
      ISubject byEntity = this.Codex.GetByEntity((object) drop.SampleItem, (GameLocation) null);
      Color color = Color.op_Multiply(byEntity != null ? Color.Blue : Color.Black, flag2 ? 0.75f : 1f);
      spriteBatch.DrawSpriteWithin(drop.Sprite, position.X + num4, position.Y + num1, size, new Color?(Color.op_Multiply(Color.White, flag2 ? 0.5f : 1f)));
      float num6 = (float) ((double) position.X + (double) num4 + (double) size.X + 5.0);
      string text1 = I18n.Generic_PercentChanceOf((object) (int) (Math.Round((double) drop.Probability, 4) * 100.0), (object) drop.SampleItem.DisplayName);
      if (drop.MinDrop != drop.MaxDrop)
        text1 = $"{text1} ({I18n.Generic_Range((object) drop.MinDrop, (object) drop.MaxDrop)})";
      else if (drop.MinDrop > 1)
        text1 += $" ({drop.MinDrop})";
      Vector2 vector2_1 = spriteBatch.DrawTextBlock(font, text1, new Vector2(num6, (float) ((double) position.Y + (double) num1 + 5.0)), wrapWidth, new Color?(color));
      if (byEntity != null)
      {
        Rectangle PixelArea;
        // ISSUE: explicit constructor call
        ((Rectangle) ref PixelArea).\u002Ector((int) ((double) position.X + (double) num4 + (double) size.X + 5.0), (int) ((double) position.Y + (double) num1 + (double) size.Y / 2.0), (int) vector2_1.X, (int) vector2_1.Y);
        this.LinkTextAreas.Add(new LinkTextArea(byEntity, PixelArea));
      }
      if (flag1)
        DrawHelper.DrawLine(spriteBatch, (float) ((double) position.X + (double) num4 + (double) size.X + 5.0), (float) ((double) position.Y + (double) num1 + (double) size.Y / 2.0), new Vector2(vector2_1.X, 1f), new Color?(Color.Gray));
      if (drop.Conditions != null)
      {
        string text2 = I18n.ConditionsSummary((object) HumanReadableConditionParser.Format(drop.Conditions));
        num1 += vector2_1.Y + 5f;
        vector2_1 = spriteBatch.DrawTextBlock(font, text2, new Vector2(num6, (float) ((double) position.Y + (double) num1 + 5.0)), wrapWidth);
        if (flag1)
          DrawHelper.DrawLine(spriteBatch, (float) ((double) position.X + (double) size.X + 5.0), (float) ((double) position.Y + (double) num1 + (double) size.Y / 2.0), new Vector2(vector2_1.X, 1f), new Color?(flag2 ? Color.Gray : Color.Black));
      }
      num1 += vector2_1.Y + 5f;
      if (drop.IsUnlocked & flag3)
        flag1 = true;
    }
    return new Vector2?(new Vector2(wrapWidth, num1));
  }

  private IEnumerable<FishPondDrop> GetEntries(
    int currentPopulation,
    FishPondData data,
    Object? fish,
    GameHelper gameHelper)
  {
    foreach (FishPondDropData fishPondDrop in gameHelper.GetFishPondDrops(data))
    {
      FishPondDropData data1 = fishPondDrop;
      if (fish != null && data1.Conditions != null)
      {
        string conditions = data1.Conditions;
        if (this.FilterConditions(fish, ref conditions))
        {
          if (conditions != data1.Conditions)
            data1 = new FishPondDropData(data1.MinPopulation, data1.ItemId, data1.MinDrop, data1.MaxDrop, data1.Probability, conditions);
        }
        else
          continue;
      }
      bool isUnlocked = currentPopulation >= data1.MinPopulation;
      Item sampleItem = ItemRegistry.Create(data1.ItemId, 1, 0, false);
      SpriteInfo sprite = gameHelper.GetSprite(sampleItem);
      yield return new FishPondDrop(data1, sampleItem, sprite, isUnlocked);
    }
  }

  private bool FilterConditions(Object fish, ref string? gameStateQuery)
  {
    if (GameStateQuery.IsImmutablyTrue(gameStateQuery))
    {
      gameStateQuery = (string) null;
      return true;
    }
    if (GameStateQuery.IsImmutablyFalse(gameStateQuery))
      return false;
    List<string> list = ((IEnumerable<string>) GameStateQuery.SplitRaw(gameStateQuery)).ToList<string>();
    int count = list.Count;
    for (int index = list.Count - 1; index >= 0; --index)
    {
      GameStateQuery.ParsedGameStateQuery[] parsedGameStateQueryArray = GameStateQuery.Parse(list[index]);
      if (parsedGameStateQueryArray.Length == 1)
      {
        string upperInvariant = parsedGameStateQueryArray[0].Query[0].ToUpperInvariant();
        if (upperInvariant != null)
        {
          switch (upperInvariant.Length)
          {
            case 7:
              if (upperInvariant == "ITEM_ID")
                break;
              continue;
            case 9:
              if (!(upperInvariant == "ITEM_TYPE"))
                continue;
              break;
            case 13:
              if (upperInvariant == "ITEM_CATEGORY")
                break;
              continue;
            case 14:
              if (upperInvariant == "ITEM_ID_PREFIX")
                break;
              continue;
            case 15:
              if (upperInvariant == "ITEM_NUMERIC_ID")
                break;
              continue;
            case 16 /*0x10*/:
              if (upperInvariant == "ITEM_OBJECT_TYPE")
                break;
              continue;
            case 33:
              if (upperInvariant == "ITEM_HAS_EXPLICIT_OBJECT_CATEGORY")
                break;
              continue;
            default:
              continue;
          }
          if (!GameStateQuery.CheckConditions(list[index], (GameLocation) null, (Farmer) null, (Item) null, (Item) fish, (Random) null, (HashSet<string>) null))
            return false;
          list.RemoveAt(index);
        }
      }
    }
    if (list.Count == 0)
      gameStateQuery = (string) null;
    else if (list.Count != count)
      gameStateQuery = string.Join(", ", (IEnumerable<string>) list);
    return true;
  }
}
