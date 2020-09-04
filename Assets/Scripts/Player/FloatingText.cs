using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    public GameObject textMeshProPrefab;
    public Color32 textColor;
    public byte fadeSpeed = 1;
    public float floatSpeed = 0.1f;
    public Vector2 offset = new Vector2(0, 2);

    private TextMeshPro text;
    private PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        // Make sure our prefab has a TMP component else its all going to go downhill fast.
        try
        {
            text = textMeshProPrefab.GetComponent<TextMeshPro>();
        }
        catch (UnityException e)
        {
            Debug.Log("The assigned gameobject 'Text Mesh Pro Prefab' does not have a Text Mesh Pro component. Please fix this before continuing.\nAdditional Information: " + e);
        }

        playerController = this.GetComponent<PlayerController>();
    }

    public void CreateFloatingText(string symbol, int pointsValue)
    {
        playerController.PlaySuccessfulJumpSound(pointsValue);
        GameObject go = Instantiate(textMeshProPrefab, (Vector2)transform.position + (Vector2)offset, Quaternion.identity) as GameObject;
        text = go.GetComponent<TextMeshPro>();
        // Set the new objects color and make it transparent.
        //text.color = new Color(textColor.r, textColor.g, textColor.b, 255);
        text.faceColor = new Color32(textColor.r, textColor.g, textColor.b, 255);
        text.text = symbol + pointsValue;
        StartCoroutine(FloatAndFade(go));
    }

    private IEnumerator FloatAndFade(GameObject floatingText)
    {
        while(text.faceColor.a > 0)
        {
            byte newAlpha = (byte)(text.faceColor.a - fadeSpeed);
            //text.color = new Color(textColor.r, textColor.g, textColor.b, newAlpha);
            text.faceColor = new Color32(textColor.r, textColor.g, textColor.b, newAlpha);
            floatingText.transform.position = new Vector2(floatingText.transform.position.x, floatingText.transform.position.y + 1 * Time.deltaTime);
            yield return null;
        }

        if (text.faceColor.a <= 0)
        {
            Destroy(floatingText);
            StopCoroutine("FloatAndFade");
        }
    }
}
