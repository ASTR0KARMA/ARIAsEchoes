namespace NoaDebugger
{
    abstract class SelectableGroup<T> : SelectableKeyValueBase, ISelectableGroup<T>
    {
        public abstract string GroupName { get; }

        public abstract void Update(T data);
    }
}
