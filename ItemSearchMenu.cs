using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using Find_Item.Utilities;

namespace Find_Item
{
    /// <summary>A search UI for finding items in player inventories and storage.</summary>
    public class ItemSearchMenu : IClickableMenu
    {
        private readonly List<ItemSubject> AllSubjects;
        private List<ItemSubject> FilteredSubjects;
        private readonly TextBox SearchBox;
        private readonly Action<Item> OnSelect;

        private int targetScrollOffset = 0;
        private float smoothScrollOffset = 0;
        private const int ItemHeight = 74;

        // Auto-focus management
        private bool needsInitialFocus = true;

        // Keyboard selection
        private int selectedItemIndex = 0;

        // Drag scrolling
        private bool isDraggingScroll = false;
        private bool isDraggingThumb = false;
        private int lastDragY = 0;

        // Clear button bounds
        private Rectangle clearButtonRect;

        // Scrollbar bounds
        private Rectangle scrollbarTrackRect;
        private Rectangle scrollbarThumbRect;

        public ItemSearchMenu(List<ItemSubject> subjects, Action<Item> onSelect, string initialQuery = "")
            : base(
                  GetViewportAdjustedX(),
                  GetViewportAdjustedY(),
                  GetViewportAdjustedWidth(),
                  GetViewportAdjustedHeight(),
                  true) // showUpperCloseButton
        {
            this.AllSubjects = subjects ?? new List<ItemSubject>();
            this.FilteredSubjects = this.AllSubjects;
            this.OnSelect = onSelect;

            // Initialize TextBox for search input
            Texture2D textBoxTexture = Game1.content.Load<Texture2D>("LooseSprites\\textBox");
            this.SearchBox = new TextBox(textBoxTexture, null, Game1.smallFont, Game1.textColor)
            {
                X = this.xPositionOnScreen + 24,
                Y = this.yPositionOnScreen + 24,
                Width = this.width - 48,
                Height = 44,
                Text = initialQuery
            };

            // Force focus immediately on construction
            this.SearchBox.SelectMe();
            this.SearchBox.Selected = true;
            Game1.keyboardDispatcher.Subscriber = this.SearchBox;

            this.UpdateFilteredList();
        }

        private static int GetViewportAdjustedX()
        {
            int w = GetViewportAdjustedWidth();
            return (Game1.uiViewport.Width - w) / 2;
        }

        private static int GetViewportAdjustedY()
        {
            int h = GetViewportAdjustedHeight();
            return (Game1.uiViewport.Height - h) / 2;
        }

        private static int GetViewportAdjustedWidth()
        {
            return Math.Clamp((int)(Game1.uiViewport.Width * 0.55f), 550, 950);
        }

        private static int GetViewportAdjustedHeight()
        {
            return Math.Clamp((int)(Game1.uiViewport.Height * 0.70f), 450, 850);
        }

        public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
        {
            string currentQuery = this.SearchBox.Text;
            Game1.activeClickableMenu = new ItemSearchMenu(this.AllSubjects, this.OnSelect, currentQuery);
        }

        private int GetListStartY()
        {
            return this.SearchBox.Y + this.SearchBox.Height + 34;
        }

        private int GetVisibleHeight()
        {
            return this.yPositionOnScreen + this.height - this.GetListStartY() - 16;
        }

        private void UpdateFilteredList()
        {
            string query = this.SearchBox.Text?.Trim() ?? "";
            if (string.IsNullOrWhiteSpace(query))
            {
                this.FilteredSubjects = this.AllSubjects;
            }
            else
            {
                this.FilteredSubjects = this.AllSubjects
                    .Where(s => s.Matches(query))
                    .ToList();
            }

            if (this.selectedItemIndex >= this.FilteredSubjects.Count)
            {
                this.selectedItemIndex = Math.Max(0, this.FilteredSubjects.Count - 1);
            }
        }

        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            // Upper close button check
            if (this.upperRightCloseButton != null && this.upperRightCloseButton.containsPoint(x, y))
            {
                if (playSound)
                    Game1.playSound("bigDeSelect");
                this.exitThisMenu();
                return;
            }

            // Clear button check
            if (!string.IsNullOrEmpty(this.SearchBox.Text) && this.clearButtonRect.Contains(x, y))
            {
                this.SearchBox.Text = "";
                this.SearchBox.SelectMe();
                this.SearchBox.Selected = true;
                Game1.keyboardDispatcher.Subscriber = this.SearchBox;
                this.UpdateFilteredList();
                if (playSound)
                    Game1.playSound("coin");
                return;
            }

            // Clicking inside the search box focuses it
            Rectangle searchBoxBounds = new Rectangle(this.SearchBox.X, this.SearchBox.Y, this.SearchBox.Width, this.SearchBox.Height);
            if (searchBoxBounds.Contains(x, y))
            {
                this.SearchBox.SelectMe();
                this.SearchBox.Selected = true;
                Game1.keyboardDispatcher.Subscriber = this.SearchBox;
                return;
            }

            // Scrollbar click to jump
            if (this.scrollbarTrackRect.Contains(x, y))
            {
                this.isDraggingThumb = true;
                this.UpdateScrollFromThumbY(y);
                return;
            }

            // Check item selection
            int listStartY = this.GetListStartY();
            int visibleHeight = this.GetVisibleHeight();
            Rectangle listArea = new Rectangle(this.xPositionOnScreen + 20, listStartY, this.width - 55, visibleHeight);

            if (listArea.Contains(x, y))
            {
                for (int i = 0; i < this.FilteredSubjects.Count; i++)
                {
                    int itemY = listStartY + i * ItemHeight - (int)this.smoothScrollOffset;
                    Rectangle r = new Rectangle(this.xPositionOnScreen + 20, itemY, this.width - 65, ItemHeight);
                    if (r.Contains(x, y))
                    {
                        if (playSound)
                            Game1.playSound("smallSelect");
                        this.OnSelect(this.FilteredSubjects[i].Item);
                        this.exitThisMenu();
                        return;
                    }
                }
            }

            base.receiveLeftClick(x, y, playSound);
        }

        public override void leftClickHeld(int x, int y)
        {
            if (this.isDraggingThumb)
            {
                this.UpdateScrollFromThumbY(y);
                return;
            }

            int listStartY = this.GetListStartY();
            int visibleHeight = this.GetVisibleHeight();
            Rectangle scrollRect = new Rectangle(this.xPositionOnScreen + 20, listStartY, this.width - 40, visibleHeight);

            if (scrollRect.Contains(x, y))
            {
                if (!this.isDraggingScroll)
                {
                    this.isDraggingScroll = true;
                    this.lastDragY = y;
                }
                else
                {
                    int delta = y - this.lastDragY;
                    this.lastDragY = y;
                    int totalContentHeight = this.FilteredSubjects.Count * ItemHeight;
                    int maxScroll = Math.Max(0, totalContentHeight - visibleHeight);
                    this.targetScrollOffset = Math.Clamp(this.targetScrollOffset - delta, 0, maxScroll);
                }
            }

            base.leftClickHeld(x, y);
        }

        public override void releaseLeftClick(int x, int y)
        {
            this.isDraggingScroll = false;
            this.isDraggingThumb = false;
            base.releaseLeftClick(x, y);
        }

        private void UpdateScrollFromThumbY(int mouseY)
        {
            int trackTop = this.scrollbarTrackRect.Y;
            int trackHeight = this.scrollbarTrackRect.Height;
            if (trackHeight <= 0) return;

            float relativeY = Math.Clamp((float)(mouseY - trackTop) / trackHeight, 0f, 1f);
            int totalContentHeight = this.FilteredSubjects.Count * ItemHeight;
            int visibleHeight = this.GetVisibleHeight();
            int maxScroll = Math.Max(0, totalContentHeight - visibleHeight);

            this.targetScrollOffset = (int)(relativeY * maxScroll);
        }

        public override void receiveScrollWheelAction(int direction)
        {
            int totalContentHeight = this.FilteredSubjects.Count * ItemHeight;
            int visibleHeight = this.GetVisibleHeight();
            int maxScroll = Math.Max(0, totalContentHeight - visibleHeight);

            int scrollIncrement = 70;
            this.targetScrollOffset = Math.Clamp(this.targetScrollOffset - Math.Sign(direction) * scrollIncrement, 0, maxScroll);
        }

        private void EnsureItemVisible(int index)
        {
            if (index < 0 || index >= this.FilteredSubjects.Count)
                return;

            int itemTop = index * ItemHeight;
            int itemBottom = itemTop + ItemHeight;
            int visibleHeight = this.GetVisibleHeight();

            if (itemTop < this.targetScrollOffset)
            {
                this.targetScrollOffset = itemTop;
            }
            else if (itemBottom > this.targetScrollOffset + visibleHeight)
            {
                this.targetScrollOffset = itemBottom - visibleHeight;
            }

            int totalContentHeight = this.FilteredSubjects.Count * ItemHeight;
            int maxScroll = Math.Max(0, totalContentHeight - visibleHeight);
            this.targetScrollOffset = Math.Clamp(this.targetScrollOffset, 0, maxScroll);
        }

        public override void receiveKeyPress(Keys key)
        {
            // Exit on Escape
            if (key == Keys.Escape)
            {
                this.exitThisMenu();
                return;
            }

            // Keyboard navigation: Enter to select
            if (key == Keys.Enter)
            {
                if (this.FilteredSubjects.Count > 0)
                {
                    int index = Math.Clamp(this.selectedItemIndex, 0, this.FilteredSubjects.Count - 1);
                    Game1.playSound("smallSelect");
                    this.OnSelect(this.FilteredSubjects[index].Item);
                    this.exitThisMenu();
                    return;
                }
            }

            // Navigate down
            if (key == Keys.Down)
            {
                if (this.FilteredSubjects.Count > 0 && this.selectedItemIndex < this.FilteredSubjects.Count - 1)
                {
                    this.selectedItemIndex++;
                    this.EnsureItemVisible(this.selectedItemIndex);
                }
                return;
            }

            // Navigate up
            if (key == Keys.Up)
            {
                if (this.FilteredSubjects.Count > 0 && this.selectedItemIndex > 0)
                {
                    this.selectedItemIndex--;
                    this.EnsureItemVisible(this.selectedItemIndex);
                }
                return;
            }

            // Page down
            if (key == Keys.PageDown)
            {
                int pageSize = Math.Max(1, this.GetVisibleHeight() / ItemHeight);
                this.selectedItemIndex = Math.Min(this.FilteredSubjects.Count - 1, this.selectedItemIndex + pageSize);
                this.EnsureItemVisible(this.selectedItemIndex);
                return;
            }

            // Page up
            if (key == Keys.PageUp)
            {
                int pageSize = Math.Max(1, this.GetVisibleHeight() / ItemHeight);
                this.selectedItemIndex = Math.Max(0, this.selectedItemIndex - pageSize);
                this.EnsureItemVisible(this.selectedItemIndex);
                return;
            }

            // If textbox dropped focus, re-focus it when keys are pressed
            if (!this.SearchBox.Selected || Game1.keyboardDispatcher.Subscriber != this.SearchBox)
            {
                this.SearchBox.SelectMe();
                this.SearchBox.Selected = true;
                Game1.keyboardDispatcher.Subscriber = this.SearchBox;
            }
        }

        public override void update(GameTime time)
        {
            base.update(time);

            // Auto-focus logic: keep selected until user releases mouse button
            if (this.needsInitialFocus)
            {
                this.SearchBox.SelectMe();
                this.SearchBox.Selected = true;
                Game1.keyboardDispatcher.Subscriber = this.SearchBox;

                if (Game1.input.GetMouseState().LeftButton == ButtonState.Released)
                {
                    this.needsInitialFocus = false;
                }
            }

            this.SearchBox.Update();

            if (this.needsInitialFocus)
            {
                this.SearchBox.SelectMe();
                this.SearchBox.Selected = true;
                Game1.keyboardDispatcher.Subscriber = this.SearchBox;
            }

            this.UpdateFilteredList();

            // Clamp target scroll
            int totalContentHeight = this.FilteredSubjects.Count * ItemHeight;
            int visibleHeight = this.GetVisibleHeight();
            int maxScroll = Math.Max(0, totalContentHeight - visibleHeight);
            this.targetScrollOffset = Math.Clamp(this.targetScrollOffset, 0, maxScroll);

            // Smooth interpolation
            this.smoothScrollOffset = MathHelper.Lerp(this.smoothScrollOffset, this.targetScrollOffset, 0.25f);
        }

        protected override void cleanupBeforeExit()
        {
            base.cleanupBeforeExit();
            if (Game1.keyboardDispatcher?.Subscriber == this.SearchBox)
            {
                Game1.keyboardDispatcher.Subscriber = null;
            }
            this.SearchBox.Selected = false;
        }

        public override void draw(SpriteBatch b)
        {
            // Dim background
            b.Draw(
                Game1.fadeToBlackRect,
                new Rectangle(0, 0, Game1.uiViewport.Width, Game1.uiViewport.Height),
                Color.Black * 0.7f
            );

            // Draw menu dialog box
            IClickableMenu.drawTextureBox(
                b,
                Game1.menuTexture,
                new Rectangle(0, 256, 60, 60),
                this.xPositionOnScreen,
                this.yPositionOnScreen,
                this.width,
                this.height,
                Color.White,
                1f,
                drawShadow: true
            );

            // Draw SearchBox
            this.SearchBox.Draw(b);

            // Position and draw Clear ('X') button if there is text in the search box
            if (!string.IsNullOrEmpty(this.SearchBox.Text))
            {
                int clearBtnSize = 28;
                this.clearButtonRect = new Rectangle(
                    this.SearchBox.X + this.SearchBox.Width - clearBtnSize - 8,
                    this.SearchBox.Y + (this.SearchBox.Height - clearBtnSize) / 2,
                    clearBtnSize,
                    clearBtnSize
                );

                bool clearHovered = this.clearButtonRect.Contains(Game1.getMouseX(), Game1.getMouseY());
                Color clearColor = clearHovered ? Color.Red : new Color(125, 125, 125);

                Vector2 xSize = Game1.smallFont.MeasureString("X");
                Vector2 xPos = new Vector2(
                    this.clearButtonRect.X + (this.clearButtonRect.Width - xSize.X) / 2,
                    this.clearButtonRect.Y + (this.clearButtonRect.Height - xSize.Y) / 2
                );
                b.DrawString(Game1.smallFont, "X", xPos, clearColor);
            }
            else
            {
                this.clearButtonRect = Rectangle.Empty;

                // Draw hint text when search query is empty
                string hintText = ModEntry.I18n("search.hint");
                Vector2 hintSize = Game1.smallFont.MeasureString(hintText);
                Vector2 hintPos = new Vector2(this.SearchBox.X + 16, this.SearchBox.Y + (this.SearchBox.Height - hintSize.Y) / 2);
                b.DrawString(Game1.smallFont, hintText, hintPos, new Color(140, 140, 140));
            }

            // Draw search result count / status bar using uniform smallFont
            int listStartY = this.GetListStartY();
            string countText;
            if (this.FilteredSubjects.Count == this.AllSubjects.Count)
            {
                countText = ModEntry.I18n("search.results-count", this.AllSubjects.Count);
            }
            else
            {
                countText = ModEntry.I18n("search.results-count-filtered", this.FilteredSubjects.Count, this.AllSubjects.Count);
            }
            Vector2 countPos = new Vector2(this.xPositionOnScreen + 30, this.SearchBox.Y + this.SearchBox.Height + 8);
            b.DrawString(Game1.smallFont, countText, countPos, new Color(90, 90, 90), 0f, Vector2.Zero, 0.82f, SpriteEffects.None, 1f);

            // Prepare Scissor rectangle for items list clipping
            int visibleHeight = this.GetVisibleHeight();
            int listWidth = this.width - 56;
            Rectangle clipRect = new Rectangle(
                this.xPositionOnScreen + 20,
                listStartY,
                listWidth,
                visibleHeight
            );

            // Check if there are no items
            if (this.FilteredSubjects.Count == 0)
            {
                string emptyText = ModEntry.I18n("search.no-results", this.SearchBox.Text);
                Vector2 textSize = Game1.smallFont.MeasureString(emptyText);
                Vector2 centerPos = new Vector2(
                    this.xPositionOnScreen + (this.width - textSize.X) / 2,
                    listStartY + (visibleHeight - textSize.Y) / 2
                );
                b.DrawString(Game1.smallFont, emptyText, centerPos, Color.DimGray * 0.8f);
            }
            else
            {
                // Scissor test for smooth scroll rendering
                b.End();
                RasterizerState rState = new RasterizerState { ScissorTestEnable = true };
                b.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, rState);
                Game1.graphics.GraphicsDevice.ScissorRectangle = clipRect;

                int mouseX = Game1.getMouseX();
                int mouseY = Game1.getMouseY();

                for (int i = 0; i < this.FilteredSubjects.Count; i++)
                {
                    int itemY = listStartY + i * ItemHeight - (int)this.smoothScrollOffset;

                    // Skip drawing items far outside view
                    if (itemY + ItemHeight < listStartY - 20 || itemY > listStartY + visibleHeight + 20)
                        continue;

                    Rectangle itemRect = new Rectangle(this.xPositionOnScreen + 20, itemY, listWidth - 10, ItemHeight);
                    bool hovered = itemRect.Contains(mouseX, mouseY);
                    bool isSelected = (i == this.selectedItemIndex);

                    // If hovered by mouse, update selectedItemIndex
                    if (hovered && !this.isDraggingScroll && !this.isDraggingThumb)
                    {
                        this.selectedItemIndex = i;
                    }

                    // Draw hover or keyboard-selected background highlight
                    if (hovered || isSelected)
                    {
                        Color highlightColor = isSelected ? (Color.Gold * 0.22f) : (Color.White * 0.15f);
                        b.Draw(Game1.staminaRect, itemRect, highlightColor);

                        // Draw subtle left indicator bar
                        b.Draw(
                            Game1.staminaRect,
                            new Rectangle(itemRect.X, itemRect.Y, 4, itemRect.Height),
                            isSelected ? Color.Goldenrod : Color.CornflowerBlue
                        );
                    }

                    // Draw item icon using ItemDrawHelper
                    Item currentItem = this.FilteredSubjects[i].Item;
                    float itemScale = 2.4f;
                    float iconSize = 16f * itemScale;
                    Vector2 iconPos = new Vector2(itemRect.X + 16, itemRect.Y + (itemRect.Height - iconSize) / 2f);
                    ItemDrawHelper.DrawItemIcon(b, currentItem, iconPos, itemScale);

                    float textStartX = itemRect.X + iconSize + 28;

                    // Secondary info (category / internal English name if different)
                    string displayName = this.FilteredSubjects[i].Name;
                    string subInfo = "";
                    try
                    {
                        string category = currentItem?.getCategoryName() ?? "";
                        if (!string.IsNullOrEmpty(category))
                            subInfo = category;

                        if (currentItem?.Name != null && currentItem.Name != displayName)
                        {
                            subInfo = string.IsNullOrEmpty(subInfo) ? currentItem.Name : $"{subInfo} ({currentItem.Name})";
                        }
                    }
                    catch { }

                    // Measure text for perfect vertical centering
                    float nameHeight = Game1.smallFont.MeasureString(displayName).Y;
                    float subScale = 0.75f;
                    float subHeight = !string.IsNullOrEmpty(subInfo) ? (Game1.smallFont.MeasureString(subInfo).Y * subScale) : 0f;
                    float totalTextHeight = !string.IsNullOrEmpty(subInfo) ? (nameHeight + subHeight + 2f) : nameHeight;
                    float textStartY = itemRect.Y + (itemRect.Height - totalTextHeight) / 2f;

                    // Draw item display name with standard crisp smallFont
                    Vector2 namePos = new Vector2(textStartX, textStartY);
                    Color nameColor = (hovered || isSelected) ? Color.Black : new Color(35, 35, 35);
                    b.DrawString(Game1.smallFont, displayName, namePos, nameColor);

                    // Draw secondary info with scaled-down smallFont for perfect font harmony
                    if (!string.IsNullOrEmpty(subInfo))
                    {
                        Vector2 subPos = new Vector2(textStartX, textStartY + nameHeight + 2f);
                        b.DrawString(Game1.smallFont, subInfo, subPos, new Color(115, 115, 115), 0f, Vector2.Zero, subScale, SpriteEffects.None, 1f);
                    }

                    // Draw bottom divider line
                    if (i < this.FilteredSubjects.Count - 1)
                    {
                        b.Draw(
                            Game1.staminaRect,
                            new Rectangle(itemRect.X + 6, itemRect.Bottom - 1, itemRect.Width - 12, 1),
                            Color.Gray * 0.25f
                        );
                    }
                }

                b.End();
                b.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
            }

            // Draw Scrollbar if content exceeds visible area
            int totalHeight = this.FilteredSubjects.Count * ItemHeight;
            if (totalHeight > visibleHeight)
            {
                int trackWidth = 10;
                int trackX = this.xPositionOnScreen + this.width - 24;
                this.scrollbarTrackRect = new Rectangle(trackX, listStartY, trackWidth, visibleHeight);

                // Draw track
                b.Draw(Game1.staminaRect, this.scrollbarTrackRect, Color.Black * 0.12f);

                // Draw thumb
                int maxScroll = totalHeight - visibleHeight;
                float scrollRatio = Math.Clamp(this.smoothScrollOffset / maxScroll, 0f, 1f);
                int thumbHeight = Math.Max(30, (int)((float)visibleHeight / totalHeight * visibleHeight));
                int thumbY = listStartY + (int)(scrollRatio * (visibleHeight - thumbHeight));

                this.scrollbarThumbRect = new Rectangle(trackX, thumbY, trackWidth, thumbHeight);
                bool thumbHovered = this.scrollbarThumbRect.Contains(Game1.getMouseX(), Game1.getMouseY());
                Color thumbColor = (this.isDraggingThumb || thumbHovered) ? Color.DimGray : Color.Gray * 0.7f;

                b.Draw(Game1.staminaRect, this.scrollbarThumbRect, thumbColor);
            }
            else
            {
                this.scrollbarTrackRect = Rectangle.Empty;
                this.scrollbarThumbRect = Rectangle.Empty;
            }

            // Base draw (for close button) and cursor
            base.draw(b);
            this.drawMouse(b);
        }
    }
}
