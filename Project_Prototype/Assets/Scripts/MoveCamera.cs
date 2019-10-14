using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public GameObject trackLocationGroup;
    private List<Transform> trackLocations = new List<Transform>();

    public float speed = 0.5f;
    private int trackIndex = 0;
    private float lerpTimer = 0.0f;
    private bool startMoving = false;
    private bool hasReachedEnd = false;

    private void Awake()
    {
        // Adding the track locations to the list.
        for (int i = 0; i < trackLocationGroup.transform.childCount; ++i)
            trackLocations.Add(trackLocationGroup.transform.GetChild(i).transform);
    }

    // Update is called once per frame
    private void Update()
    {
        // Only moving if we have the track locations, and the track index is in bounds.
        if (startMoving)
        {
            // Increasing the timer for the lerp.
            lerpTimer += speed * Time.deltaTime;

            // Lerping the camera position.
            Vector3 pos = this.gameObject.transform.position;
            Quaternion rot = this.gameObject.transform.rotation;
            this.gameObject.transform.position = Vector3.Lerp(pos, trackLocations[trackIndex].position, (lerpTimer / 1.0f) * Time.deltaTime);
            this.gameObject.transform.rotation = Quaternion.Lerp(rot, trackLocations[trackIndex].rotation, (lerpTimer / 1.0f) * Time.deltaTime);

            // Increasing the track index.
            if ((pos - trackLocations[trackIndex].position).magnitude < 1.0f)
            {
                ++trackIndex;
                if (trackIndex >= trackLocations.Count)
                {
                    startMoving = false;
                    trackIndex = 0;
                    lerpTimer = 0.0f;
                    hasReachedEnd = true;
                }
            }
        }
    }

    // Has the camera reached the end of the tracks?
    public bool HasReachedEnd
    {
        get
        {
            return hasReachedEnd;
        }
    }


    // Set this to true, to start the camera move.
    public bool StartCameraMove
    {
        get
        {
            return startMoving;
        }
        set
        {
            startMoving = value;
        }
    }

    // Resets the camera to the start.
    public void ResetCameraToStart()
    {
        this.gameObject.transform.position = trackLocations[0].position;
        this.gameObject.transform.rotation = trackLocations[0].rotation;
    }
}
