using UnityEngine;
using FMOD.Studio;

public class LevelMusic : MonoBehaviour
{
    [FMODUnity.EventRef] private string musicEvent = "event:/music/Music";

    private string musicIntenseParam = "musiek_intensiteit";
    private string musicHealthParam = "health_param";

    private EventInstance musicInstance;

    private ParameterInstance paramMusicIntense;
    private ParameterInstance paramMusicHealth;

    private readonly float tenionMultiplier = 2f;

    private float healthValue, tentionCounter;

    public float HealthValue
    {
        set { healthValue = value; }
    }

    public float StartMainMusic
    {
        set { if (tentionCounter < 0.1f && value == 0.1f) tentionCounter = value; }
    }
    
    public float StartBossMusic
    {
        set { if (value == 1f) tentionCounter = value; }
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
        if(tentionCounter >= 0.1f || tentionCounter < 0.9f) tentionCounter += Time.deltaTime * tenionMultiplier;
        paramMusicIntense.setValue(tentionCounter);
    }
}