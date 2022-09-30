using System.IO;
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
            m_settingEditor.OnInspectorGUIWithoutScript();

            using ( new EditorGUI.DisabledScope( !m_setting.CanCreate ) )
            {
                if ( GUILayout.Button( "Create" ) )
                {
                    OnCreate();
                }
            }

            m_scrollPosition = scope.scrollPosition;
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