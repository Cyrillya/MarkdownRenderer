using System;
using System.Collections.Generic;
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
    public BaseMarkdownBlock Parent;
    public IModifier[] Modifiers;
    public virtual int Height => 0;
    public virtual int Width => 0;

    public object Clone()
    {
        var clone = Activator.CreateInstance(GetType()) as BaseTextInline;
        clone.Parent = Parent;
        clone.Modifiers = [.. Modifiers];
        return clone;
    }

    public abstract void Draw(SpriteBatch spriteBatch, int x, int y);
}

public abstract class BaseTextInline : BaseMarkdownInline
{
    public List<TextSnippet> TextSnippets = [];

    public override int Height => (int)(TextSnippets.GetSnippetsSize(Font.Value).Y * Parent.ZoomScale * ZoomScale);

    public override int Width => (int)(TextSnippets.GetSnippetsSize(Font.Value).X * Parent.ZoomScale * ZoomScale);

    public virtual float ZoomScale => 1f;

    public bool HasLeadingLineWrap;
    public bool HasTrailingLineWrap;

    public virtual Asset<DynamicSpriteFont> Font
    {
        get
        {
            if (Parent is HeadingElement)
                return FontAssets.DeathText;

            return FontAssets.MouseText;
        }
    }

    public BaseTextInline()
    {
    }

    public BaseTextInline(List<TextSnippet> text)
    {
        TextSnippets = text;
    }
}