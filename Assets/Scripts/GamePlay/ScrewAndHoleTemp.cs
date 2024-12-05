using System.Collections.Generic;
using UnityEngine;

class ScrewAndHoleTemp : MonoBehaviour
{
    public List<Plate> GetAllPlate ()
    {
        List<Plate> plates = new();
        foreach(var coll in Physics2D.OverlapPointAll(transform.position))
        {
            if(coll.gameObject.TryGetComponent(out Plate plate))
                plates.Add(plate);
        }
        return plates;
    }
}