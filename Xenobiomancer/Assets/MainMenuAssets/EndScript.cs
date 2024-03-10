using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class EndScript : MonoBehaviour
{
    [SerializeField] protected Animator animator;
    private void Start()
    {
        animator.Play("CreditsPlay");
    }
    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
