using System;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using MarkdownRenderer.Blocks;
using MarkdownRenderer.Inlines;
using Microsoft.Xna.Framework;
using Terraria.UI.Chat;

namespace MarkdownRenderer.Renderers.Blocks;

public class ParagraphRenderer : ObjectRenderer<ParagraphBlock>
{
    protected override void Write(MarkdownTextRenderer renderer, ParagraphBlock obj)
    {
        if (renderer == null) throw new ArgumentNullException(nameof(renderer));

        var paragraph = new ParagraphElement(renderer.Text, renderer.WorkingContainer);
        renderer.WorkingContainer.AddBlock(paragraph);
        renderer.WriteLeafInline(obj);
    }
}
