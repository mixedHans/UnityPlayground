using Microsoft.SemanticKernel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using UnityEngine;

public static class BuilderExtensions
{
    public static IKernelBuilder AddPluginFromObject(this IKernelBuilder builder, object target, string pluginName = null)
    {
        //pluginName ??= target.GetType().Name;
        MethodInfo[] methods = target.GetType().GetMethods();// BindingFlags.Public | BindingFlags.Instance);
        var functions = new List<KernelFunction>();
        foreach (MethodInfo method in methods)
        {
            if (method.GetCustomAttribute<UnityFunctionAttribute>() is not null)
            {
                var methodDescription = method.GetCustomAttribute<DescriptionAttribute>(inherit: true)?.Description;
                var methodName = method.Name;
                functions.Add(KernelFunctionFactory.CreateFromMethod(method, target, methodName, methodDescription));
                Debug.Log("Added kernel function");
            }
        }
        var description = target.GetType().GetCustomAttribute<DescriptionAttribute>(inherit: true)?.Description;
        var plugin = KernelPluginFactory.CreateFromFunctions(pluginName, description, functions);
        builder.Plugins.Add(plugin);
        return builder;
    }

    public static IKernelBuilder AddPluginFromObject(this IKernelBuilder builder, PluginMonoBehaviour plugin, string pluginName = null)
    {
        pluginName ??= plugin.GetType().Name;
        MethodInfo[] methods = plugin.GetType().GetMethods();// BindingFlags.Public | BindingFlags.Instance);
        var functions = new List<KernelFunction>();
        foreach (MethodInfo method in methods)
        {
            if (method.GetCustomAttribute<UnityFunctionAttribute>() is not null)
            {
                var methodDescription = method.GetCustomAttribute<DescriptionAttribute>(inherit: true)?.Description;
                var methodName = method.Name;
                functions.Add(KernelFunctionFactory.CreateFromMethod(method, plugin, methodName, methodDescription));
            }
        }
        var description = plugin.GetType().GetCustomAttribute<DescriptionAttribute>(inherit: true)?.Description;
        var kernelPlugin = KernelPluginFactory.CreateFromFunctions(pluginName, description, functions);
        builder.Plugins.Add(kernelPlugin);
        return builder;
    }
}
