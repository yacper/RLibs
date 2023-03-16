// created: 2022/07/26 14:58
// author:  rush
// email:   yacper@gmail.com
// 
// purpose:
// modifiers:

namespace RLib.Graphics;


internal class ObjectPool<T> where T : class
{
    public void Dispose()
    {
        Index = 0;
        List_.Clear();
        List_ = null;
    }
    public void Reset() { Index = 0; }

    public T Get()
    {
        if (Index >= List_.Count)
        {
            int expand = List_.Count / 2;
            for (int i = 0; i != expand; ++i)
                List_.Add(Activator.CreateInstance<T>());
        }

        return List_[Index++];
    }

    public ObjectPool(int count)
    {
        for (int i = 0; i != count; ++i)
            List_.Add(Activator.CreateInstance<T>());
    }

    protected int     Index = 0;
    protected List<T> List_ = new List<T>();
}