using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using IContainer = QuestPDF.Infrastructure.IContainer;
using Colors = QuestPDF.Helpers.Colors;

namespace SmartGloveRebuild2.ViewModels.Admin
{
    static class SimpleExtension
    {
        private static IContainer Cell(this IContainer container, bool dark)
        {
            return container
                .Border(1)
                .Background(dark ? Colors.Grey.Lighten2 : Colors.White)
                .Padding(2);
        }        
        
        private static IContainer OriginalCell(this IContainer container)
        {
            return container
                .Padding(3);
        }

        // displays only text label
        public static void LabelCell(this IContainer container, string text) => container.Cell(true).Text(text).Medium();
        public static void EmptyCell(this IContainer container, string text) => container.OriginalCell().AlignMiddle().Text(text).FontSize(10);
        public static void BoldEmptyCell(this IContainer container, string text) => container.OriginalCell().AlignMiddle().Text(text).FontSize(10).ExtraBold();
        public static void HeaderEmptyCell(this IContainer container, string text) => container.OriginalCell().PaddingVertical(10).AlignMiddle().Text(text).FontSize(18);
        public static void CheckBox(this IContainer container) => container.OriginalCell().Border(1).Padding(0).Text("");
        public static void EmptyValueCell(this IContainer container, string text) => container.OriginalCell();

        // allows to inject any type of content, e.g. image
        public static IContainer ValueCell(this IContainer container) => container.Cell(false);

    }
}
