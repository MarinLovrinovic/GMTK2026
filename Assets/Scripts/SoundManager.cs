using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource[] sources;

    public void playSound(int idx)
    {
        //sources[idx].Play();
    }

    public void stopSound(int idx)
    {
        //sources[idx].Stop();
    }
}
