using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Floater : MonoBehaviour
{
    [SerializeField] private float findRange = 6f;
    protected SoulPowerable target;

    protected float minModifier = 7f;
    protected float maxModifier = 11f;

    private Vector3 floaterVelocity = Vector3.zero;
    protected bool isFollowing = false;

    private void Start()
    {
        var objects = GameObject.FindObjectsOfType(typeof(SoulPowerable));
        var powerables = objects
            .Select(ob => ((SoulPowerable)ob))
            .Where(powerable => powerable.AcceptsSouls())
            .Where(powerable => Vector2.Distance(transform.position, powerable.transform.position) < findRange)
            .OrderBy(powerable => Vector2.Distance(transform.position, powerable.transform.position));
        if (powerables.Count() == 0) 
        {
            Destroy(this.gameObject);
            return;
        }
        target = powerables.First();
        if (target != null) startFollow();

    }
    public void startFollow()
    {
        isFollowing = true;
    }

    private void FixedUpdate()
    {
        if (!isFollowing) return;
        transform.position = Vector3.SmoothDamp(transform.position, target.transform.position, ref floaterVelocity, Time.deltaTime * Random.Range(minModifier, maxModifier));
        if (Vector2.Distance(transform.position, target.transform.position) < 0.2f)
        {
            target.addSoul();
            Destroy(this.gameObject);
        }
    }
}
