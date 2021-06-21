using UnityEngine;
using System.Collections;

public class UpdatableData : ScriptableObject
{

    public event System.Action OnValuesUpdated;
    public bool autoUpdate;

    // Pre processor directive
#if UNITY_EDITOR

    // OnValidate method
    // Update when a value is changed in the inspector or when scripts compile
    protected virtual void OnValidate()
    {
        if (autoUpdate)
        {
            UnityEditor.EditorApplication.update += NotifyOfUpdatedValues;
        }
    }

    // NotifyOfUpdatedValues method
    public void NotifyOfUpdatedValues()
    {
        UnityEditor.EditorApplication.update -= NotifyOfUpdatedValues;
        if (OnValuesUpdated != null)
        {
            OnValuesUpdated();
        }
    }

#endif
}
