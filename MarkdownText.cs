using System;
using System.Collections.Generic;
using MarkdownRenderer.BlockContainers;
using MarkdownRenderer.Blocks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Graphics;
using Terraria;
using Terraria.GameContent;

namespace MarkdownRenderer;

public class MarkdownText : BaseBlockContainer
{
    public MarkdownText()
    {
        Width = 400;
    }

    // Colors of Blocks, will affect inlines in those blocks
    public Color QuoteTextColor = DefaultColors.QuoteTextGray;
    public Color QuoteShadowColor = DefaultColors.QuoteShadowDark;
    public Color QuoteIndicatorColor = DefaultColors.QuoteIndicatorDarkGray;

    public float Scale = 1f;
    public float TextSpread = 2f;

    public List<BaseMarkdownBlock> Blocks = [];
    public bool Interactable = true;
    public bool PreparedToDraw = false;

    public delegate void DelegateRelativeLinkClicked(string url);
    public DelegateRelativeLinkClicked OnRelativeLinkClicked;

    public Asset<DynamicSpriteFont> HeadingFont = FontAssets.DeathText;
    public Asset<DynamicSpriteFont> ParagraphFont = FontAssets.MouseText;

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
            block.Width = Width;
            block.Prepare();
        }
        PreparedToDraw = true;
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 position)
    {
        PrepareToDraw();

        int y = 0;
        foreach (var block in Blocks)
        {
            block.Y = y;
            block.Draw(spriteBatch, position + block.Position);
            y += block.Height;
            y += (int)(block.SpacingY * Scale);
        }
    }

    public override void AddBlock(BaseMarkdownBlock block)
    {
        Blocks.Add(block);
    }

    public override BaseMarkdownBlock GetWorkingBlock() => Blocks[^1];
}
