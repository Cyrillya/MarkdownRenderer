using System;

namespace MarkdownRenderer.Renderers.Inlines.Modifiers;

public class HyperlinkModifier : IModifier
{
    public string Url;
    public bool IsAbsoluteLink;
}
