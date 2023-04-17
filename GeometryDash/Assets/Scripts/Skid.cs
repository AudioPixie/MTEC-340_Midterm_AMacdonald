using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skid : MonoBehaviour
{
    public Player Player;
    public Transform pt;

    void FixedUpdate()
    {
        if (Player.mode == "Regular" || (Player.mode == "Switch" && Player.gravTemp == 1))
        {
            transform.rotation = Quaternion.Euler(-90, 0, 0);
        }

        if (Player.mode == "UpsideDown" || (Player.mode == "Switch" && Player.gravTemp == -1))
        {
            transform.rotation = Quaternion.Euler(-270, 0, 0);
        }
    }
}
