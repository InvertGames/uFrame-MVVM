using System;
using System.Collections;
using System.Linq;


[DiagramInfoAttribute("Checkers")]
public partial class CheckerBoardViewModel : ViewModel {
    
    public readonly ModelCollection<CheckerViewModel> _CheckersProperty = new ModelCollection<CheckerViewModel>();
    
    public readonly ModelCollection<CheckerPlateViewModel> _PlatesProperty = new ModelCollection<CheckerPlateViewModel>();
    
    public virtual System.Collections.Generic.ICollection<CheckerViewModel> Checkers {
        get {
            return _CheckersProperty;
        }
        set {
            _CheckersProperty.Value = value.ToList();
        }
    }
    
    public virtual System.Collections.Generic.ICollection<CheckerPlateViewModel> Plates {
        get {
            return _PlatesProperty;
        }
        set {
            _PlatesProperty.Value = value.ToList();
        }
    }
    
    public override void Write(ISerializerStream stream) {
		base.Write(stream);
    }
    
    public override void Read(ISerializerStream stream) {
		base.Read(stream);
    }
}

[DiagramInfoAttribute("Checkers")]
public partial class CheckerMoveViewModel : ViewModel {
    
    public override void Write(ISerializerStream stream) {
		base.Write(stream);
    }
    
    public override void Read(ISerializerStream stream) {
		base.Read(stream);
    }
}

[DiagramInfoAttribute("Checkers")]
public partial class CheckerPlateViewModel : ViewModel {
    
    public readonly P<System.Boolean> _CanMoveToProperty = new P<bool>();
    
    public readonly P<UnityEngine.Vector2> _PositionProperty = new P<UnityEngine.Vector2>();
    
    public readonly P<System.Boolean> _IsEvenProperty = new P<bool>();
    
    private ICommand _SelectCommand;
    
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
    
    public readonly P<System.Int32> _BlackScoreProperty = new P<int>();
    
    public readonly P<CheckerBoardViewModel> _BoardProperty = new P<CheckerBoardViewModel>();
    
    public readonly P<CheckerViewModel> _CurrentCheckerProperty = new P<CheckerViewModel>();
    
    public readonly P<CheckerType> _CurrentPlayerProperty = new P<CheckerType>();
    
    public readonly P<System.Int32> _RedScoreProperty = new P<int>();
    
    public readonly ModelCollection<CheckerMoveViewModel> _AllowedMovesProperty = new ModelCollection<CheckerMoveViewModel>();
    
    private ICommand _GameOver;
    
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
            _AllowedMovesProperty.Value = value.ToList();
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
    
    public override void Write(ISerializerStream stream) {
		base.Write(stream);
		stream.SerializeInt("BlackScore", this.BlackScore);
		stream.SerializeInt("CurrentPlayer", (int)this.CurrentPlayer);
		stream.SerializeInt("RedScore", this.RedScore);
    }
    
    public override void Read(ISerializerStream stream) {
		base.Read(stream);
		this.BlackScore = stream.DeserializeInt("BlackScore");
		this.CurrentPlayer = (CheckerType)stream.DeserializeInt("CurrentPlayer");
		this.RedScore = stream.DeserializeInt("RedScore");
    }
}

[DiagramInfoAttribute("Checkers")]
public partial class CheckerViewModel : ViewModel {
    
    public readonly P<System.Boolean> _IsKingMeProperty = new P<bool>();
    
    public readonly P<UnityEngine.Vector2> _PositionProperty = new P<UnityEngine.Vector2>();
    
    public readonly P<System.Boolean> _SelectedProperty = new P<bool>();
    
    public readonly P<CheckerType> _TypeProperty = new P<CheckerType>();
    
    private ICommand _SelectCommand;
    
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
    
    public virtual ICommand Play {
        get {
            return _Play;
        }
        set {
            _Play = value;
        }
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
