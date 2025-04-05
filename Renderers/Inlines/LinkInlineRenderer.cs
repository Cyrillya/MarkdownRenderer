using System;
using Markdig.Syntax.Inlines;
using MarkdownRenderer.Inlines;
using MarkdownRenderer.Renderers.Inlines.Modifiers;

namespace MarkdownRenderer.Renderers.Inlines;

public class LinkInlineRenderer : ObjectRenderer<LinkInline>
{
    protected override void Write(MarkdownTextRenderer renderer, LinkInline obj)
    {
        if (renderer == null) throw new ArgumentNullException(nameof(renderer));
        if (obj == null) throw new ArgumentNullException(nameof(obj));

        var url = obj.GetDynamicUrl != null ? obj.GetDynamicUrl() ?? obj.Url : obj.Url;

        if (!Uri.IsWellFormedUriString(url, UriKind.RelativeOrAbsolute))
        {
            url = "#";
        }

        if (obj.IsImage)
        {
        }
        else
        {
            var modifier = new HyperlinkModifier
            {
                Url = obj.Url,
                // determine if the link is absolute or relative
                IsAbsoluteLink = Uri.IsWellFormedUriString(url, UriKind.Absolute)
            };

            renderer.ModifiersStack.Push(modifier);

            var inline = new LiteralInlineElement()
            {
                Parent = renderer.Text.Blocks[^1],
                TextSnippets = [.. TextHelper.GetTextSnippets(!string.IsNullOrEmpty(obj.Title) ? obj.Title : obj.Url)],
                Modifiers = [.. renderer.ModifiersStack]
            };
            renderer.Text.Blocks[^1].AddInline(inline);

            renderer.ModifiersStack.Pop();
        }
    }
}
