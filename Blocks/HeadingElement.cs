using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Graphics;
using Terraria.GameContent;
using Terraria.UI.Chat;

namespace MarkdownRenderer.Blocks;

public class HeadingElement(MarkdownText parent) : BaseMarkdownBlock(parent)
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

    public override void Draw(SpriteBatch spriteBatch)
    {
        var drawPosition = DrawPosition;
        var textPosition = drawPosition;
        // vanilla drawing is strange, have to add offset myself
        if (Level != 1)
        {
            textPosition.Y -= 8 * Parent.Scale;
        }
        else
        {
            textPosition.Y += 4 * Parent.Scale;
        }

        Lines.Draw(spriteBatch, textPosition, out Height);

        if (Level >= 3) return;

        int x = (int)drawPosition.X;
        int y = (int)drawPosition.Y + Height;
        var texture = TextureAssets.MagicPixel.Value;
        spriteBatch.Draw(texture, new Rectangle(x, y, Width, 1), Color.Gray);
    }
}
