using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FieldOutline : MonoBehaviour
{
    // array not dynamic
    public static readonly FieldManager.FieldType[] TypesWithOutlines = {
        FieldManager.FieldType.WALL_FIELD,
        FieldManager.FieldType.GRAY_KEY_DOOR_FIELD,
        FieldManager.FieldType.RED_KEY_DOOR_FIELD,
        FieldManager.FieldType.BLUE_KEY_DOOR_FIELD,
        FieldManager.FieldType.GREEN_KEY_DOOR_FIELD,
        FieldManager.FieldType.YELLOW_KEY_DOOR_FIELD
    };

    public Color color = Color.black;
    public bool imitateAlpha = false;
    public float weight = 0.1f;
    public int order = 2;
    [Space]
    public string[] connectTags;
    public float rayLength = 1f;

    private readonly Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
    [HideInInspector] public bool updateOnStart = true;
    private GameObject lineContainer;
    private SpriteRenderer spriteRenderer;
    private bool hasSpriteRenderer = false;

    private void Awake()
    {
        lineContainer = new("LineContainer");
        lineContainer.transform.parent = transform;

        if (connectTags.Length == 0) connectTags = new string[] { transform.tag };

        if (TryGetComponent(out spriteRenderer)) hasSpriteRenderer = true;
    }

    private void Start()
    {
        UpdateAlpha();

        if (updateOnStart) UpdateOutline(true);
    }

    private void Update()
    {
        UpdateAlpha();
    }

    // TODO: code duplication
    public void UpdateOutline(bool updateAround = false)
    {
        if (Dbg.Instance.dbgEnabled && !Dbg.Instance.wallOutlines) return;

        foreach (Transform child in lineContainer.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Vector2 d in directions)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, d, rayLength);
            if (Dbg.Instance.drawRays) Debug.DrawRay(transform.position, d, Color.red, 20);

            bool there = false;

            // check if there are objects in that direction
            foreach (RaycastHit2D r in hits)
            {
                if(updateAround && r.collider.gameObject.GetComponent<FieldOutline>() != null)
                {
                    r.collider.gameObject.GetComponent<FieldOutline>().UpdateOutline(false);
                }

                if (connectTags.Contains(r.collider.tag))
                {
                    there = true;
                    break;
                }
            }

            if(!there)
            {
                if (d.Equals(Vector2.up) || d.Equals(Vector2.down))
                {
                    LineManager.SetWeight(weight);
                    LineManager.SetFill(color);
                    LineManager.DrawLine(transform.localPosition.x - transform.localScale.x / 2, transform.localPosition.y + (d.y / 2), transform.localPosition.x + transform.localScale.x / 2, transform.localPosition.y + (d.y / 2), order, lineContainer.transform);
                }
                else if (d.Equals(Vector2.left) || d.Equals(Vector2.right))
                {
                    LineManager.SetWeight(weight);
                    LineManager.SetFill(color);
                    LineManager.DrawLine(transform.localPosition.x + d.x / 2, transform.localPosition.y + transform.localScale.y / 2, transform.localPosition.x + d.x / 2, transform.localPosition.y - transform.localScale.y / 2, order, lineContainer.transform);
                }
            } 
        }
    }

    public void UpdateOutline(Vector2 direction, bool update = false)
    {
        if (Dbg.Instance.dbgEnabled && !Dbg.Instance.wallOutlines) return;

        foreach (Transform child in lineContainer.transform)
        {
            if((Vector2)child.gameObject.transform.localPosition == direction) Destroy(child.gameObject);
        }

        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, rayLength);
        if (Dbg.Instance.drawRays) Debug.DrawRay(transform.position, direction, Color.red, 20);

        bool there = false;

        // check if there are objects in that direction
        foreach (RaycastHit2D r in hits)
        {
            if (update && r.collider.gameObject.GetComponent<FieldOutline>() != null)
            {
                r.collider.gameObject.GetComponent<FieldOutline>().UpdateOutline(false);
            }

            if (connectTags.Contains(r.collider.tag))
            {
                there = true;
                break;
            }
        }

        if(!there)
        {
            if (direction.Equals(Vector2.up) || direction.Equals(Vector2.down))
            {
                LineManager.SetWeight(weight);
                LineManager.SetFill(color);
                LineManager.DrawLine(transform.localPosition.x - transform.localScale.x / 2, transform.localPosition.y + (direction.y / 2), transform.localPosition.x + transform.localScale.x / 2, transform.localPosition.y + (direction.y / 2), order, lineContainer.transform);
            }
            else if (direction.Equals(Vector2.left) || direction.Equals(Vector2.right))
            {
                LineManager.SetWeight(weight);
                LineManager.SetFill(color);
                LineManager.DrawLine(transform.localPosition.x + direction.x / 2, transform.localPosition.y + transform.localScale.y / 2, transform.localPosition.x + direction.x / 2, transform.localPosition.y - transform.localScale.y / 2, order, lineContainer.transform);
            }
        }
    }

    public void UpdateAlpha()
    {
        if (imitateAlpha && hasSpriteRenderer && spriteRenderer.color.a != color.a)
        {
            color = new(color.r, color.g, color.b, spriteRenderer.color.a);
            foreach (LineRenderer line in GetComponentsInChildren<LineRenderer>())
            {
                line.startColor = color;
                line.endColor = color;
            }
        }
    }
}
