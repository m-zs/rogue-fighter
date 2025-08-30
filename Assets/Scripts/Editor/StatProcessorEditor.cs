using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(StatProcessor))]
public class StatProcessorEditor : Editor
{
    private SerializedProperty operationsProperty;
    private SerializedProperty defaultValueProperty;
    private StatProcessor processor;
    private Vector2 scrollPosition;
    private Dictionary<OperationPayload, bool> foldoutStates = new Dictionary<OperationPayload, bool>();

    private void OnEnable()
    {
        operationsProperty = serializedObject.FindProperty("operations");
        defaultValueProperty = serializedObject.FindProperty("defaultValue");
        processor = (StatProcessor)target;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        // Draw the defaultValue field at the top
        EditorGUILayout.PropertyField(defaultValueProperty);
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Operations", EditorStyles.boldLabel);
        
        if (processor.operations == null)
        {
            processor.operations = new List<OperationPayload>();
        }

        // Draw operations list
        for (var i = 0; i < processor.operations.Count; i++)
        {
            DrawOperation(processor.operations[i], i, processor.operations);
            EditorGUILayout.Space();
        }

        // Add operation buttons
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add Stat Operation"))
        {
            processor.operations.Add(new OperationStatPayload());
        }
        if (GUILayout.Button("Add Value Operation"))
        {
            processor.operations.Add(new OperationValuePayload());
        }
        if (GUILayout.Button("Add Combined Operation"))
        {
            processor.operations.Add(new CombinedPayload());
        }
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Clear All Operations"))
        {
            processor.operations.Clear();
            foldoutStates.Clear();
        }

        EditorGUILayout.Space();

        // Preview section
        EditorGUILayout.LabelField("Preview", EditorStyles.boldLabel);
        
        if (GUILayout.Button("Show Computation Formula"))
        {
            ShowFormula();
        }

        if (Application.isPlaying && GUILayout.Button("Test Compute with Default Stats"))
        {
            TestComputation();
        }

        EditorGUILayout.EndScrollView();
        serializedObject.ApplyModifiedProperties();
    }

    private void DrawOperation(OperationPayload operation, int index, List<OperationPayload> parentList, int depth = 0)
    {
        if (operation == null) return;

        // Initialize foldout state
        if (!foldoutStates.ContainsKey(operation))
            foldoutStates[operation] = depth == 0; // Expand top-level by default

        var indent = new string(' ', depth * 15);
        var headerPrefix = depth > 0 ? $"{indent}↳ " : "";

        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        
        // Header
        EditorGUILayout.BeginHorizontal();
        
        if (operation is CombinedPayload)
        {
            foldoutStates[operation] = EditorGUILayout.Foldout(foldoutStates[operation], 
                $"{headerPrefix}Combined Operation [{index}]", true);
        }
        else
        {
            EditorGUILayout.LabelField($"{headerPrefix}Operation [{index}]", EditorStyles.boldLabel);
        }
        
        // Move buttons
        if (GUILayout.Button("↑", GUILayout.Width(20)) && index > 0)
        {
            (parentList[index], parentList[index - 1]) = (parentList[index - 1], parentList[index]);
        }
        
        if (GUILayout.Button("↓", GUILayout.Width(20)) && index < parentList.Count - 1)
        {
            (parentList[index], parentList[index + 1]) = (parentList[index + 1], parentList[index]);
        }
        
        // Remove button
        if (GUILayout.Button("X", GUILayout.Width(20)))
        {
            parentList.RemoveAt(index);
            foldoutStates.Remove(operation);
            return;
        }
        
        EditorGUILayout.EndHorizontal();

        // Operation content
        if (operation is not CombinedPayload || foldoutStates[operation])
        {
            DrawOperationContent(operation, parentList, depth);
        }

        EditorGUILayout.EndVertical();
    }

    private void DrawOperationContent(OperationPayload operation, List<OperationPayload> parentList, int depth)
    {
        // Draw operation type dropdown (only for non-Combined operations)
        if (!(operation is CombinedPayload))
        {
            EditorGUI.BeginChangeCheck();
            var currentType = operation.GetType();
            var newTypeIndex = currentType == typeof(OperationStatPayload) ? 0 : 
                              currentType == typeof(OperationValuePayload) ? 1 : 2;
            
            var selectedIndex = EditorGUILayout.Popup("Type", newTypeIndex, 
                new[] { "Stat Operation", "Value Operation", "Combined Operation" });
            
            if (EditorGUI.EndChangeCheck() && selectedIndex != newTypeIndex)
            {
                OperationPayload newOp = selectedIndex switch
                {
                    0 => new OperationStatPayload { operation = operation.operation },
                    1 => new OperationValuePayload { operation = operation.operation, value = 0 },
                    2 => new CombinedPayload { operation = operation.operation },
                    _ => operation
                };
                
                var index = parentList.IndexOf(operation);
                if (index >= 0)
                {
                    parentList[index] = newOp;
                    foldoutStates.Remove(operation);
                    if (newOp is CombinedPayload)
                        foldoutStates[newOp] = true;
                }
                return;
            }
        }

        // Draw common operation field
        operation.operation = (Operation)EditorGUILayout.EnumPopup("Operation", operation.operation);

        switch (operation)
        {
            // Draw type-specific fields
            case OperationStatPayload statOp:
                statOp.stat = (Stat)EditorGUILayout.EnumPopup("Stat", statOp.stat);
                break;
            case OperationValuePayload valueOp:
                valueOp.value = EditorGUILayout.IntField("Value", valueOp.value);
                break;
            case CombinedPayload combinedOp:
            {
                // Draw nested operations
                if (combinedOp.operations == null)
                    combinedOp.operations = new List<OperationPayload>();

                EditorGUILayout.LabelField("Nested Operations:", EditorStyles.miniBoldLabel);
                EditorGUILayout.BeginVertical(EditorStyles.textArea);
            
                for (var i = 0; i < combinedOp.operations.Count; i++)
                {
                    DrawOperation(combinedOp.operations[i], i, combinedOp.operations, depth + 1);
                    EditorGUILayout.Space();
                }

                // Add nested operation buttons
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Add Stat", EditorStyles.miniButtonLeft))
                {
                    combinedOp.operations.Add(new OperationStatPayload());
                }
                if (GUILayout.Button("Add Value", EditorStyles.miniButtonMid))
                {
                    combinedOp.operations.Add(new OperationValuePayload());
                }
                if (GUILayout.Button("Add Nested", EditorStyles.miniButtonRight))
                {
                    combinedOp.operations.Add(new CombinedPayload());
                }
                EditorGUILayout.EndHorizontal();

                if (GUILayout.Button("Clear Nested", EditorStyles.miniButton))
                {
                    combinedOp.operations.Clear();
                }

                EditorGUILayout.EndVertical();
                break;
            }
        }
    }

    private void ShowFormula()
    {
        if (processor.operations == null || processor.operations.Count == 0)
        {
            EditorUtility.DisplayDialog("Formula", $"Result = {processor.defaultValue}", "OK");
            return;
        }

        var formula = processor.defaultValue.ToString();
        formula = BuildFormula(formula, processor.operations, 0);
        
        EditorUtility.DisplayDialog("Computation Formula", formula, "OK");
    }

    private string BuildFormula(string currentFormula, List<OperationPayload> operations, int depth)
    {
        foreach (var op in operations)
        {
            var operand = op switch
            {
                OperationStatPayload statOp => $"{statOp.stat}",
                OperationValuePayload valueOp => valueOp.value.ToString(),
                CombinedPayload combinedOp => $"({BuildFormula(combinedOp.operations[0].GetValue(new Stats()).ToString(), combinedOp.operations, depth + 1)})",
                _ => "?"
            };

            string opSymbol = op.operation switch
            {
                Operation.Add => " + ",
                Operation.Subtract => " - ",
                Operation.Multiply => " * ",
                Operation.Divide => " / ",
                Operation.Power => " ^ ",
                Operation.Clamp => " clamp to ",
                _ => " ? "
            };

            currentFormula += $"{opSymbol}{operand}";
        }

        return currentFormula;
    }

    private void TestComputation()
    {
        var testStats = new Stats()
        {
            Strength = 10,
            Dexterity = 10,
            Intelligence = 10,
            Endurance = 10,
            Luck = 10,
            Size = 10,
            MovementSpeed = 5,
            MaxPhysicalDefence = 80,
            PhysicalDefence = 10,
            MaxElementalDefence = 80,
            ElementalDefence = 10
        };
        
        var result = processor.Compute(testStats);
        EditorUtility.DisplayDialog("Compute Result", $"Result: {result}", "OK");
    }
}
