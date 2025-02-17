using System;
using BattleSystem;
using Common.Events;
using UnityEngine;

namespace UI.Battle
{
    public class PhaseUI : MonoBehaviour
    {
        [SerializeField] private GameObject _normal;
        [SerializeField] private GameObject _ethered;
        public void DisplayPhase(PhaseEventData data)
        {
            if(data.phase == EPhase.Normal)
            {
                _normal.SetActive(true);
                _ethered.SetActive(false);
            }
            else
            {
                _normal.SetActive(false);
                _ethered.SetActive(true);
            }
        }
    }
}