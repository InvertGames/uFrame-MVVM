using UnityEngine;
using System.Collections;

/*
 * This class holds information about level. We have made it monobehavior,
 * since we wanted to introduce an example of in-game db based on monobehaviours.
 * If you use another source of data, you can simply remove the monobeh inharitance.
 */
//Remove monobehaviour inheritance to use LevelDescriptor outside of unity
public class LevelDescriptor : MonoBehaviour
{
    public int Id;
    public string Title;
    public string Description;
    public string LevelScene;
    public bool IsLocked;
}
