using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityMenuPanelController : MonoBehaviour
{
    const string ShowKey = "Show";
    const string HideKey = "Hide";
    const string EntryPoolKey = "AbilityMenuPanel.Entry";
    const int MenuCount = 4;

    [SerializeField] GameObject entryPrefab;
    [SerializeField] Text titleLabel;
    [SerializeField] PanelPosition panel;
    [SerializeField] GameObject canvas;
    List<AbilityMenuEntry> menuEntries = new List<AbilityMenuEntry>(MenuCount);
    public int selection { get; private set; }

    private void Awake()
    {
        //Configure pool manager to generate menu entries
        GameObjectPoolController.AddEntry(EntryPoolKey, entryPrefab, MenuCount, int.MaxValue);

    }

    //Methods to get and return menu entries to the pool manager
    AbilityMenuEntry Dequeue()
    {
        Poolable p = GameObjectPoolController.Dequeue(EntryPoolKey);
        AbilityMenuEntry entry = p.GetComponent<AbilityMenuEntry>();
        entry.transform.SetParent(panel.transform, false);
        entry.transform.localScale = Vector3.one;
        entry.Reset();
        return entry;
    }

    void Enqueue (AbilityMenuEntry entry)
    {
        Poolable p = entry.GetComponent<Poolable>();
        GameObjectPoolController.Enqueue(p);
    }

    //Method to clear the menu
    void Clear()
    {
        for (int i = 0; i < menuEntries.Count; --i)
            Enqueue(menuEntries[i]);
        menuEntries.Clear();
    }

    //Start - Panel hidden and disabled
    private void Start()
    {
        panel.SetPosition(HideKey, false);
        canvas.SetActive(false);
    }

    //Show Menu through game events - Animation to in
    Tweener TogglePos(string pos)
    {
        Tweener t = panel.SetPosition(pos, true);
        t.easingControl.duration = 0.5f;
        t.easingControl.equation = EasingEquations.EaseOutQuad;
        return t;
    }

    // Menu appears, one entry is always highlighted as Selected
    bool SetSelection(int value)
    {
        //Don't select entries that are locked
        if (menuEntries[value].IsLocked)
            return false;

        //Deselect the previously selected entry
        if (selection >= 0 && selection < menuEntries.Count)
            menuEntries[selection].IsSelected = false;

        selection = value;

        // Select the new entry
        if(selection >= 0 && selection < menuEntries.Count)
            menuEntries[selection].IsSelected = true;

        return true;
    }


    #region Public Methods

    //Methods for the menu controller to select previous or next entry in the list
    // Loop to check adjacent entry is locked or not, modulus operator to have the index calue to wrap
    public void Next()
    {
        for (int i = selection + 1; i < selection + menuEntries.Count; ++i)
        {
            int index = i % menuEntries.Count;
            if (SetSelection(index))
                break;
        }
    }

    public void Previous()
    {
        for (int i = selection - 1 + menuEntries.Count; i > selection; --i)
        {
            int index = i % menuEntries.Count;
            if (SetSelection(index))
                break;
        }
    }

    //Load and Display menu
    // Title -> Title to display in the header
    //Options -> Strings to display for each entry in the menu
    public void Show(string title, List<string> options)
    {
        canvas.SetActive(true);
        Clear();
        titleLabel.text = title;
        for (int i = 0; i < options.Count; ++i)
        {
            AbilityMenuEntry entry = Dequeue();
            entry.Title = options[i];
            menuEntries.Add(entry);
        }

        SetSelection(0);
        TogglePos(ShowKey);
    }

    //Specify some of the menu entries are locked
    public void SetLocked(int index, bool value)
    {
        if(index < 0 || index >= menuEntries.Count) return;

        menuEntries[index].IsLocked = value;
        if(value && selection == index)
            Next();
    }

    // Hide - after menu selection (it happens with Fire1 input in Input Settings), dismiss the panel
    public void Hide()
    {
        Tweener t = TogglePos(HideKey);
        t.easingControl.completedEvent += delegate (object sender, System.EventArgs e)
        {
            if (panel.currentPosition == panel[HideKey])
            {
                Clear();
                canvas.SetActive(false);
            }
        };
    }

    #endregion
}
