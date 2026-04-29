using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Display_PlayerPrefs))]
public class Display_PlayerPrefs_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Display_PlayerPrefs playerPrefDisplay = (Display_PlayerPrefs)target;

        if (GUILayout.Button("Refresh")) //Refreshes the player prefs
        {
            playerPrefDisplay.RefreshPlayerPrefs();
        }

        //
        // Styling types
        //
        GUIStyle sectionTitle = new GUIStyle();
        sectionTitle.fontSize = 20;
        sectionTitle.fontStyle = FontStyle.Bold;

        GUIStyle subTitle = new GUIStyle();
        subTitle.fontSize = 16;
        subTitle.fontStyle = FontStyle.Bold;
        subTitle.margin.left = 2;

        GUIStyle text = new GUIStyle();
        text.margin.left = 4;


        //
        // Actual layout
        //
        EditorGUILayout.PrefixLabel("General", sectionTitle);
        EditorGUILayout.LabelField("Game exists: " + playerPrefDisplay.existingGame.ToString(), text);
        EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Arived home late: " + playerPrefDisplay.lateHomeArival.ToString(), text);
            EditorGUILayout.LabelField("Heavy drinking yesterday: " + playerPrefDisplay.heavyDrinking.ToString(), text);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.LabelField("");

        // Emotions and afflicts
        EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();
                EditorGUILayout.PrefixLabel("Emotions", subTitle);
                EditorGUILayout.LabelField("Happy: " + playerPrefDisplay.happy.ToString(), text);
                EditorGUILayout.LabelField("Sad: " + playerPrefDisplay.sad.ToString(), text);
                EditorGUILayout.LabelField("Angry: " + playerPrefDisplay.angry.ToString(), text);
                EditorGUILayout.LabelField("Pain: " + playerPrefDisplay.pain.ToString(), text);
                EditorGUILayout.LabelField("Tired: " + playerPrefDisplay.tired.ToString(), text);
                EditorGUILayout.LabelField("Stress: " + playerPrefDisplay.stress.ToString(), text);
                EditorGUILayout.LabelField("Bored: " + playerPrefDisplay.bored.ToString(), text);
                EditorGUILayout.LabelField("Feeling sick: " + playerPrefDisplay.feelingSick.ToString(), text);
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical();
                EditorGUILayout.PrefixLabel("Afflicts", subTitle);
                EditorGUILayout.LabelField("Throat swelling: " + playerPrefDisplay.tinglingThroat.ToString(), text);
                EditorGUILayout.LabelField("Runny nose: " + playerPrefDisplay.runnyNose.ToString(), text);
                EditorGUILayout.LabelField("Tight chest: " + playerPrefDisplay.tightChest.ToString(), text);
                EditorGUILayout.LabelField("Hard to breath: " + playerPrefDisplay.hardToBreath.ToString(), text);
                EditorGUILayout.LabelField("Itchy: " + playerPrefDisplay.itchy.ToString(), text);
                EditorGUILayout.LabelField("sick: " + playerPrefDisplay.sick.ToString(), text);
            EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
    }
}
