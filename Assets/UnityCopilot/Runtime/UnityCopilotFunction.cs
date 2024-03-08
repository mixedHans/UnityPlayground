using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

public class UnityCopilotFunction
{
    public string PluginName { get; }
    public string MethodName { get; }
    public Dictionary<string, object> Arguments { get; private set; }

    public UnityCopilotFunction(string pluginName, string methodName, Dictionary<string, object> arguments = null)
    {
        PluginName = pluginName;
        MethodName = methodName;
        Arguments = arguments;
    }

    public void TryConvertArgumentsToNativeTypes()
    {
        var convertedArguments = new Dictionary<string, object>();

        foreach (var kvp in Arguments)
        {
            if (kvp.Value is JsonElement element)
            {
                object convertedValue = element.ValueKind switch
                {
                    JsonValueKind.Number when element.TryGetInt32(out int intValue) => intValue,
                    JsonValueKind.Number => element.GetDouble(), // Default to float for any other number
                    JsonValueKind.String => element.GetString(),
                    JsonValueKind.True => true,
                    JsonValueKind.False => false,
                    _ => throw new NotImplementedException($"Cant convert JsonElement to native .NET type! Type: {kvp.Value}")
                };

                convertedArguments.Add(kvp.Key, convertedValue);
            }
            else
            {
                throw new NotImplementedException($"Value is not a JsonElement! Value: {kvp.Value}");
            }
        }

        Arguments = convertedArguments;
    }

    public string GetMethodWithArgumentsAsString(bool withPluginName = false)
    {
        var argumentList = Arguments.Select(kv => $"{kv.Key}: {kv.Value}");
        var arguments = string.Join(", ", argumentList);

        var methodNameWithArguments = string.Empty;
        if (withPluginName)
            methodNameWithArguments = $"{PluginName}.";

        methodNameWithArguments = $"{MethodName}({arguments})";
        return methodNameWithArguments;
    }
}
