using System.Collections.Generic;
using DatSystem;
using TMPro;
using UnityEngine;

public class UIResourceCost : MonoBehaviour
{
    [SerializeField]
    private List<GroupResource> _groupResources;

    public void SetInfo(ResourceAndQuantity[] resourceAndQuantities)
    {
        var playerData = DataManager.Instance.PlayerData;
        ResetGroupResource();
        foreach (var cost in resourceAndQuantities)
        {
            var groupResource = GetGroupResource(cost.resourceType);
            groupResource.textCost.text = cost.quantity.ToString();
            groupResource.textCost.color =
                cost.quantity >= playerData.GetResource(cost.resourceType)
                    ? Color.red
                    : Color.white;
            groupResource.objCost.SetActive(true);
        }
    }

    private void ResetGroupResource()
    {
        foreach (var groupResource in _groupResources)
        {
            groupResource.objCost.SetActive(false);
        }
    }

    private GroupResource GetGroupResource(EResourceType resourceType)
    {
        return _groupResources.Find(group => group.resourceType == resourceType);
    }
}

[System.Serializable]
public class GroupResource
{
    public EResourceType resourceType;
    public GameObject objCost;
    public TMP_Text textCost;
}
