using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class SeaPlane : MonoBehaviour
{
    public static SeaPlane Instance;

    Material waveMaterial;

    public float waveSpeed;
    public float waveStrength;


    private void Awake()
    {
        if (Instance != null) { Debug.LogError("Two SeaPlane instances in scene?"); }
        Instance = this;

        // Get parameters from the shader
        waveMaterial = GetComponent<MeshRenderer>().material;
        waveSpeed = waveMaterial.GetFloat("_Wave_Speed");
        waveStrength = waveMaterial.GetFloat("_Wave_Strength");
    }



    public float SampleHeight(Vector3 position)
    {
        //float height = Mathf.Sin(((position.x) + (position.z)) + (Time.time * waveSpeed)) * waveStrength;
        //return transform.position.y + (height * transform.localScale.y);

        float displacement = ((position.x) + (position.z)) + (Time.time * waveSpeed);
        displacement = Mathf.Sin(displacement);
        displacement *= waveStrength;
        return transform.position.y + displacement;
    }
}
