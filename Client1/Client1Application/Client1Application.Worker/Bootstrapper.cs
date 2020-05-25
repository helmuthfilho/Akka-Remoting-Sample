using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Client1Application.Worker.Guardian;
using Client1Application.Worker.Worker;
using System;
using System.Collections.Generic;

using System.Text;

namespace Client1Application.Worker
{
    public class Bootstrapper
    {
        public static IWindsorContainer Initialize()
        {
            var contatiner = new WindsorContainer();

            //Guardian
            contatiner.Register(Component.For<Client1ApplicationGuardian>().LifestyleTransient());

            //Worker
            contatiner.Register(Component.For<Client1ApplicationWorker>().LifestyleTransient());

            return contatiner;
        }
    }
}
