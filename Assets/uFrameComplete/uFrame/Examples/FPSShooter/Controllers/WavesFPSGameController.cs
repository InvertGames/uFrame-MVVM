using System.Collections;
using UnityEngine;

public class WavesFPSGameController : WavesFPSGameControllerBase
{
    public float _WaveDifficultyMultiplier = 0.15f;

  
    public void ApplyDamage(FPSGameViewModel gameViewModel, FPSEnemyViewModel enemy)
    {
        var player = gameViewModel.CurrentPlayer;
        player.Health -= (WavesFPSGame.CurrentWave * _WaveDifficultyMultiplier);
        if (player.Health <= 0f)
        {
            EndGame();
        }
    }

    private int _EnemiesSpawned = 0;

    public override IEnumerator StartGame()
    {
        WavesFPSGame.State = FPSGameState.Active;
        //base.StartGame();
        yield return StartCoroutine(SpawnEnemies());
    }

    public IEnumerator NextWave()
    {
        WavesFPSGame.WaveKills = 0;
        WavesFPSGame.CurrentWave++;

        WavesFPSGame.KillsToNextWave += _NumberOfEnemiesMultiplier;
        _SpawnWait *= _SpawnWaitMultiplier;
        _EnemiesSpawned = 0;
        yield return StartCoroutine(SpawnEnemies());
    }

    public IEnumerator SpawnEnemies()
    {
        while (_EnemiesSpawned < WavesFPSGame.KillsToNextWave)
        {
            yield return new WaitForSeconds(_SpawnWait);    
            SpawnEnemy();
            _EnemiesSpawned++;
        }
    }

    public override void EnemyDied(FPSEnemyViewModel enemy)
    {
        base.EnemyDied(enemy);
        WavesFPSGame.WaveKills++;
        if (WavesFPSGame.KillsToNextWave == WavesFPSGame.WaveKills)
        {
            StartCoroutine(NextWave());
        }
    }

    public override void InitializeWavesFPSGame(WavesFPSGameViewModel wavesFPSGame)
    {
        // Additionaly call the base initialize
        InitializeFPSGame(wavesFPSGame);
        wavesFPSGame.CurrentPlayer = FPSPlayerController.CreateFPSPlayer();
        wavesFPSGame.CurrentWave = 1;
        wavesFPSGame.KillsToNextWave = _NumberOfEnemiesAtStart;
    }
}