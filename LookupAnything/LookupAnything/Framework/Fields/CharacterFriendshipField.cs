// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Framework.Fields.CharacterFriendshipField
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pathoschild.Stardew.Common;
using Pathoschild.Stardew.Common.UI;
using Pathoschild.Stardew.LookupAnything.Framework.Models;
using System;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Framework.Fields;

internal class CharacterFriendshipField : GenericField
{
  private readonly FriendshipModel Friendship;

  public CharacterFriendshipField(string label, FriendshipModel friendship)
    : base(label, true)
  {
    this.Friendship = friendship;
  }

  public override Vector2? DrawValue(
    SpriteBatch spriteBatch,
    SpriteFont font,
    Vector2 position,
    float wrapWidth)
  {
    FriendshipModel friendship1 = this.Friendship;
    float num1 = 0.0f;
    string text1 = I18n.For(friendship1.Status, friendship1.IsHousemate);
    Vector2 vector2_1 = spriteBatch.DrawTextBlock(font, text1, new Vector2(position.X + num1, position.Y), wrapWidth - num1);
    float num2 = num1 + (vector2_1.X + DrawHelper.GetSpaceWidth(font));
    Rectangle rectangle1;
    for (int index = 0; index < friendship1.TotalHearts; ++index)
    {
      Rectangle rectangle2;
      Color color1;
      if (friendship1.LockedHearts >= friendship1.TotalHearts - index)
      {
        rectangle2 = CommonSprites.Icons.FilledHeart;
        color1 = Color.op_Multiply(Color.Black, 0.35f);
      }
      else if (index >= friendship1.FilledHearts)
      {
        rectangle2 = CommonSprites.Icons.EmptyHeart;
        color1 = Color.White;
      }
      else
      {
        rectangle2 = CommonSprites.Icons.FilledHeart;
        color1 = Color.White;
      }
      SpriteBatch spriteBatch1 = spriteBatch;
      Texture2D sheet = CommonSprites.Icons.Sheet;
      Rectangle sprite = rectangle2;
      double x = (double) position.X + (double) num2;
      double y = (double) position.Y;
      rectangle1 = CommonSprites.Icons.FilledHeart;
      Point size = ((Rectangle) ref rectangle1).Size;
      Color? color2 = new Color?(color1);
      spriteBatch1.DrawSprite(sheet, sprite, (float) x, (float) y, size, color2, 4f);
      num2 += (float) (CommonSprites.Icons.FilledHeart.Width * 4);
    }
    if (friendship1.HasStardrop)
    {
      float num3 = num2 + 1f;
      float num4 = (float) ((double) CommonSprites.Icons.EmptyHeart.Height / ((double) CommonSprites.Icons.Stardrop.Height * 1.0) * 4.0);
      SpriteBatch spriteBatch2 = spriteBatch;
      Texture2D sheet = CommonSprites.Icons.Sheet;
      Rectangle stardrop = CommonSprites.Icons.Stardrop;
      double x = (double) position.X + (double) num3;
      double y = (double) position.Y;
      rectangle1 = CommonSprites.Icons.Stardrop;
      Point size = ((Rectangle) ref rectangle1).Size;
      Color? color = new Color?(Color.op_Multiply(Color.White, 0.25f));
      double scale = (double) num4;
      spriteBatch2.DrawSprite(sheet, stardrop, (float) x, (float) y, size, color, (float) scale);
      num2 = num3 + (float) CommonSprites.Icons.Stardrop.Width * num4;
    }
    string text2 = (string) null;
    FriendshipModel friendship2 = this.Friendship;
    if ((object) friendship2 != null && friendship2.EmptyHearts == 0 && friendship2.LockedHearts > 0)
    {
      text2 = $"({I18n.Npc_Friendship_NeedBouquet()})";
    }
    else
    {
      int pointsToNext = this.Friendship.GetPointsToNext();
      if (pointsToNext > 0)
        text2 = $"({I18n.Npc_Friendship_NeedPoints((object) pointsToNext)})";
    }
    float spaceWidth = DrawHelper.GetSpaceWidth(font);
    Vector2 vector2_2 = Vector2.Zero;
    if (text2 != null)
      vector2_2 = spriteBatch.DrawTextBlock(font, text2, new Vector2(position.X + num2 + spaceWidth, position.Y), wrapWidth - num2);
    return new Vector2?(new Vector2((float) (CommonSprites.Icons.FilledHeart.Width * 4 * this.Friendship.TotalHearts) + vector2_2.X + spaceWidth, Math.Max((float) (CommonSprites.Icons.FilledHeart.Height * 4), vector2_2.Y)));
  }
}
