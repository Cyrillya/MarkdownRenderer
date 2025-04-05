using System;
using System.Collections.Generic;
using System.Text;
using MarkdownRenderer.Blocks;
using MarkdownRenderer.Inlines;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using ReLogic.Text;
using Terraria.GameContent.UI.Chat;
using Terraria.UI.Chat;

namespace MarkdownRenderer;

public static class TextHelper
{
    public static TextSnippet[] GetTextSnippets(string text)
    {
        TextSnippet[] originalSnippets = [.. ChatManager.ParseMessage(text, Color.White)];
        ChatManager.ConvertNormalSnippets(originalSnippets);
        return originalSnippets;
    }

    public static List<InlineContainer> WordwrapString(List<BaseMarkdownInline> originalInlines, BaseMarkdownBlock block)
    {
        float maxWidth = block.Width / block.Scale;
        int longestWordLength = 19;

        int lineCount = 1; // 行数
        float workingLineLength = 0f; // 当前行长度
        var workingLineContainer = new InlineContainer(block);

        List<InlineContainer> finalContainers = [];
        Stack<BaseMarkdownInline> pendingInline = new();

        // push all inlines into the stack in reversed order (from count to 0)
        for (int i = originalInlines.Count - 1; i >= 0; i--)
        {
            pendingInline.Push(originalInlines[i]);
        }

        while (pendingInline.TryPeek(out var inline))
        {
            // pop the inline
            pendingInline.Pop();
            // calculate the remaining space for this line
            int remainingSpace = (int)(maxWidth - workingLineLength);

            if (inline is BaseTextInline textInline)
            {
                var font = textInline.Font.Value;

                // let TextSnippetWordwrapString decide wether to wrap
                var output = ChopTextSnippet(textInline, font, remainingSpace, out int lineBreakSnippetIndex, longestWordLength);

                // there's no line breaking
                if (lineBreakSnippetIndex is -1)
                {
                    workingLineContainer.AddInline(textInline);
                    workingLineLength += textInline.Width;
                }
                // line breaking
                else
                {
                    var snippetsBeforeWrap = output[..lineBreakSnippetIndex];
                    var snippetsAfterWrap = output[(lineBreakSnippetIndex + 1)..];
                    // add snippets to workingLineContainer
                    var newInline = textInline.Clone() as BaseTextInline;
                    newInline.TextSnippets.AddRange(snippetsBeforeWrap);
                    // add to working line
                    if (newInline.TextSnippets.Count > 0)
                    {
                        newInline.HasTrailingLineWrap = true;
                        workingLineContainer.AddInline(newInline);
                    }
                    // break the line
                    finalContainers.Add(workingLineContainer);
                    workingLineContainer = new InlineContainer(block); ;
                    lineCount++;
                    workingLineLength = 0;
                    // construct the new inline including the remaining snippets
                    if (snippetsAfterWrap.Count > 0)
                    {
                        var newerInline = textInline.Clone() as BaseTextInline;
                        newerInline.HasLeadingLineWrap = true;
                        newerInline.TextSnippets.AddRange(snippetsAfterWrap);
                        // push to the stack
                        pendingInline.Push(newerInline);
                    }
                }
            }
            // direct line break
            else if (inline is LineBreakInlineElement)
            {
                finalContainers.Add(workingLineContainer);
                workingLineContainer = new InlineContainer(block);
                lineCount++;
                workingLineLength = 0;
            }
            else
            {
                // if there is enough space for the whole inline, just add it to the current line
                // otherwise add to the next line
                // dont care if the block itself is big enough (blockWidth < inlineWidth who asked)
                if (inline.Width + workingLineLength > maxWidth)
                {
                    finalContainers.Add(workingLineContainer);
                    workingLineContainer = new InlineContainer(block);
                    lineCount++;
                    workingLineLength = 0;
                }

                workingLineContainer.AddInline(inline);
                workingLineLength += inline.Width;
            }
        }

        if (workingLineContainer.Inlines.Count > 0)
        {
            finalContainers.Add(workingLineContainer);
        }

        return finalContainers;
    }


    /// <summary>
    /// 将一组TextSnippet切成两行，理论上这些TextSnippets内不包括换行符
    /// </summary>
    public static List<TextSnippet> ChopTextSnippet(BaseTextInline textInline, DynamicSpriteFont font,
        int maxWidth, out int lineBreakSnippetIndex, int longestWordLength = 19)
    {
        lineBreakSnippetIndex = -1;
        float workingLineLength = 0f; // 当前行长度
        List<TextSnippet> finalSnippets = [new TextSnippet()];
        var originalSnippets = textInline.TextSnippets.ToArray();

        for (int j = 0; j < originalSnippets.Length; j++)
        {
            var snippet = originalSnippets[j];

            if (snippet is PlainTagHandler.PlainSnippet)
            {
                var cacheString = new StringBuilder(""); // 缓存字符串 - 准备输入的字符
                for (int i = 0; i < snippet.Text.Length; i++)
                {
                    GlyphMetrics characterMetrics = font.GetCharacterMetrics(snippet.Text[i]);
                    workingLineLength += (font.CharacterSpacing + characterMetrics.KernedWidth) * textInline.ZoomScale;

                    if (workingLineLength > maxWidth && !char.IsWhiteSpace(snippet.Text[i]))
                    {
                        // 如果第一个字符是空格，单词长度小于19（实际上是18因为第一个字符为空格），可以空格换行
                        bool canWrapWord = cacheString.Length > 1 && cacheString.Length < longestWordLength;

                        // 找不到空格，或者拆腻子，则强制换行
                        if (!canWrapWord || (i > 0 && CanBreakBetween(snippet.Text[i - 1], snippet.Text[i])))
                        {
                            // 加入缓存行
                            finalSnippets.Add(new PlainTagHandler.PlainSnippet(cacheString.ToString(), snippet.Color));
                            // 加入换行符
                            lineBreakSnippetIndex = finalSnippets.Count;
                            finalSnippets.Add(new PlainTagHandler.PlainSnippet("\n"));
                            // 加入该Snippet剩余文本
                            finalSnippets.Add(new PlainTagHandler.PlainSnippet(snippet.Text[i..], snippet.Color));
                            // 加入剩余Snippet
                            finalSnippets.AddRange(originalSnippets[(j + 1)..]);

                            return finalSnippets;
                        }
                        // 空格换行
                        else
                        {
                            // 由于下面那一段“将CJK字符与非CJK字符分割”可能会导致空格换行后的第一个字符不是空格，所以这里手动加一个空格
                            // 就不改下面的cacheString[1..]了
                            if (cacheString[0] != ' ')
                                cacheString.Append(" " + cacheString);

                            // 先换行，然后在下一行加入缓存的“单词”
                            lineBreakSnippetIndex = finalSnippets.Count;
                            finalSnippets.Add(new PlainTagHandler.PlainSnippet("\n"));
                            finalSnippets.Add(new PlainTagHandler.PlainSnippet(cacheString.ToString()[1..], snippet.Color));
                            // 加入该Snippet剩余文本
                            finalSnippets.Add(new PlainTagHandler.PlainSnippet(snippet.Text[i..], snippet.Color));
                            // 加入剩余Snippet
                            finalSnippets.AddRange(originalSnippets[(j + 1)..]);

                            return finalSnippets;
                        }
                    }

                    // 这么做可以分割单词，并且使自然分割单词（即不因换行过长强制分割的单词）第一个字符总是空格
                    // 或者是将CJK字符与非CJK字符分割
                    if (cacheString.Length > 0 && (char.IsWhiteSpace(snippet.Text[i]) ||
                                                        IsCjk(cacheString[^1]) != IsCjk(snippet.Text[i])))
                    {
                        finalSnippets.Add(new PlainTagHandler.PlainSnippet(cacheString.ToString(), snippet.Color));
                        cacheString.Clear();
                    }

                    // 原有换行则将当前行长度重置
                    // 注：Markdown文本内本身理论上不可能出现换行符
                    if (snippet.Text[i] is '\n')
                    {
                        workingLineLength = 0;
                    }

                    cacheString.Append(snippet.Text[i]);
                }

                finalSnippets.Add(new PlainTagHandler.PlainSnippet(cacheString.ToString(), snippet.Color));
            }
            else
            {
                float length = snippet.GetStringLength(font);
                workingLineLength += length * textInline.ZoomScale;
                // 超了 - 换行再添加，注意起始长度
                if (workingLineLength > maxWidth)
                {
                    // 加入换行符
                    lineBreakSnippetIndex = finalSnippets.Count;
                    finalSnippets.Add(new PlainTagHandler.PlainSnippet("\n"));
                    // 加入该特殊Snippet
                    finalSnippets.Add(snippet);
                    // 加入剩余Snippet
                    finalSnippets.AddRange(originalSnippets[(j + 1)..]);

                    return finalSnippets;
                }

                finalSnippets.Add(snippet);
            }
        }

        // 如果没有执行任何换行操作，从这里出
        return finalSnippets;
    }

    public static bool IsLineBreakSnippet(TextSnippet snippet)
    {
        return snippet.Text == "\n";
    }

    // https://unicode-table.com/cn/blocks/cjk-unified-ideographs/ 中日韩统一表意文字
    // https://unicode-table.com/cn/blocks/cjk-symbols-and-punctuation/ 中日韩符号和标点
    public static bool IsCjk(char a)
    {
        return (a >= 0x4E00 && a <= 0x9FFF) || (a >= 0x3000 && a <= 0x303F);
    }

    internal static bool CanBreakBetween(char previousChar, char nextChar)
    {
        if (IsCjk(previousChar) || IsCjk(nextChar))
            return true;

        return false;
    }

    public static Vector2 GetSnippetsSize(this List<TextSnippet> snippets, DynamicSpriteFont font) =>
        ChatManager.GetStringSize(font, [.. snippets], Vector2.One);

    public static void DrawColorCodedStringShadow(SpriteBatch spriteBatch, DynamicSpriteFont font, TextSnippet[] snippets, Vector2 position, Color baseColor, Vector2 baseScale, float spread = 2f, Vector2[] directions = null)
    {
        directions ??= [Vector2.UnitX, Vector2.UnitY, -Vector2.UnitX, -Vector2.UnitY];
        for (int i = 0; i < directions.Length; i++)
        {
            ChatManager.DrawColorCodedString(spriteBatch, font, snippets, position + directions[i] * spread, baseColor, 0f, Vector2.Zero, baseScale, out var _, -1, ignoreColors: true);
        }
    }

    public static Vector2 DrawColorCodedString(SpriteBatch spriteBatch, DynamicSpriteFont font, TextSnippet[] snippets, Vector2 position, Color baseColor, Vector2 baseScale) => ChatManager.DrawColorCodedString(spriteBatch, font, snippets, position, baseColor, 0f, Vector2.Zero, baseScale, out var _, -1, ignoreColors: baseColor != Color.White);

    public static Vector2 DrawColorCodedStringWithShadow(SpriteBatch spriteBatch, DynamicSpriteFont font, TextSnippet[] snippets, Vector2 position, Color baseColor, Color shadowColor, Vector2 baseScale, float spread = 2f, Vector2[] directions = null)
    {
        DrawColorCodedStringShadow(spriteBatch, font, snippets, position, shadowColor, baseScale, spread, directions);
        return DrawColorCodedString(spriteBatch, font, snippets, position, baseColor, baseScale);
    }
}
