using System;

namespace Kogane.Internal
{
    [Serializable]
    internal sealed class AssemblyDefinitionJson
    {
        public string   name             = string.Empty;
        public string[] references       = new string[ 0 ];
        public string[] includePlatforms = new string[ 0 ];
        public string[] excludePlatforms = new string[ 0 ];
        public bool     allowUnsafeCode;
        public bool     overrideReferences;
        public string[] precompiledReferences = new string[ 0 ];
        public bool     autoReferenced;
        public string[] defineConstraints = new string[ 0 ];
        public string[] versionDefines    = new string[ 0 ];
        public bool     noEngineReferences;
    }
}