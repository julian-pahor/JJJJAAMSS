using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Wobbit : MonoBehaviour
{
    public static Wobbit instance;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("oh no another wobbit");
            return;
        }
        instance = this;
    }

    private void Start()
    {
        timeline = FindObjectOfType<BeatTimeline>();

        playerMovement.onTakeDamage += TakeDamage;

        slowSnapShot = FMODUnity.RuntimeManager.CreateInstance("snapshot:/TimeSlow");
        slowSnapShot.start();
    }


    public void GoToEditor()
    {
        GetComponent<BeatTimeline>().saveFileDropdown.StoreSongIndex();
        SceneManager.LoadScene("JulesUIBreaking");
    }

    //very temporary references to junk I need for the demo

    public Bullet bulletFab;
    public BoomBlock zoneFab;
    public Transform bossOrigin;
    public Transform player;
    public GameObject warning;
    public GameObject pink;

    public LineTracer lineTracer;

    public Tracer tracer;
    public GameObject tracerWindup;
    public Transform hand1;
    public Transform hand2;

    public Parry playerParry;

    public Orbiter orbiterPrefab;

    public BeatTimeline timeline;

    public DelayedDangerZone delayedDangerZoneTest;
    public DelayedDangerZone seekerTest;

    public ParryAttack2 pa2;

    public ParryIndicator parryIndicatorPrefab;

    public TextMeshProUGUI numberwang;

    public PoolPool poolPool;

    public PersistentData persistentData;


    //TimeSlow stuff testing 

    public FMOD.Studio.EventInstance slowSnapShot;
    public FreeFormOrbitalMove playerMovement;
    private float timeScale = 1;
    private float targetScale = 1;


    public GameOverscreen gameOverScreen;
    public ResultsScreen resultScreen;
    
    public void TakeDamage()
    {
        targetScale = 0;
        persistentData.currentSongHits += 1;
    }

    private void Update()
    {
        
        timeScale = Mathf.Lerp(timeScale, targetScale, Time.deltaTime * 4.75f);
        Time.timeScale = timeScale;
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("TimeSlow", timeScale);
        

        targetScale += Time.deltaTime * 4.5f;
        targetScale = Mathf.Clamp01(targetScale);

        
    }
    
    public void EndGame()
    {
        gameOverScreen.Activate();
    }

    public void FinishSong()
    {
        resultScreen.Activate();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        persistentData.currentSongRestarts += 1;
    }

    public void MainMenu()
    {
        
        playerMovement.currentHP = playerMovement.maxHP;
        playerMovement.onHealthChanged?.Invoke();
        SceneManager.LoadScene("MainMenu");
    }

    public void CreateCountDownIndicator(int beats)
    {
        if (parryIndicatorPrefab == null)
            return;

        ParryIndicator indicator = Instantiate(parryIndicatorPrefab, player.position,Quaternion.identity);
        indicator.Setup(beats,player);
    }
}
