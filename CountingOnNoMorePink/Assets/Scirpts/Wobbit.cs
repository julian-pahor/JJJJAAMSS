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

    //BOY WAS I WRONG THESE ARE NOW INTEGRAL TO THE FUNCTION OF THE GAME

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
    public Transform anchor1;
    public Transform anchor2;

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

    public bool paused = false;

    
    //TimeSlow stuff testing 

    public FMOD.Studio.EventInstance slowSnapShot;
    public FreeFormOrbitalMove playerMovement;
    private float timeScale = 1;
    private float targetScale = 1;


    public GameOverscreen gameOverScreen;
    public ResultsScreen resultScreen;

    //scene transition manager
    public LoadingScreen loadingScreen;

    //highscoreSave
    public SongScoreSaver songScoreSaver;

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
        BeatBroadcast.instance.StopMusic();
        poolPool.ClearBullets();
        GetComponent<BeatTimeline>().saveFileDropdown.StoreSongIndex();
        loadingScreen.BeginLoad("FinalMainMenu");

    }

    public void GoToEditor()
    {
        BeatBroadcast.instance.StopMusic();
        poolPool.ClearBullets();
        GetComponent<BeatTimeline>().saveFileDropdown.StoreSongIndex();
        loadingScreen.BeginLoad("FinalTimelineEditor");
    }

    public void TakeDamage()
    {
        targetScale = 0;
        persistentData.currentSongHits += 1;
    }

    private void Update()
    {
        if(!paused)
        {
            timeScale = Mathf.Lerp(timeScale, targetScale, Time.deltaTime * 4.75f);
            Time.timeScale = timeScale;
            FMODUnity.RuntimeManager.StudioSystem.setParameterByName("TimeSlow", timeScale);
        }
        
        

        targetScale += Time.deltaTime * 4.5f;
        targetScale = Mathf.Clamp01(targetScale);

        
    }
    
    public void EndGame()
    {
        BeatBroadcast.instance.StopMusic();
        poolPool.ClearBullets();
        StartCoroutine(GameOverScreen());
    }


    IEnumerator GameOverScreen()
    {
        yield return new WaitForSeconds(2f);
        gameOverScreen.Activate();
    }


    public void FinishSong()
    {
        FreeFormOrbitalMove playerController = player.GetComponent<FreeFormOrbitalMove>();
        //do not win if you are not win
        if (playerController.IsAlive())
        {
            //boss.animator.Play("Dying", 0, 0f); //Call from animation manager instead
            playerController.SetDamageEnabled(false);
            resultScreen.Activate();
        }

        SongScoreData data = resultScreen.GetScoreData();

        songScoreSaver.SaveSongScore(timeline.saveFileDropdown.SongNameFromIndex(persistentData.songIndex), data);
       
    }

    public void Restart()
    {
        BeatBroadcast.instance.StopMusic();
        poolPool.ClearBullets();
        loadingScreen.BeginLoad(SceneManager.GetActiveScene().name);
        persistentData.currentSongRestarts += 1;
    }

    //public void MainMenu()
    //{
        
    //    playerMovement.currentHP = playerMovement.maxHP;
    //    playerMovement.onHealthChanged?.Invoke();
    //    loadingScreen.BeginLoad("MainMenu");
    //}

    public void CreateCountDownIndicator(int beats)
    {
        if (parryIndicatorPrefab == null)
            return;

        ParryIndicator indicator = Instantiate(parryIndicatorPrefab, player.position,Quaternion.identity);
        indicator.Setup(beats,player);
    }
}
