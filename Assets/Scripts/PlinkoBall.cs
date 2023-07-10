using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlinkoBall : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        PlinkoGame.Instance.BallHit();
    }
}
