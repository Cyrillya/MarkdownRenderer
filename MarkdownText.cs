using System;
using System.Collections.Generic;
using MarkdownRenderer.Blocks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Graphics;

namespace MarkdownRenderer;

public class MarkdownText
{
    public int X;
    public int Y;

    public int MaxWidth = 400;
    public Color ShadowColor = Color.Black;
    public Color TextColor = Color.White;
    public float Scale = 1f;
    public float Spread = 2f;
    public List<BaseMarkdownBlock> Blocks = [];
    public bool PreparedToDraw = false;

    public delegate void DelegateRelativeLinkClicked(string url);
    public DelegateRelativeLinkClicked OnRelativeLinkClicked;

    // public Asset<DynamicSpriteFont> CustomHeadingRegularFont;
    // public Asset<DynamicSpriteFont> CustomHeadingBoldFont;
    // public Asset<DynamicSpriteFont> CustomHeadingItalicFont;
    // public Asset<DynamicSpriteFont> CustomParagraphRegularFont;
    // public Asset<DynamicSpriteFont> CustomParagraphBoldFont;
    // public Asset<DynamicSpriteFont> CustomParagraphItalicFont;

    public void PrepareToDraw(bool force = false)
    {
        if (PreparedToDraw && !force)
        {
            return;
        }

        foreach (var block in Blocks)
        {
            if (block.OriginalLines.Count == 0)
            {
                continue;
            }

            var inlineContainers = TextHelper.WordwrapString(block.OriginalLines, block);
            block.Lines = inlineContainers;
        }
        PreparedToDraw = true;
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 position)
    {
        PrepareToDraw();

        X = (int)position.X;
        Y = (int)position.Y;
        int y = 0;
        foreach (var block in Blocks)
        {
            block.Y = y;
            block.Draw(spriteBatch);
            Y += block.Height;
            Y += (int)(block.SpacingY * Scale);
        }
    }
}
