using System;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Kogane.Internal
{
    internal sealed class AssemblyDefinitionSetting : ScriptableObject
    {
        [SerializeField] private string                    m_name = "NewAssembly";
        [SerializeField] private bool                      m_isEditor;
        [SerializeField] private bool                      m_allowUnsafeCode;
        [SerializeField] private bool                      m_autoReferenced = true;
        [SerializeField] private bool                      m_noEngineReferences;
        [SerializeField] private bool                      m_overrideReferences;
        [SerializeField] private string                    m_rootNamespace;
        [SerializeField] private string[]                  m_defineConstraints            = Array.Empty<string>();
        [SerializeField] private AssemblyDefinitionAsset[] m_assemblyDefinitionReferences = Array.Empty<AssemblyDefinitionAsset>();

        public bool CanCreate => !string.IsNullOrWhiteSpace( m_name );

        public bool IsEditor { set => m_isEditor = value; }

        public string GetPath( string directoryName )
        {
            return $"{directoryName}/{m_name}.asmdef";
        }

        public string ToJson()
        {
            var references = m_assemblyDefinitionReferences
                    .Select( x => AssetDatabase.GetAssetPath( x ) )
                    .Select( x => AssetDatabase.AssetPathToGUID( x ) )
                    .Select( x => $"GUID:{x}" )
                    .ToArray()
                ;

            var data = new JsonAssemblyDefinition
            {
                name               = m_name,
                includePlatforms   = m_isEditor ? new[] { "Editor" } : Array.Empty<string>(),
                autoReferenced     = m_autoReferenced,
                allowUnsafeCode    = m_allowUnsafeCode,
                noEngineReferences = m_noEngineReferences,
                overrideReferences = m_overrideReferences,
                rootNamespace      = m_rootNamespace,
                defineConstraints  = m_defineConstraints,
                references         = references,
            };

            return JsonUtility.ToJson( data, true );
        }
    }
}