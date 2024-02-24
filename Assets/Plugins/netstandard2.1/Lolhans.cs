using UnityEngine;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using System;
using UnityEngine.UI;
using System.ComponentModel;
using Microsoft.SemanticKernel.ChatCompletion;
using System.Collections.Generic;
using Azure.AI.OpenAI;
using System.Runtime.CompilerServices;
using static Microsoft.SemanticKernel.KernelJsonSchema;
using System.Collections;
using System.Text.Json;


public class Lolhans : MonoBehaviour
{
    public InputField inputField;
    public Button button;
    public Text answerText;

    private Kernel _kernel;

    private async void Start()
    {
        var builder = Kernel.CreateBuilder();

        var apiKey = "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";
        var model = "gpt-3.5-turbo";

        builder
            .AddOpenAIChatCompletion(model, apiKey)
            .AddOpenAITextGeneration("gpt-3.5-turbo");

        builder.Plugins.AddFromType<LeonsPlugin>();

        _kernel = builder.Build();



        // Enable auto function calling
        OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new()
        {
            ToolCallBehavior = ToolCallBehavior.EnableKernelFunctions,
        };

        var chatCompletionService = _kernel.GetRequiredService<IChatCompletionService>();

        var result = await chatCompletionService
            .GetChatMessageContentsAsync(
                "I want to add a lastname to firstname Leon and also a middlename Little",
                executionSettings: openAIPromptExecutionSettings,
                kernel: _kernel);


        var pluginName = "";
        var methodName = "";
        Dictionary<string, object> arguments = new();

        foreach (var content in result )
        {
            Debug.Log(content.Content);
            Debug.Log(content.InnerContent);
            Debug.Log(content.Items);

            foreach (var item in content.Metadata)
            {
                Debug.Log(item.Key);
                Debug.Log(item.Value);
            }

            var success = content.Metadata.TryGetValue("ChatResponseMessage.FunctionToolCalls", out var functionToolCalls);

            foreach (var functionToolCall in (List<ChatCompletionsFunctionToolCall>)functionToolCalls)
            {
                pluginName = functionToolCall.Name.Split('-')[0];
                methodName = functionToolCall.Name.Split('-')[1];

                Debug.Log($"{functionToolCall.Name}");
                Debug.Log($"{functionToolCall.Arguments}");

                arguments = JsonSerializer.Deserialize<Dictionary<string, object>>(functionToolCall.Arguments);

                if (arguments != null)
                {
                    foreach (KeyValuePair<string, object> entry in arguments)
                    {
                        Debug.Log($"{entry.Key}: {entry.Value}");
                    }
                }
            }

            if (content.ToString() != string.Empty )
            {
                Debug.Log(content);
            }
        }

        string answer = await _kernel.InvokeAsync<string>(pluginName, methodName, new KernelArguments(arguments));
        Debug.Log(answer);
    }
}
