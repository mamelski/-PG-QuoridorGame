using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace QuoridorClient
{
    public class RestServiceConnector
    {
        public string ServiceHostUrl { get; set; }

        public async Task<string> CallMethodAsync(String methodName, params String[] methodParams)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(ServiceHostUrl);
            if (!ServiceHostUrl.EndsWith("/")) {
                sb.Append("/" + methodName);
            } else {
                sb.Append(methodName);
            }
            foreach (string methodParam in methodParams) {
                sb.Append("/" + methodParam);
            }
            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, sb.ToString());
            HttpResponseMessage response = await client.SendAsync(request);
            return await response.Content.ReadAsStringAsync();
        }

        public string CallMethod(String methodName, params String[] methodParams)
        {
            return AsyncHelpers.RunSync(() => CallMethodAsync(methodName, methodParams));
        }
    }
}
