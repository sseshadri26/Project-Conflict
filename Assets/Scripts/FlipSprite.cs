using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipSprite : MonoBehaviour
{
    // Simple script to flip a sprite if it faces left
    // Check's parent transform because sprites are children and parent has main logic
    private SpriteRenderer sprite;
    void Start() {
        sprite = gameObject.GetComponent<SpriteRenderer>();
    }
    void FixedUpdate()
    {
        if (this.transform.parent.transform.up.x < 0) {
            sprite.flipX = true;
        } else {
            sprite.flipX = false;
        }
    }
}
