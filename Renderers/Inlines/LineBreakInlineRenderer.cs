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

        var workingBlock = renderer.WorkingContainer.GetWorkingBlock();
        var inline = new LineBreakInlineElement()
        {
            ParentBlock = workingBlock,
        };
        workingBlock.AddInline(inline);
    }
}
