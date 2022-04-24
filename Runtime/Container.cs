using System;
using System.Collections.Generic;

namespace com.enemyhideout.servicelocator
{
    public class Container
    {
        public Dictionary<Type, object> Objects = new Dictionary<Type,object>();
        public Dictionary<Type, Func<object>> Factories = new Dictionary<Type, Func<object>>();

    }
}