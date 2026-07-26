using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource[] sources;

    private void Awake()
    {
        startTrack();
    }

    public void startTrack()
    {
        double startTime = AudioSettings.dspTime;

        sources[8].PlayScheduled(startTime);
        sources[6].PlayScheduled(startTime + sources[8].clip.length);
    }

    public void stopTrack()
    {
        sources[6].Stop();
        sources[7].Play();
    }

    public void playSound(int idx)
    {
        //sources[idx].Play();
    }

    public void stopSound(int idx)
    {
        //sources[idx].Stop();
    }
}
