using UnityEditor;
using UnityEngine;

namespace ShadowRace.EditorTools
{
    public class LevelBuilderTool : EditorWindow
    {
        [MenuItem("Shadow Race/Level Builder Tool")]
        public static void ShowWindow()
        {
            GetWindow<LevelBuilderTool>("Level Builder");
        }

        private void OnGUI()
        {
            GUILayout.Label("Shadow Race - Level Scaffolding", EditorStyles.boldLabel);
            GUILayout.Space(10);

            if (GUILayout.Button("Create Basic Level Rig"))
            {
                CreateLevelRig();
            }

            GUILayout.Space(5);

            if (GUILayout.Button("Create Grid/Tilemap Layout"))
            {
                CreateGridSystem();
            }
        }

        private void CreateLevelRig()
        {
            // Managers
            GameObject managers = new GameObject("_Managers");
            
            // If they don't explicitly exist in the scene, add the core ones
            if (FindObjectOfType<ShadowRace.Core.LevelManager>() == null)
            {
                managers.AddComponent<ShadowRace.Core.LevelManager>();
            }

            // Environment group
            GameObject env = new GameObject("_Environment");
            new GameObject("Ground").transform.parent = env.transform;
            new GameObject("Hazards").transform.parent = env.transform;
            new GameObject("Platforms").transform.parent = env.transform;

            // Entities group
            GameObject entities = new GameObject("_Entities");
            new GameObject("Enemies").transform.parent = entities.transform;
            new GameObject("Loot").transform.parent = entities.transform;

            // Player Spawn Point
            GameObject pSpawn = new GameObject("PlayerSpawn");
            pSpawn.transform.position = Vector3.zero;

            Debug.Log("Level Rig Scaffolding Created!");
        }

        private void CreateGridSystem()
        {
            // Standard Unity 2D Tilemap setup
            GameObject grid = new GameObject("Grid");
            grid.AddComponent<Grid>();

            // Base Layer
            GameObject tilemapGround = new GameObject("Tilemap_Ground");
            tilemapGround.transform.parent = grid.transform;
            tilemapGround.AddComponent<UnityEngine.Tilemaps.Tilemap>();
            tilemapGround.AddComponent<UnityEngine.Tilemaps.TilemapRenderer>();
            var collider = tilemapGround.AddComponent<UnityEngine.Tilemaps.TilemapCollider2D>();
            var composite = tilemapGround.AddComponent<CompositeCollider2D>();
            composite.geometryType = CompositeCollider2D.GeometryType.Polygons;
            collider.usedByComposite = true;
            
            // Set Rigidbody to Static so standard tilemaps don't fall
            Rigidbody2D rb = tilemapGround.GetComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Static;

            Debug.Log("Standard Grid and Tilemap Created!");
        }
    }
}
