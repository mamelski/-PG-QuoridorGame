namespace SendObjectTest
{
    using System.ComponentModel;
    using System.IO;
    using System.Net;

    using Newtonsoft.Json;

    /// <summary>
    /// The test.
    /// </summary>
    public class Test
    {
        /// <summary>
        /// The board event to send.
        /// </summary>
        private readonly BoardEvent boardEventToSend = new BoardEvent { BoardEventType = BoardEventType.PlayerMoved, Move = MoveDirection.Forward, SenderId = 10 };

        /// <summary>
        /// The send board event.
        /// </summary>
        public void SendBoardEvent()
        {
            // TODO change address
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://quoridor.cloudapp.net/Quoridor.svc/SendBoardEvent");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                var postData = JsonConvert.SerializeObject(this.boardEventToSend);

                streamWriter.Write(postData);
            }

            httpWebRequest.GetResponse();
        }

        /// <summary>
        /// The get board event.
        /// </summary>
        public void GetBoardEvent()
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://quoridor.cloudapp.net/Quoridor.svc/GetBoardEvent");
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            using (var reader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var dataString = reader.ReadToEnd();
                var x = JsonConvert.DeserializeObject<BoardEvent>(dataString);
                int a = 0;
            }
        }
    }
}
