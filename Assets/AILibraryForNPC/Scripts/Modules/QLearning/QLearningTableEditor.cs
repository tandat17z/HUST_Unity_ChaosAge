using System.IO;
using UnityEditor;
using UnityEngine;

namespace AILibraryForNPC.Modules.QLearning
{
    [CustomEditor(typeof(QLearningTableSO))]
    public class QLearningTableEditor : Editor
    {
        private Vector2 scrollPosition;
        private string searchFilter = "";
        private bool showOnlyNonZero = false;
        private string newKey = "";
        private float newValue = 0f;
        private string jsonFilePath = "";

        public override void OnInspectorGUI()
        {
            QLearningTableSO qTable = (QLearningTableSO)target;

            // Default inspector
            DrawDefaultInspector();

            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("Q-Table Management", EditorStyles.boldLabel);

            // Search and filter options
            EditorGUILayout.BeginHorizontal();
            searchFilter = EditorGUILayout.TextField("Search", searchFilter);
            showOnlyNonZero = EditorGUILayout.Toggle("Show Non-Zero Only", showOnlyNonZero);
            EditorGUILayout.EndHorizontal();

            // // Add manual entry
            // EditorGUILayout.Space(5);
            // EditorGUILayout.LabelField("Add New Entry", EditorStyles.boldLabel);
            // newKey = EditorGUILayout.TextField("Key", newKey);
            // newValue = EditorGUILayout.FloatField("Value", newValue);
            // if (GUILayout.Button("Add Entry"))
            // {
            //     if (!string.IsNullOrEmpty(newKey))
            //     {
            //         qTable.AddManualEntry(newKey, newValue);
            //         newKey = "";
            //         newValue = 0f;
            //     }
            // }

            // Export/Import buttons
            EditorGUILayout.Space(5);
            EditorGUILayout.LabelField("JSON Operations", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            jsonFilePath = EditorGUILayout.TextField("File Path", jsonFilePath);
            if (GUILayout.Button("Browse", GUILayout.Width(60)))
            {
                jsonFilePath = EditorUtility.SaveFilePanel(
                    "Save Q-Table JSON",
                    "",
                    "qtable.json",
                    "json"
                );
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Export to JSON"))
            {
                if (!string.IsNullOrEmpty(jsonFilePath))
                {
                    qTable.ExportToJson(jsonFilePath);
                    EditorUtility.DisplayDialog(
                        "Export Complete",
                        "Q-Table has been exported to JSON file.",
                        "OK"
                    );
                }
                else
                {
                    EditorUtility.DisplayDialog("Error", "Please specify a file path first.", "OK");
                }
            }

            if (GUILayout.Button("Import from JSON"))
            {
                if (!string.IsNullOrEmpty(jsonFilePath) && File.Exists(jsonFilePath))
                {
                    if (
                        EditorUtility.DisplayDialog(
                            "Import Q-Table",
                            "This will replace all current Q-Table data. Continue?",
                            "Yes",
                            "No"
                        )
                    )
                    {
                        qTable.ImportFromJson(jsonFilePath);
                        EditorUtility.DisplayDialog(
                            "Import Complete",
                            "Q-Table has been imported from JSON file.",
                            "OK"
                        );
                    }
                }
                else
                {
                    EditorUtility.DisplayDialog(
                        "Error",
                        "Please specify a valid file path first.",
                        "OK"
                    );
                }
            }
            EditorGUILayout.EndHorizontal();

            // Reset button
            if (GUILayout.Button("Reset Q-Table"))
            {
                if (
                    EditorUtility.DisplayDialog(
                        "Reset Q-Table",
                        "Are you sure you want to reset the Q-Table? This action cannot be undone.",
                        "Yes",
                        "No"
                    )
                )
                {
                    qTable.ResetTable();
                }
            }

            // EditorGUILayout.Space(5);
            // EditorGUILayout.LabelField("Q-Table Contents", EditorStyles.boldLabel);

            // // Display Q-Table contents
            // scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            // var table = qTable.GetQTable();
            // var filteredEntries = table
            //     .Where(kvp => string.IsNullOrEmpty(searchFilter) || kvp.Key.Contains(searchFilter))
            //     .Where(kvp => !showOnlyNonZero || kvp.Value != 0)
            //     .OrderByDescending(kvp => kvp.Value);

            // foreach (var entry in filteredEntries)
            // {
            //     EditorGUILayout.BeginHorizontal();
            //     EditorGUILayout.LabelField(entry.Key, GUILayout.Width(300));
            //     EditorGUILayout.LabelField(entry.Value.ToString("F4"));
            //     EditorGUILayout.EndHorizontal();
            // }

            // EditorGUILayout.EndScrollView();
        }
    }
}
