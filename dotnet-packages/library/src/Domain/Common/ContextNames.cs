namespace Library.Domain.Common
{
    public static class ContextNames
    {
        public static class Exchange
        {
            public const string Book = "Book";
            public const string Library = "Bookstore";
        }

        public static class Queue
        {
            public const string Library = "Library";
            public const string Book_Library = "Book_Library";
        }

        public static class Content
        {
            public const string UpdateBook = "UpdateBookEvent";
            public const string Reservation = "ReservationCommand";
            public const string ReservationExpired = "ReservationExpiredCommand";
            public const string ReservationEvent = "ReservationEventCommand";
        }
    }
}