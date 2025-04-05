using System;
using System.Collections;
using System.Collections.Generic;
using MarkdownRenderer.Blocks;
using MarkdownRenderer.Inlines;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MarkdownRenderer;

public static class Extensions
{
    public static void Draw(this IList<InlineContainer> lines, SpriteBatch spriteBatch, Vector2 position, out int totalHeight)
    {
        totalHeight = 0;
        int y = (int)position.Y;
        foreach (var line in lines)
        {
            line.Draw(spriteBatch, (int)position.X, y);
            y += line.Height;
        }

        totalHeight = y - (int)position.Y;
    }
}
