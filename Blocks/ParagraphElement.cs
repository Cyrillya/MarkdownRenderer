using System;
using MarkdownRenderer.BlockContainers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.UI.Chat;

namespace MarkdownRenderer.Blocks;

public class ParagraphElement(MarkdownText text, BaseBlockContainer parent) : BaseMarkdownBlock(text, parent)
{
    public override void Draw(SpriteBatch spriteBatch, Vector2 drawPosition)
    {
        Lines.Draw(spriteBatch, drawPosition, out Height);
    }
}
