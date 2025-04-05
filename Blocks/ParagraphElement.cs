using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.UI.Chat;

namespace MarkdownRenderer.Blocks;

public class ParagraphElement(MarkdownText parent) : BaseMarkdownBlock(parent)
{
    public override void Draw(SpriteBatch spriteBatch)
    {
        Lines.Draw(spriteBatch, DrawPosition, out Height);
    }
}
