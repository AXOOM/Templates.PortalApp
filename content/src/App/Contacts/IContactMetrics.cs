using System;

namespace MyVendorName.MyAppName.Contacts
{
    /// <summary>
    /// Used to report metrics relating to contacts.
    /// </summary>
    public interface IContactMetrics
    {
        /// <summary>
        /// Starts a timer for the duration of a persistence write operation. Place inside a using statement.
        /// </summary>
        IDisposable Write();

        /// <summary>
        /// Counts one poke of a contact.
        /// </summary>
        void Poke();
    }
}
