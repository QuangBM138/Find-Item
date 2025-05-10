// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Components.Sprites
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pathoschild.Stardew.Common;
using StardewValley;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Components;

internal static class Sprites
{
  public static readonly Texture2D Pixel = CommonHelper.Pixel;

  public static class Letter
  {
    public static readonly Rectangle Sprite = new Rectangle(0, 0, 320, 180);

    public static Texture2D Sheet => Game1.content.Load<Texture2D>("LooseSprites\\letterBG");
  }

  public static class Textbox
  {
    public static Texture2D Sheet => Game1.content.Load<Texture2D>("LooseSprites\\textBox");
  }
}
