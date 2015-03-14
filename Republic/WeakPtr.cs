using System;
using System.Collections.Generic;
using System.Text;

namespace GroogyLib
{
    namespace Core
    {
        public class WeakPtr<T>
        {
            private WeakReference reference = null;

            public WeakPtr(T obj)
            {
                this.reference = new WeakReference(obj);
            }

            public bool IsAlive
            {
                get
                {
                    return this.reference.IsAlive;
                }
            }

            public T Target
            {
                get
                {
                    return (T)this.reference.Target;
                }
                set
                {
                    this.Target = value;
                }
            }
        }
    }
}
