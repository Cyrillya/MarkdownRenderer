using System;
using System.Collections.Generic;
using MarkdownRenderer.Blocks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace MarkdownRenderer.BlockContainers;

public class QuoteContainer : BaseBlockContainer
{
    public QuoteContainer(QuoteElement parent)
    {
        var parentContainer = parent.Parent;
        Width = parentContainer.Width - parent.Indent;
        ShadowColor = parentContainer.ShadowColor;
        TextColor = parent.MarkdownElement.QuoteTextColor;
        LinkColor = parentContainer.LinkColor;
        HighlightColor = parentContainer.HighlightColor;
    }

    public List<BaseMarkdownBlock> Blocks = [];

    public override void AddBlock(BaseMarkdownBlock block)
    {
        Blocks.Add(block);
    }

    public override BaseMarkdownBlock GetWorkingBlock() => Blocks[^1];
}
