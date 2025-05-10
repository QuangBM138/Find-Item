// Decompiled with JetBrains decompiler
// Type: Pathoschild.Stardew.LookupAnything.ModEntry
// Assembly: LookupAnything, Version=1.50.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4C943E7-A147-473E-9B25-194C64926EB2
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Mods\LookupAnything\LookupAnything.dll

using Microsoft.Xna.Framework;
using Newtonsoft.Json.Linq;
using Pathoschild.Stardew.Common;
using Pathoschild.Stardew.Common.Integrations.GenericModConfigMenu;
using Pathoschild.Stardew.Common.Integrations.IconicFramework;
using Pathoschild.Stardew.LookupAnything.Components;
using Pathoschild.Stardew.LookupAnything.Framework;
using Pathoschild.Stardew.LookupAnything.Framework.Lookups;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;

#nullable enable
namespace Pathoschild.Stardew.LookupAnything;

internal class ModEntry : Mod
{
  private ModConfig Config;
  private Metadata? Metadata;
  private readonly string DatabaseFileName = "assets/data.json";
  private GameHelper? GameHelper;
  private TargetFactory? TargetFactory;
  private PerScreen<Pathoschild.Stardew.LookupAnything.Components.DebugInterface>? DebugInterface;
  private readonly PerScreen<Stack<IClickableMenu>> PreviousMenus = new PerScreen<Stack<IClickableMenu>>((Func<Stack<IClickableMenu>>) (() => new Stack<IClickableMenu>()));

  private ModConfigKeys Keys => this.Config.Controls;

  [MemberNotNullWhen(true, new string[] {"Metadata", "GameHelper", "TargetFactory", "DebugInterface"})]
  private bool IsDataValid { [MemberNotNullWhen(true, new string[] {"Metadata", "GameHelper", "TargetFactory", "DebugInterface"})] get; [MemberNotNullWhen(true, new string[] {"Metadata", "GameHelper", "TargetFactory", "DebugInterface"})] set; }

  public virtual void Entry(IModHelper helper)
  {
    CommonHelper.RemoveObsoleteFiles((IMod) this, "LookupAnything.pdb");
    this.Config = this.LoadConfig();
    I18n.Init(helper.Translation);
    this.Metadata = this.LoadMetadata();
    Metadata metadata = this.Metadata;
    this.IsDataValid = (object) metadata != null && metadata.LooksValid();
    if (!this.IsDataValid)
      this.Monitor.Log($"The {this.DatabaseFileName} file seems to be missing or corrupt. Lookups will be disabled.", (LogLevel) 4);
    if (!helper.Translation.GetTranslations().Any<Translation>())
      this.Monitor.Log("The translation files in this mod's i18n folder seem to be missing. The mod will still work, but you'll see 'missing translation' messages. Try reinstalling the mod to fix this.", (LogLevel) 3);
    helper.Events.GameLoop.GameLaunched += new EventHandler<GameLaunchedEventArgs>(this.OnGameLaunched);
    helper.Events.GameLoop.DayStarted += new EventHandler<DayStartedEventArgs>(this.OnDayStarted);
    helper.Events.Display.RenderedHud += new EventHandler<RenderedHudEventArgs>(this.OnRenderedHud);
    helper.Events.Display.MenuChanged += new EventHandler<MenuChangedEventArgs>(this.OnMenuChanged);
    helper.Events.Input.ButtonsChanged += new EventHandler<ButtonsChangedEventArgs>(this.OnButtonsChanged);
  }

  private void OnGameLaunched(object? sender, GameLaunchedEventArgs e)
  {
    if (!this.IsDataValid)
      return;
    this.GameHelper = new GameHelper(this.Metadata, this.Monitor, this.Helper.ModRegistry, this.Helper.Reflection);
    this.TargetFactory = new TargetFactory(this.Helper.Reflection, this.GameHelper, (Func<ModConfig>) (() => this.Config), (Func<bool>) (() => this.Config.EnableTileLookups));
    this.DebugInterface = new PerScreen<Pathoschild.Stardew.LookupAnything.Components.DebugInterface>((Func<Pathoschild.Stardew.LookupAnything.Components.DebugInterface>) (() => new Pathoschild.Stardew.LookupAnything.Components.DebugInterface(this.GameHelper, this.TargetFactory, (Func<ModConfig>) (() => this.Config), this.Monitor)));
    ((IMod) this).AddGenericModConfigMenu<ModConfig>((IGenericModConfigMenuIntegrationFor<ModConfig>) new GenericModConfigMenuIntegrationForLookupAnything(), (Func<ModConfig>) (() => this.Config), (Action<ModConfig>) (config => this.Config = config));
    IconicFrameworkIntegration frameworkIntegration = new IconicFrameworkIntegration(this.Helper.ModRegistry, this.Monitor);
    if (!frameworkIntegration.IsLoaded)
      return;
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    frameworkIntegration.AddToolbarIcon("LooseSprites/Cursors", new Rectangle?(new Rectangle(330, 357, 7, 13)), ModEntry.\u003C\u003EO.\u003C0\u003E__Icon_ToggleSearch_Name ?? (ModEntry.\u003C\u003EO.\u003C0\u003E__Icon_ToggleSearch_Name = new Func<string>(I18n.Icon_ToggleSearch_Name)), ModEntry.\u003C\u003EO.\u003C1\u003E__Icon_ToggleSearch_Desc ?? (ModEntry.\u003C\u003EO.\u003C1\u003E__Icon_ToggleSearch_Desc = new Func<string>(I18n.Icon_ToggleSearch_Desc)), (Action) (() => this.ShowLookup(true)), new Action(this.TryToggleSearch));
  }

  private void OnDayStarted(object? sender, DayStartedEventArgs e)
  {
    if (!this.IsDataValid)
      return;
    this.GameHelper.ResetCache(this.Monitor);
  }

  private void OnButtonsChanged(object? sender, ButtonsChangedEventArgs e)
  {
    if (!this.IsDataValid)
      return;
    this.Monitor.InterceptErrors("handling your input", (Action) (() =>
    {
      ModConfigKeys keys = this.Keys;
      if (keys.ToggleSearch.JustPressed())
        this.TryToggleSearch();
      else if (keys.ToggleLookup.JustPressed())
        this.ToggleLookup();
      else if (keys.ScrollUp.JustPressed())
      {
        if (Game1.activeClickableMenu is IScrollableMenu activeClickableMenu5)
        {
          int? amount = new int?();
          activeClickableMenu5.ScrollUp(amount);
        }
      }
      else if (keys.ScrollDown.JustPressed())
      {
        if (Game1.activeClickableMenu is IScrollableMenu activeClickableMenu6)
        {
          int? amount = new int?();
          activeClickableMenu6.ScrollDown(amount);
        }
      }
      else if (keys.PageUp.JustPressed())
      {
        if (Game1.activeClickableMenu is IScrollableMenu activeClickableMenu7)
          activeClickableMenu7.ScrollUp(new int?(Game1.activeClickableMenu.height));
      }
      else if (keys.PageDown.JustPressed())
      {
        if (Game1.activeClickableMenu is IScrollableMenu activeClickableMenu8)
          activeClickableMenu8.ScrollDown(new int?(Game1.activeClickableMenu.height));
      }
      else if (keys.ToggleDebug.JustPressed() && Context.IsPlayerFree)
        this.DebugInterface.Value.Enabled = !this.DebugInterface.Value.Enabled;
      if (!this.Config.HideOnKeyUp || keys.ToggleLookup.GetState() != 3)
        return;
      this.HideLookup();
    }));
  }

  private void OnMenuChanged(object? sender, MenuChangedEventArgs e)
  {
    this.Monitor.InterceptErrors("restoring the previous menu", (Action) (() =>
    {
      bool flag1 = e.NewMenu == null;
      if (flag1)
      {
        bool flag2;
        switch (e.OldMenu)
        {
          case LookupMenu _:
          case SearchMenu _:
            flag2 = true;
            break;
          default:
            flag2 = false;
            break;
        }
        flag1 = flag2;
      }
      if (!flag1 || !this.PreviousMenus.Value.Any<IClickableMenu>())
        return;
      Game1.activeClickableMenu = this.PreviousMenus.Value.Pop();
    }));
  }

  private void OnRenderedHud(object? sender, RenderedHudEventArgs e)
  {
    if (!this.IsDataValid || !this.DebugInterface.Value.Enabled)
      return;
    this.DebugInterface.Value.Draw(Game1.spriteBatch);
  }

  private void ToggleLookup()
  {
    if (Game1.activeClickableMenu is LookupMenu)
      this.HideLookup();
    else
      this.ShowLookup();
  }

  private void ShowLookup(bool ignoreCursor = false)
  {
    if (!this.IsDataValid)
      return;
    StringBuilder logMessage = new StringBuilder("Received a lookup request...");
    this.Monitor.InterceptErrors("looking that up", (Action) (() =>
    {
      try
      {
        ISubject subject = this.GetSubject(logMessage, ignoreCursor);
        if (subject == null)
        {
          this.Monitor.Log($"{logMessage} no target found.", (LogLevel) 0);
        }
        else
        {
          this.Monitor.Log(logMessage.ToString(), (LogLevel) 0);
          this.ShowLookupFor(subject);
        }
      }
      catch
      {
        this.Monitor.Log($"{logMessage} an error occurred.", (LogLevel) 0);
        throw;
      }
    }));
  }

  internal void ShowLookupFor(ISubject subject)
  {
    this.Monitor.InterceptErrors("looking that up", (Action) (() =>
    {
      this.Monitor.Log($"Showing {subject.GetType().Name}::{subject.Type}::{subject.Name}.", (LogLevel) 0);
      this.PushMenu((IClickableMenu) new LookupMenu(subject, this.Monitor, this.Helper.Reflection, this.Config.ScrollAmount, this.Config.ShowDataMiningFields, this.Config.ForceFullScreen, new Action<ISubject>(this.ShowLookupFor)));
    }));
  }

  private void HideLookup()
  {
    this.Monitor.InterceptErrors("closing the menu", (Action) (() =>
    {
      if (!(Game1.activeClickableMenu is LookupMenu activeClickableMenu2))
        return;
      activeClickableMenu2.QueueExit();
    }));
  }

  private void TryToggleSearch()
  {
    if (Game1.activeClickableMenu is SearchMenu)
    {
      this.HideSearch();
    }
    else
    {
      if (!Context.IsWorldReady || Game1.activeClickableMenu is LookupMenu)
        return;
      this.ShowSearch();
    }
  }

  private void ShowSearch()
  {
    if (!this.IsDataValid)
      return;
    this.PushMenu((IClickableMenu) new SearchMenu(this.TargetFactory.GetSearchSubjects(), new Action<ISubject>(this.ShowLookupFor), this.Monitor, this.Config.ScrollAmount));
  }

  private void HideSearch()
  {
    if (!(Game1.activeClickableMenu is SearchMenu))
      return;
    Game1.playSound("bigDeSelect", new int?());
    Game1.activeClickableMenu = (IClickableMenu) null;
  }

  private ModConfig LoadConfig()
  {
    try
    {
      if (File.Exists(Path.Combine(this.Helper.DirectoryPath, "config.json")))
      {
        JObject jobject1 = this.Helper.ReadConfig<JObject>();
        JObject jobject2 = ((JToken) jobject1).Value<JObject>((object) "Controls");
        string str1 = ((JToken) jobject2)?.Value<string>((object) "ToggleLookup");
        string str2 = ((JToken) jobject2)?.Value<string>((object) "ToggleLookupInFrontOfPlayer");
        if (!string.IsNullOrWhiteSpace(str2))
        {
          jobject2.Remove("ToggleLookupInFrontOfPlayer");
          jobject2["ToggleLookup"] = JToken.op_Implicit(string.Join(", ", ((IEnumerable<string>) (str1 ?? "").Split(',')).Concat<string>((IEnumerable<string>) str2.Split(',')).Select<string, string>((Func<string, string>) (p => p.Trim())).Where<string>((Func<string, bool>) (p => p != "")).Distinct<string>()));
          this.Helper.WriteConfig<JObject>(jobject1);
        }
      }
    }
    catch (Exception ex)
    {
      this.Monitor.Log("Couldn't migrate legacy settings in config.json; they'll be removed instead.", (LogLevel) 3);
      this.Monitor.Log(ex.ToString(), (LogLevel) 0);
    }
    return this.Helper.ReadConfig<ModConfig>();
  }

  private ISubject? GetSubject(StringBuilder logMessage, bool ignoreCursor = false)
  {
    if (!this.IsDataValid)
      return (ISubject) null;
    Vector2 cursorPos = this.GameHelper.GetScreenCoordinatesFromCursor();
    if (!Game1.uiMode)
      cursorPos = Utility.ModifyCoordinatesForUIScale(cursorPos);
    bool hasCursor = !ignoreCursor && Constants.TargetPlatform != null && Game1.wasMouseVisibleThisFrame;
    if (Game1.activeClickableMenu != null)
    {
      StringBuilder stringBuilder1 = logMessage;
      StringBuilder stringBuilder2 = stringBuilder1;
      StringBuilder.AppendInterpolatedStringHandler interpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(30, 1, stringBuilder1);
      interpolatedStringHandler.AppendLiteral(" searching the open '");
      interpolatedStringHandler.AppendFormatted(Game1.activeClickableMenu.GetType().Name);
      interpolatedStringHandler.AppendLiteral("' menu...");
      ref StringBuilder.AppendInterpolatedStringHandler local = ref interpolatedStringHandler;
      stringBuilder2.Append(ref local);
      return this.TargetFactory.GetSubjectFrom(Game1.activeClickableMenu, cursorPos);
    }
    if (hasCursor)
    {
      foreach (IClickableMenu onScreenMenu in (IEnumerable<IClickableMenu>) Game1.onScreenMenus)
      {
        if (onScreenMenu.isWithinBounds((int) cursorPos.X, (int) cursorPos.Y))
        {
          StringBuilder stringBuilder3 = logMessage;
          StringBuilder stringBuilder4 = stringBuilder3;
          StringBuilder.AppendInterpolatedStringHandler interpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(35, 1, stringBuilder3);
          interpolatedStringHandler.AppendLiteral(" searching the on-screen '");
          interpolatedStringHandler.AppendFormatted(onScreenMenu.GetType().Name);
          interpolatedStringHandler.AppendLiteral("' menu...");
          ref StringBuilder.AppendInterpolatedStringHandler local = ref interpolatedStringHandler;
          stringBuilder4.Append(ref local);
          return this.TargetFactory.GetSubjectFrom(onScreenMenu, cursorPos);
        }
      }
    }
    logMessage.Append(" searching the world...");
    return this.TargetFactory.GetSubjectFrom(Game1.player, Game1.currentLocation, hasCursor);
  }

  private void PushMenu(IClickableMenu menu)
  {
    if (this.ShouldRestoreMenu(Game1.activeClickableMenu))
    {
      this.PreviousMenus.Value.Push(Game1.activeClickableMenu);
      this.Helper.Reflection.GetField<IClickableMenu>(typeof (Game1), "_activeClickableMenu", true).SetValue(menu);
    }
    else
      Game1.activeClickableMenu = menu;
  }

  private Metadata? LoadMetadata()
  {
    Metadata metadata = (Metadata) null;
    this.Monitor.InterceptErrors("loading metadata", (Action) (() => metadata = this.Helper.Data.ReadJsonFile<Metadata>(this.DatabaseFileName)));
    return metadata;
  }

  private bool ShouldRestoreMenu(IClickableMenu? menu)
  {
    return menu != null && (!this.Config.HideOnKeyUp || !(menu is LookupMenu));
  }
}
