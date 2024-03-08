using Microsoft.SemanticKernel;
using System.Threading.Tasks;

public static class KernelExtensions
{
    public static async Task ExecuteKernelFunction(this Kernel kernel, UnityCopilotFunction kernelFunctionDescription)
    {
        if (kernelFunctionDescription.Arguments.Count == 0)
            await kernel.InvokeAsync(kernelFunctionDescription.PluginName, kernelFunctionDescription.MethodName);
        else
            await kernel.InvokeAsync<string>(kernelFunctionDescription.PluginName, kernelFunctionDescription.MethodName, new KernelArguments(kernelFunctionDescription.Arguments));
    }
}