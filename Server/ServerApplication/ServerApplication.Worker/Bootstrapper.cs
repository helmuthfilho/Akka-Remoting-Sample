using Castle.MicroKernel.Registration;
using Castle.Windsor;
using ServerApplication.Worker.Guardian;
using ServerApplication.Worker.Worker;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace ServerApplication.Worker
{
    public class Bootstrapper
    {
        public static IWindsorContainer Initialize()
        {
            var container = new WindsorContainer();

            //Guardian
            container.Register(Component.For<ServerApplicationGuardian>().LifestyleTransient());

            //Worker
            container.Register(Component.For<ServerApplicationWorker>().LifestyleTransient());
            container.Register(Component.For<ServerApplicationWorker1>().LifestyleTransient());

            return container;
        }
    }
}
