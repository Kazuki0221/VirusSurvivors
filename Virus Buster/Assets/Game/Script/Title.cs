using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class Title : MonoBehaviour
{
    //[SerializeField] Image fade;
    //int alpaha = 0;
    void Start()
    {
        //fade.color = new Color(0, 0, 0, alpaha);
    }


    public void StartGame()
    {
        SceneManager.LoadScene("Main");
    }
}
