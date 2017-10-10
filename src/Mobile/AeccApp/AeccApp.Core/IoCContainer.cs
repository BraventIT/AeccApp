using Autofac;
using System;
using System.Collections.Generic;
using System.Text;

namespace AeccApp.Core
{
    public class IocContainer
    {
        private static IContainer _container;
        private static ContainerBuilder _builder;

        public static void Register<O>()
        {
            if (_builder == null)
            {
                _builder = new ContainerBuilder();
            }

            _builder.RegisterType<O>();
        }

        public static void Register<I, O>() where O : I
        {
            if (_builder == null)
            {
                _builder = new ContainerBuilder();
            }

            _builder.RegisterType<O>().As<I>();
        }

        public static void RegisterAsSingleton<I, O>() where O : I
        {
            if (_builder == null)
            {
                _builder = new ContainerBuilder();
            }

            _builder.RegisterType<O>().As<I>().SingleInstance();
        }


        public static O Resolve<O>()
        {
          return  _container.Resolve<O>();
        }

        public static object Resolve(Type O)
        {
            return _container.Resolve(O);
        }

        public static void Build()
        {
            if (_container != null)
            {
                _container.Dispose();
            }
            if (_builder != null)
            {
                _container = _builder.Build();
            }
        }
    }
}
