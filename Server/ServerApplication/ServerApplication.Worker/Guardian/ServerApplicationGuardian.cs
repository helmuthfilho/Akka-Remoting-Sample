using Akka.Actor;
using Akka.DI.Core;
using Akka.Routing;
using ServerApplication.Worker.Worker;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ServerApplication.Worker.Guardian
{
    public class ServerApplicationGuardian : ReceiveActor
    {
        private IActorRef _workerRouter;
        private IActorRef _client1Router;
        private IActorRef _client2Router;
        private IActorRef _client3Router;

        public ServerApplicationGuardian()
        {
            Receive<string[]>(SendToClients);
        }

        protected override void PreStart()
        {
            var props = Context.DI().Props<ServerApplicationWorker>().WithRouter(new RoundRobinPool(250, new DefaultResizer(1, 1100, 200)));
            var client1Worker = Context.DI().Props<ServerApplicationWorker1>().WithRouter(new RoundRobinPool(3, new DefaultResizer(1, 15, 3)));
            _workerRouter = Context.ActorOf(props, "server-application-worker");
            _client1Router = Context.ActorOf(client1Worker, "server-client1Application-worker");
            base.PreStart();
        }

        private void SendToClients(string[] textLines)
        {
            Console.WriteLine("Enviando dados para a primeira aplicação cliente...");
            _client1Router.Tell(textLines);
        }
    }
}
