using System.Diagnostics;

namespace Quoridor
{
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;

    using Newtonsoft.Json;
    using QuoridorClient.Utils;

    public class ServiceWrapper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceWrapper"/> class.
        /// </summary>
        /// <param name="serviceUrl"></param>
        public ServiceWrapper(string serviceUrl)
        {
            this.ServiceUrl = serviceUrl;
        }

        /// <summary>
        /// Gets the service host url.
        /// </summary>
        public string ServiceUrl { get; }

        /// <summary>
        /// The call get method.
        /// </summary>
        /// <param name="methodName">
        /// The method name.
        /// </param>
        /// <param name="methodParams">
        /// The method params.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<T> CallGetMethod<T>(string methodName, params string[] methodParams)
        {
            var url = this.PrepareUrl(methodName, methodParams);
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            Debug.WriteLine("HTTP Request: " + url);

            var response = await httpWebRequest.GetResponseAsync();

            T receivedObject;
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                receivedObject = JsonParser.Deserialize<T>(reader.ReadToEnd());
            }

            return receivedObject;
        }

        /// <summary>
        /// The call post method.
        /// </summary>
        /// <param name="methodName">
        /// The method Name.
        /// </param>
        /// <param name="objectToPost">
        /// The object to post.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<bool> CallPostMethod<T>(string methodName, T objectToPost)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(this.ServiceUrl + "/" + methodName);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(await httpWebRequest.GetRequestStreamAsync()))
            {
                var postData = JsonConvert.SerializeObject(objectToPost);
                streamWriter.Write(postData);
            }

            var response = await httpWebRequest.GetResponseAsync();

            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                var result = JsonConvert.DeserializeObject<bool>(streamReader.ReadToEnd());
                return result;
            }
        }

        /// <summary>
        /// The prepare url.
        /// </summary>
        /// <param name="methodName">
        /// The method name.
        /// </param>
        /// <param name="methodParams">
        /// The method params.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private string PrepareUrl(string methodName, IEnumerable<string> methodParams)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append(this.ServiceUrl);

            if (!this.ServiceUrl.EndsWith("/"))
            {
                stringBuilder.Append("/" + methodName);
            }
            else
            {
                stringBuilder.Append(methodName);
            }

            foreach (var methodParam in methodParams)
            {
                stringBuilder.Append("/" + methodParam);
            }

            return stringBuilder.ToString();
        }
    }
}
