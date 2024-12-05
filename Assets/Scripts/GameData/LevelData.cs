using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/LevelData")]
class LevelData : ScriptableObject
{
    public List<Stage> stages;
    public float time;
    public int totalCoin;
    public AudioClip bgm;
    //public GameObject clearReward;
}