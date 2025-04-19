using System;
using System.Collections.Generic;
using System.Linq;
using Markdig.Syntax.Inlines;
using MarkdownRenderer;
using MarkdownRenderer.BlockContainers;
using MarkdownRenderer.Inlines;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Graphics;
using Terraria.GameContent;
using Terraria.UI.Chat;

namespace MarkdownRenderer.Blocks;

public abstract class BaseMarkdownBlock
{
    public BaseMarkdownBlock(MarkdownText text, BaseBlockContainer parent)
    {
        MarkdownElement = text;
        Parent = parent;
        Width = Parent.Width;
        ShadowColor = Parent.ShadowColor;
        TextColor = Parent.TextColor;
        LinkColor = Parent.LinkColor;
        HighlightColor = Parent.HighlightColor;
        Initialize();
    }

    public MarkdownText MarkdownElement;
    public BaseBlockContainer Parent;
    public int X;
    public int Y;
    public int Width;
    public int Height;
    public virtual int SpacingY => 8;

    public Vector2 Position
    {
        get
        {
            return new Vector2(X, Y);
        }
        set
        {
            X = (int)value.X;
            Y = (int)value.Y;
        }
    }

    public virtual Asset<DynamicSpriteFont> Font => MarkdownElement.ParagraphFont;

    public virtual Color ShadowColor { get; set; }
    public virtual Color TextColor { get; set; }
    public virtual Color LinkColor { get; set; }
    public virtual Color HighlightColor { get; set; }
    public virtual float Scale => MarkdownElement.Scale * ZoomScale;
    public virtual float ZoomScale => 1f;
    public Vector2 ScaleVector => new Vector2(Scale);

    public List<BaseMarkdownInline> OriginalLines = [];
    public List<InlineContainer> Lines = [];

    public abstract void Draw(SpriteBatch spriteBatch, Vector2 drawPosition);

    public void AddInline(BaseMarkdownInline inline)
    {
        OriginalLines.Add(inline);
    }

    public virtual void Prepare()
    {
        if (OriginalLines.Count == 0)
        {
            return;
        }

        var inlineContainers = TextHelper.WordwrapString(OriginalLines, this);
        Lines = inlineContainers;
        Height = Lines.GetTotalHeight();
    }

    public virtual void Initialize() { }
}