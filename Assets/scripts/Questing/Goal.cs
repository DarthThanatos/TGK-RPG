
public abstract class Goal
{
    public string Description { get; set; }
    public bool Completed { get; set; }
    public int CurrentAmount { get; set; }
    public int RequiredAmount { get; set; }
    public Quest Quest { get; set; }
    public Phase Phase { get; set; }

    public virtual void Init()
    {

    }

    public virtual void UnInit()
    {

    }

    public virtual void Finish()
    {

    }

    public abstract string GetGoalState();

    public virtual void Evaluate()
    {
        if (CurrentAmount >= RequiredAmount)
        {
            Complete();
        }
        else
        {
            Completed = false;
        }
    }

    public void Complete()
    {
        Completed = true;
    }
}
