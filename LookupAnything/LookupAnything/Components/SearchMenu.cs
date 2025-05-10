// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Components.SearchMenu
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pathoschild.Stardew.Common;
using Pathoschild.Stardew.Common.UI;
using Pathoschild.Stardew.LookupAnything.Framework.Lookups;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Components;

internal class SearchMenu : BaseMenu, IScrollableMenu, IDisposable
{
  private readonly Action<ISubject> ShowLookup;
  private readonly IMonitor Monitor;
  private readonly Vector2 AspectRatio = new Vector2((float) Sprites.Letter.Sprite.Width, (float) Sprites.Letter.Sprite.Height);
  private readonly ClickableTextureComponent ScrollUpButton;
  private readonly ClickableTextureComponent ScrollDownButton;
  private readonly int ScrollAmount;
  private int MaxScroll;
  private int CurrentScroll;
  private readonly ILookup<string, ISubject> SearchLookup;
  private readonly SearchTextBox SearchTextbox;
  private IEnumerable<SearchResultComponent> SearchResults = (IEnumerable<SearchResultComponent>) Array.Empty<SearchResultComponent>();
  private Rectangle SearchResultArea;
  private readonly int SearchResultGutter = 15;
  private readonly int ScrollButtonGutter = 15;

  public SearchMenu(
    IEnumerable<ISubject> searchSubjects,
    Action<ISubject> showLookup,
    IMonitor monitor,
    int scroll)
  {
    this.ShowLookup = showLookup;
    this.Monitor = monitor;
    this.SearchLookup = searchSubjects.Where<ISubject>((Func<ISubject, bool>) (p => !string.IsNullOrWhiteSpace(p.Name))).ToLookup<ISubject, string>((Func<ISubject, string>) (p => p.Name), (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
    this.ScrollAmount = scroll;
    this.SearchTextbox = new SearchTextBox(Game1.smallFont, Color.Black);
    this.ScrollUpButton = new ClickableTextureComponent(Rectangle.Empty, CommonSprites.Icons.Sheet, CommonSprites.Icons.UpArrow, 1f, false);
    this.ScrollDownButton = new ClickableTextureComponent(Rectangle.Empty, CommonSprites.Icons.Sheet, CommonSprites.Icons.DownArrow, 1f, false);
    this.UpdateLayout();
    this.SearchTextbox.Select();
    this.SearchTextbox.OnChanged += (EventHandler<string>) ((_, text) => this.ReceiveSearchTextboxChanged(text));
  }

  public virtual void receiveLeftClick(int x, int y, bool playSound = true)
  {
    if (((ClickableComponent) this.upperRightCloseButton).containsPoint(x, y))
    {
      this.exitThisMenu(true);
    }
    else
    {
      Rectangle bounds = this.SearchTextbox.Bounds;
      if (((Rectangle) ref bounds).Contains(x, y))
        this.SearchTextbox.Select();
      else if (((ClickableComponent) this.ScrollUpButton).containsPoint(x, y))
        this.ScrollUp(new int?());
      else if (((ClickableComponent) this.ScrollDownButton).containsPoint(x, y))
      {
        this.ScrollDown(new int?());
      }
      else
      {
        if (!((Rectangle) ref this.SearchResultArea).Contains(x, y))
          return;
        foreach (SearchResultComponent searchResultComponent in this.GetResultsPossiblyOnScreen())
        {
          if (searchResultComponent.containsPoint(x, y))
          {
            this.ShowLookup(searchResultComponent.Subject);
            Game1.playSound("coin", new int?());
            break;
          }
        }
      }
    }
  }

  public virtual void receiveKeyPress(Keys key)
  {
    if (!key.Equals((object) (Keys) 27))
      return;
    this.exitThisMenu(true);
  }

  public virtual void receiveGamePadButton(Buttons button)
  {
    if (button != 8192 /*0x2000*/)
    {
      if (button != 16777216 /*0x01000000*/)
      {
        if (button == 33554432 /*0x02000000*/)
          this.ScrollDown(new int?());
        else
          base.receiveGamePadButton(button);
      }
      else
        this.ScrollUp(new int?());
    }
    else
      this.exitThisMenu(true);
  }

  public virtual void receiveScrollWheelAction(int direction)
  {
    if (direction > 0)
      this.ScrollUp(new int?());
    else
      this.ScrollDown(new int?());
  }

  public virtual void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
  {
    this.UpdateLayout();
  }

  public virtual void draw(SpriteBatch b)
  {
    int positionOnScreen1 = this.xPositionOnScreen;
    int positionOnScreen2 = this.yPositionOnScreen;
    int searchResultGutter = this.SearchResultGutter;
    float num1 = (float) searchResultGutter;
    float num2 = (float) searchResultGutter;
    float height = (float) this.SearchResultArea.Height;
    SpriteFont smallFont = Game1.smallFont;
    float y1 = smallFont.MeasureString("ABC").Y;
    float spaceWidth = DrawHelper.GetSpaceWidth(smallFont);
    using (SpriteBatch spriteBatch1 = new SpriteBatch(Game1.graphics.GraphicsDevice))
    {
      spriteBatch1.Begin((SpriteSortMode) 0, BlendState.NonPremultiplied, SamplerState.PointClamp, (DepthStencilState) null, (RasterizerState) null, (Effect) null, new Matrix?());
      SpriteBatch spriteBatch2 = spriteBatch1;
      Texture2D sheet = Sprites.Letter.Sheet;
      Rectangle sprite1 = Sprites.Letter.Sprite;
      double x = (double) positionOnScreen1;
      double y2 = (double) positionOnScreen2;
      Rectangle sprite2 = Sprites.Letter.Sprite;
      Point size = ((Rectangle) ref sprite2).Size;
      float num3 = (float) this.width / (float) Sprites.Letter.Sprite.Width;
      Color? color = new Color?();
      double scale = (double) num3;
      spriteBatch2.DrawSprite(sheet, sprite1, (float) x, (float) y2, size, color, (float) scale);
      spriteBatch1.End();
    }
    using (SpriteBatch spriteBatch3 = new SpriteBatch(Game1.graphics.GraphicsDevice))
    {
      GraphicsDevice graphicsDevice = Game1.graphics.GraphicsDevice;
      Rectangle scissorRectangle = graphicsDevice.ScissorRectangle;
      try
      {
        graphicsDevice.ScissorRectangle = this.SearchResultArea;
        SpriteBatch spriteBatch4 = spriteBatch3;
        BlendState nonPremultiplied = BlendState.NonPremultiplied;
        SamplerState pointClamp = SamplerState.PointClamp;
        RasterizerState rasterizerState = new RasterizerState();
        rasterizerState.ScissorTestEnable = true;
        Matrix? nullable = new Matrix?();
        spriteBatch4.Begin((SpriteSortMode) 0, nonPremultiplied, pointClamp, (DepthStencilState) null, rasterizerState, (Effect) null, nullable);
        this.CurrentScroll = Math.Max(0, this.CurrentScroll);
        this.CurrentScroll = Math.Min(this.MaxScroll, this.CurrentScroll);
        float num4 = num2 - (float) this.CurrentScroll;
        float num5 = (float) this.width - num1 - (float) searchResultGutter;
        Vector2 vector2_1 = spriteBatch3.DrawTextBlock(smallFont, "Search", new Vector2((float) positionOnScreen1 + num1, (float) positionOnScreen2 + num4), num5, bold: true);
        Vector2 vector2_2 = spriteBatch3.DrawTextBlock(smallFont, "(Lookup Anything)", new Vector2((float) positionOnScreen1 + num1 + vector2_1.X + spaceWidth, (float) positionOnScreen2 + num4), num5);
        float num6 = num4 + Math.Max(vector2_1.Y, vector2_2.Y);
        this.SearchTextbox.Bounds = new Rectangle(positionOnScreen1 + (int) num1, positionOnScreen2 + (int) num6, (int) num5, this.SearchTextbox.Bounds.Height);
        this.SearchTextbox.Draw(spriteBatch3);
        float num7 = num6 + (float) this.SearchTextbox.Bounds.Height;
        int mouseX = Game1.getMouseX();
        int mouseY = Game1.getMouseY();
        bool flag1 = false;
        bool flag2 = false;
        bool flag3 = ((Rectangle) ref this.SearchResultArea).Contains(mouseX, mouseY) && !((ClickableComponent) this.ScrollUpButton).containsPoint(mouseX, mouseY) && !((ClickableComponent) this.ScrollDownButton).containsPoint(mouseX, mouseY);
        foreach (SearchResultComponent searchResult in this.SearchResults)
        {
          if (!flag1 || !flag2)
          {
            if (this.IsResultPossiblyOnScreen(searchResult))
            {
              flag1 = true;
              bool highlight = flag3 && searchResult.containsPoint(mouseX, mouseY);
              searchResult.Draw(spriteBatch3, new Vector2((float) positionOnScreen1 + num1, (float) positionOnScreen2 + num7), (int) num5, highlight);
            }
            else if (flag1)
              flag2 = true;
          }
          num7 += 70f;
        }
        this.MaxScroll = Math.Max(0, (int) ((double) (num7 + y1) - (double) height + (double) this.CurrentScroll));
        if (this.MaxScroll > 0 && this.CurrentScroll > 0)
          this.ScrollUpButton.draw(b);
        if (this.MaxScroll > 0 && this.CurrentScroll < this.MaxScroll)
          this.ScrollDownButton.draw(b);
        spriteBatch3.End();
      }
      catch (ArgumentException ex) when (
      {
        // ISSUE: unable to correctly present filter
        int num8;
        if (!BaseMenu.UseSafeDimensions && ex.ParamName == "value")
        {
          string stackTrace = ex.StackTrace;
          num8 = stackTrace != null ? (stackTrace.Contains("Microsoft.Xna.Framework.Graphics.GraphicsDevice.set_ScissorRectangle") ? 1 : 0) : 0;
        }
        else
          num8 = 0;
        if ((uint) num8 > 0U)
        {
          SuccessfulFiltering;
        }
        else
          throw;
      }
      )
      {
        this.Monitor.Log("The viewport size seems to be inaccurate. Enabling compatibility mode; lookup menu may be misaligned.", (LogLevel) 3);
        this.Monitor.Log(ex.ToString(), (LogLevel) 0);
        BaseMenu.UseSafeDimensions = true;
        this.UpdateLayout();
      }
      finally
      {
        graphicsDevice.ScissorRectangle = scissorRectangle;
      }
    }
    this.upperRightCloseButton.draw(b);
    this.drawMouse(Game1.spriteBatch, false, -1);
  }

  public void Dispose() => this.SearchTextbox.Dispose();

  public void ScrollUp(int? amount = null) => this.CurrentScroll -= amount ?? this.ScrollAmount;

  public void ScrollDown(int? amount = null) => this.CurrentScroll += amount ?? this.ScrollAmount;

  private IEnumerable<SearchResultComponent> GetResultsPossiblyOnScreen()
  {
    bool reachedViewport = false;
    foreach (SearchResultComponent searchResult in this.SearchResults)
    {
      if (!this.IsResultPossiblyOnScreen(searchResult))
      {
        if (reachedViewport)
          break;
      }
      else
      {
        reachedViewport = true;
        yield return searchResult;
      }
    }
  }

  private bool IsResultPossiblyOnScreen(SearchResultComponent result)
  {
    int index = result.Index;
    int num = (index - 3) * 70;
    return (index + 3) * 70 > this.CurrentScroll && num < this.CurrentScroll + this.height;
  }

  private void ReceiveSearchTextboxChanged(string? search)
  {
    string[] words = (search ?? "").Split(' ', StringSplitOptions.RemoveEmptyEntries);
    if (!((IEnumerable<string>) words).Any<string>())
      this.SearchResults = (IEnumerable<SearchResultComponent>) Array.Empty<SearchResultComponent>();
    else
      this.SearchResults = (IEnumerable<SearchResultComponent>) this.SearchLookup.Where<IGrouping<string, ISubject>>((Func<IGrouping<string, ISubject>, bool>) (entry => ((IEnumerable<string>) words).All<string>((Func<string, bool>) (word => entry.Key.IndexOf(word, StringComparison.OrdinalIgnoreCase) >= 0)))).SelectMany<IGrouping<string, ISubject>, ISubject>((Func<IGrouping<string, ISubject>, IEnumerable<ISubject>>) (entry => (IEnumerable<ISubject>) entry)).OrderBy<ISubject, string>((Func<ISubject, string>) (subject => subject.Name), (IComparer<string>) StringComparer.OrdinalIgnoreCase).Select<ISubject, SearchResultComponent>((Func<ISubject, int, SearchResultComponent>) ((subject, index) => new SearchResultComponent(subject, index))).ToArray<SearchResultComponent>();
  }

  private void UpdateLayout()
  {
    Point viewportSize = this.GetViewportSize();
    this.width = Math.Min(896, viewportSize.X);
    this.height = Math.Min((int) ((double) this.AspectRatio.Y / (double) this.AspectRatio.X * (double) this.width), viewportSize.Y);
    Vector2 centeringOnScreen = Utility.getTopLeftPositionForCenteringOnScreen(this.width, this.height, 0, 0);
    int num1 = this.xPositionOnScreen = (int) centeringOnScreen.X;
    int num2 = this.yPositionOnScreen = (int) centeringOnScreen.Y;
    int searchResultGutter = this.SearchResultGutter;
    float num3 = (float) (this.width - searchResultGutter * 2);
    float num4 = (float) (this.height - searchResultGutter * 2);
    this.SearchResultArea = new Rectangle(num1 + searchResultGutter, num2 + searchResultGutter, (int) num3, (int) num4);
    int scrollButtonGutter = this.ScrollButtonGutter;
    ((ClickableComponent) this.ScrollUpButton).bounds = new Rectangle(num1 + scrollButtonGutter, (int) ((double) num2 + (double) num4 - (double) CommonSprites.Icons.UpArrow.Height - (double) scrollButtonGutter - (double) CommonSprites.Icons.DownArrow.Height), CommonSprites.Icons.UpArrow.Height, CommonSprites.Icons.UpArrow.Width);
    ((ClickableComponent) this.ScrollDownButton).bounds = new Rectangle(num1 + scrollButtonGutter, (int) ((double) num2 + (double) num4 - (double) CommonSprites.Icons.DownArrow.Height), CommonSprites.Icons.DownArrow.Height, CommonSprites.Icons.DownArrow.Width);
    this.initializeUpperRightCloseButton();
  }
}
