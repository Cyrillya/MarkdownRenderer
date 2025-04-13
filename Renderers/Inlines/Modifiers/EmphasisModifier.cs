using System;

namespace MarkdownRenderer.Renderers.Inlines.Modifiers;

public class EmphasisModifier : IModifier
{
    public bool IsBold;
    public bool IsItalic;
    public bool IsStrikethrough;
    public bool IsInserted; // Underline
    public bool IsMarked; // Highlight
    public bool IsSubscript; // 下标
    public bool IsSuperscript; // 上标

    public void Merge(EmphasisModifier other)
    {
        IsBold |= other.IsBold;
        IsItalic |= other.IsItalic;
        IsStrikethrough |= other.IsStrikethrough;
        IsInserted |= other.IsInserted;
        IsMarked |= other.IsMarked;
        IsSubscript |= other.IsSubscript;
        IsSuperscript |= other.IsSuperscript;
    }
}
