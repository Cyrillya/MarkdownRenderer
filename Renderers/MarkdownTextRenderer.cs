using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Markdig.Renderers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using MarkdownRenderer.BlockContainers;
using MarkdownRenderer.Renderers.Blocks;
using MarkdownRenderer.Renderers.Inlines;
using MarkdownRenderer.Renderers.Inlines.Modifiers;
using Terraria.WorldBuilding;

namespace MarkdownRenderer.Renderers;

public class MarkdownTextRenderer : RendererBase
{
    public MarkdownTextRenderer()
    {
    }

    public MarkdownTextRenderer(MarkdownText text)
    {
        LoadText(text);
    }

    public virtual void LoadText(MarkdownText text)
    {
        Text = text ?? throw new ArgumentNullException(nameof(text));
        ContainersStack.Push(Text);
        LoadRenderers();
    }

    public MarkdownText Text { get; protected set; }

    public BaseBlockContainer WorkingContainer => ContainersStack.Peek();

    public Stack<BaseBlockContainer> ContainersStack = [];

    public Stack<IModifier> ModifiersStack = [];

    /// <inheritdoc/>
    public override object Render(MarkdownObject markdownObject)
    {
        Write(markdownObject);
        return WorkingContainer;
    }

    protected virtual void LoadRenderers()
    {
        // Default block renderers
        // ObjectRenderers.Add(new CodeBlockRenderer());
        ObjectRenderers.Add(new ListRenderer());
        ObjectRenderers.Add(new HeadingRenderer());
        ObjectRenderers.Add(new ParagraphRenderer());
        ObjectRenderers.Add(new QuoteRenderer());
        // ObjectRenderers.Add(new ThematicBreakRenderer());

        // Default inline renderers
        ObjectRenderers.Add(new AutolinkInlineRenderer());
        // ObjectRenderers.Add(new CodeInlineRenderer());
        // ObjectRenderers.Add(new DelimiterInlineRenderer());
        ObjectRenderers.Add(new EmphasisInlineRenderer());
        // ObjectRenderers.Add(new HtmlEntityInlineRenderer());
        ObjectRenderers.Add(new LineBreakInlineRenderer());
        ObjectRenderers.Add(new LinkInlineRenderer());
        ObjectRenderers.Add(new LiteralInlineRenderer());

        // Extension renderers
        // ObjectRenderers.Add(new TableRenderer());
        // ObjectRenderers.Add(new TaskListRenderer());
    }

    /// <summary>
    /// Writes the inlines of a leaf inline.
    /// </summary>
    /// <param name="leafBlock">The leaf block.</param>
    /// <returns>This instance</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteLeafInline(LeafBlock leafBlock)
    {
        if (leafBlock == null) throw new ArgumentNullException(nameof(leafBlock));
        var inline = leafBlock.Inline as Inline;
        while (inline != null)
        {
            Write(inline);
            inline = inline.NextSibling;
        }
    }

    /// <summary>
    /// Writes the inlines of a leaf inline.
    /// </summary>
    /// <param name="container">The leaf block.</param>
    /// <returns>This instance</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void WriteContainerInline(ContainerInline container)
    {
        if (container == null) throw new ArgumentNullException(nameof(container));
        var inline = container.FirstChild;
        while (inline != null)
        {
            Write(inline);
            inline = inline.NextSibling;
        }
    }
}
