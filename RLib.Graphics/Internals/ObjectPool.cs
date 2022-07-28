// created: 2022/07/26 14:58
// author:  rush
// email:   yacper@gmail.com
// 
// purpose:
// modifiers:

namespace RLib.Graphics;


internal class ObjectPool<T> where T : class
{
    public void Reset() { Index = 0; }

    public T Get()
    {
        if (Index > _List.Count)
        {
            int expand = _List.Count / 2;
            for (int i = 0; i != expand; ++i)
                _List.Add(Activator.CreateInstance<T>());
        }

        return _List[Index++];
    }

    public ObjectPool(int count)
    {
        for (int i = 0; i != count; ++i)
            _List.Add(Activator.CreateInstance<T>());
    }

    protected int     Index = 0;
    protected List<T> _List = new List<T>();
}