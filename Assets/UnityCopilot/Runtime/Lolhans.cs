using Azure.AI.OpenAI;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public sealed class UnityFunctionAttribute : Attribute
{
    /// <summary>Initializes the attribute.</summary>
    public UnityFunctionAttribute() { }

    /// <summary>Initializes the attribute.</summary>
    /// <param name="name">The name to use for the function.</param>
    public UnityFunctionAttribute(string? name) => this.Name = name;

    /// <summary>Gets the function's name.</summary>
    /// <remarks>If null, a name will based on the name of the attributed method will be used.</remarks>
    public string? Name { get; }
}

public class MyPlugin
{
    [UnityFunction]
    [Description("Ask for the age of Leon")]
    public static void TestMethod()
    {
        Debug.Log("Leon is 28 years old!");
    }
}



public class Lolhans : MonoBehaviour
{
    public InputField inputField;
    public Button sendButton;

    public GameObject confirmationObject;
    public Button confirmButton;
    public Button denyButton;
    public Text answerText;

    private TaskCompletionSource<bool> _confirmButtonClickedTcs;

    private async void Awake()
    {
        var leonsPlugin = new LeonsPlugin();
        MyKernel.SetupKernel("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", "gpt-3.5-turbo", leonsPlugin);

        //var executionSettings = new OpenAIPromptExecutionSettings
        //{ ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions, };

        //var chatCompletionService = MyKernel.Instance.GetRequiredService<IChatCompletionService>();

        //var streamResult = chatCompletionService
        //    .GetStreamingChatMessageContentsAsync(
        //        prompt: "Spawn a Cube",
        //        executionSettings: executionSettings,
        //        kernel: MyKernel.Instance);

        //await foreach (var chat in streamResult) 
        //{
        //    Debug.Log(chat.Content);
        //}

        //var taskHandle = chatCompletionService
        //    .GetChatMessageContentsAsync(
        //        prompt: "Spawn a Cube",
        //        executionSettings: executionSettings,
        //        kernel: MyKernel.Instance);

        //try
        //{
        //    var result = await taskHandle;
        //    foreach (var chat in result)
        //    {
        //        Debug.Log($"{chat.Content}");
        //    }
        //}
        //catch (Exception ex)
        //{
        //    Debug.LogException(ex);
        //}
        
        sendButton.onClick.AddListener(() => RequestKernelFunction(inputField.text));
        confirmButton.onClick.AddListener(ConfirmButtonClicked);
        denyButton.onClick.AddListener(DenyButtonClicked);
        confirmationObject.SetActive(false);
    }

    private async void RequestKernelFunction(string prompt)
    {
        var (success, kernelFunctionDescriptionList) = await MyKernel.TryRequestKernelFunction(prompt);

        if (success == false)
            return; // failed!

        // Todo: Make a scrollview with adjustable arguments for each kernelfunction
        var kernelFunction = kernelFunctionDescriptionList[0];
        kernelFunction.TryConvertArgumentsToNativeTypes();

        confirmationObject.SetActive(true);
        answerText.text = kernelFunction.GetMethodWithArgumentsAsString();
        _confirmButtonClickedTcs = new TaskCompletionSource<bool>();
        var confirmed = await _confirmButtonClickedTcs.Task;

        if (confirmed == false)
            return;

        await MyKernel.ExecuteKernelFunction(kernelFunction);

        confirmationObject.SetActive(false);
    }

    private void ConfirmButtonClicked()
    {
        _confirmButtonClickedTcs?.TrySetResult(true);
    }

    private void DenyButtonClicked()
    {
        _confirmButtonClickedTcs?.TrySetResult(false);
    }

    public async Task<bool> Confirmation(string confirmationDetails)
    {
        confirmationObject.SetActive(true);
        answerText.text = confirmationDetails;
        _confirmButtonClickedTcs = new TaskCompletionSource<bool>();

        var confirmed = await _confirmButtonClickedTcs.Task;
        answerText.text = "";
        confirmationObject.SetActive(false);

        return confirmed;
    }
}

public static partial class MyKernel
{
    public static Kernel Instance { get; private set; } = null;

    public static void SetupKernel(string apiKey, string modelId, params object[] plugins)
    {
        if (Instance == null)
        {
            var builder = Kernel.CreateBuilder();

            builder
                .AddOpenAITextGeneration(modelId)
                .AddOpenAIChatCompletion(modelId, apiKey)
                .AddOpenAITextGeneration(modelId);

            foreach (var plugin in plugins)
                builder.Plugins.AddFromObject(plugin);

            var myPlugin = new MyPlugin();
            //var kernelPlugin = BuilderExtension.CreateFromObject(myPlugin);

            //builder.Plugins.Add(kernelPlugin);

            Instance = builder.Build();
        }
    }

    public static async Task RequestKernelFunction(string prompt, Func<string, Task<bool>> comfirmation)
    {
        var executionSettings = new OpenAIPromptExecutionSettings
        { ToolCallBehavior = ToolCallBehavior.EnableKernelFunctions, };

        var chatCompletionService = Instance.GetRequiredService<IChatCompletionService>();

        var result = await chatCompletionService
            .GetChatMessageContentAsync(
                prompt: prompt,
                executionSettings: executionSettings,
                kernel: Instance);

        var success = false;
        var pluginName = string.Empty;
        var methodName = string.Empty;
        object functionToolCalls = null;
        Dictionary<string, object> arguments = new();

        if (success == false)
        {
            success = result.Metadata.TryGetValue("ChatResponseMessage.FunctionToolCalls", out functionToolCalls);
        }

        if (success == true)
        {
            foreach (var functionToolCall in (List<ChatCompletionsFunctionToolCall>)functionToolCalls)
            {
                pluginName = functionToolCall.Name.Split('-')[0];
                methodName = functionToolCall.Name.Split('-')[1];
                arguments = JsonSerializer.Deserialize<Dictionary<string, object>>(functionToolCall.Arguments);
            }
        }

        if (success == false)
        {
            _ = comfirmation("Can't find method that matches your request!");
            return;
        }

        var confirmationDetails = "Would you like to invoke the following method: \n" +
            $"-> Method: {pluginName}-{methodName} \n" +
            $"With arguments: \n";

        foreach (var argument in arguments)
        {
            confirmationDetails += "-> " + argument.Key.ToString();
            confirmationDetails += ": ";
            confirmationDetails += argument.Value.ToString();
            confirmationDetails += ",\n";
        }

        if (await comfirmation(confirmationDetails) == true)
        {
            if (arguments.Count == 0)
                await Instance.InvokeAsync(pluginName, methodName);
            else
                await Instance.InvokeAsync<string>(pluginName, methodName, new KernelArguments(arguments));
        }
    }

    public static async Task<(bool success, List<UnityCopilotFunction> kernelFunctionDescription)> TryRequestKernelFunction(string prompt)
    {
        var executionSettings = new OpenAIPromptExecutionSettings
            { ToolCallBehavior = ToolCallBehavior.EnableKernelFunctions, };

        var chatCompletionService = Instance.GetRequiredService<IChatCompletionService>();

        var result = await chatCompletionService
            .GetChatMessageContentAsync(
                prompt: prompt,
                executionSettings: executionSettings,
                kernel: Instance);

        if (result.Metadata.TryGetValue("ChatResponseMessage.FunctionToolCalls", out var functionToolCalls))
        {
            List<UnityCopilotFunction> kernelFunctionDescription = new();

            foreach (var functionToolCall in (List<ChatCompletionsFunctionToolCall>)functionToolCalls)
            {
                string pluginName = functionToolCall.Name.Split('-')[0];
                string methodName = functionToolCall.Name.Split('-')[1];
                Dictionary<string, object> arguments = JsonSerializer.Deserialize<Dictionary<string, object>>(functionToolCall.Arguments);
                kernelFunctionDescription.Add(new UnityCopilotFunction(pluginName, methodName, arguments));
            }

            return (true, kernelFunctionDescription);
        }

        return (false, null);
    }

    public static async Task ExecuteKernelFunction(UnityCopilotFunction kernelFunctionDescription)
    {
        if (kernelFunctionDescription.Arguments.Count == 0)
            await Instance.InvokeAsync(kernelFunctionDescription.PluginName, kernelFunctionDescription.MethodName);
        else
            await Instance.InvokeAsync<string>(kernelFunctionDescription.PluginName, kernelFunctionDescription.MethodName, new KernelArguments(kernelFunctionDescription.Arguments));
    }

    public static async Task UseAIServiceWithManualInvokationStreaming(string prompt, Func<string, Task<bool>> comfirmation)
    {
        var executionSettings = new OpenAIPromptExecutionSettings
        { ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions };

        var chatCompletionService = Instance.GetRequiredService<IChatCompletionService>();

        var result = chatCompletionService
            .GetStreamingChatMessageContentsAsync(
                prompt: prompt,
                executionSettings: executionSettings,
                kernel: Instance);

        await foreach (var content in result)
        {
            Debug.Log(content);
        }
    }
}
