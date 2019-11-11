using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ScrollViewManager : ViewManager, ICell
{
    [SerializeField] GameObject cellPrefab;
    [SerializeField] GameObject addPopupViewPrefab;
    [SerializeField] GameObject detailViewPrefab;
    [SerializeField] GameObject removePopupPrefab;

    [SerializeField] RectTransform content;

    List<Cell> cellList = new List<Cell>();

    float cellHeight = 240f;
    Contacts? contacts;

    // Cell 편집 버튼 관련
    bool isEditable = false;


    private void Awake()
    {

        // Title 지정
        title = "YH 연락처";

        // Add 버튼
        rightNavgationViewButton = Instantiate(buttonPrefab).GetComponent<SCButton>();
        rightNavgationViewButton.SetTitle("추가");

        rightNavgationViewButton.SetOnClickAction(() =>
        {
            // TODO: AddPopupViewManager를 표시하는 동작
            AddPopupViewManager addPopupViewManager =
            Instantiate(addPopupViewPrefab, mainManager.transform).GetComponent<AddPopupViewManager>();

            // 새로운 연락처를 추가했을때 할일
            addPopupViewManager.addContactCallback = (contact) =>
            {
                AddContact(contact);
                ClearCell();
                LoadData();
            };

            // AddPopupViewManager 열기
            addPopupViewManager.Open();
        });

        // 왼쪽 버튼(Edit: 셀을 삭제할 수 있는 기능)
        leftNavgationViewButton = Instantiate(buttonPrefab).GetComponent<SCButton>();
        leftNavgationViewButton.SetTitle((isEditable) ? "완료" : "편집");
        leftNavgationViewButton.SetOnClickAction(() =>
        {
            isEditable = !isEditable;

            if (isEditable)
            {
                leftNavgationViewButton.SetTitle("완료");
                foreach (Cell cell in cellList)
                {
                    cell.ActiveDelete = true;
                    //cell.GetComponent<Button>().interactable = false;
                }
            }
            else
            {
                leftNavgationViewButton.SetTitle("편집");
                foreach (Cell cell in cellList)
                {
                    cell.ActiveDelete = false;
                    //cell.GetComponent<Button>().interactable = true;
                }
            }
        });
    }

    private void Start()
    {
        contacts = FileManager<Contacts>.Load(Constant.kFileName);
        LoadData();
    }

    void LoadData()
    {
        if (contacts.HasValue)
        {
            Contacts contactsValue = contacts.Value;

            contactsValue.contactList.Sort();
            
            for (int i = 0; i < contactsValue.contactList.Count; i++)
            {

                AddCell(contactsValue.contactList[i], i);

            }
        }
    }

    // Contact 정보로 Cell 객체를 만들어서 content에 추가하는 함수
    void AddCell(Contact contact, int index)
    {
        Cell cell = Instantiate(cellPrefab, content).GetComponent<Cell>();
        cell.Title = contact.name;
        cell.ProfilePhotoSprite = SpriteManager.GetSprite(contact.profilePhotoFileName);
        cell.cellDelegate = this;
        cellList.Add(cell);

        // Content의 높이 재조정
        AdjustConent();
    }

    void AddContact(Contact contact)
    {
        if (contacts.HasValue)
        {
            Contacts contactsValue = contacts.Value;
            contactsValue.contactList.Add(contact);
        }
        else
        {
            List<Contact> contactsList = new List<Contact>();
            contactsList.Add(contact);
            contacts = new Contacts(contactsList);
        }
    }

    // Content의 높이 재조정
    void AdjustConent()
    {
        if (contacts.HasValue)
        {
            Contacts contacactsValue = contacts.Value;
            content.sizeDelta = new Vector2(0, contacactsValue.contactList.Count * cellHeight);
        }
        else
        {
            content.sizeDelta = Vector2.zero;
        }

    }

    private void OnApplicationQuit()
    {
        if (contacts.HasValue)
            FileManager<Contacts>.Save(contacts.Value, Constant.kFileName);
    }

    // Cell이 터치되었을때 호출하는 함수
    public void DidSelectCell(Cell cell)
    {
        // Cell과 관련된 Detail 화면 표시
        //Destroy(leftNavgationViewButton.gameObject);
        if (contacts.HasValue)
        {
            int cellIndex = cellList.IndexOf(cell);

            DetailViewManager detailViewManager = Instantiate(detailViewPrefab).GetComponent<DetailViewManager>();

            Contact selectedContact = contacts.Value.contactList[cellIndex];
            detailViewManager.contact = selectedContact;

            // ---절취선---

            detailViewManager.saveDelegate = (newContact) =>
            {
                contacts.Value.contactList[cellIndex] = newContact;
                cell.Title = newContact.name;
            };
            mainManager.PresentViewManager(detailViewManager);
        }
    }

    public void DidSelectDeleteCell(Cell cell)
    {  
        if (contacts.HasValue)
        {
            RemovePopup removePopup = Instantiate(removePopupPrefab, mainManager.transform).GetComponent<RemovePopup>();

            removePopup.removePopupViewManagerDelegate = () =>
            {
                int cellIndex = cellList.IndexOf(cell);

                List<Contact> contactList = contacts.Value.contactList;
                contactList.RemoveAt(cellIndex);
                cellList.RemoveAt(cellIndex);

                Destroy(cell.gameObject);
                AdjustConent();
            };
            removePopup.Open();
        }
    }

    void ClearCell()
    {
        foreach(Cell cell in cellList)
        {
            Destroy(cell.gameObject);
        }
        cellList.RemoveRange(0, cellList.Count);
    }
}