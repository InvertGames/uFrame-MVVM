using System;
using System.Collections;
using System.Linq;


[DiagramInfoAttribute("Martifacts")]
public partial class MartifactsGameViewModel : ViewModel {
    
    public readonly P<RoverViewModel> _RoverProperty;
    
    public readonly P<TileViewModel> _TileLeftProperty;
    
    public readonly P<TileViewModel> _TileRightProperty;
    
    public readonly P<TileViewModel> _TileBackProperty;
    
    public readonly P<TileViewModel> _TileFrontProperty;
    
    public readonly P<TileViewModel> _TileFrontRightProperty;
    
    public readonly P<TileViewModel> _TileFrontLeftProperty;
    
    public readonly P<TileViewModel> _TileBackRightProperty;
    
    public readonly P<TileViewModel> _TileBackLeftProperty;
    
    public readonly P<TileViewModel> _CurrentTileProperty;
    
    public readonly P<System.String> _MessageProperty;
    
    public readonly P<System.String> _MessageTitleProperty;
    
    public readonly P<MartifactsGameState> _StateProperty;
    
    public readonly P<TileViewModel> _CompleteTileProperty;
    
    public readonly P<System.Int32> _MoveCountProperty;
    
    public readonly ModelCollection<TileViewModel> _TilesProperty = new ModelCollection<TileViewModel>();
    
    public readonly ModelCollection<ArtifactViewModel> _ArtifactsProperty = new ModelCollection<ArtifactViewModel>();
    
    private ICommand _Retry;
    
    public MartifactsGameViewModel() : 
            base() {
        _RoverProperty = new P<RoverViewModel>(this, "Rover");
        _TileLeftProperty = new P<TileViewModel>(this, "TileLeft");
        _TileRightProperty = new P<TileViewModel>(this, "TileRight");
        _TileBackProperty = new P<TileViewModel>(this, "TileBack");
        _TileFrontProperty = new P<TileViewModel>(this, "TileFront");
        _TileFrontRightProperty = new P<TileViewModel>(this, "TileFrontRight");
        _TileFrontLeftProperty = new P<TileViewModel>(this, "TileFrontLeft");
        _TileBackRightProperty = new P<TileViewModel>(this, "TileBackRight");
        _TileBackLeftProperty = new P<TileViewModel>(this, "TileBackLeft");
        _CurrentTileProperty = new P<TileViewModel>(this, "CurrentTile");
        _MessageProperty = new P<string>(this, "Message");
        _MessageTitleProperty = new P<string>(this, "MessageTitle");
        _StateProperty = new P<MartifactsGameState>(this, "State");
        _CompleteTileProperty = new P<TileViewModel>(this, "CompleteTile");
        _MoveCountProperty = new P<int>(this, "MoveCount");
    }
    
    public MartifactsGameViewModel(MartifactsGameControllerBase controller) : 
            this() {
        this.Controller = controller;
    }
    
    public virtual RoverViewModel Rover {
        get {
            return _RoverProperty.Value;
        }
        set {
            _RoverProperty.Value = value;

        }
    }
    
    public virtual TileViewModel TileLeft {
        get {
            return _TileLeftProperty.Value;
        }
        set {
            _TileLeftProperty.Value = value;
        }
    }
    
    public virtual TileViewModel TileRight {
        get {
            return _TileRightProperty.Value;
        }
        set {
            _TileRightProperty.Value = value;
        }
    }
    
    public virtual TileViewModel TileBack {
        get {
            return _TileBackProperty.Value;
        }
        set {
            _TileBackProperty.Value = value;
        }
    }
    
    public virtual TileViewModel TileFront {
        get {
            return _TileFrontProperty.Value;
        }
        set {
            _TileFrontProperty.Value = value;
        }
    }
    
    public virtual TileViewModel TileFrontRight {
        get {
            return _TileFrontRightProperty.Value;
        }
        set {
            _TileFrontRightProperty.Value = value;
        }
    }
    
    public virtual TileViewModel TileFrontLeft {
        get {
            return _TileFrontLeftProperty.Value;
        }
        set {
            _TileFrontLeftProperty.Value = value;
        }
    }
    
    public virtual TileViewModel TileBackRight {
        get {
            return _TileBackRightProperty.Value;
        }
        set {
            _TileBackRightProperty.Value = value;
        }
    }
    
    public virtual TileViewModel TileBackLeft {
        get {
            return _TileBackLeftProperty.Value;
        }
        set {
            _TileBackLeftProperty.Value = value;
        }
    }
    
    public virtual TileViewModel CurrentTile {
        get {
            return _CurrentTileProperty.Value;
        }
        set {
            _CurrentTileProperty.Value = value;
        }
    }
    
    public virtual string Message {
        get {
            return _MessageProperty.Value;
        }
        set {
            _MessageProperty.Value = value;
        }
    }
    
    public virtual string MessageTitle {
        get {
            return _MessageTitleProperty.Value;
        }
        set {
            _MessageTitleProperty.Value = value;
        }
    }
    
    public virtual MartifactsGameState State {
        get {
            return _StateProperty.Value;
        }
        set {
            _StateProperty.Value = value;
        }
    }
    
    public virtual TileViewModel CompleteTile {
        get {
            return _CompleteTileProperty.Value;
        }
        set {
            _CompleteTileProperty.Value = value;
        }
    }
    
    public virtual int MoveCount {
        get {
            return _MoveCountProperty.Value;
        }
        set {
            _MoveCountProperty.Value = value;
        }
    }
    
    public virtual System.Collections.Generic.ICollection<TileViewModel> Tiles {
        get {
            return _TilesProperty;
        }
        set {
            _TilesProperty.Value = value.ToList();
        }
    }
    
    public virtual System.Collections.Generic.ICollection<ArtifactViewModel> Artifacts {
        get {
            return _ArtifactsProperty;
        }
        set {
            _ArtifactsProperty.Value = value.ToList();
        }
    }
    
    public virtual ICommand Retry {
        get {
            return _Retry;
        }
        set {
            _Retry = value;
        }
    }
    
    protected override void WireCommands(Controller controller) {
        var martifactsGame = controller as MartifactsGameControllerBase;
        this.Retry = new Command(martifactsGame.Retry);
    }
    
    public override void Write(ISerializerStream stream) {
		base.Write(stream);
		stream.SerializeObject("Rover", this.Rover);
		stream.SerializeObject("TileLeft", this.TileLeft);
		stream.SerializeObject("TileRight", this.TileRight);
		stream.SerializeObject("TileBack", this.TileBack);
		stream.SerializeObject("TileFront", this.TileFront);
		stream.SerializeObject("TileFrontRight", this.TileFrontRight);
		stream.SerializeObject("TileFrontLeft", this.TileFrontLeft);
		stream.SerializeObject("TileBackRight", this.TileBackRight);
		stream.SerializeObject("TileBackLeft", this.TileBackLeft);
		stream.SerializeObject("CurrentTile", this.CurrentTile);
		stream.SerializeString("Message", this.Message);
		stream.SerializeString("MessageTitle", this.MessageTitle);
		stream.SerializeInt("State", (int)this.State);
		stream.SerializeObject("CompleteTile", this.CompleteTile);
		stream.SerializeInt("MoveCount", this.MoveCount);
		stream.SerializeArray("Tiles", this.Tiles);
		stream.SerializeArray("Artifacts", this.Artifacts);
    }
    
    public override void Read(ISerializerStream stream) {
		base.Read(stream);
		this.Rover = stream.DeserializeObject<RoverViewModel>("Rover");
		this.TileLeft = stream.DeserializeObject<TileViewModel>("TileLeft");
		this.TileRight = stream.DeserializeObject<TileViewModel>("TileRight");
		this.TileBack = stream.DeserializeObject<TileViewModel>("TileBack");
		this.TileFront = stream.DeserializeObject<TileViewModel>("TileFront");
		this.TileFrontRight = stream.DeserializeObject<TileViewModel>("TileFrontRight");
		this.TileFrontLeft = stream.DeserializeObject<TileViewModel>("TileFrontLeft");
		this.TileBackRight = stream.DeserializeObject<TileViewModel>("TileBackRight");
		this.TileBackLeft = stream.DeserializeObject<TileViewModel>("TileBackLeft");
		this.CurrentTile = stream.DeserializeObject<TileViewModel>("CurrentTile");
		this.Message = stream.DeserializeString("Message");
		this.MessageTitle = stream.DeserializeString("MessageTitle");
		this.State = (MartifactsGameState)stream.DeserializeInt("State");
		this.CompleteTile = stream.DeserializeObject<TileViewModel>("CompleteTile");
		this.MoveCount = stream.DeserializeInt("MoveCount");
		this.Tiles = stream.DeserializeObjectArray<TileViewModel>("Tiles").ToList();
		this.Artifacts = stream.DeserializeObjectArray<ArtifactViewModel>("Artifacts").ToList();
    }
}

[DiagramInfoAttribute("Martifacts")]
public partial class TileViewModel : TiledItemViewModel {
    
    private UnityEngine.Vector3 _position;
    
    public readonly P<System.Boolean> _SelectedProperty;
    
    public TileViewModel() : 
            base() {
        _SelectedProperty = new P<bool>(this, "Selected");
    }
    
    public TileViewModel(TileControllerBase controller) : 
            this() {
        this.Controller = controller;
    }
    
    public virtual UnityEngine.Vector3 Position {
        get {
            return this._position;
        }
        set {
            _position = value;
            Dirty = true;
        }
    }
    
    public virtual bool Selected {
        get {
            return _SelectedProperty.Value;
        }
        set {
            _SelectedProperty.Value = value;
        }
    }
    
    protected override void WireCommands(Controller controller) {
        base.WireCommands(controller);
    }
    
    public override void Write(ISerializerStream stream) {
		base.Write(stream);
		stream.SerializeBool("Selected", this.Selected);
    }
    
    public override void Read(ISerializerStream stream) {
		base.Read(stream);
		this.Selected = stream.DeserializeBool("Selected");
    }
}

[DiagramInfoAttribute("Martifacts")]
public partial class RoverViewModel : ViewModel {
    
    private UnityEngine.Vector3 _position;
    
    public readonly P<System.Int32> _BatteryProperty;
    
    public readonly P<RoverState> _StateProperty;
    
    public readonly P<System.Single> _SpeedProperty;
    
    public readonly P<System.Int32> _TileXProperty;
    
    public readonly P<System.Int32> _TileYProperty;
    
    public readonly P<System.Int32> _DrillFactorProperty;
    
    public readonly P<System.Single> _SonarTimeProperty;
    
    public readonly P<System.Int32> _MaxBatteryProperty;
    
    public readonly ModelCollection<RoverMove> _MovesProperty = new ModelCollection<RoverMove>();
    
    public readonly ModelCollection<ArtifactViewModel> _CollectedArtifactsProperty = new ModelCollection<ArtifactViewModel>();
    
    private ICommand _MoveLeft;
    
    private ICommand _MoveRight;
    
    private ICommand _MoveForward;
    
    private ICommand _MoveBackward;
    
    private ICommand _ShootFlare;
    
    private ICommand _Drill;
    
    private ICommand _Sonar;
    
    private ICommand _ReachedDestination;
    
    public RoverViewModel() : 
            base() {
        _BatteryProperty = new P<int>(this, "Battery");
        _StateProperty = new P<RoverState>(this, "State");
        _SpeedProperty = new P<float>(this, "Speed");
        _TileXProperty = new P<int>(this, "TileX");
        _TileYProperty = new P<int>(this, "TileY");
        _DrillFactorProperty = new P<int>(this, "DrillFactor");
        _SonarTimeProperty = new P<float>(this, "SonarTime");
        _MaxBatteryProperty = new P<int>(this, "MaxBattery");
    }
    
    public RoverViewModel(RoverControllerBase controller) : 
            this() {
        this.Controller = controller;
    }
    
    public virtual UnityEngine.Vector3 Position {
        get {
            return this._position;
        }
        set {
            _position = value;
            Dirty = true;
        }
    }

    public virtual int Battery {
        get {
            return _BatteryProperty.Value;
        }
        set {
            _BatteryProperty.Value = value;
        }
    }
    
    public virtual RoverState State {
        get {
            return _StateProperty.Value;
        }
        set {
            _StateProperty.Value = value;
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
    
    public virtual int TileX {
        get {
            return _TileXProperty.Value;
        }
        set {
            _TileXProperty.Value = value;
        }
    }
    
    public virtual int TileY {
        get {
            return _TileYProperty.Value;
        }
        set {
            _TileYProperty.Value = value;
        }
    }
    
    public virtual int DrillFactor {
        get {
            return _DrillFactorProperty.Value;
        }
        set {
            _DrillFactorProperty.Value = value;
        }
    }
    
    public virtual float SonarTime {
        get {
            return _SonarTimeProperty.Value;
        }
        set {
            _SonarTimeProperty.Value = value;
        }
    }
    
    public virtual int MaxBattery {
        get {
            return _MaxBatteryProperty.Value;
        }
        set {
            _MaxBatteryProperty.Value = value;
        }
    }
    
    public virtual System.Collections.Generic.ICollection<RoverMove> Moves {
        get {
            return _MovesProperty;
        }
        set {
            _MovesProperty.Value = value.ToList();
        }
    }
    
    public virtual System.Collections.Generic.ICollection<ArtifactViewModel> CollectedArtifacts {
        get {
            return _CollectedArtifactsProperty;
        }
        set {
            _CollectedArtifactsProperty.Value = value.ToList();
        }
    }
    
    public virtual ICommand MoveLeft {
        get {
            return _MoveLeft;
        }
        set {
            _MoveLeft = value;
        }
    }
    
    public virtual ICommand MoveRight {
        get {
            return _MoveRight;
        }
        set {
            _MoveRight = value;
        }
    }
    
    public virtual ICommand MoveForward {
        get {
            return _MoveForward;
        }
        set {
            _MoveForward = value;
        }
    }
    
    public virtual ICommand MoveBackward {
        get {
            return _MoveBackward;
        }
        set {
            _MoveBackward = value;
        }
    }
    
    public virtual ICommand ShootFlare {
        get {
            return _ShootFlare;
        }
        set {
            _ShootFlare = value;
        }
    }
    
    public virtual ICommand Drill {
        get {
            return _Drill;
        }
        set {
            _Drill = value;
        }
    }
    
    public virtual ICommand Sonar {
        get {
            return _Sonar;
        }
        set {
            _Sonar = value;
        }
    }
    
    public virtual ICommand ReachedDestination {
        get {
            return _ReachedDestination;
        }
        set {
            _ReachedDestination = value;
        }
    }
    
    protected override void WireCommands(Controller controller) {
        var rover = controller as RoverControllerBase;
        this.MoveLeft = new Command(rover.MoveLeft);
        this.MoveRight = new Command(rover.MoveRight);
        this.MoveForward = new Command(rover.MoveForward);
        this.MoveBackward = new Command(rover.MoveBackward);
        this.ShootFlare = new YieldCommand(rover.ShootFlare);
        this.Drill = new YieldCommand(rover.Drill);
        this.Sonar = new YieldCommand(rover.Sonar);
        this.ReachedDestination = new Command(rover.ReachedDestination);
    }
    
    public override void Write(ISerializerStream stream) {
		base.Write(stream);
		stream.SerializeInt("Battery", this.Battery);
		stream.SerializeInt("State", (int)this.State);
		stream.SerializeFloat("Speed", this.Speed);
		stream.SerializeInt("TileX", this.TileX);
		stream.SerializeInt("TileY", this.TileY);
		stream.SerializeInt("DrillFactor", this.DrillFactor);
		stream.SerializeFloat("SonarTime", this.SonarTime);
		stream.SerializeInt("MaxBattery", this.MaxBattery);
		stream.SerializeArray("CollectedArtifacts", this.CollectedArtifacts);
    }
    
    public override void Read(ISerializerStream stream) {
		base.Read(stream);
		this.Battery = stream.DeserializeInt("Battery");
		this.State = (RoverState)stream.DeserializeInt("State");
		this.Speed = stream.DeserializeFloat("Speed");
		this.TileX = stream.DeserializeInt("TileX");
		this.TileY = stream.DeserializeInt("TileY");
		this.DrillFactor = stream.DeserializeInt("DrillFactor");
		this.SonarTime = stream.DeserializeFloat("SonarTime");
		this.MaxBattery = stream.DeserializeInt("MaxBattery");
		this.CollectedArtifacts = stream.DeserializeObjectArray<ArtifactViewModel>("CollectedArtifacts").ToList();
    }
}

[DiagramInfoAttribute("Martifacts")]
public partial class RockyTileViewModel : TileViewModel {
    
    public RockyTileViewModel() : 
            base() {
    }
    
    public RockyTileViewModel(RockyTileControllerBase controller) : 
            this() {
        this.Controller = controller;
    }
    
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

[DiagramInfoAttribute("Martifacts")]
public partial class ResourceTileViewModel : TileViewModel {
    
    public ResourceTileViewModel() : 
            base() {
    }
    
    public ResourceTileViewModel(ResourceTileControllerBase controller) : 
            this() {
        this.Controller = controller;
    }
    
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

[DiagramInfoAttribute("Martifacts")]
public partial class FlatTileViewModel : TileViewModel {
    
    public readonly P<ArtifactViewModel> _ArtifactProperty;
    
    public FlatTileViewModel() : 
            base() {
        _ArtifactProperty = new P<ArtifactViewModel>(this, "Artifact");
    }
    
    public FlatTileViewModel(FlatTileControllerBase controller) : 
            this() {
        this.Controller = controller;
    }
    
    public virtual ArtifactViewModel Artifact {
        get {
            return _ArtifactProperty.Value;
        }
        set {
            _ArtifactProperty.Value = value;
        }
    }
    
    protected override void WireCommands(Controller controller) {
        base.WireCommands(controller);
    }
    
    public override void Write(ISerializerStream stream) {
		base.Write(stream);
		stream.SerializeObject("Artifact", this.Artifact);
    }
    
    public override void Read(ISerializerStream stream) {
		base.Read(stream);
		this.Artifact = stream.DeserializeObject<ArtifactViewModel>("Artifact");
    }
}

[DiagramInfoAttribute("Martifacts")]
public partial class ArtifactViewModel : TiledItemViewModel {
    
    public readonly P<ArtifactType> _TypeProperty;
    
    public readonly P<System.Int32> _BatteryDeltaProperty;
    
    public ArtifactViewModel() : 
            base() {
        _TypeProperty = new P<ArtifactType>(this, "Type");
        _BatteryDeltaProperty = new P<int>(this, "BatteryDelta");
    }
    
    public ArtifactViewModel(ArtifactControllerBase controller) : 
            this() {
        this.Controller = controller;
    }
    


    public virtual ArtifactType Type {
        get {
            return _TypeProperty.Value;
        }
        set {
            _TypeProperty.Value = value;
        }
    }
    
    public virtual int BatteryDelta {
        get {
            return _BatteryDeltaProperty.Value;
        }
        set {
            _BatteryDeltaProperty.Value = value;
        }
    }
    
    protected override void WireCommands(Controller controller) {
        base.WireCommands(controller);
    }
    
    public override void Write(ISerializerStream stream) {
		base.Write(stream);
		stream.SerializeInt("Type", (int)this.Type);
		stream.SerializeInt("BatteryDelta", this.BatteryDelta);
    }
    
    public override void Read(ISerializerStream stream) {
		base.Read(stream);
		this.Type = (ArtifactType)stream.DeserializeInt("Type");
		this.BatteryDelta = stream.DeserializeInt("BatteryDelta");
    }
}

[DiagramInfoAttribute("Martifacts")]
public partial class TiledItemViewModel : ViewModel {
    
    public readonly P<System.Int32> _TileXProperty;
    
    public readonly P<System.Int32> _TileYProperty;
    
    public TiledItemViewModel() : 
            base() {
        _TileXProperty = new P<int>(this, "TileX");
        _TileYProperty = new P<int>(this, "TileY");
    }
    
    public TiledItemViewModel(TiledItemControllerBase controller) : 
            this() {
        this.Controller = controller;
    }
    
    public virtual int TileX {
        get {
            return _TileXProperty.Value;
        }
        set {
            _TileXProperty.Value = value;
        }
    }
    
    public virtual int TileY {
        get {
            return _TileYProperty.Value;
        }
        set {
            _TileYProperty.Value = value;
        }
    }
    
    protected override void WireCommands(Controller controller) {
    }
    
    public override void Write(ISerializerStream stream) {
		base.Write(stream);
		stream.SerializeInt("TileX", this.TileX);
		stream.SerializeInt("TileY", this.TileY);
    }
    
    public override void Read(ISerializerStream stream) {
		base.Read(stream);
		this.TileX = stream.DeserializeInt("TileX");
		this.TileY = stream.DeserializeInt("TileY");
    }
}

public enum RoverState {
    
    Idle,
    
    Moving,
    
    Firing,
    
    Drilling,
}

public enum RoverMove {
    
    Left,
    
    Right,
    
    Forward,
    
    Back,
}

public enum MartifactsGameState {
    
    Started,
    
    BirdsEye,
    
    GameOver,
    
    Complete,
}

public enum ArtifactType {
    
    BedRock,
    
    EtThenRock,
    
    PinnacleRock,
    
    MysteryRock,
}
