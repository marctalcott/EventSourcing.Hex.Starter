// See https://aka.ms/new-console-template for more information

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Application.Advertisers.Commands;
using Domain.Aggregates;
using Domain.Enums;
using Domain.ES.Events;
using Domain.ES.EventStore;
using MediatR;

Console.WriteLine("Hello, World!");
GetDocs();

static void GetDocs()
{
    Ad add = new Ad(Guid.Empty, "hi", AdStatus.Published, false);
    AddAdvertiser cmd = new AddAdvertiser(new EventUserInfo("", false), Guid.Empty, "");

    var assms = FindAssemblies();
    List<Type> events = new List<Type>();
    foreach (var assm in assms)
    {
        events.AddRange(FindDerivedTypes(assm, typeof(EventBase)));
    }

    foreach (var e in events)
    {
        Console.WriteLine(e.FullName);
    }

    // IRequestHandler<ChangeAdvertiserAddress>
    List<Type> commands = new List<Type>();
    foreach (var assm in assms)
    {
        // commands.AddRange(FindDerivedTypes(assm, typeof(IRequestHandler<ChangeAdvertiserAddress>)));  
        commands.AddRange(FindDerivedTypes(assm, typeof(IRequest)));
        // FindDerivedTypes(assm, typeof(IRequestHandler<IRequest>)));

        List<Type> commandHandlers = new List<Type>();
        foreach (var command in commands)
        {
            var name = $"IRequestHandler<{command}>";
            Type tt = assm.GetType(name);

            Console.WriteLine(command.FullName);

            if (tt != null)
            {
                commandHandlers = FindDerivedTypes(assm, tt).ToList();
                foreach (var handler in commandHandlers)
                {
                    Console.WriteLine(handler);
                }
            }
        }
    }
}

static Assembly[] FindAssemblies()
{
    return AppDomain.CurrentDomain.GetAssemblies();
}

static IEnumerable<Type> FindDerivedTypes(Assembly assembly, Type baseType)
{
    return assembly.GetTypes().Where(t => baseType.IsAssignableFrom(t));
}

static IEnumerable<Type> FindInterfaceImplementations(Assembly assembly, Type interfaceType)
{
    return Assembly.GetExecutingAssembly()
        .GetTypes()
        .Where(type => interfaceType.IsAssignableFrom(type) && !type.IsInterface);
}