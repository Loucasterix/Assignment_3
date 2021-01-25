using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float m_speed = 1f;

    public GameObject gameOverText;
    public GameObject winText;

    private Rigidbody m_playerRigidbody;
    private float m_movementX;
    private float m_movementY;
    private int m_collectablesTotalCount, m_collectablesCounter;
    private Stopwatch m_stopwatch;


    private void Start()
    {
        gameOverText.SetActive(false);
        winText.SetActive(false);

        m_playerRigidbody = GetComponent<Rigidbody>();

        m_collectablesTotalCount = m_collectablesCounter = GameObject.FindGameObjectsWithTag("Collectable").Length;

        m_stopwatch = Stopwatch.StartNew();

    }

    private void OnMove(InputValue inputValue)
    {
        Vector2 movementVector = inputValue.Get<Vector2>();

        m_movementX = movementVector.x;
        m_movementY = movementVector.y;


    }

    private void  FixedUpdate()
    {
       
        Vector3 movement = new Vector3(m_movementX, 0f, m_movementY);

        m_playerRigidbody.AddForce(movement * m_speed);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Collectable"))
        {
            other.gameObject.SetActive(false);

            m_collectablesCounter--;

            if (m_collectablesCounter == 0)
            {
                UnityEngine.Debug.Log("YOU WIN!");
                winText.SetActive(true);
                StartCoroutine(afterWin());

                UnityEngine.Debug.Log($"It took you {m_stopwatch.Elapsed} to find all {m_collectablesTotalCount} collectables.");

            }
            else
            {
                UnityEngine.Debug.Log($"You've already found {m_collectablesTotalCount - m_collectablesCounter} of {m_collectablesTotalCount} collectables! ");
            }

        }
        else if(other.gameObject.CompareTag("Enemy"))
        {
            UnityEngine.Debug.Log("GAME OVER!");
            StartCoroutine(afterLose());
        }
    }

    public IEnumerator afterWin()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("Level 2");
    }

    public IEnumerator afterLose()
    {
        gameOverText.SetActive(true);
        yield return new WaitForSeconds(5);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#endif
    }

}
 