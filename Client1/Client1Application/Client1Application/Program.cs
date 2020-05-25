using Client1Application.Worker;
using System;
using Topshelf;

namespace Client1Application
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(host =>
            {
                host.Service<Startup>(config =>
                {
                    config.ConstructUsing(s => new Startup());
                    config.WhenStarted(s => s.Run());
                    config.WhenStopped(s => s.Stop());
                });

                host.RunAsLocalSystem();
                host.SetDescription("Client1Application Service");
                host.SetDisplayName("Akka Remoting Sample Client1");
                host.SetServiceName("Akka Remoting Sample Client1");
            });
        }
    }
}
