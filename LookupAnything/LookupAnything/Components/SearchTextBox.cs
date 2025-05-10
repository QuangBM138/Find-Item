// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Components.SearchTextBox
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley.Menus;
using System;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Components;

internal class SearchTextBox : IDisposable
{
  private readonly TextBox Textbox;
  private string LastText = string.Empty;
  private Rectangle BoundsImpl;

  public event EventHandler<string>? OnChanged;

  public Rectangle Bounds
  {
    get => this.BoundsImpl;
    set
    {
      this.BoundsImpl = value;
      this.Textbox.X = value.X;
      this.Textbox.Y = value.Y;
      this.Textbox.Width = value.Width;
      this.Textbox.Height = value.Height;
    }
  }

  public SearchTextBox(SpriteFont font, Color textColor)
  {
    this.Textbox = new TextBox(Sprites.Textbox.Sheet, (Texture2D) null, font, textColor);
    this.Bounds = new Rectangle(this.Textbox.X, this.Textbox.Y, this.Textbox.Width, this.Textbox.Height);
  }

  public void Select() => this.Textbox.Selected = true;

  public void Draw(SpriteBatch batch)
  {
    this.NotifyChange();
    this.Textbox.Draw(batch, true);
  }

  private void NotifyChange()
  {
    if (!(this.Textbox.Text != this.LastText))
      return;
    EventHandler<string> onChanged = this.OnChanged;
    if (onChanged != null)
      onChanged((object) this, this.Textbox.Text);
    this.LastText = this.Textbox.Text;
  }

  public void Dispose()
  {
    this.OnChanged = (EventHandler<string>) null;
    this.Textbox.Selected = false;
  }
}
