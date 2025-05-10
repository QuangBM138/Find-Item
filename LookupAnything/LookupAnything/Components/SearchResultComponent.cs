// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Components.SearchResultComponent
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pathoschild.Stardew.Common;
using Pathoschild.Stardew.LookupAnything.Framework.Lookups;
using StardewValley;
using StardewValley.Menus;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Components;

internal class SearchResultComponent : ClickableComponent
{
  public const int FixedHeight = 70;

  public ISubject Subject { get; }

  public int Index { get; }

  public SearchResultComponent(ISubject subject, int index)
    : base(Rectangle.Empty, subject.Name)
  {
    this.Subject = subject;
    this.Index = index;
  }

  public Vector2 Draw(SpriteBatch spriteBatch, Vector2 position, int width, bool highlight = false)
  {
    this.bounds.X = (int) position.X;
    this.bounds.Y = (int) position.Y;
    this.bounds.Width = width;
    this.bounds.Height = 70;
    int num1 = 70;
    int num2 = this.bounds.Height / 2;
    if (highlight)
      DrawHelper.DrawLine(spriteBatch, (float) this.bounds.X, (float) this.bounds.Y, new Vector2((float) this.bounds.Width, (float) this.bounds.Height), new Color?(Color.Beige));
    DrawHelper.DrawLine(spriteBatch, (float) this.bounds.X, (float) this.bounds.Y, new Vector2((float) this.bounds.Width, 2f), new Color?(Color.Black));
    spriteBatch.DrawTextBlock(Game1.smallFont, $"{this.Subject.Name} ({this.Subject.Type})", Vector2.op_Addition(new Vector2((float) this.bounds.X, (float) this.bounds.Y), new Vector2((float) num1, (float) num2)), (float) (this.bounds.Width - num1));
    this.Subject.DrawPortrait(spriteBatch, position, new Vector2((float) num1));
    return new Vector2((float) this.bounds.Width, (float) this.bounds.Height);
  }
}
