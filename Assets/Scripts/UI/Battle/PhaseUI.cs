using System;
using BattleSystem;
using Common.Events;
using Common.Events.UserInterface;
using UnityEngine;
using Views.Battle;

namespace UI.Battle
{
    public class PhaseUI : MonoBehaviour , IPhaseView
    {
        [SerializeField] private GameObject _normal;
        [SerializeField] private GameObject _ethered;
        public void OnPhaseSelected(PhaseEventData data)
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