using UnityEngine;
using Microsoft.SemanticKernel;
using System.ComponentModel;
using System.Globalization;

public class LeonsPlugin
{
    [KernelFunction()]
    [Description("Answers to a ping request")]
    public string AwnserPingWithPong()
    {
        Debug.Log("AwnserPingWithPong invoked");
        return "pong";
    }

    private GameObject gameobject;

    [KernelFunction()]
    [Description("Spawns a cube gameobject")]
    public async Awaitable SpawnCubeGameobject()
    {
        await Awaitable.MainThreadAsync();
        Debug.Log("SpawnCubeGameobject invoked");
        gameobject = GameObject.CreatePrimitive(PrimitiveType.Cube);
    }

    [KernelFunction()]
    [Description("Spawns a capsule gameobject")]
    public void SpawnCapsuleGameobject()
    {
        Debug.Log("SpawnCapsuleGameobject invoked");
        gameobject = GameObject.CreatePrimitive(PrimitiveType.Capsule);
    }

    [KernelFunction()]
    [Description("Scales a gameobject")]
    public void ScaleGameobject(
        [Description("The scale factor")] string factor)
    {
        Debug.Log("ScaleGameobject invoked");
        Debug.Log(factor);
        Debug.Log(float.Parse(factor, CultureInfo.InvariantCulture));
        gameobject.transform.localScale *= float.Parse(factor, CultureInfo.InvariantCulture);
    }

    [KernelFunction()]
    [Description("Spawns a gameobject")]
    public void SpawnMultipleGameobject(
        [Description("The count how many gameobjects to spawn")] int count)
    {
        Debug.Log("SpawnMultipleGameobject invoked");
        for (int i = 0; i<= count; i++)
        {
            GameObject.CreatePrimitive(PrimitiveType.Capsule);
        }
    }

    [KernelFunction()]
    [Description("Delets a gameobject")]
    public void DeleteGameobject()
    {
        Debug.Log("DeleteGameobject invoked");
        GameObject.Destroy(gameobject);
    }

    [KernelFunction()]
    [Description("Adds a lastname to a firstname")]
    public string AddLastName(
        [Description("The firstname to add a lastname to")] string firstname)
    {
        Debug.Log("AddLastName invoked");
        return firstname + " Lolhans";
    }

    [KernelFunction()]
    [Description("Adds a lastname and a middlename to a firstname")]
    public string AddLastAndMiddleName(
    [Description("The firstname")] string firstname,
    [Description("The middlename")] string middlename)
    {
        Debug.Log("AddLastAndMiddleName invoked");
        return firstname + " " + middlename + " Lolhans";
    }

    [KernelFunction()]
    [Description("Here i ask, if Nina is stupid")]
    public string IsNinaStupid()
    {
        Debug.Log("IsNinaStupid");
        return "No, Nina is super cool and smart!";
    }
}