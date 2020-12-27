using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Random = System.Random;

namespace Team14
{
    [CreateAssetMenu(menuName="PieHunter/TargetConfig")]
    public class TargetConfigs : ScriptableObject
    {
        public Sprite[] headwear;
    }

    public class TargetGenerator : MonoBehaviour
    {
        public Transform[] TargetSpawns;
        
        private Target[] _targets;
        [SerializeField] private Target _targetPrefab;
        [SerializeField] private TargetConfigs _targetConfig;


        private void Start()
        {
            GenerateTargets();
        }

        public void GenerateTargets()
        {
            Random rand = new Random();
            var headgear = _targetConfig.headwear
                .OrderBy(x => rand.Next()).Take(TargetSpawns.Length).ToList();
            

            if (_targets != null)
            {
                foreach (Target t in _targets)
                    Destroy(t.gameObject);
            }
            _targets = new Target[TargetSpawns.Length];
            
            for (int i = 0; i < TargetSpawns.Length; i++)
            {
                Transform spawn = TargetSpawns[i];
                _targets[i] = Instantiate(_targetPrefab, spawn);
                _targets[i].Generate(headgear[i]);
            }
            Target wanted = _targets[rand.Next(0, TargetSpawns.Length)];
            wanted.IsWanted = true;

        }

    }
}
