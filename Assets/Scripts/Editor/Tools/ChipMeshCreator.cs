#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Tools
{
    public class ChipMeshCreator : EditorUtility 
    {
        // [MenuItem("Tools/Add Position (x,z) to selected")]
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

        //[MenuItem("Tools/Create Chip Meshes")]
        public static void CreateChipMeshes()
        {
            var cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            var meshFilter = cylinder.GetComponent<MeshFilter>();
            var cylinderMesh = meshFilter.sharedMesh;

            for (int row = 0; row < 8; row++)
            {
                for (int column = 0; column < 8; column++)
                { 
                    Mesh chipMesh = new Mesh();
                    var vertices = new List<Vector3>();
                    for (int i = 0; i < cylinderMesh.vertices.Length; i++)
                    {
                        var v = cylinderMesh.vertices[i];
                        vertices.Add(new Vector3(v.x, v.y / 100f, v.z));
                    }
                    chipMesh.vertices = vertices.ToArray();

                    int[] triangles = new int[cylinderMesh.triangles.Length];
                    for (int i = 0; i < cylinderMesh.triangles.Length; i++)
                    {
                        triangles[i] = cylinderMesh.triangles[i];
                    }
                    chipMesh.triangles = triangles;

                    var cellOffset = 0.125f;
                    var firstHeadOffset = new Vector2(-.5f, 0);
                    var secondHeadOffset = new Vector2(cellOffset * row, cellOffset * column);
                    Vector2[] uvs = new []{
                        new Vector2(0f, 0f),
                        new Vector2(0.005f, 0f),
                        new Vector2(0f, 0f),
                        new Vector2(0.005f, 0f),
                        new Vector2(0f, 0f),
                        new Vector2(0.005f, 0f),
                        new Vector2(0f, 0f),
                        new Vector2(0.005f, 0f),
                        new Vector2(0f, 0f),
                        new Vector2(0.005f, 0f),
                        new Vector2(0f, 0f),
                        new Vector2(0.005f, 0f),
                        new Vector2(0f, 0f),
                        new Vector2(0.005f, 0f),
                        new Vector2(0f, 0f),
                        new Vector2(0.005f, 0f),
                        new Vector2(0f, 0f),
                        new Vector2(0.005f, 0f),
                        new Vector2(0f, 0f),
                        new Vector2(0.005f, 0f), //20 снизу для ребра
                        new Vector2(0f, 0.005f),
                        new Vector2(0.005f, 0.005f),
                        new Vector2(0f, 0.005f),
                        new Vector2(0.005f, 0.005f),
                        new Vector2(0f, 0.005f),
                        new Vector2(0.005f, 0.005f),
                        new Vector2(0f, 0.005f),
                        new Vector2(0.005f, 0.005f),
                        new Vector2(0f, 0.005f),
                        new Vector2(0.005f, 0.005f),
                        new Vector2(0f, 0.005f),
                        new Vector2(0.005f, 0.005f),
                        new Vector2(0f, 0.005f),
                        new Vector2(0.005f, 0.005f),
                        new Vector2(0f, 0.005f),
                        new Vector2(0.005f, 0.005f),
                        new Vector2(0f, 0.005f),
                        new Vector2(0.005f, 0.005f),
                        new Vector2(0f, 0.005f),
                        new Vector2(0.005f, 0.005f), //20 сверху для ребра
                        
                        secondHeadOffset + (new Vector2(0.75f, .25f) + firstHeadOffset) / 4f, //центр решки //40
                        new Vector2(0.25f, .25f)/4f, //центр орла  //41

                        new Vector2(0.005f, 0f),
                        new Vector2(0.005f, 0.005f),
                        new Vector2(0f, 0f),
                        new Vector2(0f, 0.005f),
                        new Vector2(0.005f, 0f),
                        new Vector2(0.005f, 0.005f), //47

                        //решка:
                        secondHeadOffset + (new Vector2(.55f, .4f)  + firstHeadOffset) / 4f ,
                        secondHeadOffset + (new Vector2(.51f, .33f) + firstHeadOffset) / 4f,
                        secondHeadOffset + (new Vector2(.6f, .46f) + firstHeadOffset) / 4f,
                        secondHeadOffset + (new Vector2(.67f, .49f) + firstHeadOffset) / 4f,
                        secondHeadOffset + (new Vector2(.75f, .5f) + firstHeadOffset) / 4f, //52
                        secondHeadOffset + (new Vector2(.82f, .49f) + firstHeadOffset) / 4f,
                        secondHeadOffset + (new Vector2(.89f, .46f) + firstHeadOffset) / 4f,
                        secondHeadOffset + (new Vector2(.95f, .40f) + firstHeadOffset) / 4f,
                        secondHeadOffset + (new Vector2(.98f, .33f) + firstHeadOffset) / 4f,
                        secondHeadOffset + (new Vector2(1f, .25f) + firstHeadOffset) / 4f, //57
                        secondHeadOffset + (new Vector2(.98f, .18f) + firstHeadOffset) / 4f,
                        secondHeadOffset + (new Vector2(.95f, .11f) + firstHeadOffset) / 4f,
                        secondHeadOffset + (new Vector2(.89f, .06f) + firstHeadOffset) / 4f,
                        secondHeadOffset + (new Vector2(.82f, 0.02f) + firstHeadOffset) / 4f, 
                        secondHeadOffset + (new Vector2(.75f, 0f) + firstHeadOffset) / 4f, //62
                        secondHeadOffset + (new Vector2(.67f, .02f) + firstHeadOffset) / 4f,
                        secondHeadOffset + (new Vector2(.6f, .06f) + firstHeadOffset) / 4f,
                        secondHeadOffset + (new Vector2(.55f, .11f) + firstHeadOffset) / 4f, 
                        secondHeadOffset + (new Vector2(.51f, .18f) + firstHeadOffset) / 4f,
                        secondHeadOffset + (new Vector2(.5f, .25f) + firstHeadOffset) / 4f,  //67
                        //орел:
                        (new Vector2(.48f, .33f))/4f,
                        (new Vector2(.45f, .40f))/4f,
                        (new Vector2(.40f, .45f))/4f,
                        (new Vector2(.33f, .49f))/4f,
                        (new Vector2(.25f, .50f))/4f, //72
                        (new Vector2(.18f, .49f))/4f,
                        (new Vector2(.11f, .45f))/4f,
                        (new Vector2(.05f, .40f))/4f,
                        (new Vector2(.02f, .33f))/4f,
                        (new Vector2(0f, .25f))/4f, //77
                        (new Vector2(.02f, .18f))/4f,
                        (new Vector2(.05f, .11f))/4f,
                        (new Vector2(.11f, .06f))/4f,
                        (new Vector2(.18f, .02f))/4f, 
                        (new Vector2(.25f, 0f))/4f, //82
                        (new Vector2(.33f, .02f))/4f,
                        (new Vector2(.40f, .06f))/4f,
                        (new Vector2(.45f, .11f))/4f, 
                        (new Vector2(.48f, .18f))/4f,
                        (new Vector2(.5f, .25f))/4f,  //87
                        
                    };
                    Debug.Log($"verts.Length - {vertices.Count}");
                    Debug.Log($"uv.Length - {uvs.Length}");

                    chipMesh.uv = uvs;
                    chipMesh.RecalculateNormals();

                    AssetDatabase.CreateAsset(chipMesh, $"Assets/ChipMesh_{row}_{column}.asset");
        
                }
            }

            AssetDatabase.SaveAssets();

            Object.DestroyImmediate(cylinder);
        }

        //[MenuItem("Tools/Create Simple Chip Mesh")]
        public static void CreateSimpleChipMesh()
        {
            var cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            var cylinderMesh = cylinder.GetComponent<MeshFilter>().sharedMesh;

            Mesh chipMesh = new Mesh();
            var duplicates = new Dictionary<int, int>();
            var usedVertices = new List<Vector3>();

            var vertices = new List<Vector3>();
            for (int i = 0; i < cylinderMesh.vertices.Length; i++)
            {
                var v = cylinderMesh.vertices[i];
                if (usedVertices.Contains(v))
                {
                    duplicates.Add(i, usedVertices.IndexOf(v));
                }
                else
                {
                    usedVertices.Add(v);
                    vertices.Add(new Vector3(v.x, v.y / 100f, v.z));
                }
            }
            chipMesh.vertices = vertices.ToArray();

            int[] triangles = new int[cylinderMesh.triangles.Length];
            for (int i = 0; i < cylinderMesh.triangles.Length; i++)
            {
                if (duplicates.TryGetValue(cylinderMesh.triangles[i], out var duplicate))
                {
                    triangles[i] = duplicate;
                    Debug.Log($"{i} - duplicate {duplicate}");
                }
                else
                {
                    triangles[i] = cylinderMesh.triangles[i];
                    Debug.Log($"{i} - {cylinderMesh.triangles[i]}");
                }
            }
            chipMesh.triangles = triangles;

            Debug.Log($"verts.Length - {vertices.Count}");

            chipMesh.RecalculateNormals();

            AssetDatabase.CreateAsset(chipMesh, $"Assets/ChipSimpleMesh.asset");

            Object.DestroyImmediate(cylinder);
        }

    }
}
#endif