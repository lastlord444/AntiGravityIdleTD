using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using BlockForge.Gameplay;
using BlockForge.Meta;

namespace BlockForge.Editor
{
    public static class DataSetupTool
    {
        [MenuItem("BlockForge/Generate Default Data")]
        public static void GenerateDefaultData()
        {
            GenerateShapes();
            GenerateMetaItems();
            Debug.Log("[DataSetupTool] All default data generated!");
        }

        private static void GenerateShapes()
        {
            string path = "Assets/_Project/ScriptableObjects/Shapes";
            EnsureDirectory(path);

            // 1. Single Block
            CreateShape(path, "classic_1x1", "Single", new List<Vector2Int> { Vector2Int.zero }, Color.yellow);

            // 2. I-Shapes
            CreateShape(path, "classic_i2", "I-2", new List<Vector2Int> { new Vector2Int(0,0), new Vector2Int(1,0) }, Color.cyan);
            CreateShape(path, "classic_i3", "I-3", new List<Vector2Int> { new Vector2Int(0,0), new Vector2Int(1,0), new Vector2Int(2,0) }, Color.cyan);
            CreateShape(path, "classic_i4", "I-4", new List<Vector2Int> { new Vector2Int(0,0), new Vector2Int(1,0), new Vector2Int(2,0), new Vector2Int(3,0) }, Color.cyan);
            CreateShape(path, "classic_i5", "I-5", new List<Vector2Int> { new Vector2Int(0,0), new Vector2Int(1,0), new Vector2Int(2,0), new Vector2Int(3,0), new Vector2Int(4,0) }, Color.cyan);

            // 3. L-Shapes (2x2 L, 3x3 L)
            CreateShape(path, "classic_l2", "L-Small", new List<Vector2Int> { new Vector2Int(0,0), new Vector2Int(0,1), new Vector2Int(1,0) }, Color.green);
            CreateShape(path, "classic_l3", "L-Large", new List<Vector2Int> { new Vector2Int(0,0), new Vector2Int(0,1), new Vector2Int(0,2), new Vector2Int(1,2) }, Color.green);

            // 4. Square Shapes
            CreateShape(path, "classic_sq2", "Square-2x2", new List<Vector2Int> { new Vector2Int(0,0), new Vector2Int(1,0), new Vector2Int(0,1), new Vector2Int(1,1) }, Color.yellow);
            CreateShape(path, "classic_sq3", "Square-3x3", new List<Vector2Int> { 
                new Vector2Int(0,0), new Vector2Int(1,0), new Vector2Int(2,0),
                new Vector2Int(0,1), new Vector2Int(1,1), new Vector2Int(2,1),
                new Vector2Int(0,2), new Vector2Int(1,2), new Vector2Int(2,2)
            }, Color.yellow);

            // 5. T-Shapes
            CreateShape(path, "classic_t3", "T-Shape", new List<Vector2Int> { new Vector2Int(0,0), new Vector2Int(1,0), new Vector2Int(2,0), new Vector2Int(1,1) }, Color.magenta);

            // More variations...
            CreateShape(path, "classic_z3", "Z-Shape", new List<Vector2Int> { new Vector2Int(0,1), new Vector2Int(1,1), new Vector2Int(1,0), new Vector2Int(2,0) }, Color.red);
            CreateShape(path, "classic_s3", "S-Shape", new List<Vector2Int> { new Vector2Int(0,0), new Vector2Int(1,0), new Vector2Int(1,1), new Vector2Int(2,1) }, Color.red);

            AssetDatabase.SaveAssets();
        }

        private static void CreateShape(string folder, string id, string displayName, List<Vector2Int> blocks, Color color)
        {
            string fullPath = $"{folder}/{id}.asset";
            var shape = ScriptableObject.CreateInstance<ShapeSO>();
            
            // ScriptableObject private field'lerine erişim zor oldugu icin SerializedObject kullanabiliriz veya public init metodu ekleyebilirdik.
            // Ama ShapeSO'da fieldlar private ve SerializeField.
            // Editör scriptinde oldugumuz icin SerializedObject ile set edebiliriz.
            
            var so = new SerializedObject(shape);
            so.FindProperty("_id").stringValue = id;
            so.FindProperty("_displayName").stringValue = displayName;
            so.FindProperty("_color").colorValue = color;
            
            var blocksProp = so.FindProperty("_blocks");
            blocksProp.ClearArray();
            for (int i = 0; i < blocks.Count; i++)
            {
                blocksProp.InsertArrayElementAtIndex(i);
                blocksProp.GetArrayElementAtIndex(i).vector2IntValue = blocks[i];
            }
            so.ApplyModifiedProperties();

            AssetDatabase.CreateAsset(shape, fullPath);
        }

        private static void GenerateMetaItems()
        {
            // Items
            string itemPath = "Assets/_Project/ScriptableObjects/Items";
            EnsureDirectory(itemPath);
            CreateItem(itemPath, "raw_block", "Raw Block", "Basic building block", 10);
            CreateItem(itemPath, "iron_plate", "Iron Plate", "Refined metal", 50);
            CreateItem(itemPath, "gear", "Gear", "Mechanical part", 150);

            // Machines
            string machinePath = "Assets/_Project/ScriptableObjects/Machines";
            EnsureDirectory(machinePath);
            CreateMachine(machinePath, "furnace", "Furnace", "Smelts raw blocks", 100);
            CreateMachine(machinePath, "assembler", "Assembler", "Assembles parts", 500);

            // Contracts
            string contractPath = "Assets/_Project/ScriptableObjects/Contracts";
            EnsureDirectory(contractPath);
            CreateContract(contractPath, "contract_001", "Starter Order", "Deliver 100 Raw Blocks", 100, "raw_block", 100);
            CreateContract(contractPath, "contract_002", "Iron Supply", "Deliver 50 Iron Plates", 500, "iron_plate", 50);

            AssetDatabase.SaveAssets();
        }

        private static void CreateItem(string folder, string id, string name, string desc, int value)
        {
            var asset = ScriptableObject.CreateInstance<ItemSO>();
            var so = new SerializedObject(asset);
            so.FindProperty("_id").stringValue = id;
            so.FindProperty("_displayName").stringValue = name;
            so.FindProperty("_description").stringValue = desc;
            so.FindProperty("_baseValue").intValue = value;
            so.ApplyModifiedProperties();
            AssetDatabase.CreateAsset(asset, $"{folder}/{id}.asset");
        }

        private static void CreateMachine(string folder, string id, string name, string desc, int cost)
        {
            var asset = ScriptableObject.CreateInstance<MachineSO>();
            var so = new SerializedObject(asset);
            so.FindProperty("_id").stringValue = id;
            so.FindProperty("_displayName").stringValue = name;
            so.FindProperty("_description").stringValue = desc;
            so.FindProperty("_baseCost").intValue = cost;
            // Production outputs vs eklenebilir
            so.ApplyModifiedProperties();
            AssetDatabase.CreateAsset(asset, $"{folder}/{id}.asset");
        }

        private static void CreateContract(string folder, string id, string name, string desc, int reward, string reqItemId, int reqAmount)
        {
            var asset = ScriptableObject.CreateInstance<ContractSO>();
            var so = new SerializedObject(asset);
            so.FindProperty("_id").stringValue = id;
            so.FindProperty("_title").stringValue = name;
            so.FindProperty("_description").stringValue = desc;
            so.FindProperty("_coinReward").intValue = reward;
            
            // Requirements array
            var reqs = so.FindProperty("_requirements");
            reqs.ClearArray();
            reqs.InsertArrayElementAtIndex(0);
            var req = reqs.GetArrayElementAtIndex(0);
            
            // ItemSO referansı bulmaya çalışalım
            var item = AssetDatabase.LoadAssetAtPath<ItemSO>($"Assets/_Project/ScriptableObjects/Items/{reqItemId}.asset");
            if (item != null)
            {
                req.FindPropertyRelative("item").objectReferenceValue = item;
                req.FindPropertyRelative("amount").intValue = reqAmount;
            }

            so.ApplyModifiedProperties();
            AssetDatabase.CreateAsset(asset, $"{folder}/{id}.asset");
        }

        private static void EnsureDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}
