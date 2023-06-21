using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeSwampsManager : MonoBehaviour
{
    [SerializeField] GameObject _realSwamp;
    [SerializeField] List<FakeSwamp> _fakeSwampList = new List<FakeSwamp>();
    [SerializeField] FakeSwamp _currentFakeSwamp;
    // Start is called before the first frame update

    static FakeSwampsManager _fakeSwampsManager;
    
    public static FakeSwampsManager GetFakeSwampsManager()
    {
        if (_fakeSwampsManager == null)
        {
            _fakeSwampsManager = InitFakeSwampManager();
        }
        return _fakeSwampsManager;
    }

    private static FakeSwampsManager InitFakeSwampManager()
    {
        GameObject fakeSwampManager = new GameObject("FakeSwampManager");
        fakeSwampManager.transform.position = Vector3.zero;
        return fakeSwampManager.AddComponent<FakeSwampsManager>();
    }

    void Start()
    {
        _realSwamp = FindObjectOfType<WaterShapeController>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCurrentSwamp(FakeSwamp fakeSwamp)
    {
        if (!OnlyPossibleActive(fakeSwamp))
        {
            Debug.Log("Multiple Swamps Can Be Active!");
            return;
        }

        _realSwamp.transform.position = fakeSwamp.transform.position;
        _realSwamp.transform.localScale = fakeSwamp.transform.localScale;
        _currentFakeSwamp = fakeSwamp;
        ActivateAllFakeSwampsRender();
        _currentFakeSwamp.SetActiveFakeSwampRender(false);
    }

    public void AddToFakeSwampsList(FakeSwamp fakeSwamp)
    {
        _fakeSwampList.Add(fakeSwamp);
    }

    private bool OnlyPossibleActive(FakeSwamp fakeSwamp)
    {
        if (!fakeSwamp.CanGetActive())
        {
            return false;
        }

        foreach (FakeSwamp swamp in _fakeSwampList)
        {
            if (swamp.CanGetActive() && swamp != fakeSwamp)
            {
                return false;
            }
        }

        return true;
    }

    private void ActivateAllFakeSwampsRender()
    {
        foreach (FakeSwamp fs in _fakeSwampList)
        {
            fs.SetActiveFakeSwampRender(true);
        }
    }
}
