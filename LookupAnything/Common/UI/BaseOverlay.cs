// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.Common.UI.BaseOverlay
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using System;
using System.Reflection;
using xTile.Dimensions;

#nullable enable
namespace Pathoschild.Stardew.Common.UI;

internal abstract class BaseOverlay : IDisposable
{
  private readonly IModEvents Events;
  protected readonly IInputHelper InputHelper;
  protected readonly IReflectionHelper Reflection;
  private readonly int ScreenId;
  private Rectangle LastViewport;
  private readonly Func<bool>? KeepAliveCheck;
  private readonly bool? AssumeUiMode;

  public virtual void Dispose()
  {
    this.Events.Display.RenderedActiveMenu -= new EventHandler<RenderedActiveMenuEventArgs>(this.OnRendered);
    this.Events.Display.RenderedWorld -= new EventHandler<RenderedWorldEventArgs>(this.OnRenderedWorld);
    this.Events.GameLoop.UpdateTicked -= new EventHandler<UpdateTickedEventArgs>(this.OnUpdateTicked);
    this.Events.Input.ButtonPressed -= new EventHandler<ButtonPressedEventArgs>(this.OnButtonPressed);
    this.Events.Input.ButtonsChanged -= new EventHandler<ButtonsChangedEventArgs>(this.OnButtonsChanged);
    this.Events.Input.CursorMoved -= new EventHandler<CursorMovedEventArgs>(this.OnCursorMoved);
    this.Events.Input.MouseWheelScrolled -= new EventHandler<MouseWheelScrolledEventArgs>(this.OnMouseWheelScrolled);
  }

  protected BaseOverlay(
    IModEvents events,
    IInputHelper inputHelper,
    IReflectionHelper reflection,
    Func<bool>? keepAlive = null,
    bool? assumeUiMode = null)
  {
    this.Events = events;
    this.InputHelper = inputHelper;
    this.Reflection = reflection;
    this.KeepAliveCheck = keepAlive;
    this.LastViewport = new Rectangle(((Rectangle) ref Game1.uiViewport).X, ((Rectangle) ref Game1.uiViewport).Y, ((Rectangle) ref Game1.uiViewport).Width, ((Rectangle) ref Game1.uiViewport).Height);
    this.ScreenId = Context.ScreenId;
    this.AssumeUiMode = assumeUiMode;
    events.GameLoop.UpdateTicked += new EventHandler<UpdateTickedEventArgs>(this.OnUpdateTicked);
    if (this.IsMethodOverridden("DrawUi"))
      events.Display.RenderedActiveMenu += new EventHandler<RenderedActiveMenuEventArgs>(this.OnRendered);
    if (this.IsMethodOverridden("DrawWorld"))
      events.Display.RenderedWorld += new EventHandler<RenderedWorldEventArgs>(this.OnRenderedWorld);
    if (this.IsMethodOverridden("ReceiveLeftClick"))
      events.Input.ButtonPressed += new EventHandler<ButtonPressedEventArgs>(this.OnButtonPressed);
    if (this.IsMethodOverridden("ReceiveButtonsChanged"))
      events.Input.ButtonsChanged += new EventHandler<ButtonsChangedEventArgs>(this.OnButtonsChanged);
    if (this.IsMethodOverridden("ReceiveCursorHover"))
      events.Input.CursorMoved += new EventHandler<CursorMovedEventArgs>(this.OnCursorMoved);
    if (!this.IsMethodOverridden("ReceiveScrollWheelAction"))
      return;
    events.Input.MouseWheelScrolled += new EventHandler<MouseWheelScrolledEventArgs>(this.OnMouseWheelScrolled);
  }

  protected virtual void Update()
  {
  }

  protected virtual void DrawUi(SpriteBatch batch)
  {
  }

  protected virtual void DrawWorld(SpriteBatch batch)
  {
  }

  protected virtual bool ReceiveLeftClick(int x, int y) => false;

  protected virtual void ReceiveButtonsChanged(object? sender, ButtonsChangedEventArgs e)
  {
  }

  protected virtual bool ReceiveScrollWheelAction(int amount) => false;

  protected virtual bool ReceiveCursorHover(int x, int y) => false;

  protected virtual void ReceiveGameWindowResized()
  {
  }

  protected void DrawCursor()
  {
    if (Game1.options.hardwareCursor)
      return;
    Vector2 vector2;
    // ISSUE: explicit constructor call
    ((Vector2) ref vector2).\u002Ector((float) Game1.getMouseX(), (float) Game1.getMouseY());
    if (Constants.TargetPlatform == null)
      vector2 = Vector2.op_Multiply(vector2, Game1.options.zoomLevel / this.Reflection.GetProperty<float>(typeof (Game1), "NativeZoomLevel", true).GetValue());
    Game1.spriteBatch.Draw(Game1.mouseCursors, vector2, new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, Game1.options.SnappyMenus ? 44 : 0, 16 /*0x10*/, 16 /*0x10*/)), Color.op_Multiply(Color.White, Game1.mouseCursorTransparency), 0.0f, Vector2.Zero, (float) (4.0 + (double) Game1.dialogueButtonScale / 150.0), (SpriteEffects) 0, 1f);
  }

  private void OnRendered(object? sender, RenderedActiveMenuEventArgs e)
  {
    if (Context.ScreenId != this.ScreenId)
      return;
    this.DrawUi(Game1.spriteBatch);
  }

  private void OnRenderedWorld(object? sender, RenderedWorldEventArgs e)
  {
    if (Context.ScreenId != this.ScreenId)
      return;
    this.DrawWorld(e.SpriteBatch);
  }

  private void OnUpdateTicked(object? sender, UpdateTickedEventArgs e)
  {
    if (Context.ScreenId != this.ScreenId)
    {
      if (Context.HasScreenId(this.ScreenId))
        return;
      this.Dispose();
    }
    else if (this.KeepAliveCheck != null && !this.KeepAliveCheck())
    {
      this.Dispose();
    }
    else
    {
      Rectangle uiViewport = Game1.uiViewport;
      if (((Rectangle) ref this.LastViewport).Width != ((Rectangle) ref uiViewport).Width || ((Rectangle) ref this.LastViewport).Height != ((Rectangle) ref uiViewport).Height)
      {
        // ISSUE: explicit constructor call
        ((Rectangle) ref uiViewport).\u002Ector(((Rectangle) ref uiViewport).X, ((Rectangle) ref uiViewport).Y, ((Rectangle) ref uiViewport).Width, ((Rectangle) ref uiViewport).Height);
        this.ReceiveGameWindowResized();
        this.LastViewport = uiViewport;
      }
      this.Update();
    }
  }

  private void OnButtonsChanged(object? sender, ButtonsChangedEventArgs e)
  {
    if (Context.ScreenId != this.ScreenId)
      return;
    this.ReceiveButtonsChanged(sender, e);
  }

  private void OnButtonPressed(object? sender, ButtonPressedEventArgs e)
  {
    if (Context.ScreenId != this.ScreenId || e.Button != 1000 && !SButtonExtensions.IsUseToolButton(e.Button))
      return;
    bool flag = ((int) this.AssumeUiMode ?? (Game1.uiMode ? 1 : 0)) != 0;
    bool leftClick;
    if (Constants.TargetPlatform == null)
    {
      float num = this.Reflection.GetProperty<float>(typeof (Game1), "NativeZoomLevel", true).GetValue();
      leftClick = this.ReceiveLeftClick((int) ((double) Game1.getMouseX() * (double) Game1.options.zoomLevel / (double) num), (int) ((double) Game1.getMouseY() * (double) Game1.options.zoomLevel / (double) num));
    }
    else
      leftClick = this.ReceiveLeftClick(Game1.getMouseX(flag), Game1.getMouseY(flag));
    if (!leftClick)
      return;
    this.InputHelper.Suppress(e.Button);
  }

  private void OnMouseWheelScrolled(object? sender, MouseWheelScrolledEventArgs e)
  {
    if (Context.ScreenId != this.ScreenId || !this.ReceiveScrollWheelAction(e.Delta))
      return;
    MouseState oldMouseState = Game1.oldMouseState;
    Game1.oldMouseState = new MouseState(((MouseState) ref oldMouseState).X, ((MouseState) ref oldMouseState).Y, e.NewValue, ((MouseState) ref oldMouseState).LeftButton, ((MouseState) ref oldMouseState).MiddleButton, ((MouseState) ref oldMouseState).RightButton, ((MouseState) ref oldMouseState).XButton1, ((MouseState) ref oldMouseState).XButton2);
  }

  private void OnCursorMoved(object? sender, CursorMovedEventArgs e)
  {
    if (Context.ScreenId != this.ScreenId)
      return;
    bool flag = ((int) this.AssumeUiMode ?? (Game1.uiMode ? 1 : 0)) != 0;
    if (!this.ReceiveCursorHover(Game1.getMouseX(flag), Game1.getMouseY(flag)))
      return;
    Game1.InvalidateOldMouseMovement();
  }

  private bool IsMethodOverridden(string name)
  {
    MethodInfo method = this.GetType().GetMethod(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
    if (method == (MethodInfo) null)
      throw new InvalidOperationException($"Can't find method {this.GetType().FullName}.{name}.");
    return method.DeclaringType != typeof (BaseOverlay);
  }
}
