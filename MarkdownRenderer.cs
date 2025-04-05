using System;
using Markdig;
using MarkdownRenderer.Renderers;
using Terraria.ModLoader;

namespace MarkdownRenderer;

public class MarkdownRenderer : Mod
{
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