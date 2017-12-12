using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Quoridor.Events
{
    public abstract class WebEventDispatcher
    {
        private readonly Task _mainWebServiceConnectorTask;
        protected volatile bool Run;

        private ConcurrentQueue<EventRequest> EventRequests;

        public WebEventDispatcher()
        {
            _mainWebServiceConnectorTask = new Task(Do);
            EventRequests = new ConcurrentQueue<EventRequest>();
        }

        public void Start()
        {
            Run = true;
            _mainWebServiceConnectorTask.Start();
        }

        public void Stop()
        {
            Run = false;
        }

        public bool IsRunning()
        {
            return Run;
        }

        private void Do()
        {
            while (Run) {
                EventRequest eventRequest = null;
                EventRequests.TryDequeue(out eventRequest);
                if (eventRequest != null) {
                    DispatchEventRequest(eventRequest);
                }
            }
        }

        public void AddRequest(EventRequest eventRequest)
        {
            EventRequests.Enqueue(eventRequest);
        }

        protected abstract void DispatchEventRequest(EventRequest eventRequest);
    }
}
