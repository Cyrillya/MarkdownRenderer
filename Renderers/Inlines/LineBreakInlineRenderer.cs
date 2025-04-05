using System;
using Markdig.Syntax.Inlines;
using MarkdownRenderer.Inlines;

namespace MarkdownRenderer.Renderers.Inlines;

public class LineBreakInlineRenderer : ObjectRenderer<LineBreakInline>
{
    protected override void Write(MarkdownTextRenderer renderer, LineBreakInline obj)
    {
        if (renderer == null) throw new ArgumentNullException(nameof(renderer));
        if (obj == null) throw new ArgumentNullException(nameof(obj));

        var inline = new LineBreakInlineElement()
        {
            Parent = renderer.Text.Blocks[^1],
        };
        renderer.Text.Blocks[^1].AddInline(inline);
    }
}
