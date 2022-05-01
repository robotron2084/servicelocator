using System;
using System.Collections.Generic;

namespace DefaultNamespace
{
    public class ServiceLocatorCore
    {
        public static void Register<T>(Dictionary<Type, object> registry, Object obj)
        {
            Register(registry, typeof(T), obj);
        }

        public static void Register(Dictionary<Type, object> registry, Type t,  Object obj)
        {
            if (obj == null)
            {
                throw new NullReferenceException($"Attempting to insert a null reference for {t}");
            }
            registry.Add(t, obj);
        }

        
        public static void RegisterFactory<T>(Dictionary<Type, Func<object>> factories, Func<object> factoryMethod)
        {
            RegisterFactory(factories, typeof(T), factoryMethod);
        }

        public static void RegisterFactory(Dictionary<Type, Func<object>> factories, Type t, Func<object> factoryMethod)
        {
            factories[t] = factoryMethod;
        }
        
        public static T Get<T>(
            Dictionary<Type, Func<object>> factories,
            Dictionary<Type, object> registry
            )
        {
            return (T)Get(factories, registry, typeof(T));
        }

        public static object Get(
            Dictionary<Type, Func<object>> factories,
            Dictionary<Type, object> registry,
            Type t)
        {
            object retVal;
            if (!registry.TryGetValue(t, out retVal))
            {
                retVal = BuildFromFactory(factories, t);
                Register(registry, t, retVal);
            }
            return retVal;
        }

        private static object BuildFromFactory(Dictionary<Type, Func<object>> factories, Type t)
        {
            object retVal = null;
            Func<object> factoryMethod;
            if (factories.TryGetValue(t, out factoryMethod))
            {
                retVal = factoryMethod();
                if (retVal.GetType() != t)
                {
                    throw new InvalidCastException($"{retVal.GetType()} is not of expected type {t}");
                }
            }
            else
            {
                throw new NullReferenceException($"No Factory found for {t}");
            }

            return retVal;
        }


    }
}