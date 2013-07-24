using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;


namespace SCGE 
{
	class SpriteLut
	{
		public int source; // index into source texture list, will only have multiple entries with same source, when splitting
		public Texture2D sourceTex;
		public Texture2D tex; // texture to atlas
		
		public bool isSplit; // is this part of a split?
		public int rx, ry, rw, rh; // split rectangle in texture coords
		
		public bool isDuplicate; // is this a duplicate texture?
		public int atlasIndex; // index in the atlas
	}
}

[CustomEditor(typeof(tk2dSpriteCollection))]
public class tk2dSpriteCollectionEditor : Editor
{
	int[] padAmountValues;
	string[] padAmountLabels;

	void OnEnable()
	{
		int MAX_PAD_AMOUNT = 18;
		padAmountValues = new int[MAX_PAD_AMOUNT];
		padAmountLabels = new string[MAX_PAD_AMOUNT];
		for (int i = 0; i < MAX_PAD_AMOUNT; ++i)
		{
			padAmountValues[i] = -1 + i;
			padAmountLabels[i] = (i==0)?"Default":((i-1).ToString());
		}
		
	}
	
	void OnDestroy()
	{
		tk2dSpriteThumbnailCache.ReleaseSpriteThumbnailCache();
	}
	
    public override void OnInspectorGUI()
    {
        tk2dSpriteCollection gen = (tk2dSpriteCollection)target;
        EditorGUILayout.BeginVertical();
		
		bool rebuild = false;
		bool edit = false;
		tk2dSpriteCollectionBuilder.ResetCurrentBuild();
		
		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button("Commit")) rebuild = true;
		GUILayout.Space(16.0f);
		if (GUILayout.Button("Edit...")) edit = true;
		EditorGUILayout.EndHorizontal();
		
		
		DrawDefaultInspector();
		
		// Draw additional stuff
		gen.padAmount = EditorGUILayout.IntPopup("Pad Amount", gen.padAmount, padAmountLabels, padAmountValues);
		
		
		DrawSizeView(gen);
		DrawAtlasView(gen);
		
		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button("Commit")) rebuild = true;
		GUILayout.Space(16.0f);
		if (GUILayout.Button("Edit...")) edit = true;
		EditorGUILayout.EndHorizontal();

		
		if (rebuild) 
		{
			tk2dSpriteCollectionBuilder.Rebuild(gen);
		}
		if (edit) 
		{
			if (gen.textureRefs != null && gen.textureRefs.Length > 0)
			{
				bool dirty = false;
				if (gen.textureRefs.Length != gen.textureParams.Length) 
				{
					dirty = true;
				}
				if (!dirty)
				{
					for (int i = 0; i < gen.textureRefs.Length; ++i)
					{
						if (gen.textureParams[i].fromSpriteSheet == false && gen.textureRefs[i] != gen.textureParams[i].texture)
						{
							dirty = true;
							break;
						}
					}
				}
				
				if (dirty)
				{
					tk2dSpriteCollectionBuilder.Rebuild(gen);
				}
				
				tk2dSpriteCollectionEditorPopup v = EditorWindow.GetWindow( typeof(tk2dSpriteCollectionEditorPopup), true, "Sprite Collection Editor" ) as tk2dSpriteCollectionEditorPopup;
				v.SetGenerator(gen);
			}
		}

        EditorGUILayout.EndVertical();
    }
	
	void DrawSizeView(tk2dSpriteCollection gen)
	{
		gen.useTk2dCamera = EditorGUILayout.Toggle("Use tk2d Camera", gen.useTk2dCamera);
		if (gen.useTk2dCamera)
		{
			gen.targetHeight = 1;
			gen.targetOrthoSize = 0.5f;
		}
		else
		{
			EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
			gen.targetHeight = EditorGUILayout.IntField("Target Height", gen.targetHeight);
			gen.targetOrthoSize = EditorGUILayout.FloatField("Target Ortho Size", gen.targetOrthoSize);
			EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
		}
	}
	
	bool displayAtlasFoldout = true;
	
	void DrawAtlasView(tk2dSpriteCollection gen)
	{
		EditorGUILayout.BeginVertical();
		
		int oldIndentLevel = EditorGUI.indentLevel;
		
		EditorGUI.indentLevel = 0;
		displayAtlasFoldout = EditorGUILayout.Foldout(displayAtlasFoldout, "Atlas");
		if (displayAtlasFoldout)
		{
			EditorGUI.indentLevel = 3;
			
			int[] allowedAtlasSizes = { 128, 256, 512, 1024, 2048, 4096 };
			string[] allowedAtlasSizesString = new string[allowedAtlasSizes.Length];
			for (int i = 0; i < allowedAtlasSizes.Length; ++i)
				allowedAtlasSizesString[i] = allowedAtlasSizes[i].ToString();
			
			gen.maxTextureSize = EditorGUILayout.IntPopup("Max Texture Size", gen.maxTextureSize, allowedAtlasSizesString, allowedAtlasSizes);
			gen.allowMultipleAtlases = EditorGUILayout.Toggle("Multiple Atlases", gen.allowMultipleAtlases);
			gen.textureCompression = (tk2dSpriteCollection.TextureCompression)EditorGUILayout.EnumPopup("Compression", gen.textureCompression);
			gen.forceSquareAtlas = EditorGUILayout.Toggle("Force Square", gen.forceSquareAtlas);
			
			if (gen.allowMultipleAtlases)
			{
				EditorGUILayout.LabelField("Num Atlases", gen.atlasTextures.Length.ToString());
			}
			else
			{
				EditorGUILayout.LabelField("Atlas Width", gen.atlasWidth.ToString());
				EditorGUILayout.LabelField("Atlas Height", gen.atlasHeight.ToString());
				EditorGUILayout.LabelField("Atlas Wastage", gen.atlasWastage.ToString("0.00") + "%");
			}
		}
		
		EditorGUILayout.EndVertical();
		EditorGUI.indentLevel = oldIndentLevel;
	}
	

	
	// Menu entries
	
	[MenuItem("Assets/Create/tk2d/Sprite Collection", false, 10000)]
    static void DoCollectionCreate()
    {
		string path = tk2dEditorUtility.CreateNewPrefab("SpriteCollection");
        if (path != null)
        {
            GameObject go = new GameObject();
            go.AddComponent<tk2dSpriteCollection>();
            go.active = false;

#if (UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_4)
			Object p = EditorUtility.CreateEmptyPrefab(path);
            EditorUtility.ReplacePrefab(go, p, ReplacePrefabOptions.ConnectToPrefab);
#else
			Object p = PrefabUtility.CreateEmptyPrefab(path);
            PrefabUtility.ReplacePrefab(go, p, ReplacePrefabOptions.ConnectToPrefab);
#endif
			
            GameObject.DestroyImmediate(go);
        }
    }	
}
