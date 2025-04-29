namespace Exadel.ReportHub.Pdf;

public static class Constants
{
    public static class Text
    {
        public static class Label
        {
            public const string Invoice = "Invoice";
            public const string InvoiceNumber = "Invoice Number";
            public const string IssueDate = "Issue Date";
            public const string DueDate = "Due Date";
            public const string ClientName = "Client Name";
            public const string CustomerName = "Customer Name";
            public const string ClientBankAccountNumber = "Client Bank Account Number";
            public const string Total = "Total";
        }

        public static class Header
        {
            public const string Name = "Name";
            public const string Description = "Description";
            public const string Price = "Price";
        }

        public static class TextStyle
        {
            public const string Font = "Liberation Sans";
            public const int FontSizeTitle = 18;
            public const int FontSize = 12;
        }
    }

    public static class Size
    {
        public static class Page
        {
            public const int Width = 595;
            public const int Height = 842;
            public const int XCenter = Width / 2;
        }

        public static class Table
        {
            public const int NameWidth = 150;
            public const int DescriptionWidth = 250;
            public const int PriceWidth = 115;
        }
    }

    public static class MarginInfo
    {
        public static class Page
        {
            public const int Left = 40;
            public const int Bottom = 40;
            public const int Right = 40;
            public const int Top = 40;
        }

        public static class InvoiceTable
        {
            public const int Left = 5;
            public const int Bottom = 5;
            public const int Right = 5;
            public const int Top = 5;
        }
    }

    public static class BorderInfo
    {
        public static class IvoiceTable
        {
            public const float Border = 0.5f;
            public const float CellBorder = 0.5f;
        }
    }
}
