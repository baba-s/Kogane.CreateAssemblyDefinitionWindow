using UnityEditor;

namespace Kogane
{
    public static class SerializedObjectExtensionMethods
    {
        public static void OnInspectorGUIWithoutScript( this Editor self )
        {
            self.serializedObject.DoDrawDefaultInspectorWithoutScript();
        }

        public static bool DoDrawDefaultInspectorWithoutScript( this SerializedObject self )
        {
            using var scope = new EditorGUI.ChangeCheckScope();

            self.UpdateIfRequiredOrScript();

            var iterator = self.GetIterator();

            for ( var enterChildren = true; iterator.NextVisible( enterChildren ); enterChildren = false )
            {
                var propertyPath = iterator.propertyPath;

                if ( propertyPath == "m_Script" ) continue;
                EditorGUILayout.PropertyField( iterator, true );
            }

            self.ApplyModifiedProperties();

            return scope.changed;
        }
    }
}