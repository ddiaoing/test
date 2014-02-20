using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;


public static class Utils
{
    static Utils()
    {

        material = new Material(shader);
        material.hideFlags = HideFlags.HideAndDontSave;
        material.shader.hideFlags = HideFlags.HideAndDontSave;

    }


    static Material material;

    static string shader = @"
Shader ""Lines/Colored Blended""
{ 
    SubShader 
    {                   
        Pass
        { 
            Blend SrcAlpha OneMinusSrcAlpha 
            
            BindChannels
            { 
                Bind ""Color"",color 
            }

            ZWrite On 
            Cull Front 
            Fog 
            {
                Mode Off 
            }
        }
    } 
}
";

    public static void DrawRectangle(Rect position, Color color)
    {
        GL.PushMatrix();
        GL.LoadOrtho();

        material.SetPass(0);
        // Optimization hint: 
        // Consider Graphics.DrawMeshNow
        GL.Color(color);
        GL.Begin(GL.QUADS);
        GL.Vertex3(position.x, position.y, 0);
        GL.Vertex3(position.x + position.width, position.y, 0);
        GL.Vertex3(position.x + position.width, position.y + position.height, 0);
        GL.Vertex3(position.x, position.y + position.height, 0);
        GL.End();
        GL.PopMatrix();
    }

    #region XML Serializer

    /// <summary>
    /// 
    ///    public class TestObject   
    ///    {
    ///        public string name = "test";
    ///        public int id = 100;
    ///    }
    ///    Utils.Save<TestObject>("Assets/Resources/test.xml", new TestObject());
    ///    TestObject obj = Utils.Load<TestObject>("test");
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="strFileName"></param>
    /// <returns></returns>
    public static T Load<T>(String strFileName)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(T));
        return Load<T>(strFileName, serializer);
    }

    internal static T Load<T>(String strFileName, XmlAttributeOverrides overrides)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(T), overrides);
        return Load<T>(strFileName, serializer);
    }

    private static T Load<T>(String strFileName, XmlSerializer serializer)
    {
        T ret = default(T);
        try
        {
          //  Stream stream = File.OpenRead(strFileName);
            string path = strFileName;
            TextAsset textData = Resources.Load(path, typeof(TextAsset)) as TextAsset;

            MemoryStream stream = new MemoryStream(textData.bytes);

            using (stream)
            {
                ret = (T)serializer.Deserialize(stream);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            return default(T);
        }

        return ret;
    }

    public static bool Save<T>(String strFileName, T intance)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(T));
        return Save<T>(strFileName, intance, serializer);
    }

    public static bool Save<T>(String strFileName, T intance, XmlAttributeOverrides overrides)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(T), overrides);
        return Save<T>(strFileName, intance, serializer);
    }

    public static bool RemoveReadOnly(String strFileName)
    {
        FileInfo fileInfo = new FileInfo(strFileName);
        if (!fileInfo.Exists)
        {
            return false;
        }

        fileInfo.Attributes &= ~FileAttributes.ReadOnly;
        return true;
    }

    static bool Save<T>(String strFileName, T intance, XmlSerializer serializer)
    {
        RemoveReadOnly(strFileName);
        Stream stream = File.Open(strFileName, FileMode.Create, FileAccess.Write);
        try
        {
            using (stream)
            {
                serializer.Serialize(stream, intance);
            }
        }
        catch (Exception)
        {
            return false;
        }

        return true;
    }

    #endregion
}

