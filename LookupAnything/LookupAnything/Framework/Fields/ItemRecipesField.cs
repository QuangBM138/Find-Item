// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.Fields.ItemRecipesField
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Netcode;
using Pathoschild.Stardew.Common;
using Pathoschild.Stardew.LookupAnything.Framework.Fields.Models;
using Pathoschild.Stardew.LookupAnything.Framework.Lookups;
using Pathoschild.Stardew.LookupAnything.Framework.Models;
using StardewValley;
using StardewValley.ItemTypeDefinitions;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework.Fields;

internal class ItemRecipesField : GenericField
{
  private readonly RecipeByTypeGroup[] RecipesByType;
  private readonly GameHelper GameHelper;
  private readonly ISubjectRegistry Codex;
  private readonly bool ShowUnknownRecipes;
  private readonly bool ShowInvalidRecipes;
  private readonly bool ShowLabelForSingleGroup;
  private readonly bool ShowOutputLabels;
  private readonly float LineHeight = Game1.smallFont.MeasureString("ABC").Y;
  private readonly Item? Ingredient;

  private float IconSize => this.LineHeight;

  public ItemRecipesField(
    GameHelper gameHelper,
    ISubjectRegistry codex,
    string label,
    Item? ingredient,
    RecipeModel[] recipes,
    bool showUnknownRecipes,
    bool showInvalidRecipes,
    bool showLabelForSingleGroup = true,
    bool showOutputLabels = true)
    : base(label, true)
  {
    this.GameHelper = gameHelper;
    this.Codex = codex;
    this.RecipesByType = this.BuildRecipeGroups(ingredient, recipes).ToArray<RecipeByTypeGroup>();
    this.ShowUnknownRecipes = showUnknownRecipes;
    this.ShowInvalidRecipes = showInvalidRecipes;
    this.ShowLabelForSingleGroup = showLabelForSingleGroup;
    this.ShowOutputLabels = showOutputLabels;
    this.Ingredient = ingredient;
  }

  public int GetShownRecipesCount()
  {
    int shownRecipesCount = 0;
    foreach (RecipeByTypeGroup recipeByTypeGroup in this.RecipesByType)
    {
      foreach (RecipeEntry recipe in recipeByTypeGroup.Recipes)
      {
        if ((recipe.IsValid || this.ShowInvalidRecipes) && (recipe.IsKnown || this.ShowUnknownRecipes))
          ++shownRecipesCount;
      }
    }
    return shownRecipesCount;
  }

  public override Vector2? DrawValue(
    SpriteBatch spriteBatch,
    SpriteFont font,
    Vector2 position,
    float wrapWidth)
  {
    this.LinkTextAreas.Clear();
    Color white = Color.White;
    Color black = Color.Black;
    Color blue = Color.Blue;
    Color color1 = Color.op_Multiply(white, 0.5f);
    Color gray = Color.Gray;
    Color color2 = Color.op_Multiply(blue, 0.65f);
    float x = font.MeasureString("+").X;
    float num1 = x;
    Vector2 position1 = position;
    float absoluteWrapWidth1 = position.X + wrapWidth;
    float lineHeight = this.LineHeight;
    Vector2 vector2_1;
    // ISSUE: explicit constructor call
    ((Vector2) ref vector2_1).\u002Ector(this.IconSize);
    float num2 = x + num1 * 2f;
    position1.Y += 6f;
    foreach (RecipeByTypeGroup recipeByTypeGroup in this.RecipesByType)
    {
      bool flag = (double) wrapWidth >= (double) recipeByTypeGroup.TotalColumnWidth + (double) num1 + (double) (recipeByTypeGroup.ColumnWidths.Length - 1) * (double) num2;
      if (this.ShowLabelForSingleGroup || this.RecipesByType.Length > 1)
      {
        position1.X = position.X + 0.0f;
        position1 = Vector2.op_Addition(position1, this.DrawIconText(spriteBatch, font, position1, absoluteWrapWidth1, recipeByTypeGroup.Type + ":", Color.Black));
      }
      int count = 0;
      foreach (RecipeEntry recipe in recipeByTypeGroup.Recipes)
      {
        if (this.ShowInvalidRecipes || recipe.IsValid)
        {
          if (!this.ShowUnknownRecipes && !recipe.IsKnown)
          {
            ++count;
          }
          else
          {
            Color color3 = recipe.IsKnown ? white : color1;
            Color textColor1 = recipe.IsKnown ? black : gray;
            float num3 = position.X + 14f;
            // ISSUE: explicit constructor call
            ((Vector2) ref position1).\u002Ector(num3, position1.Y + 5f);
            float num4 = 0.0f;
            if (this.ShowOutputLabels)
            {
              ISubject subject = this.GetSubject(recipe.Output.Entity);
              Color textColor2 = subject != null ? (recipe.IsKnown ? blue : color2) : textColor1;
              Vector2 vector2_2 = this.DrawIconText(spriteBatch, font, position1, absoluteWrapWidth1, recipe.Output.DisplayText, textColor2, recipe.Output.Sprite, new Vector2?(vector2_1), new Color?(color3), recipe.Output.Quality);
              float num5 = flag ? recipeByTypeGroup.ColumnWidths[0] : vector2_2.X;
              if (subject != null)
              {
                Rectangle PixelArea;
                // ISSUE: explicit constructor call
                ((Rectangle) ref PixelArea).\u002Ector((int) position1.X, (int) position1.Y, (int) num5, (int) lineHeight);
                this.LinkTextAreas.Add(new LinkTextArea(subject, PixelArea));
              }
              num4 = position1.X + num5 + num1;
              position1.X = num4;
            }
            int index1 = 0;
            for (int index2 = recipe.Inputs.Length - 1; index1 <= index2; ++index1)
            {
              RecipeItemEntry input = recipe.Inputs[index1];
              ISubject subject = this.GetSubject(input.Entity);
              Vector2 vector2_3 = vector2_1;
              if ((object) input != null && input.IsGoldPrice && input.Sprite != null)
              {
                Rectangle sourceRectangle = input.Sprite.SourceRectangle;
                vector2_3 = Vector2.op_Multiply(Utility.PointToVector2(((Rectangle) ref sourceRectangle).Size), 4f);
              }
              Vector2 vector2_4 = this.DrawIconText(spriteBatch, font, position1, absoluteWrapWidth1, input.DisplayText, textColor1, input.Sprite, new Vector2?(vector2_3), new Color?(color3), input.Quality, true);
              if (flag)
                vector2_4.X = recipeByTypeGroup.ColumnWidths[index1 + 1];
              if ((double) position1.X + (double) vector2_4.X > (double) absoluteWrapWidth1)
              {
                // ISSUE: explicit constructor call
                ((Vector2) ref position1).\u002Ector(num4, (float) ((double) position1.Y + (double) lineHeight + 2.0));
              }
              Color textColor3 = subject != null ? (recipe.IsKnown ? blue : color2) : textColor1;
              this.DrawIconText(spriteBatch, font, position1, absoluteWrapWidth1, input.DisplayText, textColor3, input.Sprite, new Vector2?(vector2_3), new Color?(color3), input.Quality);
              if (subject != null)
              {
                Rectangle PixelArea;
                // ISSUE: explicit constructor call
                ((Rectangle) ref PixelArea).\u002Ector((int) position1.X, (int) position1.Y, (int) vector2_4.X, (int) lineHeight);
                this.LinkTextAreas.Add(new LinkTextArea(subject, PixelArea));
              }
              // ISSUE: explicit constructor call
              ((Vector2) ref position1).\u002Ector(position1.X + vector2_4.X, position1.Y);
              if (index1 != index2)
              {
                if ((double) position1.X + (double) num2 > (double) absoluteWrapWidth1)
                {
                  // ISSUE: explicit constructor call
                  ((Vector2) ref position1).\u002Ector(num4, (float) ((double) position1.Y + (double) lineHeight + 2.0));
                }
                else
                  position1.X += num1;
                Vector2 vector2_5 = this.DrawIconText(spriteBatch, font, position1, absoluteWrapWidth1, "+", textColor1);
                position1.X += vector2_5.X + num1;
              }
            }
            position1.X = num3;
            position1.Y += lineHeight;
            if (recipe.Conditions != null)
            {
              ref float local = ref position1.Y;
              double num6 = (double) local;
              SpriteBatch batch = spriteBatch;
              SpriteFont font1 = font;
              Vector2 vector2_6 = position1;
              vector2_6.X = position1.X + this.IconSize + (float) this.IconMargin;
              Vector2 position2 = vector2_6;
              double absoluteWrapWidth2 = (double) absoluteWrapWidth1;
              string text = I18n.ConditionsSummary((object) recipe.Conditions);
              Color textColor4 = textColor1;
              Vector2? iconSize = new Vector2?();
              Color? iconColor = new Color?();
              int? qualityIcon = new int?();
              double y = (double) this.DrawIconText(batch, font1, position2, (float) absoluteWrapWidth2, text, textColor4, iconSize: iconSize, iconColor: iconColor, qualityIcon: qualityIcon).Y;
              local = (float) (num6 + y);
            }
          }
        }
      }
      if (count > 0)
      {
        // ISSUE: explicit constructor call
        ((Vector2) ref position1).\u002Ector(position.X + 14f + (float) this.IconMargin + this.IconSize, position1.Y + 5f);
        this.DrawIconText(spriteBatch, font, position1, absoluteWrapWidth1, I18n.Item_UnknownRecipes((object) count), Color.Gray);
        position1.Y += lineHeight;
      }
      position1.Y += lineHeight;
    }
    position1.Y += 6f;
    return new Vector2?(new Vector2(wrapWidth, position1.Y - position.Y - lineHeight));
  }

  public override void CollapseIfLengthExceeds(int minResultsForCollapse, int countForLabel)
  {
    if (this.RecipesByType.Length != 0)
    {
      int count = ((IEnumerable<RecipeByTypeGroup>) this.RecipesByType).Sum<RecipeByTypeGroup>((Func<RecipeByTypeGroup, int>) (group => ((IEnumerable<RecipeEntry>) group.Recipes).Count<RecipeEntry>((Func<RecipeEntry, bool>) (recipe => this.ShowUnknownRecipes || recipe.IsKnown))));
      if (count < minResultsForCollapse)
        return;
      this.CollapseByDefault(I18n.Generic_ShowXResults((object) count));
    }
    else
      base.CollapseIfLengthExceeds(minResultsForCollapse, countForLabel);
  }

  private IEnumerable<RecipeByTypeGroup> BuildRecipeGroups(
    Item? ingredient,
    RecipeModel[] rawRecipes)
  {
    foreach ((string str, RecipeEntry[] Recipes) in ((IEnumerable<RecipeModel>) rawRecipes).Select<RecipeModel, RecipeEntry>((Func<RecipeModel, RecipeEntry>) (recipe =>
    {
      Item obj1 = ingredient == null || !recipe.IsForMachine(ingredient) ? recipe.TryCreateItem(ingredient) : recipe.TryCreateItem((Item) null);
      if (recipe.OutputQualifiedItemId == "__COMPLEX_RECIPE__")
      {
        string key = recipe.Key;
        string displayType = recipe.DisplayType;
        int num = recipe.IsKnown() ? 1 : 0;
        RecipeItemEntry[] inputs = Array.Empty<RecipeItemEntry>();
        string name = I18n.Item_RecipesForMachine_TooComplex();
        Item obj2 = obj1;
        SpriteInfo sprite = recipe.SpecialOutput?.Sprite;
        bool? nullable = new bool?(true);
        int? quality = new int?();
        bool? isValid = nullable;
        RecipeItemEntry itemEntry = this.CreateItemEntry(name, obj2, sprite, quality: quality, isValid: isValid);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        string conditions = recipe.Conditions.Length != 0 ? I18n.List((IEnumerable<object>) ((IEnumerable<string>) recipe.Conditions).Select<string, string>(ItemRecipesField.\u003C\u003EO.\u003C0\u003E__Format ?? (ItemRecipesField.\u003C\u003EO.\u003C0\u003E__Format = new Func<string, string>(HumanReadableConditionParser.Format)))) : (string) null;
        return new RecipeEntry(key, displayType, num != 0, inputs, itemEntry, conditions);
      }
      RecipeItemEntry output = !(ItemRegistry.GetDataOrErrorItem(recipe.OutputQualifiedItemId)?.ItemId == "DROP_IN") ? this.CreateItemEntry(recipe.SpecialOutput?.DisplayText ?? obj1?.DisplayName ?? string.Empty, obj1, recipe.SpecialOutput?.Sprite, recipe.MinOutput, recipe.MaxOutput, recipe.OutputChance, recipe.Quality, true, recipe.SpecialOutput?.IsValid, recipe.SpecialOutput?.Entity) : this.CreateItemEntry(I18n.Item_RecipesForMachine_SameAsInput(), minCount: recipe.MinOutput, maxCount: recipe.MaxOutput, chance: recipe.OutputChance, quality: recipe.Quality, hasInputAndOutput: true, isValid: new bool?(true));
      IEnumerable<RecipeItemEntry> recipeItemEntries = ((IEnumerable<RecipeIngredientModel>) recipe.Ingredients).Select<RecipeIngredientModel, RecipeItemEntry>(new Func<RecipeIngredientModel, RecipeItemEntry>(this.TryCreateItemEntry)).WhereNotNull<RecipeItemEntry>();
      bool flag;
      switch (recipe.Type)
      {
        case RecipeType.MachineInput:
        case RecipeType.TailorInput:
          flag = true;
          break;
        default:
          flag = false;
          break;
      }
      if (!flag)
        recipeItemEntries = (IEnumerable<RecipeItemEntry>) recipeItemEntries.OrderBy<RecipeItemEntry, string>((Func<RecipeItemEntry, string>) (entry => entry.DisplayText));
      if (recipe.GoldPrice > 0)
      {
        // ISSUE: object of a compiler-generated type is created
        recipeItemEntries = recipeItemEntries.Concat<RecipeItemEntry>((IEnumerable<RecipeItemEntry>) new \u003C\u003Ez__ReadOnlySingleElementList<RecipeItemEntry>(new RecipeItemEntry(new SpriteInfo(Game1.debrisSpriteSheet, new Rectangle(5, 69, 6, 6)), Utility.getNumberWithCommas(recipe.GoldPrice), new int?(), true)));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return new RecipeEntry(recipe.Key, recipe.DisplayType, recipe.IsKnown(), recipeItemEntries.ToArray<RecipeItemEntry>(), output, recipe.Conditions.Length != 0 ? I18n.List((IEnumerable<object>) ((IEnumerable<string>) recipe.Conditions).Select<string, string>(ItemRecipesField.\u003C\u003EO.\u003C0\u003E__Format ?? (ItemRecipesField.\u003C\u003EO.\u003C0\u003E__Format = new Func<string, string>(HumanReadableConditionParser.Format)))) : (string) null);
    })).GroupBy<RecipeEntry, string>((Func<RecipeEntry, string>) (recipe => recipe.UniqueKey)).Select<IGrouping<string, RecipeEntry>, RecipeEntry>((Func<IGrouping<string, RecipeEntry>, RecipeEntry>) (item => item.First<RecipeEntry>())).OrderBy<RecipeEntry, string>((Func<RecipeEntry, string>) (recipe => recipe.Type)).ThenBy<RecipeEntry, string>((Func<RecipeEntry, string>) (recipe => recipe.Output.DisplayText)).GroupBy<RecipeEntry, string>((Func<RecipeEntry, string>) (p => p.Type)).ToDictionary<IGrouping<string, RecipeEntry>, string, RecipeEntry[]>((Func<IGrouping<string, RecipeEntry>, string>) (p => p.Key), (Func<IGrouping<string, RecipeEntry>, RecipeEntry[]>) (p => p.ToArray<RecipeEntry>())))
    {
      List<float> columnWidths = new List<float>();
      foreach (RecipeEntry recipeEntry in Recipes)
      {
        TrackWidth(0, recipeEntry.Output.DisplayText + ":", recipeEntry.Output.Sprite);
        for (int index = 0; index < recipeEntry.Inputs.Length; ++index)
          TrackWidth(index + 1, recipeEntry.Inputs[index].DisplayText, recipeEntry.Inputs[index].Sprite);
      }
      yield return new RecipeByTypeGroup(str, Recipes, columnWidths.ToArray());

      void TrackWidth(int index, string text, SpriteInfo? icon)
      {
        while (columnWidths.Count < index + 1)
          columnWidths.Add(0.0f);
        float x = Game1.smallFont.MeasureString(text).X;
        if (icon != null)
          x += this.IconSize + (float) this.IconMargin;
        columnWidths[index] = Math.Max(columnWidths[index], x);
      }
    }
  }

  private ISubject? GetSubject(object? entity)
  {
    if (entity == null)
      return (ISubject) null;
    return entity is Item obj && (obj.ItemId == "__COMPLEX_RECIPE__" || obj.QualifiedItemId == this.Ingredient?.QualifiedItemId) ? (ISubject) null : this.Codex.GetByEntity(entity, (GameLocation) null);
  }

  private RecipeItemEntry TryCreateItemEntry(RecipeIngredientModel ingredient)
  {
    if (ingredient.InputId == "-777")
    {
      string name = I18n.Item_WildSeeds();
      int count1 = ingredient.Count;
      int count2 = ingredient.Count;
      bool? nullable = new bool?(true);
      int? quality = new int?();
      bool? isValid = nullable;
      return this.CreateItemEntry(name, minCount: count1, maxCount: count2, quality: quality, isValid: isValid);
    }
    int result;
    if (int.TryParse(ingredient.InputId, out result) && result < 0)
    {
      Item obj = (Item) this.GameHelper.GetObjectsByCategory(result).FirstOrDefault<Object>();
      if (obj != null)
      {
        string str;
        switch (obj.Category)
        {
          case -6:
            str = Game1.content.LoadString("Strings\\StringsFromCSFiles:CraftingRecipe.cs.573");
            break;
          case -5:
            str = Game1.content.LoadString("Strings\\StringsFromCSFiles:CraftingRecipe.cs.572");
            break;
          default:
            str = obj.getCategoryName();
            break;
        }
        string name = str;
        int count3 = ingredient.Count;
        int count4 = ingredient.Count;
        bool? nullable = new bool?(true);
        int? quality = new int?();
        bool? isValid = nullable;
        return this.CreateItemEntry(name, minCount: count3, maxCount: count4, quality: quality, isValid: isValid);
      }
    }
    if (ingredient.InputId != null)
    {
      Item obj = ItemRegistry.Create(ingredient.InputId, 1, 0, true);
      if (obj is Object @object)
      {
        if (ingredient.PreservedItemId != null)
          ((NetFieldBase<string, NetString>) @object.preservedParentSheetIndex).Value = ingredient.PreservedItemId;
        Object.PreserveType? preserveType = ingredient.PreserveType;
        if (preserveType.HasValue)
        {
          NetNullableEnum<Object.PreserveType> preserve = @object.preserve;
          preserveType = ingredient.PreserveType;
          Object.PreserveType? nullable = new Object.PreserveType?(preserveType.Value);
          ((NetFieldBase<Object.PreserveType?, NetNullableEnum<Object.PreserveType>>) preserve).Value = nullable;
        }
      }
      if (obj != null)
      {
        string name = obj.DisplayName ?? obj.ItemId;
        if (ingredient.InputContextTags.Length != 0)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          name = $"{name} ({I18n.List((IEnumerable<object>) ((IEnumerable<string>) ingredient.InputContextTags).Select<string, string>(ItemRecipesField.\u003C\u003EO.\u003C1\u003E__Format ?? (ItemRecipesField.\u003C\u003EO.\u003C1\u003E__Format = new Func<string, string>(HumanReadableContextTagParser.Format))))})";
        }
        return this.CreateItemEntry(name, obj, minCount: ingredient.Count, maxCount: ingredient.Count);
      }
    }
    if (ingredient.InputContextTags.Length != 0)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      string name = I18n.List((IEnumerable<object>) ((IEnumerable<string>) ingredient.InputContextTags).Select<string, string>(ItemRecipesField.\u003C\u003EO.\u003C1\u003E__Format ?? (ItemRecipesField.\u003C\u003EO.\u003C1\u003E__Format = new Func<string, string>(HumanReadableContextTagParser.Format))));
      int count5 = ingredient.Count;
      int count6 = ingredient.Count;
      bool? nullable = new bool?(true);
      int? quality = new int?();
      bool? isValid = nullable;
      return this.CreateItemEntry(name, minCount: count5, maxCount: count6, quality: quality, isValid: isValid);
    }
    ObjectDataDefinition objectTypeDefinition = ItemRegistry.GetObjectTypeDefinition();
    string str1 = ingredient.InputId;
    if (ingredient.InputContextTags.Length != 0)
    {
      string str2;
      if (!string.IsNullOrWhiteSpace(str1))
      {
        object obj = (object) str1;
        string[] inputContextTags = ingredient.InputContextTags;
        int index1 = 0;
        object[] items = new object[1 + inputContextTags.Length];
        items[index1] = obj;
        int index2 = index1 + 1;
        foreach (string str3 in inputContextTags)
        {
          items[index2] = (object) str3;
          ++index2;
        }
        // ISSUE: object of a compiler-generated type is created
        str2 = I18n.List((IEnumerable<object>) new \u003C\u003Ez__ReadOnlyArray<object>(items));
      }
      else
        str2 = I18n.List((IEnumerable<object>) ingredient.InputContextTags);
      str1 = str2;
    }
    if (str1 == null)
      str1 = "???";
    string name1 = str1;
    SpriteInfo sprite = new SpriteInfo(((BaseItemDataDefinition) objectTypeDefinition).GetErrorTexture(), ((BaseItemDataDefinition) objectTypeDefinition).GetErrorSourceRect());
    bool? nullable1 = new bool?(false);
    int? quality1 = new int?();
    bool? isValid1 = nullable1;
    return this.CreateItemEntry(name1, sprite: sprite, quality: quality1, isValid: isValid1);
  }

  private RecipeItemEntry CreateItemEntry(
    string name,
    Item? item = null,
    SpriteInfo? sprite = null,
    int minCount = 1,
    int maxCount = 1,
    Decimal chance = 100M,
    int? quality = null,
    bool hasInputAndOutput = false,
    bool? isValid = null,
    object? entity = null)
  {
    string DisplayText = minCount == maxCount ? (minCount <= 1 ? name : I18n.Item_RecipesForMachine_MultipleItems((object) name, (object) minCount)) : I18n.Item_RecipesForMachine_MultipleItems((object) name, (object) I18n.Generic_Range((object) minCount, (object) maxCount));
    if (chance > 0M && chance < 100M)
      DisplayText = $"{DisplayText} ({I18n.Generic_Percent((object) chance)})";
    if (hasInputAndOutput)
      DisplayText += ":";
    return new RecipeItemEntry(sprite ?? this.GameHelper.GetSprite(item), DisplayText, quality, false, ((int) isValid ?? (item == null ? 0 : (ItemRegistry.Exists(item.QualifiedItemId) ? 1 : 0))) != 0, entity ?? (object) item);
  }
}
