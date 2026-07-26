using System;
using UnityEngine;

public class FollowWave : MonoBehaviour
{
    [SerializeField] private Transform rootTransform;
    [SerializeField] private Transform waveifyTransform;
    
    [Space(10)]
    [SerializeField] private float WaveSpeed;
    [SerializeField] private float WaveStrength;

    float dajVisinu(Vector3 tocka)
    {
        float displacement = (tocka.x + tocka.z) + (Time.time * WaveSpeed);
        displacement = (float)Math.Sin(displacement);
        displacement *= WaveStrength;
        return displacement;
    }

    void Update()
    {
        Vector3 rootPose = rootTransform.position;
        Vector3 targetPose = rootPose;
        
        targetPose.y += dajVisinu(rootPose);

        // Normal
        float e = 0.1f;
        float hL = dajVisinu(rootPose + Vector3.left * e);    // x - e
        float hR = dajVisinu(rootPose + Vector3.right * e);    // x + e
        float hB = dajVisinu(rootPose + Vector3.back * e);    // z - e
        float hF = dajVisinu(rootPose + Vector3.forward * e);    // z + e
        Vector3 normal = new Vector3(
            hL - hR,
            2f * e,
            hB - hF
        ).normalized;
        waveifyTransform.rotation = Quaternion.LookRotation(waveifyTransform.forward, normal);


        waveifyTransform.position = targetPose;
    }
}
