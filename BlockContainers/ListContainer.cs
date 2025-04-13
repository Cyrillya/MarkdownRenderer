using System;
using System.Collections.Generic;
using MarkdownRenderer.Blocks;

namespace MarkdownRenderer.BlockContainers;

public class ListContainer : BaseBlockContainer
{
    public ListContainer(ListElement parent)
    {
        var parentContainer = parent.Parent;
        Width = parentContainer.Width - parent.Indent;
        ShadowColor = parentContainer.ShadowColor;
        TextColor = parentContainer.TextColor;
        LinkColor = parentContainer.LinkColor;
        HighlightColor = parentContainer.HighlightColor;
    }

    /// <summary>
    /// List level
    /// </summary>
    public int ListLevel;

    public List<BaseMarkdownBlock> Blocks = [];

    public override void AddBlock(BaseMarkdownBlock block)
    {
        Blocks.Add(block);
    }

    public override BaseMarkdownBlock GetWorkingBlock() => Blocks[^1];
}
