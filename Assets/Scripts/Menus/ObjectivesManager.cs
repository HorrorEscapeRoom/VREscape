using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu()]
public class ObjectivesManager : ScriptableObject
{
    private List<Objective> objectives;

    public UnityEvent<List<Objective>> OnValueChanged;
    
    public void ModifyObjective(int id, string title, string text)
    {
        var whatever = objectives.Find(x => x.id == id);
        if (whatever == null)
        {
            return;
        }

        if (title != null)
        {
            whatever.title = title;
        }

        if (text != null)
        {
            whatever.text = text;
        }

        OnValueChanged.Invoke(objectives);
        // obj.title = 
    }

    public void AddObjective(int id, string title, string text)
    {
        Objective objective = objectives.Find(x => x.id == id);
        if ( objective != null )
        {   
            objective = new Objective(id, title, text);
            objectives.Add(objective);
        }
        else
        {
            objective.title = title;
            objective.text = text;
        }

        OnValueChanged.Invoke(objectives);
    }


    public void RemoveObjective(int id)
    {
        Objective objective = objectives.Find(x => x.id == id);

        objectives.Remove(objective);

        OnValueChanged.Invoke(objectives);
    }

    public Objective GetObjective(int id)
    {
        return objectives.Find(x => x.id == id);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
