using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


[DiagramInfoAttribute("AngryFlappers")]
public partial class AngryFlappersGameViewModel : ViewModel {
    
    public readonly P<BirdViewModel> _BirdProperty;
    
    public readonly P<AngryFlappersGameState> _StateProperty;
    
    public readonly P<System.Int32> _ScoreProperty;
    
    public readonly P<System.Single> _PipeSpawnSpeedProperty;
    
    public readonly P<System.Single> _ScrollSpeedProperty;
    
    public readonly ModelCollection<PipeViewModel> _PipesProperty;
    
    private ICommand _GameOver;
    
    private ICommand _Play;
    
    public AngryFlappersGameViewModel() : 
            base() {
        _BirdProperty = new P<BirdViewModel>(this, "Bird");
        _StateProperty = new P<AngryFlappersGameState>(this, "State");
        _ScoreProperty = new P<int>(this, "Score");
        _PipeSpawnSpeedProperty = new P<float>(this, "PipeSpawnSpeed");
        _ScrollSpeedProperty = new P<float>(this, "ScrollSpeed");
        _PipesProperty = new ModelCollection<PipeViewModel>(this, "Pipes");
        _PipesProperty.CollectionChangedWith += PipesCollectionChanged;
    }
    
    public AngryFlappersGameViewModel(AngryFlappersGameControllerBase controller) : 
            this() {
        this.Controller = controller;
    }
    
    public virtual BirdViewModel Bird {
        get {
            return _BirdProperty.Value;
        }
        set {
            _BirdProperty.Value = value;
            if (value != null) value.ParentAngryFlappersGame = this;
        }
    }
    
    public virtual AngryFlappersGameState State {
        get {
            return _StateProperty.Value;
        }
        set {
            _StateProperty.Value = value;
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
    
    public virtual float PipeSpawnSpeed {
        get {
            return _PipeSpawnSpeedProperty.Value;
        }
        set {
            _PipeSpawnSpeedProperty.Value = value;
        }
    }
    
    public virtual float ScrollSpeed {
        get {
            return _ScrollSpeedProperty.Value;
        }
        set {
            _ScrollSpeedProperty.Value = value;
        }
    }
    
    public virtual System.Collections.Generic.ICollection<PipeViewModel> Pipes {
        get {
            return _PipesProperty;
        }
        set {
            _PipesProperty.Clear();
            _PipesProperty.AddRange(value);
        }
    }
    
    public virtual ICommand GameOver {
        get {
            return _GameOver;
        }
        set {
            _GameOver = value;
        }
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
        var angryFlappersGame = controller as AngryFlappersGameControllerBase;
        this.GameOver = new Command(angryFlappersGame.GameOver);
        this.Play = new Command(angryFlappersGame.Play);
    }
    
    public override void Unbind() {
        base.Unbind();
        _PipesProperty.CollectionChangedWith -= PipesCollectionChanged;
    }
    
    protected override void FillProperties(List<ViewModelPropertyInfo> list) {
        base.FillProperties(list);;
        list.Add(new ViewModelPropertyInfo(_BirdProperty, true, false, false));
        list.Add(new ViewModelPropertyInfo(_StateProperty, false, false, true));
        list.Add(new ViewModelPropertyInfo(_ScoreProperty, false, false, false));
        list.Add(new ViewModelPropertyInfo(_PipeSpawnSpeedProperty, false, false, false));
        list.Add(new ViewModelPropertyInfo(_ScrollSpeedProperty, false, false, false));
        list.Add(new ViewModelPropertyInfo(_PipesProperty, true, true, false));
    }
    
    protected override void FillCommands(List<ViewModelCommandInfo> list) {
        base.FillCommands(list);;
        list.Add(new ViewModelCommandInfo("GameOver", GameOver));
        list.Add(new ViewModelCommandInfo("Play", Play));
    }
    
    private void PipesCollectionChanged(ModelCollectionChangeEventWith<PipeViewModel> args) {
        foreach (var item in args.OldItemsOfT) item.ParentAngryFlappersGame = null;;
        foreach (var item in args.NewItemsOfT) item.ParentAngryFlappersGame = this;;
    }
    
    public override void Write(ISerializerStream stream) {
		base.Write(stream);
		stream.SerializeObject("Bird", this.Bird);
		stream.SerializeInt("State", (int)this.State);
		stream.SerializeInt("Score", this.Score);
		stream.SerializeFloat("PipeSpawnSpeed", this.PipeSpawnSpeed);
		stream.SerializeFloat("ScrollSpeed", this.ScrollSpeed);
		stream.SerializeArray("Pipes", this.Pipes);
    }
    
    public override void Read(ISerializerStream stream) {
		base.Read(stream);
		this.Bird = stream.DeserializeObject<BirdViewModel>("Bird");
		this.State = (AngryFlappersGameState)stream.DeserializeInt("State");
		this.Score = stream.DeserializeInt("Score");
		this.PipeSpawnSpeed = stream.DeserializeFloat("PipeSpawnSpeed");
		this.ScrollSpeed = stream.DeserializeFloat("ScrollSpeed");
		this.Pipes = stream.DeserializeObjectArray<PipeViewModel>("Pipes").ToList();
    }
}

[DiagramInfoAttribute("AngryFlappers")]
public partial class BirdViewModel : ViewModel {
    
    private UnityEngine.Vector3 _position;
    
    public readonly P<BirdState> _StateProperty;
    
    public readonly P<System.Single> _GravityProperty;
    
    public readonly P<System.Single> _MaxSpeedProperty;
    
    public readonly P<System.Single> _FlapVelocityProperty;
    
    private ICommand _Flapped;
    
    private ICommand _Hit;
    
    private AngryFlappersGameViewModel _ParentAngryFlappersGame;
    
    public BirdViewModel() : 
            base() {
        _StateProperty = new P<BirdState>(this, "State");
        _GravityProperty = new P<float>(this, "Gravity");
        _MaxSpeedProperty = new P<float>(this, "MaxSpeed");
        _FlapVelocityProperty = new P<float>(this, "FlapVelocity");
    }
    
    public BirdViewModel(BirdControllerBase controller) : 
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
    
    public virtual BirdState State {
        get {
            return _StateProperty.Value;
        }
        set {
            _StateProperty.Value = value;
        }
    }
    
    public virtual float Gravity {
        get {
            return _GravityProperty.Value;
        }
        set {
            _GravityProperty.Value = value;
        }
    }
    
    public virtual float MaxSpeed {
        get {
            return _MaxSpeedProperty.Value;
        }
        set {
            _MaxSpeedProperty.Value = value;
        }
    }
    
    public virtual float FlapVelocity {
        get {
            return _FlapVelocityProperty.Value;
        }
        set {
            _FlapVelocityProperty.Value = value;
        }
    }
    
    public virtual ICommand Flapped {
        get {
            return _Flapped;
        }
        set {
            _Flapped = value;
        }
    }
    
    public virtual ICommand Hit {
        get {
            return _Hit;
        }
        set {
            _Hit = value;
        }
    }
    
    public virtual AngryFlappersGameViewModel ParentAngryFlappersGame {
        get {
            return this._ParentAngryFlappersGame;
        }
        set {
            _ParentAngryFlappersGame = value;
        }
    }
    
    protected override void WireCommands(Controller controller) {
        var bird = controller as BirdControllerBase;
        this.Flapped = new Command(bird.Flapped);
        this.Hit = new Command(bird.Hit);
    }
    
    public override void Unbind() {
        base.Unbind();
    }
    
    protected override void FillProperties(List<ViewModelPropertyInfo> list) {
        base.FillProperties(list);;
        list.Add(new ViewModelPropertyInfo(_StateProperty, false, false, true));
        list.Add(new ViewModelPropertyInfo(_GravityProperty, false, false, false));
        list.Add(new ViewModelPropertyInfo(_MaxSpeedProperty, false, false, false));
        list.Add(new ViewModelPropertyInfo(_FlapVelocityProperty, false, false, false));
    }
    
    protected override void FillCommands(List<ViewModelCommandInfo> list) {
        base.FillCommands(list);;
        list.Add(new ViewModelCommandInfo("Flapped", Flapped));
        list.Add(new ViewModelCommandInfo("Hit", Hit));
    }
    
    public override void Write(ISerializerStream stream) {
		base.Write(stream);
		stream.SerializeInt("State", (int)this.State);
		stream.SerializeFloat("Gravity", this.Gravity);
		stream.SerializeFloat("MaxSpeed", this.MaxSpeed);
		stream.SerializeFloat("FlapVelocity", this.FlapVelocity);
		stream.SerializeVector3("TransformPosition", this.TransformPosition);
    }
    
    public override void Read(ISerializerStream stream) {
		base.Read(stream);
		this.State = (BirdState)stream.DeserializeInt("State");
		this.Gravity = stream.DeserializeFloat("Gravity");
		this.MaxSpeed = stream.DeserializeFloat("MaxSpeed");
		this.FlapVelocity = stream.DeserializeFloat("FlapVelocity");
		this.TransformPosition = stream.DeserializeVector3("TransformPosition");
    }
}

[DiagramInfoAttribute("AngryFlappers")]
public partial class PipeViewModel : ViewModel {
    
    public readonly P<System.Single> _ScrollSpeedProperty;
    
    private AngryFlappersGameViewModel _ParentAngryFlappersGame;
    
    public PipeViewModel() : 
            base() {
        _ScrollSpeedProperty = new P<float>(this, "ScrollSpeed");
    }
    
    public PipeViewModel(PipeControllerBase controller) : 
            this() {
        this.Controller = controller;
    }
    
    public virtual float ScrollSpeed {
        get {
            return _ScrollSpeedProperty.Value;
        }
        set {
            _ScrollSpeedProperty.Value = value;
        }
    }
    
    public virtual AngryFlappersGameViewModel ParentAngryFlappersGame {
        get {
            return this._ParentAngryFlappersGame;
        }
        set {
            _ParentAngryFlappersGame = value;
        }
    }
    
    protected override void WireCommands(Controller controller) {
    }
    
    public override void Unbind() {
        base.Unbind();
    }
    
    protected override void FillProperties(List<ViewModelPropertyInfo> list) {
        base.FillProperties(list);;
        list.Add(new ViewModelPropertyInfo(_ScrollSpeedProperty, false, false, false));
    }
    
    protected override void FillCommands(List<ViewModelCommandInfo> list) {
        base.FillCommands(list);;
    }
    
    public override void Write(ISerializerStream stream) {
		base.Write(stream);
		stream.SerializeFloat("ScrollSpeed", this.ScrollSpeed);
    }
    
    public override void Read(ISerializerStream stream) {
		base.Read(stream);
		this.ScrollSpeed = stream.DeserializeFloat("ScrollSpeed");
    }
}

public enum BirdState {
    
    Idle,
    
    Alive,
    
    Dead,
}

public enum AngryFlappersGameState {
    
    Menu,
    
    Playing,
    
    GameOver,
}
