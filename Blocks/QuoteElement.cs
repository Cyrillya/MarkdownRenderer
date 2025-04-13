using System;
using System.Collections.Generic;
using MarkdownRenderer.BlockContainers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MarkdownRenderer.Blocks;

public class QuoteElement : BaseMarkdownBlock
{
    public int Indent = 20;
    public QuoteContainer Container;

    public QuoteElement(MarkdownText text, BaseBlockContainer parent) : base(text, parent)
    {
        Container = new QuoteContainer(this);
    }

    public override void Draw(SpriteBatch spriteBatch, Vector2 drawPosition)
    {
        Height = 0;
        for (int i = 0; i < Container.Blocks.Count; i++)
        {
            var block = Container.Blocks[i];
            block.Y = Height;
            block.Draw(spriteBatch, drawPosition + block.Position + new Vector2(Indent, 0));
            Height += block.Height;
        }

        // indent indicator
        int x = (int)drawPosition.X;
        int y = (int)drawPosition.Y;
        var texture = MarkdownRenderer.Pixel;
        // make it a bit shorter to suit the text perfectly
        var indicatorHeight = Height;
        if (Container.Blocks.Count > 0)
            indicatorHeight -= (int)(Container.Blocks[^1].Font.Value.GetLineGap() * Scale);
        spriteBatch.Draw(texture.Value, new Rectangle(x, y, 5, indicatorHeight), MarkdownElement.QuoteIndicatorColor);
    }

    public override void Prepare()
    {
        Container.Width = Parent.Width - Indent;
        foreach (var block in Container.Blocks)
        {
            block.Width = Container.Width;
            block.Prepare();
        }
    }
}
