using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;

namespace Find_Item.Utilities // Using the namespace provided by the user
{
    public static class ItemDrawHelper
    {
        public static void DrawItemIcon(SpriteBatch b, Item item, Vector2 position, float scale)
        {
            if (item == null)
                return;

            if (item is Tool tool)
            {
                if (tool is StardewValley.Tools.MeleeWeapon weapon)
                {
                    // For weapons, use the weapons texture
                    Rectangle toolSourceRect = Game1.getSourceRectForStandardTileSheet(
                        Tool.weaponsTexture,
                        weapon.IndexOfMenuItemView,
                        16,
                        16);
                    b.Draw(
                        Tool.weaponsTexture,
                        position,
                        toolSourceRect,
                        Color.White,
                        0f,
                        Vector2.Zero,
                        scale,
                        SpriteEffects.None,
                        1f // layerDepth
                    );
                }
                else
                {
                    // For regular tools (hoe, pickaxe, etc), use the tools texture
                    Rectangle sourceRect = Game1.getSourceRectForStandardTileSheet(
                        Game1.toolSpriteSheet,
                        tool.IndexOfMenuItemView,
                        16,
                        16);
                    b.Draw(
                        Game1.toolSpriteSheet,
                        position,
                        sourceRect,
                        Color.White,
                        0f,
                        Vector2.Zero,
                        scale,
                        SpriteEffects.None,
                        1f // layerDepth
                    );
                }
            }
            else if (item is StardewValley.Object obj)
            {
                // Use the item's own drawInMenu method for proper rendering.
                // This handles jar products (jelly, pickles, wine, etc.) correctly.
                // The 'scale' parameter for obj.drawInMenu is relative to the standard 64x64px draw size (Game1.tileSize).
                // Our function's 'scale' parameter is relative to the 16x16 sprite sheet size.
                // So, we need to convert: drawInMenu_scale = passed_scale / (64px / 16px).
                // Game1.pixelZoom is typically this factor (e.g., 4).
                float drawInMenuScale = scale / Game1.pixelZoom;
                obj.drawInMenu(b, position, drawInMenuScale, 1f, 1f, StackDrawType.Draw, Color.White, true);
            }
            else
            {
                // Fallback for other item types (e.g., Rings, Hats, Boots which are not StardewValley.Object).
                // Most Item subclasses implement drawInMenu, which is a more robust way to draw them.
                // The scale conversion is the same as for StardewValley.Object.
                float drawInMenuScale = scale / Game1.pixelZoom;
                item.drawInMenu(b, position, drawInMenuScale, 1f, 1f, StackDrawType.Draw, Color.White, true);
            }
        }

        public static float GetIconWidth(float scale)
        {
            // Assumes the icon base width is 16px, scaled by the 'scale' parameter.
            // Adds 10 pixels for padding.
            return 16 * scale + 10;
        }
    }
}