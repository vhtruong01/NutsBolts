using UnityEngine;
using System.Collections;


namespace TMPro.Examples
{

    public class Benchmark02 : MonoBehaviour
    {

        public int SpawnType = 0;
        public int NumberOfNPC = 12;

        public bool IsTextObjectScaleStatic;
        private TextMeshProFloatingText floatingText_Script;


        void Start()
        {

            for (int i = 0; i < NumberOfNPC; i++)
            {


                if (SpawnType == 0)
                {
                    // TextMesh Pro Implementation
                    GameObject Go = new GameObject();
                    Go.transform.position = new Vector3(Random.Range(-95f, 95f), 0.25f, Random.Range(-95f, 95f));

                    TextMeshPro textMeshPro = Go.AddComponent<TextMeshPro>();

                    textMeshPro.autoSizeTextContainer = true;
                    textMeshPro.rectTransform.pivot = new Vector2(0.5f, 0);

                    textMeshPro.alignment = TextAlignmentOptions.Bottom;
                    textMeshPro.fontSize = 96;
                    textMeshPro.enableKerning = false;

                    textMeshPro.color = new Color32(255, 255, 0, 255);
                    textMeshPro.text = "!";
                    textMeshPro.isTextObjectScaleStatic = IsTextObjectScaleStatic;

                    // Spawn Floating Text
                    floatingText_Script = Go.AddComponent<TextMeshProFloatingText>();
                    floatingText_Script.SpawnType = 0;
                    floatingText_Script.IsTextObjectScaleStatic = IsTextObjectScaleStatic;
                }
                else if (SpawnType == 1)
                {
                    // TextMesh Implementation
                    GameObject Go = new GameObject();
                    Go.transform.position = new Vector3(Random.Range(-95f, 95f), 0.25f, Random.Range(-95f, 95f));

                    TextMesh textMesh = Go.AddComponent<TextMesh>();
                    textMesh.font = Resources.Load<Font>("Fonts/ARIAL");
                    textMesh.GetComponent<Renderer>().sharedMaterial = textMesh.font.material;

                    textMesh.anchor = TextAnchor.LowerCenter;
                    textMesh.fontSize = 96;

                    textMesh.color = new Color32(255, 255, 0, 255);
                    textMesh.text = "!";

                    // Spawn Floating Text
                    floatingText_Script = Go.AddComponent<TextMeshProFloatingText>();
                    floatingText_Script.SpawnType = 1;
                }
                else if (SpawnType == 2)
                {
                    // Canvas WorldSpace Camera
                    GameObject Go = new GameObject();
                    Canvas canvas = Go.AddComponent<Canvas>();
                    canvas.worldCamera = Camera.main;

                    Go.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                    Go.transform.position = new Vector3(Random.Range(-95f, 95f), 5f, Random.Range(-95f, 95f));

                    TextMeshProUGUI textObject = new GameObject().AddComponent<TextMeshProUGUI>();
                    textObject.rectTransform.SetParent(Go.transform, false);

                    textObject.color = new Color32(255, 255, 0, 255);
                    textObject.alignment = TextAlignmentOptions.Bottom;
                    textObject.fontSize = 96;
                    textObject.text = "!";

                    // Spawn Floating Text
                    floatingText_Script = Go.AddComponent<TextMeshProFloatingText>();
                    floatingText_Script.SpawnType = 0;
                }



            }
        }
    }
}
