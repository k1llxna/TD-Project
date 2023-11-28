using UnityEngine;

public class CameraController : MonoBehaviour
{
    // pan speed (on plane)
    public float panSpeed = 30f;
    public float panBoarderThickness = 10f;
    public float scrollSpeed = 5f;

    public float minY = 10f;
    public float maxY = 80f;

    private bool doMovement = true;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown (KeyCode.Escape))
        {
            doMovement = !doMovement;
        }

        if (!doMovement)
            return;

        if (Input.GetKey("w"))
        {
            transform.Translate(Vector3.forward * panSpeed * Time.deltaTime, Space.World); // space world ignore rotation of object
        }
        if (Input.GetKey("s"))
        {
            transform.Translate(Vector3.back * panSpeed * Time.deltaTime, Space.World); // space world ignore rotation of object
        }
        if (Input.GetKey("d"))
        {
            transform.Translate(Vector3.right * panSpeed * Time.deltaTime, Space.World); // space world ignore rotation of object
        }
        if (Input.GetKey("a"))
        {
            transform.Translate(Vector3.left * panSpeed * Time.deltaTime, Space.World); // space world ignore rotation of object
        }

        //if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - panBoarderThickness)
        //{
        //    transform.Translate(Vector3.forward * panSpeed * Time.deltaTime, Space.World); // space world ignore rotation of object
        //}
        //if (Input.GetKey("s") || Input.mousePosition.y <= panBoarderThickness)
        //{
        //    transform.Translate(Vector3.back * panSpeed * Time.deltaTime, Space.World); // space world ignore rotation of object
        //}
        //if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panBoarderThickness)
        //{
        //    transform.Translate(Vector3.right * panSpeed * Time.deltaTime, Space.World); // space world ignore rotation of object
        //}
        //if (Input.GetKey("a") || Input.mousePosition.x <= panBoarderThickness)
        //{
        //    transform.Translate(Vector3.left * panSpeed * Time.deltaTime, Space.World); // space world ignore rotation of object
        //}

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        Vector3 pos = transform.position;
        pos.y -= scroll * 1000 * scrollSpeed * Time.deltaTime;
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        transform.position = pos;
    }
}
