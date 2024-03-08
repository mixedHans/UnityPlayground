using System.Collections.Generic;
using UnityEngine;

public class UnityCopilot : MonoBehaviour
{

    //https://github.com/RageAgainstThePixel/com.openai.unity

    // Public Fields
    public string ApiKey;
    public string ModelId;
    public float Temperature;
    public string RoleDescription;
    public UnityCopilotKernel kernel = new();
    public List<PluginMonoBehaviour> plugins = new();

    private void Awake()
    {
        // Todo: prevent multiple plugins of the same type!
        kernel.SetupCopilotAsync(ApiKey, ModelId, plugins);
        
        //var answer = await kernel.AutoExecuteCopilotFunctionAsync("Destroy the cube for me");
        //Debug.Log(answer);
        //answer = await kernel.AutoExecuteCopilotFunctionAsync("Please spawn a cube for me");
        //Debug.Log(answer);
        //answer = await kernel.AutoExecuteCopilotFunctionAsync("Destroy the cube for me");
        //Debug.Log(answer);
    }
}