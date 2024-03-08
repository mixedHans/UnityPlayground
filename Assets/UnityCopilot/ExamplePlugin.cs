using System.ComponentModel;
using UnityEngine;

public class ExamplePlugin : PluginMonoBehaviour
{
    private GameObject cube = null;

    [UnityFunction]
    [Description("Spawns a cube gameobject")]
    public void SpawnCubeGameobject()
    {
        Debug.Log("SpawnCubeGameobject invoked");
        cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
    }

    [UnityFunction]
    [Description("Delete the cube gameobject")]
    public void DestroyCubeGameobject()
    {
        if (cube == null)
        {
            throw new System.Exception("Can't delete cube, cause the instance of the cube is null");
        }

        Destroy(cube);
        Debug.Log("DestroyCubeGameobject invoked");
    }
}

public class PluginMonoBehaviour : MonoBehaviour
{
}