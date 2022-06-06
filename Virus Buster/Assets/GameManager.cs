using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager
{
    static private GameManager instance = new GameManager();
    static public GameManager Instance => instance;
    private GameManager() { }

    Player player = null;
    static public Player Player => instance.player;
    public void SetPlayer(Player p) { player = p; }
    Heart heart = null;
    static public Heart Heart => instance.heart;
    public void SetHeart(Heart h) { heart = h; }

    List<Enemy> enemies = new List<Enemy>();
    static public List<Enemy> EnemyList => instance.enemies;
    public void SetList()
    {
        enemies = GameObject.FindObjectsOfType<Enemy>(true).ToList();
    }

}
