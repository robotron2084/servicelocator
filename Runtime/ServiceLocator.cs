using System;
using System.Collections.Generic;

namespace com.enemyhideout
{
  public static class ServiceLocator
  {
    private static Dictionary<Type, Object> objects = new Dictionary<Type,Object>();
    private static Dictionary<Type, Func<Object>> factories = new Dictionary<Type, Func<object>>();
    
    public static void Register<T>(Object obj)
    {
      objects[typeof(T)] = obj;
    }

    
    public static void RegisterFactory<T>(Func<Object> factoryMethod)
    {
      factories[typeof(T)] = factoryMethod;
    }

    public static T Get<T>()
    {
      Object retVal;
      objects.TryGetValue(typeof(T), out retVal);
      if (retVal == null)
      {
        Func<Object> factoryMethod;
        if (factories.TryGetValue(typeof(T), out factoryMethod))
        {
          retVal = factoryMethod();
          if (retVal.GetType() != typeof(T))
          {
            throw new InvalidCastException($"{retVal.GetType()} is not of expected type {typeof(T)}");
          }
          Register<T>(retVal);
        }
      }
      return (T)retVal;
    }
  }
}