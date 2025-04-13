using System;
using Microsoft.Xna.Framework;
using Terraria;

namespace MarkdownRenderer;

public static class DefaultColors
{
    public static Color QuoteTextGray = new(145, 152, 161);
    public static Color QuoteShadowDark = QuoteTextGray.MultiplyRGB(QuoteTextGray * 0.1f);
    public static Color QuoteIndicatorDarkGray = new(61, 68, 77);
    public static Color ShadowDark = Color.White.MultiplyRGB(Color.White * 0.1f);
    public static Color TextWhite = Color.White;
    public static Color LinkCyan = Color.Cyan;
    public static Color HighlightYellow = Color.Yellow;
}
