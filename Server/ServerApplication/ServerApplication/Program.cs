using ServerApplication.Worker;
using System;
using Topshelf;

namespace ServerApplication
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
                host.SetDescription("ServerApplication Service");
                host.SetDisplayName("Akka Remoting Sample");
                host.SetServiceName("Akka Remoting Sample");
            });
        }
    }
}
