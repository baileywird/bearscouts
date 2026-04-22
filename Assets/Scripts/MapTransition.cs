using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using UnityEngine;

public class MapTransition : MonoBehaviour
{
    [SerializeField] PolygonCollider2D mapBoundry;
    [SerializeField] bool isDefault;
    CinemachineConfiner confiner;
    [SerializeField] Direction direction;
    [SerializeField] float additivePos = 1f;

    private static bool isTransitioning = false;

    enum Direction { Up, Down, Left, Right }

    private void Start()
    {
        if(isDefault)
        {
            Debug.Log("Setting default boundary: " + mapBoundry.name + " confiner: " + confiner);
            confiner.m_BoundingShape2D = mapBoundry;
            confiner.InvalidatePathCache();

            confiner.enabled = false;
            confiner.enabled = true;
        }
    }

    private void Awake()
    {
        confiner = FindObjectOfType<CinemachineConfiner>();
    }
    
    IEnumerator ResetTransition()
    {
        yield return new WaitForSeconds(0.2f);
        isTransitioning = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isTransitioning) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            isTransitioning = true;

            confiner.m_BoundingShape2D = mapBoundry;
            UpdatePlayerPosition(collision.gameObject);

            Debug.Log("Setting new boundary: " + mapBoundry.name);

            StartCoroutine(ResetTransition());
        }
    }

    private void UpdatePlayerPosition(GameObject player)
    {
        Vector3 newPos = player.transform.position;

        switch (direction)
        {
            case Direction.Up:
                newPos.y += additivePos;
                break;
            case Direction.Down:
                newPos.y -= additivePos;
                break;
            case Direction.Left:
                newPos.x += additivePos;
                break;
            case Direction.Right:
                newPos.x -= additivePos;
                break;
        }

        player.transform.position = newPos;
    }
}
