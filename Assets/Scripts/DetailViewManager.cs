using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class SCInputField
{
    public static void SetImmutable(this InputField inputField, bool editMode)
    {
        if (editMode)
        {
            inputField.transform.Find("Text").GetComponent<Text>().color
                = Color.black;
        }
        else
        {
            inputField.transform.Find("Text").GetComponent<Text>().color
                = Color.white;
        }

        inputField.transform.Find("Placeholder").gameObject.SetActive(editMode);
        inputField.GetComponent<InputField>().interactable = editMode;
        inputField.GetComponent<Image>().enabled = editMode;
    }
}
public class DetailViewManager : ViewManager
{
    [SerializeField] InputField nameInputField;
    [SerializeField] InputField phoneNumberInputField;
    [SerializeField] InputField emailInputField;
    [SerializeField] Button saveButton;
    [SerializeField] Image detailImage;

    public delegate void DetailViewManagerSaveDelegate(Contact contact);
    public DetailViewManagerSaveDelegate saveDelegate;

    public Contact? contacts;

    bool editMode = true;

    void ToggleEditMode(bool updateInputField = false)
    {
        editMode = !editMode;

        // 저장버튼
        saveButton.gameObject.SetActive(editMode);

        nameInputField.SetImmutable(editMode);
        phoneNumberInputField.SetImmutable(editMode);
        emailInputField.SetImmutable(editMode);

        if (editMode)
        {
            rightNavgationViewButton.SetTitle("취소");
        }
        else
        {
            rightNavgationViewButton.SetTitle("편집");

            // 데이터 화면 출력
            if (contacts.HasValue && !updateInputField)
            {

                Contact contactValue = contacts.Value;
                nameInputField.text = contactValue.name;
                phoneNumberInputField.text = contactValue.phoneNumber;
                emailInputField.text = contactValue.email;
                detailImage.sprite = SpriteManager.GetSprite(contactValue.profilePhotoFileName);
            }
        }
    }

    private void Awake()
    {
        // Title 지정
        title = "상세화면";
        // Add 버튼 지정
        rightNavgationViewButton = Instantiate(buttonPrefab).GetComponent<SCButton>();
        rightNavgationViewButton.SetTitle("편집");
        rightNavgationViewButton.SetOnClickAction(() =>
        {
            ToggleEditMode();
        });
    }

    private void Start()
    {
        ToggleEditMode();
    }

    private void OnDestroy()
    {
        Destroy(rightNavgationViewButton.gameObject);
    }

    public void Save()
    {

        Contact newContact = new Contact();
        newContact.name = nameInputField.text;
        newContact.phoneNumber = phoneNumberInputField.text;
        newContact.email = emailInputField.text;
         
        saveDelegate?.Invoke(newContact);

        ToggleEditMode(true);
    }
}
