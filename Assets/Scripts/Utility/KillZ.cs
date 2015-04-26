using UnityEngine;
using System.Collections;

public class KillZ : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == GameManager.Player.tag)
        {
            GameManager.EndGame(0);
        }
    }
}