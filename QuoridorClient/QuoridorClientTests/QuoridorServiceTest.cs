using System.Collections.Generic;
using NUnit.Framework;
using QuoridorClient.Enums;
using QuoridorClient.Model;
using QuoridorClient.ServiceConnector;
using QuoridorClient.Utils;

namespace QuoridorClientTests
{
    [TestFixture]
    public class QuoridorServiceTest
    {
        private QuoridorService _quoridorService;

        [TestFixtureSetUp]
        public void InitFixture()
        {
            _quoridorService = new QuoridorService();
        }

        [SetUp]
        public void Init()
        {
            _quoridorService.LogIn("Mateusz", "Bartczak");
        }

        [Test]
        public void LogInTest()
        {
            Assert.That(_quoridorService.IsLogged(), Is.True);
        }

        [Test]
        public void GetConnectedClientsTest()
        {
            _quoridorService.LogIn("Konrad", "Kotlarz");
            List<Player> players = _quoridorService.GetConnectedUsers();
            Assert.That(players.Count, Is.GreaterThan(1));
        }

        [Test]
        public void SendAndReceiveTest()
        {
            SendTestMessages();
            List<PlayerMessage> messages = _quoridorService.GetMessages();
            Assert.That(messages.Count, Is.EqualTo(2));
        }

        private void SendTestMessages()
        {
            _quoridorService.SendMessage(_quoridorService.MyId.ToString(), "Test");
            _quoridorService.SendMessage(_quoridorService.MyId.ToString(), "Test2");
        }

        [Test]
        public void SendAndReceiveGameEventTest()
        {
            SendGameInvitationEvent();
            List<PlayerMessage> messages = _quoridorService.GetMessages();
            Assert.That(messages.Count, Is.EqualTo(1));
            GameEvent ge = GameEventParser.Deserialize(messages[0].Message);
            Assert.That(ge.GetGameEventType(), Is.EqualTo(GameEventType.GameInvitation));
        }

        private void SendGameInvitationEvent()
        {
            GameInvitation gi = new GameInvitation() { InvitingPlayerId = _quoridorService.MyId };
            _quoridorService.SendGameEvent(_quoridorService.MyId.ToString(), gi);
        }
    }
}
