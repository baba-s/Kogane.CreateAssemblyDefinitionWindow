﻿using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Kogane.Internal
{
    internal sealed class CreateAssemblyDefinitionWindow : EditorWindow
    {
        private string                    m_directoryName = string.Empty;
        private AssemblyDefinitionSetting m_setting;
        private Editor                    m_settingEditor;
        private Vector2                   m_scrollPosition;

        [MenuItem( "Assets/Kogane/Create Assembly Definition", priority = 1156162169 )]
        private static void Open()
        {
            var asset     = Selection.activeObject;
            var assetPath = AssetDatabase.GetAssetPath( asset );

            var directoryName = string.IsNullOrWhiteSpace( assetPath )
                    ? "Assets"
                    : assetPath
                ;

            if ( !AssetDatabase.IsValidFolder( directoryName ) )
            {
                directoryName = Path
                        .GetDirectoryName( directoryName )
                        ?.Replace( "\\", "/" )
                    ;
            }

            var window = GetWindow<CreateAssemblyDefinitionWindow>
            (
                utility: true,
                title: "Create Assembly Definition",
                focus: true
            );

            window.m_directoryName    = directoryName;
            window.m_setting          = CreateInstance<AssemblyDefinitionSetting>();
            window.m_setting.IsEditor = directoryName.Split( '/' ).Contains( "Editor" );

            Editor.CreateCachedEditor( window.m_setting, null, ref window.m_settingEditor );
        }

        private void OnGUI()
        {
            using var scope = new EditorGUILayout.ScrollViewScope( m_scrollPosition );

            EditorGUILayout.LabelField( "Directory Name", m_directoryName );
            DoDrawDefaultInspector( m_settingEditor.serializedObject );

            using ( new EditorGUI.DisabledScope( !m_setting.CanCreate ) )
            {
                if ( GUILayout.Button( "Create" ) )
                {
                    OnCreate();
                }
            }

            m_scrollPosition = scope.scrollPosition;
        }

        private static bool DoDrawDefaultInspector( SerializedObject serializedObject )
        {
            using var scope = new EditorGUI.ChangeCheckScope();

            serializedObject.UpdateIfRequiredOrScript();

            var iterator = serializedObject.GetIterator();

            for ( var enterChildren = true; iterator.NextVisible( enterChildren ); enterChildren = false )
            {
                var propertyPath = iterator.propertyPath;

                if ( propertyPath == "m_Script" ) continue;
                if ( propertyPath == "m_allowUnsafeCode" )
                {
                    EditorGUILayout.PropertyField( iterator, new GUIContent( "Allow 'unsafe' Code" ), true );
                    continue;
                }

                EditorGUILayout.PropertyField( iterator, true );
            }

            serializedObject.ApplyModifiedProperties();

            return scope.changed;
        }

        private void OnCreate()
        {
            File.WriteAllText
            (
                path: m_setting.GetPath( m_directoryName ),
                contents: m_setting.ToJson(),
                encoding: Encoding.UTF8
            );

            AssetDatabase.Refresh();

            Close();
        }
    }
}