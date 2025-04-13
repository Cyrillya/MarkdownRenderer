using System;
using System.Collections.Generic;
using MarkdownRenderer.Blocks;
using Microsoft.Xna.Framework.Graphics;

namespace MarkdownRenderer.Inlines;

public class InlineContainer(BaseMarkdownBlock parent)
{
    public BaseMarkdownBlock Parent = parent;

    public List<BaseMarkdownInline> Inlines = [];

    public int Height;

    public void AddInline(BaseMarkdownInline inline)
    {
        int inlineHeight = inline.Height;
        Height = Math.Max(Height, inlineHeight);
        inline.ParentBlock = Parent;
        Inlines.Add(inline);
    }

    public void Draw(SpriteBatch spriteBatch, int x, int y)
    {
        foreach (var inline in Inlines)
        {
            int bottom = y + Height;
            int inlineY = bottom - inline.Height;
            inline.Draw(spriteBatch, x, inlineY);
            x += inline.Width;
        }
    }

    public void Clear()
    {
        Inlines.Clear();
        Height = 0;
    }
}
