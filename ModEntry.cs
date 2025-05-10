// Decompiled with JetBrains decompiler
// Type: Item_Locator.ModEntry
// Assembly: Item Locator, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BFE121E-49FA-41A1-80AC-34270D5A3C38
// Assembly location: D:\game indi\Item Locator\Item Locator.dll

using GenericModConfigMenu;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable enable
namespace Item_Locator;

internal sealed class ModEntry : Mod
{
  private Texture2D? tileHighlight;
  public static bool shouldDraw = false;
  public static Dictionary<List<Vector2>, Color> pathColors = new Dictionary<List<Vector2>, Color>();
  public static List<List<Vector2>> paths = new List<List<Vector2>>();
  public SButton openMenuKeybind;
  public static List<string> locateHistory = new List<string>();
  public static bool updateLocateHistory = false;

  [field: DebuggerBrowsable]
  private ModConfig Config { get; set; } = new ModConfig();

  public virtual void Entry(IModHelper helper)
  {
    this.Config = helper.ReadConfig<ModConfig>();
    ModEntry.locateHistory = this.Config.locateHistory;
    SButton openMenuKey = this.Config.openMenuKey;
    helper.Events.GameLoop.GameLaunched += new EventHandler<GameLaunchedEventArgs>(this.OnGameLaunched);
    this.tileHighlight = helper.ModContent.Load<Texture2D>("/assets/tileColor.png");
    helper.Events.Input.ButtonPressed += new EventHandler<ButtonPressedEventArgs>(this.OpenItemMenu);
    helper.Events.Display.WindowResized += new EventHandler<WindowResizedEventArgs>(this.resizeCustomMenu);
    helper.Events.Display.RenderedWorld += new EventHandler<RenderedWorldEventArgs>(this.RenderedWorld);
    helper.Events.Player.Warped += new EventHandler<WarpedEventArgs>(this.ChangedLocation);
  }

  private void RenderedWorld(object? sender, RenderedWorldEventArgs e)
  {
    if (ModEntry.updateLocateHistory)
    {
      this.Helper.WriteConfig<ModConfig>(this.Config);
      ModEntry.updateLocateHistory = false;
    }
    if (ModEntry.paths.Count <= 0 || !ModEntry.shouldDraw)
      return;
    this.DrawPath(e, ModEntry.paths);
  }

  private void DrawPath(RenderedWorldEventArgs e, List<List<Vector2>> paths)
  {
    foreach (List<Vector2> path in paths)
    {
      foreach (Vector2 vector2 in path)
      {
        Vector2 local = Game1.GlobalToLocal(Game1.viewport, Vector2.op_Multiply(vector2, 64f));
        e.SpriteBatch.Draw(this.tileHighlight, local, Color.op_Multiply(ModEntry.pathColors[path], Math.Abs(this.Config.pathTransparency - 1f)));
      }
    }
  }

  private void ChangedLocation(object? sender, WarpedEventArgs e)
  {
    if (!e.IsLocalPlayer)
      return;
    ModEntry.paths.Clear();
    ModEntry.shouldDraw = false;
  }

  private void OpenItemMenu(object? sender, ButtonPressedEventArgs e)
  {
    if (!Context.IsWorldReady || e.Button != this.Config.openMenuKey || Game1.activeClickableMenu != null || !Context.IsPlayerFree)
      return;
    Game1.activeClickableMenu = (IClickableMenu) new CustomItemMenu();
  }

  private void resizeCustomMenu(object? sender, WindowResizedEventArgs e)
  {
    if (!(Game1.activeClickableMenu is CustomItemMenu))
      return;
    Game1.activeClickableMenu = (IClickableMenu) new CustomItemMenu();
  }

  private void OnGameLaunched(object? sender, GameLaunchedEventArgs e)
  {
    IGenericModConfigMenuApi api = this.Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
    if (api == null)
      return;
    api.Register(this.ModManifest, (Action) (() => this.Config = new ModConfig()), (Action) (() => this.Helper.WriteConfig<ModConfig>(this.Config)));
    api.AddKeybind(this.ModManifest, (Func<SButton>) (() => this.Config.openMenuKey), (Action<SButton>) (value => this.Config.openMenuKey = value), (Func<string>) (() => "Change Keybind: "));
    api.AddNumberOption(this.ModManifest, (Func<float>) (() => this.Config.pathTransparency), (Action<float>) (value => this.Config.pathTransparency = value), (Func<string>) (() => "Path Transparency: "), min: new float?(0.0f), max: new float?(1f), interval: new float?(0.05f));
  }
}
