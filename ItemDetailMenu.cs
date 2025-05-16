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

        public ItemDetailMenu(Item item)
            : base(
                  Game1.viewport.Width / 8,  // xPositionOnScreen (moved further left)
                  Game1.viewport.Height / 8, // yPositionOnScreen (moved further down)
                  Game1.viewport.Width * 3 / 4,  // width (three-quarters of viewport width)
                  Game1.viewport.Height * 3 / 4, // height (three-quarters of viewport height)
                  true)
        {
            this.item = item;
            // Use the item's description if available.
            this.description = (item as StardewValley.Object)?.getDescription() ?? "No description available.";
            // Determine the current location of the item.
            this.locationInfo = "Location: " + DetermineLocation(item);
            //this.obtainInfo = "How to obtain: Typically purchased from shops, dropped as loot, or crafted/farmed in-game.";
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

            // Check if the item is in the player's inventory and count occurrences
            int inventoryCount = Game1.player.Items.Count(i => ItemsMatch(i, item));
            if (inventoryCount > 0)
            {
                locations.Add($"Player Inventory ({inventoryCount} items)");
            }

            // Search through all locations.
            foreach (GameLocation location in Game1.locations)
            {
                // Check chests in the location.
                foreach (var obj in location.objects.Values)
                {
                    if (obj is Chest chest)
                    {
                        // Count the number of matching items in each chest.
                        int itemCount = chest.Items.Count(i => ItemsMatch(i, item));
                        if (itemCount > 0)
                        {
                            string containerKey = $"{chest.DisplayName} in {location.Name}";
                            string locationText = $"{containerKey} ({itemCount} items)";
                            
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

                // Check farmhouse refrigerators.
                if (location is StardewValley.Locations.FarmHouse house)
                {
                    if (house.fridge.Value != null && house.fridge.Value.Items != null)
                    {
                        int fridgeCount = house.fridge.Value.Items.Count(i => ItemsMatch(i, item));
                        if (fridgeCount > 0)
                        {
                            locations.Add($"Fridge in {location.Name} ({fridgeCount} items)");
                        }
                    }
                }
                else if (location is StardewValley.Locations.IslandFarmHouse islandHouse)
                {
                    if (islandHouse.fridge.Value != null && islandHouse.fridge.Value.Items != null)
                    {
                        int fridgeCount = islandHouse.fridge.Value.Items.Count(i => ItemsMatch(i, item));
                        if (fridgeCount > 0)
                        {
                            locations.Add($"Fridge in {location.Name} ({fridgeCount} items)");
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
    }
}