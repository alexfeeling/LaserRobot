using Robot;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    public enum GameStatus
    {
        NotStart,
        Playing,
        Over,
        Win,
    }

    public GameStatus Status = GameStatus.NotStart;

    public Transform P_Floor;
    public GameStartUI StartUI;

    public Transform SceneNode;

    public Transform PlayerRobot;

    public Transform CameraControll;

    public int StartLevel = 1;

    void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        BuildMap();

        Status = GameStatus.NotStart;

        StartUI.gameObject.SetActive(true);
        StartUI.SetStart();
    }

    // Update is called once per frame
    void Update()
    {

        CameraControll.position = PlayerRobot.position;
    }

    private void BuildMap()
    {
        //BuildFloor();
    }

    private void BuildFloor()
    {
        var left = -20;
        var right = 20;
        var up = 20;
        var down = -20;
        for (int i = left; i <= right; i++)
        {
            for (int j = down; j <= up; j++)
            {
                var floor = Instantiate(P_Floor, SceneNode);
                floor.position = new Vector3(i, 0, j);
                floor.name = "Floor_" + i + "_" + j;
            }
        }
    }

    public LevelGroup CurrLevelGroup;
    private void LoadLevel(int level)
    {
        var levelPrefab = Resources.Load<Transform>("LevelGroup/LevelGroup_" + level);
        if (levelPrefab == null)
        {
            //通关
            GameWin();
            return;
        }
        if (CurrLevelGroup != null)
        {
            Destroy(CurrLevelGroup.gameObject);
        }
        var lg = Instantiate(levelPrefab);
        CurrLevelGroup = lg.GetComponent<LevelGroup>();
        var playerBorn = lg.GetComponentInChildren<PlayerBornPoint>();
        PlayerRobot.GetComponent<Robot.RobotCharacterController>().Teleport(
            playerBorn.transform.position, playerBorn.transform.rotation);

        Destroy(playerBorn.gameObject);

    }

    public void GameStart()
    {
        Status = GameStatus.Playing;
        AudioManager.Insatnce.PlayMusic(AudioManager.Insatnce.MusicGame);
        PlayerRobot.GetComponent<RobotPlayer>().ResetPlayer();
        LoadLevel(StartLevel);
    }

    public void LevelSucc()
    {
        var nextLv = CurrLevelGroup.level + 1;
        LoadLevel(nextLv);
    }

    public void GameOver()
    {
        Status = GameStatus.Over;
        StartUI.gameObject.SetActive(true);
        StartUI.SetOver();
        PlayerRobot.GetComponent<RobotCharacterController>().StopMove();

    }

    public void GameWin()
    {
        Status = GameStatus.Win;
        StartUI.gameObject.SetActive(true);
        StartUI.SetWin();

        
    }

    public void Exit()
    {
        Application.Quit();
    }

}
