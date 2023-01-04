using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{


    [HideInInspector]
    public AudioSource source;

    public string name;
    public AudioClip clip;

    [Range (0f,1f)]
    public float volume;
    [Range(.1f,3f)]
    public float pitch;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
