using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DraggableObject : NetworkBehaviour
{
    private Rigidbody _rigidbody;
    private float _startYPos;
    private BoardController _board;
    //public GameObject board;

    void Start()
    {
        _board = GetComponentInParent<BoardController>();
        //_board = board.GetComponent<BoardController>();
        _rigidbody = GetComponent<Rigidbody>();

        _startYPos = gameObject.transform.position.y;
    }

    private void OnMouseDrag()
    {
		if (isLocalPlayer)
		{
            Vector3 newWorldPosition = new Vector3(_board.CurrentMousePosition.x, _startYPos + 1, _board.CurrentMousePosition.z);

            var difference = newWorldPosition - transform.position;

            var speed = 10 * difference;
            _rigidbody.velocity = speed;
            //TODO: cap the rotation of the card
            _rigidbody.rotation = Quaternion.Euler(new Vector3(speed.z * 0.1f, 0, -speed.x * 0.1f));
		}
    }
}