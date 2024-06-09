using UnityEditor;
using System.IO;
using UnityEngine;

public class CSVtoSO
{
    static string enemyCSVPath = "/Editor/CSVs/EnemyData.csv";

    [UnityEditor.MenuItem("MyUtilties/Generate Enemies")]
    public static void GenerateEnemies()
    {
        string[] data = File.ReadAllLines(Application.dataPath + enemyCSVPath);
        Debug.Log("data.Length : " + data.Length);
        for (int i = 1; i < data.Length; i++)
        {
            string[] splitData = data[i].Split(',');

            EnemyStatsSO enemy = ScriptableObject.CreateInstance<EnemyStatsSO>();
            enemy.name = splitData[0];
            enemy.maxHealth = float.Parse(splitData[1]);
            enemy.curHealth = float.Parse(splitData[2]);
            enemy.moveSpeed = float.Parse(splitData[3]);
            enemy.projectileDamage = float.Parse(splitData[4]);
            enemy.AOEDamage = float.Parse(splitData[5]);
            enemy.contactDamage = float.Parse(splitData[6]);

            AssetDatabase.CreateAsset(enemy, $"Assets/Datas/Enemy/{enemy.name}.asset");
        }
        AssetDatabase.SaveAssets();
    }
}
