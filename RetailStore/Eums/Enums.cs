namespace RetailStore.Eums
{
    public static class Enums
    {
        /// <summary>
        /// Enum for Order Size
        /// </summary>
        public enum OrderSize : short 
        {
            /// <summary>
            /// Order size more than 10 items
            /// </summary>
            Large = 0,

            /// <summary>
            /// Order size between 5 and 10 items
            /// </summary>
            Medium = 1,

            /// <summary>
            /// Order size between 2 and 5 items
            /// </summary>
            Small = 2,

            /// <summary>
            /// Order size with 1 item
            /// </summary>
            SingleItem = 3
        }
    }
}
