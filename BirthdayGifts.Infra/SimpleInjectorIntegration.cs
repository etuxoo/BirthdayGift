using Serilog;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Text;

namespace BirthdayGifts.Infra
{
    class SimpleInjectorIntegration
    {
        public static readonly Container Vessel = new Container();

        public SimpleInjectorIntegration()
        {
            InitializeContainer();

            Vessel.Verify();
        }

        private static void InitializeContainer()
        {
            Vessel.Register<IConnectionStringProvider, ConnectionStringProvider>(Lifestyle.Singleton);

            Vessel.Register(typeof(ILogFacility<>), typeof(SeriLogFacility<>), Lifestyle.Scoped);

            Vessel.RegisterInstance(Log.Logger);
        }
    }
}
