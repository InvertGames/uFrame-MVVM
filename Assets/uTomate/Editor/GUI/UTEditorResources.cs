//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Holds all resources used by uTomate's editor GUI.
/// </summary>
public class UTEditorResources
{
	private static GUIStyle _ExpressionButtonStyle;
	private static GUIStyle _FolderButtonStyle;
	private static GUIStyle _DeleteButtonStyle;
	private static GUIStyle _HelpButtonStyle;
	private static GUIStyle _GroupStyle;
	private static GUIStyle _GraphBackgroundStyle;
	private static GUIStyle _GraphNodeSelectedStyle;
	private static GUIStyle _GraphNodeHighlightStyle;
	private static GUIStyle _GraphNodeStyle;
	private static GUIStyle _GraphCommentStyle;
	private static GUIStyle _GraphCommentSelectedStyle;
	private static GUIStyle _GraphNodeConnectorStyle;
	private static GUIStyle _GraphNodeConnectorStyleEmpty;
	private static GUIStyle _GraphNodeHeaderStyle;
	private static GUIStyle _GraphNodeTextStyle;
	private static GUIStyle _GraphNodeConnectorLabelCenteredStyle;
	private static GUIStyle _GraphNodeConnectorLabelLeftStyle;
	private static GUIStyle _GraphNodeConnectorLabelRightStyle;
	private static Texture _FirstNodeTexture;
	private static Texture _ExecutePlanTexture;
	private static Texture2D _LineTexture;
	private static GUIContent _HelpIcon;
	private static GUIContent _DeleteIcon;
	private static GUIContent _FolderIcon;
	private static GUIContent _PrivateIcon;
	private static GUIContent _PublicIcon;
	private static GUIStyle _ArrayLabelStyle;
#if UTOMATE_DEMO
	private static GUIStyle _DemoVersionLabelStyle;
#endif
	private static GUIStyle _ProgressBarStyle;
	private static GUIStyle _RunAgainButtonStyle;
	private static GUIContent _RunAgainIcon;
	private static GUIStyle _RunAgainBoxStyle;
	private static GUIStyle _CancelButtonStyle;
	private static GUIContent _CancelIcon;
	private static GUIStyle _SelectedPropertyStyle;
	public static GUIContent ExpressionButton = new GUIContent ("f(x)", "Use expression?");
	private static CUEditorAssetUtility assetUtility;
	
	public static string VariableWarningText {
		get {
			return "Your string seems to contain a variable reference. Did you forget to tick the f(x) button?";
		}
	}
	
	public static Texture2D LineTexture {
		get {
			if (_LineTexture == null) {
				_LineTexture = new Texture2D (2, 2);
			}
			return _LineTexture;
		}
	}

	public static Texture FirstNodeTexture {
		get {
			if (_FirstNodeTexture == null) {
				_FirstNodeTexture = LoadTexture ("1stNode.png");
			}
			return _FirstNodeTexture;
		}
	}

	public static Texture ExecutePlanTexture {
		get {
			if (_ExecutePlanTexture == null) {
				_ExecutePlanTexture = LoadTexture ("ExecutePlanNode.png");
			}
			return _ExecutePlanTexture;
		}
	}
	
	public static GUIStyle GraphBackgroundStyle {
		get {
			
			if (_GraphBackgroundStyle == null) {
				_GraphBackgroundStyle = new GUIStyle ();
				var tex = LoadTexture ("NodeEditorBackground.png");
				_GraphBackgroundStyle.border = new RectOffset (115, 3, 45, 3);
				_GraphBackgroundStyle.normal.background = tex;
				_GraphBackgroundStyle.active.background = tex;
				_GraphBackgroundStyle.focused.background = tex;
			}
			return _GraphBackgroundStyle;
		}
	}
	
	public static GUIStyle GraphCommentStyle {
		get {
			if (_GraphCommentStyle == null) {
				_GraphCommentStyle = new GUIStyle ();
				var tex = LoadTexture ("CommentBackground.png");
				_GraphCommentStyle.border = new RectOffset (25, 25, 35, 25);
				_GraphCommentStyle.overflow = new RectOffset (13, 13, 13, 13);
				_GraphCommentStyle.normal.background = tex;
				_GraphCommentStyle.active.background = tex;
				_GraphCommentStyle.focused.background = tex;
			}
			return _GraphCommentStyle;
		}
	}
	
	public static GUIStyle GraphCommentSelectedStyle {
		get {
			if (_GraphCommentSelectedStyle == null) {
				_GraphCommentSelectedStyle = new GUIStyle ();
				var tex = LoadTexture ("CommentBackgroundSelected.png");
				_GraphCommentSelectedStyle.border = new RectOffset (25, 25, 35, 25);
				_GraphCommentSelectedStyle.overflow = new RectOffset (13, 13, 13, 13);
				_GraphCommentSelectedStyle.normal.background = tex;
				_GraphCommentSelectedStyle.active.background = tex;
				_GraphCommentSelectedStyle.focused.background = tex;
			}
			return _GraphCommentSelectedStyle;
		}
	}
	
	public static GUIStyle GraphNodeStyle {
		get {
			if (_GraphNodeStyle == null) {
				_GraphNodeStyle = new GUIStyle ();
				var tex = LoadTexture ("NodeBackground.png");
				_GraphNodeStyle.border = new RectOffset (25, 25, 35, 25);
				_GraphNodeStyle.overflow = new RectOffset (13, 13, 13, 13);
				_GraphNodeStyle.normal.background = tex;
				_GraphNodeStyle.active.background = tex;
				_GraphNodeStyle.focused.background = tex;
			}
			return _GraphNodeStyle;
		}
	}
	
	public static GUIStyle GraphNodeSelectedStyle {
		get {
			if (_GraphNodeSelectedStyle == null) {
				_GraphNodeSelectedStyle = new GUIStyle ();
				var tex = LoadTexture ("NodeBackgroundSelected.png");
				_GraphNodeSelectedStyle.border = new RectOffset (25, 25, 35, 25);
				_GraphNodeSelectedStyle.overflow = new RectOffset (13, 13, 13, 13);
				_GraphNodeSelectedStyle.normal.background = tex;
				_GraphNodeSelectedStyle.active.background = tex;
				_GraphNodeSelectedStyle.focused.background = tex;
			}
			return _GraphNodeSelectedStyle;
		}
	}

	public static GUIStyle GraphNodeHighlightStyle {
		get {
			if (_GraphNodeHighlightStyle == null) {
				_GraphNodeHighlightStyle = new GUIStyle ();
				var tex = LoadTexture ("NodeBackgroundRunning.png");
				_GraphNodeHighlightStyle.border = new RectOffset (25, 25, 35, 25);
				_GraphNodeHighlightStyle.overflow = new RectOffset (13, 13, 13, 13);
				_GraphNodeHighlightStyle.normal.background = tex;
				_GraphNodeHighlightStyle.active.background = tex;
				_GraphNodeHighlightStyle.focused.background = tex;
			}
			return _GraphNodeHighlightStyle;
		}
	}

	public static GUIStyle GraphNodeConnectorStyle {
		get {
			if (_GraphNodeConnectorStyle == null) {
				_GraphNodeConnectorStyle = new GUIStyle ();
				var tex = LoadTexture ("NodeConnector.png");
				_GraphNodeConnectorStyle.fixedWidth = 14;
				_GraphNodeConnectorStyle.fixedHeight = 14;
				_GraphNodeConnectorStyle.normal.background = tex;
				_GraphNodeConnectorStyle.active.background = tex;
				_GraphNodeConnectorStyle.focused.background = tex;
			}
			return _GraphNodeConnectorStyle;
		}
	}

	public static GUIStyle GraphNodeConnectorStyleEmpty {
		get {
			if (_GraphNodeConnectorStyleEmpty == null) {
				_GraphNodeConnectorStyleEmpty = new GUIStyle ();
				var tex = LoadTexture ("NodeConnectorEmpty.png");
				_GraphNodeConnectorStyleEmpty.fixedWidth = 14;
				_GraphNodeConnectorStyleEmpty.fixedHeight = 14;
				_GraphNodeConnectorStyleEmpty.normal.background = tex;
				_GraphNodeConnectorStyleEmpty.active.background = tex;
				_GraphNodeConnectorStyleEmpty.focused.background = tex;
			}
			return _GraphNodeConnectorStyleEmpty;
		}
	}
		
	public static GUIStyle GraphNodeHeaderStyle {
		get {
			if (_GraphNodeHeaderStyle == null) {
				_GraphNodeHeaderStyle = new GUIStyle ();
				_GraphNodeHeaderStyle.normal.textColor = Color.white;
				_GraphNodeHeaderStyle.fontStyle = FontStyle.Bold;
				_GraphNodeHeaderStyle.padding = new RectOffset (6, 6, 3, 0);
				_GraphNodeHeaderStyle.alignment = TextAnchor.UpperCenter;
			}
			return _GraphNodeHeaderStyle;
		}
	}
	
	public static GUIStyle GraphNodeTextStyle {
		get {
			if (_GraphNodeTextStyle == null) {
				_GraphNodeTextStyle = new GUIStyle ();
				_GraphNodeTextStyle.normal.textColor = Color.black;
				_GraphNodeTextStyle.fontStyle = FontStyle.Normal;
				_GraphNodeTextStyle.padding = new RectOffset (6, 6, 6, 6);
				_GraphNodeTextStyle.alignment = TextAnchor.UpperLeft;
				_GraphNodeTextStyle.wordWrap = true;
			}
			return _GraphNodeTextStyle;
		}
	}

	public static GUIStyle GraphNodeConnectorLabelCenteredStyle {
		get {
			if (_GraphNodeConnectorLabelCenteredStyle == null) {
				_GraphNodeConnectorLabelCenteredStyle = new GUIStyle ();
				_GraphNodeConnectorLabelCenteredStyle.normal.textColor = Color.black;
				_GraphNodeConnectorLabelCenteredStyle.padding = new RectOffset (6, 6, 0, 0);
				_GraphNodeConnectorLabelCenteredStyle.alignment = TextAnchor.UpperCenter;
			}
			return _GraphNodeConnectorLabelCenteredStyle;
		}
	}
	
	public static GUIStyle GraphNodeConnectorLabelLeftStyle {
		get {
			if (_GraphNodeConnectorLabelLeftStyle == null) {
				_GraphNodeConnectorLabelLeftStyle = new GUIStyle ();
				_GraphNodeConnectorLabelLeftStyle.normal.textColor = Color.black;
				_GraphNodeConnectorLabelLeftStyle.padding = new RectOffset (13, 6, 0, 0);
				_GraphNodeConnectorLabelLeftStyle.alignment = TextAnchor.UpperLeft;
			}
			return _GraphNodeConnectorLabelLeftStyle;
		}
	}

	public static GUIStyle GraphNodeConnectorLabelRightStyle {
		get {
			if (_GraphNodeConnectorLabelRightStyle == null) {
				_GraphNodeConnectorLabelRightStyle = new GUIStyle ();
				_GraphNodeConnectorLabelRightStyle.normal.textColor = Color.black;
				_GraphNodeConnectorLabelRightStyle.padding = new RectOffset (6, 13, 0, 0);
				_GraphNodeConnectorLabelRightStyle.alignment = TextAnchor.UpperRight;
			}
			return _GraphNodeConnectorLabelRightStyle;
		}
	}
	
	public static GUIStyle ExpressionButtonStyle {
		get {
			if (_ExpressionButtonStyle == null) {
				_ExpressionButtonStyle = new GUIStyle (EditorStyles.miniButton) {
			        stretchWidth = false,
			        stretchHeight = false,
					fixedWidth = 35
			      };
			}
			return _ExpressionButtonStyle;
		}
	}
		
	public static GUIStyle ProgressBarStyle {
		get {
			if (_ProgressBarStyle == null) {
				_ProgressBarStyle = new GUIStyle () {
			        fixedHeight = 20,
					margin = new RectOffset (5, 5, 2, 2),
					stretchWidth = true,
			        stretchHeight = false
			      };
			}
			return _ProgressBarStyle;
		}
	}
	
	public static GUIStyle FolderButtonStyle {
		get {
			if (_FolderButtonStyle == null) {
				_FolderButtonStyle = new GUIStyle (GUIStyle.none) {
					fixedWidth = 16,
					margin = new RectOffset (0, 0, 2, 0)
			      };
			}
			return _FolderButtonStyle;
		}
	}

	public static GUIStyle DeleteButtonStyle {
		get {
			if (_DeleteButtonStyle == null) {
				_DeleteButtonStyle = new GUIStyle (GUIStyle.none) {
					fixedWidth = 16,
					margin = new RectOffset (0, 0, 2, 0)
			      };
			}
			return _DeleteButtonStyle;
		}
	}

	public static GUIStyle HelpButtonStyle {
		get {
			if (_HelpButtonStyle == null) {
				_HelpButtonStyle = new GUIStyle (GUIStyle.none) {
					fixedWidth = 16,
					padding = new RectOffset (0, 0, 2, 0)
			      };
			}
			return _HelpButtonStyle;
		}
	}

	public static GUIStyle TitleStyle {
		get {
			return CUListStyle.DefaultStyle.titleStyle;
		}
	}
	
	public static GUIStyle GroupStyle {
		get {
			if (_GroupStyle == null) {
				_GroupStyle = new GUIStyle (TitleStyle);
				_GroupStyle.fontStyle = FontStyle.Normal;
				_GroupStyle.normal.textColor = Color.gray;
			}
			return _GroupStyle;
		}
	}
	
	public static GUIStyle ArrayLabelStyle {
		get {
			if (_ArrayLabelStyle == null) {
				_ArrayLabelStyle = new GUIStyle (EditorGUIUtility.GetBuiltinSkin (EditorSkin.Inspector).label);
				_ArrayLabelStyle.normal.textColor = Color.gray;
			}
			return _ArrayLabelStyle;
		}
	}

#if UTOMATE_DEMO
	public static GUIStyle DemoVersionLabelStyle {
		get {
			if (_DemoVersionLabelStyle == null) {
				_DemoVersionLabelStyle = new GUIStyle (EditorGUIUtility.GetBuiltinSkin (EditorSkin.Inspector).label);
				_DemoVersionLabelStyle.normal.textColor = Color.yellow;
				_DemoVersionLabelStyle.fontSize = 18;
				_DemoVersionLabelStyle.padding = new RectOffset(5,5,5,5);
			}
			return _DemoVersionLabelStyle;
		}
	}
#endif

	public static GUIContent HelpIcon {
		get {
			if (_HelpIcon == null) {
				_HelpIcon = new GUIContent (EditorGUIUtility.Load ("icons/_help.png") as Texture2D, "Show help");
			}
			return _HelpIcon;
		}
	}

	public static GUIContent DeleteIcon {
		get {
			if (_DeleteIcon == null) {
				_DeleteIcon = new GUIContent (EditorGUIUtility.Load ("icons/treeeditor.trash.png") as Texture2D, "Delete");
			}
			return _DeleteIcon;
		}
	}

	public static GUIContent FolderIcon {
		get {
			if (_FolderIcon == null) {
				_FolderIcon = new GUIContent (LoadTexture ("FolderIcon.png"), "Select");
			}
			return _FolderIcon;
		}
	}
	
	public static GUIContent PublicIcon {
		get {
			if (_PublicIcon == null) {
				_PublicIcon = new GUIContent (LoadTexture ("UTPublic.png"), "Enable privacy");
			}
			return _PublicIcon;
		}
	}
	
	public static GUIContent PrivateIcon {
		get {
			if (_PrivateIcon == null) {
				_PrivateIcon = new GUIContent (LoadTexture ("UTPrivate.png"), "Disable privacy");
			}
			return _PrivateIcon;
		}
	}
	
	public static GUIStyle RunAgainButtonStyle {
		get {
			if (_RunAgainButtonStyle == null) {
				_RunAgainButtonStyle = new GUIStyle (GUIStyle.none) {
					fixedWidth = 23,
					fixedHeight = 23,
					padding = new RectOffset (4, 4, 4, 4)
				};
			}
			return _RunAgainButtonStyle;
		}
	}

	public static GUIContent RunAgainIcon {
		get {
			if (_RunAgainIcon == null) {
				_RunAgainIcon = new GUIContent (LoadTexture ("UTAgain.png"), "Run again");
			}
			return _RunAgainIcon;
		}
	}
	
	public static GUIStyle RunAgainBoxStyle {
		get {
			if (_RunAgainBoxStyle == null) {
				_RunAgainBoxStyle = new GUIStyle (GUIStyle.none) {
					margin = new RectOffset (4, 4, 4, 4)
				};
			}
			return _RunAgainBoxStyle;
		}
	}

	public static GUIStyle CancelButtonStyle {
		get {
			if (_CancelButtonStyle == null) {
				_CancelButtonStyle = new GUIStyle (GUIStyle.none) {
					fixedWidth = 15,
					fixedHeight = 15,
					margin = new RectOffset (4, 4, 4, 4)
				//padding = new RectOffset(4,4,4,4)
				};
			}
			return _CancelButtonStyle;
		}
	}

	public static GUIContent CancelIcon {
		get {
			if (_CancelIcon == null) {
				_CancelIcon = new GUIContent (LoadTexture ("UTCancel.png"), "Cancel");
			}
			return _CancelIcon;
		}
	}
	
	public static GUIStyle SelectedPropertyStyle {
		get {
			if (_SelectedPropertyStyle == null) {
				_SelectedPropertyStyle = new GUIStyle ();
				if (EditorGUIUtility.isProSkin) {
					_SelectedPropertyStyle.normal.background = LoadTexture ("HighlightPropertyBackgroundDark.png");
				} else {
					_SelectedPropertyStyle.normal.background = LoadTexture ("HighlightPropertyBackgroundLight.png");
				}
				
			}
			return _SelectedPropertyStyle;
		}
	}
	
	public static Texture2D LoadTexture(string name) { 
		if (assetUtility == null) {
			assetUtility = new CUEditorAssetUtility(UTEditorResourcesLocator.ResourcePath, "uTomate", "UTEditorResourcesLocator");
		}
		return assetUtility.FindTexture(name);
    }
}

