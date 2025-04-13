using System;
using Markdig.Syntax.Inlines;
using MarkdownRenderer.Inlines;
using MarkdownRenderer.Renderers.Inlines.Modifiers;

namespace MarkdownRenderer.Renderers.Inlines;

public class EmphasisInlineRenderer : ObjectRenderer<EmphasisInline>
{
    protected override void Write(MarkdownTextRenderer renderer, EmphasisInline obj)
    {
        if (renderer == null) throw new ArgumentNullException(nameof(renderer));
        if (obj == null) throw new ArgumentNullException(nameof(obj));

        EmphasisModifier modifier;

        if (renderer.ModifiersStack.TryPeek(out var peakModifier) && peakModifier is EmphasisModifier)
            modifier = peakModifier as EmphasisModifier;
        else
            modifier = new EmphasisModifier();

        switch (obj.DelimiterChar)
        {
            case '*':
            case '_':
                if (obj.DelimiterCount == 2)
                    modifier.IsBold = true;
                else
                    modifier.IsItalic = true;
                break;
            case '~':
                if (obj.DelimiterCount == 2)
                    modifier.IsStrikethrough = true;
                else
                    modifier.IsSubscript = true;
                break;
            case '^':
                modifier.IsSuperscript = true;
                break;
            case '+':
                modifier.IsInserted = true;
                break;
            case '=':
                modifier.IsMarked = true;
                break;
        }

        renderer.ModifiersStack.Push(modifier);
        renderer.WriteContainerInline(obj);
        renderer.ModifiersStack.Pop();
    }
}
