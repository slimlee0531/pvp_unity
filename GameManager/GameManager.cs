using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Singleton;

    [SerializeField]
    public MatchingSettings MatchingSettings;

    private static Dictionary<string, Player> players = new Dictionary<string, Player>();

    private void Awake()
    {
        Singleton = this;
    }

    public void RegisterPlayer(string playerName, Player player)
    {
        player.transform.name = playerName;
        players.Add(playerName, player);
    }

    public void UnRegisterPlayer(string name, Player player)
    {
        players.Remove(name);
    }

    public Player GetPlayer(string name)
    {
        return players[name];
    }

    private static string info;

    public static void UpdateInfo(string _info)
    {
        info = _info;
    }

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(200f, 200f, 200f, 400f));//x, y, height, width
        GUILayout.BeginVertical();

        GUI.color = Color.green;
        GUILayout.Label(info);

        foreach(string name in players.Keys)
        {
            Player player = GetPlayer(name);
            GUI.color = Color.red;
            GUILayout.Label(name + "--" + player.GetHealth());
        }

        GUILayout.EndVertical();
        GUILayout.EndArea();
    }
}
