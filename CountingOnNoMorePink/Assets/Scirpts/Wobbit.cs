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
        loadingScreen = GetComponent<LoadingScreen>();

        playerMovement.onTakeDamage += TakeDamage;

        slowSnapShot = FMODUnity.RuntimeManager.CreateInstance("snapshot:/TimeSlow");
        slowSnapShot.start();
    }


    public void GoToMenu()
    {
        GetComponent<BeatTimeline>().saveFileDropdown.StoreSongIndex();

        loadingScreen.BeginLoad("MainMenu");
     
    }

    public void GoToEditor()
    {
        GetComponent<BeatTimeline>().saveFileDropdown.StoreSongIndex();
        loadingScreen.BeginLoad("JulesUIBreaking");
    }

    //very temporary references to junk I need for the demo

    public Bullet bulletFab;
    public BoomBlock zoneFab;
    public Transform bossOrigin;
    public Boss boss;
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

    //scene transition manager
    public LoadingScreen loadingScreen;

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
        FreeFormOrbitalMove playerController = player.GetComponent<FreeFormOrbitalMove>();
        //do not win if you are not win
        if (playerController.IsAlive())
        {
            playerController.SetDamageEnabled(false);
            resultScreen.Activate();
        }
    }

    public void Restart()
    {
        loadingScreen.BeginLoad(SceneManager.GetActiveScene().name);
        persistentData.currentSongRestarts += 1;
    }

    public void MainMenu()
    {
        
        playerMovement.currentHP = playerMovement.maxHP;
        playerMovement.onHealthChanged?.Invoke();
        loadingScreen.BeginLoad("MainMenu");
    }

    public void CreateCountDownIndicator(int beats)
    {
        if (parryIndicatorPrefab == null)
            return;

        ParryIndicator indicator = Instantiate(parryIndicatorPrefab, player.position,Quaternion.identity);
        indicator.Setup(beats,player);
    }
}
