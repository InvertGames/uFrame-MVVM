using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[DiagramInfoAttribute("FPSShooterProject")]
public partial class FPSDamageableViewModel : ViewModel {
    
    public P<Single> _HealthProperty;
    
    public P<FPSPlayerState> _StateProperty;
    
    public P<Vector3> _PositionProperty;
    
    public P<String> _DataProperty;
    
    private CommandWithSenderAndArgument<FPSDamageableViewModel, Int32> _ApplyDamage;
    
    public FPSDamageableViewModel() : 
            base() {
        _HealthProperty = new P<Single>(this, "Health");
        _StateProperty = new P<FPSPlayerState>(this, "State");
        _PositionProperty = new P<Vector3>(this, "Position");
        _DataProperty = new P<String>(this, "Data");
    }
    
    public FPSDamageableViewModel(FPSDamageableControllerBase controller) : 
            this() {
        this.Controller = controller;
    }
    
    public virtual P<Single> HealthProperty {
        get {
            return this._HealthProperty;
        }
    }
    
    public virtual Single Health {
        get {
            return _HealthProperty.Value;
        }
        set {
            _HealthProperty.Value = value;
        }
    }
    
    public virtual P<FPSPlayerState> StateProperty {
        get {
            return this._StateProperty;
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
    
    public virtual P<Vector3> PositionProperty {
        get {
            return this._PositionProperty;
        }
    }
    
    public virtual Vector3 Position {
        get {
            return _PositionProperty.Value;
        }
        set {
            _PositionProperty.Value = value;
        }
    }
    
    public virtual P<String> DataProperty {
        get {
            return this._DataProperty;
        }
    }
    
    public virtual String Data {
        get {
            return _DataProperty.Value;
        }
        set {
            _DataProperty.Value = value;
        }
    }
    
    public virtual CommandWithSenderAndArgument<FPSDamageableViewModel, Int32> ApplyDamage {
        get {
            return _ApplyDamage;
        }
        set {
            _ApplyDamage = value;
        }
    }
    
    protected override void WireCommands(Controller controller) {
        var fPSDamageable = controller as FPSDamageableControllerBase;
        this.ApplyDamage = new CommandWithSenderAndArgument<FPSDamageableViewModel, Int32>(this, fPSDamageable.ApplyDamage);
    }
    
    public override void Unbind() {
        base.Unbind();
    }
    
    protected override void FillProperties(List<ViewModelPropertyInfo> list) {
        base.FillProperties(list);;
        list.Add(new ViewModelPropertyInfo(_HealthProperty, false, false, false));
        list.Add(new ViewModelPropertyInfo(_StateProperty, false, false, true));
        list.Add(new ViewModelPropertyInfo(_PositionProperty, false, false, false));
        list.Add(new ViewModelPropertyInfo(_DataProperty, false, false, false));
    }
    
    protected override void FillCommands(List<ViewModelCommandInfo> list) {
        base.FillCommands(list);;
        list.Add(new ViewModelCommandInfo("ApplyDamage", ApplyDamage));
    }
    
    public override void Write(ISerializerStream stream) {
		base.Write(stream);
		stream.SerializeInt("State", (int)this.State);
    }
    
    public override void Read(ISerializerStream stream) {
		base.Read(stream);
		this.State = (FPSPlayerState)stream.DeserializeInt("State");
    }
}

[DiagramInfoAttribute("FPSShooterProject")]
public partial class FPSEnemyViewModel : FPSDamageableViewModel {
    
    public P<Single> _SpeedProperty;
    
    public P<Single> _DistanceToPlayerProperty;
    
    private FPSGameViewModel _ParentFPSGame;
    
    public FPSEnemyViewModel() : 
            base() {
        _SpeedProperty = new P<Single>(this, "Speed");
        _DistanceToPlayerProperty = new P<Single>(this, "DistanceToPlayer");
    }
    
    public FPSEnemyViewModel(FPSEnemyControllerBase controller) : 
            this() {
        this.Controller = controller;
    }
    
    public virtual P<Single> SpeedProperty {
        get {
            return this._SpeedProperty;
        }
    }
    
    public virtual Single Speed {
        get {
            return _SpeedProperty.Value;
        }
        set {
            _SpeedProperty.Value = value;
        }
    }
    
    public virtual P<Single> DistanceToPlayerProperty {
        get {
            return this._DistanceToPlayerProperty;
        }
    }
    
    public virtual Single DistanceToPlayer {
        get {
            return _DistanceToPlayerProperty.Value;
        }
        set {
            _DistanceToPlayerProperty.Value = value;
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
        list.Add(new ViewModelPropertyInfo(_DistanceToPlayerProperty, false, false, false));
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

[DiagramInfoAttribute("FPSShooterProject")]
public partial class FPSGameViewModel : ViewModel {
    
    public P<FPSGameState> _StateProperty;
    
    public P<FPSPlayerViewModel> _CurrentPlayerProperty;
    
    public P<Int32> _ScoreProperty;
    
    public P<Int32> _KillsProperty;
    
    public ModelCollection<FPSEnemyViewModel> _EnemiesProperty;
    
    private CommandWithSender<FPSGameViewModel> _MainMenu;
    
    private CommandWithSender<FPSGameViewModel> _QuitGame;
    
    public FPSGameViewModel() : 
            base() {
        _StateProperty = new P<FPSGameState>(this, "State");
        _CurrentPlayerProperty = new P<FPSPlayerViewModel>(this, "CurrentPlayer");
        _ScoreProperty = new P<Int32>(this, "Score");
        _KillsProperty = new P<Int32>(this, "Kills");
        _EnemiesProperty = new ModelCollection<FPSEnemyViewModel>(this, "Enemies");
        _EnemiesProperty.CollectionChanged += EnemiesCollectionChanged;
    }
    
    public FPSGameViewModel(FPSGameControllerBase controller) : 
            this() {
        this.Controller = controller;
    }
    
    public virtual P<FPSGameState> StateProperty {
        get {
            return this._StateProperty;
        }
    }
    
    public virtual FPSGameState State {
        get {
            return _StateProperty.Value;
        }
        set {
            _StateProperty.Value = value;
        }
    }
    
    public virtual P<FPSPlayerViewModel> CurrentPlayerProperty {
        get {
            return this._CurrentPlayerProperty;
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
    
    public virtual P<Int32> ScoreProperty {
        get {
            return this._ScoreProperty;
        }
    }
    
    public virtual Int32 Score {
        get {
            return _ScoreProperty.Value;
        }
        set {
            _ScoreProperty.Value = value;
        }
    }
    
    public virtual P<Int32> KillsProperty {
        get {
            return this._KillsProperty;
        }
    }
    
    public virtual Int32 Kills {
        get {
            return _KillsProperty.Value;
        }
        set {
            _KillsProperty.Value = value;
        }
    }
    
    public virtual ModelCollection<FPSEnemyViewModel> Enemies {
        get {
            return this._EnemiesProperty;
        }
    }
    
    public virtual CommandWithSender<FPSGameViewModel> MainMenu {
        get {
            return _MainMenu;
        }
        set {
            _MainMenu = value;
        }
    }
    
    public virtual CommandWithSender<FPSGameViewModel> QuitGame {
        get {
            return _QuitGame;
        }
        set {
            _QuitGame = value;
        }
    }
    
    protected override void WireCommands(Controller controller) {
        var fPSGame = controller as FPSGameControllerBase;
        this.MainMenu = new CommandWithSender<FPSGameViewModel>(this, fPSGame.MainMenu);
        this.QuitGame = new CommandWithSender<FPSGameViewModel>(this, fPSGame.QuitGame);
    }
    
    public override void Unbind() {
        base.Unbind();
        _EnemiesProperty.CollectionChanged -= EnemiesCollectionChanged;
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
    
    private void EnemiesCollectionChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs args) {
        foreach (var item in args.OldItems.OfType<FPSEnemyViewModel>()) item.ParentFPSGame = null;;
        foreach (var item in args.NewItems.OfType<FPSEnemyViewModel>()) item.ParentFPSGame = this;;
    }
    
    public override void Write(ISerializerStream stream) {
		base.Write(stream);
		stream.SerializeInt("State", (int)this.State);
		stream.SerializeObject("CurrentPlayer", this.CurrentPlayer);
        stream.SerializeArray("Enemies", this.Enemies);
    }
    
    public override void Read(ISerializerStream stream) {
		base.Read(stream);
		this.State = (FPSGameState)stream.DeserializeInt("State");
		this.CurrentPlayer = stream.DeserializeObject<FPSPlayerViewModel>("CurrentPlayer");
        this.Enemies.Clear();
        this.Enemies.AddRange(stream.DeserializeObjectArray<FPSEnemyViewModel>("Enemies"));
    }
}

[DiagramInfoAttribute("FPSShooterProject")]
public partial class FPSPlayerViewModel : FPSDamageableViewModel {
    
    public P<Int32> _CurrentWeaponIndexProperty;
    
    public ModelCollection<FPSWeaponViewModel> _WeaponsProperty;
    
    private CommandWithSender<FPSPlayerViewModel> _PreviousWeapon;
    
    private CommandWithSender<FPSPlayerViewModel> _NextWeapon;
    
    private CommandWithSenderAndArgument<FPSPlayerViewModel, FPSWeaponViewModel> _PickupWeapon;
    
    private CommandWithSenderAndArgument<FPSPlayerViewModel, Int32> _SelectWeapon;
    
    private FPSGameViewModel _ParentFPSGame;
    
    public FPSPlayerViewModel() : 
            base() {
        _CurrentWeaponIndexProperty = new P<Int32>(this, "CurrentWeaponIndex");
        _WeaponsProperty = new ModelCollection<FPSWeaponViewModel>(this, "Weapons");
        _WeaponsProperty.CollectionChanged += WeaponsCollectionChanged;
    }
    
    public FPSPlayerViewModel(FPSPlayerControllerBase controller) : 
            this() {
        this.Controller = controller;
    }
    
    public virtual P<Int32> CurrentWeaponIndexProperty {
        get {
            return this._CurrentWeaponIndexProperty;
        }
    }
    
    public virtual Int32 CurrentWeaponIndex {
        get {
            return _CurrentWeaponIndexProperty.Value;
        }
        set {
            _CurrentWeaponIndexProperty.Value = value;
        }
    }
    
    public virtual ModelCollection<FPSWeaponViewModel> Weapons {
        get {
            return this._WeaponsProperty;
        }
    }
    
    public virtual CommandWithSender<FPSPlayerViewModel> PreviousWeapon {
        get {
            return _PreviousWeapon;
        }
        set {
            _PreviousWeapon = value;
        }
    }
    
    public virtual CommandWithSender<FPSPlayerViewModel> NextWeapon {
        get {
            return _NextWeapon;
        }
        set {
            _NextWeapon = value;
        }
    }
    
    public virtual CommandWithSenderAndArgument<FPSPlayerViewModel, FPSWeaponViewModel> PickupWeapon {
        get {
            return _PickupWeapon;
        }
        set {
            _PickupWeapon = value;
        }
    }
    
    public virtual CommandWithSenderAndArgument<FPSPlayerViewModel, Int32> SelectWeapon {
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
        this.SelectWeapon = new CommandWithSenderAndArgument<FPSPlayerViewModel, Int32>(this, fPSPlayer.SelectWeapon);
    }
    
    public override void Unbind() {
        base.Unbind();
        _WeaponsProperty.CollectionChanged -= WeaponsCollectionChanged;
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
    
    private void WeaponsCollectionChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs args) {
        foreach (var item in args.OldItems.OfType<FPSWeaponViewModel>()) item.ParentFPSPlayer = null;;
        foreach (var item in args.NewItems.OfType<FPSWeaponViewModel>()) item.ParentFPSPlayer = this;;
    }
    
    public override void Write(ISerializerStream stream) {
		base.Write(stream);
        stream.SerializeArray("Weapons", this.Weapons);
    }
    
    public override void Read(ISerializerStream stream) {
		base.Read(stream);
        this.Weapons.Clear();
        this.Weapons.AddRange(stream.DeserializeObjectArray<FPSWeaponViewModel>("Weapons"));
    }
}

[DiagramInfoAttribute("FPSShooterProject")]
public partial class FPSWeaponViewModel : ViewModel {
    
    public P<Int32> _AmmoProperty;
    
    public P<FPSWeaponState> _StateProperty;
    
    public P<Int32> _ZoomIndexProperty;
    
    public P<Int32> _MaxZoomsProperty;
    
    public P<WeaponType> _WeaponTypeProperty;
    
    public P<Single> _ReloadTimeProperty;
    
    public P<Int32> _RoundSizeProperty;
    
    public P<Int32> _MinSpreadProperty;
    
    public P<Int32> _BurstSizeProperty;
    
    public P<Single> _RecoilSpeedProperty;
    
    public P<Single> _FireSpeedProperty;
    
    public P<Single> _BurstSpeedProperty;
    
    public P<Single> _SpreadMultiplierProperty;
    
    private CommandWithSender<FPSWeaponViewModel> _BeginFire;
    
    private CommandWithSender<FPSWeaponViewModel> _NextZoom;
    
    private CommandWithSender<FPSWeaponViewModel> _Reload;
    
    private CommandWithSender<FPSWeaponViewModel> _EndFire;
    
    private CommandWithSender<FPSWeaponViewModel> _BulletFired;
    
    private FPSPlayerViewModel _ParentFPSPlayer;
    
    public FPSWeaponViewModel() : 
            base() {
        _AmmoProperty = new P<Int32>(this, "Ammo");
        _StateProperty = new P<FPSWeaponState>(this, "State");
        _ZoomIndexProperty = new P<Int32>(this, "ZoomIndex");
        _MaxZoomsProperty = new P<Int32>(this, "MaxZooms");
        _WeaponTypeProperty = new P<WeaponType>(this, "WeaponType");
        _ReloadTimeProperty = new P<Single>(this, "ReloadTime");
        _RoundSizeProperty = new P<Int32>(this, "RoundSize");
        _MinSpreadProperty = new P<Int32>(this, "MinSpread");
        _BurstSizeProperty = new P<Int32>(this, "BurstSize");
        _RecoilSpeedProperty = new P<Single>(this, "RecoilSpeed");
        _FireSpeedProperty = new P<Single>(this, "FireSpeed");
        _BurstSpeedProperty = new P<Single>(this, "BurstSpeed");
        _SpreadMultiplierProperty = new P<Single>(this, "SpreadMultiplier");
    }
    
    public FPSWeaponViewModel(FPSWeaponControllerBase controller) : 
            this() {
        this.Controller = controller;
    }
    
    public virtual P<Int32> AmmoProperty {
        get {
            return this._AmmoProperty;
        }
    }
    
    public virtual Int32 Ammo {
        get {
            return _AmmoProperty.Value;
        }
        set {
            _AmmoProperty.Value = value;
        }
    }
    
    public virtual P<FPSWeaponState> StateProperty {
        get {
            return this._StateProperty;
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
    
    public virtual P<Int32> ZoomIndexProperty {
        get {
            return this._ZoomIndexProperty;
        }
    }
    
    public virtual Int32 ZoomIndex {
        get {
            return _ZoomIndexProperty.Value;
        }
        set {
            _ZoomIndexProperty.Value = value;
        }
    }
    
    public virtual P<Int32> MaxZoomsProperty {
        get {
            return this._MaxZoomsProperty;
        }
    }
    
    public virtual Int32 MaxZooms {
        get {
            return _MaxZoomsProperty.Value;
        }
        set {
            _MaxZoomsProperty.Value = value;
        }
    }
    
    public virtual P<WeaponType> WeaponTypeProperty {
        get {
            return this._WeaponTypeProperty;
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
    
    public virtual P<Single> ReloadTimeProperty {
        get {
            return this._ReloadTimeProperty;
        }
    }
    
    public virtual Single ReloadTime {
        get {
            return _ReloadTimeProperty.Value;
        }
        set {
            _ReloadTimeProperty.Value = value;
        }
    }
    
    public virtual P<Int32> RoundSizeProperty {
        get {
            return this._RoundSizeProperty;
        }
    }
    
    public virtual Int32 RoundSize {
        get {
            return _RoundSizeProperty.Value;
        }
        set {
            _RoundSizeProperty.Value = value;
        }
    }
    
    public virtual P<Int32> MinSpreadProperty {
        get {
            return this._MinSpreadProperty;
        }
    }
    
    public virtual Int32 MinSpread {
        get {
            return _MinSpreadProperty.Value;
        }
        set {
            _MinSpreadProperty.Value = value;
        }
    }
    
    public virtual P<Int32> BurstSizeProperty {
        get {
            return this._BurstSizeProperty;
        }
    }
    
    public virtual Int32 BurstSize {
        get {
            return _BurstSizeProperty.Value;
        }
        set {
            _BurstSizeProperty.Value = value;
        }
    }
    
    public virtual P<Single> RecoilSpeedProperty {
        get {
            return this._RecoilSpeedProperty;
        }
    }
    
    public virtual Single RecoilSpeed {
        get {
            return _RecoilSpeedProperty.Value;
        }
        set {
            _RecoilSpeedProperty.Value = value;
        }
    }
    
    public virtual P<Single> FireSpeedProperty {
        get {
            return this._FireSpeedProperty;
        }
    }
    
    public virtual Single FireSpeed {
        get {
            return _FireSpeedProperty.Value;
        }
        set {
            _FireSpeedProperty.Value = value;
        }
    }
    
    public virtual P<Single> BurstSpeedProperty {
        get {
            return this._BurstSpeedProperty;
        }
    }
    
    public virtual Single BurstSpeed {
        get {
            return _BurstSpeedProperty.Value;
        }
        set {
            _BurstSpeedProperty.Value = value;
        }
    }
    
    public virtual P<Single> SpreadMultiplierProperty {
        get {
            return this._SpreadMultiplierProperty;
        }
    }
    
    public virtual Single SpreadMultiplier {
        get {
            return _SpreadMultiplierProperty.Value;
        }
        set {
            _SpreadMultiplierProperty.Value = value;
        }
    }
    
    public virtual CommandWithSender<FPSWeaponViewModel> BeginFire {
        get {
            return _BeginFire;
        }
        set {
            _BeginFire = value;
        }
    }
    
    public virtual CommandWithSender<FPSWeaponViewModel> NextZoom {
        get {
            return _NextZoom;
        }
        set {
            _NextZoom = value;
        }
    }
    
    public virtual CommandWithSender<FPSWeaponViewModel> Reload {
        get {
            return _Reload;
        }
        set {
            _Reload = value;
        }
    }
    
    public virtual CommandWithSender<FPSWeaponViewModel> EndFire {
        get {
            return _EndFire;
        }
        set {
            _EndFire = value;
        }
    }
    
    public virtual CommandWithSender<FPSWeaponViewModel> BulletFired {
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
        this.Reload = new CommandWithSender<FPSWeaponViewModel>(this, fPSWeapon.Reload);
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
		stream.SerializeInt("State", (int)this.State);
		stream.SerializeInt("WeaponType", (int)this.WeaponType);
    }
    
    public override void Read(ISerializerStream stream) {
		base.Read(stream);
		this.State = (FPSWeaponState)stream.DeserializeInt("State");
		this.WeaponType = (WeaponType)stream.DeserializeInt("WeaponType");
    }
}

[DiagramInfoAttribute("FPSShooterProject")]
public partial class WavesFPSGameViewModel : FPSGameViewModel {
    
    public P<Int32> _KillsToNextWaveProperty;
    
    public P<Int32> _WaveKillsProperty;
    
    public P<Int32> _CurrentWaveProperty;
    
    public WavesFPSGameViewModel() : 
            base() {
        _KillsToNextWaveProperty = new P<Int32>(this, "KillsToNextWave");
        _WaveKillsProperty = new P<Int32>(this, "WaveKills");
        _CurrentWaveProperty = new P<Int32>(this, "CurrentWave");
    }
    
    public WavesFPSGameViewModel(WavesFPSGameControllerBase controller) : 
            this() {
        this.Controller = controller;
    }
    
    public virtual P<Int32> KillsToNextWaveProperty {
        get {
            return this._KillsToNextWaveProperty;
        }
    }
    
    public virtual Int32 KillsToNextWave {
        get {
            return _KillsToNextWaveProperty.Value;
        }
        set {
            _KillsToNextWaveProperty.Value = value;
        }
    }
    
    public virtual P<Int32> WaveKillsProperty {
        get {
            return this._WaveKillsProperty;
        }
    }
    
    public virtual Int32 WaveKills {
        get {
            return _WaveKillsProperty.Value;
        }
        set {
            _WaveKillsProperty.Value = value;
        }
    }
    
    public virtual P<Int32> CurrentWaveProperty {
        get {
            return this._CurrentWaveProperty;
        }
    }
    
    public virtual Int32 CurrentWave {
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
    }
    
    public override void Read(ISerializerStream stream) {
		base.Read(stream);
    }
}

[DiagramInfoAttribute("FPSShooterProject")]
public partial class FPSMenuViewModel : ViewModel {
    
    private CommandWithSender<FPSMenuViewModel> _Play;
    
    public FPSMenuViewModel() : 
            base() {
    }
    
    public FPSMenuViewModel(FPSMenuControllerBase controller) : 
            this() {
        this.Controller = controller;
    }
    
    public virtual CommandWithSender<FPSMenuViewModel> Play {
        get {
            return _Play;
        }
        set {
            _Play = value;
        }
    }
    
    protected override void WireCommands(Controller controller) {
        var fPSMenu = controller as FPSMenuControllerBase;
        this.Play = new CommandWithSender<FPSMenuViewModel>(this, fPSMenu.Play);
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

[DiagramInfoAttribute("FPSShooterProject")]
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
