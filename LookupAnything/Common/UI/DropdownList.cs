// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.Common.UI.DropdownList`1
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using xTile.Dimensions;

#nullable enable
namespace Pathoschild.Stardew.Common.UI;

internal class DropdownList<TValue> : ClickableComponent
{
  private const int DropdownPadding = 5;
  private DropdownList<
  #nullable disable
  TValue>.DropListOption SelectedOption;
  private readonly 
  #nullable enable
  DropdownList<
  #nullable disable
  TValue>.DropListOption[] Options;
  private int FirstVisibleIndex;
  private int MaxItems;
  private readonly 
  #nullable enable
  SpriteFont Font;
  private ClickableTextureComponent UpArrow;
  private ClickableTextureComponent DownArrow;

  private int LastVisibleIndex => this.FirstVisibleIndex + this.MaxItems - 1;

  private int MaxFirstVisibleIndex => this.Options.Length - this.MaxItems;

  private bool CanScrollUp => this.FirstVisibleIndex > 0;

  private bool CanScrollDown => this.FirstVisibleIndex < this.MaxFirstVisibleIndex;

  public TValue SelectedValue => this.SelectedOption.Value;

  public string SelectedLabel => this.SelectedOption.label;

  public int MaxLabelHeight { get; }

  public int MaxLabelWidth { get; private set; }

  public int TopComponentId
  {
    get
    {
      return ((IEnumerable<DropdownList<TValue>.DropListOption>) this.Options).First<DropdownList<TValue>.DropListOption>((Func<DropdownList<TValue>.DropListOption, bool>) (p => p.visible)).myID;
    }
  }

  public DropdownList(
    TValue? selectedValue,
    TValue[] items,
    Func<TValue, string> getLabel,
    int x,
    int y,
    SpriteFont font)
    : base(new Rectangle(), nameof (DropdownList<TValue>))
  {
    this.Options = ((IEnumerable<TValue>) items).Select<TValue, DropdownList<TValue>.DropListOption>((Func<TValue, int, DropdownList<TValue>.DropListOption>) ((item, index) => new DropdownList<TValue>.DropListOption(Rectangle.Empty, index, getLabel(item), item, font))).ToArray<DropdownList<TValue>.DropListOption>();
    this.Font = font;
    this.MaxLabelHeight = ((IEnumerable<DropdownList<TValue>.DropListOption>) this.Options).Max<DropdownList<TValue>.DropListOption>((Func<DropdownList<TValue>.DropListOption, int>) (p => p.LabelHeight));
    int index1 = Array.IndexOf<TValue>(items, selectedValue);
    this.SelectedOption = index1 >= 0 ? this.Options[index1] : ((IEnumerable<DropdownList<TValue>.DropListOption>) this.Options).First<DropdownList<TValue>.DropListOption>();
    this.bounds.X = x;
    this.bounds.Y = y;
    this.ReinitializeComponents();
  }

  public void ReceiveScrollWheelAction(int direction) => this.Scroll(direction > 0 ? -1 : 1);

  public bool TryClick(int x, int y, out bool itemClicked)
  {
    DropdownList<TValue>.DropListOption dropListOption = ((IEnumerable<DropdownList<TValue>.DropListOption>) this.Options).FirstOrDefault<DropdownList<TValue>.DropListOption>((Func<DropdownList<TValue>.DropListOption, bool>) (p => p.visible && p.containsPoint(x, y)));
    if (dropListOption != null)
    {
      this.SelectedOption = dropListOption;
      itemClicked = true;
      return true;
    }
    itemClicked = false;
    if (((ClickableComponent) this.UpArrow).containsPoint(x, y))
    {
      this.Scroll(-1);
      return true;
    }
    if (!((ClickableComponent) this.DownArrow).containsPoint(x, y))
      return false;
    this.Scroll(1);
    return true;
  }

  public bool TrySelect(TValue value)
  {
    DropdownList<TValue>.DropListOption dropListOption = ((IEnumerable<DropdownList<TValue>.DropListOption>) this.Options).FirstOrDefault<DropdownList<TValue>.DropListOption>((Func<DropdownList<TValue>.DropListOption, bool>) (p =>
    {
      if ((object) p.Value == null && (object) value == null)
        return true;
      TValue obj = p.Value;
      ref TValue local = ref obj;
      return (object) local != null && local.Equals((object) value);
    }));
    if (dropListOption == null)
      return false;
    this.SelectedOption = dropListOption;
    return true;
  }

  public virtual bool containsPoint(int x, int y)
  {
    return base.containsPoint(x, y) || ((ClickableComponent) this.UpArrow).containsPoint(x, y) || ((ClickableComponent) this.DownArrow).containsPoint(x, y);
  }

  public void Draw(SpriteBatch sprites, float opacity = 1f)
  {
    foreach (DropdownList<TValue>.DropListOption option in this.Options)
    {
      if (option.visible)
      {
        if (option.containsPoint(Game1.getMouseX(), Game1.getMouseY()))
          sprites.Draw(CommonSprites.DropDown.Sheet, option.bounds, new Rectangle?(CommonSprites.DropDown.HoverBackground), Color.op_Multiply(Color.White, opacity));
        else if (option.Index == this.SelectedOption.Index)
          sprites.Draw(CommonSprites.DropDown.Sheet, option.bounds, new Rectangle?(CommonSprites.DropDown.ActiveBackground), Color.op_Multiply(Color.White, opacity));
        else
          sprites.Draw(CommonSprites.DropDown.Sheet, option.bounds, new Rectangle?(CommonSprites.DropDown.InactiveBackground), Color.op_Multiply(Color.White, opacity));
        Vector2 vector2;
        // ISSUE: explicit constructor call
        ((Vector2) ref vector2).\u002Ector((float) (option.bounds.X + 5), (float) (option.bounds.Y + 4));
        sprites.DrawString(this.Font, option.label, vector2, Color.op_Multiply(Color.Black, opacity));
      }
    }
    if (this.CanScrollUp)
      this.UpArrow.draw(sprites, Color.op_Multiply(Color.White, opacity), 1f, 0, 0, 0);
    if (!this.CanScrollDown)
      return;
    this.DownArrow.draw(sprites, Color.op_Multiply(Color.White, opacity), 1f, 0, 0, 0);
  }

  [MemberNotNull(new string[] {"UpArrow", "DownArrow"})]
  public void ReinitializeComponents()
  {
    int x = this.bounds.X;
    int y = this.bounds.Y;
    int num1 = this.MaxLabelWidth = Math.Max(((IEnumerable<DropdownList<TValue>.DropListOption>) this.Options).Max<DropdownList<TValue>.DropListOption>((Func<DropdownList<TValue>.DropListOption, int>) (p => p.LabelWidth)), 128 /*0x80*/) + 10;
    int maxLabelHeight = this.MaxLabelHeight;
    this.MaxItems = Math.Min((((Rectangle) ref Game1.uiViewport).Height - y) / maxLabelHeight, this.Options.Length);
    this.FirstVisibleIndex = this.GetValidFirstItem(this.FirstVisibleIndex, this.MaxFirstVisibleIndex);
    this.bounds.Width = num1;
    this.bounds.Height = maxLabelHeight * this.MaxItems;
    int num2 = y;
    foreach (DropdownList<TValue>.DropListOption option in this.Options)
    {
      option.visible = option.Index >= this.FirstVisibleIndex && option.Index <= this.LastVisibleIndex;
      if (option.visible)
      {
        option.bounds = new Rectangle(x, num2, num1, maxLabelHeight);
        num2 += maxLabelHeight;
      }
    }
    Rectangle upArrow = CommonSprites.Icons.UpArrow;
    Rectangle downArrow = CommonSprites.Icons.DownArrow;
    this.UpArrow = new ClickableTextureComponent("up-arrow", new Rectangle(x - upArrow.Width, y, upArrow.Width, upArrow.Height), "", "", CommonSprites.Icons.Sheet, upArrow, 1f, false);
    this.DownArrow = new ClickableTextureComponent("down-arrow", new Rectangle(x - downArrow.Width, y + this.bounds.Height - downArrow.Height, downArrow.Width, downArrow.Height), "", "", CommonSprites.Icons.Sheet, downArrow, 1f, false);
    this.ReinitializeControllerFlow();
  }

  public void ReinitializeControllerFlow()
  {
    int firstVisibleIndex = this.FirstVisibleIndex;
    int lastVisibleIndex = this.LastVisibleIndex;
    int num1 = 1100000;
    foreach (DropdownList<TValue>.DropListOption option in this.Options)
    {
      int index = option.Index;
      int num2 = num1 + index;
      option.myID = num2;
      option.upNeighborID = index > firstVisibleIndex ? num2 - 1 : -99999;
      option.downNeighborID = index < lastVisibleIndex ? num2 + 1 : -1;
    }
  }

  public IEnumerable<ClickableComponent> GetChildComponents()
  {
    return (IEnumerable<ClickableComponent>) this.Options;
  }

  private void Scroll(int amount)
  {
    int validFirstItem = this.GetValidFirstItem(this.FirstVisibleIndex + amount, this.MaxFirstVisibleIndex);
    if (validFirstItem == this.FirstVisibleIndex)
      return;
    this.FirstVisibleIndex = validFirstItem;
    this.ReinitializeComponents();
  }

  private int GetValidFirstItem(int value, int maxIndex) => Math.Max(Math.Min(value, maxIndex), 0);

  private class DropListOption : ClickableComponent
  {
    public int Index { get; }

    public TValue Value { get; }

    public int LabelWidth { get; }

    public int LabelHeight { get; }

    public DropListOption(
      Rectangle bounds,
      int index,
      string label,
      TValue value,
      SpriteFont font)
      : base(bounds, index.ToString(), label)
    {
      this.Index = index;
      this.Value = value;
      Vector2 vector2 = font.MeasureString(label);
      this.LabelWidth = (int) vector2.X;
      this.LabelHeight = (int) vector2.Y;
    }
  }
}
