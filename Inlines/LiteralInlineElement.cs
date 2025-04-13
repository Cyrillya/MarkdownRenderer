using System;
using System.Collections.Generic;
using System.Linq;
using MarkdownRenderer.Renderers.Inlines.Modifiers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader.UI;
using Terraria.UI.Chat;

namespace MarkdownRenderer.Inlines;

public class LiteralInlineElement : BaseTextInline
{
    public readonly float ScriptScale = 0.5f;

    public int GlyphHeight => Font.Value.GetLineHeight();

    public override float ZoomScale
    {
        get
        {
            EmphasisModifier emphasisModifier = GetEmphasisModifier();

            if (emphasisModifier != null && (emphasisModifier.IsSubscript || emphasisModifier.IsSuperscript))
            {
                return ScriptScale;
            }
            else
            {
                return 1f;
            }
        }
    }

    // To support scripts, height should not be affected by native zoom scale
    public override int Height => (int)(TextSnippets.GetSnippetsSize(Font.Value).Y * ParentBlock.ZoomScale);

    public override void Draw(SpriteBatch spriteBatch, int x, int y)
    {
        var drawPosition = new Vector2(x, y);
        var snippetsArray = TextSnippets.ToArray();
        var textColor = ParentBlock.TextColor;
        float textScale = ParentBlock.Scale;

        EmphasisModifier emphasisModifier = GetEmphasisModifier();
        HyperlinkModifier hyperlinkModifier = null;
        if (Modifiers is not null)
        {
            hyperlinkModifier = Modifiers.FirstOrDefault(i => i is HyperlinkModifier) as HyperlinkModifier;
        }

        if (hyperlinkModifier is not null)
        {
            if (emphasisModifier is not null)
                emphasisModifier.IsInserted = true;
            else
                emphasisModifier = new EmphasisModifier() { IsInserted = true };

            textColor = ParentBlock.LinkColor;
        }

        if (emphasisModifier is not null)
        {
            if (emphasisModifier.IsSuperscript)
            {
                textScale *= ScriptScale;
                // Scaling in vanilla drawing will automatically adjust the text to fit in the underline, so we need to manually adjust the position
                drawPosition = new Vector2(x, y);
            }
            else if (emphasisModifier.IsSubscript)
            {
                textScale *= ScriptScale;
                // Scaling in vanilla drawing will automatically adjust the text to fit in the underline, so we need to manually adjust the position
                drawPosition = new Vector2(x, y + GlyphHeight * 0.5f);
            }
        }

        if (hyperlinkModifier != null)
        {
            var snippetsSize = (TextHelper.GetSnippetsSize(TextSnippets, Font.Value) * textScale).ToPoint();
            var snippetsRectangle = new Rectangle((int)drawPosition.X, (int)drawPosition.Y, snippetsSize.X, snippetsSize.Y);
            if (ParentMarkdown.Interactable && snippetsRectangle.Contains(Main.MouseScreen.ToPoint()))
            {
                // indicate that this can be clicked
                Main.instance.MouseText($"[i:{ItemID.TitanGlove}]", 0, 0, Main.mouseX + 16, Main.mouseY - 10);
                // hover feedback
                textColor = textColor.MultiplyRGB(Color.LightGray);
                // show title if there's any
                if (!string.IsNullOrEmpty(hyperlinkModifier.Title))
                    UICommon.TooltipMouseText(hyperlinkModifier.Title);

                if (Main.mouseLeft && Main.mouseLeftRelease)
                {
                    Main.mouseLeftRelease = false;
                    if (hyperlinkModifier.IsAbsoluteLink)
                        Utils.OpenToURL(hyperlinkModifier.Url);
                    else
                        ParentMarkdown.OnRelativeLinkClicked(hyperlinkModifier.Url);
                }
            }
        }

        DrawText(emphasisModifier, spriteBatch, Font.Value, snippetsArray, drawPosition, textColor, ParentBlock.ShadowColor, Vector2.One * textScale, ParentBlock.MarkdownElement.TextSpread);
    }

    private void TryDrawLine(EmphasisModifier modifier, SpriteBatch spriteBatch, Vector2 drawPosition, Color textColor, float textScale)
    {
        if (!modifier.IsStrikethrough && !modifier.IsInserted) return;

        Point snippetsSize = TextHelper.GetSnippetsSize(TextSnippets, Font.Value).ToPoint();
        int x = (int)drawPosition.X;
        int width = (int)(snippetsSize.X * textScale);
        var texture = MarkdownRenderer.Pixel;
        if (modifier.IsStrikethrough) // delete line
        {
            int y = (int)(drawPosition.Y + GlyphHeight / 2 * textScale);
            spriteBatch.Draw(texture.Value, new Rectangle(x - 2, y, width + 2, 2), textColor);
        }
        if (modifier.IsInserted) // underline
        {
            int y = (int)(drawPosition.Y + GlyphHeight * textScale);
            spriteBatch.Draw(texture.Value, new Rectangle(x - 2, y, width + 2, 2), textColor);
        }
    }

    private void TryDrawHighlight(EmphasisModifier modifier, SpriteBatch spriteBatch, Vector2 drawPosition, float textScale)
    {
        if (!modifier.IsMarked) return;

        Point snippetsSize = TextHelper.GetSnippetsSize(TextSnippets, Font.Value).ToPoint();
        int x = (int)drawPosition.X;
        int y = (int)drawPosition.Y;
        int width = (int)(snippetsSize.X * textScale);
        int height = (int)((GlyphHeight + 2) * textScale);
        var texture = MarkdownRenderer.Pixel;
        spriteBatch.Draw(texture.Value, new Rectangle(x - 2, y - 2, width + 4, height + 4), ParentBlock.HighlightColor);
    }

    public void DrawText(EmphasisModifier modifier, SpriteBatch spriteBatch, DynamicSpriteFont font, TextSnippet[] snippets, Vector2 position, Color baseColor, Color shadowColor, Vector2 baseScale, float spread = 2f)
    {
        var drawPosition = position;
        float x = position.X;
        float y = position.Y;

        if (baseScale.X <= 0.5f && spread > 1.1f)
            spread = (float)Math.Round(spread * baseScale.X);

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

    /// <summary>
    /// Gets the marged emphasis modifier from the modifiers array.
    /// </summary>
    /// <returns>An emphasis modifier containing all emphasis features</returns>
    private EmphasisModifier GetEmphasisModifier()
    {
        var emphasisModifier = default(EmphasisModifier);

        if (Modifiers is null) return emphasisModifier;

        foreach (var modifier in Modifiers)
        {
            if (modifier is EmphasisModifier emphasis)
            {
                if (emphasisModifier is null)
                    emphasisModifier = emphasis;
                else
                    emphasisModifier.Merge(emphasis);
            }
        }

        return emphasisModifier;
    }
}
