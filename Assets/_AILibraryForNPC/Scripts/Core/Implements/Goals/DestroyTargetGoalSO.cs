using UnityEngine;
using AILibraryForNPC.core;
using System.Collections.Generic;

namespace AILibraryForNPC.core.Implements.Goals
{
    [CreateAssetMenu(fileName = "DestroyTargetGoal", menuName = "AI/Goals/Destroy Target")]
    public class DestroyTargetGoalSO : GOAPGoalSO
    {
        //[SerializeField] private float targetPriority = 1f;

        //private void OnEnable()
        //{
        //    //// Định nghĩa trạng thái mục tiêu
        //    //goalState["TargetDestroyed"] = true;
        //    //goalState["HasTarget"] = false;
        //}

        //public override float EvaluateGoal(Dictionary<string, object> state)
        //{
        //    // Kiểm tra nếu mục tiêu đã bị phá hủy
        //    if (state.ContainsKey("TargetDestroyed") && (bool)state["TargetDestroyed"])
        //    {
        //        return 0f; // Mục tiêu đã đạt được
        //    }

        //    // Kiểm tra nếu có mục tiêu
        //    if (state.ContainsKey("HasTarget") && (bool)state["HasTarget"])
        //    {
        //        var target = state["CurrentTarget"] as Transform;
        //        var agent = state["Agent"] as Transform;

        //        if (target != null && agent != null)
        //        {
        //            // Tính điểm dựa trên khoảng cách và độ ưu tiên
        //            float distance = Vector3.Distance(agent.position, target.position);
        //            return Mathf.Clamp01(1f - (distance / 20f)) * targetPriority;
        //        }
        //    }

        //    return 0f;
        //}

        //public override bool IsValid(Dictionary<string, object> state)
        //{
        //    // Kiểm tra nếu có mục tiêu và chưa bị phá hủy
        //    return state.ContainsKey("HasTarget") &&
        //           (bool)state["HasTarget"] &&
        //           (!state.ContainsKey("TargetDestroyed") || !(bool)state["TargetDestroyed"]);
        //}
    }
}
