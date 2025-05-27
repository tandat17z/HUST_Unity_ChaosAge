using System.Collections.Generic;
using AILibraryForNPC.Core;
using AILibraryForNPC.Modules.GOAP;
using UnityEditor;
using UnityEngine;

public class GOAPDebugWindow : EditorWindow
{
    private GOAPAgent executor;
    private Dictionary<int, bool> expandedStates = new Dictionary<int, bool>();
    private float expandedStateHeight = 200f;
    private float collapsedStateHeight = 120f;

    [MenuItem("Tools/GOAP Debug Window")]
    public static void ShowWindow()
    {
        GetWindow<GOAPDebugWindow>("GOAP Debug");
    }

    private void OnEnable()
    {
        EditorApplication.update += EditorUpdate;
    }

    private void OnDisable()
    {
        EditorApplication.update -= EditorUpdate;
    }

    private void EditorUpdate()
    {
        Repaint(); // Buộc cửa sổ update liên tục
    }

    void OnGUI()
    {
        EditorGUILayout.LabelField("GOAP Debugger", EditorStyles.boldLabel);
        executor = (GOAPAgent)
            EditorGUILayout.ObjectField("Target Agent", executor, typeof(GOAPAgent), true);

        if (executor == null)
        {
            EditorGUILayout.HelpBox(
                "Drag your ExecutorSystem-based Agent here to view debug info.",
                MessageType.Info
            );
            return;
        }

        DrawWorldState();
        DrawGoalInfo();
        DrawCurrentPlan();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("GOAP Plan Graph", EditorStyles.boldLabel);
        DrawPlanGraph();
    }

    void DrawWorldState()
    {
        var ws = executor.GetWorldState();
        if (ws == null)
            return;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("World State", EditorStyles.boldLabel);

        foreach (var key in ws.GetStates())
        {
            EditorGUILayout.LabelField(key.Key + ": " + key.Value);
        }
    }

    void DrawGoalInfo()
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Current Goal", EditorStyles.boldLabel);

        var goalSystem = executor.GetComponent<GOAPGoalSystem>();
        if (goalSystem != null)
        {
            var currentGoal = goalSystem.GetCurrentGoal();
            var worldState = goalSystem.GetWorldState();
            for (int i = 0; i < goalSystem.goals.Count; i++)
            {
                if (goalSystem.goals[i] == currentGoal)
                    EditorGUILayout.LabelField(
                        $"{goalSystem.goals[i].GetName()}: {goalSystem.goals[i].GetWeight(worldState)}",
                        new GUIStyle(EditorStyles.label) { normal = { textColor = Color.green } }
                    );
                else
                    EditorGUILayout.LabelField(
                        $"{goalSystem.goals[i].GetName()}: {goalSystem.goals[i].GetWeight(worldState)}"
                    );
            }
            return;
            // if (currentGoal != null)
            // {
            //     EditorGUILayout.LabelField(
            //         currentGoal.GetName() + ": " + currentGoal.GetWeight(worldState)
            //     );
            //     return;
            // }
        }
        EditorGUILayout.LabelField("No goal system found.");
    }

    void DrawCurrentPlan()
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Plan (Action Queue)", EditorStyles.boldLabel);

        var plan = executor.GetComponent<GOAPActionSystem>().GetCurrentPlan();
        if (plan == null || plan.Count == 0)
        {
            EditorGUILayout.LabelField("Plan is empty or not generated.");
            return;
        }

        var currentActionIndex = executor.GetComponent<GOAPActionSystem>().GetCurrentActionIndex();
        for (int i = 0; i < plan.Count; i++)
        {
            if (i == currentActionIndex)
                EditorGUILayout.LabelField(
                    $"Step {i + 1}: {plan[i].GetType().Name}",
                    new GUIStyle(EditorStyles.label) { normal = { textColor = Color.green } }
                );
            else
                EditorGUILayout.LabelField($"Step {i + 1}: {plan[i].GetType().Name}");
        }
    }

    private Vector2 graphScroll;

    private void DrawPlanGraph()
    {
        var plan = executor.GetComponent<GOAPActionSystem>().GetCurrentPlan();
        var worldState = executor.GetComponent<GOAPActionSystem>().GetBeginWorldState();
        if (executor == null || plan == null || plan.Count == 0)
        {
            EditorGUILayout.HelpBox("No plan to draw.", MessageType.Info);
            return;
        }

        // Draw graph in a single box
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        graphScroll = EditorGUILayout.BeginScrollView(graphScroll, GUILayout.Height(400));
        Rect canvas = GUILayoutUtility.GetRect(plan.Count * 250, 350);
        GUI.BeginGroup(canvas);

        float nodeWidth = 150f;
        float nodeHeight = 40f;
        float stateNodeWidth = 200f;
        float spacing = 250f;
        float startX = 50f;
        float startY = 20f;

        Handles.BeginGUI();

        var currentActionIndex = executor.GetComponent<GOAPActionSystem>().GetCurrentActionIndex();
        var nextWorldState = worldState.Clone();

        // Draw initial state node
        Rect initialStateRect = new Rect(startX, startY, stateNodeWidth, collapsedStateHeight);
        GUI.color = Color.white;
        GUI.Box(initialStateRect, "Initial State");
        GUI.color = Color.white;

        // Draw initial state content
        var initialStateScroll = new Vector2();
        var initialStateContent = new Rect(
            initialStateRect.x + 5,
            initialStateRect.y + 25,
            stateNodeWidth - 10,
            collapsedStateHeight - 30
        );
        GUI.BeginGroup(initialStateContent);
        initialStateScroll = GUI.BeginScrollView(
            new Rect(0, 0, stateNodeWidth - 10, collapsedStateHeight - 30),
            initialStateScroll,
            new Rect(0, 0, stateNodeWidth - 20, 100)
        );
        DrawStateContent(worldState.GetStates());
        GUI.EndScrollView();
        GUI.EndGroup();

        // Add expand/collapse button for initial state
        if (
            GUI.Button(
                new Rect(initialStateRect.xMax - 25, initialStateRect.y + 5, 20, 20),
                expandedStates.ContainsKey(-1) && expandedStates[-1] ? "▼" : "▶"
            )
        )
        {
            if (!expandedStates.ContainsKey(-1))
                expandedStates[-1] = false;
            expandedStates[-1] = !expandedStates[-1];
        }

        var preRect = initialStateRect;
        for (int i = 0; i < plan.Count; i++)
        {
            var action = plan[i];
            Vector2 pos = new Vector2(startX + (i + 1) * spacing, startY);

            // Draw action node
            Rect nodeRect = new Rect(pos.x, pos.y, nodeWidth, nodeHeight);
            GUI.color = (i == currentActionIndex) ? Color.green : Color.white;
            GUI.Box(nodeRect, action.GetType().Name);
            GUI.color = Color.white;

            // Draw state node
            float stateHeight =
                expandedStates.ContainsKey(i) && expandedStates[i]
                    ? expandedStateHeight
                    : collapsedStateHeight;
            Rect stateRect = new Rect(pos.x, pos.y + nodeHeight + 10, stateNodeWidth, stateHeight);
            GUI.color = Color.yellow;
            GUI.Box(stateRect, "State " + (i + 1));
            GUI.color = Color.white;

            // Apply action effect to get next state
            action.ApplyEffect(nextWorldState);

            // Draw state content in scroll view
            var stateScroll = new Vector2();
            var stateContent = new Rect(
                stateRect.x + 5,
                stateRect.y + 25,
                stateNodeWidth - 10,
                stateHeight - 30
            );
            GUI.BeginGroup(stateContent);
            stateScroll = GUI.BeginScrollView(
                new Rect(0, 0, stateNodeWidth - 10, stateHeight - 30),
                stateScroll,
                new Rect(0, 0, stateNodeWidth - 20, 100)
            );
            DrawStateContent(nextWorldState.GetStates());
            GUI.EndScrollView();
            GUI.EndGroup();

            // Add expand/collapse button
            if (
                GUI.Button(
                    new Rect(stateRect.xMax - 25, stateRect.y + 5, 20, 20),
                    expandedStates.ContainsKey(i) && expandedStates[i] ? "▼" : "▶"
                )
            )
            {
                if (!expandedStates.ContainsKey(i))
                    expandedStates[i] = false;
                expandedStates[i] = !expandedStates[i];
            }

            // Draw arrows
            if (i < plan.Count - 1)
            {
                // Arrow from initial state to first action
                Vector2 from = new Vector2(preRect.xMax, preRect.center.y);
                Vector2 to = new Vector2(nodeRect.xMin, nodeRect.center.y);
                DrawArrow(from, to);
            }
            preRect = nodeRect;
        }

        Handles.EndGUI();
        GUI.EndGroup();
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }

    private void DrawStateContent(Dictionary<string, float> states)
    {
        float yOffset = 0;
        foreach (var state in states)
        {
            GUI.Label(new Rect(0, yOffset, 180, 20), $"{state.Key}: {state.Value:F2}");
            yOffset += 20;
        }
    }

    private void DrawArrow(Vector2 from, Vector2 to)
    {
        Handles.DrawLine(from, to);

        // Draw arrowhead
        Vector2 dir = (to - from).normalized;
        Vector2 perp = Vector2.Perpendicular(dir) * 5f;
        Handles.DrawAAConvexPolygon(to, to - dir * 10f + perp, to - dir * 10f - perp);
    }
}
