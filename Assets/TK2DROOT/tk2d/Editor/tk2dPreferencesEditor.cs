using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class tk2dPreferences
{
	static tk2dPreferences _inst = null;	
	public static tk2dPreferences inst
	{
		get 
		{
			if (_inst == null)
			{
				_inst = new tk2dPreferences();
				_inst.Read();
			}
			return _inst;
		}
	}
	
	bool _displayTextureThumbs;
	bool _horizontalAnimDisplay;
	bool _autoRebuild;
	bool _showIds;

	public bool displayTextureThumbs { get { return _displayTextureThumbs; } set { if (_displayTextureThumbs != value) { _displayTextureThumbs = value; Write(); } } }
	public bool horizontalAnimDisplay { get { return _horizontalAnimDisplay; } set { if (_horizontalAnimDisplay != value) { _horizontalAnimDisplay = value; Write(); } } }
	public bool autoRebuild { get { return _autoRebuild; } set { if (_autoRebuild != value) { _autoRebuild = value; Write(); } } }
	public bool showIds { get { return _showIds; } set { if (_showIds != value) { _showIds = value; Write(); } } }
	
	void Read()
	{
		_displayTextureThumbs = EditorPrefs.GetBool("tk2d_displayTextureThumbs", true);
		_horizontalAnimDisplay = EditorPrefs.GetBool("tk2d_horizontalAnimDisplay", false);
		_autoRebuild = EditorPrefs.GetBool("tk2d_autoRebuild", true);
		_showIds = EditorPrefs.GetBool("tk2d_showIds", false);
	}
	
	public void Write()
	{
		EditorPrefs.SetBool("tk2d_displayTextureThumbs", _displayTextureThumbs);
		EditorPrefs.SetBool("tk2d_horizontalAnimDisplay", _horizontalAnimDisplay);
		EditorPrefs.SetBool("tk2d_autoRebuild", _autoRebuild);
		EditorPrefs.SetBool("tk2d_showIds", _showIds);
	}
}

public class tk2dPreferencesEditor : EditorWindow
{
	GUIContent label_spriteThumbnails = new GUIContent("Sprite Thumbnails", "Turn off sprite thumbnails to save memory.");
	
	GUIContent label_animationFrames = new GUIContent("Animation Frame Display", "Select the direction of frames in the SpriteAnimation inspector.");
	GUIContent label_animFrames_Horizontal = new GUIContent("Horizontal");
	GUIContent label_animFrames_Vertical = new GUIContent("Vertical");
	
	GUIContent label_autoRebuild = new GUIContent("Auto Rebuild", "Auto rebuild sprite collections when source textures have changed.");

	GUIContent label_showIds = new GUIContent("Show Ids", "Show sprite and animation Ids.");
	
	void OnGUI()
	{
		tk2dPreferences prefs = tk2dPreferences.inst;
		
		EditorGUIUtility.LookLikeControls(150.0f);
		
		prefs.displayTextureThumbs = EditorGUILayout.Toggle(label_spriteThumbnails, prefs.displayTextureThumbs);
		
		int had = EditorGUILayout.Popup(label_animationFrames, prefs.horizontalAnimDisplay?0:1, new GUIContent[] { label_animFrames_Horizontal, label_animFrames_Vertical } );
		prefs.horizontalAnimDisplay = (had == 0)?true:false;
		
		prefs.autoRebuild = EditorGUILayout.Toggle(label_autoRebuild, prefs.autoRebuild);
		
		prefs.showIds = EditorGUILayout.Toggle(label_showIds, prefs.showIds);
	}
}
