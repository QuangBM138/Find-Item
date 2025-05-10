// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.Common.UI.Dropdown`1
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley.Menus;
using System;
using System.Collections.Generic;

#nullable enable
namespace Pathoschild.Stardew.Common.UI;

internal class Dropdown<TItem> : ClickableComponent
{
  private readonly SpriteFont Font;
  private readonly DropdownList<TItem> List;
  private readonly int BorderWidth = CommonSprites.Tab.TopLeft.Width * 2 * 4;
  private bool IsExpandedImpl;

  private bool IsAndroid => Constants.TargetPlatform == 0;

  public bool IsExpanded
  {
    get => this.IsExpandedImpl;
    set
    {
      this.IsExpandedImpl = value;
      this.downNeighborID = value ? this.List.TopComponentId : this.DefaultDownNeighborId;
    }
  }

  public TItem Selected => this.List.SelectedValue;

  public int DefaultDownNeighborId { get; set; } = -99999;

  public Dropdown(
    int x,
    int y,
    SpriteFont font,
    TItem? selectedItem,
    TItem[] items,
    Func<TItem, string> getLabel)
    : base(Rectangle.Empty, (object) selectedItem != null ? getLabel(selectedItem) : string.Empty)
  {
    this.Font = font;
    this.List = new DropdownList<TItem>(selectedItem, items, getLabel, x, y, font);
    this.bounds.X = x;
    this.bounds.Y = y;
    this.ReinitializeComponents();
  }

  public virtual bool containsPoint(int x, int y)
  {
    if (base.containsPoint(x, y))
      return true;
    return this.IsExpanded && ((ClickableComponent) this.List).containsPoint(x, y);
  }

  public bool TryClick(int x, int y) => this.TryClick(x, y, out bool _, out bool _);

  public bool TryClick(int x, int y, out bool itemClicked, out bool dropdownToggled)
  {
    itemClicked = false;
    dropdownToggled = false;
    if (this.IsExpanded && this.List.TryClick(x, y, out itemClicked))
    {
      if (itemClicked)
      {
        this.IsExpanded = false;
        dropdownToggled = true;
      }
      return true;
    }
    if (!((Rectangle) ref this.bounds).Contains(x, y) && !this.IsExpanded)
      return false;
    this.IsExpanded = !this.IsExpanded;
    dropdownToggled = true;
    return true;
  }

  public bool TrySelect(TItem value) => this.List.TrySelect(value);

  public void ReceiveScrollWheelAction(int direction)
  {
    if (!this.IsExpanded)
      return;
    this.List.ReceiveScrollWheelAction(direction);
  }

  public void Draw(SpriteBatch sprites, float opacity = 1f)
  {
    Vector2 innerDrawPosition;
    CommonHelper.DrawTab(sprites, this.bounds.X, this.bounds.Y, this.List.MaxLabelWidth, this.List.MaxLabelHeight, out innerDrawPosition, drawShadow: this.IsAndroid);
    sprites.DrawString(this.Font, this.List.SelectedLabel, innerDrawPosition, Color.op_Multiply(Color.Black, opacity));
    if (!this.IsExpanded)
      return;
    this.List.Draw(sprites, opacity);
  }

  public void ReinitializeComponents()
  {
    this.bounds.Height = (int) this.Font.MeasureString("ABCDEFGHIJKLMNOPQRSTUVWXYZ").Y - 10 + this.BorderWidth;
    this.bounds.Width = this.List.MaxLabelWidth + this.BorderWidth;
    this.List.bounds.X = this.bounds.X;
    this.List.bounds.Y = ((Rectangle) ref this.bounds).Bottom;
    this.List.ReinitializeComponents();
    this.ReinitializeControllerFlow();
  }

  public void ReinitializeControllerFlow()
  {
    this.List.ReinitializeControllerFlow();
    this.IsExpanded = this.IsExpanded;
  }

  public IEnumerable<ClickableComponent> GetChildComponents() => this.List.GetChildComponents();
}
