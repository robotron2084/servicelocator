using System;
using System.Collections.Generic;
using DefaultNamespace;

namespace com.enemyhideout.servicelocator
{
  public static class ServiceLocator
  {
    
    static ServiceLocator()
    {
      _container = new Container();
    }

    public static void Reset()
    {
      _container = new Container();
    }

    // default container. Todo: more containers?
    private static Container _container;
    
    public static void Register<T>(object obj)
    {
      ServiceLocatorCore.Register<T>(_container.Objects, obj);
    }
    
    public static void RegisterFactory<T>(Func<object> factoryMethod)
    {
      ServiceLocatorCore.RegisterFactory<T>(_container.Factories, factoryMethod);
    }

    public static T Get<T>()
    {
      return ServiceLocatorCore.Get<T>(_container.Factories, _container.Objects);
    }

  }
}