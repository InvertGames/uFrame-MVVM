using System.Collections;
using UnityEngine;

public class WavesFPSGameController : WavesFPSGameControllerBase
{
    public float _WaveDifficultyMultiplier = 0.15f;

    public WavesFPSGameViewModel WavesGame
    {
        get { return FPSGame as WavesFPSGameViewModel; }
    }
  
    public void ApplyDamage(FPSGameViewModel gameViewModel, FPSEnemyViewModel enemy)
    {
        var player = gameViewModel.CurrentPlayer;
        player.Health -= (WavesGame.CurrentWave * _WaveDifficultyMultiplier);
        if (player.Health <= 0f)
        {
            EndGame();
        }
    }

    private int _EnemiesSpawned = 0;

    public WavesFPSGameController()
    {
    }

    public WavesFPSGameController(SceneContext context)
    {
        Context = context;
    }

    public override IEnumerator StartGame()
    {
        FPSGame.State = FPSGameState.Active;
        //base.StartGame();
        yield return StartCoroutine(SpawnEnemies());
    }

    public IEnumerator NextWave()
    {
        WavesGame.WaveKills = 0;
        WavesGame.CurrentWave++;

        WavesGame.KillsToNextWave += _NumberOfEnemiesMultiplier;
        _SpawnWait *= _SpawnWaitMultiplier;
        _EnemiesSpawned = 0;
        yield return StartCoroutine(SpawnEnemies());
    }

    public IEnumerator SpawnEnemies()
    {
        while (_EnemiesSpawned < WavesGame.KillsToNextWave)
        {
            yield return new WaitForSeconds(_SpawnWait);    
            SpawnEnemy();
            _EnemiesSpawned++;
        }
    }

    public override void EnemyDied(FPSEnemyViewModel enemy)
    {
        base.EnemyDied(enemy);
        WavesGame.WaveKills++;
        if (WavesGame.KillsToNextWave == WavesGame.WaveKills)
        {
            StartCoroutine(NextWave());
        }
    }

    public override void InitializeWavesFPSGame(WavesFPSGameViewModel wavesFPSGame)
    {
        
        // Additionaly call the base initialize
        //wavesFPSGame.CurrentPlayer = FPSPlayerController.CreateFPSPlayer();
        wavesFPSGame.CurrentWave = 1;
        wavesFPSGame.KillsToNextWave = _NumberOfEnemiesAtStart;
    }
}