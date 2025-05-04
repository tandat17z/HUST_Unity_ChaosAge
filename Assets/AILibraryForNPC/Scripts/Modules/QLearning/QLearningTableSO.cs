using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace AILibraryForNPC.Modules.QLearning
{
    [CreateAssetMenu(fileName = "QLearningTableSO", menuName = "AI/QLearning/QLearning Table")]
    public class QLearningTableSO : ScriptableObject
    {
        [Serializable]
        private class QTableEntry
        {
            public string key;
            public float value;
        }

        [Serializable]
        private class QTableData
        {
            public List<QTableEntry> entries = new List<QTableEntry>();
            public float defaultQValue;
        }

        [SerializeField]
        private List<QTableEntry> serializedQTable = new List<QTableEntry>();

        private Dictionary<string, float> qTable = new Dictionary<string, float>();

        [SerializeField]
        private float defaultQValue = 0f;

        private void OnEnable()
        {
            // Load data from serialized list when the asset is loaded
            qTable.Clear();
            foreach (var entry in serializedQTable)
            {
                qTable[entry.key] = entry.value;
            }
        }

        private void OnDisable()
        {
            // Save data to serialized list when the asset is unloaded
            serializedQTable.Clear();
            foreach (var kvp in qTable)
            {
                serializedQTable.Add(new QTableEntry { key = kvp.Key, value = kvp.Value });
            }
        }

        public float GetQValue(string state, int action)
        {
            string key = $"{state}_{action}";
            if (!qTable.ContainsKey(key))
            {
                qTable[key] = defaultQValue;
            }
            return qTable[key];
        }

        public void UpdateQValue(string state, int action, float value)
        {
            string key = $"{state}_{action}";
            qTable[key] = value;
        }

        public void AddManualEntry(string key, float value)
        {
            qTable[key] = value;
        }

        public void ResetTable()
        {
            qTable.Clear();
            serializedQTable.Clear();
        }

        public Dictionary<string, float> GetQTable()
        {
            return new Dictionary<string, float>(qTable);
        }

        public void ExportToJson(string filePath)
        {
            var data = new QTableData
            {
                entries = qTable
                    .Select(kvp => new QTableEntry { key = kvp.Key, value = kvp.Value })
                    .ToList(),
                defaultQValue = defaultQValue,
            };

            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        public void ImportFromJson(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Debug.LogError($"File not found: {filePath}");
                return;
            }

            string json = File.ReadAllText(filePath);
            var data = JsonConvert.DeserializeObject<QTableData>(json);

            if (data != null)
            {
                qTable.Clear();
                foreach (var entry in data.entries)
                {
                    qTable[entry.key] = entry.value;
                }
                defaultQValue = data.defaultQValue;
            }
        }
    }
}
