using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ServerApplication.Worker.Worker
{
    public class ServerApplicationWorker1 : ReceiveActor
    {
        private readonly ActorSelection _clientApplication1 = Context.ActorSelection("akka.tcp://clientApplication1@localhost:7000/user/client1-application-guardian");
        public ServerApplicationWorker1()
        {
            Receive<string[]>(SendToClient);
        }

        private void SendToClient(string[] textLines)
        {
            foreach(var line in textLines)
            {
                _clientApplication1.Tell(line);
            }
        }
    }
}
