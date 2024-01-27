using GTI.Core.Contracts;
using GTI.Core.Contracts.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace GTI.Core.Services
{
    public class GoogleTaskCalDAVWriter : IGoogleTaskWriter
    {
        private readonly IGoogleTaskToICalSerializer _taskSerializer;
        private readonly GoogleTaskCalDAVWriteOptions _options;
        private static readonly HttpClient httpClient = new();

        public GoogleTaskCalDAVWriter(IGoogleTaskToICalSerializer taskSerializer, GoogleTaskCalDAVWriteOptions options)
        {
            this._taskSerializer = taskSerializer;
            this._options = options;

            initHttpClient();
        }

        private void initHttpClient()
        {
            httpClient.BaseAddress = _options.BaseUri;

            if (!httpClient.BaseAddress.AbsoluteUri.EndsWith('/'))
                httpClient.BaseAddress = new Uri(_options.BaseUri.ToString() + '/');

            string authBasic = getBasicAuthString(_options.AuthUser, _options.AuthPass);
            httpClient.DefaultRequestHeaders.Add("Authorization", authBasic);
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

                HttpStatusCode createStatus = sendDAVRequest(listTitle, "MKCOL", createBody);

                if (createStatus == HttpStatusCode.NotFound)
                {
                    throw new Exception(
                        $"Something went wrong, the server reports it couldn't find the given CalDAV endpoint (HTTP {createStatus})");
                }

                Console.WriteLine($"Done.");
                return;
            }

            throw new WebException("Unexpected HTTP Status from server: " + calendarStatus);
        }

        private void uploadTasks(GoogleTaskList list)
        {
            Console.WriteLine($"Uploading list '{list.Title}' with {list.Items.Count} items..");

            Dictionary<string, string> items = _taskSerializer.SerializeMany(list);
            string listTitle = getUriTitleFromDisplayTitle(list.Title);

            // calendar items have to be uploaded one at a time
            int counter = 0;
            foreach (KeyValuePair<string, string> item in items)
            {
                Console.Write($"{counter}.. ");

                HttpStatusCode davResponseStatusCode = sendDAVRequest($"{listTitle}/{item.Key}.ics", "PUT", item.Value);
                if (davResponseStatusCode != HttpStatusCode.Created)
                {
                    Console.Write($"{Enum.GetName(typeof(HttpStatusCode), davResponseStatusCode)}! ");
                }

                counter++;
            }

            Console.WriteLine();
            Console.WriteLine($"Finished list '{list.Title}'.");
            Console.WriteLine();
        }

        private HttpStatusCode sendDAVRequest(string requestPath, string method, string body)
        {
            HttpRequestMessage httpRequestMessage = new(new HttpMethod(method), requestPath);

            if (body != null)
            {
                httpRequestMessage.Content = new StringContent(body);

                switch (method)
                {
                    case "MKCOL":
                        httpRequestMessage.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/xml");
                        break;
                    case "PUT":
                        httpRequestMessage.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("text/calendar");
                        break;
                }
            }

            HttpResponseMessage response = httpClient.Send(httpRequestMessage);
            return response.StatusCode;
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