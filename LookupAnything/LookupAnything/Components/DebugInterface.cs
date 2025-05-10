// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.Components.DebugInterface
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pathoschild.Stardew.Common;
using Pathoschild.Stardew.LookupAnything.Framework;
using Pathoschild.Stardew.LookupAnything.Framework.Lookups;
using StardewModdingAPI;
using StardewValley;
using System;
using System.Linq;
using xTile.Dimensions;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything.Components;

internal class DebugInterface
{
  private readonly GameHelper GameHelper;
  private readonly TargetFactory TargetFactory;
  private readonly IMonitor Monitor;
  private readonly Func<ModConfig> Config;

  public bool Enabled { get; set; }

  public DebugInterface(
    GameHelper gameHelper,
    TargetFactory targetFactory,
    Func<ModConfig> config,
    IMonitor monitor)
  {
    this.GameHelper = gameHelper;
    this.TargetFactory = targetFactory;
    this.Config = config;
    this.Monitor = monitor;
  }

  public void Draw(SpriteBatch spriteBatch)
  {
    if (!this.Enabled)
      return;
    this.Monitor.InterceptErrors("drawing debug info", (Action) (() =>
    {
      ModConfig modConfig = this.Config();
      GameLocation currentLocation = Game1.currentLocation;
      Vector2 currentCursorTile = Game1.currentCursorTile;
      Vector2 coordinatesFromCursor = this.GameHelper.GetScreenCoordinatesFromCursor();
      this.GameHelper.DrawHoverBox(spriteBatch, $"Debug info enabled; press {string.Join(" or ", (object) modConfig.Controls.ToggleDebug)} to disable. Cursor tile ({currentCursorTile.X}, {currentCursorTile.Y}), position ({coordinatesFromCursor.X}, {coordinatesFromCursor.Y}).", Vector2.Zero, (float) ((Rectangle) ref Game1.uiViewport).Width);
      DrawHelper.DrawLine(spriteBatch, coordinatesFromCursor.X - 1f, coordinatesFromCursor.Y - 1f, new Vector2(4f, 4f), new Color?(Color.DarkRed));
      Rectangle coordinatesFromTile1 = this.GameHelper.GetScreenCoordinatesFromTile(Game1.currentCursorTile);
      foreach (ITarget target in this.TargetFactory.GetNearbyTargets(currentLocation, currentCursorTile).Where<ITarget>((Func<ITarget, bool>) (p => p.Type != SubjectType.Tile)))
      {
        Rectangle worldArea1 = target.GetWorldArea();
        bool flag = ((Rectangle) ref worldArea1).Intersects(coordinatesFromTile1);
        ISubject subject = target.GetSubject();
        Rectangle coordinatesFromTile2 = this.GameHelper.GetScreenCoordinatesFromTile(target.Tile);
        Color color1 = Color.op_Multiply(subject != null ? Color.Green : Color.Red, 0.5f);
        DrawHelper.DrawLine(spriteBatch, (float) coordinatesFromTile2.X, (float) coordinatesFromTile2.Y, new Vector2((float) coordinatesFromTile2.Width, (float) coordinatesFromTile2.Height), new Color?(color1));
        if (subject != null)
        {
          int num = 3;
          Color color2 = Color.Green;
          if (!flag)
          {
            num = 1;
            color2 = Color.op_Multiply(color2, 0.5f);
          }
          Rectangle worldArea2 = target.GetWorldArea();
          DrawHelper.DrawLine(spriteBatch, (float) worldArea2.X, (float) worldArea2.Y, new Vector2((float) worldArea2.Width, (float) num), new Color?(color2));
          DrawHelper.DrawLine(spriteBatch, (float) worldArea2.X, (float) worldArea2.Y, new Vector2((float) num, (float) worldArea2.Height), new Color?(color2));
          DrawHelper.DrawLine(spriteBatch, (float) (worldArea2.X + worldArea2.Width), (float) worldArea2.Y, new Vector2((float) num, (float) worldArea2.Height), new Color?(color2));
          DrawHelper.DrawLine(spriteBatch, (float) worldArea2.X, (float) (worldArea2.Y + worldArea2.Height), new Vector2((float) worldArea2.Width, (float) num), new Color?(color2));
        }
      }
      ISubject subjectFrom = this.TargetFactory.GetSubjectFrom(Game1.player, currentLocation, Game1.wasMouseVisibleThisFrame);
      if (subjectFrom == null)
        return;
      this.GameHelper.DrawHoverBox(spriteBatch, subjectFrom.Name, Vector2.op_Addition(new Vector2((float) Game1.getMouseX(), (float) Game1.getMouseY()), new Vector2(32f)), (float) ((Rectangle) ref Game1.uiViewport).Width / 4f);
    }), new Action<Exception>(this.OnDrawError));
  }

  private void OnDrawError(Exception ex)
  {
    this.Monitor.InterceptErrors("handling an error in the debug code", (Action) (() => this.Enabled = false));
  }
}
