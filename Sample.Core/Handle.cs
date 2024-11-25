namespace Sample.Core;

public readonly unsafe struct Handle<T> where T : unmanaged
{
#pragma warning disable CS0649 // It's never set because this is returned from PInvoke 
    private readonly T* _ptr;
#pragma warning restore CS0649

    public bool IsNull => _ptr == null;

    public ref T Value
    {
        get
        {
            if (_ptr == null) throw new NullReferenceException();
            return ref *_ptr;
        }
    }
}
