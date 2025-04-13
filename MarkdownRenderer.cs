using System;
using Markdig;
using MarkdownRenderer.Renderers;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;

namespace MarkdownRenderer;

public class MarkdownRenderer : Mod
{
	public static Asset<Texture2D> Pixel;
	public static Asset<Texture2D> Disc;
	public static Asset<Texture2D> Circle;

	public override void Load()
	{
		Pixel = ModAsset.Pixel_Async;
		Disc = ModAsset.Disc_Async;
		Circle = ModAsset.Circle_Async;
	}

	public static MarkdownText ToMarkdownText(string markdown, MarkdownPipeline? pipeline = null, MarkdownTextRenderer? renderer = null)
	{
		if (markdown == null) throw new ArgumentNullException(nameof(markdown));
		pipeline ??= new MarkdownPipelineBuilder()
			.UseEmphasisExtras()
			.UseAutoLinks()
			.Build();

		var result = new MarkdownText();

		if (renderer == null)
			renderer = new MarkdownTextRenderer(result);
		else
			renderer.LoadText(result);

		pipeline.Setup(renderer);

		var document = Markdown.Parse(markdown, pipeline);
		renderer.Render(document);

		return result;
	}
}