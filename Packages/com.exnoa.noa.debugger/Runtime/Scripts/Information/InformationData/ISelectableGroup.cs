namespace NoaDebugger
{
    interface ISelectableGroup<T> : ISelectableGroupBase
    {
        void Update(T data);
    }

    interface ISelectableGroupBase
    {
        string GroupName { get; }
    }
}
