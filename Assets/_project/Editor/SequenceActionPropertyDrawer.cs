// using _project.ScriptableObjects.Scripts;
// using _project.Scripts;
// using UnityEditor;
// using UnityEngine;
//
// namespace _project.Editor
// {
//     [CustomPropertyDrawer(typeof(SequenceData.SequenceAction))]
//     public class SequenceActionEditor : PropertyDrawer
//     {
//         private bool _foldoutState;
//
//         public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
//         {
//             if (!_foldoutState) return EditorGUIUtility.singleLineHeight;
//             var actionTypeProperty = property.FindPropertyRelative(nameof(SequenceData.SequenceAction.ActionType));
//
//             switch ((SequenceConfig.ActionType)actionTypeProperty.enumValueIndex)
//             {
//                 case SequenceConfig.ActionType.Pickup:
//                 case SequenceConfig.ActionType.Drop:
//                     return EditorGUIUtility.singleLineHeight * 3;
//                     break;
//                 case SequenceConfig.ActionType.Scan:
//                     return EditorGUIUtility.singleLineHeight * 4;
//                     break;
//                 default:
//                     return EditorGUIUtility.singleLineHeight * 2;
//                     break;
//             }
//         }
//         public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
//         {
//             EditorGUI.BeginProperty(rect, label, property);
//
//             _foldoutState = EditorGUI.BeginFoldoutHeaderGroup(rect, _foldoutState, label);
//             if (!_foldoutState)
//             {
//                 EditorGUI.EndFoldoutHeaderGroup();
//                 return;
//             }
//             var beatDelayProperty = property.FindPropertyRelative(nameof(SequenceData.SequenceAction.BeatDelayToPrevious));
//             var actionTypeProperty = property.FindPropertyRelative(nameof(SequenceData.SequenceAction.ActionType));
//             var clotheTypeProperty = property.FindPropertyRelative(nameof(SequenceData.SequenceAction.ClotheType));
//             var clotheColorProperty = property.FindPropertyRelative(nameof(SequenceData.SequenceAction.ClotheColor));
//             
//             beatDelayProperty.intValue = EditorGUILayout.IntField(ObjectNames.NicifyVariableName(nameof(SequenceData.SequenceAction.BeatDelayToPrevious)), beatDelayProperty.intValue);
//             
//             actionTypeProperty.enumValueIndex = (int)(SequenceConfig.ActionType)EditorGUILayout.EnumPopup(ObjectNames.NicifyVariableName(nameof(SequenceData.SequenceAction.ActionType)), (SequenceConfig.ActionType)actionTypeProperty.enumValueIndex);
//
//             
//             switch ((SequenceConfig.ActionType)actionTypeProperty.enumValueIndex)
//             {
//                 case SequenceConfig.ActionType.Pickup:
//                 case SequenceConfig.ActionType.Drop:
//                     clotheTypeProperty.enumValueIndex = (int)(SequenceConfig.ClotheType)EditorGUILayout.EnumPopup(ObjectNames.NicifyVariableName(nameof(SequenceData.SequenceAction.ClotheType)), (SequenceConfig.ClotheType)clotheTypeProperty.enumValueIndex);
//                     break;
//                 case SequenceConfig.ActionType.Scan:
//                     clotheTypeProperty.enumValueIndex = (int)(SequenceConfig.ClotheType)EditorGUILayout.EnumPopup(ObjectNames.NicifyVariableName(nameof(SequenceData.SequenceAction.ClotheType)), (SequenceConfig.ClotheType)clotheTypeProperty.enumValueIndex);
//                     clotheColorProperty.enumValueIndex = (int)(SequenceConfig.ClotheColor)EditorGUILayout.EnumPopup(ObjectNames.NicifyVariableName(nameof(SequenceData.SequenceAction.ClotheColor)), (SequenceConfig.ClotheColor)clotheColorProperty.enumValueIndex);
//                     break;
//                 default:
//                     break;
//             }
//
//             EditorGUI.EndFoldoutHeaderGroup();
//             
//             EditorGUI.EndProperty();
//         }
//     }
// }