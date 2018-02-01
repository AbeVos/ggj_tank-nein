using UnityEngine;
using FMOD.Studio;

public class LevelMusic : MonoBehaviour
{
    [FMODUnity.EventRef] private string musicEvent = "event:/music/Music";

    private string musicIntenseParam = "music_intensiteit";
    private string musicHealthParam = "health_param";

    private EventInstance musicInstance;

    private ParameterInstance paramMusicIntense;
    private ParameterInstance paramMusicHealth;

    private bool startthething = false;
    private readonly float tenionMultiplier = 2f;

    private float healthValue, tentionCounter;

    public float HealthValue
    {
        set { healthValue = value; }
    }

    public float StartMainMusic
    {
        set
        {
            if (tentionCounter < 0.1f) tentionCounter = value;
            startthething = true;
        }
    }
    
    public float StartBossMusic
    {
        set { tentionCounter = value; }
    }


    private void OnEnable()
    {
        musicInstance = FMODUnity.RuntimeManager.CreateInstance(musicEvent);
        musicInstance.getParameter(musicIntenseParam, out paramMusicIntense);
        musicInstance.getParameter(musicHealthParam, out paramMusicHealth);

        musicInstance.start();
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        paramMusicHealth.setValue(healthValue);
        if((tentionCounter >= 0.1f || tentionCounter < 0.8f) && startthething) tentionCounter += Time.deltaTime * tenionMultiplier;
        paramMusicIntense.setValue(tentionCounter);
    }
}