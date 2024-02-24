using UnityEngine;
using Microsoft.SemanticKernel;
using System.ComponentModel;

public class LeonsPlugin
{
    [KernelFunction()]
    [Description("Answers to a ping request")]
    public string AwnserPingWithPong()
    {
        Debug.Log("AwnserPingWithPong invoked");
        return "pong";
    }

    //[KernelFunction()]
    //[Description("Spawns a gameobject")]
    //public void SpawnGameobject()
    //{
    //    Debug.Log("SpawnGameobject invoked");
    //    GameObject.CreatePrimitive(PrimitiveType.Capsule);
    //}

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
}