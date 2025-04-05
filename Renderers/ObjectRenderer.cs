using System;
using Markdig.Renderers;
using Markdig.Syntax;

namespace MarkdownRenderer.Renderers;

public abstract class ObjectRenderer<TObject> : MarkdownObjectRenderer<MarkdownTextRenderer, TObject>
        where TObject : MarkdownObject
{
}
