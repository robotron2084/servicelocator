# Service Locator Example

This is a working example of the Service Locator pattern for use within the Unity game engine. This repository is provided partially as an actual usable resource, but also as an example of the Service Locator pattern.

## The Service Locator Pattern

While [Wikipedia](https://en.wikipedia.org/wiki/Service_locator_pattern) provides a good overview, I want to focus on the following:

 * Why you should use this pattern over Singletons.
 * How to use this pattern.
 * How not to write services or other singleton code.

Things I won't touch on here:
 * Dependency Injection and its Benefits
 * Disadvantages, why this is an 'anti-pattern', etc.
 
## Stop Writing Singletons

One of the trickiest aspects of development is figuring out how to 'get access to the thing'. Frequently developers need one instance of an important item. They write code to allow this one instance to be accessible in their code and access it via `MyThing.Instance`. 

This pattern works but you are really stuck with that one item. And you have to write that `.Instance` code in each class that uses it. This gets worse if you work on a team and everyone writes their singletons slightly differently. And don't ever suddenly need more than one of the item.

## Locating of Services

Enter Service Locator. In essence, this is an uber-Singleton. A singleton to rule them all. It provides a consistent system you can use to provide access to all your Singletons, which, in this enlightened new world we'll call Services. Yep, most of the time the Service in Service Locator is really just a Singleton. But I guess Singleton Locator doesn't have the same ring to it.

## Installation

This repo is a Unity Package. You can install from disk or from Github via Package Manager.

## Using Service Locator

There are two steps involved to using services: registering them, and using them. 


## Registering Services
You can register a service by either registering the service directly, or by registering a 'factory' method that will create the service when it is first accessed. Do this upon startup of your game.

```c#
void InitializeOnStartup()
{
  // Create and register the service immediately.
  ServiceLocator.Register<MyService>(new MyService());
  // Or register the service now, and let it be made later.
  ServiceLocator.RegisterFactory<MyService>(() => new MyService());
  ... register more services here...
}
```

## Using Services
When using a service, you use `ServiceLocator.Get<T>()` to get the item.
```c#
private MyService _myService;

void OnStartup()
{
    _myService = ServiceLocator.Get<MyService>();
    // ... use your service to do service-y things.
}
```

And that's it! You now have a system that is just as easy to use as a singleton, but with less code!

## Super Secret Pro Tips

I probably shouldn't be telling you this, but here are some extra cool things you can do with this pattern.

### Service-ception
Frequently you may have services that depend upon services. When these dependencies exist, its quite simple to just use service locator to fetch them:

```c#
void IntializeOnStartup()
{
    ServiceLocator.RegisterFactory(() => new MyService());
    ServiceLocator.RegisterFactory(() => new MyOtherService(ServiceLocator.Get<MyService>());
}
```
This allows your dependencies to all initialize lazily and as needed by your other services. Cool!

### Interfaces instead of types

Another pro tip is that you can use interfaces to register your services and register different services for different reasons.
```c#
void InitializeOnStartup()
{
    IStoreService storeService = null;
    if(Application.Platform == RuntimePlatform.iOS)
    {
        storeService = new IOSStoreProvider();
    else if(Application.Platform == RuntimePlatform.Android)
    {
        storeService = new AndroidStoreProvider();
    }
    else{
        storeService = new DefaultStoreProvider();
    }
    ServiceLocator.Register<IStoreProvider>(storeService);
}
```
This works great for unit tests as well!