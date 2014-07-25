using System;
using System.Collections;
using System.Linq;


[DiagramInfoAttribute("FPSShooter")]
public partial class FPSDamageableViewModel : ViewModel {
    
    public readonly P<System.Single> _HealthProperty = new P<float>();
    
    public readonly P<FPSPlayerState> _StateProperty = new P<FPSPlayerState>();
    
    private ICommand _ApplyDamage;
    
    public virtual float Health {
        get {
            return _HealthProperty.Value;
        }
        set {
            _HealthProperty.Value = value;
        }
    }
    
    public virtual FPSPlayerState State {
        get {
            return _StateProperty.Value;
        }
        set {
            _StateProperty.Value = value;
        }
    }
    
    public virtual ICommand ApplyDamage {
        get {
            return _ApplyDamage;
        }
        set {
            _ApplyDamage = value;
        }
    }
    
    protected override void WireCommands(Controller controller) {
        var fPSDamageable = controller as FPSDamageableControllerBase;
        this.ApplyDamage = new CommandWithSenderAndArgument<FPSDamageableViewModel, int>(this, fPSDamageable.ApplyDamage);
    }
    
    public override void Write(ISerializerStream stream) {
		base.Write(stream);
		stream.SerializeFloat("Health", this.Health);
		stream.SerializeInt("State", (int)this.State);
    }
    
    public override void Read(ISerializerStream stream) {
		base.Read(stream);
		this.Health = stream.DeserializeFloat("Health");
		this.State = (FPSPlayerState)stream.DeserializeInt("State");
    }
}

[DiagramInfoAttribute("FPSShooter")]
public partial class FPSEnemyViewModel : FPSDamageableViewModel {
    
    public readonly P<System.Single> _SpeedProperty = new P<float>();
    
    public virtual float Speed {
        get {
            return _SpeedProperty.Value;
        }
        set {
            _SpeedProperty.Value = value;
        }
    }
    
    protected override void WireCommands(Controller controller) {
        base.WireCommands(controller);
    }
    
    public override void Write(ISerializerStream stream) {
		base.Write(stream);
		stream.SerializeFloat("Speed", this.Speed);
    }
    
    public override void Read(ISerializerStream stream) {
		base.Read(stream);
		this.Speed = stream.DeserializeFloat("Speed");
    }
}

[DiagramInfoAttribute("FPSShooter")]
public partial class FPSGameViewModel : ViewModel {
    
    public readonly P<FPSGameState> _StateProperty = new P<FPSGameState>();
    
    public readonly P<FPSPlayerViewModel> _CurrentPlayerProperty = new P<FPSPlayerViewModel>();
    
    public readonly P<System.Int32> _ScoreProperty = new P<int>();
    
    public readonly P<System.Int32> _KillsProperty = new P<int>();
    
    public readonly ModelCollection<FPSEnemyViewModel> _EnemiesProperty = new ModelCollection<FPSEnemyViewModel>();
    
    private ICommand _MainMenu;
    
    private ICommand _QuitGame;
    
    public virtual FPSGameState State {
        get {
            return _StateProperty.Value;
        }
        set {
            _StateProperty.Value = value;
        }
    }
    
    public virtual FPSPlayerViewModel CurrentPlayer {
        get {
            return _CurrentPlayerProperty.Value;
        }
        set {
            _CurrentPlayerProperty.Value = value;
        }
    }
    
    public virtual int Score {
        get {
            return _ScoreProperty.Value;
        }
        set {
            _ScoreProperty.Value = value;
        }
    }
    
    public virtual int Kills {
        get {
            return _KillsProperty.Value;
        }
        set {
            _KillsProperty.Value = value;
        }
    }
    
    public virtual System.Collections.Generic.ICollection<FPSEnemyViewModel> Enemies {
        get {
            return _EnemiesProperty;
        }
        set {
            _EnemiesProperty.Value = value.ToList();
        }
    }
    
    public virtual ICommand MainMenu {
        get {
            return _MainMenu;
        }
        set {
            _MainMenu = value;
        }
    }
    
    public virtual ICommand QuitGame {
        get {
            return _QuitGame;
        }
        set {
            _QuitGame = value;
        }
    }
    
    protected override void WireCommands(Controller controller) {
        var fPSGame = controller as FPSGameControllerBase;
        this.MainMenu = new Command(fPSGame.MainMenu);
        this.QuitGame = new Command(fPSGame.QuitGame);
    }
    
    public override void Write(ISerializerStream stream) {
		base.Write(stream);
		stream.SerializeInt("State", (int)this.State);
		stream.SerializeObject("CurrentPlayer", this.CurrentPlayer);
		stream.SerializeInt("Score", this.Score);
		stream.SerializeInt("Kills", this.Kills);
		stream.SerializeArray("Enemies", this.Enemies);
    }
    
    public override void Read(ISerializerStream stream) {
		base.Read(stream);
		this.State = (FPSGameState)stream.DeserializeInt("State");
		this.CurrentPlayer = stream.DeserializeObject<FPSPlayerViewModel>("CurrentPlayer");
		this.Score = stream.DeserializeInt("Score");
		this.Kills = stream.DeserializeInt("Kills");
		this.Enemies = stream.DeserializeObjectArray<FPSEnemyViewModel>("Enemies").ToList();
    }
}

[DiagramInfoAttribute("FPSShooter")]
public partial class FPSPlayerViewModel : FPSDamageableViewModel {
    
    public readonly P<System.Int32> _CurrentWeaponIndexProperty = new P<int>();
    
    public readonly ModelCollection<FPSWeaponViewModel> _WeaponsProperty = new ModelCollection<FPSWeaponViewModel>();
    
    private ICommand _PreviousWeapon;
    
    private ICommand _NextWeapon;
    
    private ICommand _PickupWeapon;
    
    private ICommand _SelectWeapon;
    
    public virtual int CurrentWeaponIndex {
        get {
            return _CurrentWeaponIndexProperty.Value;
        }
        set {
            _CurrentWeaponIndexProperty.Value = value;
        }
    }
    
    public virtual System.Collections.Generic.ICollection<FPSWeaponViewModel> Weapons {
        get {
            return _WeaponsProperty;
        }
        set {
            _WeaponsProperty.Value = value.ToList();
        }
    }
    
    public virtual ICommand PreviousWeapon {
        get {
            return _PreviousWeapon;
        }
        set {
            _PreviousWeapon = value;
        }
    }
    
    public virtual ICommand NextWeapon {
        get {
            return _NextWeapon;
        }
        set {
            _NextWeapon = value;
        }
    }
    
    public virtual ICommand PickupWeapon {
        get {
            return _PickupWeapon;
        }
        set {
            _PickupWeapon = value;
        }
    }
    
    public virtual ICommand SelectWeapon {
        get {
            return _SelectWeapon;
        }
        set {
            _SelectWeapon = value;
        }
    }
    
    protected override void WireCommands(Controller controller) {
        base.WireCommands(controller);
        var fPSPlayer = controller as FPSPlayerControllerBase;
        this.PreviousWeapon = new CommandWithSender<FPSPlayerViewModel>(this, fPSPlayer.PreviousWeapon);
        this.NextWeapon = new CommandWithSender<FPSPlayerViewModel>(this, fPSPlayer.NextWeapon);
        this.PickupWeapon = new CommandWithSenderAndArgument<FPSPlayerViewModel, FPSWeaponViewModel>(this, fPSPlayer.PickupWeapon);
        this.SelectWeapon = new CommandWithSenderAndArgument<FPSPlayerViewModel, int>(this, fPSPlayer.SelectWeapon);
    }
    
    public override void Write(ISerializerStream stream) {
		base.Write(stream);
		stream.SerializeInt("CurrentWeaponIndex", this.CurrentWeaponIndex);
		stream.SerializeArray("Weapons", this.Weapons);
    }
    
    public override void Read(ISerializerStream stream) {
		base.Read(stream);
		this.CurrentWeaponIndex = stream.DeserializeInt("CurrentWeaponIndex");
		this.Weapons = stream.DeserializeObjectArray<FPSWeaponViewModel>("Weapons").ToList();
    }
}

[DiagramInfoAttribute("FPSShooter")]
public partial class FPSWeaponViewModel : ViewModel {
    
    public readonly P<System.Int32> _AmmoProperty = new P<int>();
    
    public readonly P<FPSWeaponState> _StateProperty = new P<FPSWeaponState>();
    
    public readonly P<System.Int32> _ZoomIndexProperty = new P<int>();
    
    public readonly P<System.Int32> _MaxZoomsProperty = new P<int>();
    
    public readonly P<WeaponType> _WeaponTypeProperty = new P<WeaponType>();
    
    public readonly P<System.Single> _ReloadTimeProperty = new P<float>();
    
    public readonly P<System.Int32> _RoundSizeProperty = new P<int>();
    
    public readonly P<System.Int32> _MinSpreadProperty = new P<int>();
    
    public readonly P<System.Int32> _BurstSizeProperty = new P<int>();
    
    public readonly P<System.Single> _RecoilSpeedProperty = new P<float>();
    
    public readonly P<System.Single> _FireSpeedProperty = new P<float>();
    
    public readonly P<System.Single> _BurstSpeedProperty = new P<float>();
    
    public readonly P<System.Single> _SpreadMultiplierProperty = new P<float>();
    
    private ICommand _BeginFire;
    
    private ICommand _NextZoom;
    
    private ICommand _Reload;
    
    private ICommand _EndFire;
    
    private ICommand _BulletFired;
    
    public virtual int Ammo {
        get {
            return _AmmoProperty.Value;
        }
        set {
            _AmmoProperty.Value = value;
        }
    }
    
    public virtual FPSWeaponState State {
        get {
            return _StateProperty.Value;
        }
        set {
            _StateProperty.Value = value;
        }
    }
    
    public virtual int ZoomIndex {
        get {
            return _ZoomIndexProperty.Value;
        }
        set {
            _ZoomIndexProperty.Value = value;
        }
    }
    
    public virtual int MaxZooms {
        get {
            return _MaxZoomsProperty.Value;
        }
        set {
            _MaxZoomsProperty.Value = value;
        }
    }
    
    public virtual WeaponType WeaponType {
        get {
            return _WeaponTypeProperty.Value;
        }
        set {
            _WeaponTypeProperty.Value = value;
        }
    }
    
    public virtual float ReloadTime {
        get {
            return _ReloadTimeProperty.Value;
        }
        set {
            _ReloadTimeProperty.Value = value;
        }
    }
    
    public virtual int RoundSize {
        get {
            return _RoundSizeProperty.Value;
        }
        set {
            _RoundSizeProperty.Value = value;
        }
    }
    
    public virtual int MinSpread {
        get {
            return _MinSpreadProperty.Value;
        }
        set {
            _MinSpreadProperty.Value = value;
        }
    }
    
    public virtual int BurstSize {
        get {
            return _BurstSizeProperty.Value;
        }
        set {
            _BurstSizeProperty.Value = value;
        }
    }
    
    public virtual float RecoilSpeed {
        get {
            return _RecoilSpeedProperty.Value;
        }
        set {
            _RecoilSpeedProperty.Value = value;
        }
    }
    
    public virtual float FireSpeed {
        get {
            return _FireSpeedProperty.Value;
        }
        set {
            _FireSpeedProperty.Value = value;
        }
    }
    
    public virtual float BurstSpeed {
        get {
            return _BurstSpeedProperty.Value;
        }
        set {
            _BurstSpeedProperty.Value = value;
        }
    }
    
    public virtual float SpreadMultiplier {
        get {
            return _SpreadMultiplierProperty.Value;
        }
        set {
            _SpreadMultiplierProperty.Value = value;
        }
    }
    
    public virtual ICommand BeginFire {
        get {
            return _BeginFire;
        }
        set {
            _BeginFire = value;
        }
    }
    
    public virtual ICommand NextZoom {
        get {
            return _NextZoom;
        }
        set {
            _NextZoom = value;
        }
    }
    
    public virtual ICommand Reload {
        get {
            return _Reload;
        }
        set {
            _Reload = value;
        }
    }
    
    public virtual ICommand EndFire {
        get {
            return _EndFire;
        }
        set {
            _EndFire = value;
        }
    }
    
    public virtual ICommand BulletFired {
        get {
            return _BulletFired;
        }
        set {
            _BulletFired = value;
        }
    }
    
    protected override void WireCommands(Controller controller) {
        var fPSWeapon = controller as FPSWeaponControllerBase;
        this.BeginFire = new CommandWithSender<FPSWeaponViewModel>(this, fPSWeapon.BeginFire);
        this.NextZoom = new CommandWithSender<FPSWeaponViewModel>(this, fPSWeapon.NextZoom);
        this.Reload = new YieldCommandWithSender<FPSWeaponViewModel>(this, fPSWeapon.Reload);
        this.EndFire = new CommandWithSender<FPSWeaponViewModel>(this, fPSWeapon.EndFire);
        this.BulletFired = new CommandWithSender<FPSWeaponViewModel>(this, fPSWeapon.BulletFired);
    }
    
    public override void Write(ISerializerStream stream) {
		base.Write(stream);
		stream.SerializeInt("Ammo", this.Ammo);
		stream.SerializeInt("State", (int)this.State);
		stream.SerializeInt("ZoomIndex", this.ZoomIndex);
		stream.SerializeInt("MaxZooms", this.MaxZooms);
		stream.SerializeInt("WeaponType", (int)this.WeaponType);
		stream.SerializeFloat("ReloadTime", this.ReloadTime);
		stream.SerializeInt("RoundSize", this.RoundSize);
		stream.SerializeInt("MinSpread", this.MinSpread);
		stream.SerializeInt("BurstSize", this.BurstSize);
		stream.SerializeFloat("RecoilSpeed", this.RecoilSpeed);
		stream.SerializeFloat("FireSpeed", this.FireSpeed);
		stream.SerializeFloat("BurstSpeed", this.BurstSpeed);
		stream.SerializeFloat("SpreadMultiplier", this.SpreadMultiplier);
    }
    
    public override void Read(ISerializerStream stream) {
		base.Read(stream);
		this.Ammo = stream.DeserializeInt("Ammo");
		this.State = (FPSWeaponState)stream.DeserializeInt("State");
		this.ZoomIndex = stream.DeserializeInt("ZoomIndex");
		this.MaxZooms = stream.DeserializeInt("MaxZooms");
		this.WeaponType = (WeaponType)stream.DeserializeInt("WeaponType");
		this.ReloadTime = stream.DeserializeFloat("ReloadTime");
		this.RoundSize = stream.DeserializeInt("RoundSize");
		this.MinSpread = stream.DeserializeInt("MinSpread");
		this.BurstSize = stream.DeserializeInt("BurstSize");
		this.RecoilSpeed = stream.DeserializeFloat("RecoilSpeed");
		this.FireSpeed = stream.DeserializeFloat("FireSpeed");
		this.BurstSpeed = stream.DeserializeFloat("BurstSpeed");
		this.SpreadMultiplier = stream.DeserializeFloat("SpreadMultiplier");
    }
}

[DiagramInfoAttribute("FPSShooter")]
public partial class WavesFPSGameViewModel : FPSGameViewModel {
    
    public readonly P<System.Int32> _KillsToNextWaveProperty = new P<int>();
    
    public readonly P<System.Int32> _WaveKillsProperty = new P<int>();
    
    public readonly P<System.Int32> _CurrentWaveProperty = new P<int>();
    
    public virtual int KillsToNextWave {
        get {
            return _KillsToNextWaveProperty.Value;
        }
        set {
            _KillsToNextWaveProperty.Value = value;
        }
    }
    
    public virtual int WaveKills {
        get {
            return _WaveKillsProperty.Value;
        }
        set {
            _WaveKillsProperty.Value = value;
        }
    }
    
    public virtual int CurrentWave {
        get {
            return _CurrentWaveProperty.Value;
        }
        set {
            _CurrentWaveProperty.Value = value;
        }
    }
    
    protected override void WireCommands(Controller controller) {
        base.WireCommands(controller);
    }
    
    public override void Write(ISerializerStream stream) {
		base.Write(stream);
		stream.SerializeInt("KillsToNextWave", this.KillsToNextWave);
		stream.SerializeInt("WaveKills", this.WaveKills);
		stream.SerializeInt("CurrentWave", this.CurrentWave);
    }
    
    public override void Read(ISerializerStream stream) {
		base.Read(stream);
		this.KillsToNextWave = stream.DeserializeInt("KillsToNextWave");
		this.WaveKills = stream.DeserializeInt("WaveKills");
		this.CurrentWave = stream.DeserializeInt("CurrentWave");
    }
}

[DiagramInfoAttribute("FPSShooter")]
public partial class FPSMenuViewModel : ViewModel {
    
    private ICommand _Play;
    
    public virtual ICommand Play {
        get {
            return _Play;
        }
        set {
            _Play = value;
        }
    }
    
    protected override void WireCommands(Controller controller) {
        var fPSMenu = controller as FPSMenuControllerBase;
        this.Play = new Command(fPSMenu.Play);
    }
    
    public override void Write(ISerializerStream stream) {
		base.Write(stream);
    }
    
    public override void Read(ISerializerStream stream) {
		base.Read(stream);
    }
}

[DiagramInfoAttribute("FPSShooter")]
public partial class DeathMatchGameViewModel : FPSGameViewModel {
    
    protected override void WireCommands(Controller controller) {
        base.WireCommands(controller);
    }
    
    public override void Write(ISerializerStream stream) {
		base.Write(stream);
    }
    
    public override void Read(ISerializerStream stream) {
		base.Read(stream);
    }
}

public enum FPSPlayerState {
    
    Alive,
    
    Dead,
}

public enum FPSGameState {
    
    Active,
    
    Paused,
    
    Done,
}

public enum FPSWeaponState {
    
    Active,
    
    Reloading,
    
    InActive,
    
    Empty,
    
    Firing,
}

public enum WeaponType {
    
    MP5,
    
    UMP5,
    
    Colt,
}
