using System;
using System.Collections.Generic;
using System.Reflection;
using Aspose.Cells;
using UnityEditor;
using UnityEngine;
using System.CodeDom.Compiler;


public class TableProcess : EditorWindow {

    // create asset
    static string assetName = "TableProcess.asset";
    static string assetOutputPath = @"Assets\Resources\TableAssets";
	
    [MenuItem("Helper/Table Process")]
    public static TableProcess NewWindow() {
        TableProcess newWindow = EditorWindow.GetWindow<TableProcess>();		
        return newWindow;
    }

    [MenuItem("Assets/Create/Custom Assets/Table Process")]
    static void CreateAnimationTableAsset() {
        EditorHelper.CreateNewEditorProfile<TableProcessProfile>(assetName);
    }

    
    // 
    TableProcessProfile asset;		
    List<SerializedProperty> tables;
    SerializedObject serialized;
    Assembly asm;

    void OnEnable() {
        name = "Table Process";
        autoRepaintOnSceneChange = false;

        asset = AssetDatabase.LoadAssetAtPath(EditorHelper.profileFolder + "/" + assetName,
                                              typeof(TableProcessProfile)) as TableProcessProfile;
        if (asset == null) {
            Debug.LogError(EditorHelper.profileFolder + "/" + assetName + " no found, please create it first!" );
        }

        tables = new List<SerializedProperty>();
        serialized = new SerializedObject(asset);
        SerializedProperty tableInfos = serialized.FindProperty("tableInfos");
        for (int i = 0; i < tableInfos.arraySize; i++) {
            SerializedProperty obj = tableInfos.GetArrayElementAtIndex(i);
            if (obj != null) {
                tables.Add(obj);
            }
        }

        if (asm == null) {
            asm = Assembly.Load("Assembly-CSharp");
        }
    }


    int currentIndex = -1;
    Vector2 scrololPos = Vector2.zero;
    void OnGUI() {

        GUIStyle style = null;

        // if we don't have a profile
        if (asset == null) {
            GUILayout.Space(10);

            style = new GUIStyle();
            style.fontStyle = FontStyle.Bold;
            style.normal.textColor = Color.red;
            GUILayout.Label("Please create a Table Asset!", style);
            return;
        }

        GUILayout.Space(10);

        // ======================================================== 
        // excel table 
        // ========================================================      
        style = new GUIStyle();
        style.fontStyle = FontStyle.Bold;

        scrololPos = GUILayout.BeginScrollView(scrololPos);
        for (int i = 0; i < tables.Count; i++) {

			style.normal.textColor = (i == currentIndex) ? Color.green : EditorStyles.label.normal.textColor;

            EditorGUILayout.BeginHorizontal();
			    EditorGUILayout.LabelField(asset.tableInfos[i].table_class_name, style, GUILayout.Width(250));
			    if (GUILayout.Button("Exec", GUILayout.Width(50)) && asset.tableInfos[i].input_excel != null) {
				    currentIndex = i;
                    ExportTable(asset.tableInfos[i]);
			    }
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.PropertyField(tables[i].FindPropertyRelative("input_excel"));
			EditorGUILayout.LabelField("", style, GUILayout.Width(200));
        }
        GUILayout.EndScrollView();
        serialized.ApplyModifiedProperties();
    }


    const int characterTableTitleRow = 1;
    const int characterTableValueTypeRow = 2;
    string[] header = null;
    string[] type = null;
    Dictionary<string, Dictionary<string, string>> excelData;

    void ExportTable(TableProcessInfo _tableInfo) {

        excelData = new Dictionary<string, Dictionary<string, string>>();
        Worksheet sheet
            = EditorHelper.LoadExcelSheet(AssetDatabase.GetAssetPath(_tableInfo.input_excel), _tableInfo.sheet_name);

        int maxColumns = sheet.Cells.MaxDataColumn + 1;
        Debug.Log("sheet Cells Columns Count " + maxColumns);
        // parse title 
        header = new string[maxColumns];
        type = new string[maxColumns];
        for (int i = 0; i < maxColumns; ++i)
        {
            header[i] = sheet.Cells.Rows[characterTableTitleRow].GetCellOrNull(i)
                        != null ? sheet.Cells.Rows[1].GetCellOrNull(i).StringValue : null;
            if (header[i] != null) {
                header[i].Trim();
            }

            if (string.IsNullOrEmpty(header[i]))
            {
                Debug.LogError("null column name :" + i);
            }

            type[i] = sheet.Cells.Rows[characterTableValueTypeRow].GetCellOrNull(i)
                        != null ? sheet.Cells.Rows[2].GetCellOrNull(i).StringValue : null;
            if (type[i] != null) {
                type[i].Trim();
            }
            if (string.IsNullOrEmpty(type[i]))
            {
                Debug.LogError("null column type :" + i);
            }
            if (type[i] == "varchar")
            {
                type[i] = "string";
            }

            if (type[i] == "bit")
            {
                type[i] = "bool";
            }

            if (type[i] == "char")
            {
                type[i] = "byte";
            }

            Debug.Log("column : " + header[i] + ", type : " + type[i]);
        }

        // parse table content
        foreach (Row row in sheet.Cells.Rows)
        {
            if (row.Index <= characterTableValueTypeRow)
                continue;

            string key = row.GetCellOrNull(0) != null ? row.GetCellOrNull(0).StringValue : null;
            if (string.IsNullOrEmpty(key) == false)
            {
                if (excelData.ContainsKey(key) == false)
                {
                    excelData.Add(key, new Dictionary<string, string>());
                }

                for (int i = 0; i < maxColumns; ++i)
                {
                    string value = row.GetCellOrNull(i) != null ? row.GetCellOrNull(i).StringValue : null;

                    if (string.IsNullOrEmpty(value) == false)
                    {
                        if (excelData[key].ContainsKey(header[i]))
                        {
                            Debug.LogError("Try to add duplicate key " + header[i] + " for animation " + key);
                            continue;
                        }
                        excelData[key].Add(header[i], value);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(header[i]) == false)
                        {
                            excelData[key].Add(header[i], "");
                        }
                    }
                }
            }
        }
        /*
        foreach (string s in excelData.Keys)
        {
            string str = "";
            Dictionary<string, string> dict = excelData[s];
            foreach (string k in dict.Keys)
            {
                str += ((dict[k]) + " ");
            }
            Debug.Log(str);
        }
        */
        // parse excelData
        TableParseMethod(_tableInfo);
    }

	void TableParseMethod(TableProcessInfo _tableInfo)
	{
        ScriptableObject table
            = EditorHelper.CreateNewEditorProfile<ScriptableObject>(_tableInfo.ouput_asset_name,
                                                                    assetOutputPath,
                                                                    _tableInfo.table_class_name);
        if (table == null) {
            Debug.LogError("create table " + _tableInfo.table_class_name + " failed! output path : " + assetOutputPath + ", table class name : " + _tableInfo.table_class_name);
            return;
        }

        foreach (Dictionary<string, string> row in excelData.Values) {
            int index = 0;

            Type t = asm.GetType(_tableInfo.data_class_name);
            System.Object c = Activator.CreateInstance(t);
            foreach (string title in row.Keys) {
                FieldInfo mInfo = t.GetField(header[index], BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (mInfo != null) {
                    object param = EditorHelper.GetTableValue(type[index], row[title]);
                    if (param != null) {
//                         try
//                         {
                            t.InvokeMember(mInfo.Name, BindingFlags.SetField | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, c, new object[] { param });
//                         }
//                         catch(Exception e)
//                         {
//                             Debug.LogError("cannot set field " + header[index]);
//                         }
                    }
                }

                PropertyInfo pInfo = t.GetProperty(header[index], BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (pInfo != null)
                {
                    object param = EditorHelper.GetTableValue(type[index], row[title]);
                    if (param != null)
                    {
                        t.InvokeMember(pInfo.Name, BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.Public, null, c, new object[] { param });
                    }
                }
                if (mInfo == null && pInfo == null)
                {
                    Debug.LogError("cannot find field " + header[index]);
                }
                index++;
            }

            IDataCollection lst = table as IDataCollection;
            if (lst != null) {
                lst.Add(c);
            }
            else {
                Debug.LogError(table.GetType() + " " + "isn't implemente IDataList interface!");
            }
        }     	           
        EditorUtility.SetDirty(table);
        AssetDatabase.SaveAssets();
	}
}
