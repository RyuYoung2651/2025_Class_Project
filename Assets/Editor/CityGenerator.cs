using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Dynamic;

public class CityGenerator : EditorWindow
{
    private int gridSizeX = 10;             //도시 가로
    private int gridSizeZ = 10;             //도시 세로
    private float buildingSpacing = 15;     //건물 간격
    private float roadWidth = 5f;           //도로 폭
    private bool makeStatic = true;         //생성되는 오브젝트를 Static으로 만들지 여부

    [MenuItem("Tools/City Cenerator")]   //유니티 상단 메뉴에 버튼 추가

    public static void ShowWindow()
    {
        GetWindow<CityGenerator>("City Generator");     //에디터 창 열기
    }


    private void OnGUI() // 에디터 창 그리기
    {
        GUILayout.Label("Simple City Generator", EditorStyles.boldLabel); // 제목 표시

        gridSizeX = EditorGUILayout.IntField("Grid Size X", gridSizeX); // X크기 입력
        gridSizeZ = EditorGUILayout.IntField("Grid Size Z", gridSizeZ); // Z크기 입력
        buildingSpacing = EditorGUILayout.FloatField("Building Spacing", buildingSpacing);
        roadWidth = EditorGUILayout.FloatField("Road Width", roadWidth); // 도로 폭 입력
        makeStatic = EditorGUILayout.Toggle("Make Static", makeStatic); // Static 설정

        GUILayout.Space(10);

        if (GUILayout.Button("Generate City")) // 도시 생성 버튼
        {
            GenerateCity();
        }

        if (GUILayout.Button("Clear City")) // 도시 삭제 버튼
        {
            ClearCity();
        }

       
    }

    private void GenerateCity()             //도시 생성함수
    {
        GameObject cityParent = new GameObject("City");                 //전체 도시를담은 부모 프로젝트

        GameObject bulidingsParent = new GameObject("Buildings");       //건물 묶은 부모
        bulidingsParent.transform.SetParent(cityParent.transform, false);

        GameObject roadsParent = new GameObject("Roads");       //도로 묶은 부모
        bulidingsParent.transform.SetParent(cityParent.transform, false);


        for (int x = 0; x < gridSizeX; x++)         //x방향 반복
        {
            for (int z = 0; z< gridSizeZ; z++)      //z방향 반복
            {
                Vector3 position = new Vector3(x * buildingSpacing, 0, z * buildingSpacing); //각위치 계산

                if(x % 2 == 0 || z % 2 == 0)        //짝수 중에는 도로 생성
                {
                    CreateRoad(position, roadsParent.transform);
                }
                else
                {
                    CreateBuilding(position, bulidingsParent.transform);
                }

            }
        }
    }




    private void CreateBuilding(Vector3 position, Transform parent)
    {
        GameObject building = GameObject.CreatePrimitive(PrimitiveType.Cube);
        building.name = "Building";     //오브젝트 이름 설정

        float height = Random.Range(5.0f, 20.0f); //랜던 건물 높이
        building.transform.position = position + Vector3.up * height / 2.0f;
        building.transform.localScale = new Vector3(buildingSpacing - roadWidth -1f, height ,buildingSpacing - roadWidth -1f);
        building.transform.SetParent(parent);  //Buildings 그룹 아래로 넣기 

        Renderer renderer = building.GetComponent<Renderer>();   //색변경을 위한 렌더러
        renderer.material.color = new Color(Random.Range(0.5f, 0.8f), Random.Range(0.5f, 0.8f), Random.Range(0.5f, 0.8f));

        if(makeStatic)
        {
            building.isStatic = true;
        }
    }

    private void CreateRoad(Vector3 position, Transform parent)
    {
        GameObject road = GameObject.CreatePrimitive(PrimitiveType.Cube);

        road.transform.position = position + Vector3.up * 0.1f; // 살짝 바닥 위에 두기
        road.transform.localScale = new Vector3(buildingSpacing, 0.2f, buildingSpacing);
        road.transform.SetParent(parent); // Roads 그룹 아래로 넣기

        Renderer renderer = road.GetComponent<Renderer>(); // 색 변경을 위한 Renderer
        renderer.material.color = new Color(0.3f, 0.3f, 0.3f); // 회색 색상

        if (makeStatic) // Static 설정
        {
            road.isStatic = true; // 도로 Static 처리
        }
    }
    
    private void ClearCity() // 도시 삭제 함수
    {
        GameObject city = GameObject.Find("City"); // City 오브젝트 찾기
        if (city != null)
        {
            DestroyImmediate(city); // 에디터에서 즉시 삭제
            Debug.Log("City cleared");
        }
        else
        {
            Debug.Log("도시가 없습니다.");
        }
    }

}
