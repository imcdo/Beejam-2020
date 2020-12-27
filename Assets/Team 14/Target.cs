using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team14
{
    [RequireComponent(typeof(Collider2D))]
    public class Target : MonoBehaviour
    {
        public bool IsWanted = false;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Pie pie = collision.GetComponent<Pie>();   
            
            if (pie)
            {
                if (IsWanted)
                {
                    // WIN!!!
                    MinigameManager.Instance.minigame.gameWin = true;
                    pie.Reset();

                }
                else
                {
                    pie.Reset();
                }
            }
        }
    }
}
