using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FileExplorer : MonoBehaviour {

  
    string[] subDirectories;
    string[] directoryFiles;
    string currentPath;
    string previousFolder;
    string currentFolderName;

    #region PUBLIC REFERENCES
    public GameObject scrollViewContent;
    public Text currentFolderCanvasReference;
    public GameObject ContentElementPrefab;
    public Text fileChosenCanvasReference;
    #endregion

    List<GameObject> subDirectoryGOReference = new List<GameObject>();
    


    private void Awake()
    {
        currentPath = Application.dataPath;
        subDirectories = Directory.GetDirectories(currentPath);
        directoryFiles = Directory.GetFiles(currentPath);
        UpdateFolderReferences();
        CreateDirectoryList();

    }

    // Use this for initialization
    void Start ()
    {
        /*
        print(currentPath);
        for (int i = 0; i < subDirectories.Length; i++)
        {
            print(subDirectories[i] + " extension: " + Path.GetExtension(subDirectories[i]) +
                "number of characters in extension: " + Path.GetExtension(subDirectories[i]).Length);
        }
        for (int i = 0; i < directoryFiles.Length; i++)
        {
            print(directoryFiles[i] + " extension: " + Path.GetExtension(directoryFiles[i]) +
                "number of characters in extension: " + Path.GetExtension(directoryFiles[i]).Length);
        }
        */

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void UpdateFolderReferences()
    {
        for (int i = currentPath.Length - 2; i >= 0; i--)
        {
            if (currentPath[i] == '/')
            {
                currentFolderName = currentPath.Substring(i);
                if (currentFolderCanvasReference != null)
                { currentFolderCanvasReference.text = "Current Folder: " + currentFolderName.Substring(1); }

                previousFolder = currentPath.Substring(0, i) + '/';
                //print("New previous Folder:  " + previousFolder);

                break;                
            }
        }
    }

    public void GoToPreviousFolder()
    {
        print(currentPath);
        //CHANGE THE CURRENT PATH TO THE PREVIOUS ONE

        if (previousFolder == "C:/")
        {
            currentPath = previousFolder;
            subDirectories = Directory.GetDirectories(currentPath);
            directoryFiles = Directory.GetFiles(currentPath);
            /*
            print(currentPath);
            for (int i = 0; i < subDirectories.Length; i++)
            {
                print("NEW: " + subDirectories[i]);

            }
            for (int i = 0; i < directoryFiles.Length; i++)
            {
                print("NEW: " + directoryFiles[i]);

            }
            */
            if (currentFolderCanvasReference != null)
            { currentFolderCanvasReference.text = "Current Folder: " + "C:/"; }

        }
        else
        {
            currentPath = previousFolder;
            subDirectories = Directory.GetDirectories(currentPath);
            directoryFiles = Directory.GetFiles(currentPath);



            /*
            print(currentPath);
            for (int i = 0; i < subDirectories.Length; i++)
            {
                print("NEW: " + subDirectories[i]);

            }
            for (int i = 0; i < directoryFiles.Length; i++)
            {
                print("NEW: " + directoryFiles[i]);

            }
            */
        }
        //DETECT THE NEW PREVIOUS FOLDER
        UpdateFolderReferences();
            CreateDirectoryList();
        
           
    }

    

    void CreateDirectoryList()
    {
        //NO ELIMINES: SUSTITUYE DATOS. LOS ELEMENTOS DE MÁS, SÍ QUE DEBES ELIMINARLOS. LOS ELEMENTOS DE MENOS, CREARLOS.
        int ShownElementsCount = 0;

        //Current Folders!!!
        for (int i = 0; i < subDirectories.Length; i++)
        {
            if (Path.GetExtension(subDirectories[i]).Length == 0)
            {
                print("Encontramos carpeta!!!");
                if (subDirectoryGOReference.Count > ShownElementsCount)
                {
                    subDirectoryGOReference[ShownElementsCount].GetComponentInChildren<Text>().text = Path.GetFileName(subDirectories[i]);
                }
                else
                {
                    GameObject newItem = Instantiate(ContentElementPrefab, scrollViewContent.transform);
                    newItem.SetActive(true);                 

                    newItem.GetComponentInChildren<Text>().text = Path.GetFileName(subDirectories[i]);
                    subDirectoryGOReference.Add(newItem);
                    subDirectoryGOReference[ShownElementsCount].GetComponent<RectTransform>().anchoredPosition =
                      new Vector2(subDirectoryGOReference[ShownElementsCount].GetComponent<RectTransform>().anchoredPosition.x,
                                  subDirectoryGOReference[ShownElementsCount].GetComponent<RectTransform>().anchoredPosition.y - 30 * ShownElementsCount);
                }


                //print(Path.GetExtension(subDirectories[i]));
                //subDirectoryGOReference[ShownElementsCount].GetComponent<RectTransform>().localPosition= new Vector3(0f, 0f, 0f);

                ShownElementsCount++;
            }
        }

        //CURRENT FILES
        for (int i = 0; i < directoryFiles.Length; i++)
        {
            if (Path.GetExtension(directoryFiles[i]) == ".PNG" 
                || Path.GetExtension(directoryFiles[i]) == ".png")
            {
                print("Encontramos PNG!!!");
                if (subDirectoryGOReference.Count > ShownElementsCount)
                {
                    subDirectoryGOReference[ShownElementsCount].GetComponentInChildren<Text>().text = Path.GetFileName(directoryFiles[i]);
                }
                else
                {
                    GameObject newItem = Instantiate(ContentElementPrefab, scrollViewContent.transform);
                    newItem.SetActive(true);

                    newItem.GetComponentInChildren<Text>().text = Path.GetFileName(directoryFiles[i]);
                    subDirectoryGOReference.Add(newItem);
                    subDirectoryGOReference[ShownElementsCount].GetComponent<RectTransform>().anchoredPosition =
                      new Vector2(subDirectoryGOReference[ShownElementsCount].GetComponent<RectTransform>().anchoredPosition.x,
                                  subDirectoryGOReference[ShownElementsCount].GetComponent<RectTransform>().anchoredPosition.y - 30 * ShownElementsCount);
                }


                //print(Path.GetExtension(directoryFiles[i]));
                //subDirectoryGOReference[ShownElementsCount].GetComponent<RectTransform>().localPosition= new Vector3(0f, 0f, 0f);

                ShownElementsCount++;
            }
        }


        if (ShownElementsCount < subDirectoryGOReference.Count)
        {
            for (int j = subDirectoryGOReference.Count - 1; j >= ShownElementsCount; j--)
            {
                GameObject trash = subDirectoryGOReference[j];
                subDirectoryGOReference.Remove(trash);
                DestroyObject(trash);
            }
        }

    }


    public void ChangeCurrentDirectory(Text buttonText)
    {
        print(Path.GetExtension(currentPath));
        if (Path.GetExtension(currentPath).Length == 0)
        {
            if (Directory.Exists(currentPath + '/' + buttonText.text))
            {
                currentPath = currentPath + '/' + buttonText.text;
            }
            subDirectories = Directory.GetDirectories(currentPath);
            directoryFiles = Directory.GetFiles(currentPath);

           /* for (int i = 0; i < subDirectories.Length; i++)
            {
                print("NEW: " + subDirectories[i]);

            }
            for (int i = 0; i < directoryFiles.Length; i++)
            {
                print("NEW: " + directoryFiles[i]);

            }
            */
        }
        else //SPOILER: ES PNG
        {
            if (fileChosenCanvasReference != null)
            {
                fileChosenCanvasReference.text = "Chosen File: " + buttonText.text;
            }
            
        }

        UpdateFolderReferences();
        CreateDirectoryList();
    }

    public void ApplySelectedTexture()
    { }




}
