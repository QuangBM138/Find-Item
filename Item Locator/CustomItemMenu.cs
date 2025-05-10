// Decompiled with JetBrains decompiler
// Type: Item_Locator.CustomItemMenu
// Assembly: Item Locator, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4BFE121E-49FA-41A1-80AC-34270D5A3C38
// Assembly location: D:\game indi\Item Locator\Item Locator.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using xTile.Dimensions;

#nullable enable
namespace Item_Locator;

public class CustomItemMenu : IClickableMenu
{
  public static string SearchedItem = "";
  public static string errorMessageText = "";
  public static ClickableTextureComponent? locateButton;
  public static ClickableTextureComponent? clearButton;
  public static ClickableTextureComponent? clearInputButton;
  public static List<ClickableTextureComponent> listOfHistoryButtons = new List<ClickableTextureComponent>();
  public static List<Rectangle> listOfHistoryButtonsRects = new List<Rectangle>();
  public static List<ClickableComponent> listOfHistoryButtonsText = new List<ClickableComponent>();
  private static int UIWidth = 632;
  private static int UIHeight = 500;
  private static int UIHistoryWidth = 300;
  private static int UIHistoryHeight = 500;
  private static int xPos = (int) ((double) ((Rectangle) ref Game1.viewport).Width * (double) Game1.options.zoomLevel / (double) Game1.options.uiScale / 2.0 - (double) (CustomItemMenu.UIWidth / 2));
  private static int yPos = (int) ((double) ((Rectangle) ref Game1.viewport).Height * (double) Game1.options.zoomLevel / (double) Game1.options.uiScale / 2.0 - (double) CustomItemMenu.UIHeight);
  private static int xPosUIHistory = CustomItemMenu.xPos - 275;
  private static int yPosUIHistory = CustomItemMenu.yPos;
  private ClickableComponent TitleLabel;
  private ClickableComponent HistoryLabel;
  private ClickableComponent? errorMessage;
  private TextBox getItem;
  private Rectangle getItemRect;
  private Rectangle locateButtonRect;
  private Rectangle clearButtonRect;
  private Rectangle clearInputButtonRect;

  public CustomItemMenu()
  {
    CustomItemMenu.xPos = (int) ((double) ((Rectangle) ref Game1.viewport).Width * (double) Game1.options.zoomLevel / (double) Game1.options.uiScale / 2.0 - (double) (CustomItemMenu.UIWidth / 2));
    CustomItemMenu.yPos = (int) ((double) ((Rectangle) ref Game1.viewport).Height * (double) Game1.options.zoomLevel / (double) Game1.options.uiScale / 2.0 - (double) CustomItemMenu.UIHeight);
    CustomItemMenu.xPos = Math.Max(0, Math.Min(CustomItemMenu.xPos, ((Rectangle) ref Game1.viewport).Width - CustomItemMenu.UIWidth));
    CustomItemMenu.yPos = Math.Max(0, Math.Min(CustomItemMenu.yPos, ((Rectangle) ref Game1.viewport).Height - CustomItemMenu.UIHeight));
    CustomItemMenu.xPosUIHistory = CustomItemMenu.xPos - 275;
    CustomItemMenu.yPosUIHistory = CustomItemMenu.yPos;
    Vector2 vector2 = Game1.smallFont.MeasureString("   ");
    this.TitleLabel = new ClickableComponent(new Rectangle(CustomItemMenu.xPos + CustomItemMenu.UIWidth / 2 - (CustomItemMenu.UIWidth - 400) / 2 - (int) vector2.X, CustomItemMenu.yPos + 125, CustomItemMenu.UIWidth - 400, 64 /*0x40*/), "   Item Locator\nEnter Item Name:");
    this.HistoryLabel = new ClickableComponent(new Rectangle(CustomItemMenu.xPosUIHistory + (int) Game1.smallFont.MeasureString("History").X, CustomItemMenu.yPosUIHistory + 125, CustomItemMenu.UIHistoryWidth - 400, 64 /*0x40*/), "History:");
    this.getItem = new TextBox(Game1.content.Load<Texture2D>("LooseSprites\\textBox"), Game1.content.Load<Texture2D>("LooseSprites\\Cursors"), Game1.smallFont, Game1.textColor)
    {
      X = CustomItemMenu.xPos + CustomItemMenu.UIWidth / 2 - this.TitleLabel.bounds.Width / 2 - 35,
      Y = this.TitleLabel.bounds.Y + this.TitleLabel.bounds.Height + 30,
      Width = this.TitleLabel.bounds.Width
    };
    this.getItem.Text = CustomItemMenu.SearchedItem;
    CustomItemMenu.locateButton = new ClickableTextureComponent(new Rectangle(CustomItemMenu.xPos + CustomItemMenu.UIWidth / 2 + 84, this.getItem.Y + 75 + 52, 14, 15), Game1.content.Load<Texture2D>("LooseSprites\\Cursors"), new Rectangle(208 /*0xD0*/, 321, 14, 15), 6f, false);
    this.locateButtonRect = new Rectangle(((ClickableComponent) CustomItemMenu.locateButton).bounds.X, ((ClickableComponent) CustomItemMenu.locateButton).bounds.Y, ((ClickableComponent) CustomItemMenu.locateButton).bounds.Width * (int) ((ClickableComponent) CustomItemMenu.locateButton).scale, ((ClickableComponent) CustomItemMenu.locateButton).bounds.Height * (int) ((ClickableComponent) CustomItemMenu.locateButton).scale);
    CustomItemMenu.clearButton = new ClickableTextureComponent(new Rectangle(CustomItemMenu.xPos + CustomItemMenu.UIWidth / 2 - 168, this.getItem.Y + 75 + 52, 14, 15), Game1.content.Load<Texture2D>("LooseSprites\\Cursors"), new Rectangle(269, 471, 14, 15), 6f, false);
    this.clearButtonRect = new Rectangle(((ClickableComponent) CustomItemMenu.clearButton).bounds.X, ((ClickableComponent) CustomItemMenu.clearButton).bounds.Y, ((ClickableComponent) CustomItemMenu.clearButton).bounds.Width * (int) ((ClickableComponent) CustomItemMenu.clearButton).scale, ((ClickableComponent) CustomItemMenu.clearButton).bounds.Height * (int) ((ClickableComponent) CustomItemMenu.clearButton).scale);
    CustomItemMenu.clearInputButton = new ClickableTextureComponent(new Rectangle(this.getItem.X + 10 + this.getItem.Width, this.getItem.Y, 64 /*0x40*/, 64 /*0x40*/), Game1.content.Load<Texture2D>("LooseSprites\\Cursors"), new Rectangle(192 /*0xC0*/, 256 /*0x0100*/, 64 /*0x40*/, 64 /*0x40*/), 0.7f, false);
    this.clearInputButtonRect = new Rectangle(((ClickableComponent) CustomItemMenu.clearInputButton).bounds.X, ((ClickableComponent) CustomItemMenu.clearInputButton).bounds.Y, (int) ((double) ((ClickableComponent) CustomItemMenu.clearInputButton).bounds.Width * (double) ((ClickableComponent) CustomItemMenu.clearInputButton).scale), (int) ((double) ((ClickableComponent) CustomItemMenu.clearInputButton).bounds.Height * (double) ((ClickableComponent) CustomItemMenu.clearInputButton).scale));
    // ISSUE: method pointer
    this.getItem.OnEnterPressed += new TextBoxEvent((object) this, __methodptr(EnterPressed));
    this.updateHistoryList();
  }

  private void EnterPressed(TextBox sender) => CustomItemMenu.SearchedItem = sender.Text.ToLower();

  private void scaleTransition(ClickableTextureComponent icon, float scaleResult, float delta)
  {
    if ((double) delta > 0.0)
    {
      if ((double) ((ClickableComponent) icon).scale < (double) scaleResult)
      {
        ClickableTextureComponent textureComponent = icon;
        ((ClickableComponent) textureComponent).scale = ((ClickableComponent) textureComponent).scale + delta;
      }
      else
        ((ClickableComponent) icon).scale = scaleResult;
    }
    else if ((double) ((ClickableComponent) icon).scale > (double) scaleResult)
    {
      ClickableTextureComponent textureComponent = icon;
      ((ClickableComponent) textureComponent).scale = ((ClickableComponent) textureComponent).scale + delta;
    }
    else
      ((ClickableComponent) icon).scale = scaleResult;
  }

  public virtual void receiveKeyPress(Keys key)
  {
    if (this.getItem != null && this.getItem.Selected)
    {
      if (key == 27)
      {
        this.getItem.Selected = false;
        this.getItem.Text = "";
        CustomItemMenu.SearchedItem = "";
      }
      else
      {
        if (key == 69)
          return;
        base.receiveKeyPress(key);
      }
    }
    else
      base.receiveKeyPress(key);
  }

  public virtual void receiveLeftClick(int x, int y, bool playSound = true)
  {
    this.getItemRect = new Rectangle(this.getItem.X, this.getItem.Y, this.getItem.Width, this.getItem.Height);
    if (((Rectangle) ref this.getItemRect).Contains(x, y))
    {
      this.getItem.Selected = true;
    }
    else
    {
      CustomItemMenu.SearchedItem = this.getItem.Text;
      this.getItem.Selected = false;
    }
    if (((Rectangle) ref this.locateButtonRect).Contains(x, y))
    {
      this.scaleTransition(CustomItemMenu.locateButton, 5.7f, -0.08f);
      this.scaleTransition(CustomItemMenu.locateButton, 6f, 0.08f);
      this.ClickLocate(true);
    }
    if (((Rectangle) ref this.clearButtonRect).Contains(x, y))
    {
      Game1.playSound("select", new int?());
      ModEntry.paths.Clear();
      ModEntry.shouldDraw = false;
      Game1.activeClickableMenu = (IClickableMenu) null;
    }
    if (((Rectangle) ref this.clearInputButtonRect).Contains(x, y))
    {
      Game1.playSound("select", new int?());
      this.scaleTransition(CustomItemMenu.clearInputButton, 0.67f, -0.02f);
      this.scaleTransition(CustomItemMenu.clearInputButton, 0.7f, 0.02f);
      this.getItem.Text = "";
      CustomItemMenu.SearchedItem = "";
    }
    for (int index = 0; index < CustomItemMenu.listOfHistoryButtons.Count; ++index)
    {
      Rectangle historyButtonsRect = CustomItemMenu.listOfHistoryButtonsRects[index];
      if (((Rectangle) ref historyButtonsRect).Contains(x, y))
      {
        CustomItemMenu.SearchedItem = CustomItemMenu.listOfHistoryButtonsText[index].name;
        this.ClickLocate(false);
      }
    }
  }

  public void ClickLocate(bool isHistory)
  {
    if (CustomItemMenu.SearchedItem == null || !(Game1.activeClickableMenu is CustomItemMenu))
      return;
    Game1.playSound("select", new int?());
    Path_Finding.GetPaths();
    List<Vector2> containerLocs = FindContainers.get_container_locs(((Character) Game1.player).currentLocation, CustomItemMenu.SearchedItem);
    if (Path_Finding.invalidPlayerTile)
      CustomItemMenu.errorMessageText = "Please stand in a valid tile";
    else if (ModEntry.paths.Count == 0 && containerLocs.Count == 0)
      CustomItemMenu.errorMessageText = "No paths or containers found :(";
    else if (ModEntry.paths.Count == 0 && containerLocs.Count > 0)
    {
      CustomItemMenu.errorMessageText = $"No paths found, but {containerLocs.Count} containers found";
    }
    else
    {
      Game1.activeClickableMenu = (IClickableMenu) null;
      CustomItemMenu.errorMessageText = "";
    }
    this.errorMessage = new ClickableComponent(new Rectangle(this.getItem.X, this.getItem.Y + 75, 30, 30), CustomItemMenu.errorMessageText);
    if (isHistory)
      this.changeLocateHistory(ModEntry.locateHistory, CustomItemMenu.SearchedItem);
    this.updateHistoryList();
  }

  public virtual void performHoverAction(int x, int y)
  {
    base.performHoverAction(x, y);
    if (CustomItemMenu.locateButton == null || CustomItemMenu.clearButton == null || CustomItemMenu.clearInputButton == null)
      return;
    if (((Rectangle) ref this.locateButtonRect).Contains(x, y))
    {
      CustomItemMenu.locateButton.hoverText = "Locate Item";
      this.scaleTransition(CustomItemMenu.locateButton, 6.3f, 0.08f);
    }
    else
    {
      CustomItemMenu.locateButton.hoverText = "";
      this.scaleTransition(CustomItemMenu.locateButton, 6f, -0.08f);
    }
    if (((Rectangle) ref this.clearButtonRect).Contains(x, y))
    {
      CustomItemMenu.clearButton.hoverText = "Clear All Paths";
      this.scaleTransition(CustomItemMenu.clearButton, 6.3f, 0.08f);
    }
    else
    {
      CustomItemMenu.clearButton.hoverText = "";
      this.scaleTransition(CustomItemMenu.clearButton, 6f, -0.08f);
    }
    if (((Rectangle) ref this.clearInputButtonRect).Contains(x, y))
    {
      CustomItemMenu.clearInputButton.hoverText = "Clear Input";
      this.scaleTransition(CustomItemMenu.clearInputButton, 0.73f, 0.08f);
    }
    else
    {
      CustomItemMenu.clearInputButton.hoverText = "";
      this.scaleTransition(CustomItemMenu.clearInputButton, 0.7f, -0.08f);
    }
    for (int index = 0; index < CustomItemMenu.listOfHistoryButtons.Count; ++index)
    {
      Rectangle historyButtonsRect = CustomItemMenu.listOfHistoryButtonsRects[index];
      if (((Rectangle) ref historyButtonsRect).Contains(x, y))
        this.scaleTransition(CustomItemMenu.listOfHistoryButtons[index], 2.3f, 0.04f);
      else
        this.scaleTransition(CustomItemMenu.listOfHistoryButtons[index], 2f, -0.04f);
    }
  }

  public virtual void draw(SpriteBatch b)
  {
    SpriteBatch spriteBatch = b;
    Texture2D fadeToBlackRect = Game1.fadeToBlackRect;
    Viewport viewport = Game1.graphics.GraphicsDevice.Viewport;
    Rectangle bounds = ((Viewport) ref viewport).Bounds;
    Color color = Color.op_Multiply(Color.Black, 0.75f);
    spriteBatch.Draw(fadeToBlackRect, bounds, color);
    Game1.drawDialogueBox(CustomItemMenu.xPos, CustomItemMenu.yPos, CustomItemMenu.UIWidth, CustomItemMenu.UIHeight, false, true, (string) null, false, true, -1, -1, -1);
    Game1.drawDialogueBox(CustomItemMenu.xPos - 275, CustomItemMenu.yPos, 300, CustomItemMenu.UIHeight, false, true, (string) null, false, true, -1, -1, -1);
    Utility.drawTextWithShadow(b, this.TitleLabel.name, Game1.dialogueFont, new Vector2((float) this.TitleLabel.bounds.X, (float) this.TitleLabel.bounds.Y), Color.Black, 1f, -1f, -1, -1, 1f, 3);
    Utility.drawTextWithShadow(b, this.HistoryLabel.name, Game1.dialogueFont, new Vector2((float) this.HistoryLabel.bounds.X, (float) this.HistoryLabel.bounds.Y), Color.Black, 1f, -1f, -1, -1, 1f, 3);
    this.getItem.Draw(b, true);
    CustomItemMenu.locateButton?.draw(b);
    CustomItemMenu.clearButton?.draw(b);
    CustomItemMenu.clearInputButton?.draw(b);
    for (int index = 0; index < CustomItemMenu.listOfHistoryButtons.Count; ++index)
    {
      CustomItemMenu.listOfHistoryButtons[index].draw(b);
      Utility.drawTextWithShadow(b, CustomItemMenu.listOfHistoryButtonsText[index].name, Game1.smallFont, new Vector2((float) CustomItemMenu.listOfHistoryButtonsText[index].bounds.X, (float) CustomItemMenu.listOfHistoryButtonsText[index].bounds.Y), Color.Black, 1f, -1f, -1, -1, 1f, 3);
    }
    if (this.errorMessage != null)
    {
      Vector2 vector2 = Game1.smallFont.MeasureString(CustomItemMenu.errorMessageText);
      Utility.drawTextWithShadow(b, this.errorMessage.name, Game1.smallFont, new Vector2((float) (CustomItemMenu.xPos + CustomItemMenu.UIWidth / 2) - vector2.X / 2f, (float) this.errorMessage.bounds.Y), Color.Red, 1f, -1f, -1, -1, 1f, 3);
    }
    if (!string.IsNullOrEmpty(CustomItemMenu.locateButton?.hoverText))
      IClickableMenu.drawHoverText(b, CustomItemMenu.locateButton.hoverText, Game1.smallFont, 0, 0, -1, (string) null, -1, (string[]) null, (Item) null, 0, (string) null, -1, -1, -1, 1f, (CraftingRecipe) null, (IList<Item>) null, (Texture2D) null, new Rectangle?(), new Color?(), new Color?(), 1f, -1, -1);
    if (!string.IsNullOrEmpty(CustomItemMenu.clearButton?.hoverText))
      IClickableMenu.drawHoverText(b, CustomItemMenu.clearButton.hoverText, Game1.smallFont, 0, 0, -1, (string) null, -1, (string[]) null, (Item) null, 0, (string) null, -1, -1, -1, 1f, (CraftingRecipe) null, (IList<Item>) null, (Texture2D) null, new Rectangle?(), new Color?(), new Color?(), 1f, -1, -1);
    if (!string.IsNullOrEmpty(CustomItemMenu.clearInputButton?.hoverText))
      IClickableMenu.drawHoverText(b, CustomItemMenu.clearInputButton.hoverText, Game1.smallFont, 0, 0, -1, (string) null, -1, (string[]) null, (Item) null, 0, (string) null, -1, -1, -1, 1f, (CraftingRecipe) null, (IList<Item>) null, (Texture2D) null, new Rectangle?(), new Color?(), new Color?(), 1f, -1, -1);
    this.drawMouse(b, false, -1);
  }

  public virtual void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
  {
    CustomItemMenu.xPos = (int) ((double) ((Rectangle) ref Game1.viewport).Width * (double) Game1.options.zoomLevel / (double) Game1.options.uiScale / 2.0 - (double) (CustomItemMenu.UIWidth / 2));
    CustomItemMenu.yPos = (int) ((double) ((Rectangle) ref Game1.viewport).Height * (double) Game1.options.zoomLevel / (double) Game1.options.uiScale / 2.0 - (double) CustomItemMenu.UIHeight);
    CustomItemMenu.xPos = Math.Max(0, Math.Min(CustomItemMenu.xPos, ((Rectangle) ref Game1.viewport).Width - CustomItemMenu.UIWidth));
    CustomItemMenu.yPos = Math.Max(0, Math.Min(CustomItemMenu.yPos, ((Rectangle) ref Game1.viewport).Height - CustomItemMenu.UIHeight));
    CustomItemMenu.xPosUIHistory = CustomItemMenu.xPos - 275;
    CustomItemMenu.yPosUIHistory = CustomItemMenu.yPos;
  }

  private void changeLocateHistory(List<string> locHist, string item)
  {
    locHist.Insert(0, item);
    while (locHist.Count > 5)
      locHist.RemoveAt(locHist.Count - 1);
    ModEntry.updateLocateHistory = true;
  }

  private void updateHistoryList()
  {
    CustomItemMenu.listOfHistoryButtons.Clear();
    CustomItemMenu.listOfHistoryButtonsRects.Clear();
    CustomItemMenu.listOfHistoryButtonsText.Clear();
    for (int index = 0; index < 5; ++index)
    {
      ClickableTextureComponent textureComponent = new ClickableTextureComponent(new Rectangle(CustomItemMenu.xPosUIHistory + 32 /*0x20*/ + 15, this.HistoryLabel.bounds.Y + this.HistoryLabel.bounds.Height + index * 50, 16 /*0x10*/, 16 /*0x10*/), Game1.content.Load<Texture2D>("LooseSprites\\Cursors"), new Rectangle(274, 284, 16 /*0x10*/, 16 /*0x10*/), 2f, false);
      Rectangle rectangle;
      // ISSUE: explicit constructor call
      ((Rectangle) ref rectangle).\u002Ector(((ClickableComponent) textureComponent).bounds.X, ((ClickableComponent) textureComponent).bounds.Y, ((ClickableComponent) textureComponent).bounds.Width * (int) ((ClickableComponent) textureComponent).scale, ((ClickableComponent) textureComponent).bounds.Height * (int) ((ClickableComponent) textureComponent).scale);
      ClickableComponent clickableComponent = new ClickableComponent(new Rectangle(this.HistoryLabel.bounds.X, this.HistoryLabel.bounds.Y + this.HistoryLabel.bounds.Height + index * 50, this.HistoryLabel.bounds.Width, this.HistoryLabel.bounds.Height), ModEntry.locateHistory[index]);
      CustomItemMenu.listOfHistoryButtonsRects.Add(rectangle);
      CustomItemMenu.listOfHistoryButtons.Add(textureComponent);
      CustomItemMenu.listOfHistoryButtonsText.Add(clickableComponent);
    }
  }
}
