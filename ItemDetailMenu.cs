//ItemDetailMenu.cs
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Objects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Find_Item
{
    /// <summary>
    /// Displays a larger TextureBox showing full details about a selected item.
    /// </summary>
    public class ItemDetailMenu : IClickableMenu
    {
        private Item item;
        private string description;
        private string locationInfo;
        //private string obtainInfo;

        // Renamed field to hide parent's readyToClose member.
        private new bool readyToClose = false;

        private Rectangle closeButton;
        private Rectangle showPathButton;
        private List<ItemSubject> allSubjects;
        private Action<Item> onSelect;

        public ItemDetailMenu(Item item, List<ItemSubject> subjects, Action<Item> onSelect)
            : base(
                  Game1.viewport.Width / 8,  // xPositionOnScreen (moved further left)
                  Game1.viewport.Height / 8, // yPositionOnScreen (moved further down)
                  Game1.viewport.Width * 3 / 4,  // width (three-quarters of viewport width)
                  Game1.viewport.Height * 3 / 4, // height (three-quarters of viewport height)
                  true)
        {
            this.item = item;
            this.allSubjects = subjects;
            this.onSelect = onSelect;
            // Use the item's description if available.
            this.description = GetItemDescription(item);
            // Determine the current location of the item.
            this.locationInfo = DetermineLocation(item);

            // Create close button in the top-right corner
            this.closeButton = new Rectangle(
                this.xPositionOnScreen + this.width - 48,  // X position
                this.yPositionOnScreen + 8,                // Y position
                36,                                        // Width
                36                                         // Height
            );

            // Create show path button below close button
            this.showPathButton = new Rectangle(
                this.xPositionOnScreen + this.width - 48,  // Same X as close button
                this.yPositionOnScreen + 50,              // Below close button
                36,                                       // Width
                36                                        // Height
            );
        }

        private string GetItemDescription(Item item)
        {
            if (item == null)
                return "No description available.";

            // Kiểm tra từng loại item khác nhau
            if (item is StardewValley.Object obj)
                return obj.getDescription();
            else if (item is Tool tool)
                return tool.Description;  // Tools có property Description
            else
                return item.getDescription() ?? "No description available.";
        }

        public override void update(GameTime time)
        {
            base.update(time);
            // Allow menu to be closed after the first update.
            readyToClose = true;
        }

        public override void draw(SpriteBatch b)
        {
            // Darken the background.
            b.Draw(Game1.fadeToBlackRect, new Rectangle(0, 0, Game1.viewport.Width, Game1.viewport.Height), Color.Black * 0.75f);

            // Draw a larger texture box for the detail panel.
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
                drawShadow: true);

            // Prepare multiline detail text.
            string title = $"Item: {this.item.DisplayName}";
            string descLine = $"Description: {this.description}";
            string locationHeader = "Locations:";
            //string obtainLine = this.obtainInfo;
            
            // Starting position with a larger margin.
            Vector2 textPos = new Vector2(this.xPositionOnScreen + 40, this.yPositionOnScreen + 40);
            Color textColor = Color.Black;
            Color headerColor = new Color(85, 85, 85); // Slightly darker for headers
            
            // Draw each section
            b.DrawString(Game1.smallFont, title, textPos, textColor);
            textPos.Y += Game1.smallFont.MeasureString(title).Y + 15;

            b.DrawString(Game1.smallFont, descLine, textPos, textColor);
            textPos.Y += Game1.smallFont.MeasureString(descLine).Y + 30;

            // Draw locations header
            b.DrawString(Game1.smallFont, locationHeader, textPos, headerColor);
            textPos.Y += Game1.smallFont.MeasureString(locationHeader).Y + 5;

            // Draw each location on a new line
            foreach (string location in this.locationInfo.Split('\n'))
            {
                if (!string.IsNullOrWhiteSpace(location))
                {
                    b.DrawString(Game1.smallFont, "• " + location.Trim(),
                        new Vector2(textPos.X + 20, textPos.Y), textColor);
                    textPos.Y += Game1.smallFont.MeasureString(location).Y + 5;
                }
            }

            textPos.Y += 15; // Extra spacing after locations
            //b.DrawString(Game1.smallFont, obtainLine, textPos, textColor);
            //textPos.Y += Game1.smallFont.MeasureString(descLine).Y + 30;

            // Draw close button
            b.Draw(Game1.mouseCursors, 
                new Vector2(this.closeButton.X, this.closeButton.Y),
                new Rectangle(337, 494, 9, 9),  // Texture region for the X button
                Color.White,
                0f,
                Vector2.Zero,
                4f,                            // Larger scale for better visibility
                SpriteEffects.None,
                1f);

            // Draw the mouse cursor.
            this.drawMouse(b);
        }

        public override void receiveKeyPress(Keys key)
        {
            if (key == Keys.Escape && readyToClose)
                this.exitThisMenu();
        }

        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            if (this.closeButton.Contains(x, y))
            {
                if (playSound)
                    Game1.playSound("bigDeSelect");
                
                // Quay lại menu tìm kiếm
                Game1.activeClickableMenu = new ItemSearchMenu(this.allSubjects, this.onSelect);
                return;
            }
            
            // Nếu click ngoài nút X thì thoát như cũ
            this.exitThisMenu();
            base.receiveLeftClick(x, y, playSound);
        }

        /// <summary>
        /// Attempts to determine all locations and container types where the given item exists.
        /// </summary>
        private string DetermineLocation(Item item)
        {
            List<string> locations = new List<string>();
            Dictionary<string, int> containerCounts = new Dictionary<string, int>();

            // Check player's inventory
            var inventoryGroups = Game1.player.Items
                .Where(i => ItemsMatch(i, item))
                .GroupBy(i => (i as StardewValley.Object)?.Quality ?? -1)
                .OrderBy(g => g.Key);

            if (inventoryGroups.Any())
            {
                var quantities = new List<string>();
                foreach (var group in inventoryGroups)
                {
                    int total = group.Sum(i => i.Stack);
                    string quality = GetQualityText(group.Key);
                    quantities.Add($"{total} {quality}");
                }
                locations.Add($"Player Inventory ({string.Join(", ", quantities)})");
            }

            // Search through all locations
            foreach (GameLocation location in Game1.locations)
            {
                foreach (var obj in location.objects.Values)
                {
                    if (obj is Chest chest)
                    {
                        var chestGroups = chest.Items
                            .Where(i => ItemsMatch(i, item))
                            .GroupBy(i => (i as StardewValley.Object)?.Quality ?? -1)
                            .OrderBy(g => g.Key);

                        if (chestGroups.Any())
                        {
                            string containerKey = $"{chest.DisplayName} in {location.Name}";
                            var quantities = new List<string>();
                            foreach (var group in chestGroups)
                            {
                                int total = group.Sum(i => i.Stack);
                                string quality = GetQualityText(group.Key);
                                quantities.Add($"{total} {quality}");
                            }
                            string locationText = $"{containerKey} ({string.Join(", ", quantities)})";

                            if (!containerCounts.ContainsKey(containerKey))
                            {
                                containerCounts[containerKey] = 1;
                                locations.Add(locationText);
                            }
                            else
                            {
                                containerCounts[containerKey]++;
                                int lastIndex = locations.FindLastIndex(loc => loc.StartsWith(containerKey));
                                if (lastIndex >= 0)
                                {
                                    locations[lastIndex] = $"{locationText} [Storage #{containerCounts[containerKey]}]";
                                }
                            }
                        }
                    }
                }

                // Check fridges with quality grouping
                if (location is StardewValley.Locations.FarmHouse house)
                {
                    if (house.fridge.Value != null && house.fridge.Value.Items != null)
                    {
                        var fridgeGroups = house.fridge.Value.Items
                            .Where(i => ItemsMatch(i, item))
                            .GroupBy(i => (i as StardewValley.Object)?.Quality ?? -1)
                            .OrderBy(g => g.Key);

                        if (fridgeGroups.Any())
                        {
                            var quantities = new List<string>();
                            foreach (var group in fridgeGroups)
                            {
                                int total = group.Sum(i => i.Stack);
                                string quality = GetQualityText(group.Key);
                                quantities.Add($"{total} {quality}");
                            }
                            locations.Add($"Fridge in {location.Name} ({string.Join(", ", quantities)})");
                        }
                    }
                }
                else if (location is StardewValley.Locations.IslandFarmHouse islandHouse)
                {
                    if (islandHouse.fridge.Value != null && islandHouse.fridge.Value.Items != null)
                    {
                        var fridgeGroups = islandHouse.fridge.Value.Items
                            .Where(i => ItemsMatch(i, item))
                            .GroupBy(i => (i as StardewValley.Object)?.Quality ?? -1)
                            .OrderBy(g => g.Key);

                        if (fridgeGroups.Any())
                        {
                            var quantities = new List<string>();
                            foreach (var group in fridgeGroups)
                            {
                                int total = group.Sum(i => i.Stack);
                                string quality = GetQualityText(group.Key);
                                quantities.Add($"{total} {quality}");
                            }
                            locations.Add($"Fridge in {location.Name} ({string.Join(", ", quantities)})");
                        }
                    }
                }
            }

            if (locations.Count == 0)
                return "Unknown";

            return string.Join("\n", locations);
        }

        /// <summary>
        /// Compares two items to determine if they are the same type and have the same basic properties.
        /// </summary>
        private bool ItemsMatch(Item item1, Item item2)
        {
            if (item1 == null || item2 == null)
                return false;

            // Compare basic properties
            bool basicMatch = item1.Name == item2.Name && 
                             item1.DisplayName == item2.DisplayName;

            // Compare detailed properties for StardewValley.Object
            if (item1 is StardewValley.Object obj1 && item2 is StardewValley.Object obj2)
            {
                return basicMatch && 
                       obj1.ParentSheetIndex == obj2.ParentSheetIndex &&
                       obj1.Quality == obj2.Quality;  // Added comparison for Quality
            }

            // Special comparison for other item types
            if (item1.GetType() != item2.GetType())
                return false;

            return basicMatch;
        }

        /// <summary>
        /// Returns a string representation of the item's quality.
        /// </summary>
        private string GetQualityText(int quality)
        {
            return quality switch
            {
                0 => "Normal",
                1 => "Silver",
                2 => "Gold",
                4 => "Iridium",
                _ => ""
            };
        }
    }
}