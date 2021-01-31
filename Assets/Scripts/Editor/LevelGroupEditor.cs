using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelGroup))]
public class LevelGroupEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("清除所有"))
        {
            Clear();
        }
        if (GUILayout.Button("生成基本地面"))
        {
            ClearFloor();

            var levelGroup = (LevelGroup)target;
            var startX = -levelGroup.width >> 1;
            var endX = startX + levelGroup.width;
            var startY = -levelGroup.height >> 1;
            var endY = startY + levelGroup.height;
            var floorGo = new GameObject("FloorNode");
            var wallGo = new GameObject("WallNode");
            floorGo.transform.SetParent(levelGroup.transform);
            wallGo.transform.SetParent(levelGroup.transform);

            for (int i = startX; i < endX; i++)
            {
                for (int j = startY; j < endY; j++)
                {
                    var pos = new Vector3(i, 0, j);
                    var floor = (Transform)PrefabUtility.InstantiatePrefab(levelGroup.P_Floor, floorGo.transform);
                    //var floor = Instantiate(levelGroup.P_Floor, floorGo.transform);
                    floor.position = pos;
                    floor.name = "Floor[" + i + "," + j + "]";
                    if (i == startX || i == endX - 1 || j == startY || j == endY - 1)
                    {
                        var wall = (Transform)PrefabUtility.InstantiatePrefab(levelGroup.P_Wall, wallGo.transform);
                        //var wall = Instantiate(levelGroup.P_Wall, wallGo.transform);
                        wall.name = "Wall[" + i + "," + j + "]";
                        wall.position = pos;
                    }
                }
            }
        }
    }

    private void Clear()
    {
        var levelGroup = (LevelGroup)target;

        var cnt = levelGroup.transform.childCount;
        for (int i = cnt - 1; i >= 0; i--)
        {
            DestroyImmediate(levelGroup.transform.GetChild(i).gameObject);
        }
    }

    private void ClearFloor()
    {
        var levelGroup = (LevelGroup)target;
        var floorNode = levelGroup.transform.Find("FloorNode");
        var wallNode = levelGroup.transform.Find("WallNode");
        if (floorNode != null)
        {
            DestroyImmediate(floorNode.gameObject);
        }

        if (wallNode != null)
        {
            DestroyImmediate(wallNode.gameObject);
        }
    }

}
