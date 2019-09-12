using System.Collections.Generic;
using System.IO;
using BML_ExperimentToolkit.Scripts.UI.Editor;
using UnityEditor;
using UnityEngine;

namespace BML_ExperimentToolkit.Scripts.VariableSystem.VariableUI {
    [CustomEditor(typeof(VariableConfigurationFile))]
    public class VariableConfigurationFileEditor : Editor {


        bool               showAdvanced;
        SerializedProperty factory;
        SerializedProperty trialTableGenerationMode;
        SerializedProperty orderConfigs;
        SerializedProperty randomizationMode;
        SerializedProperty repeatTrialsInBlock;
        SerializedProperty repeatAllBlocks;
        SerializedProperty columnNameSettings;
        SerializedProperty controlSettings;
        SerializedProperty guiSettings;

        void OnEnable() {
            factory = serializedObject.FindProperty(nameof(VariableConfigurationFile.Factory));
            trialTableGenerationMode = serializedObject.FindProperty(nameof(VariableConfigurationFile.GenerateExperimentTable));
            orderConfigs = serializedObject.FindProperty(nameof(VariableConfigurationFile.BlockOrderConfigurations));
            
            randomizationMode = serializedObject.FindProperty(nameof(VariableConfigurationFile.RandomizationMode));
            repeatTrialsInBlock = serializedObject.FindProperty(nameof(VariableConfigurationFile.RepeatTrialsInBlock));
            repeatAllBlocks = serializedObject.FindProperty(nameof(VariableConfigurationFile.RepeatAllBlocks));
            columnNameSettings = serializedObject.FindProperty(nameof(VariableConfigurationFile.ColumnNamesSettings));
            controlSettings = serializedObject.FindProperty(nameof(VariableConfigurationFile.ControlSettings));
            guiSettings = serializedObject.FindProperty(nameof(VariableConfigurationFile.GuiSettings));
            
            
            
        }
        

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            EditorGUILayout.LabelField("Randomization and Repetition settings:");
            EditorGUILayout.PropertyField(randomizationMode);
            EditorGUILayout.PropertyField(repeatTrialsInBlock);
            EditorGUILayout.PropertyField(repeatAllBlocks);
            EditorGUILayout.PropertyField(factory);
            
            EditorGUILayout.LabelField("--------");
            
            if (GUILayout.Button("Preview Design", GUILayout.Width(250), GUILayout.Height(50))){
                DesignPreviewWindow.ShowWindow(target as VariableConfigurationFile);
                
            }
            
            EditorGUILayout.LabelField("--------");
            EditorGUILayout.LabelField("Advanced (see wiki for information):", EditorStyles.boldLabel);

            EditorGUILayout.BeginVertical();
            EditorGUI.indentLevel++;
            if (showAdvanced) {
                ShowAdvancedOptions();
            }
            else {
                if (GUILayout.Button("Show", GUILayout.Width(250))) {
                    showAdvanced = true;
                }
            }

            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(target);
        }

        void ShowAdvancedOptions() {
            
            if (GUILayout.Button("Hide Advanced Options", GUILayout.Width(250))) {
                showAdvanced = false;
            }

            EditorGUILayout.LabelField("Pre-generated experiment table options:");
            EditorGUI.indentLevel += 2;

            EditorGUILayout.PropertyField(trialTableGenerationMode);


            if (GUILayout.Button("Generate Design File Manually", GUILayout.Width(250))) {
                DesignSaverWindow.ShowWindow(Selection.activeObject as VariableConfigurationFile);
            }

            EditorGUI.indentLevel -= 2;

            EditorGUILayout.LabelField("Manual block order configuration");
            
            EditorGUI.indentLevel += 2;
            EditorGUILayout.LabelField("Note: This system works is but due for an overhaul, see wiki", EditorStyles.miniLabel);

            for (int i = 0; i < orderConfigs.arraySize; i++) {
                SerializedProperty order = orderConfigs.GetArrayElementAtIndex(i);
                EditorGUILayout.PropertyField(order);
            }


            if (GUILayout.Button("Add New BlockOrderDefinition")) {
                VariableConfigurationFile variableConfigurationFile = target as VariableConfigurationFile;
                if (variableConfigurationFile != null) {
                    List<BlockOrderDefinition> orders = variableConfigurationFile.BlockOrderConfigurations;
                    BlockOrderDefinition newBlockOrderDefinition = CreateInstance<BlockOrderDefinition>();
                    newBlockOrderDefinition.InitFromDesign(variableConfigurationFile);
                    orders.Add(newBlockOrderDefinition);
                    string savePath = Path.GetDirectoryName(path: AssetDatabase.GetAssetPath(Selection.activeObject)) + "/New Block Order Definition.asset";
                    AssetDatabase.CreateAsset(newBlockOrderDefinition, savePath);
                    AssetDatabase.SaveAssets();

                    EditorUtility.FocusProjectWindow();

                    Selection.activeObject = newBlockOrderDefinition;
                }
            }
            
            
            EditorGUI.indentLevel -= 2;
            
            EditorGUILayout.LabelField("Settings:");
            EditorGUI.indentLevel += 2;
            EditorGUILayout.PropertyField(columnNameSettings);
            EditorGUILayout.PropertyField(controlSettings);
            EditorGUILayout.PropertyField(guiSettings);
            EditorGUI.indentLevel -= 2;
        }
    }
}