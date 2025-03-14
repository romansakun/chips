#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Tools
{
    public class ChipMeshCreator : EditorUtility 
    {
        [MenuItem("Tools/Add Position (x,z) to selected")]
        public static void AddPositionsToNames() 
        {
            GameObject[] selected = Selection.gameObjects;
            if (selected.Length == 0) {
                Debug.LogError("Нет выделенных объектов!");
                return;
            }

            foreach (var obj in selected)
            {
                var pos = obj.transform.position;
                string newName = obj.name + " - " + new Vector2(pos.x, pos.z).ToString();
                obj.name = newName;
            }
        }
        
        [MenuItem("Tools/Create Chip Mesh")]
        public static void CreateChipMesh()
        {
            var cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            var meshFilter = cylinder.GetComponent<MeshFilter>();
            var cylinderMesh = meshFilter.sharedMesh;


            // for (var i = 0; i < cylinderMesh.vertices.Length; i++)
            // {
            //     var vertex = cylinderMesh.vertices[i];
            //     Debug.Log($"vertex_{i} - {vertex}");
            // }
            //
            // for (var i = 0; i < cylinderMesh.uv2.Length; i++)
            // {
            //     var uv = cylinderMesh.uv2[i];
            //     Debug.Log($"uv_{i} - {uv}");
            //     //if (i >= 40)
            //     {
            //         var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            //         go.name = $"{i} - {uv}";
            //         go.transform.position = new Vector3(uv.x, 0, uv.y);
            //         go.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            //     }
            // }
            //
            // for (var i = 0; i < cylinderMesh.triangles.Length; i++)
            // {
            //     var uv = cylinderMesh.triangles[i];
            //     Debug.Log($"triangles_{i} - {uv}");
            // }
            //
            // for (var i = 0; i < cylinderMesh.normals.Length; i++)
            // {
            //     var uv = cylinderMesh.normals[i];
            //     Debug.Log($"normal_{i} - {uv}");
            // }

            Mesh chipMesh = new Mesh();
            // Вершины
            var vertices = new List<Vector3>();
            for (int i = 0; i < cylinderMesh.vertices.Length; i++)
            {
                var v = cylinderMesh.vertices[i];
                vertices.Add(new Vector3(v.x, v.y / 100f, v.z));
            }
            chipMesh.vertices = vertices.ToArray();
            
            // Треугольники (индексы вершин) - прокатит как есть
            int[] triangles = new int[cylinderMesh.triangles.Length];
            for (int i = 0; i < cylinderMesh.triangles.Length; i++)
            {
                triangles[i] = cylinderMesh.triangles[i];
            }
            chipMesh.triangles = triangles;

            Vector2[] uvs = new []{
                new Vector2(0f, 0f),
                new Vector2(0.05f, 0f),
                new Vector2(0f, 0f),
                new Vector2(0.05f, 0f),
                new Vector2(0f, 0f),
                new Vector2(0.05f, 0f),
                new Vector2(0f, 0f),
                new Vector2(0.05f, 0f),
                new Vector2(0f, 0f),
                new Vector2(0.05f, 0f),
                new Vector2(0f, 0f),
                new Vector2(0.05f, 0f),
                new Vector2(0f, 0f),
                new Vector2(0.05f, 0f),
                new Vector2(0f, 0f),
                new Vector2(0.05f, 0f),
                new Vector2(0f, 0f),
                new Vector2(0.05f, 0f),
                new Vector2(0f, 0f),
                new Vector2(0.05f, 0f), //20 снизу для ребра
                new Vector2(0f, 0.05f),
                new Vector2(0.05f, .05f),
                new Vector2(0f, 0.05f),
                new Vector2(0.05f, .05f),
                new Vector2(0f, 0.05f),
                new Vector2(0.05f, .05f),
                new Vector2(0f, 0.05f),
                new Vector2(0.05f, .05f),
                new Vector2(0f, 0.05f),
                new Vector2(0.05f, .05f),
                new Vector2(0f, 0.05f),
                new Vector2(0.05f, .05f),
                new Vector2(0f, 0.05f),
                new Vector2(0.05f, .05f),
                new Vector2(0f, 0.05f),
                new Vector2(0.05f, .05f),
                new Vector2(0f, 0.05f),
                new Vector2(0.05f, .05f),
                new Vector2(0f, 0.05f),
                new Vector2(0.05f, .05f), //20 сверху для ребра
                new Vector2(0.75f, .25f), //центр решки //40
                new Vector2(0.25f, .25f), //центр орла  //41
                //дальше хренота, надо аккуратно по каждой позиции..
                new Vector2(0.05f, 0f),
                new Vector2(0.05f, .05f),
                new Vector2(0f, 0f),
                new Vector2(0f, .05f),
                new Vector2(.05f, 0f),
                new Vector2(.05f, .05f), //47
                //решка:
                new Vector2(.55f, .4f),
                new Vector2(.51f, .33f),
                new Vector2(.6f, .46f),
                new Vector2(.67f, .49f),
                new Vector2(.75f, .5f), //52
                new Vector2(.82f, .49f),
                new Vector2(.89f, .46f),
                new Vector2(.95f, .40f),
                new Vector2(.98f, .33f),
                new Vector2(1f, .25f), //57
                new Vector2(.98f, .18f),
                new Vector2(.95f, .11f),
                new Vector2(.89f, .06f),
                new Vector2(.82f, 0.02f), 
                new Vector2(.75f, 0f), //62
                new Vector2(.67f, .02f),
                new Vector2(.6f, .06f),
                new Vector2(.55f, .11f), 
                new Vector2(.51f, .18f),
                new Vector2(.5f, .25f),  //67
                new Vector2(.48f, .33f),
                new Vector2(.45f, .40f),
                new Vector2(.40f, .45f),
                new Vector2(.33f, .49f),
                new Vector2(.25f, .50f), //72
                new Vector2(.18f, .49f),
                new Vector2(.11f, .45f),
                new Vector2(.05f, .40f),
                new Vector2(.02f, .33f),
                new Vector2(0f, .25f), //77
                new Vector2(.02f, .18f),
                new Vector2(.05f, .11f),
                new Vector2(.11f, .06f),
                new Vector2(.18f, .02f), 
                new Vector2(.25f, 0f), //82
                new Vector2(.33f, .02f),
                new Vector2(.40f, .06f),
                new Vector2(.45f, .11f), 
                new Vector2(.48f, .18f),
                new Vector2(.5f, .25f),  //87
            };
            
            Debug.Log($"verts.Length - {vertices.Count}");
            Debug.Log($"uv.Length - {uvs.Length}");
            
            // UV-координаты
            chipMesh.uv = uvs;

            // UV-координаты - надо увеличить орел и решку, а ребро сжать до нескольких пикселей
            //chipMesh.uv = cylinderMesh.uv2; 
            // Нормали (опционально) - вроде норм и так
            
            chipMesh.RecalculateNormals();

            // for (var i = 0; i < chipMesh.uv.Length; i++)
            // {
            //     var uv = chipMesh.uv[i];
            //     Debug.Log($"uv_{i} - {uv}");
            // }

            for (var i = 0; i < chipMesh.triangles.Length; i++)
            {
                var uv = chipMesh.triangles[i];
                Debug.Log($"triangles_{i} - {uv}");
            }

            for (var i = 0; i < chipMesh.normals.Length; i++)
            {
                var uv = chipMesh.normals[i];
                Debug.Log($"normal_{i} - {uv}");
            }
            
            // Сохранение mesh как ассета
            AssetDatabase.CreateAsset(chipMesh, "Assets/ChipMesh.asset");
            AssetDatabase.SaveAssets();
            
            GameObject.DestroyImmediate(cylinder);
        }
    }
}
#endif