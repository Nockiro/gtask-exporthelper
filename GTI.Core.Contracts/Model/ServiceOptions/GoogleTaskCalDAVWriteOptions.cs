using System;

namespace GTI.Core.Contracts.Model
{
    public class GoogleTaskCalDAVWriteOptions
    {
        /// <summary>
        /// Base URI of the CalDAV endpoint that is used.
        /// </summary>
        /// <example>https://yournextcloud.tld/remote.php/dav/calendars/youruser</example>
        public Uri BaseUri { get; set; }

        /// <summary>
        /// User for the CALDAV endpoint.
        /// </summary>
        public string AuthUser { get; set; }

        /// <summary>
        /// Password for the CALDAV endpoint.
        /// </summary>
        /// <remarks>This can also be an app password.</remarks>
        public string AuthPass { get; set; }
    }
}