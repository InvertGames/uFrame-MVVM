using System;
using System.Collections;
using System.Linq;


[DiagramInfoAttribute("Checkers")]
public partial class CheckerBoardViewModel : ViewModel {
    
    public readonly ModelCollection<CheckerViewModel> _CheckersProperty = new ModelCollection<CheckerViewModel>();
    
    public readonly ModelCollection<CheckerPlateViewModel> _PlatesProperty = new ModelCollection<CheckerPlateViewModel>();
    
    public CheckerBoardViewModel() : 
            base() {
    }
    
    public CheckerBoardViewModel(CheckerBoardControllerBase controller) : 
            this() {
        this.Controller = controller;
    }
    
    public virtual System.Collections.Generic.ICollection<CheckerViewModel> Checkers {
        get {
            return _CheckersProperty;
        }
        set {
            _CheckersProperty.Clear();
            _CheckersProperty.AddRange(value);
        }
    }
    
    public virtual System.Collections.Generic.ICollection<CheckerPlateViewModel> Plates {
        get {
            return _PlatesProperty;
        }
        set {
            _PlatesProperty.Clear();
            _PlatesProperty.AddRange(value);
        }
    }
    
    protected override void WireCommands(Controller controller) {
    }
    
    public override void Write(ISerializerStream stream) {
		base.Write(stream);
		stream.SerializeArray("Checkers", this.Checkers);
		stream.SerializeArray("Plates", this.Plates);
    }
    
    public override void Read(ISerializerStream stream) {
		base.Read(stream);
		this.Checkers = stream.DeserializeObjectArray<CheckerViewModel>("Checkers").ToList();
		this.Plates = stream.DeserializeObjectArray<CheckerPlateViewModel>("Plates").ToList();
    }
}

[DiagramInfoAttribute("Checkers")]
public partial class CheckerMoveViewModel : ViewModel {
    
    public CheckerMoveViewModel() : 
            base() {
    }
    
    public CheckerMoveViewModel(CheckerMoveControllerBase controller) : 
            this() {
        this.Controller = controller;
    }
    
    protected override void WireCommands(Controller controller) {
    }
    
    public override void Write(ISerializerStream stream) {
		base.Write(stream);
    }
    
    public override void Read(ISerializerStream stream) {
		base.Read(stream);
    }
}

[DiagramInfoAttribute("Checkers")]
public partial class CheckerPlateViewModel : ViewModel {
    
    public readonly P<System.Boolean> _CanMoveToProperty;
    
    public readonly P<UnityEngine.Vector2> _PositionProperty;
    
    public readonly P<System.Boolean> _IsEvenProperty;
    
    private ICommand _SelectCommand;
    
    public CheckerPlateViewModel() : 
            base() {
        _CanMoveToProperty = new P<bool>(this, "CanMoveTo");
        _PositionProperty = new P<UnityEngine.Vector2>(this, "Position");
        _IsEvenProperty = new P<bool>(this, "IsEven");
    }
    
    public CheckerPlateViewModel(CheckerPlateControllerBase controller) : 
            this() {
        this.Controller = controller;
    }
    
    public virtual bool CanMoveTo {
        get {
            return _CanMoveToProperty.Value;
        }
        set {
            _CanMoveToProperty.Value = value;
        }
    }
    
    public virtual UnityEngine.Vector2 Position {
        get {
            return _PositionProperty.Value;
        }
        set {
            _PositionProperty.Value = value;
        }
    }
    
    public virtual bool IsEven {
        get {
            return _IsEvenProperty.Value;
        }
        set {
            _IsEvenProperty.Value = value;
        }
    }
    
    public virtual ICommand SelectCommand {
        get {
            return _SelectCommand;
        }
        set {
            _SelectCommand = value;
        }
    }
    
    protected override void WireCommands(Controller controller) {
        var checkerPlate = controller as CheckerPlateControllerBase;
        this.SelectCommand = new CommandWithSender<CheckerPlateViewModel>(this, checkerPlate.SelectCommand);
    }
    
    public override void Write(ISerializerStream stream) {
		base.Write(stream);
		stream.SerializeBool("CanMoveTo", this.CanMoveTo);
		stream.SerializeVector2("Position", this.Position);
		stream.SerializeBool("IsEven", this.IsEven);
    }
    
    public override void Read(ISerializerStream stream) {
		base.Read(stream);
		this.CanMoveTo = stream.DeserializeBool("CanMoveTo");
		this.Position = stream.DeserializeVector2("Position");
		this.IsEven = stream.DeserializeBool("IsEven");
    }
}

[DiagramInfoAttribute("Checkers")]
public partial class CheckersGameViewModel : ViewModel {
    
    public readonly P<System.Int32> _BlackScoreProperty;
    
    public readonly P<CheckerBoardViewModel> _BoardProperty;
    
    public readonly P<CheckerViewModel> _CurrentCheckerProperty;
    
    public readonly P<CheckerType> _CurrentPlayerProperty;
    
    public readonly P<System.Int32> _RedScoreProperty;
    
    public readonly ModelCollection<CheckerMoveViewModel> _AllowedMovesProperty = new ModelCollection<CheckerMoveViewModel>();
    
    private ICommand _GameOver;
    
    public CheckersGameViewModel() : 
            base() {
        _BlackScoreProperty = new P<int>(this, "BlackScore");
        _BoardProperty = new P<CheckerBoardViewModel>(this, "Board");
        _CurrentCheckerProperty = new P<CheckerViewModel>(this, "CurrentChecker");
        _CurrentPlayerProperty = new P<CheckerType>(this, "CurrentPlayer");
        _RedScoreProperty = new P<int>(this, "RedScore");
    }
    
    public CheckersGameViewModel(CheckersGameControllerBase controller) : 
            this() {
        this.Controller = controller;
    }
    
    public virtual int BlackScore {
        get {
            return _BlackScoreProperty.Value;
        }
        set {
            _BlackScoreProperty.Value = value;
        }
    }
    
    public virtual CheckerBoardViewModel Board {
        get {
            return _BoardProperty.Value;
        }
        set {
            _BoardProperty.Value = value;
        }
    }
    
    public virtual CheckerViewModel CurrentChecker {
        get {
            return _CurrentCheckerProperty.Value;
        }
        set {
            _CurrentCheckerProperty.Value = value;
        }
    }
    
    public virtual CheckerType CurrentPlayer {
        get {
            return _CurrentPlayerProperty.Value;
        }
        set {
            _CurrentPlayerProperty.Value = value;
        }
    }
    
    public virtual int RedScore {
        get {
            return _RedScoreProperty.Value;
        }
        set {
            _RedScoreProperty.Value = value;
        }
    }
    
    public virtual System.Collections.Generic.ICollection<CheckerMoveViewModel> AllowedMoves {
        get {
            return _AllowedMovesProperty;
        }
        set {
            _AllowedMovesProperty.Clear();
            _AllowedMovesProperty.AddRange(value);
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
    
    protected override void WireCommands(Controller controller) {
        var checkersGame = controller as CheckersGameControllerBase;
        this.GameOver = new Command(checkersGame.GameOver);
    }
    
    public override void Write(ISerializerStream stream) {
		base.Write(stream);
		stream.SerializeInt("BlackScore", this.BlackScore);
		stream.SerializeObject("Board", this.Board);
		stream.SerializeObject("CurrentChecker", this.CurrentChecker);
		stream.SerializeInt("CurrentPlayer", (int)this.CurrentPlayer);
		stream.SerializeInt("RedScore", this.RedScore);
		stream.SerializeArray("AllowedMoves", this.AllowedMoves);
    }
    
    public override void Read(ISerializerStream stream) {
		base.Read(stream);
		this.BlackScore = stream.DeserializeInt("BlackScore");
		this.Board = stream.DeserializeObject<CheckerBoardViewModel>("Board");
		this.CurrentChecker = stream.DeserializeObject<CheckerViewModel>("CurrentChecker");
		this.CurrentPlayer = (CheckerType)stream.DeserializeInt("CurrentPlayer");
		this.RedScore = stream.DeserializeInt("RedScore");
		this.AllowedMoves = stream.DeserializeObjectArray<CheckerMoveViewModel>("AllowedMoves").ToList();
    }
}

[DiagramInfoAttribute("Checkers")]
public partial class CheckerViewModel : ViewModel {
    
    public readonly P<System.Boolean> _IsKingMeProperty;
    
    public readonly P<UnityEngine.Vector2> _PositionProperty;
    
    public readonly P<System.Boolean> _SelectedProperty;
    
    public readonly P<CheckerType> _TypeProperty;
    
    private ICommand _SelectCommand;
    
    public CheckerViewModel() : 
            base() {
        _IsKingMeProperty = new P<bool>(this, "IsKingMe");
        _PositionProperty = new P<UnityEngine.Vector2>(this, "Position");
        _SelectedProperty = new P<bool>(this, "Selected");
        _TypeProperty = new P<CheckerType>(this, "Type");
    }
    
    public CheckerViewModel(CheckerControllerBase controller) : 
            this() {
        this.Controller = controller;
    }
    
    public virtual bool IsKingMe {
        get {
            return _IsKingMeProperty.Value;
        }
        set {
            _IsKingMeProperty.Value = value;
        }
    }
    
    public virtual UnityEngine.Vector2 Position {
        get {
            return _PositionProperty.Value;
        }
        set {
            _PositionProperty.Value = value;
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
    
    public virtual CheckerType Type {
        get {
            return _TypeProperty.Value;
        }
        set {
            _TypeProperty.Value = value;
        }
    }
    
    public virtual ICommand SelectCommand {
        get {
            return _SelectCommand;
        }
        set {
            _SelectCommand = value;
        }
    }
    
    protected override void WireCommands(Controller controller) {
        var checker = controller as CheckerControllerBase;
        this.SelectCommand = new CommandWithSender<CheckerViewModel>(this, checker.SelectCommand);
    }
    
    public override void Write(ISerializerStream stream) {
		base.Write(stream);
		stream.SerializeBool("IsKingMe", this.IsKingMe);
		stream.SerializeVector2("Position", this.Position);
		stream.SerializeBool("Selected", this.Selected);
		stream.SerializeInt("Type", (int)this.Type);
    }
    
    public override void Read(ISerializerStream stream) {
		base.Read(stream);
		this.IsKingMe = stream.DeserializeBool("IsKingMe");
		this.Position = stream.DeserializeVector2("Position");
		this.Selected = stream.DeserializeBool("Selected");
		this.Type = (CheckerType)stream.DeserializeInt("Type");
    }
}

[DiagramInfoAttribute("Checkers")]
public partial class AICheckersGameViewModel : CheckersGameViewModel {
    
    public AICheckersGameViewModel() : 
            base() {
    }
    
    public AICheckersGameViewModel(AICheckersGameControllerBase controller) : 
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

[DiagramInfoAttribute("Checkers")]
public partial class MainMenuViewModel : ViewModel {
    
    private ICommand _Play;
    
    public MainMenuViewModel() : 
            base() {
    }
    
    public MainMenuViewModel(MainMenuControllerBase controller) : 
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
        var mainMenu = controller as MainMenuControllerBase;
        this.Play = new Command(mainMenu.Play);
    }
    
    public override void Write(ISerializerStream stream) {
		base.Write(stream);
    }
    
    public override void Read(ISerializerStream stream) {
		base.Read(stream);
    }
}

public enum CheckerType {
    
    Red,
    
    Black,
}
