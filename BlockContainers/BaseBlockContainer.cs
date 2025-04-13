using System;
using MarkdownRenderer.Blocks;
using Microsoft.Xna.Framework;
using Terraria;

namespace MarkdownRenderer.BlockContainers;

public abstract class BaseBlockContainer
{
    public int Width;

    // Colors of Inlines
    public Color ShadowColor = DefaultColors.ShadowDark;
    public Color TextColor = DefaultColors.TextWhite;
    public Color LinkColor = DefaultColors.LinkCyan;
    public Color HighlightColor = DefaultColors.HighlightYellow;

    public abstract void AddBlock(BaseMarkdownBlock block);

    public abstract BaseMarkdownBlock GetWorkingBlock();
}
