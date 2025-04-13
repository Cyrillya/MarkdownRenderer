using System;
using System.Collections.Generic;
using MarkdownRenderer.BlockContainers;
using MarkdownRenderer.Blocks;
using MarkdownRenderer.Renderers.Inlines.Modifiers;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Graphics;
using Terraria.GameContent;
using Terraria.UI.Chat;

namespace MarkdownRenderer.Inlines;

public abstract class BaseMarkdownInline : ICloneable
{
    public BaseMarkdownBlock ParentBlock;
    public BaseBlockContainer ParentContainer => ParentBlock.Parent;
    public MarkdownText ParentMarkdown => ParentBlock.MarkdownElement;
    public IModifier[] Modifiers;
    public virtual int Height => 0;
    public virtual int Width => 0;

    public object Clone()
    {
        var clone = Activator.CreateInstance(GetType()) as BaseTextInline;
        clone.ParentBlock = ParentBlock;
        clone.Modifiers = [.. Modifiers];
        return clone;
    }

    public abstract void Draw(SpriteBatch spriteBatch, int x, int y);
}

public abstract class BaseTextInline : BaseMarkdownInline
{
    public List<TextSnippet> TextSnippets = [];

    public override int Height => (int)(TextSnippets.GetSnippetsSize(Font.Value).Y * ParentBlock.ZoomScale * ZoomScale);

    public override int Width => (int)(TextSnippets.GetSnippetsSize(Font.Value).X * ParentBlock.ZoomScale * ZoomScale);

    public virtual float ZoomScale => 1f;

    public bool HasLeadingLineWrap;
    public bool HasTrailingLineWrap;

    public virtual Asset<DynamicSpriteFont> Font => ParentBlock.Font;

    public BaseTextInline()
    {
    }

    public BaseTextInline(List<TextSnippet> text)
    {
        TextSnippets = text;
    }
}