using System;

namespace MarkdownRenderer.Renderers.Inlines.Modifiers;

public class HyperlinkModifier : IModifier
{
    public string Url;

    /// <summary>
    /// The title of the link, if any. Will be shown if the link is hovered.
    /// </summary>
    public string Title;

    public bool IsAbsoluteLink;
}
