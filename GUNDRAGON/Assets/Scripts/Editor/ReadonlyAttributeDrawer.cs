using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ReadonlyAttribute))]
public class ReadonlyAttributeDrawGer : DecoratorDrawer
{
    public override void OnGUI(Rect position)
    {
        base.OnGUI(position);

        GUI.enabled = false;
    }

    public override float GetHeight()
    {
        return 0F;
    }
}
