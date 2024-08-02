using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawOnTexture : MonoBehaviour
{
    public Texture2D baseTexture;
    public Color clearColor = Color.black;


    void Update() { DoMouseDrawing(); }


    /// <exception cref="Exception"></exception>
    private void DoMouseDrawing()
    {

        if (Camera.main == null) { throw new Exception("Camera yok"); }

        if (!Input.GetMouseButton(0) && !Input.GetMouseButton(1)) return;

        if (Input.GetMouseButton(1))
        {
            ClearTexture();
            return;
        }


        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;


        if (!Physics.Raycast(mouseRay, out hit)) return;

        if (hit.collider.transform != transform) return;


        Vector2 pixelUV = hit.textureCoord;
        pixelUV.x *= baseTexture.width;
        pixelUV.y *= baseTexture.height;


        Color colorToSet = Color.white;

        baseTexture.SetPixel((int)pixelUV.x, (int)pixelUV.y, colorToSet);
        baseTexture.Apply();
    }


    private void ClearTexture()
    {
        Color[] clearColorArray = new Color[baseTexture.width * baseTexture.height];
        for (int i = 0; i < clearColorArray.Length; i++)
        {
            clearColorArray[i] = clearColor;
        }
        baseTexture.SetPixels(clearColorArray);
        baseTexture.Apply();
    }
}
