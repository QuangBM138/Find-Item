// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.Fields.ColorField
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pathoschild.Stardew.Common;
using StardewValley;
using StardewValley.Menus;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework.Fields;

internal class ColorField : GenericField
{
  private readonly Color Color;
  private readonly int Strength;
  private readonly bool IsPrismatic;

  public ColorField(string label, Item item)
    : base(label)
  {
    if (item.Name == "Prismatic Shard")
    {
      this.IsPrismatic = true;
      this.HasValue = true;
    }
    else
    {
      Color? dyeColor = TailoringMenu.GetDyeColor(item);
      if (dyeColor.HasValue)
      {
        this.Color = dyeColor.Value;
        this.HasValue = true;
      }
    }
    if (!this.HasValue)
      return;
    if (item.HasContextTag("dye_strong"))
      this.Strength = 3;
    else if (item.HasContextTag("dye_medium"))
      this.Strength = 2;
    else
      this.Strength = 1;
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
    float num = 0.0f;
    Color color = this.IsPrismatic ? Utility.GetPrismaticColor(0, 1f) : this.Color;
    for (int index = 0; index < this.Strength; ++index)
    {
      spriteBatch.DrawSpriteWithin(CommonHelper.Pixel, new Rectangle(0, 0, 1, 1), position.X + num, position.Y, size, new Color?(color));
      num += size.X + 2f;
    }
    return new Vector2?(new Vector2(wrapWidth, size.Y + 5f));
  }
}
