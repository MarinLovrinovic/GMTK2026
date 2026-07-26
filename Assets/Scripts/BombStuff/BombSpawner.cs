using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class BombSpawner : MonoBehaviour
{
    [SerializeField] internal Bomb[] bombs;
    [SerializeField] internal float[] weightedProbabilities;

    private int totalTimer = 0;
    private int timeUntilNextBomb = 2;

    private void OnEnable()
    {
        TickDriver.instance.OnTick += Tick;
    }

    private void OnDisable()
    {
        TickDriver.instance.OnTick -= Tick;
    }

    void Tick()
    {
        totalTimer++;
        timeUntilNextBomb--;

        if (timeUntilNextBomb <= 0)
        {
            Vector2 location = new Vector2(Random.Range(-7f, 7f), Random.Range(-3.5f, 3.5f)) * Scaler.Scale;
            Bomb bombType = MathHelper.WeightedRandomFromDistributionArray<Bomb>(bombs, weightedProbabilities);
            Instantiate(bombType, location.xoy(), Quaternion.identity);

            timeUntilNextBomb = Random.Range(1, 1 + Mathf.Min(6, 60 / totalTimer));
        }
    }
    
    /*
    private float totalTimer = 0;
    [SerializeField] private Bomb[] bombs;
    [SerializeField] private float[] weightedProbabilities;
    private float timeUntilNextBomb = 2;
    private void Update()
    {
        totalTimer += Time.deltaTime;
        timeUntilNextBomb -= Time.deltaTime;
        if (timeUntilNextBomb <= 0)
        {
            Vector2 location = new Vector2(Random.Range(-7f, 7f), Random.Range(-3.5f, 3.5f)) * Scaler.Scale;
            Bomb bombType = MathHelper.WeightedRandomFromDistributionArray<Bomb>(bombs, weightedProbabilities);
            Instantiate(bombType, location.xoy(), Quaternion.identity);

            timeUntilNextBomb = Random.Range(1f, 1f + Mathf.Min(6, 60 / totalTimer));
        }
    }
    */
}

#if UNITY_EDITOR
[CustomEditor(typeof(BombSpawner))]
public class BombSpawnerEditor : Editor
{
    BombSpawner bombSpawner;
    string prefabsPath = "Prefabs/Bombs";

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        BombSpawner bombSpawner = (BombSpawner)target;

        while (bombSpawner.weightedProbabilities.Length < bombSpawner.bombs.Length)
        {
            List<float> temp = new List<float>(bombSpawner.weightedProbabilities);
            temp.Add(0f);
            bombSpawner.weightedProbabilities = temp.ToArray();
        }
        if (bombSpawner.bombs.Length != bombSpawner.weightedProbabilities.Length)
        {
            Debug.LogWarning("Bomb and weighted probabilities array sizes differ; clearing both arrays.");
            bombSpawner.bombs = new Bomb[0]; bombSpawner.weightedProbabilities = new float[0];
        }

        if (bombSpawner.bombs.Length > 0)
        {
            float longestName = 0;
            GUIStyle labelStyle = EditorStyles.label;
            foreach (Bomb b in bombSpawner.bombs)
            {
                float width = labelStyle.CalcSize(new GUIContent(GetBombName(b))).x;
                if (width > longestName) { longestName = width; }
            }
            EditorGUILayout.LabelField("Bombs");
            for (int i = 0; i < bombSpawner.bombs.Length; ++i)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(GetBombName(bombSpawner.bombs[i]), GUILayout.Width(longestName));
                bombSpawner.bombs[i] = (Bomb)EditorGUILayout.ObjectField(bombSpawner.bombs[i], typeof(Bomb), false);
                bombSpawner.weightedProbabilities[i] = EditorGUILayout.FloatField(bombSpawner.weightedProbabilities[i]);
                EditorGUILayout.EndHorizontal();
            }
        }
        else { EditorGUILayout.LabelField("No bombs added."); }


        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Fetch all bombs")) { FetchAllBombs(); }
        prefabsPath = GUILayout.TextField(prefabsPath);
        EditorGUILayout.EndHorizontal();
    }

    void FetchAllBombs()
    {
        BombSpawner bombSpawner = (BombSpawner)target;
        string fullPath = "Assets/" + prefabsPath;
        List<Bomb> foundBombs = new List<Bomb>();

        foreach (string folder in AssetDatabase.GetSubFolders(fullPath))
        {
            string folderName = System.IO.Path.GetFileName(folder);
            string prefabPath = $"{folder}/{folderName}.prefab";

            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

            if (prefab != null && prefab.GetComponent<Bomb>() is Bomb bomb)
            { foundBombs.Add(bomb); Debug.Log(bomb); }
            else
            {
                if (prefab == null) { Debug.LogWarning($"Expected prefab not found: {prefabPath}"); }
                else { Debug.LogWarning($"Prefab '{prefabPath}' doesn't have a Bomb script."); }
            }
        }

        List<float> probs = new List<float>();
        for (int i = 0; i < foundBombs.Count; ++i)
        {
            int existingIndex = System.Array.IndexOf(bombSpawner.bombs, foundBombs[i]);
            if (existingIndex != -1) { probs.Add(bombSpawner.weightedProbabilities[existingIndex]); }
            else { probs.Add(0f); }
        }
        bombSpawner.bombs = foundBombs.ToArray();
        bombSpawner.weightedProbabilities = probs.ToArray();
    }

    string GetBombName(Bomb bomb)
    {
        string result = bomb.name;
        if (result.ToLower().EndsWith("bomb"))
        {
            result = result.Substring(0, result.Length - 4);
        }
        if (result.Length == 0) { result = "Basic"; }
        return result;
    }
}

#endif