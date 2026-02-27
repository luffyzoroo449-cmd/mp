using UnityEditor;
using UnityEngine;
using ShadowRace.AI;

namespace ShadowRace.EditorTools
{
    public class EnemySetupTool : EditorWindow
    {
        private GameObject selectedEnemy;

        [MenuItem("Shadow Race/Enemy Quick Setup")]
        public static void ShowWindow()
        {
            GetWindow<EnemySetupTool>("Enemy Setup");
        }

        private void OnGUI()
        {
            GUILayout.Label("Auto-Configure Enemy Components", EditorStyles.boldLabel);
            GUILayout.Space(10);

            selectedEnemy = (GameObject)EditorGUILayout.ObjectField("Enemy Object:", selectedEnemy, typeof(GameObject), true);

            GUILayout.Space(10);

            if (selectedEnemy != null)
            {
                if (GUILayout.Button("Setup Melee Enemy Collider & Brain"))
                {
                    SetupEnemy<MeleeEnemy>();
                }

                if (GUILayout.Button("Setup Ranged Enemy Collider & Brain"))
                {
                    SetupEnemy<RangedEnemy>();
                }
            }
            else
            {
                GUILayout.Label("Select a GameObject from the Hierarchy first.", EditorStyles.helpBox);
            }
        }

        private void SetupEnemy<T>() where T : Component
        {
            if (selectedEnemy == null) return;

            // Add Rigidbody if missing
            Rigidbody2D rb = selectedEnemy.GetComponent<Rigidbody2D>();
            if (rb == null)
            {
                rb = selectedEnemy.AddComponent<Rigidbody2D>();
                rb.constraints = RigidbodyConstraints2D.FreezeRotation; // Crucial for 2D enemies
            }

            // Add Collider if missing
            BoxCollider2D col = selectedEnemy.GetComponent<BoxCollider2D>();
            if (col == null)
            {
                selectedEnemy.AddComponent<BoxCollider2D>();
            }

            // Add the Brain if missing
            if (selectedEnemy.GetComponent<T>() == null)
            {
                selectedEnemy.AddComponent<T>();
            }
            
            // Ensure child hierarchy for attack/vision points exists
            CreateHookTransform(selectedEnemy.transform, "PlayerCheckPoint", Vector3.zero);
            CreateHookTransform(selectedEnemy.transform, "GroundCheckPoint", new Vector3(0, -1f, 0));
            CreateHookTransform(selectedEnemy.transform, "WallCheckPoint", new Vector3(1f, 0, 0));

            Debug.Log($"Successfully configured {selectedEnemy.name} as a {typeof(T).Name}!");
        }

        private void CreateHookTransform(Transform parent, string name, Vector3 localOffset)
        {
            Transform existing = parent.Find(name);
            if (existing == null)
            {
                GameObject hook = new GameObject(name);
                hook.transform.SetParent(parent);
                hook.transform.localPosition = localOffset;
            }
        }
    }
}
