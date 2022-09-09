using System;

// ReSharper disable InconsistentNaming
// ReSharper disable NotAccessedField.Global

namespace Kogane.Internal
{
    [Serializable]
    internal sealed class AssemblyDefinitionJson
    {
        public string   name             = string.Empty;
        public string   rootNamespace    = string.Empty;
        public string[] references       = Array.Empty<string>();
        public string[] includePlatforms = Array.Empty<string>();
        public string[] excludePlatforms = Array.Empty<string>();
        public bool     allowUnsafeCode;
        public bool     overrideReferences;
        public string[] precompiledReferences = Array.Empty<string>();
        public bool     autoReferenced;
        public string[] defineConstraints = Array.Empty<string>();
        public string[] versionDefines    = Array.Empty<string>();
        public bool     noEngineReferences;
    }
}