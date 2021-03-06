using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Field attributes: position, type
/// </summary>
[System.Serializable]
public class FieldData : IData
{
    public int[] position;
    public string fieldType;

    public FieldData(GameObject field)
    {
        position = new int[2];
        position[0] = (int)field.transform.position.x;
        position[1] = (int)field.transform.position.y;

        FieldManager.FieldType typeEnum = (FieldManager.FieldType)FieldManager.GetFieldType(field);
        fieldType = typeEnum.ToString();
    }

    public override void CreateObject()
    {
        FieldManager.FieldType type = (FieldManager.FieldType)System.Enum.Parse(typeof(FieldManager.FieldType), fieldType);

        FieldManager.SetField(position[0], position[1], type);
    }
}
