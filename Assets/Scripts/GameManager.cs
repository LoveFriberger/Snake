using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static Vector2Int boardSize = new Vector2Int(32, 18);
    public static bool running;

    public void Start()
    {
        StartGame += () => { running = true; };
        Win += () => { running = false; };
        Lose += () => { running = false; };
        DontDestroyOnLoad(this);
    }

    public static Action StartGame;
    public static Action Score;
    public static Action Win;
    public static Action Lose;
    public static Action ClearBoard;
}


