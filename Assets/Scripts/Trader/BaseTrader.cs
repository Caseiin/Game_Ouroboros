using UnityEngine;

public class BaseTrader : MonoBehaviour
{
    public bool PlayerDetect() //transition check to the combat state;
    {
        bool Detect = Physics2D.OverlapCircle(transform.position, 3f, LayerMask.GetMask("Player"));
        return Detect;
    }
}
