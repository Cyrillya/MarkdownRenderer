using System;
using Markdig.Syntax;
using MarkdownRenderer.Blocks;

namespace MarkdownRenderer.Renderers.Blocks;

public class QuoteRenderer : ObjectRenderer<QuoteBlock>
{
    protected override void Write(MarkdownTextRenderer renderer, QuoteBlock obj)
    {
        if (renderer == null) throw new ArgumentNullException(nameof(renderer));

        var quote = new QuoteElement(renderer.Text, renderer.WorkingContainer);
        renderer.WorkingContainer.AddBlock(quote);

        renderer.ContainersStack.Push(quote.Container);
        renderer.WriteChildren(obj);
        renderer.ContainersStack.Pop();
    }
}
