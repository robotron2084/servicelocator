using System;
using System.Collections.Generic;
using DefaultNamespace;
using NUnit.Framework;

namespace com.enemyhideout.servicelocator.tests
{
    public class ServiceLocatorTest
    {
        
        [Test]
        public void TestRegister([ValueSource(nameof(RegisterTestCases))] RegisterTestCase testCase)
        {
            Dictionary<Type, object> output = new Dictionary<Type, object>(testCase.Input);
            if (testCase.Throws != null)
            {
                Assert.Throws(testCase.Throws, () => ServiceLocatorCore.Register(output, testCase.Type, testCase.Obj));
            }
            else
            {
                ServiceLocatorCore.Register(output, testCase.Type, testCase.Obj);
                Assert.That(output, Is.EqualTo(testCase.Expected));
            }
        }

        [Test]
        public void TestDoubleRegister()
        {
            Dictionary<Type, object> output = new Dictionary<Type, object>();
            ServiceLocatorCore.Register(output, typeof(TestService), _testService);
            Assert.Throws<ArgumentException>(() => ServiceLocatorCore.Register(output,typeof(TestService), _testService));
            Assert.That(output, Is.EqualTo( new Dictionary<Type, object>(){ { typeof(TestService), _testService }}));
        }

        private static Dictionary<Type, object> _emptyRegistry = new Dictionary<Type, object>();
        private static TestService _testService = new TestService();

        public static List<RegisterTestCase> RegisterTestCases = new List<RegisterTestCase>()
        {
            new RegisterTestCase()
            {
                Description = "Register Object For Its Type",
                Obj = _testService,
                Type = typeof(TestService),
                Input = _emptyRegistry,
                Expected = new Dictionary<Type, object>(){ { typeof(TestService), _testService }}
            },
            new RegisterTestCase()
            {
                Description = "Register Object For An Interface",
                Obj = _testService,
                Type = typeof(ITestService),
                Input = _emptyRegistry,
                Expected = new Dictionary<Type, object>(){ { typeof(ITestService), _testService }}
            },
            new RegisterTestCase()
            {
                Description = "Register Object For Its Type",
                Obj = _testService,
                Type = typeof(ITestService),
                Input = _emptyRegistry,
                Expected = new Dictionary<Type, object>(){ { typeof(ITestService), _testService }}
            },
            new RegisterTestCase()
            {
                Description = "Register null throws",
                Obj = null,
                Type = typeof(ITestService),
                Input = _emptyRegistry,
                Throws = typeof(NullReferenceException)
            },
            
        };
        
        public class TestService : ITestService {}
        
        public interface ITestService { }
        
        public class TestCaseBase
        {
            public string Description;

            public override string ToString()
            {
                return Description;
            }
        }
        
        public class RegisterTestCase : TestCaseBase
        {
            public object Obj;
            public Type Type;
            public Dictionary<Type, object> Input;
            public Dictionary<Type, object> Expected;
            public Type Throws;
        }
        
        public class RegisterFactoryTestCase : TestCaseBase
        {
            public Func<Object> Obj;
            public Type Type;
            public Dictionary<Type, Func<object>> Input;
            public Dictionary<Type, Func<object>> Expected;
        }

        private static Func<object> _testServiceFactoryMethod = () => _testService;
        private static Dictionary<Type, Func<object>> _emptyFactoryRegistry = new Dictionary<Type, Func<object>>();

        public static List<RegisterFactoryTestCase> RegisterFactoryTestCases = new List<RegisterFactoryTestCase>()
        {
            new RegisterFactoryTestCase()
            {
                Description = "Register Factory For Its Type",
                Obj = _testServiceFactoryMethod,
                Type = typeof(TestService),
                Input = _emptyFactoryRegistry,
                Expected = new Dictionary<Type, Func<object>>(){ { typeof(TestService), _testServiceFactoryMethod }}
            },

            new RegisterFactoryTestCase()
            {
                Description = "Register Factory For Its Interface",
                Obj = _testServiceFactoryMethod,
                Type = typeof(ITestService),
                Input = _emptyFactoryRegistry,
                Expected = new Dictionary<Type, Func<object>>(){ { typeof(ITestService), _testServiceFactoryMethod }}
            },

        };

        [Test]
        public void TestRegisterFactory(
            [ValueSource(nameof(RegisterFactoryTestCases))] RegisterFactoryTestCase testCase)
        {
            Dictionary<Type, Func<object>> output = new Dictionary<Type, Func<object>>(testCase.Input);
            ServiceLocatorCore.RegisterFactory(output, testCase.Type, testCase.Obj);
            Assert.That(output, Is.EqualTo(testCase.Expected));
        }

        [Test]
        public void TestGet([ValueSource(nameof(GetTestCases))] GetTestCase testCase)
        {
            // registry mutates, copy before use.
            Dictionary<Type, object> registry = new Dictionary<Type, object>(testCase.ObjectRegistry);
            object output = ServiceLocatorCore.Get(testCase.FactoryRegistry, registry,
                testCase.RequestedType);
            Assert.That(output, Is.EqualTo(testCase.ExpectedValue));
            if (testCase.ExpectedRegistry != null)
            {
                Assert.That(registry, Is.EqualTo(testCase.ExpectedRegistry));
                }
        }

        public static List<GetTestCase> GetTestCases = new List<GetTestCase>()
        {
            new GetTestCase
            {
                Description = "Object exists in registry",
                RequestedType = typeof(TestService),
                ExpectedValue = _testService,
                FactoryRegistry = _emptyFactoryRegistry,
                ObjectRegistry = new Dictionary<Type, object>(){ { typeof(TestService), _testService }}
            },
            new GetTestCase
            {
                Description = "Object does not exist in registry, factory method exists",
                RequestedType = typeof(TestService),
                ExpectedValue = _testService,
                ExpectedRegistry = new Dictionary<Type, object>(){ { typeof(TestService), _testService }},
                FactoryRegistry = new Dictionary<Type, Func<object>>(){ { typeof(TestService), _testServiceFactoryMethod}},
                ObjectRegistry = _emptyRegistry
            }

        };
        
        public class GetTestCase : TestCaseBase
        {
            public Type RequestedType;
            public object ExpectedValue;
            public Dictionary<Type, object> ExpectedRegistry;
            public Dictionary<Type, Func<object>> FactoryRegistry;
            public Dictionary<Type, object> ObjectRegistry;
        }

    }

    
}