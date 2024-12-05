using UnityEngine;
using System.Collections;


namespace TMPro.Examples
{
    
    public class TextMeshSpawner : MonoBehaviour
    {

        public int SpawnType = 0;
        public int NumberOfNPC = 12;

        public Font TheFont;

        private TextMeshProFloatingText floatingText_Script;

        void Awake()
        {

        }

        void Start()
        {

            for (int i = 0; i < NumberOfNPC; i++)
            {
                if (SpawnType == 0)
                {
                    // TextMesh Pro Implementation     
                    //Go.transform.localScale = new Vector3(2, 2, 2);
                    GameObject Go = new GameObject(); //"NPC " + i);
                    Go.transform.position = new Vector3(Random.Range(-95f, 95f), 0.5f, Random.Range(-95f, 95f));

                    //Go.transform.position = new Vector3(0, 1.01f, 0);
                    //Go.renderer.castShadows = false;
                    //Go.renderer.receiveShadows = false;
                    //Go.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);

                    TextMeshPro textMeshPro = Go.AddComponent<TextMeshPro>();
                    //textMeshPro.FontAsset = Resources.Load("Fonts & Materials/LiberationSans SDF", typeof(TextMeshProFont)) as TextMeshProFont;
                    //textMeshPro.anchor = AnchorPositions.Bottom;
                    textMeshPro.fontSize = 96;

                    textMeshPro.text = "!";
                    textMeshPro.color = new Color32(255, 255, 0, 255);
                    //textMeshPro.Text = "!";


                    // Spawn Floating Text
                    floatingText_Script = Go.AddComponent<TextMeshProFloatingText>();
                    floatingText_Script.SpawnType = 0;
                }
                else
                {
                    // TextMesh Implementation
                    GameObject Go = new GameObject(); //"NPC " + i);
                    Go.transform.position = new Vector3(Random.Range(-95f, 95f), 0.5f, Random.Range(-95f, 95f));

                    //Go.transform.position = new Vector3(0, 1.01f, 0);

                    TextMesh textMesh = Go.AddComponent<TextMesh>();
                    textMesh.GetComponent<Renderer>().sharedMaterial = TheFont.material;
                    textMesh.font = TheFont;
                    textMesh.anchor = TextAnchor.LowerCenter;
                    textMesh.fontSize = 96;

                    textMesh.color = new Color32(255, 255, 0, 255);
                    textMesh.text = "!";

                    // Spawn Floating Text
                    floatingText_Script = Go.AddComponent<TextMeshProFloatingText>();
                    floatingText_Script.SpawnType = 1;
                }
            }
        }

    }
}
