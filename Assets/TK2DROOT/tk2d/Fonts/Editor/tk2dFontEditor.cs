using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;

[CustomEditor(typeof(tk2dFont))]
public class tk2dFontEditor : Editor 
{
	public Shader GetShader(bool gradient)
	{
		if (gradient) return Shader.Find("tk2d/Blend2TexVertexColor");
		else return Shader.Find("tk2d/BlendVertexColor");
	}
	
	public override void OnInspectorGUI()
	{
		tk2dFont gen = (tk2dFont)target;
		EditorGUILayout.BeginVertical();

		DrawDefaultInspector();
		
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
		
		// Warning when texture is compressed
		if (gen.texture != null)
		{
			Texture2D tex = (Texture2D)gen.texture;
			if (tex && IsTextureCompressed(tex))
			{
				int buttonPressed;
				if ((buttonPressed = tk2dGuiUtility.InfoBoxWithButtons(
					"Font texture appears to be compressed. " +
					"Quality will be lost and the texture may appear blocky in game.\n" +
					"Do you wish to change the format?", 
					tk2dGuiUtility.WarningLevel.Warning, 
					new string[] { "16bit", "Truecolor" }
					)) != -1)
				{
					if (buttonPressed == 0)
					{
						ConvertTextureToFormat(tex, TextureImporterFormat.Automatic16bit);
					}
					else
					{
						ConvertTextureToFormat(tex, TextureImporterFormat.AutomaticTruecolor);
					}
				}
			}
		}
		
		// Warning when gradient texture is compressed
		if (gen.gradientTexture != null && 
			(gen.gradientTexture.format != TextureFormat.ARGB32 && gen.gradientTexture.format != TextureFormat.RGB24 && gen.gradientTexture.format != TextureFormat.RGBA32))
		{
			if (tk2dGuiUtility.InfoBoxWithButtons(
				"The gradient texture should be truecolor for best quality. " +
				"Current format is " + gen.gradientTexture.format.ToString() + ".",
				tk2dGuiUtility.WarningLevel.Warning,
				new string[] { "Fix" }
				) != -1)
			{
				ConvertTextureToFormat(gen.gradientTexture, TextureImporterFormat.AutomaticTruecolor);
			}
		}

		if (GUILayout.Button("Commit..."))
		{
			if (gen.bmFont == null || gen.texture == null)
			{
				EditorUtility.DisplayDialog("BMFont", "Need an bmFont and texture bound to work", "Ok");
				return;
			}
			
			if (gen.material == null)
			{
				gen.material = new Material(GetShader(gen.gradientTexture != null));
				string materialPath = AssetDatabase.GetAssetPath(gen).Replace(".prefab", "material.mat");
				AssetDatabase.CreateAsset(gen.material, materialPath);
			}
			
			if (gen.data == null)
			{
				string bmFontPath = AssetDatabase.GetAssetPath(gen).Replace(".prefab", "data.prefab");
				
				GameObject go = new GameObject();
				go.AddComponent<tk2dFontData>();
				go.active = false;
				
#if (UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_4)
				Object p = EditorUtility.CreateEmptyPrefab(bmFontPath);
				EditorUtility.ReplacePrefab(go, p);
#else
				Object p = PrefabUtility.CreateEmptyPrefab(bmFontPath);
				PrefabUtility.ReplacePrefab(go, p);
#endif
				GameObject.DestroyImmediate(go);
				AssetDatabase.SaveAssets();
				
				gen.data = AssetDatabase.LoadAssetAtPath(bmFontPath, typeof(tk2dFontData)) as tk2dFontData;
			}
			
			ParseBMFont(AssetDatabase.GetAssetPath(gen.bmFont), gen.data, gen);

			if (gen.manageMaterial)
			{
				Shader s = GetShader(gen.gradientTexture != null);
				if (gen.material.shader != s)
				{
					gen.material.shader = s;
					EditorUtility.SetDirty(gen.material);
				}
				if (gen.material.mainTexture != gen.texture)
				{
					gen.material.mainTexture = gen.texture;
					EditorUtility.SetDirty(gen.material);
				}
				if (gen.gradientTexture != null && gen.gradientTexture != gen.material.GetTexture("_GradientTex"))
				{
					gen.material.SetTexture("_GradientTex", gen.gradientTexture);
					EditorUtility.SetDirty(gen.material);
				}
			}
			
			gen.data.version = tk2dFontData.CURRENT_VERSION;

			gen.data.material = gen.material;
			gen.data.textureGradients = gen.gradientTexture != null;
			gen.data.gradientCount = gen.gradientCount;
			gen.data.gradientTexture = gen.gradientTexture;
			
			gen.data.invOrthoSize = 1.0f / gen.targetOrthoSize;
			gen.data.halfTargetHeight = gen.targetHeight * 0.5f;
			
            // Rebuild assets already present in the scene
            tk2dTextMesh[] sprs = Resources.FindObjectsOfTypeAll(typeof(tk2dTextMesh)) as tk2dTextMesh[];
            foreach (tk2dTextMesh spr in sprs)
            {
                spr.Init(true);
            }
			
			EditorUtility.SetDirty(gen);
			EditorUtility.SetDirty(gen.data);
        }

		EditorGUILayout.EndVertical();
	}
	
	bool IsTextureCompressed(Texture2D texture)
	{
		if (texture.format == TextureFormat.ARGB32 
			|| texture.format == TextureFormat.ARGB4444 
			|| texture.format == TextureFormat.Alpha8 
			|| texture.format == TextureFormat.RGB24 
			|| texture.format == TextureFormat.RGB565 
			|| texture.format == TextureFormat.RGBA32)
		{
			return false;
		}
		else
		{
			return true;
		}
	}
	
	void ConvertTextureToFormat(Texture2D texture, TextureImporterFormat format)
	{
		string assetPath = AssetDatabase.GetAssetPath(texture);
		if (assetPath != "")
		{
			// make sure the source texture is npot and readable, and uncompressed
        	TextureImporter importer = (TextureImporter)TextureImporter.GetAtPath(assetPath);
			if (importer.textureFormat != format)
				importer.textureFormat = format;
			
			AssetDatabase.ImportAsset(assetPath);
		}
	}
	
	// Internal structures to fill and process
	class IntChar
	{
		public int id = 0, x = 0, y = 0, width = 0, height = 0, xoffset = 0, yoffset = 0, xadvance = 0;
	};
	
	class IntKerning
	{
		public int first = 0, second = 0, amount = 0;
	};
	
	class IntFontInfo
	{
		public int scaleW = 0, scaleH = 0;
		public int lineHeight = 0;
		
		public List<IntChar> chars = new List<IntChar>();
		public List<IntKerning> kernings = new List<IntKerning>();
	};
	
	
	IntFontInfo ParseBMFontXml(XmlDocument doc)
	{
		IntFontInfo fontInfo = new IntFontInfo();
		
        XmlNode nodeCommon = doc.SelectSingleNode("/font/common");
		fontInfo.scaleW = ReadIntAttribute(nodeCommon, "scaleW");
		fontInfo.scaleH = ReadIntAttribute(nodeCommon, "scaleH");
		fontInfo.lineHeight = ReadIntAttribute(nodeCommon, "lineHeight");
		int pages = ReadIntAttribute(nodeCommon, "pages");
		if (pages != 1)
		{
			EditorUtility.DisplayDialog("Fatal error", "Only one page supported in font. Please change the setting and re-export.", "Ok");
			return null;
		}

		foreach (XmlNode node in doc.SelectNodes(("/font/chars/char")))
		{
			IntChar thisChar = new IntChar();
			thisChar.id = ReadIntAttribute(node, "id");
            thisChar.x = ReadIntAttribute(node, "x");
            thisChar.y = ReadIntAttribute(node, "y");
            thisChar.width = ReadIntAttribute(node, "width");
            thisChar.height = ReadIntAttribute(node, "height");
            thisChar.xoffset = ReadIntAttribute(node, "xoffset");
            thisChar.yoffset = ReadIntAttribute(node, "yoffset");
            thisChar.xadvance = ReadIntAttribute(node, "xadvance");
			
			fontInfo.chars.Add(thisChar);
		}
		
		foreach (XmlNode node in doc.SelectNodes("/font/kernings/kerning"))
		{
			IntKerning thisKerning = new IntKerning();
			thisKerning.first = ReadIntAttribute(node, "first");
			thisKerning.second = ReadIntAttribute(node, "second");
			thisKerning.amount = ReadIntAttribute(node, "amount");
			
			fontInfo.kernings.Add(thisKerning);
		}

		return fontInfo;
	}
	
	string FindKeyValue(string[] tokens, string key)
	{
		string keyMatch = key + "=";
		for (int i = 0; i < tokens.Length; ++i)
		{
			if (tokens[i].Length > keyMatch.Length && tokens[i].Substring(0, keyMatch.Length) == keyMatch)
				return tokens[i].Substring(keyMatch.Length);
		}
		
		return "";
	}
	
	IntFontInfo ParseBMFontText(string path)
	{
		IntFontInfo fontInfo = new IntFontInfo();
		
		FileInfo finfo = new FileInfo(path);
		StreamReader reader = finfo.OpenText();
		string line;
		while ((line = reader.ReadLine()) != null) 
		{
			string[] tokens = line.Split( ' ' );
			
			if (tokens[0] == "common")
			{
				fontInfo.lineHeight = int.Parse( FindKeyValue(tokens, "lineHeight") );
				fontInfo.scaleW = int.Parse( FindKeyValue(tokens, "scaleW") );
				fontInfo.scaleH = int.Parse( FindKeyValue(tokens, "scaleH") );
				int pages = int.Parse( FindKeyValue(tokens, "pages") );
				if (pages != 1)
				{
					EditorUtility.DisplayDialog("Fatal error", "Only one page supported in font. Please change the setting and re-export.", "Ok");
					return null;
				}
			}
			else if (tokens[0] == "char")
			{
				IntChar thisChar = new IntChar();
				thisChar.id = int.Parse(FindKeyValue(tokens, "id"));
				thisChar.x = int.Parse(FindKeyValue(tokens, "x"));
				thisChar.y = int.Parse(FindKeyValue(tokens, "y"));
				thisChar.width = int.Parse(FindKeyValue(tokens, "width"));
				thisChar.height = int.Parse(FindKeyValue(tokens, "height"));
				thisChar.xoffset = int.Parse(FindKeyValue(tokens, "xoffset"));
				thisChar.yoffset = int.Parse(FindKeyValue(tokens, "yoffset"));
				thisChar.xadvance = int.Parse(FindKeyValue(tokens, "xadvance"));
				fontInfo.chars.Add(thisChar);
			}
			else if (tokens[0] == "kerning")
			{
				IntKerning thisKerning = new IntKerning();
				thisKerning.first = int.Parse(FindKeyValue(tokens, "first"));
				thisKerning.second = int.Parse(FindKeyValue(tokens, "second"));
				thisKerning.amount = int.Parse(FindKeyValue(tokens, "amount"));
				fontInfo.kernings.Add(thisKerning);
			}
		}
		reader.Close();
		
		return fontInfo;
	}
	
	bool ParseBMFont(string path, tk2dFontData fontData, tk2dFont source)
	{
		IntFontInfo fontInfo = null;
		
		try
		{
			XmlDocument doc = new XmlDocument();
			doc.Load(path);
			fontInfo = ParseBMFontXml(doc);
		}
		catch
		{
			fontInfo = ParseBMFontText(path);
		}
		
		if (fontInfo == null || fontInfo.chars.Count == 0)
			return false;
	
		float texWidth = fontInfo.scaleW;
        float texHeight = fontInfo.scaleH;
        float lineHeight = fontInfo.lineHeight;

		float scale = 2.0f * source.targetOrthoSize / source.targetHeight;

        fontData.lineHeight = lineHeight * scale;
		
		// Get number of characters (lastindex + 1)
		int maxCharId = 0;
		int maxUnicodeChar = 100000;
		foreach (var theChar in fontInfo.chars)
		{
			if (theChar.id > maxUnicodeChar)
			{
				// in most cases the font contains unwanted characters!
				Debug.LogError("Unicode character id exceeds allowed limit: " + theChar.id.ToString() + ". Skipping.");
				continue;
			}
			
			if (theChar.id > maxCharId) maxCharId = theChar.id;
		}
		
		// decide to use dictionary if necessary
		// 2048 is a conservative lower floor
		bool useDictionary = maxCharId > 2048;
		
		Dictionary<int, tk2dFontChar> charDict = (useDictionary)?new Dictionary<int, tk2dFontChar>():null;
		tk2dFontChar[] chars = (useDictionary)?null:new tk2dFontChar[maxCharId + 1];
		int minChar = 0x7fffffff;
		int maxCharWithinBounds = 0;
		int numLocalChars = 0;
		float largestWidth = 0.0f;
		foreach (var theChar in fontInfo.chars)
		{
			tk2dFontChar thisChar = new tk2dFontChar();
			int id = theChar.id;
            int x = theChar.x;
            int y = theChar.y;
            int width = theChar.width;
            int height = theChar.height;
            int xoffset = theChar.xoffset;
            int yoffset = theChar.yoffset;
            int xadvance = theChar.xadvance + source.charPadX;
			
			// special case, if the width and height are zero, the origin doesn't need to be offset
			// handles problematic case highlighted here:
			// http://unikronsoftware.com/2dtoolkit/forum/index.php/topic,89.msg220.html
			if (width == 0 && height == 0)
			{
				xoffset = 0;
				yoffset = 0;
			}

            // precompute required data
            float px = xoffset * scale;
            float py = (lineHeight - yoffset) * scale;

            thisChar.p0 = new Vector3(px, py, 0);
            thisChar.p1 = new Vector3(px + width * scale, py - height * scale, 0);
			
			if (source.flipTextureY)
			{
	            thisChar.uv0 = new Vector2(x / texWidth, y / texHeight);
	            thisChar.uv1 = new Vector2(thisChar.uv0.x + width / texWidth, thisChar.uv0.y + height / texHeight);
			}
			else
			{
	            thisChar.uv0 = new Vector2(x / texWidth, 1.0f - y / texHeight);
	            thisChar.uv1 = new Vector2(thisChar.uv0.x + width / texWidth, thisChar.uv0.y - height / texHeight);
			}
            thisChar.advance = xadvance * scale;
			largestWidth = Mathf.Max(thisChar.advance, largestWidth);
			
			// Needs gradient data
			if (source.gradientTexture != null)
			{
				// build it up assuming the first gradient
				float x0 = (float)(0.0f / source.gradientCount);
				float x1 = (float)(1.0f / source.gradientCount);
				float y0 = 1.0f;
				float y1 = 0.0f;

				// align to glyph if necessary
				
				thisChar.gradientUv = new Vector2[4];
				thisChar.gradientUv[0] = new Vector2(x0, y0);
				thisChar.gradientUv[1] = new Vector2(x1, y0);
				thisChar.gradientUv[2] = new Vector2(x0, y1);
				thisChar.gradientUv[3] = new Vector2(x1, y1);
			}

			if (id <= maxCharId)
			{
				maxCharWithinBounds = (id > maxCharWithinBounds) ? id : maxCharWithinBounds;
				minChar = (id < minChar) ? id : minChar;
				
				if (useDictionary)
					charDict[id] = thisChar;
				else
					chars[id] = thisChar;
				
				++numLocalChars;
			}
		}
		
		// duplicate capitals to lower case, or vice versa depending on which ones exist
        if (source.dupeCaps)
        {
            for (int uc = 'A'; uc <= 'Z'; ++uc)
            {
                int lc = uc + ('a' - 'A');
				
				if (useDictionary)
				{
					if (charDict.ContainsKey(uc))
						charDict[lc] = charDict[uc];
					else if (charDict.ContainsKey(lc))
						charDict[uc] = charDict[lc];
				}
				else
				{
	                if (chars[lc] == null) chars[lc] = chars[uc];
	                else if (chars[uc] == null) chars[uc] = chars[lc];
				}
            }
        }
		
		// share null char, same pointer
		var nullChar = new tk2dFontChar();
		nullChar.gradientUv = new Vector2[4]; // this would be null otherwise
		
		fontData.largestWidth = largestWidth;
		if (useDictionary)
		{
			// guarantee at least the first 256 characters
			for (int i = 0; i < 256; ++i)
			{
				if (!charDict.ContainsKey(i))
					charDict[i] = nullChar;
			}

			fontData.chars = null;
			fontData.SetDictionary(charDict);
			fontData.useDictionary = true;
		}
		else
		{
			fontData.chars = new tk2dFontChar[maxCharId + 1];
			for (int i = 0; i <= maxCharId; ++i)
			{
				fontData.chars[i] = chars[i];
				if (fontData.chars[i] == null)
				{
					fontData.chars[i] = nullChar; // zero everything, null char
				}
			}
			
			fontData.charDict = null;
			fontData.useDictionary = false;
		}
		
		// kerning
		fontData.kerning = new tk2dFontKerning[fontInfo.kernings.Count];
		for (int i = 0; i < fontData.kerning.Length; ++i)
		{
			tk2dFontKerning kerning = new tk2dFontKerning();
			kerning.c0 = fontInfo.kernings[i].first;
			kerning.c1 = fontInfo.kernings[i].second;
			kerning.amount = fontInfo.kernings[i].amount * scale;
			fontData.kerning[i] = kerning;
		}
		
		return true;
	}

	int ReadIntAttribute(XmlNode node, string attribute)
	{
		return int.Parse(node.Attributes[attribute].Value, System.Globalization.NumberFormatInfo.InvariantInfo);
	}
	float ReadFloatAttribute(XmlNode node, string attribute)
	{
		return float.Parse(node.Attributes[attribute].Value, System.Globalization.NumberFormatInfo.InvariantInfo);
	}
	Vector2 ReadVector2Attributes(XmlNode node, string attributeX, string attributeY)
	{
		return new Vector2(ReadFloatAttribute(node, attributeX), ReadFloatAttribute(node, attributeY));
	}
	
	[MenuItem("Assets/Create/tk2d/Font", false, 11000)]
	static void DoBMFontCreate()
	{
		string path = tk2dEditorUtility.CreateNewPrefab("Font");
		if (path != null)
		{
			GameObject go = new GameObject();
			tk2dFont font = go.AddComponent<tk2dFont>();
			font.manageMaterial = true;
			go.active = false;

#if (UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_4)
			Object p = EditorUtility.CreateEmptyPrefab(path);
			EditorUtility.ReplacePrefab(go, p, ReplacePrefabOptions.ConnectToPrefab);
#else
			Object p = PrefabUtility.CreateEmptyPrefab(path);
			PrefabUtility.ReplacePrefab(go, p, ReplacePrefabOptions.ConnectToPrefab);
#endif
			GameObject.DestroyImmediate(go);
			
			tk2dEditorUtility.GetOrCreateIndex().AddFont(AssetDatabase.LoadAssetAtPath(path, typeof(tk2dFont)) as tk2dFont);
			tk2dEditorUtility.CommitIndex();
		}
	}

    [MenuItem("GameObject/Create Other/tk2d/TextMesh", false, 13905)]
    static void DoCreateBMTextMesh()
    {
		tk2dFontData fontData = null;
		Material material = null;
		
		// Find reference in scene
        tk2dTextMesh dupeMesh = GameObject.FindObjectOfType(typeof(tk2dTextMesh)) as tk2dTextMesh;
		if (dupeMesh) 
		{
			fontData = dupeMesh.font;
			material = dupeMesh.GetComponent<MeshRenderer>().sharedMaterial;
		}
		
		// Find in library
		if (fontData == null)
		{
			tk2dFont[] allFontData = tk2dEditorUtility.GetOrCreateIndex().GetFonts();
			foreach (var v in allFontData)
			{
				if (v.data != null)
				{
					fontData = v.data;
					material = fontData.material;
				}
			}
		}
		
		if (fontData == null)
		{
			EditorUtility.DisplayDialog("Create TextMesh", "Unable to create text mesh as no Fonts have been found.", "Ok");
			return;
		}

		GameObject go = tk2dEditorUtility.CreateGameObjectInScene("TextMesh");
        tk2dTextMesh textMesh = go.AddComponent<tk2dTextMesh>();
		textMesh.font = fontData;
		textMesh.text = "New TextMesh";
		textMesh.Commit();
		textMesh.GetComponent<MeshRenderer>().material = material;
    }
}
