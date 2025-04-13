using System;
using System.Text;
using Humanizer;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MarkdownRenderer.Example;

public class MarkdownExample : ILoadable
{
    public string Text;

    public void Load(Mod mod)
    {
        Text = Encoding.UTF8.GetString(mod.GetFileBytes("Example/ExampleMarkdown.md"));

        Main.QueueMainThreadAction(() =>
        {
            On_Main.DrawFPS += (orig, self) =>
            {
                orig(self);

                if (Text is null) return;

                var text = MarkdownRenderer.ToMarkdownText(Text);
                text.Width = 700;
                text.TextSpread = 1f;
                text.Draw(Main.spriteBatch, new Vector2(20, 20));
            };
        });
    }

    public void Unload()
    {
    }
}
