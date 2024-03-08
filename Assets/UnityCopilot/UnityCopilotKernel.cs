using Azure.AI.OpenAI;
using Microsoft.Extensions.Azure;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.Services;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

public class UnityCopilotKernel
{
    // Config
    private float m_temperature;
    private string m_roleDescription;

    // Private members
    private Kernel m_Kernel;
    private IChatCompletionService m_ChatCompletionService;

    // Private Methods
    public void SetupCopilotAsync(string apiKey, string modelId, List<PluginMonoBehaviour> plugins, float temperature = 0.9f, string roleDescription = "You are a helpful and friendly assistent")
    {
        var builder = Kernel.CreateBuilder();
        builder.AddOpenAITextGeneration(modelId);
        builder.AddOpenAIChatCompletion(modelId, apiKey);

        foreach (var plugin in plugins) { builder.AddPluginFromObject(plugin); }
        m_Kernel = builder.Build();

        m_ChatCompletionService = m_Kernel.GetRequiredService<IChatCompletionService>();
        m_temperature = temperature;
        m_roleDescription = roleDescription;
    }

    // Public Methods
    public async Task<List<UnityCopilotFunction>> RequestCopilotFunctionsAsync(string prompt)
    {
        var executionSettings = new OpenAIPromptExecutionSettings
        {
            ToolCallBehavior = ToolCallBehavior.EnableKernelFunctions,
            Temperature = m_temperature,
            ChatSystemPrompt = m_roleDescription
        };

        var result = await m_ChatCompletionService.GetChatMessageContentAsync(
                prompt: prompt,
                executionSettings: executionSettings,
                kernel: m_Kernel);

        List<UnityCopilotFunction> kernelFunctionDescription = new();

        if (result.Metadata.TryGetValue("ChatResponseMessage.FunctionToolCalls", out var functionToolCalls))
        {
            foreach (var functionToolCall in (List<ChatCompletionsFunctionToolCall>)functionToolCalls)
            {
                string pluginName = functionToolCall.Name.Split('-')[0];
                string methodName = functionToolCall.Name.Split('-')[1];
                Dictionary<string, object> arguments = JsonSerializer.Deserialize<Dictionary<string, object>>(functionToolCall.Arguments);
                kernelFunctionDescription.Add(new UnityCopilotFunction(pluginName, methodName, arguments));
            }
        }

        return kernelFunctionDescription;
    }

    public async Task ExecuteCopilotFunctionAsync(UnityCopilotFunction function)
    {
        function.TryConvertArgumentsToNativeTypes();
        await m_Kernel.ExecuteKernelFunction(function);
    }

    public async Task<string> AutoExecuteCopilotFunctionAsync(string prompt)
    {
        OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new()
        {
            ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions,
            Temperature = m_temperature,
            ChatSystemPrompt = m_roleDescription
        };

        var result = m_ChatCompletionService.GetStreamingChatMessageContentsAsync(
            prompt,
            executionSettings: openAIPromptExecutionSettings,
            kernel: m_Kernel);

        string fullMessage = "";
        await foreach (var content in result) { fullMessage += content.Content; }
        return fullMessage;
    }

    public async IAsyncEnumerable<string> AutoExecuteCopilotFunctionStreamedAsync(string prompt)
    {
        OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new()
        {
            ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions,
            Temperature = m_temperature,
            ChatSystemPrompt = m_roleDescription
        };

        var result = m_ChatCompletionService.GetStreamingChatMessageContentsAsync(
            prompt,
            executionSettings: openAIPromptExecutionSettings,
            kernel: m_Kernel);

        await foreach (var content in result) 
        { 
            if (content.Content != string.Empty)
                yield return content.Content; 
        }
    }
}