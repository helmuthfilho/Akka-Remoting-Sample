using Akka.Actor;
using Akka.DI.Core;
using Akka.Routing;
using Client1Application.Worker.Worker;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client1Application.Worker.Guardian
{
    public class Client1ApplicationGuardian : ReceiveActor
    {
        private IActorRef _workerRouter;
        public Client1ApplicationGuardian()
        {
            Receive<string>(SendToWorker);
        }

        protected override void PreStart()
        {
            var props = Context.DI().Props<Client1ApplicationWorker>().WithRouter(new RoundRobinPool(50, new DefaultResizer(1, 100, 15)));
            _workerRouter = Context.ActorOf(props, "client1-application-worker");
            base.PreStart();
        }

        private void SendToWorker(string line)
        {
            _workerRouter.Tell(line);
        }
    }
}
