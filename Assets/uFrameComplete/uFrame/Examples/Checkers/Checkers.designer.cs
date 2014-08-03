using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


[DiagramInfoAttribute("Checkers")]
public partial class CheckerBoardViewModel : ViewModel {
    
    public readonly ModelCollection<CheckerViewModel> _CheckersProperty = new ModelCollection<CheckerViewModel>();
    
    public readonly ModelCollection<CheckerPlateViewModel> _PlatesProperty = new ModelCollection<CheckerPlateViewModel>();
    
    private CheckersGameViewModel _ParentCheckersGame;
    
    public CheckerBoardViewModel() : 
            base() {
        _CheckersProperty.CollectionChangedWith += CheckersCollectionChanged;
        _PlatesProperty.CollectionChangedWith += PlatesCollectionChanged;
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
    
    public virtual CheckersGameViewModel ParentCheckersGame {
        get {
            return this._ParentCheckersGame;
        }
        set {
            _ParentCheckersGame = value;
        }
    }
    
    protected override void WireCommands(Controller controller) {
    }
    
    public override void Unbind() {
        base.Unbind();
        _CheckersProperty.CollectionChangedWith -= CheckersCollectionChanged;
        _PlatesProperty.CollectionChangedWith -= PlatesCollectionChanged;
    }
    
    protected override void FillProperties(List<ViewModelPropertyInfo> list) {
        base.FillProperties(list);;
        list.Add(new ViewModelPropertyInfo(_CheckersProperty, true, true, false));
        list.Add(new ViewModelPropertyInfo(_PlatesProperty, true, true, false));
    }
    
    protected override void FillCommands(List<ViewModelCommandInfo> list) {
        base.FillCommands(list);;
    }
    
    private void CheckersCollectionChanged(ModelCollectionChangeEventWith<CheckerViewModel> args) {
        foreach (var item in args.OldItemsOfT) item.ParentCheckerBoard = null;;
        foreach (var item in args.NewItemsOfT) item.ParentCheckerBoard = this;;
    }
    
    private void PlatesCollectionChanged(ModelCollectionChangeEventWith<CheckerPlateViewModel> args) {
        foreach (var item in args.OldItemsOfT) item.ParentCheckerBoard = null;;
        foreach (var item in args.NewItemsOfT) item.ParentCheckerBoard = this;;
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
    
    private CheckersGameViewModel _ParentCheckersGame;
    
    public CheckerMoveViewModel() : 
            base() {
    }
    
    public CheckerMoveViewModel(CheckerMoveControllerBase controller) : 
            this() {
        this.Controller = controller;
    }
    
    public virtual CheckersGameViewModel ParentCheckersGame {
        get {
            return this._ParentCheckersGame;
        }
        set {
            _ParentCheckersGame = value;
        }
    }
    
    protected override void WireCommands(Controller controller) {
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

[DiagramInfoAttribute("Checkers")]
public partial class CheckerPlateViewModel : ViewModel {
    
    public readonly P<System.Boolean> _CanMoveToProperty;
    
    public readonly P<UnityEngine.Vector2> _PositionProperty;
    
    public readonly P<System.Boolean> _IsEvenProperty;
    
    private ICommand _SelectCommand;
    
    private CheckerBoardViewModel _ParentCheckerBoard;
    
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
    
    public virtual CheckerBoardViewModel ParentCheckerBoard {
        get {
            return this._ParentCheckerBoard;
        }
        set {
            _ParentCheckerBoard = value;
        }
    }
    
    protected override void WireCommands(Controller controller) {
        var checkerPlate = controller as CheckerPlateControllerBase;
        this.SelectCommand = new CommandWithSender<CheckerPlateViewModel>(this, checkerPlate.SelectCommand);
    }
    
    public override void Unbind() {
        base.Unbind();
    }
    
    protected override void FillProperties(List<ViewModelPropertyInfo> list) {
        base.FillProperties(list);;
        list.Add(new ViewModelPropertyInfo(_CanMoveToProperty, false, false, false));
        list.Add(new ViewModelPropertyInfo(_PositionProperty, false, false, false));
        list.Add(new ViewModelPropertyInfo(_IsEvenProperty, false, false, false));
    }
    
    protected override void FillCommands(List<ViewModelCommandInfo> list) {
        base.FillCommands(list);;
        list.Add(new ViewModelCommandInfo("SelectCommand", SelectCommand));
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
    
    public readonly P<System.Int32> _RedScoreProperty;
    
    public readonly P<CheckerBoardViewModel> _BoardProperty;
    
    public readonly P<CheckerViewModel> _CurrentCheckerProperty;
    
    public readonly P<CheckerType> _CurrentPlayerProperty;
    
    public readonly ModelCollection<CheckerMoveViewModel> _AllowedMovesProperty = new ModelCollection<CheckerMoveViewModel>();
    
    private ICommand _GameOver;
    
    public CheckersGameViewModel() : 
            base() {
        _BlackScoreProperty = new P<int>(this, "BlackScore");
        _RedScoreProperty = new P<int>(this, "RedScore");
        _BoardProperty = new P<CheckerBoardViewModel>(this, "Board");
        _CurrentCheckerProperty = new P<CheckerViewModel>(this, "CurrentChecker");
        _CurrentPlayerProperty = new P<CheckerType>(this, "CurrentPlayer");
        _AllowedMovesProperty.CollectionChangedWith += AllowedMovesCollectionChanged;
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
    
    public virtual int RedScore {
        get {
            return _RedScoreProperty.Value;
        }
        set {
            _RedScoreProperty.Value = value;
        }
    }
    
    public virtual CheckerBoardViewModel Board {
        get {
            return _BoardProperty.Value;
        }
        set {
            _BoardProperty.Value = value;
            if (value != null) value.ParentCheckersGame = this;
        }
    }
    
    public virtual CheckerViewModel CurrentChecker {
        get {
            return _CurrentCheckerProperty.Value;
        }
        set {
            _CurrentCheckerProperty.Value = value;
            if (value != null) value.ParentCheckersGame = this;
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
    
    public override void Unbind() {
        base.Unbind();
        _AllowedMovesProperty.CollectionChangedWith -= AllowedMovesCollectionChanged;
    }
    
    protected override void FillProperties(List<ViewModelPropertyInfo> list) {
        base.FillProperties(list);;
        list.Add(new ViewModelPropertyInfo(_BlackScoreProperty, false, false, false));
        list.Add(new ViewModelPropertyInfo(_RedScoreProperty, false, false, false));
        list.Add(new ViewModelPropertyInfo(_BoardProperty, true, false, false));
        list.Add(new ViewModelPropertyInfo(_CurrentCheckerProperty, true, false, false));
        list.Add(new ViewModelPropertyInfo(_CurrentPlayerProperty, false, false, true));
        list.Add(new ViewModelPropertyInfo(_AllowedMovesProperty, true, true, false));
    }
    
    protected override void FillCommands(List<ViewModelCommandInfo> list) {
        base.FillCommands(list);;
        list.Add(new ViewModelCommandInfo("GameOver", GameOver));
    }
    
    private void AllowedMovesCollectionChanged(ModelCollectionChangeEventWith<CheckerMoveViewModel> args) {
        foreach (var item in args.OldItemsOfT) item.ParentCheckersGame = null;;
        foreach (var item in args.NewItemsOfT) item.ParentCheckersGame = this;;
    }
    
    public override void Write(ISerializerStream stream) {
		base.Write(stream);
		stream.SerializeInt("BlackScore", this.BlackScore);
		stream.SerializeInt("RedScore", this.RedScore);
		stream.SerializeObject("Board", this.Board);
		stream.SerializeObject("CurrentChecker", this.CurrentChecker);
		stream.SerializeInt("CurrentPlayer", (int)this.CurrentPlayer);
		stream.SerializeArray("AllowedMoves", this.AllowedMoves);
    }
    
    public override void Read(ISerializerStream stream) {
		base.Read(stream);
		this.BlackScore = stream.DeserializeInt("BlackScore");
		this.RedScore = stream.DeserializeInt("RedScore");
		this.Board = stream.DeserializeObject<CheckerBoardViewModel>("Board");
		this.CurrentChecker = stream.DeserializeObject<CheckerViewModel>("CurrentChecker");
		this.CurrentPlayer = (CheckerType)stream.DeserializeInt("CurrentPlayer");
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
    
    private CheckerBoardViewModel _ParentCheckerBoard;
    
    private CheckersGameViewModel _ParentCheckersGame;
    
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
    
    public virtual CheckerBoardViewModel ParentCheckerBoard {
        get {
            return this._ParentCheckerBoard;
        }
        set {
            _ParentCheckerBoard = value;
        }
    }
    
    public virtual CheckersGameViewModel ParentCheckersGame {
        get {
            return this._ParentCheckersGame;
        }
        set {
            _ParentCheckersGame = value;
        }
    }
    
    protected override void WireCommands(Controller controller) {
        var checker = controller as CheckerControllerBase;
        this.SelectCommand = new CommandWithSender<CheckerViewModel>(this, checker.SelectCommand);
    }
    
    public override void Unbind() {
        base.Unbind();
    }
    
    protected override void FillProperties(List<ViewModelPropertyInfo> list) {
        base.FillProperties(list);;
        list.Add(new ViewModelPropertyInfo(_IsKingMeProperty, false, false, false));
        list.Add(new ViewModelPropertyInfo(_PositionProperty, false, false, false));
        list.Add(new ViewModelPropertyInfo(_SelectedProperty, false, false, false));
        list.Add(new ViewModelPropertyInfo(_TypeProperty, false, false, true));
    }
    
    protected override void FillCommands(List<ViewModelCommandInfo> list) {
        base.FillCommands(list);;
        list.Add(new ViewModelCommandInfo("SelectCommand", SelectCommand));
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
    
    public readonly P<System.String> _String1Property;
    
    public AICheckersGameViewModel() : 
            base() {
        _String1Property = new P<string>(this, "String1");
    }
    
    public AICheckersGameViewModel(AICheckersGameControllerBase controller) : 
            this() {
        this.Controller = controller;
    }
    
    public virtual string String1 {
        get {
            return _String1Property.Value;
        }
        set {
            _String1Property.Value = value;
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
        list.Add(new ViewModelPropertyInfo(_String1Property, false, false, false));
    }
    
    protected override void FillCommands(List<ViewModelCommandInfo> list) {
        base.FillCommands(list);;
    }
    
    public override void Write(ISerializerStream stream) {
		base.Write(stream);
		stream.SerializeString("String1", this.String1);
    }
    
    public override void Read(ISerializerStream stream) {
		base.Read(stream);
		this.String1 = stream.DeserializeString("String1");
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

public enum CheckerType {
    
    Red,
    
    Black,
}
