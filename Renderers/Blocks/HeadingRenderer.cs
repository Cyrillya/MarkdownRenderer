using System;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using MarkdownRenderer.Blocks;
using MarkdownRenderer.Inlines;

namespace MarkdownRenderer.Renderers.Blocks;

public class HeadingRenderer : ObjectRenderer<HeadingBlock>
{
    protected override void Write(MarkdownTextRenderer renderer, HeadingBlock obj)
    {
        if (renderer == null) throw new ArgumentNullException(nameof(renderer));

        var paragraph = new HeadingElement(renderer.Text)
        {
            Level = obj.Level
        };
        renderer.Text.Blocks.Add(paragraph);
        renderer.WriteLeafInline(obj);
    }
}
