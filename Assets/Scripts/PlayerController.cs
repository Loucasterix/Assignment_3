using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float m_speed = 1f;

    private Rigidbody m_playerRigidbody;

    private float m_movementX;
    private float m_movementY;

    private int m_collectablesTotalCount, m_collectablesCounter;

    private Stopwatch m_stopwatch;

    public Text scoreText;

    public GameObject gameOverText;


    private void Start()
    {
        m_playerRigidbody = GetComponent<Rigidbody>();

        m_collectablesTotalCount = m_collectablesCounter = GameObject.FindGameObjectsWithTag("Collectable").Length;

        scoreText.text = "Score: " + m_collectablesCounter.ToString() + " / " + m_collectablesTotalCount.ToString();

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
            scoreText.text = "Score: " + m_collectablesCounter.ToString() + " / " + m_collectablesTotalCount.ToString();

            if (m_collectablesCounter == 0)
            {
                UnityEngine.Debug.Log("YOU WIN!");
                gameOverText.SetActive(true);
                StartCoroutine(waitALittleBit());

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
            gameOverText.SetActive(true);
#if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
#endif
        }
    }

    public IEnumerator waitALittleBit()
    {
        yield return new WaitForSeconds(5);
#if UNITY_EDITOR
                UnityEditor.EditorApplication.ExitPlaymode();
#endif
    }
}
 