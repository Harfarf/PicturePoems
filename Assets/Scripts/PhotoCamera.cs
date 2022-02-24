using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoCamera : MonoBehaviour
{
    //El script lo lleva la cámara que va a hacer las "fotos"
    private static PhotoCamera instance;

    private Camera photoCam;
    private bool canTakeScreenshot;
    int photoIndex = 0;

    private void Awake()
    {
        instance = this;
        photoCam = gameObject.GetComponent<Camera>();
    }
    private void Update()
    {
        //Condición para sacar la foto
        //Hacer que la cámara se coloque en el punto deseado y cuando esté en él haya que pulsar un botón?
        //if (Input.GetKeyDown(KeyCode.F))
        //{
        //    Screenshot(500, 500);
        //}
    }

    private void OnPostRender()
    {
        if (canTakeScreenshot)
        {
            canTakeScreenshot = false;
            photoIndex++;
            RenderTexture renderTexture = photoCam.targetTexture;

            Texture2D renderOutput = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
            Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);
            renderOutput.ReadPixels(rect, 0, 0);

            //Convertimos la textura 2D en .PNG y lo guardamos en el path deseado
            byte[] byteArray = renderOutput.EncodeToPNG();
            System.IO.File.WriteAllBytes(Application.dataPath + "/Photos" + "/Photo" + photoIndex + ".png", byteArray); //Crear carpeta llamada Photos en Assets
            //Debug.Log(Application.dataPath);

            RenderTexture.ReleaseTemporary(renderTexture);
            photoCam.targetTexture = null;
        }
    }

    public static void Screenshot(int width, int height) 
    {
        //Saca la captura con las medidas deseadas
        instance.TakeScreenshot(width, height);
    }
    private void TakeScreenshot(int width, int height)
    {
        photoCam.targetTexture = RenderTexture.GetTemporary(width, height, 16);
        canTakeScreenshot = true;
        Debug.Log("PHOTO TAKEN");
    }

}
