using System.Collections;
using UnityEngine;

//public class WavesFPSHudView : WavesFPSGameViewBase
//{
//    public GUIText _KillsRemainingLabel;

//    public GUIText _WaveStartLabel;

//    public override void WaveKillsChanged(int kills)
//    {
//        _KillsRemainingLabel.text = string.Format("{0}/{1} Remaining", WavesFPSGame.KillsToNextWave - kills, WavesFPSGame.KillsToNextWave);
//    }

//    public override void CurrentWaveChanged(int wave)
//    {
//        _WaveStartLabel.gameObject.SetActive(true);
//        _WaveStartLabel.text = string.Format("Wave {0} started!", wave);
//        StartCoroutine(HideLabelInSeconds());
//        WaveKillsChanged(WavesFPSGame.WaveKills);
//    }

//    public override void KillsToNextWaveChanged(int value)
//    {
//        base.KillsToNextWaveChanged(value);
//        CurrentWaveChanged(WavesFPSGame.CurrentWave);
//    }

//    public IEnumerator HideLabelInSeconds(float seconds = 3f)
//    {
//        yield return new WaitForSeconds(seconds);
//        _WaveStartLabel.gameObject.SetActive(false);
//    }


//    public override void KillsChanged(int value)
//    {
        
//    }
//}