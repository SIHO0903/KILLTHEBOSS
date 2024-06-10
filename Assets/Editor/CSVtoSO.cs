using UnityEditor;
using System.IO;
using UnityEngine;
using Model;


public class CSVtoSO
{
    //Editor폴더에 있는 .csv파일위치
    static string enemyCSVPath = "/Editor/CSVs/EnemyData.csv";

    [MenuItem("MyUtilties/Generate Enemies")]
    public static void GenerateEnemies()
    {
        //파일읽은후 EnemyStatsSO에 데이터대입후 스크립터블오브젝트생성 및 저장
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
