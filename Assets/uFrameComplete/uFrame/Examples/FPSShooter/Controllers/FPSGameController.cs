using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

public class FPSGameController : FPSGameControllerBase
{
    public int _NumberOfEnemiesAtStart = 2;
    public int _NumberOfEnemiesMultiplier = 2;
    public float _SpawnWait = 5f;
    public float _SpawnWaitMultiplier = 0.9f;

    public virtual void EndGame()
    {
        FPSGame.State = FPSGameState.Done;
    }

    public virtual IEnumerator StartGame()
    {
        FPSGame.State = FPSGameState.Active;
        yield break;
    }

    protected void SpawnEnemy()
    {
        var enemy = FPSEnemyController.CreateFPSEnemy();
        this.SubscribeToProperty(enemy,enemy._StateProperty, (e, state) =>
        {
            if (e.State == FPSPlayerState.Dead)
                EnemyDied(enemy);
        });
        FPSGame.Enemies.Add(enemy);
    }

    public virtual void EnemyDied(FPSEnemyViewModel enemy)
    {
        FPSGame.Kills++;
        FPSGame.Enemies.Remove(enemy);
    }

    public override void InitializeFPSGame(FPSGameViewModel fPSGame)
    {
        UnityEngine.Debug.Log("FPSGame:" +fPSGame.GetHashCode().ToString());
        UnityEngine.Debug.Log("Weapon Count" + fPSGame.CurrentPlayer.Weapons.Count);
    }



    public override void MainMenu()
    {
        
    }

}