using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Kogane.Internal
{
	internal sealed class AssemblyDefinitionCreator : EditorWindow
	{
		private const float WINDOW_HEIGHT = 164;
		private const float LABEL_WIDTH   = 144;

		private string m_directory          = string.Empty;
		private string m_name               = "NewAssembly";
		private bool   m_isEditor           = false;
		private bool   m_allowUnsafeCode    = false;
		private bool   m_overrideReferences = false;
		private bool   m_noEngineReferences = false;
		private bool   m_autoReferenced     = true;

		[MenuItem( "Assets/Open Assembly Definition Creator" )]
		private static void Open()
		{
			var asset     = Selection.activeObject;
			var assetPath = AssetDatabase.GetAssetPath( asset );
			var directory = string.IsNullOrWhiteSpace( assetPath ) ? "Assets" : assetPath;

			if ( !AssetDatabase.IsValidFolder( directory ) )
			{
				directory = Path
						.GetDirectoryName( directory )
						?.Replace( "\\", "/" )
					;
			}

			var window = GetWindow<AssemblyDefinitionCreator>
			(
				utility: true,
				title: "Assembly Definition Creator",
				focus: true
			);

			window.m_directory = directory;
			window.m_isEditor  = directory.Split( '/' ).Any( x => x == "Editor" );

			var minSize = window.minSize;
			var maxSize = window.maxSize;

			minSize.y = WINDOW_HEIGHT;
			maxSize.y = WINDOW_HEIGHT;

			window.minSize = minSize;
			window.maxSize = maxSize;
		}

		private void OnGUI()
		{
			var oldLabelWidth = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = LABEL_WIDTH;

			EditorGUILayout.LabelField( "Path", m_directory );

			m_name               = EditorGUILayout.TextField( "Name", m_name );
			m_allowUnsafeCode    = EditorGUILayout.Toggle( "Allow 'unsafe' Code", m_allowUnsafeCode );
			m_autoReferenced     = EditorGUILayout.Toggle( "Auto Referenced", m_autoReferenced );
			m_overrideReferences = EditorGUILayout.Toggle( "Override References", m_overrideReferences );
			m_noEngineReferences = EditorGUILayout.Toggle( "No Engine References", m_noEngineReferences );
			m_isEditor           = EditorGUILayout.Toggle( "Is Editor", m_isEditor );

			EditorGUIUtility.labelWidth = oldLabelWidth;

			using ( new EditorGUI.DisabledScope( string.IsNullOrWhiteSpace( m_name ) ) )
			{
				if ( GUILayout.Button( "Create" ) )
				{
					OnCreate();
				}
			}
		}

		private void OnCreate()
		{
			var path       = $"{m_directory}/{m_name}.asmdef";
			var uniquePath = AssetDatabase.GenerateUniqueAssetPath( path );

			var data = new AssemblyDefinitionJson
			{
				name             = Path.GetFileNameWithoutExtension( uniquePath ),
				includePlatforms = m_isEditor ? new[] { "Editor" } : new string[0],
				autoReferenced   = m_autoReferenced,
			};

			var json = JsonUtility.ToJson( data, true );

			File.WriteAllText( uniquePath, json, Encoding.UTF8 );

			AssetDatabase.Refresh();

			Close();
		}
	}
}