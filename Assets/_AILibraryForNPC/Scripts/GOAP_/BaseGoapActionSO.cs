using System;
using AILibraryForNPC.core;

public abstract class BaseGoapActionSO : BaseActionSO
{
    /// <summary>
    /// Trạng thái hiện tại có thể thực hiện action này không?
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    public abstract bool ArePreconditionsSatisfied(GoapWorldState state);

    /// <summary>
    /// Chi phí thực hiện action
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    public abstract float GetCost(GoapWorldState state);

    /// <summary>
    /// Áp dụng tác động của action lên trạng thái hiện tại
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    public abstract GoapWorldState ApplyEffects(GoapWorldState state);
}
