using System;
using System.Collections.Generic;
using System.Linq;
using Markdig.Syntax.Inlines;
using MarkdownRenderer.Inlines;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Graphics;
using Terraria.GameContent;
using Terraria.UI.Chat;

namespace MarkdownRenderer.Blocks;

public abstract class BaseMarkdownBlock(MarkdownText parent)
{
    public MarkdownText Parent = parent;
    public int X;
    public int Y;
    public int Width => Parent.MaxWidth;
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

    public Vector2 DrawPosition
    {
        get
        {
            return new Vector2(X + Parent.X, Y + Parent.Y);
        }
    }

    public virtual Color ShadowColor => Parent.ShadowColor;
    public virtual Color TextColor => Parent.TextColor;
    public virtual float Scale => Parent.Scale * ZoomScale;
    public virtual float ZoomScale => 1f;
    public Vector2 ScaleVector => new Vector2(Scale);
    public virtual float Spread => Parent.Spread;

    public List<BaseMarkdownInline> OriginalLines = [];
    public List<InlineContainer> Lines = [];

    public abstract void Draw(SpriteBatch spriteBatch);

    public void AddInline(BaseMarkdownInline inline)
    {
        OriginalLines.Add(inline);
    }
}
