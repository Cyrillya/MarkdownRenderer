using System;
using System.Globalization;
using Markdig.Syntax;
using MarkdownRenderer.BlockContainers;
using MarkdownRenderer.Blocks;

namespace MarkdownRenderer.Renderers.Blocks;

public class ListRenderer : ObjectRenderer<ListBlock>
{
    protected override void Write(MarkdownTextRenderer renderer, ListBlock listBlock)
    {
        if (renderer == null) throw new ArgumentNullException(nameof(renderer));

        var list = new ListElement(renderer.Text, renderer.WorkingContainer);

        renderer.WorkingContainer.AddBlock(list);
        if (listBlock.IsOrdered)
        {
            list.MarkerStyle = ListElement.TextMarkerStyle.Decimal;

            if (listBlock.OrderedStart != null && (listBlock.DefaultOrderedStart != listBlock.OrderedStart))
            {
                list.StartIndex = int.Parse(listBlock.OrderedStart, NumberFormatInfo.InvariantInfo);
            }
        }
        else
        {
            list.MarkerStyle = ListElement.TextMarkerStyle.Disc;

            if (renderer.ContainersStack.TryPeek(out var parent) && parent is ListContainer listContainer)
            {
                list.Container.ListLevel = listContainer.ListLevel + 1;

                if (list.Container.ListLevel is 1)
                    list.MarkerStyle = ListElement.TextMarkerStyle.Circle;
                else
                    list.MarkerStyle = ListElement.TextMarkerStyle.Box;
            }
        }

        // renderer.ContainersStack.Push(list.Container);

        // foreach (var item in listBlock)
        // {
        //     var listItemBlock = (ListItemBlock)item;
        //     var listItem = new ListItem();
        //     renderer.Push(listItem);
        //     renderer.WriteChildren(listItemBlock);
        //     renderer.Pop();
        // }

        // renderer.ContainersStack.Pop();

        renderer.ContainersStack.Push(list.Container);
        renderer.WriteChildren(listBlock);
        renderer.ContainersStack.Pop();
    }
}
