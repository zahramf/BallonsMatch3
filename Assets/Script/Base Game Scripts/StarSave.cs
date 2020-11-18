using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Star_Save_Data")]
public class StarSave : ScriptableObject
{
    public Levelclass[] LEVEL;

}
[System.Serializable]
public class Levelclass
{
    public string NameLevel;
    public int StarNumber;
}