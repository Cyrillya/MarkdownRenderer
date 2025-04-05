using System;
using Markdig.Syntax.Inlines;
using MarkdownRenderer.Inlines;
using MarkdownRenderer.Renderers.Inlines.Modifiers;

namespace MarkdownRenderer.Renderers.Inlines;

public class AutolinkInlineRenderer : ObjectRenderer<AutolinkInline>
{
    protected override void Write(MarkdownTextRenderer renderer, AutolinkInline obj)
    {
        if (renderer == null) throw new ArgumentNullException(nameof(renderer));
        if (obj == null) throw new ArgumentNullException(nameof(obj));

        var url = obj.Url;
        if (obj.IsEmail)
        {
            url = "mailto:" + url;
        }

        if (!Uri.IsWellFormedUriString(url, UriKind.RelativeOrAbsolute))
        {
            url = "#";
        }

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
            TextSnippets = [.. TextHelper.GetTextSnippets(modifier.Url)],
            Modifiers = [.. renderer.ModifiersStack]
        };
        renderer.Text.Blocks[^1].AddInline(inline);

        renderer.ModifiersStack.Pop();
    }
}
