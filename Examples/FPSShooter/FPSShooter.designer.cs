using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


[DiagramInfoAttribute("FPSShooter")]
public partial class FPSDamageableViewModel : ViewModel {
    
    public readonly P<System.Single> _HealthProperty;
    
    public readonly P<FPSPlayerState> _StateProperty;
    
    private ICommand _ApplyDamage;
    
    public FPSDamageableViewModel() : 
            base() {
        _HealthProperty = new P<float>(this, "Health");
        _StateProperty = new P<FPSPlayerState>(this, "State");
    }
    
    public FPSDamageableViewModel(FPSDamageableControllerBase controller) : 
            this() {
        this.Controller = controller;
    }
    
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
    
    public override void Unbind() {
        base.Unbind();
    }
    
    protected override void FillProperties(List<ViewModelPropertyInfo> list) {
        base.FillProperties(list);;
        list.Add(new ViewModelPropertyInfo(_HealthProperty, false, false, false));
        list.Add(new ViewModelPropertyInfo(_StateProperty, false, false, true));
    }
    
    protected override void FillCommands(List<ViewModelCommandInfo> list) {
        base.FillCommands(list);;
        list.Add(new ViewModelCommandInfo("ApplyDamage", ApplyDamage));
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
    
    private UnityEngine.Vector3 _position;
    
    public readonly P<System.Single> _SpeedProperty;
    
    private FPSGameViewModel _ParentFPSGame;
    
    public FPSEnemyViewModel() : 
            base() {
        _SpeedProperty = new P<float>(this, "Speed");
    }
    
    public FPSEnemyViewModel(FPSEnemyControllerBase controller) : 
            this() {
        this.Controller = controller;
    }
    
    public virtual UnityEngine.Vector3 TransformPosition {
        get {
            return this._position;
        }
        set {
            _position = value;
        }
    }
    
    public virtual float Speed {
        get {
            return _SpeedProperty.Value;
        }
        set {
            _SpeedProperty.Value = value;
        }
    }
    
    public virtual FPSGameViewModel ParentFPSGame {
        get {
            return this._ParentFPSGame;
        }
        set {
            _ParentFPSGame = value;
        }
    }
    
    protected override void WireCommands(Controller controller) {
        base.WireCommands(controller);
    }
    
    public override void Unbind() {
        base.Unbind();
    }
    
    protected override void FillProperties(List<ViewModelPropertyInfo> list) {
        base.FillProperties(list);;
        list.Add(new ViewModelPropertyInfo(_SpeedProperty, false, false, false));
    }
    
    protected override void FillCommands(List<ViewModelCommandInfo> list) {
        base.FillCommands(list);;
    }
    
    public override void Write(ISerializerStream stream) {
		base.Write(stream);
		stream.SerializeFloat("Speed", this.Speed);
		stream.SerializeVector3("TransformPosition", this.TransformPosition);
    }
    
    public override void Read(ISerializerStream stream) {
		base.Read(stream);
		this.Speed = stream.DeserializeFloat("Speed");
		this.TransformPosition = stream.DeserializeVector3("TransformPosition");
    }
}

[DiagramInfoAttribute("FPSShooter")]
public partial class FPSGameViewModel : ViewModel {
    
    public readonly P<FPSGameState> _StateProperty;
    
    public readonly P<FPSPlayerViewModel> _CurrentPlayerProperty;
    
    public readonly P<System.Int32> _ScoreProperty;
    
    public readonly P<System.Int32> _KillsProperty;
    
    public readonly ModelCollection<FPSEnemyViewModel> _EnemiesProperty;
    
    private ICommand _MainMenu;
    
    private ICommand _QuitGame;
    
    public FPSGameViewModel() : 
            base() {
        _StateProperty = new P<FPSGameState>(this, "State");
        _CurrentPlayerProperty = new P<FPSPlayerViewModel>(this, "CurrentPlayer");
        _ScoreProperty = new P<int>(this, "Score");
        _KillsProperty = new P<int>(this, "Kills");
        _EnemiesProperty = new ModelCollection<FPSEnemyViewModel>(this, "Enemies");
        _EnemiesProperty.CollectionChangedWith += EnemiesCollectionChanged;
    }
    
    public FPSGameViewModel(FPSGameControllerBase controller) : 
            this() {
        this.Controller = controller;
    }
    
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
            if (value != null) value.ParentFPSGame = this;
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
            _EnemiesProperty.Clear();
            _EnemiesProperty.AddRange(value);
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
    
    public override void Unbind() {
        base.Unbind();
        _EnemiesProperty.CollectionChangedWith -= EnemiesCollectionChanged;
    }
    
    protected override void FillProperties(List<ViewModelPropertyInfo> list) {
        base.FillProperties(list);;
        list.Add(new ViewModelPropertyInfo(_StateProperty, false, false, true));
        list.Add(new ViewModelPropertyInfo(_CurrentPlayerProperty, true, false, false));
        list.Add(new ViewModelPropertyInfo(_ScoreProperty, false, false, false));
        list.Add(new ViewModelPropertyInfo(_KillsProperty, false, false, false));
        list.Add(new ViewModelPropertyInfo(_EnemiesProperty, true, true, false));
    }
    
    protected override void FillCommands(List<ViewModelCommandInfo> list) {
        base.FillCommands(list);;
        list.Add(new ViewModelCommandInfo("MainMenu", MainMenu));
        list.Add(new ViewModelCommandInfo("QuitGame", QuitGame));
    }
    
    private void EnemiesCollectionChanged(ModelCollectionChangeEventWith<FPSEnemyViewModel> args) {
        foreach (var item in args.OldItemsOfT) item.ParentFPSGame = null;;
        foreach (var item in args.NewItemsOfT) item.ParentFPSGame = this;;
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
    
    private UnityEngine.Vector3 _position;
    
    public readonly P<System.Int32> _CurrentWeaponIndexProperty;
    
    public readonly ModelCollection<FPSWeaponViewModel> _WeaponsProperty;
    
    private ICommand _PreviousWeapon;
    
    private ICommand _NextWeapon;
    
    private ICommand _PickupWeapon;
    
    private ICommand _SelectWeapon;
    
    private FPSGameViewModel _ParentFPSGame;
    
    public FPSPlayerViewModel() : 
            base() {
        _CurrentWeaponIndexProperty = new P<int>(this, "CurrentWeaponIndex");
        _WeaponsProperty = new ModelCollection<FPSWeaponViewModel>(this, "Weapons");
        _WeaponsProperty.CollectionChangedWith += WeaponsCollectionChanged;
    }
    
    public FPSPlayerViewModel(FPSPlayerControllerBase controller) : 
            this() {
        this.Controller = controller;
    }
    
    public virtual UnityEngine.Vector3 TransformPosition {
        get {
            return this._position;
        }
        set {
            _position = value;
        }
    }
    
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
            _WeaponsProperty.Clear();
            _WeaponsProperty.AddRange(value);
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
    
    public virtual FPSGameViewModel ParentFPSGame {
        get {
            return this._ParentFPSGame;
        }
        set {
            _ParentFPSGame = value;
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
    
    public override void Unbind() {
        base.Unbind();
        _WeaponsProperty.CollectionChangedWith -= WeaponsCollectionChanged;
    }
    
    protected override void FillProperties(List<ViewModelPropertyInfo> list) {
        base.FillProperties(list);;
        list.Add(new ViewModelPropertyInfo(_CurrentWeaponIndexProperty, false, false, false));
        list.Add(new ViewModelPropertyInfo(_WeaponsProperty, true, true, false));
    }
    
    protected override void FillCommands(List<ViewModelCommandInfo> list) {
        base.FillCommands(list);;
        list.Add(new ViewModelCommandInfo("PreviousWeapon", PreviousWeapon));
        list.Add(new ViewModelCommandInfo("NextWeapon", NextWeapon));
        list.Add(new ViewModelCommandInfo("PickupWeapon", PickupWeapon));
        list.Add(new ViewModelCommandInfo("SelectWeapon", SelectWeapon));
    }
    
    private void WeaponsCollectionChanged(ModelCollectionChangeEventWith<FPSWeaponViewModel> args) {
        foreach (var item in args.OldItemsOfT) item.ParentFPSPlayer = null;;
        foreach (var item in args.NewItemsOfT) item.ParentFPSPlayer = this;;
    }
    
    public override void Write(ISerializerStream stream) {
		base.Write(stream);
		stream.SerializeInt("CurrentWeaponIndex", this.CurrentWeaponIndex);
		stream.SerializeVector3("TransformPosition", this.TransformPosition);
		stream.SerializeArray("Weapons", this.Weapons);
    }
    
    public override void Read(ISerializerStream stream) {
		base.Read(stream);
		this.CurrentWeaponIndex = stream.DeserializeInt("CurrentWeaponIndex");
		this.TransformPosition = stream.DeserializeVector3("TransformPosition");
		this.Weapons = stream.DeserializeObjectArray<FPSWeaponViewModel>("Weapons").ToList();
    }
}

[DiagramInfoAttribute("FPSShooter")]
public partial class FPSWeaponViewModel : ViewModel {
    
    public readonly P<System.Int32> _AmmoProperty;
    
    public readonly P<FPSWeaponState> _StateProperty;
    
    public readonly P<System.Int32> _ZoomIndexProperty;
    
    public readonly P<System.Int32> _MaxZoomsProperty;
    
    public readonly P<WeaponType> _WeaponTypeProperty;
    
    public readonly P<System.Single> _ReloadTimeProperty;
    
    public readonly P<System.Int32> _RoundSizeProperty;
    
    public readonly P<System.Int32> _MinSpreadProperty;
    
    public readonly P<System.Int32> _BurstSizeProperty;
    
    public readonly P<System.Single> _RecoilSpeedProperty;
    
    public readonly P<System.Single> _FireSpeedProperty;
    
    public readonly P<System.Single> _BurstSpeedProperty;
    
    public readonly P<System.Single> _SpreadMultiplierProperty;
    
    private ICommand _BeginFire;
    
    private ICommand _NextZoom;
    
    private ICommand _Reload;
    
    private ICommand _EndFire;
    
    private ICommand _BulletFired;
    
    private FPSPlayerViewModel _ParentFPSPlayer;
    
    public FPSWeaponViewModel() : 
            base() {
        _AmmoProperty = new P<int>(this, "Ammo");
        _StateProperty = new P<FPSWeaponState>(this, "State");
        _ZoomIndexProperty = new P<int>(this, "ZoomIndex");
        _MaxZoomsProperty = new P<int>(this, "MaxZooms");
        _WeaponTypeProperty = new P<WeaponType>(this, "WeaponType");
        _ReloadTimeProperty = new P<float>(this, "ReloadTime");
        _RoundSizeProperty = new P<int>(this, "RoundSize");
        _MinSpreadProperty = new P<int>(this, "MinSpread");
        _BurstSizeProperty = new P<int>(this, "BurstSize");
        _RecoilSpeedProperty = new P<float>(this, "RecoilSpeed");
        _FireSpeedProperty = new P<float>(this, "FireSpeed");
        _BurstSpeedProperty = new P<float>(this, "BurstSpeed");
        _SpreadMultiplierProperty = new P<float>(this, "SpreadMultiplier");
    }
    
    public FPSWeaponViewModel(FPSWeaponControllerBase controller) : 
            this() {
        this.Controller = controller;
    }
    
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
    
    public virtual FPSPlayerViewModel ParentFPSPlayer {
        get {
            return this._ParentFPSPlayer;
        }
        set {
            _ParentFPSPlayer = value;
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
    
    public override void Unbind() {
        base.Unbind();
    }
    
    protected override void FillProperties(List<ViewModelPropertyInfo> list) {
        base.FillProperties(list);;
        list.Add(new ViewModelPropertyInfo(_AmmoProperty, false, false, false));
        list.Add(new ViewModelPropertyInfo(_StateProperty, false, false, true));
        list.Add(new ViewModelPropertyInfo(_ZoomIndexProperty, false, false, false));
        list.Add(new ViewModelPropertyInfo(_MaxZoomsProperty, false, false, false));
        list.Add(new ViewModelPropertyInfo(_WeaponTypeProperty, false, false, true));
        list.Add(new ViewModelPropertyInfo(_ReloadTimeProperty, false, false, false));
        list.Add(new ViewModelPropertyInfo(_RoundSizeProperty, false, false, false));
        list.Add(new ViewModelPropertyInfo(_MinSpreadProperty, false, false, false));
        list.Add(new ViewModelPropertyInfo(_BurstSizeProperty, false, false, false));
        list.Add(new ViewModelPropertyInfo(_RecoilSpeedProperty, false, false, false));
        list.Add(new ViewModelPropertyInfo(_FireSpeedProperty, false, false, false));
        list.Add(new ViewModelPropertyInfo(_BurstSpeedProperty, false, false, false));
        list.Add(new ViewModelPropertyInfo(_SpreadMultiplierProperty, false, false, false));
    }
    
    protected override void FillCommands(List<ViewModelCommandInfo> list) {
        base.FillCommands(list);;
        list.Add(new ViewModelCommandInfo("BeginFire", BeginFire));
        list.Add(new ViewModelCommandInfo("NextZoom", NextZoom));
        list.Add(new ViewModelCommandInfo("Reload", Reload));
        list.Add(new ViewModelCommandInfo("EndFire", EndFire));
        list.Add(new ViewModelCommandInfo("BulletFired", BulletFired));
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
    
    public readonly P<System.Int32> _KillsToNextWaveProperty;
    
    public readonly P<System.Int32> _WaveKillsProperty;
    
    public readonly P<System.Int32> _CurrentWaveProperty;
    
    public WavesFPSGameViewModel() : 
            base() {
        _KillsToNextWaveProperty = new P<int>(this, "KillsToNextWave");
        _WaveKillsProperty = new P<int>(this, "WaveKills");
        _CurrentWaveProperty = new P<int>(this, "CurrentWave");
    }
    
    public WavesFPSGameViewModel(WavesFPSGameControllerBase controller) : 
            this() {
        this.Controller = controller;
    }
    
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
    
    public override void Unbind() {
        base.Unbind();
    }
    
    protected override void FillProperties(List<ViewModelPropertyInfo> list) {
        base.FillProperties(list);;
        list.Add(new ViewModelPropertyInfo(_KillsToNextWaveProperty, false, false, false));
        list.Add(new ViewModelPropertyInfo(_WaveKillsProperty, false, false, false));
        list.Add(new ViewModelPropertyInfo(_CurrentWaveProperty, false, false, false));
    }
    
    protected override void FillCommands(List<ViewModelCommandInfo> list) {
        base.FillCommands(list);;
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
    
    public FPSMenuViewModel() : 
            base() {
    }
    
    public FPSMenuViewModel(FPSMenuControllerBase controller) : 
            this() {
        this.Controller = controller;
    }
    
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
    
    public override void Unbind() {
        base.Unbind();
    }
    
    protected override void FillProperties(List<ViewModelPropertyInfo> list) {
        base.FillProperties(list);;
    }
    
    protected override void FillCommands(List<ViewModelCommandInfo> list) {
        base.FillCommands(list);;
        list.Add(new ViewModelCommandInfo("Play", Play));
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
    
    public DeathMatchGameViewModel() : 
            base() {
    }
    
    public DeathMatchGameViewModel(DeathMatchGameControllerBase controller) : 
            this() {
        this.Controller = controller;
    }
    
    protected override void WireCommands(Controller controller) {
        base.WireCommands(controller);
    }
    
    public override void Unbind() {
        base.Unbind();
    }
    
    protected override void FillProperties(List<ViewModelPropertyInfo> list) {
        base.FillProperties(list);;
    }
    
    protected override void FillCommands(List<ViewModelCommandInfo> list) {
        base.FillCommands(list);;
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
