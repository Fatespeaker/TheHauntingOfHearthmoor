using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // For TextMeshPro Dropdown

public class CharacterSelection : MonoBehaviour
{
    [Header("UI Elements")]
    public Button maleButton;
    public Button femaleButton;
    public TMP_Dropdown raceDropdown;
    public TMP_Dropdown hairDropdown; // Dropdown for selecting hairstyles

    [Header("Character Models")]
    public Transform characterContainer;
    public GameObject[] maleModels; // Prefabs for Male Races
    public GameObject[] femaleModels; // Prefabs for Female Races

    private GameObject currentModel; // The active character
    private bool isMale = true; // Default gender
    private int raceIndex = 0; // Default race
    public int hairIndex = 0; // Default hairstyle

    private bool isDragging = false;
    public float rotationSpeed = 5f;

    public Transform hairContainer; // Stores the found HairContainer
    public List<GameObject> hairList = new List<GameObject>(); // List of hair objects

    void Start()
    {
        // Assign UI listeners
        maleButton.onClick.AddListener(() => SetGender(true));
        femaleButton.onClick.AddListener(() => SetGender(false));
        raceDropdown.onValueChanged.AddListener(SetRace);
        hairDropdown.onValueChanged.AddListener(SetHairStyle);

        // Initialize character preview
        UpdateCharacter();
    }

    void Update()
{
    if (Input.GetMouseButtonDown(0)) // Left mouse button pressed
        isDragging = true;

    if (Input.GetMouseButtonUp(0)) // Left mouse button released
        isDragging = false;

    if (isDragging && currentModel != null)
    {
        float rotation = Input.GetAxis("Mouse X") * rotationSpeed;
        currentModel.transform.Rotate(Vector3.up, -rotation);
    }
}

    void SetGender(bool male)
    {
        isMale = male;
        UpdateCharacter();
        hairIndex = 0;
    }

    void SetRace(int index)
    {
        raceIndex = index;
        UpdateCharacter();
    }

    void SetHairStyle(int index)
    {
        hairIndex = index;
        UpdateHair();
    }

    void UpdateCharacter()
    {
        // Destroy previous model
        if (currentModel != null)
            Destroy(currentModel);

        // Select the correct model prefab
        GameObject modelPrefab = isMale ? maleModels[raceIndex] : femaleModels[raceIndex];

        // Instantiate the new character inside the container
        currentModel = Instantiate(modelPrefab, characterContainer);
        currentModel.transform.localPosition = Vector3.zero;
        currentModel.transform.localRotation = Quaternion.identity;
        currentModel.transform.localScale = Vector3.one;

        // Automatically find the HairContainer in the character model
        hairContainer = FindChildByName(currentModel.transform, "hairHolder");

        // Automatically load hair models from HairContainer
        LoadHairModels();

        // Apply the default hairstyle
        UpdateHair();

        List<string> hairsForGender;
        if (isMale == true) {
        hairsForGender = new List<string> { "Short", "Bowl Cut", "Long", "Orange Slice" };
        } else {
        hairsForGender = new List<string> { "Loose", "Braid", "Pigtails", "Short" };
        }
        
        hairDropdown.ClearOptions();
        hairDropdown.AddOptions(hairsForGender);
        hairDropdown.value = 0;
    }

    void LoadHairModels()
    {
        hairList.Clear(); // Clear previous list

        if (hairContainer != null)
        {
            foreach (Transform child in hairContainer)
            {
                hairList.Add(child.gameObject); // Add all hair models
            }
        }
    }
void UpdateHair()
    {
        if (hairList.Count == 0) return;

        // Disable all hairstyles
        foreach (GameObject hair in hairList)
        {
            hair.SetActive(false);
        }

        // Enable the selected hairstyle
        if (hairIndex < hairList.Count)
        {
            hairList[hairIndex].SetActive(true);
        }
    }

    Transform FindChildByName(Transform parent, string childName) {

    foreach (Transform child in parent)
    {
        if (child.name == childName)
        {
            return child;
        }
        Transform found = FindChildByName(child, childName);
        if (found != null)
            return found;
    }

    return null;
}
}
