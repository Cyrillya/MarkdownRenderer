using System;
using System.Collections.Generic;
using System.Linq;
using MarkdownRenderer.Renderers.Inlines.Modifiers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.UI.Chat;

namespace MarkdownRenderer.Inlines;

public class LiteralInlineElement : BaseTextInline
{
    public const float ScriptScale = 0.5f;

    public int GlyphHeight => (int)(Font.Value.LineSpacing * 0.8f);

    public override float ZoomScale
    {
        get
        {
            EmphasisModifier emphasisModifier = null;
            if (Modifiers is not null)
            {
                emphasisModifier = Modifiers.FirstOrDefault(i => i is EmphasisModifier) as EmphasisModifier;
            }

            if (emphasisModifier != null && (emphasisModifier.IsSubscript || emphasisModifier.IsSuperscript))
            {
                return Parent.ZoomScale * ScriptScale;
            }
            else
            {
                return Parent.ZoomScale;
            }
        }
    }

    public override void Draw(SpriteBatch spriteBatch, int x, int y)
    {
        var drawPosition = new Vector2(x, y);
        var snippetsArray = TextSnippets.ToArray();
        var textColor = Parent.TextColor;
        float textScale = Parent.Scale;

        EmphasisModifier emphasisModifier = null;
        HyperlinkModifier hyperlinkModifier = null;
        if (Modifiers is not null)
        {
            emphasisModifier = Modifiers.FirstOrDefault(i => i is EmphasisModifier) as EmphasisModifier;
            hyperlinkModifier = Modifiers.FirstOrDefault(i => i is HyperlinkModifier) as HyperlinkModifier;
        }

        if (hyperlinkModifier is not null)
        {
            if (emphasisModifier is not null)
                emphasisModifier.IsInserted = true;
            else
                emphasisModifier = new EmphasisModifier() { IsInserted = true };

            textColor = Color.Cyan;
            var snippetsSize = TextHelper.GetSnippetsSize(TextSnippets, Font.Value).ToPoint();
            var snippetsRectangle = new Rectangle(x, y, snippetsSize.X, snippetsSize.Y);
            if (snippetsRectangle.Contains(Main.MouseScreen.ToPoint()) && Main.mouseLeft && Main.mouseLeftRelease)
            {
                if (hyperlinkModifier.IsAbsoluteLink)
                    Utils.OpenToURL(hyperlinkModifier.Url);
                else
                    Parent.Parent.OnRelativeLinkClicked(hyperlinkModifier.Url);
            }
        }

        if (emphasisModifier is not null)
        {
            if (emphasisModifier.IsSuperscript)
            {
                textScale *= ScriptScale;
                // Scaling in vanilla drawing will automatically adjust the text to fit in the underline, so we need to manually adjust the position
                drawPosition = new Vector2(x, y - GlyphHeight * 0.6f);
            }
            else if (emphasisModifier.IsSubscript)
            {
                textScale *= ScriptScale;
                // Scaling in vanilla drawing will automatically adjust the text to fit in the underline, so we need to manually adjust the position
                drawPosition = new Vector2(x, y - GlyphHeight * 0.2f);
            }
        }

        if (hyperlinkModifier != null)
        {
            var snippetsSize = (TextHelper.GetSnippetsSize(TextSnippets, Font.Value) * textScale).ToPoint();
            var snippetsRectangle = new Rectangle((int)drawPosition.X, (int)drawPosition.Y, snippetsSize.X, snippetsSize.Y);
            if (snippetsRectangle.Contains(Main.MouseScreen.ToPoint()))
            {
                textColor = textColor.MultiplyRGB(Color.LightGray);

                if (Main.mouseLeft && Main.mouseLeftRelease)
                {
                    if (hyperlinkModifier.IsAbsoluteLink)
                        Utils.OpenToURL(hyperlinkModifier.Url);
                    else
                        Parent.Parent.OnRelativeLinkClicked(hyperlinkModifier.Url);
                }
            }
        }

        DrawText(emphasisModifier, spriteBatch, Font.Value, snippetsArray, drawPosition, textColor, Parent.ShadowColor, Vector2.One * textScale, Parent.Spread);
    }

    private void TryDrawLine(EmphasisModifier modifier, SpriteBatch spriteBatch, Vector2 drawPosition, Color textColor, float textScale)
    {
        if (!modifier.IsStrikethrough && !modifier.IsInserted) return;

        Point snippetsSize = TextHelper.GetSnippetsSize(TextSnippets, Font.Value).ToPoint();
        int x = (int)drawPosition.X;
        int width = (int)(snippetsSize.X * textScale);
        var texture = TextureAssets.MagicPixel.Value;
        if (modifier.IsStrikethrough) // delete line
        {
            int y = (int)(drawPosition.Y + GlyphHeight / 2 * textScale);
            spriteBatch.Draw(texture, new Rectangle(x - 2, y, width + 2, 2), textColor);
        }
        if (modifier.IsInserted) // underline
        {
            int y = (int)(drawPosition.Y + GlyphHeight * textScale);
            spriteBatch.Draw(texture, new Rectangle(x - 2, y, width + 2, 2), textColor);
        }
    }

    private void TryDrawHighlight(EmphasisModifier modifier, SpriteBatch spriteBatch, Vector2 drawPosition, float textScale)
    {
        if (!modifier.IsMarked) return;

        Point snippetsSize = TextHelper.GetSnippetsSize(TextSnippets, Font.Value).ToPoint();
        int x = (int)drawPosition.X;
        int y = (int)drawPosition.Y;
        int width = (int)(snippetsSize.X * textScale);
        int height = (int)(GlyphHeight * textScale);
        var texture = TextureAssets.MagicPixel.Value;
        spriteBatch.Draw(texture, new Rectangle(x - 2, y - 2, width + 2, height + 2), Color.Yellow);
    }

    public void DrawText(EmphasisModifier modifier, SpriteBatch spriteBatch, DynamicSpriteFont font, TextSnippet[] snippets, Vector2 position, Color baseColor, Color shadowColor, Vector2 baseScale, float spread = 2f)
    {
        var drawPosition = position;
        float x = position.X;
        float y = position.Y;

        if (spread == 2f && baseScale.X <= 0.5f)
            spread = 1f;

        if (modifier != null)
        {
            TryDrawHighlight(modifier, spriteBatch, drawPosition, baseScale.X);

            if (modifier.IsBold)
            {
                for (int i = 0; i <= 1; i++)
                {
                    drawPosition = new Vector2(x + i, y);
                    TextHelper.DrawColorCodedStringShadow(spriteBatch, font, snippets, position, shadowColor, baseScale, spread);
                }

                for (int i = 0; i <= 1; i++)
                {
                    drawPosition = new Vector2(x + i, y);
                    TextHelper.DrawColorCodedString(spriteBatch, font, snippets, position, baseColor, baseScale);
                }
            }
            else
            {
                TextHelper.DrawColorCodedStringWithShadow(spriteBatch, font, snippets, drawPosition, baseColor, shadowColor, baseScale, spread);
            }

            TryDrawLine(modifier, spriteBatch, drawPosition, baseColor, baseScale.X);
            return;
        }

        TextHelper.DrawColorCodedStringWithShadow(spriteBatch, font, snippets, drawPosition, baseColor, shadowColor, baseScale, spread);
    }
}
