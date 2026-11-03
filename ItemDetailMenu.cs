// ItemDetailMenu.cs
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using Find_Item.Utilities;
using Find_Item.Config;
using Find_Item.Framework;

namespace Find_Item
{
    /// <summary>
    /// Displays a larger TextureBox showing full details and storage locations for a selected item.
    /// </summary>
    public class ItemDetailMenu : IClickableMenu
    {
        private readonly Item item;
        private readonly string description;
        private readonly string locationInfo;

        private new bool readyToClose = false;

        private Rectangle closeButton;
        private Rectangle findButton;
        private readonly List<ItemSubject> allSubjects;
        private readonly Action<Item> onSelect;

        public ItemDetailMenu(Item item, List<ItemSubject> subjects, Action<Item> onSelect)
            : base(
                  GetViewportAdjustedX(),
                  GetViewportAdjustedY(),
                  GetViewportAdjustedWidth(),
                  GetViewportAdjustedHeight(),
                  true)
        {
            this.item = item;
            this.allSubjects = subjects;
            this.onSelect = onSelect;
            this.description = this.GetItemDescription(item);
            this.locationInfo = this.DetermineLocation(item);
            this.CalculateButtons();
        }

        private static int GetViewportAdjustedX()
        {
            return Game1.uiViewport.Width / 8;
        }

        private static int GetViewportAdjustedY()
        {
            return Game1.uiViewport.Height / 8;
        }

        private static int GetViewportAdjustedWidth()
        {
            return Game1.uiViewport.Width * 3 / 4;
        }

        private static int GetViewportAdjustedHeight()
        {
            return Game1.uiViewport.Height * 3 / 4;
        }

        private Rectangle CalculateButtons()
        {
            // Close button in the top-right corner
            this.closeButton = new Rectangle(
                this.xPositionOnScreen + this.width - 48,
                this.yPositionOnScreen + 12,
                36,
                36
            );

            // Find button at the bottom of the menu, dynamically sized based on text
            string findButtonText = ModEntry.I18n("detail.find-button");
            Vector2 findTextSize = Game1.smallFont.MeasureString(findButtonText);
            int buttonWidth = Math.Max(220, (int)findTextSize.X + 60);
            int buttonHeight = 56;

            this.findButton = new Rectangle(
                this.xPositionOnScreen + (this.width - buttonWidth) / 2,
                this.yPositionOnScreen + this.height - buttonHeight - 20,
                buttonWidth,
                buttonHeight
            );

            return this.findButton;
        }

        private string GetItemDescription(Item item)
        {
            if (item == null)
                return ModEntry.I18n("detail.no-description");

            string rawDescription;
            if (item is StardewValley.Object obj)
                rawDescription = obj.getDescription();
            else if (item is Tool tool)
                rawDescription = tool.Description;
            else
                rawDescription = item.getDescription() ?? ModEntry.I18n("detail.no-description");

            return this.FormatDescription(rawDescription);
        }

        private string FormatDescription(string description)
        {
            if (string.IsNullOrEmpty(description))
                return ModEntry.I18n("detail.no-description");

            description = description.Replace("\n", " ").Replace("\r", " ");
            while (description.Contains("  "))
                description = description.Replace("  ", " ");

            var words = description.Split(' ');
            var lines = new List<string>();
            var currentLine = new List<string>();
            int wordCount = 0;

            foreach (var word in words)
            {
                if (wordCount >= 14 && currentLine.Count > 0)
                {
                    lines.Add(string.Join(" ", currentLine));
                    currentLine.Clear();
                    wordCount = 0;
                }

                currentLine.Add(word);
                wordCount++;
            }

            if (currentLine.Count > 0)
            {
                lines.Add(string.Join(" ", currentLine));
            }

            return string.Join("\n", lines);
        }

        public override void update(GameTime time)
        {
            base.update(time);
            this.readyToClose = true;
        }

        public override void draw(SpriteBatch b)
        {
            // Dim background
            b.Draw(
                Game1.fadeToBlackRect,
                new Rectangle(0, 0, Game1.uiViewport.Width, Game1.uiViewport.Height),
                Color.Black * 0.75f
            );

            // Dialog texture box
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

            // Text positions
            string title = ModEntry.I18n("detail.item", this.item.DisplayName);
            string descLine = ModEntry.I18n("detail.description", this.description);
            string locationHeader = ModEntry.I18n("detail.locations");

            Vector2 textPos = new Vector2(this.xPositionOnScreen + 40, this.yPositionOnScreen + 40);
            Color textColor = Color.Black;
            Color headerColor = new Color(75, 75, 75);

            // Draw item icon
            float itemScale = 2.4f;
            Vector2 iconPos = new Vector2(textPos.X, textPos.Y);
            ItemDrawHelper.DrawItemIcon(b, this.item, iconPos, itemScale);

            // Draw title text after icon centered with icon
            float iconWidth = ItemDrawHelper.GetIconWidth(itemScale) + 16;
            Vector2 titlePos = new Vector2(textPos.X + iconWidth, textPos.Y + (16 * itemScale - Game1.smallFont.MeasureString(title).Y) / 2f);
            b.DrawString(Game1.smallFont, title, titlePos, textColor);
            textPos.Y += Math.Max(16 * itemScale, Game1.smallFont.MeasureString(title).Y) + 18;

            // Draw description
            b.DrawString(Game1.smallFont, descLine, textPos, textColor);
            textPos.Y += Game1.smallFont.MeasureString(descLine).Y + 20;

            // Draw locations header
            b.DrawString(Game1.smallFont, locationHeader, textPos, headerColor);
            textPos.Y += Game1.smallFont.MeasureString(locationHeader).Y + 8;

            string grandTotalPrefix = ModEntry.I18n("detail.grand-total", "").Split(':')[0];

            // Draw each location line
            foreach (string location in this.locationInfo.Split('\n'))
            {
                if (string.IsNullOrWhiteSpace(location))
                    continue;

                float currentX = textPos.X;

                // Grand Total line
                if (location.StartsWith("Grand Total:") || (!string.IsNullOrEmpty(grandTotalPrefix) && location.StartsWith(grandTotalPrefix)))
                {
                    b.DrawString(Game1.smallFont, location.Trim(), new Vector2(currentX, textPos.Y), new Color(218, 63, 39));
                }
                else
                {
                    currentX += 20; // Indent
                    string baseText = location;
                    if (location.Contains("("))
                    {
                        baseText = location.Substring(0, location.IndexOf("(")).Trim();
                    }
                    b.DrawString(Game1.smallFont, "• " + baseText, new Vector2(currentX, textPos.Y), textColor);
                    currentX += Game1.smallFont.MeasureString("• " + baseText).X + 10;

                    // Draw quality information
                    if (location.Contains("(") && location.Contains(")"))
                    {
                        int openParen = location.IndexOf("(");
                        int closeParen = location.IndexOf(")");
                        if (closeParen > openParen)
                        {
                            string qualityInfo = location.Substring(openParen + 1, closeParen - openParen - 1);
                            foreach (string q in qualityInfo.Split('|'))
                            {
                                var parts = q.Split(':');
                                if (parts.Length == 2)
                                {
                                    string quantity = parts[0];
                                    if (int.TryParse(parts[1], out int quality))
                                    {
                                        string quantityText = $"x{quantity}";
                                        b.DrawString(Game1.smallFont, quantityText, new Vector2(currentX, textPos.Y), textColor);
                                        currentX += Game1.smallFont.MeasureString(quantityText).X + 5;

                                        if (quality > 0)
                                        {
                                            this.DrawQualityIcon(b, quality, new Vector2(currentX, textPos.Y + 2));
                                            currentX += 20;
                                        }
                                        currentX += 10;
                                    }
                                }
                            }

                            if (location.Contains("["))
                            {
                                string totalText = location.Substring(location.IndexOf("["));
                                b.DrawString(Game1.smallFont, totalText, new Vector2(currentX, textPos.Y), new Color(110, 110, 110));
                            }
                        }
                    }
                }
                textPos.Y += Game1.smallFont.MeasureString(location).Y + 5;
            }

            // Draw Close Button (X)
            b.Draw(
                Game1.mouseCursors,
                new Vector2(this.closeButton.X, this.closeButton.Y),
                new Rectangle(337, 494, 9, 9),
                Color.White,
                0f,
                Vector2.Zero,
                4f,
                SpriteEffects.None,
                1f
            );

            // Draw Find Button
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
                true
            );

            string findButtonText = ModEntry.I18n("detail.find-button");
            Vector2 findTextSize = Game1.smallFont.MeasureString(findButtonText);
            Vector2 findTextPos = new Vector2(
                this.findButton.X + (this.findButton.Width - findTextSize.X) / 2,
                this.findButton.Y + (this.findButton.Height - findTextSize.Y) / 2
            );

            b.DrawString(
                Game1.smallFont,
                findButtonText,
                findTextPos,
                Color.Black
            );

            // Button hover effect
            if (this.findButton.Contains(Game1.getMouseX(), Game1.getMouseY()))
            {
                b.Draw(Game1.staminaRect, this.findButton, Color.White * 0.18f);
            }

            this.drawMouse(b);
        }

        public override void receiveKeyPress(Keys key)
        {
            if ((key == Keys.Escape || key == Keys.Back) && this.readyToClose)
            {
                // Return to search menu
                Game1.playSound("bigDeSelect");
                Game1.activeClickableMenu = new ItemSearchMenu(this.allSubjects, this.onSelect);
            }
            else if (ModEntry.Config != null && key == (Keys)ModEntry.Config.QuickSearchKey)
            {
                this.exitThisMenu();
                this.DrawPathToChests();
            }
            else if (ModEntry.Config != null && key == (Keys)ModEntry.Config.HidePathKey)
            {
                ModEntry.shouldDraw = false;
                ModEntry.paths.Clear();
                ModEntry.pathColors.Clear();
                Game1.showGlobalMessage(ModEntry.I18n("message.path-hidden"));
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

                this.exitThisMenu();
                this.DrawPathToChests();
                return;
            }

            base.receiveLeftClick(x, y, playSound);
        }

        public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
        {
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

            // Check player's inventory first (direct items)
            var inventoryGroups = Game1.player.Items
                .Where(i => this.ItemsMatch(i, item))
                .GroupBy(i => (i as StardewValley.Object)?.Quality ?? -1)
                .OrderBy(g => g.Key);

            if (inventoryGroups.Any())
            {
                var quantities = new List<string>();
                int inventoryTotal = 0;
                foreach (var group in inventoryGroups)
                {
                    int total = group.Sum(i => i.Stack);
                    inventoryTotal += total;
                    quantities.Add($"{total}:{group.Key}");
                }
                grandTotal += inventoryTotal;
                string inventoryName = ModEntry.I18n("detail.inventory");
                string totalStr = ModEntry.I18n("detail.total", inventoryTotal);
                locations.Add($"{inventoryName}: ({string.Join("|", quantities)}) {totalStr}");
            }

            // Check player's inventory bags (ItemBags mod support)
            var invBagItems = ItemBagHelper.GetItemsInsideBags(Game1.player.Items);
            var invBagGroups = invBagItems
                .Where(b => this.ItemsMatch(b.Item, item))
                .GroupBy(b => b.BagPath);

            foreach (var bagGroup in invBagGroups)
            {
                string bagName = bagGroup.Key;
                var qualityGroups = bagGroup
                    .GroupBy(b => (b.Item as StardewValley.Object)?.Quality ?? -1)
                    .OrderBy(g => g.Key);

                var quantities = new List<string>();
                int bagTotal = 0;
                foreach (var group in qualityGroups)
                {
                    int total = group.Sum(b => b.Item.Stack);
                    bagTotal += total;
                    quantities.Add($"{total}:{group.Key}");
                }

                grandTotal += bagTotal;
                string inventoryName = ModEntry.I18n("detail.inventory");
                string totalStr = ModEntry.I18n("detail.total", bagTotal);
                locations.Add($"{inventoryName} ({bagName}): ({string.Join("|", quantities)}) {totalStr}");
            }

            // Helper method to process location's chests
            void ProcessLocationChests(GameLocation location, string locationPrefix = "", string buildingName = "")
            {
                string cabinName = "";
                if (location is StardewValley.Locations.Cabin cabin)
                {
                    cabinName = this.GetCabinOwnerName(cabin);
                }

                foreach (var obj in location.objects.Values)
                {
                    Chest? targetChest = obj as Chest;
                    string containerTitle = obj.DisplayName;

                    // Support Auto-Grabbers
                    if (targetChest == null && obj.heldObject.Value is Chest grabberChest)
                    {
                        targetChest = grabberChest;
                        containerTitle = $"{obj.DisplayName} ({grabberChest.DisplayName})";
                    }

                    if (targetChest?.Items != null)
                    {
                        string containerKey;
                        if (!string.IsNullOrEmpty(cabinName))
                        {
                            containerKey = ModEntry.I18n("detail.container-in", containerTitle, cabinName);
                        }
                        else if (!string.IsNullOrEmpty(buildingName))
                        {
                            containerKey = ModEntry.I18n("detail.container-in", containerTitle, buildingName);
                        }
                        else if (!string.IsNullOrEmpty(locationPrefix))
                        {
                            containerKey = ModEntry.I18n("detail.container-in", containerTitle, $"{locationPrefix}{location.DisplayName}");
                        }
                        else
                        {
                            containerKey = ModEntry.I18n("detail.container-in", containerTitle, location.DisplayName);
                        }

                        // 1. Check direct items in chest
                        var chestGroups = targetChest.Items
                            .Where(i => this.ItemsMatch(i, item))
                            .GroupBy(i => (i as StardewValley.Object)?.Quality ?? -1)
                            .OrderBy(g => g.Key);

                        if (chestGroups.Any())
                        {
                            var quantities = new List<string>();
                            int containerTotal = 0;

                            foreach (var group in chestGroups)
                            {
                                int total = group.Sum(i => i.Stack);
                                containerTotal += total;
                                quantities.Add($"{total}:{group.Key}");
                            }

                            grandTotal += containerTotal;
                            string totalLabel = ModEntry.I18n("detail.total", containerTotal);
                            string locationText = $"{containerKey}: ({string.Join("|", quantities)}) {totalLabel}";

                            if (!containerCounts.ContainsKey(containerKey))
                            {
                                containerCounts[containerKey] = 1;
                                locations.Add(locationText);
                            }
                            else
                            {
                                containerCounts[containerKey]++;
                                string storageNum = ModEntry.I18n("detail.storage-num", containerCounts[containerKey]);
                                locations.Add($"{locationText} {storageNum}");
                            }
                        }

                        // 2. Check items inside any ItemBag in chest (ItemBags mod support)
                        var chestBagItems = ItemBagHelper.GetItemsInsideBags(targetChest.Items);
                        var chestBagGroups = chestBagItems
                            .Where(b => this.ItemsMatch(b.Item, item))
                            .GroupBy(b => b.BagPath);

                        foreach (var bagGroup in chestBagGroups)
                        {
                            string bagContainerKey = $"{containerKey} ({bagGroup.Key})";
                            var qualityGroups = bagGroup
                                .GroupBy(b => (b.Item as StardewValley.Object)?.Quality ?? -1)
                                .OrderBy(g => g.Key);

                            var quantities = new List<string>();
                            int bagTotal = 0;

                            foreach (var group in qualityGroups)
                            {
                                int total = group.Sum(b => b.Item.Stack);
                                bagTotal += total;
                                quantities.Add($"{total}:{group.Key}");
                            }

                            grandTotal += bagTotal;
                            string totalLabel = ModEntry.I18n("detail.total", bagTotal);
                            string locationText = $"{bagContainerKey}: ({string.Join("|", quantities)}) {totalLabel}";

                            if (!containerCounts.ContainsKey(bagContainerKey))
                            {
                                containerCounts[bagContainerKey] = 1;
                                locations.Add(locationText);
                            }
                            else
                            {
                                containerCounts[bagContainerKey]++;
                                string storageNum = ModEntry.I18n("detail.storage-num", containerCounts[bagContainerKey]);
                                locations.Add($"{locationText} {storageNum}");
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
                    Chest? fridge = house.fridge.Value;
                    string locationName;

                    if (location is StardewValley.Locations.Cabin cabin)
                    {
                        locationName = this.GetCabinOwnerName(cabin);
                    }
                    else
                    {
                        locationName = !string.IsNullOrEmpty(buildingName) ? buildingName : "Farmhouse";
                    }

                    if (fridge?.Items != null)
                    {
                        string fridgeTitle = ModEntry.I18n("detail.fridge-in", locationName);

                        // 1. Direct items in fridge
                        var fridgeGroups = fridge.Items
                            .Where(i => this.ItemsMatch(i, item))
                            .GroupBy(i => (i as StardewValley.Object)?.Quality ?? -1)
                            .OrderBy(g => g.Key);

                        if (fridgeGroups.Any())
                        {
                            var quantities = new List<string>();
                            int fridgeTotal = 0;

                            foreach (var group in fridgeGroups)
                            {
                                int total = group.Sum(i => i.Stack);
                                fridgeTotal += total;
                                quantities.Add($"{total}:{group.Key}");
                            }

                            grandTotal += fridgeTotal;
                            string totalLabel = ModEntry.I18n("detail.total", fridgeTotal);
                            locations.Add($"{fridgeTitle}: ({string.Join("|", quantities)}) {totalLabel}");
                        }

                        // 2. Check items inside any ItemBag in fridge (ItemBags mod support)
                        var fridgeBagItems = ItemBagHelper.GetItemsInsideBags(fridge.Items);
                        var fridgeBagGroups = fridgeBagItems
                            .Where(b => this.ItemsMatch(b.Item, item))
                            .GroupBy(b => b.BagPath);

                        foreach (var bagGroup in fridgeBagGroups)
                        {
                            string bagFridgeTitle = $"{fridgeTitle} ({bagGroup.Key})";
                            var qualityGroups = bagGroup
                                .GroupBy(b => (b.Item as StardewValley.Object)?.Quality ?? -1)
                                .OrderBy(g => g.Key);

                            var quantities = new List<string>();
                            int bagTotal = 0;

                            foreach (var group in qualityGroups)
                            {
                                int total = group.Sum(b => b.Item.Stack);
                                bagTotal += total;
                                quantities.Add($"{total}:{group.Key}");
                            }

                            grandTotal += bagTotal;
                            string totalLabel = ModEntry.I18n("detail.total", bagTotal);
                            locations.Add($"{bagFridgeTitle}: ({string.Join("|", quantities)}) {totalLabel}");
                        }
                    }
                }
            }

            // Process all locations
            foreach (GameLocation location in Game1.locations)
            {
                if (location is Farm farm)
                {
                    ProcessLocationChests(farm);

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
                    ProcessLocationChests(location);
                    CheckFridge(location);
                }
            }

            if (locations.Count == 0)
                return ModEntry.I18n("detail.not-found-storage");

            locations.Sort();
            locations.Insert(0, ModEntry.I18n("detail.grand-total", grandTotal));
            return string.Join("\n", locations);
        }

        private bool ItemsMatch(Item item1, Item item2)
        {
            if (item1 == null || item2 == null)
                return false;

            bool basicMatch = item1.Name == item2.Name && item1.DisplayName == item2.DisplayName;

            if (item1 is StardewValley.Object obj1 && item2 is StardewValley.Object obj2)
            {
                return basicMatch && obj1.ParentSheetIndex == obj2.ParentSheetIndex;
            }

            if (item1.GetType() != item2.GetType())
                return false;

            return basicMatch;
        }

        /// <summary>
        /// Draws paths to chests containing the selected item in the current location.
        /// </summary>
        private void DrawPathToChests()
        {
            GameLocation currentLocation = Game1.currentLocation;
            List<Vector2> chestLocations = new List<Vector2>();
            List<string> containerDetails = new List<string>();

            // Check if item is in inventory first (direct or inside Item Bags)
            bool inInvDirect = Game1.player.Items.Any(i => this.ItemsMatch(i, this.item));
            var invBagItems = ItemBagHelper.GetItemsInsideBags(Game1.player.Items)
                .Where(b => this.ItemsMatch(b.Item, this.item))
                .ToList();

            if (inInvDirect || invBagItems.Any())
            {
                string invTitle = ModEntry.I18n("detail.inventory");
                if (invBagItems.Any())
                {
                    string bagNames = string.Join(", ", invBagItems.Select(b => b.BagPath).Distinct());
                    invTitle = $"{invTitle} ({bagNames})";
                }
                containerDetails.Add(invTitle);
            }

            void CheckLocationContainers(GameLocation location, string locationPrefix = "")
            {
                string cabinName = "";
                if (location is StardewValley.Locations.Cabin currentCabin)
                {
                    cabinName = this.GetCabinOwnerName(currentCabin);
                }

                // Check objects (chests & auto-grabbers)
                foreach (var obj in location.objects.Pairs)
                {
                    Chest? targetChest = obj.Value as Chest;
                    string containerTitle = obj.Value.DisplayName;

                    if (targetChest == null && obj.Value.heldObject.Value is Chest grabberChest)
                    {
                        targetChest = grabberChest;
                        containerTitle = $"{obj.Value.DisplayName} ({grabberChest.DisplayName})";
                    }

                    if (targetChest?.Items != null)
                    {
                        bool directMatch = targetChest.Items.Any(i => this.ItemsMatch(i, this.item));
                        var matchingBags = ItemBagHelper.GetItemsInsideBags(targetChest.Items)
                            .Where(b => this.ItemsMatch(b.Item, this.item))
                            .ToList();

                        if (directMatch || matchingBags.Any())
                        {
                            string finalTitle = containerTitle;
                            if (matchingBags.Any())
                            {
                                string bagNames = string.Join(", ", matchingBags.Select(b => b.BagPath).Distinct());
                                finalTitle = $"{finalTitle} ({bagNames})";
                            }

                            if (location == currentLocation)
                            {
                                chestLocations.Add(obj.Key);
                                string containerName = !string.IsNullOrEmpty(cabinName)
                                    ? ModEntry.I18n("detail.container-in", finalTitle, cabinName)
                                    : finalTitle;
                                containerDetails.Add(containerName);
                            }
                            else
                            {
                                string locName;
                                if (!string.IsNullOrEmpty(cabinName))
                                {
                                    locName = cabinName;
                                }
                                else
                                {
                                    locName = !string.IsNullOrEmpty(locationPrefix)
                                        ? $"{locationPrefix} {location.DisplayName}"
                                        : location.DisplayName;
                                }
                                containerDetails.Add(ModEntry.I18n("detail.container-in", finalTitle, locName));
                            }
                        }
                    }
                }

                // Check fridge in farmhouse or cabin
                if (location is StardewValley.Locations.FarmHouse house)
                {
                    if (house.fridge.Value?.Items != null)
                    {
                        bool directMatch = house.fridge.Value.Items.Any(i => this.ItemsMatch(i, this.item));
                        var matchingBags = ItemBagHelper.GetItemsInsideBags(house.fridge.Value.Items)
                            .Where(b => this.ItemsMatch(b.Item, this.item))
                            .ToList();

                        if (directMatch || matchingBags.Any())
                        {
                            string locationName = location is StardewValley.Locations.Cabin c ? this.GetCabinOwnerName(c) : "Farmhouse";
                            string fridgeTitle = ModEntry.I18n("detail.fridge-in", locationName);
                            if (matchingBags.Any())
                            {
                                string bagNames = string.Join(", ", matchingBags.Select(b => b.BagPath).Distinct());
                                fridgeTitle = $"{fridgeTitle} ({bagNames})";
                            }

                            if (location == currentLocation)
                            {
                                if (house.fridgePosition != Point.Zero)
                                {
                                    chestLocations.Add(new Vector2(house.fridgePosition.X, house.fridgePosition.Y));
                                }
                                containerDetails.Add(fridgeTitle);
                            }
                            else
                            {
                                containerDetails.Add(fridgeTitle);
                            }
                        }
                    }
                }
            }

            // Check current location first
            CheckLocationContainers(currentLocation);

            // If current location is Farm, check buildings
            if (currentLocation is Farm farm)
            {
                foreach (var building in farm.buildings)
                {
                    if (building.indoors.Value != null)
                    {
                        CheckLocationContainers(building.indoors.Value, building.buildingType.ToString());
                    }
                }
            }

            // If no chests found in current location, check other locations
            if (chestLocations.Count == 0)
            {
                foreach (GameLocation location in Game1.locations)
                {
                    if (location == currentLocation) continue;

                    if (!(location is Farm))
                    {
                        CheckLocationContainers(location);
                    }
                    else
                    {
                        Farm otherFarm = (Farm)location;
                        CheckLocationContainers(otherFarm);

                        foreach (var building in otherFarm.buildings)
                        {
                            if (building.indoors.Value != null)
                            {
                                CheckLocationContainers(building.indoors.Value, building.buildingType.ToString());
                            }
                        }
                    }
                }

                if (containerDetails.Count > 0)
                {
                    string locationList = string.Join("\n", containerDetails);
                    Game1.showGlobalMessage(ModEntry.I18n("message.found-in-locations", this.item.DisplayName, locationList));
                }
                else
                {
                    Game1.showGlobalMessage(ModEntry.I18n("message.not-found", this.item.DisplayName));
                }
                return;
            }

            // Draw paths to chests in current location
            Vector2 playerPos = new Vector2(
                (int)(Game1.player.Position.X / 64),
                (int)(Game1.player.Position.Y / 64)
            );

            Path_Finding.FindPaths(currentLocation, playerPos, chestLocations);

            if (ModEntry.paths.Count > 0)
            {
                string details = string.Join("\n", containerDetails);
                Game1.showGlobalMessage(ModEntry.I18n("message.found-in-containers", this.item.DisplayName, chestLocations.Count, details));
                ModEntry.SchedulePathAutoHide();
            }
            else
            {
                Game1.showGlobalMessage(ModEntry.I18n("message.cannot-find-path", this.item.DisplayName, currentLocation.Name));
            }
        }

        private void DrawQualityIcon(SpriteBatch b, int quality, Vector2 position)
        {
            if (quality <= 0) return;

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

        private string GetCabinOwnerName(StardewValley.Locations.Cabin cabin)
        {
            if (cabin == null) return "Cabin";

            string ownerName;
            if (cabin.owner != null && !string.IsNullOrEmpty(cabin.owner.Name))
            {
                ownerName = cabin.owner.Name;
            }
            else
            {
                ownerName = cabin.NameOrUniqueName.Split(' ')[0];
            }
            return $"{ownerName}'s Cabin";
        }
    }
}
