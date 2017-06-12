using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour {

    public Vector3 target { get; set; }
    public Vector3 initial { get; set; }

	// Use this for initialization
	void Start () {

	}

    public void Go(Vector3 _target, Vector3 _initial)
    {
        target = _target; initial = _initial;
        target.Set(target.x, target.y, 100); initial.Set(initial.x, initial.y, 100);
        transform.SetParent(GameObject.Find("Background Game").transform);
        transform.SetAsLastSibling();
    }
	
	// Update is called once per frame
	void Update () {
        transform.position += (target-initial)*2*Time.deltaTime;
        if ( Mathf.Abs((transform.position-initial).x/(target-initial).x) > 1f || Mathf.Abs((transform.position - initial).y / (target - initial).y) > 1) Destroy(gameObject);
    }
}
