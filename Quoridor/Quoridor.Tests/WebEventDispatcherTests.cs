using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using Quoridor.DataContracts;
using Quoridor.Events;

namespace Quoridor.Tests
{
    [TestFixture]
    class WebEventDispatcherTests
    {
        private QuoridorEventDispatcher _sut;
        List<EventBase> _recivedEvents = new List<EventBase>();

        [TestFixtureSetUp]
        public void Setup()
        {
            _sut = QuoridorEventDispatcher.getInstance();
            _sut.Start();
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            _sut.Stop();
        }

        [TearDown]
        public void After()
        {
            _recivedEvents.Clear();
        }

        [Test]
        public void TestLogIn()
        {
            var finished = new ManualResetEvent(false);
            _sut.LoggedIn += (sender, @event) => {
                _recivedEvents.Add(@event);
                finished.Set();
            };
            _sut.LogIn("Mateusz", "Bartczak");
            finished.WaitOne();
            var lie = (LogInEvent) _recivedEvents[0];
            Assert.That(lie.IsLogged, Is.EqualTo(true));
        }

        [Test]
        public void TestGetLoggedIn()
        {
            var finished = new ManualResetEvent(false);
            _sut.LoggedUsersRefreshed += (sender, @event) => {
                _recivedEvents.Add(@event);
                finished.Set();
            };
            _sut.LogIn("Mateusz", "Bartczak");
            _sut.AddRequest(new EventRequest() { RequestType = EventRequestType.GetLoggedUsers });
            finished.WaitOne();
            var users = (LoggedUsersRefreshed) _recivedEvents[0];
            Assert.That(users.LoggedUsers, Is.Not.Null);
        }

        //[Test]
        //public void TestSendInvitation()
        //{
        //    try {
        //        QuoridorWebService ws = _sut.QuoridorWebService;
        //        ws.PlayerId = 2;
        //        Invitation invitation = ws.InvitePlayer("4").Result;
        //        Assert.That(invitation, Is.Not.Null);
        //    } catch (Exception ex) {
        //        Debug.WriteLine(ex.Message);
        //    }
        //}
    }
}
