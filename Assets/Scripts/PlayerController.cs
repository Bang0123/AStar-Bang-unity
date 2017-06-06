using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Text countText;
    public Text WinText;

    private Rigidbody _rb;
    private float _jump = 20;
    public float _speed;
    private int _count;

    public void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _count = 0;
        SetCountText();
        WinText.text = "";
    }

    public void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        bool jump = Input.GetKeyDown(KeyCode.Space);


        Vector3 movement = new Vector3(moveHorizontal, jump ? _jump : 0.0f, moveVertical);

        _rb.AddForce(movement * _speed);

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pick Up"))
        {
            other.gameObject.SetActive(false);
            _count++;
            SetCountText();
        }
    }

    private void SetCountText()
    {
        countText.text = "Count: " + _count;
        if (_count >= 12)
        {
            WinText.text = "You Win!!!!!";
        }
    }

}