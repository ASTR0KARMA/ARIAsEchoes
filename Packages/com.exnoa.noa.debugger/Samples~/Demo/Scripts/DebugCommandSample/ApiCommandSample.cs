#if NOA_DEBUGGER
using System;
using NoaDebugger;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using UnityEngine.Networking;

namespace NoaDebuggerDemo
{
    public class ApiCommandSample : DebugCategoryBase
    {
        static readonly Uri BaseUrl = new("https://www.noadebugger.dmmgames.com");
        const string COMMUNICATION_OPERATIONS_GROUP_NAME = "Communication Operations";
        static readonly Random RandomGenerator = new(Environment.TickCount);

        [CommandGroup(COMMUNICATION_OPERATIONS_GROUP_NAME), DisplayName("Get Request")]
        public IEnumerator GetRequest()
        {
            var request = UnityWebRequest.Get(BaseUrl);

            var requestHeaders = new Dictionary<string, string>
            {
                {"Content-Type", "application/json"}
            };

            foreach (KeyValuePair<string, string> header in requestHeaders)
            {
                request.SetRequestHeader(header.Key, header.Value);
            }

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            yield return request.SendWebRequest();

            stopwatch.Stop();

            var apiLog = new ApiLog
            {
                Url = request.uri,
                Method = request.method,
                StatusCode = (int)request.responseCode,
                ContentSize = (long)request.downloadedBytes,
                ResponseTimeMilliSeconds = stopwatch.ElapsedMilliseconds,
                RequestHeaders = requestHeaders,
                RequestBody = { },
                ResponseHeaders = request.GetResponseHeaders(),
                ResponseBody = request.downloadHandler.text,
                PrettyPrint = true
            };

            ApiLogger.Log(apiLog);
        }

        [CommandGroup(COMMUNICATION_OPERATIONS_GROUP_NAME), DisplayName("Dummy Post Request")]
        public void DummyPostRequest()
        {
            DummyPostRequest(200);
        }

        [CommandGroup(COMMUNICATION_OPERATIONS_GROUP_NAME), DisplayName("Dummy Error Request")]
        public void DummyErrorRequest()
        {
            DummyPostRequest(500);
        }

        void DummyPostRequest(int statusCode)
        {
            var requestHeaders = new Dictionary<string, string>
            {
                {"Content-Type", "application/json"}
            };

            const string requestBody = "{\"Request1\":\"1\",\"Request2\":\"2\",\"Request3\":\"3\"}";

            var responseHeaders = new Dictionary<string, string>
            {
                {"Date", $"{DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)} GMT"},
                {"Content-Type", "application/json"},
                {"Transfer-Encoding", "chunked"},
                {"Connection", "keep-alive"}
            };

            const string responseBody =
                "{\"Response1\":\"1\",\"Response2\":\"Very long string, Very long string, Very long string, Very long string, Very long string, Very long string, Very long string, Very long string, Very long string, Very long string, Very long string, Very long string, Very long string, Very long string, Very long string, Very long string\",\"Response3\":{\"Values1\":[1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20],\"Values2\":[\"foo\",\"bar\",\"baz\",\"qux\",\"quux\",\"corge\",\"grault\",\"garply\",\"waldo\",\"fred\",\"plugh\",\"xyzzy\",\"thud\"]}}";

            var randomTime = RandomGenerator.Next(10, 201);

            var log = new ApiLog
            {
                Url = new Uri(BaseUrl, "/api/post-test"),
                Method = "POST",
                StatusCode = statusCode,
                ContentSize = responseBody.Length,
                ResponseTimeMilliSeconds = randomTime,
                RequestHeaders = requestHeaders,
                ResponseHeaders = responseHeaders,
                PrettyPrint = true
            };

            log.RequestBody = requestBody;
            log.ResponseBody = responseBody;
            ApiLogger.Log(log);
        }
    }
}
#endif
