using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeScan : MonoBehaviour
{
    // Tag to compare against
    public string subjectTag;

    // Private list to store GameObjects in range
    private List<GameObject> _inRange = new List<GameObject>();

    // Public getter and setter for the list with a custom getter to clean up null entries
    public List<GameObject> inRange
    {
        get
        {
            // Clean up null entries from the list
            for (int i = _inRange.Count - 1; i >= 0; i--)
            {
                if (_inRange[i] == null)
                {
                    _inRange.RemoveAt(i);
                }
            }
            return _inRange;
        }
        private set { }
    }

    // OnTriggerEnter2D is called when another collider enters the trigger collider
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(subjectTag) && !_inRange.Contains(collision.gameObject))
        {
            // Add the GameObject to the list if it matches the specified tag
            _inRange.Add(collision.gameObject);
            Debug.Log($"Added to inRange: {collision.gameObject.name}");
        }
    }

    // OnTriggerExit2D is called when another collider exits the trigger collider
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(subjectTag))
        {
            // Remove the GameObject from the list when it exits the trigger
            _inRange.Remove(collision.gameObject);
            Debug.Log($"Removed from inRange: {collision.gameObject.name}");
        }
    }
}
