using System;
using System.Linq;
using MarkdownRenderer.BlockContainers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Graphics;
using Terraria;
using Terraria.GameContent;

namespace MarkdownRenderer.Blocks;

public class ListElement : BaseMarkdownBlock
{
    public enum TextMarkerStyle
    {
        /// <summary>
        /// A solid disc circle.
        /// </summary>
        Disc = 1,

        /// <summary>
        /// A hollow disc circle.
        /// </summary>
        Circle = 2,

        /// <summary>
        /// A solid square box.
        /// </summary>
        Box = 3,

        /// <summary>
        /// A decimal starting with the number one. For example, 1, 2, and 3. The decimal
        /// value is automatically incremented for each item added to the list.
        /// </summary>
        Decimal = 4,
    }

    public int Indent = 30;
    public int StartIndex = 1;
    public ListContainer Container;
    public TextMarkerStyle MarkerStyle;

    public ListElement(MarkdownText text, BaseBlockContainer parent) : base(text, parent)
    {
        Container = new ListContainer(this);
    }

    public override void Draw(SpriteBatch spriteBatch, Vector2 drawPosition)
    {
        for (int i = 0; i < Container.Blocks.Count; i++)
        {
            // draw the block
            var block = Container.Blocks[i];
            var finalBlockPosition = drawPosition + block.Position + new Vector2(Indent, 0);
            block.Draw(spriteBatch, finalBlockPosition);

            // dont draw marker for sub-lists
            if (block is ListElement) continue;

            // draw marker
            finalBlockPosition.Y += block.Font.Value.GetLineHeight() * block.ZoomScale * Scale;
            finalBlockPosition.X -= 10f;

            // the font is not affected by the block
            var font = MarkdownElement.ParagraphFont;

            switch (MarkerStyle)
            {
                case TextMarkerStyle.Decimal:
                    var text = TextHelper.GetTextSnippets($"{StartIndex + i}.");
                    var textSize = TextHelper.GetSnippetsSize(text, font.Value);
                    var textPosition = finalBlockPosition - new Vector2(textSize.X, font.Value.GetLineHeight()) * Scale;
                    TextHelper.DrawColorCodedStringWithShadow(spriteBatch, font.Value, text, textPosition, TextColor, ShadowColor, ScaleVector, MarkdownElement.TextSpread);
                    break;
                case TextMarkerStyle.Disc:
                    DrawTextureMarker(MarkdownRenderer.Disc, finalBlockPosition, font);
                    break;
                case TextMarkerStyle.Circle:
                    DrawTextureMarker(MarkdownRenderer.Circle, finalBlockPosition, font);
                    break;
                case TextMarkerStyle.Box:
                    DrawTextureMarker(MarkdownRenderer.Pixel, finalBlockPosition, font);
                    break;
            }
        }
    }

    private void DrawTextureMarker(Asset<Texture2D> tex, Vector2 position, Asset<DynamicSpriteFont> font)
    {
        float radius = 4 * Scale;
        var discCenter = position;
        float glyphHeight = font.Value.GetLineHeight();
        discCenter.Y -= glyphHeight * 0.5f * Scale;
        discCenter.X -= radius;
        var drawRectangle = new Rectangle((int)(discCenter.X - radius), (int)(discCenter.Y - radius), (int)(radius * 2), (int)(radius * 2));
        var markerColor = TextColor.MultiplyRGB(TextColor * 0.94f);
        Main.spriteBatch.Draw(tex.Value, drawRectangle, markerColor);
    }

    public override void Prepare()
    {
        Container.Width = Parent.Width - Indent;

        Height = 0;
        foreach (var block in Container.Blocks)
        {
            block.Width = Container.Width;
            block.Prepare();
            block.Y = Height;
            Height += block.Height;
        }
    }
}
