using System.IO;
using UnityEditor;
using UnityEngine;
using Aspose.Cells;
using System.Reflection;

public enum TableValueType {
    BYTE,
    INT,
    LONG,
    FLOAT,
    BOOL,
    STRING,
    ENUM,
    CUSTOM,
};


public class EditorHelper
{
    public static string profileFolder = "Assets/_PrcessAssets/Profile";
    public static Assembly asm;
    ///////////////////////////////////////////////////////////////////////////////
    // 
    ///////////////////////////////////////////////////////////////////////////////

    public static T CreateNewEditorProfile<T>(string _profileName, string _path = "", string _class = "") where T : ScriptableObject {

        // 
        string path = Path.Combine((_path.Length == 0) ? profileFolder : _path, _profileName);

        bool doCreate = true;
        FileInfo fileInfo = new FileInfo(path);
        if (fileInfo.Exists) {
            doCreate = EditorUtility.DisplayDialog(_profileName + " already exists.",
                                                   "Do you want to overwrite the old one?",
                                                   "Yes", "No");
        }
        if (doCreate) {

            // check if the asset is valid to create
            if (new DirectoryInfo(profileFolder).Exists == false) {
                Debug.LogError("can't create asset, path not found");
                return null;
            }
            if (string.IsNullOrEmpty(_profileName)) {
                Debug.LogError("can't create asset, the name is empty");
                return null;
            }

            if (_class.Length != 0) {
                ScriptableObject obj = ScriptableObject.CreateInstance(_class);
                if (obj is T) {
                    AssetDatabase.CreateAsset(obj, path);
                    Selection.activeObject = obj;
                    return obj as T;
                }
                return null;
            }
            else {
                T newAsset = ScriptableObject.CreateInstance<T>();
                AssetDatabase.CreateAsset(newAsset, path);
                Selection.activeObject = newAsset;
                return newAsset;
            }
        }
        return null;
    }

    public static Worksheet LoadExcelSheet(string _excelFilePath, string _sheetName)
    {
        Workbook book = new Workbook(_excelFilePath);
        foreach (Worksheet sheet in book.Worksheets)
        {
            if (sheet.Name == _sheetName)
            {
                return sheet;
            }
        }
        Debug.LogError("can't find the sheet you want " + _sheetName);
        return null;
    }

    ///////////////////////////////////////////////////////////////////////////////
    // 
    ///////////////////////////////////////////////////////////////////////////////

    public static object GetTableValue(string _valueType, string _valueString) {

        //Debug.Log(_valueType + " " + _valueString);

        if (string.Compare(TableValueType.BYTE.ToString(), _valueType, true) == 0)
        {
            byte value;
            _valueString.Trim();

            if (byte.TryParse(_valueString, out value))
            {
                return value;
            }
            return null;
        }
        else if (string.Compare(TableValueType.INT.ToString(), _valueType, true) == 0)
        {

            int value = 0;
            _valueString.Trim();

            if (int.TryParse(_valueString, out value))
            {
                return value;
            }
            return null;
        }
        else if (string.Compare(TableValueType.LONG.ToString(), _valueType, true) == 0) {

            long value = 0;
            _valueString.Trim();

            if (long.TryParse(_valueString, out value)) {
                return value;
            }
            return null;
        }
        else if (string.Compare(TableValueType.FLOAT.ToString(), _valueType, true) == 0) {

            float value = 0;
            _valueString.Trim();

            if (float.TryParse(_valueString, out value)) {
                return value;
            }
            return null;
        }
        else if (string.Compare(TableValueType.BOOL.ToString(), _valueType, true) == 0) {

            bool value = false;
            _valueString.Trim();

            if (bool.TryParse(_valueString, out value)) {
                return value;
            }
            return null;
        }
        else if (string.Compare(TableValueType.STRING.ToString(), _valueType, true) == 0) {
            return _valueString;
        }
        else if (string.Compare(TableValueType.CUSTOM.ToString(), _valueType, true) == 0) {
            return _valueString;
        }
        else 
        {
            if (asm == null)
            {
                asm = Assembly.Load("Assembly-CSharp");
            }
            System.Type type = asm.GetType(_valueType, false, true);
            if (type == null)
            {
                Debug.LogError("error type " + _valueType);
            }
            if (type.IsEnum)
            {
                _valueString.Trim();
                return System.Enum.Parse(type, _valueString);
            }
        }
        return null;
    }
}
