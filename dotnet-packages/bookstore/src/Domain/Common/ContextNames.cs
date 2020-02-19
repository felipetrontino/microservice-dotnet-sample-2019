namespace Bookstore.Domain.Common
{
    public static class ContextNames
    {
        public static class Exchange
        {
            public const string Bookstore = "Bookstore";
            public const string Book = "Book";
        }

        public static class Queue
        {
            public const string Library = "Library";
            public const string Bookstore = "Bookstore";
            public const string Book_Bookstore = "Book_Bookstore";
        }

        public static class Content
        {
            public const string UpdateBook = "UpdateBookEvent";
            public const string Purchase = "PurchaseCommand";
            public const string ShippingEvent = "ShippingEventCommand";
        }
    }
}