// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Components.LookupMenu
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pathoschild.Stardew.Common;
using Pathoschild.Stardew.Common.UI;
using Pathoschild.Stardew.Common.Utilities;
using Pathoschild.Stardew.LookupAnything.Framework;
using Pathoschild.Stardew.LookupAnything.Framework.Constants;
using Pathoschild.Stardew.LookupAnything.Framework.DebugFields;
using Pathoschild.Stardew.LookupAnything.Framework.Fields;
using Pathoschild.Stardew.LookupAnything.Framework.Lookups;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Components;

internal class LookupMenu : BaseMenu, IScrollableMenu, IDisposable
{
  private readonly ISubject Subject;
  private readonly IMonitor Monitor;
  private readonly Action<ISubject> ShowNewPage;
  private readonly ICustomField[] Fields;
  private readonly Vector2 AspectRatio = new Vector2((float) Sprites.Letter.Sprite.Width, (float) Sprites.Letter.Sprite.Height);
  private readonly IReflectionHelper Reflection;
  private readonly int ScrollAmount;
  private readonly bool ForceFullScreen;
  private readonly ClickableTextureComponent ScrollUpButton;
  private readonly ClickableTextureComponent ScrollDownButton;
  private readonly int ScrollButtonGutter = 15;
  private readonly BlendState ContentBlendState = new BlendState()
  {
    AlphaBlendFunction = (BlendFunction) 0,
    AlphaSourceBlend = (Blend) 1,
    AlphaDestinationBlend = (Blend) 0,
    ColorBlendFunction = (BlendFunction) 0,
    ColorSourceBlend = (Blend) 4,
    ColorDestinationBlend = (Blend) 5
  };
  private int MaxScroll;
  private int CurrentScroll;
  private bool ValidatedDrawMode;
  private readonly Dictionary<ICustomField, Rectangle> LinkableFieldAreas = new Dictionary<ICustomField, Rectangle>((IEqualityComparer<ICustomField>) new ObjectReferenceComparer<ICustomField>());
  private readonly bool WasHudEnabled;
  private bool ExitOnNextTick;

  public LookupMenu(
    ISubject subject,
    IMonitor monitor,
    IReflectionHelper reflectionHelper,
    int scroll,
    bool showDebugFields,
    bool forceFullScreen,
    Action<ISubject> showNewPage)
  {
    this.Subject = subject;
    this.Fields = subject.GetData().Where<ICustomField>((Func<ICustomField, bool>) (p => p.HasValue)).ToArray<ICustomField>();
    this.Monitor = monitor;
    this.Reflection = reflectionHelper;
    this.ScrollAmount = scroll;
    this.ForceFullScreen = forceFullScreen;
    this.ShowNewPage = showNewPage;
    this.WasHudEnabled = Game1.displayHUD;
    if (showDebugFields)
      this.Fields = ((IEnumerable<ICustomField>) this.Fields).Concat<ICustomField>(subject.GetDebugFields().GroupBy<IDebugField, string>((Func<IDebugField, string>) (p =>
      {
        if (p.IsPinned)
          return "debug (pinned)";
        return p.OverrideCategory != null ? $"debug ({p.OverrideCategory})" : "debug (raw)";
      })).OrderByDescending<IGrouping<string, IDebugField>, bool>((Func<IGrouping<string, IDebugField>, bool>) (p => p.Key == "debug (pinned)")).Select<IGrouping<string, IDebugField>, ICustomField>((Func<IGrouping<string, IDebugField>, ICustomField>) (p => (ICustomField) new DataMiningField(p.Key, (IEnumerable<IDebugField>) p)))).ToArray<ICustomField>();
    this.ScrollUpButton = new ClickableTextureComponent(Rectangle.Empty, CommonSprites.Icons.Sheet, CommonSprites.Icons.UpArrow, 1f, false);
    this.ScrollDownButton = new ClickableTextureComponent(Rectangle.Empty, CommonSprites.Icons.Sheet, CommonSprites.Icons.DownArrow, 1f, false);
    this.UpdateLayout();
    Game1.displayHUD = false;
  }

  public virtual void receiveLeftClick(int x, int y, bool playSound = true)
  {
    this.HandleLeftClick(x, y);
  }

  public virtual void receiveRightClick(int x, int y, bool playSound = true)
  {
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

  public virtual void receiveGamePadButton(Buttons button)
  {
    if (button <= 8192 /*0x2000*/)
    {
      if (button != 4096 /*0x1000*/)
      {
        if (button != 8192 /*0x2000*/)
          return;
        this.exitThisMenu(true);
      }
      else
      {
        Point mousePosition = Game1.getMousePosition();
        this.HandleLeftClick(mousePosition.X, mousePosition.Y);
      }
    }
    else if (button != 16777216 /*0x01000000*/)
    {
      if (button != 33554432 /*0x02000000*/)
        return;
      this.ScrollDown(new int?());
    }
    else
      this.ScrollUp(new int?());
  }

  public virtual void update(GameTime time)
  {
    if (this.ExitOnNextTick && this.readyToClose())
      this.exitThisMenu(true);
    else
      base.update(time);
  }

  public void QueueExit() => this.ExitOnNextTick = true;

  public void ScrollUp(int? amount = null) => this.CurrentScroll -= amount ?? this.ScrollAmount;

  public void ScrollDown(int? amount = null) => this.CurrentScroll += amount ?? this.ScrollAmount;

  public void HandleLeftClick(int x, int y)
  {
    if (!this.isWithinBounds(x, y) || ((ClickableComponent) this.upperRightCloseButton).containsPoint(x, y))
      this.exitThisMenu(true);
    else if (((ClickableComponent) this.ScrollUpButton).containsPoint(x, y))
      this.ScrollUp(new int?());
    else if (((ClickableComponent) this.ScrollDownButton).containsPoint(x, y))
    {
      this.ScrollDown(new int?());
    }
    else
    {
      foreach ((ICustomField key, Rectangle rectangle) in this.LinkableFieldAreas)
      {
        ISubject subject;
        if (((Rectangle) ref rectangle).Contains(x, y) && key.TryGetLinkAt(x, y, out subject))
        {
          this.ShowNewPage(subject);
          break;
        }
      }
    }
  }

  public virtual void draw(SpriteBatch b)
  {
    this.Monitor.InterceptErrors("drawing the lookup info", (Action) (() =>
    {
      this.LinkableFieldAreas.Clear();
      ISubject subject = this.Subject;
      if (!this.ValidatedDrawMode)
      {
        if (this.Reflection.GetField<SpriteSortMode>((object) Game1.spriteBatch, "_sortMode", true).GetValue() == 1)
        {
          this.Monitor.Log("Aborted the lookup because the game's current rendering mode isn't compatible with the mod's UI. This only happens in rare cases (e.g. the Stardew Valley Fair).", (LogLevel) 3);
          this.exitThisMenu(false);
          return;
        }
        this.ValidatedDrawMode = true;
      }
      int positionOnScreen1 = this.xPositionOnScreen;
      int positionOnScreen2 = this.yPositionOnScreen;
      float num1 = 15f;
      float num2 = 15f;
      float num3 = (float) (this.width - 30);
      float num4 = (float) (this.height - 30);
      int num5 = 1;
      SpriteFont font = Game1.smallFont;
      float y1 = font.MeasureString("ABC").Y;
      float spaceWidth = DrawHelper.GetSpaceWidth(font);
      using (SpriteBatch spriteBatch1 = new SpriteBatch(Game1.graphics.GraphicsDevice))
      {
        float num6 = this.width >= this.height ? (float) this.width / (float) Sprites.Letter.Sprite.Width : (float) this.height / (float) Sprites.Letter.Sprite.Height;
        spriteBatch1.Begin((SpriteSortMode) 0, BlendState.NonPremultiplied, SamplerState.PointClamp, (DepthStencilState) null, (RasterizerState) null, (Effect) null, new Matrix?());
        SpriteBatch spriteBatch2 = spriteBatch1;
        Texture2D sheet = Sprites.Letter.Sheet;
        Rectangle sprite1 = Sprites.Letter.Sprite;
        double x = (double) positionOnScreen1;
        double y2 = (double) positionOnScreen2;
        Rectangle sprite2 = Sprites.Letter.Sprite;
        Point size = ((Rectangle) ref sprite2).Size;
        float num7 = num6;
        Color? color = new Color?();
        double scale = (double) num7;
        spriteBatch2.DrawSprite(sheet, sprite1, (float) x, (float) y2, size, color, (float) scale);
        spriteBatch1.End();
      }
      using (SpriteBatch spriteBatch3 = new SpriteBatch(Game1.graphics.GraphicsDevice))
      {
        GraphicsDevice graphicsDevice = Game1.graphics.GraphicsDevice;
        Rectangle scissorRectangle = graphicsDevice.ScissorRectangle;
        try
        {
          graphicsDevice.ScissorRectangle = new Rectangle(positionOnScreen1 + 15, positionOnScreen2 + 15, (int) num3, (int) num4);
          SpriteBatch spriteBatch4 = spriteBatch3;
          BlendState contentBlendState = this.ContentBlendState;
          SamplerState pointClamp = SamplerState.PointClamp;
          RasterizerState rasterizerState = new RasterizerState();
          rasterizerState.ScissorTestEnable = true;
          Matrix? nullable = new Matrix?();
          spriteBatch4.Begin((SpriteSortMode) 0, contentBlendState, pointClamp, (DepthStencilState) null, rasterizerState, (Effect) null, nullable);
          this.CurrentScroll = Math.Max(0, this.CurrentScroll);
          this.CurrentScroll = Math.Min(this.MaxScroll, this.CurrentScroll);
          float num8 = num2 - (float) this.CurrentScroll;
          if (subject.DrawPortrait(spriteBatch3, new Vector2((float) positionOnScreen1 + num1, (float) positionOnScreen2 + num8), new Vector2(70f, 70f)))
            num1 += 72f;
          float wrapWidth1 = (float) ((double) this.width - (double) num1 - 15.0);
          SpriteBatch batch = spriteBatch3;
          SpriteFont font1 = font;
          string text = subject.Name + ".";
          Vector2 position1 = new Vector2((float) positionOnScreen1 + num1, (float) positionOnScreen2 + num8);
          double wrapWidth2 = (double) wrapWidth1;
          bool allowBold = Constant.AllowBold;
          Color? color = new Color?();
          int num9 = allowBold ? 1 : 0;
          Vector2 vector2_1 = batch.DrawTextBlock(font1, text, position1, (float) wrapWidth2, color, num9 != 0);
          Vector2 vector2_2 = subject.Type != null ? spriteBatch3.DrawTextBlock(font, subject.Type + ".", new Vector2((float) positionOnScreen1 + num1 + vector2_1.X + spaceWidth, (float) positionOnScreen2 + num8), wrapWidth1) : Vector2.Zero;
          float num10 = num8 + Math.Max(vector2_1.Y, vector2_2.Y);
          if (subject.Description != null)
          {
            Vector2 vector2_3 = spriteBatch3.DrawTextBlock(font, subject.Description?.Replace(Environment.NewLine, " "), new Vector2((float) positionOnScreen1 + num1, (float) positionOnScreen2 + num10), wrapWidth1);
            num10 += vector2_3.Y;
          }
          float num11 = num10 + y1;
          if (((IEnumerable<ICustomField>) this.Fields).Any<ICustomField>())
          {
            ICustomField[] fields = this.Fields;
            float num12 = 3f;
            float num13 = ((IEnumerable<ICustomField>) fields).Where<ICustomField>((Func<ICustomField, bool>) (p => p.HasValue)).Max<ICustomField>((Func<ICustomField, float>) (p => font.MeasureString(p.Label).X));
            float wrapWidth3 = (float) ((double) wrapWidth1 - (double) num13 - (double) num12 * 4.0) - (float) num5;
            foreach (ICustomField key in fields)
            {
              if (key.HasValue)
              {
                Vector2 vector2_4 = spriteBatch3.DrawTextBlock(font, key.Label, new Vector2((float) positionOnScreen1 + num1 + num12, (float) positionOnScreen2 + num11 + num12), wrapWidth1);
                Vector2 position2;
                // ISSUE: explicit constructor call
                ((Vector2) ref position2).\u002Ector((float) ((double) positionOnScreen1 + (double) num1 + (double) num13 + (double) num12 * 3.0), (float) positionOnScreen2 + num11 + num12);
                Vector2 vector2_5 = key.ExpandLink == null ? key.DrawValue(spriteBatch3, font, position2, wrapWidth3) ?? spriteBatch3.DrawTextBlock(font, (IEnumerable<IFormattedText>) key.Value, position2, wrapWidth3) : spriteBatch3.DrawTextBlock(font, (IEnumerable<IFormattedText>) key.ExpandLink.Value, position2, wrapWidth3);
                Vector2 vector2_6;
                // ISSUE: explicit constructor call
                ((Vector2) ref vector2_6).\u002Ector((float) ((double) num13 + (double) wrapWidth3 + (double) num12 * 4.0), Math.Max(vector2_4.Y, vector2_5.Y));
                Color gray = Color.Gray;
                DrawHelper.DrawLine(spriteBatch3, (float) positionOnScreen1 + num1, (float) positionOnScreen2 + num11, new Vector2(vector2_6.X, (float) num5), new Color?(gray));
                DrawHelper.DrawLine(spriteBatch3, (float) positionOnScreen1 + num1, (float) positionOnScreen2 + num11 + vector2_6.Y, new Vector2(vector2_6.X, (float) num5), new Color?(gray));
                DrawHelper.DrawLine(spriteBatch3, (float) positionOnScreen1 + num1, (float) positionOnScreen2 + num11, new Vector2((float) num5, vector2_6.Y), new Color?(gray));
                DrawHelper.DrawLine(spriteBatch3, (float) ((double) positionOnScreen1 + (double) num1 + (double) num13 + (double) num12 * 2.0), (float) positionOnScreen2 + num11, new Vector2((float) num5, vector2_6.Y), new Color?(gray));
                DrawHelper.DrawLine(spriteBatch3, (float) positionOnScreen1 + num1 + vector2_6.X, (float) positionOnScreen2 + num11, new Vector2((float) num5, vector2_6.Y), new Color?(gray));
                if (key != null && key.MayHaveLinks && key.HasValue)
                  this.LinkableFieldAreas[key] = new Rectangle((int) position2.X, (int) position2.Y, (int) vector2_5.X, (int) vector2_5.Y);
                num11 += Math.Max(vector2_4.Y, vector2_5.Y);
              }
            }
          }
          this.MaxScroll = Math.Max(0, (int) ((double) num11 - (double) num4 + (double) this.CurrentScroll));
          if (this.MaxScroll > 0 && this.CurrentScroll > 0)
            this.ScrollUpButton.draw(b);
          if (this.MaxScroll > 0 && this.CurrentScroll < this.MaxScroll)
            this.ScrollDownButton.draw(b);
          spriteBatch3.End();
        }
        catch (ArgumentException ex) when (
        {
          // ISSUE: unable to correctly present filter
          int num14;
          if (!BaseMenu.UseSafeDimensions && ex.ParamName == "value")
          {
            string stackTrace = ex.StackTrace;
            num14 = stackTrace != null ? (stackTrace.Contains("Microsoft.Xna.Framework.Graphics.GraphicsDevice.set_ScissorRectangle") ? 1 : 0) : 0;
          }
          else
            num14 = 0;
          if ((uint) num14 > 0U)
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
    }), new Action<Exception>(this.OnDrawError));
  }

  public void Dispose()
  {
    ((GraphicsResource) this.ContentBlendState).Dispose();
    this.CleanupImpl();
  }

  private void UpdateLayout()
  {
    Point viewportSize = this.GetViewportSize();
    if (this.ForceFullScreen)
    {
      this.xPositionOnScreen = 0;
      this.yPositionOnScreen = 0;
      this.width = viewportSize.X;
      this.height = viewportSize.Y;
    }
    else
    {
      this.width = Math.Min(1280 /*0x0500*/, viewportSize.X);
      this.height = Math.Min((int) ((double) this.AspectRatio.Y / (double) this.AspectRatio.X * (double) this.width), viewportSize.Y);
      Vector2 vector2;
      // ISSUE: explicit constructor call
      ((Vector2) ref vector2).\u002Ector((float) (viewportSize.X / 2 - this.width / 2), (float) (viewportSize.Y / 2 - this.height / 2));
      this.xPositionOnScreen = (int) vector2.X;
      this.yPositionOnScreen = (int) vector2.Y;
    }
    int positionOnScreen1 = this.xPositionOnScreen;
    int positionOnScreen2 = this.yPositionOnScreen;
    int scrollButtonGutter = this.ScrollButtonGutter;
    float num = (float) (this.height - scrollButtonGutter * 2);
    ((ClickableComponent) this.ScrollUpButton).bounds = new Rectangle(positionOnScreen1 + scrollButtonGutter, (int) ((double) positionOnScreen2 + (double) num - (double) CommonSprites.Icons.UpArrow.Height - (double) scrollButtonGutter - (double) CommonSprites.Icons.DownArrow.Height), CommonSprites.Icons.UpArrow.Height, CommonSprites.Icons.UpArrow.Width);
    ((ClickableComponent) this.ScrollDownButton).bounds = new Rectangle(positionOnScreen1 + scrollButtonGutter, (int) ((double) positionOnScreen2 + (double) num - (double) CommonSprites.Icons.DownArrow.Height), CommonSprites.Icons.DownArrow.Height, CommonSprites.Icons.DownArrow.Width);
    this.initializeUpperRightCloseButton();
  }

  private void OnDrawError(Exception ex)
  {
    this.Monitor.InterceptErrors("handling an error in the lookup code", (Action) (() => this.exitThisMenu(true)));
  }

  protected virtual void cleanupBeforeExit()
  {
    this.CleanupImpl();
    base.cleanupBeforeExit();
  }

  private void CleanupImpl() => Game1.displayHUD = this.WasHudEnabled;
}
