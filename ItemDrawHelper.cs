using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;

namespace Find_Item.Utilities
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
                        tool.IndexOfMenuItemView,
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
                        1f
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
                        1f
                    );
                }
            }
            else
            {
                // Handle regular items
                Rectangle sourceRect = Game1.getSourceRectForStandardTileSheet(
                    Game1.objectSpriteSheet,
                    item.ParentSheetIndex,
                    16,
                    16);

                b.Draw(
                    Game1.objectSpriteSheet,
                    position,
                    sourceRect,
                    Color.White,
                    0f,
                    Vector2.Zero,
                    scale,
                    SpriteEffects.None,
                    1f
                );
            }
        }

        public static float GetIconWidth(float scale)
        {
            return 16 * scale + 10; // Add 10 pixels padding
        }
    }
}