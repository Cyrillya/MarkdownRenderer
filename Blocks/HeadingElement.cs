using System;
using MarkdownRenderer.BlockContainers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Graphics;
using Terraria.GameContent;
using Terraria.UI.Chat;

namespace MarkdownRenderer.Blocks;

public class HeadingElement(MarkdownText text, BaseBlockContainer parent) : BaseMarkdownBlock(text, parent)
{
    public override int SpacingY => 6;
    public int Level;

    public override float ZoomScale => Level switch
    {
        1 => 1f,
        2 => 0.75f,
        3 => 0.585f,
        4 => 0.5f,
        5 => 0.415f,
        6 => 0.335f,
        _ => 1f
    };

    public override Asset<DynamicSpriteFont> Font => MarkdownElement.HeadingFont;

    public override void Draw(SpriteBatch spriteBatch, Vector2 drawPosition)
    {
        var textPosition = drawPosition;

        Lines.Draw(spriteBatch, textPosition);

        if (Level >= 3) return;

        int x = (int)drawPosition.X;
        int y = (int)drawPosition.Y + Height - (int)(4 * MarkdownElement.Scale);
        var texture = TextureAssets.MagicPixel.Value;
        spriteBatch.Draw(texture, new Rectangle(x, y, Width, 1), Color.Gray);
    }
}
