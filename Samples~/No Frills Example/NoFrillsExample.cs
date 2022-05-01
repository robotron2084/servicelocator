using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.enemyhideout.servicelocator.samples
{
    public class NoFrillsExample : MonoBehaviour
    {
        // Start is called before the first frame update
        void Awake()
        {
            //Initialize our Services.
            ServiceLocator.Register<MyService>( new MyService());
        }

        void Start()
        {
            // Get the service.
            MyService myService = ServiceLocator.Get<MyService>();
            // Use the service.
            int answer = myService.GetAnswer();
            Debug.Log($"What is the answer? {answer}");
        }
    }
}