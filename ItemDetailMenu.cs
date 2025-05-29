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
using System.Threading.Tasks;
using Find_Item.Utilities;
using Find_Item.Config;

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
        private Rectangle findButton;
        private const string FindButtonText = "Find";
        private readonly List<ItemSubject> allSubjects;
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

            // Calculate button positions
            CalculateButtons();
        }

        private Rectangle CalculateButtons()
        {
            // Create close button in the top-right corner
            this.closeButton = new Rectangle(
                this.xPositionOnScreen + this.width - 48,  // X position
                this.yPositionOnScreen + 8,                // Y position
                36,                                        // Width
                36                                         // Height
            );

            // Create find button at the bottom of the menu with increased height
            Vector2 findTextSize = Game1.smallFont.MeasureString(FindButtonText);
            this.findButton = new Rectangle(
                this.xPositionOnScreen + (this.width - 300) / 2,  // Center horizontally, increased width to 300
                this.yPositionOnScreen + this.height - 100,       // 100 pixels from bottom (was 80)
                300,                                              // Wider button (was 200)
                70                                                // Taller button (was 50)
            );

            return this.findButton;
        }

        private string GetItemDescription(Item item)
        {
            if (item == null)
                return "No description available.";

            // Get the raw description first
            string rawDescription;
            if (item is StardewValley.Object obj)
                rawDescription = obj.getDescription();
            else if (item is Tool tool)
                rawDescription = tool.Description;
            else
                rawDescription = item.getDescription() ?? "No description available.";

            // Format the description with controlled line breaks
            return FormatDescription(rawDescription);
        }

        private string FormatDescription(string description)
        {
            if (string.IsNullOrEmpty(description))
                return "No description available.";

            // Remove existing line breaks and extra spaces
            description = description.Replace("\n", " ").Replace("\r", " ");
            while (description.Contains("  "))
                description = description.Replace("  ", " ");

            var words = description.Split(' ');
            var lines = new List<string>();
            var currentLine = new List<string>();
            int wordCount = 0;

            foreach (var word in words)
            {
                // If we've reached 12 words and have words in the current line
                if (wordCount >= 12 && currentLine.Count > 0)
                {
                    // Add current line to lines and start a new line
                    lines.Add(string.Join(" ", currentLine));
                    currentLine.Clear();
                    wordCount = 0;
                }

                currentLine.Add(word);
                wordCount++;
            }

            // Add remaining words
            if (currentLine.Count > 0)
            {
                lines.Add(string.Join(" ", currentLine));
            }

            // Join lines with proper line breaks
            return string.Join("\n", lines);
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

            // Draw item icon first
            float itemScale = 2f; // Make the icon a bit larger
            Vector2 iconPos = new Vector2(textPos.X, textPos.Y);

            // Use ItemDrawHelper instead of direct drawing logic
            ItemDrawHelper.DrawItemIcon(b, this.item, iconPos, itemScale);

            // Draw title text after icon with offset - using GetIconWidth from helper
            float iconWidth = ItemDrawHelper.GetIconWidth(itemScale);
            Vector2 titlePos = new Vector2(textPos.X + iconWidth, textPos.Y);
            b.DrawString(Game1.smallFont, $"Item: {this.item.DisplayName}", titlePos, textColor);
            textPos.Y += Math.Max(16 * itemScale, Game1.smallFont.MeasureString(title).Y) + 15;

            b.DrawString(Game1.smallFont, descLine, textPos, textColor);
            textPos.Y += Game1.smallFont.MeasureString(descLine).Y + 20;

            // Draw locations header
            b.DrawString(Game1.smallFont, locationHeader, textPos, headerColor);
            textPos.Y += Game1.smallFont.MeasureString(locationHeader).Y + 5;

            // Draw each location on a new line
            foreach (string location in this.locationInfo.Split('\n'))
            {
                if (!string.IsNullOrWhiteSpace(location))
                {
                    float currentX = textPos.X;
                    
                    if (location.StartsWith("Grand Total:"))
                    {
                        b.DrawString(Game1.smallFont, location.Trim(),
                            new Vector2(currentX, textPos.Y), new Color(218, 63, 39));
                    }
                    else
                    {
                        // Draw bullet point and location name
                        currentX += 20; // Indent for bullet point
                        string baseText = location;
                        if (location.Contains("("))
                        {
                            baseText = location.Substring(0, location.IndexOf("(")).Trim();
                        }
                        b.DrawString(Game1.smallFont, "• " + baseText, 
                            new Vector2(currentX, textPos.Y), textColor);
                        currentX += Game1.smallFont.MeasureString("• " + baseText).X + 10;

                        // Draw quality information
                        if (location.Contains("("))
                        {
                            string quantityInfo = location.Substring(
                                location.IndexOf("(") + 1,
                                location.IndexOf(")") - location.IndexOf("(") - 1
                            );

                            foreach (string quantityPair in quantityInfo.Split('|'))
                            {
                                string[] parts = quantityPair.Trim().Split(':');
                                if (parts.Length == 2 && int.TryParse(parts[0], out int quantity))
                                {
                                    // Draw quantity
                                    string quantityText = quantity.ToString();
                                    b.DrawString(Game1.smallFont, quantityText,
                                        new Vector2(currentX, textPos.Y), textColor);
                                    currentX += Game1.smallFont.MeasureString(quantityText).X + 5;

                                    // Draw quality icon
                                    if (int.TryParse(parts[1], out int quality) && quality > 0)
                                    {
                                        DrawQualityIcon(b, quality, new Vector2(currentX, textPos.Y));
                                        currentX += 20;
                                    }
                                    currentX += 15; // Space between quality indicators
                                }
                            }
                        }

                        // Draw total if present
                        if (location.Contains("[Total:"))
                        {
                            string totalText = location.Substring(
                                location.LastIndexOf("["),
                                location.LastIndexOf("]") - location.LastIndexOf("[") + 1
                            );
                            float totalWidth = Game1.smallFont.MeasureString(totalText).X;
                            b.DrawString(Game1.smallFont, totalText,
                                new Vector2(this.xPositionOnScreen + this.width - 60 - totalWidth, textPos.Y),
                                new Color(103, 152, 235));
                        }
                    }
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

            // Draw find button
            IClickableMenu.drawTextureBox(
                b,
                Game1.menuTexture,
                new Rectangle(0, 256, 60, 60),
                this.findButton.X,
                this.findButton.Y,
                this.findButton.Width,
                this.findButton.Height,
                Color.White,
                1f,
                true);  // Enable shadow for better visibility

            // Draw button text with bigger font
            Vector2 findTextSize = Game1.dialogueFont.MeasureString(FindButtonText); // Use larger dialogue font
            Vector2 findTextPos = new Vector2(
                this.findButton.X + (this.findButton.Width - findTextSize.X) / 2,
                this.findButton.Y + (this.findButton.Height - findTextSize.Y) / 2
            );
            b.DrawString(
                Game1.dialogueFont,  // Use larger dialogue font instead of smallFont
                FindButtonText,
                findTextPos,
                Color.Black
            );

            // Add hover effect
            if (this.findButton.Contains(Game1.getMouseX(), Game1.getMouseY()))
            {
                // Draw highlight effect when mouse hovers over button
                b.Draw(
                    Game1.staminaRect,
                    this.findButton,
                    Color.White * 0.15f
                );
            }

            // Draw the mouse cursor.
            this.drawMouse(b);
        }

        public override void receiveKeyPress(Keys key)
        {
            if (key == Keys.Escape && readyToClose)
                this.exitThisMenu();
            else if (ModEntry.Config != null && key.ToString() == ModEntry.Config.QuickSearchKey.ToString())
            {
                this.exitThisMenu();
                DrawPathToChests();
            }
            // Update hide path key handler to use config
            else if (ModEntry.Config != null && key.ToString() == ModEntry.Config.HidePathKey.ToString())
            {
                ModEntry.shouldDraw = false;
                ModEntry.paths.Clear();
                ModEntry.pathColors.Clear();
                Game1.showGlobalMessage("Path hidden"); // Optional feedback
            }
        }

        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            if (this.closeButton.Contains(x, y))
            {
                if (playSound)
                    Game1.playSound("bigDeSelect");

                // Return to search menu
                Game1.activeClickableMenu = new ItemSearchMenu(this.allSubjects, this.onSelect);
                return;
            }
            else if (this.findButton.Contains(x, y))
            {
                if (playSound)
                    Game1.playSound("select");

                // Close menu first
                this.exitThisMenu();

                // Then draw paths to chests
                DrawPathToChests();
                return;
            }

            //// If clicked outside buttons then exit as usual
            //this.exitThisMenu();
            //base.receiveLeftClick(x, y, playSound);
        }

        public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
        {
            // Create a new instance of the menu with the same item and settings
            Game1.activeClickableMenu = new ItemDetailMenu(this.item, this.allSubjects, this.onSelect);
        }

        /// <summary>
        /// Attempts to determine all locations and container types where the given item exists.
        /// </summary>
        private string DetermineLocation(Item item)
        {
            List<string> locations = new List<string>();
            Dictionary<string, int> containerCounts = new Dictionary<string, int>();
            int grandTotal = 0;

            // Check player's inventory first
            var inventoryGroups = Game1.player.Items
                .Where(i => ItemsMatch(i, item))
                .GroupBy(i => (i as StardewValley.Object)?.Quality ?? -1)
                .OrderBy(g => g.Key);

            // If item is in inventory, return early with a special message
            if (inventoryGroups.Any())
            {
                var quantities = new List<string>();
                int inventoryTotal = 0;
                foreach (var group in inventoryGroups)
                {
                    int total = group.Sum(i => i.Stack);
                    inventoryTotal += total;
                    quantities.Add($"{total}:{group.Key}"); // Format: "quantity:quality"
                }
                grandTotal += inventoryTotal;
                return $"Item is in your inventory: ({string.Join("|", quantities)}) [Total: {inventoryTotal}]";
            }

            // Helper method to process location's chests
            void ProcessLocationChests(GameLocation location, string locationPrefix = "", string buildingName = "")
            {
                // Get cabin name if this is a cabin
                string cabinName = "";
                if (location is StardewValley.Locations.Cabin cabin)
                {
                    string ownerName;
                    if (cabin.owner != null)
                    {
                        ownerName = cabin.owner.Name;
                    }
                    else
                    {
                        // Try to get name from cabin's name
                        ownerName = cabin.NameOrUniqueName.Split(' ')[0];
                    }
                    cabinName = $"{ownerName}'s Cabin";
                }

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
                            // Customize container key based on building type
                            string containerKey;
                            if (!string.IsNullOrEmpty(buildingName))
                            {
                                // For buildings in farm
                                containerKey = $"{chest.DisplayName} in {buildingName}";
                            }
                            else if (!string.IsNullOrEmpty(cabinName))
                            {
                                // For cabin
                                containerKey = $"{chest.DisplayName} in {cabinName}";
                            }
                            else if (!string.IsNullOrEmpty(locationPrefix))
                            {
                                // For other prefixed locations
                                containerKey = $"{chest.DisplayName} in {locationPrefix}{location.DisplayName}";
                            }
                            else
                            {
                                // For regular locations
                                containerKey = $"{chest.DisplayName} in {location.DisplayName}";
                            }

                            // Rest of the chest processing code...
                            var quantities = new List<string>();
                            int containerTotal = 0;

                            foreach (var group in chestGroups)
                            {
                                int total = group.Sum(i => i.Stack);
                                containerTotal += total;
                                quantities.Add($"{total}:{group.Key}");
                            }

                            grandTotal += containerTotal;
                            string locationText = $"{containerKey}: ({string.Join("|", quantities)}) [Total: {containerTotal}]";

                            if (!containerCounts.ContainsKey(containerKey))
                            {
                                containerCounts[containerKey] = 1;
                                locations.Add(locationText);
                            }
                            else
                            {
                                containerCounts[containerKey]++;
                                locations.Add($"{locationText} [Storage #{containerCounts[containerKey]}]");
                            }
                        }
                    }
                }
            }

            // Helper method to check fridges
            void CheckFridge(GameLocation location, string locationPrefix = "", string buildingName = "")
            {
                if (location is StardewValley.Locations.FarmHouse house)
                {
                    if (house.fridge.Value?.Items != null)
                    {
                        var fridgeGroups = house.fridge.Value.Items
                            .Where(i => ItemsMatch(i, item))
                            .GroupBy(i => (i as StardewValley.Object)?.Quality ?? -1)
                            .OrderBy(g => g.Key);

                        if (fridgeGroups.Any())
                        {
                            string locationName = !string.IsNullOrEmpty(buildingName) 
                                ? buildingName 
                                : (!string.IsNullOrEmpty(locationPrefix) ? $"{locationPrefix}{location.DisplayName}" : location.DisplayName);

                            var quantities = new List<string>();
                            int fridgeTotal = 0;

                            foreach (var group in fridgeGroups)
                            {
                                int total = group.Sum(i => i.Stack);
                                fridgeTotal += total;
                                quantities.Add($"{total}:{group.Key}");
                            }

                            grandTotal += fridgeTotal;
                            locations.Add($"Fridge in {locationName}: ({string.Join("|", quantities)}) [Total: {fridgeTotal}]");
                        }
                    }
                }
            }

            // Process all locations
            foreach (GameLocation location in Game1.locations)
            {
                if (location is Farm farm)
                {
                    // Process main farm location
                    ProcessLocationChests(farm);

                    // Process each building on the farm
                    foreach (var building in farm.buildings)
                    {
                        if (building.indoors.Value != null)
                        {
                            string buildingName = $"{building.buildingType} at {farm.DisplayName}";
                            ProcessLocationChests(building.indoors.Value, "", buildingName);
                            CheckFridge(building.indoors.Value, "", buildingName);
                        }
                    }
                }
                else
                {
                    // Process regular location (including cabins)
                    ProcessLocationChests(location);
                    CheckFridge(location);
                }
            }

            if (locations.Count == 0)
                return "Item not found in any storage";

            // Sort locations for better organization
            locations.Sort();
            
            // Add grand total at the beginning
            locations.Insert(0, $"Grand Total: {grandTotal} items");
            return string.Join("\n", locations);
        }

        private bool HasItemInLocation(GameLocation location, Item searchItem)
        {
            // Check chests in the main location
            foreach (var obj in location.objects.Values)
            {
                if (obj is Chest chest && chest.Items.Any(i => ItemsMatch(i, searchItem)))
                    return true;
            }

            // Check fridge
            if (location is StardewValley.Locations.FarmHouse house)
            {
                if (house.fridge.Value?.Items != null &&
                    house.fridge.Value.Items.Any(i => ItemsMatch(i, searchItem)))
                    return true;
            }

            // If it's a farm, check all buildings
            if (location is Farm farm)
            {
                foreach (var building in farm.buildings)
                {
                    if (building.indoors.Value != null)
                    {
                        // Check chests in the building
                        foreach (var obj in building.indoors.Value.objects.Values)
                        {
                            if (obj is Chest chest && chest.Items.Any(i => ItemsMatch(i, searchItem)))
                                return true;
                        }
                    }
                }
            }

            return false;
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
                       obj1.ParentSheetIndex == obj2.ParentSheetIndex; // Removed Quality comparison
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

        /// <summary>
        /// Draws paths to chests containing the selected item in the current location.
        /// </summary>
        private void DrawPathToChests()
        {
            //// Thêm cache cho đường đi
            //if (ModEntry.paths.Count > 0)
            //{
            //    // Clear old paths only when finding new ones
            //    ModEntry.paths.Clear();
            //    ModEntry.pathColors.Clear();
            //}

            // Check if item is in inventory first
            if (Game1.player.Items.Any(i => ItemsMatch(i, item)))
            {
                Game1.showGlobalMessage($"{item.DisplayName} is in your inventory");
                return;
            }

            List<Vector2> chestLocations = new List<Vector2>();
            GameLocation currentLocation = Game1.currentLocation;

            // Find all chests containing the item in current location
            foreach (var obj in currentLocation.objects.Pairs)
            {
                if (obj.Value is Chest chest && chest.Items.Any(i => ItemsMatch(i, item)))
                {
                    chestLocations.Add(obj.Key);
                }
            }

            // Check fridge if in FarmHouse
            if (currentLocation is StardewValley.Locations.FarmHouse house)
            {
                if (house.fridge.Value?.Items != null &&
                    house.fridge.Value.Items.Any(i => ItemsMatch(i, item)))
                {
                    chestLocations.Add(house.fridge.Value.TileLocation);
                }
            }
            else if (currentLocation is StardewValley.Locations.IslandFarmHouse islandHouse)
            {
                if (islandHouse.fridge.Value?.Items != null &&
                    islandHouse.fridge.Value.Items.Any(i => ItemsMatch(i, item)))
                {
                    chestLocations.Add(islandHouse.fridge.Value.TileLocation);
                }
            }

            if (chestLocations.Count == 0)
            {
                // Get a list of other locations where the item exists
                var otherLocations = Game1.locations
                    .Where(loc => loc != currentLocation)
                    .Where(loc => HasItemInLocation(loc, item))
                    .Select(loc => loc.Name)
                    .ToList();

                if (otherLocations.Count > 0)
                {
                    string locationList = string.Join(", ", otherLocations);
                    Game1.showGlobalMessage($"{item.DisplayName} is not in {currentLocation.Name}, but can be found in: {locationList}");
                }
                else
                {
                    Game1.showGlobalMessage($"{item.DisplayName} is not found in any location");
                }
                return;
            }

            // Find paths to all chests
            Vector2 playerPos = new Vector2(
                (int)(Game1.player.Position.X / 64),
                (int)(Game1.player.Position.Y / 64)
            );

            Path_Finding.FindPaths(currentLocation, playerPos, chestLocations);

            if (ModEntry.paths.Count > 0)
            {
                Game1.showGlobalMessage($"Found {item.DisplayName} in {chestLocations.Count} location(s) in {currentLocation.Name}");
                
                // Only auto-hide if enabled in config
                if (ModEntry.Config.AutoHidePaths)
                {
                    Task.Delay(ModEntry.Config.AutoHideDelay * 1000).ContinueWith(_ => 
                    {
                        ModEntry.shouldDraw = false;
                        ModEntry.paths.Clear();
                        ModEntry.pathColors.Clear();
                    });
                }
            }
            else
            {
                Game1.showGlobalMessage($"Cannot find path to {item.DisplayName} in {currentLocation.Name}");
            }
        }

        /// <summary>
        /// Draws the quality icon for an item
        /// </summary>
        private void DrawQualityIcon(SpriteBatch b, int quality, Vector2 position)
        {
            if (quality <= 0) return; // Don't draw anything for normal quality

            Rectangle qualityRect;
            switch (quality)
            {
                case 1: // Silver
                    qualityRect = new Rectangle(338, 400, 8, 8);
                    break;
                case 2: // Gold
                    qualityRect = new Rectangle(346, 400, 8, 8);
                    break;
                case 4: // Iridium
                    qualityRect = new Rectangle(346, 392, 8, 8);
                    break;
                default:
                    return;
            }

            b.Draw(Game1.mouseCursors, position, qualityRect, Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 1f);
        }
    }
}