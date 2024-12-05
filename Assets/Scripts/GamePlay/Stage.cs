using System.Collections.Generic;
using UnityEngine;

class Stage : MonoBehaviour
{
    public bool isHard;
    private SpriteRenderer bg;
    private List<Screw> screws;
    private HashSet<Plate> plates;

    public bool IsClear => plates.Count == 0;

    public void Awake()
    {
        screws = new();
        plates = new();
        gameObject.name = "Board";
        bg = GetComponent<SpriteRenderer>();
    }
    public void CreateScrewAndHole(in Screw screwPrefab, in Hole boardHolePrefab, in Hole plateHolePrefab)
    {
        foreach (var temp in GetComponentsInChildren<ScrewAndHoleTemp>())
        {
            Vector3 pos = temp.transform.position;
            CreateBoardHole(boardHolePrefab, pos);
            Screw s = CreateScrew(screwPrefab, pos);
            foreach (Plate p in temp.GetAllPlate())
            {
                Hole plateHole = CreatePlateHole(plateHolePrefab, pos);
                plateHole.SetParent(p);
                s.Connect(p);
            }
            screws.Add(s);
            temp.gameObject.SetActive(false);
        }
        foreach (var plate in GetComponentsInChildren<Plate>())
            plates.Add(plate);
    }
    public Screw CreateScrew(in Screw screwPrefab, Vector3 pos)
    {
        Screw s = Instantiate(screwPrefab);
        s.gameObject.name = "Screw";
        s.transform.SetParent(transform, false);
        s.transform.position = pos;
        return s;
    }
    public Hole CreateBoardHole(in Hole boardHolePrefab, Vector3 pos)
    {
        Hole h = Instantiate(boardHolePrefab);
        h.gameObject.name = "Hole";
        h.transform.SetParent(transform, false);
        h.transform.position = pos;
        return h;
    }
    public Hole CreatePlateHole(in Hole plateHolePrefab, Vector3 pos)
    {
        Hole h = Instantiate(plateHolePrefab);
        h.gameObject.name = "PlateHole";
        h.transform.position = pos;
        return h;
    }
    public void SetBackgroundSprite(Sprite sprite) => bg.sprite = sprite;
    public void SetScrewSprite(Sprite s1, Sprite s2)
    {
        foreach (Screw s in screws)
            s.SetSkin(s1, s2);
    }
    public bool RemovePlate(Plate plate)
    {
        if (!plates.Contains(plate))
            return false;
        plates.Remove(plate);
        StartCoroutine(plate.Disapear());
        return true;
    }
    public void RemoveScrew(Screw screw) => StartCoroutine(screw.Disapear());
}