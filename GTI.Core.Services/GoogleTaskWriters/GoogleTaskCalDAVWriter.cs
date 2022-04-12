using GTI.Core.Contracts;
using GTI.Core.Contracts.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace GTI.Core.Services
{
    public class GoogleTaskCalDAVWriter : IGoogleTaskWriter
    {
        private readonly IGoogleTaskToICalSerializer _taskSerializer;
        private readonly GoogleTaskCalDAVWriteOptions _options;

        public GoogleTaskCalDAVWriter(IGoogleTaskToICalSerializer taskSerializer, GoogleTaskCalDAVWriteOptions options)
        {
            this._taskSerializer = taskSerializer;
            this._options = options;
        }

        public void Write(List<GoogleTaskList> listsToWrite)
        {
            foreach (GoogleTaskList list in listsToWrite)
            {
                tryImportToCalDAV(list);
            }
        }

        private void tryImportToCalDAV(GoogleTaskList list)
        {
            // try to check if there already is a calendar with the name and create one, if not
            ensureReadyCalendar(list.Title);

            // if we have a calendar ready for import, upload tasks one by one
            uploadTasks(list);
        }

        private void ensureReadyCalendar(string title)
        {
            string listTitle = getUriTitleFromDisplayTitle(title);

            HttpStatusCode calendarStatus = sendDAVRequest(listTitle, "HEAD", null);
            if (calendarStatus == HttpStatusCode.OK)
            {
                Console.WriteLine($"Found calendar endpoint '{listTitle}'.");
                return;
            }

            if (calendarStatus == HttpStatusCode.NotFound)
            {
                Console.WriteLine($"Creating calendar endpoint '{listTitle}'..");

                string createBody = $@"<x0:mkcol xmlns:x0=""DAV:"">
                                        <x0:set>
                                            <x0:prop>
                                                <x0:resourcetype>
                                                    <x0:collection/>
                                                    <x1:calendar xmlns:x1=""urn:ietf:params:xml:ns:caldav""/>
                                                </x0:resourcetype>
                                                <x0:displayname>{title}</x0:displayname>
                                                <x1:supported-calendar-component-set xmlns:x1=""urn:ietf:params:xml:ns:caldav"">
                                                    <x1:comp name=""VTODO"" />
                                                </x1:supported-calendar-component-set>
                                            </x0:prop>
                                        </x0:set>
                                    </x0:mkcol>";

                sendDAVRequest(listTitle, "MKCOL", createBody);

                Console.WriteLine($"Done.");
                return;
            }

            throw new WebException("Unexpected HTTP Status from server: " + calendarStatus);
        }

        private void uploadTasks(GoogleTaskList list)
        {
            Console.WriteLine($"Uploading list '{list.Title}' with {list.Items.Count} items..");

            Dictionary<string, string> items = _taskSerializer.SerializeMany(list);

            // calendar items have to be uploaded one at a time
            int counter = 0;
            foreach (KeyValuePair<string, string> item in items)
            {
                Console.Write($"{counter}.. ");

                string listTitle = getUriTitleFromDisplayTitle(list.Title);
                sendDAVRequest(listTitle, "PUT", item.Value);

                counter++;
            }

            Console.WriteLine();
            Console.WriteLine($"Finished list '{list.Title}'.");
            Console.WriteLine();
        }

        private HttpStatusCode sendDAVRequest(string requestPath, string method, string body)
        {
            string authBasic = getBasicAuthString(_options.AuthUser, _options.AuthPass);

            var createRequest = (HttpWebRequest)WebRequest.Create(string.Join('/', _options.BaseUri.ToString().Trim('/'), requestPath));
            createRequest.Method = method;
            createRequest.Headers["Authorization"] = authBasic;

            if (createRequest.Method == "MKCOL")
                createRequest.ContentType = "application/xml";

            if (body != null)
            {
                using var streamWriter = new StreamWriter(createRequest.GetRequestStream());
                streamWriter.Write(body);
            }

            HttpWebResponse createResponse;
            try
            {
                createResponse = (HttpWebResponse)createRequest.GetResponse();
            }
            catch (WebException e)
            {
                createResponse = (HttpWebResponse)e.Response;
            }

            return createResponse.StatusCode;
        }

        private string getBasicAuthString(string user, string pass)
        {
            return "Basic " + Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(user + ":" + pass));
        }

        private static string getUriTitleFromDisplayTitle(string title)
        {
            return title.ToLower().Replace(' ', '-');
        }
    }
}