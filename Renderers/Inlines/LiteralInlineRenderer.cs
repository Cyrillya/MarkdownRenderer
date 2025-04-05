using System;
using System.Linq;
using Markdig.Syntax.Inlines;
using MarkdownRenderer.Inlines;
using Microsoft.Xna.Framework;
using Terraria.UI.Chat;

namespace MarkdownRenderer.Renderers.Inlines;

public class LiteralInlineRenderer : ObjectRenderer<LiteralInline>
{
    protected override void Write(MarkdownTextRenderer renderer, LiteralInline obj)
    {
        if (renderer == null) throw new ArgumentNullException(nameof(renderer));
        if (obj == null) throw new ArgumentNullException(nameof(obj));

        if (obj.Content.IsEmpty)
            return;

        var inline = new LiteralInlineElement()
        {
            Parent = renderer.Text.Blocks[^1],
            TextSnippets = [.. TextHelper.GetTextSnippets(obj.Content.ToString())],
            Modifiers = [.. renderer.ModifiersStack]
        };
        renderer.Text.Blocks[^1].AddInline(inline);
    }
}
