using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour
{

    public GameObject terrain;

    // object to access heightmap
    public TerrainScript t;

    public float cameraSpeed;

    public float cameraRotationSpeed;

    public float cameraHeight;

    // a buffer distance that prevents the camera from accidentally going under the terrain
    public int buffer;

    // Use this for initialization
    void Start()
    {
        t = terrain.GetComponent<TerrainScript>();
        transform.position = new Vector3(0, cameraHeight, 0);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPosition = transform.position;
        // camera position
        if (Input.GetKey(KeyCode.A))
        {
            newPosition -= transform.rotation * new Vector3(cameraSpeed, 0, 0);
        }
        if (Input.GetKey(KeyCode.W))
        {
            newPosition += transform.rotation * new Vector3(0, 0, cameraSpeed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            newPosition -= transform.rotation * new Vector3(0, 0, cameraSpeed);
        }
        if (Input.GetKey(KeyCode.D))
        {
            newPosition += transform.rotation * new Vector3(cameraSpeed, 0, 0);
        }

        // only allow camera to move to that position if it is in bounds of the world
        if (CheckBounds(newPosition))
        {
            transform.position = newPosition;
        }

        // camera roll
        if (Input.GetKey(KeyCode.Q))
        {
            transform.rotation *= Quaternion.AngleAxis(cameraSpeed, Vector3.forward);
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.rotation *= Quaternion.AngleAxis(-cameraSpeed, Vector3.forward);
        }

        // camera pitch
        transform.rotation *= Quaternion.AngleAxis(cameraRotationSpeed * Input.GetAxis("Mouse Y"), Vector3.left);

        // camera yaw
        transform.rotation *= Quaternion.AngleAxis(cameraRotationSpeed * Input.GetAxis("Mouse X"), Vector3.up);
    }

    bool CheckBounds(Vector3 pos)
    {
        float x = pos.x;
        float y = pos.y;
        float z = pos.z;
        float max = Mathf.Sqrt(t.heights.Length);
        if (x > max || x < 0)
        {
            return false;
        }
        if (z > max || z < 0)
        {
            return false;
        }
        if (y < t.heights[(int)x, (int)z] + buffer)
        {
            return false;
        }
        return true;
    }
}