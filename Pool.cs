using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dreamscape {

    /// <summary>
    /// A pool that has a limit.
    /// </summary>
    public class Pool<T> where T : IPoolable {

        /// <summary>
        /// Create a new pool.
        /// </summary>
        public Pool(int size) {
            Items = new T[size];
        }

        /// <summary>
        /// Item pool.
        /// </summary>
        private T[] Items;

        /// <summary>
        /// Next item available.
        /// </summary>
        private int NextAvailable = 0;

        /// <summary>
        /// Get an object recycled from the pool, or make a new one if needed.
        /// </summary>
        /// <param name="parameters">Initialization parameters.</param>
        /// <returns>An object recycled from the pool.</returns>
        public T GetObject(object parameters) {

            //Overflow, destroy the least ranking object and return the new one.
            if (NextAvailable > Items.Length - 1) {
                int lowest = Items.IndexOf(Items.OrderBy(x => x.Rank()).First());
                Items[lowest].DeInitialize();
                Items[lowest].Initialize(parameters);
                return Items[lowest];
            }

            //Initalize item object if needed.
            else if (Items[NextAvailable] == null) {
                Items[NextAvailable] = Activator.CreateInstance<T>();
            }

            //Initialize the item and return it.
            Items[NextAvailable].Initialize(parameters);
            return Items[NextAvailable++];

        }

        /// <summary>
        /// Recycle an object so it can be re-used in the pool.
        /// </summary>
        /// <param name="obj">Object to recycle.</param>
        public void RecycleObject(T obj) {

            //Must contain object.
            if (!Items.Contains(obj)) {
                throw new ArgumentException("Object to recycle did not come from the pool.");
            }

            //Get object index.
            int ind = Items.IndexOf(obj);

            //De-initialize.
            obj.DeInitialize();

            //Swap the free space with the last used, and decrement next available since the object can be used.
            Items.Swap(NextAvailable - 1, ind);
            NextAvailable--;

        }

    }

}
