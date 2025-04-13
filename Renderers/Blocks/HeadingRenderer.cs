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

        var paragraph = new HeadingElement(renderer.Text, renderer.WorkingContainer)
        {
            Level = obj.Level
        };
        renderer.WorkingContainer.AddBlock(paragraph);
        renderer.WriteLeafInline(obj);
    }
}
