using UnityEngine;

namespace UnityEditor
{
    internal class CelShaderGUI : ShaderGUI
    {
        private static class Styles
        {
            public static GUIContent uvSetLabel = EditorGUIUtility.TrTextContent("UV Set");

            public static GUIContent albedoText = EditorGUIUtility.TrTextContent("Albedo", "Albedo (RGB)");
            public static GUIContent lutText = EditorGUIUtility.TrTextContent("LUT", "Lookup Texture");
            public static GUIContent normalMapText = EditorGUIUtility.TrTextContent("Normal Map", "Normal Map");

            public static string primaryMapsText = "Main Maps";
        }

        MaterialProperty albedoMap = null;
        MaterialProperty albedoColor = null;
        MaterialProperty lutMap = null;
        MaterialProperty outlineColor = null;
        MaterialProperty outlineThickness = null;
        MaterialProperty specGloss = null;
        MaterialProperty smoothStepParam1 = null;
        MaterialProperty smoothStepParam2= null;
        MaterialProperty specMultiplier = null;
        MaterialProperty specPower = null;
        MaterialProperty bumpMap = null;

        MaterialEditor m_MaterialEditor;

        public void FindProperties(MaterialProperty[] props)
        {
            albedoMap = FindProperty("_MainTex", props);
            albedoColor = FindProperty("_Color", props);
            
            lutMap = FindProperty("_LUTMap", props, false);
            
            outlineColor = FindProperty("_OutlineColor", props, false);
            outlineThickness = FindProperty("_OutlineThickness", props, false);

            specGloss = FindProperty("_SpecGloss", props);
            
            smoothStepParam1 = FindProperty("_SmoothStepParam1", props);
            smoothStepParam2 = FindProperty("_SmoothStepParam2", props);
            specMultiplier = FindProperty("_SpecMultiplier", props);
            specPower = FindProperty("_SpecPower", props);
            
            bumpMap = FindProperty("_BumpMap", props);
        }

        public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] props)
        {
            FindProperties(props); // MaterialProperties can be animated so we do not cache them but fetch them every event to ensure animated values are updated correctly
            m_MaterialEditor = materialEditor;
            Material material = materialEditor.target as Material;

            ShaderPropertiesGUI(material);
            base.OnGUI(materialEditor, props);
        }

        public void ShaderPropertiesGUI(Material material)
        {
            // Use default labelWidth
            EditorGUIUtility.labelWidth = 0f;

            // Detect any changes to the material
            EditorGUI.BeginChangeCheck();
            {
                // Primary properties
                GUILayout.Label(Styles.primaryMapsText, EditorStyles.boldLabel);
                
                m_MaterialEditor.TexturePropertySingleLine(Styles.albedoText, albedoMap, albedoColor);
                m_MaterialEditor.TexturePropertySingleLine(Styles.lutText, lutMap);
                m_MaterialEditor.TexturePropertySingleLine(Styles.normalMapText, bumpMap);
                

                EditorGUILayout.Space();
            }
            
            EditorGUILayout.Space();
        }
    }
}
