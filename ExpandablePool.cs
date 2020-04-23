using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dreamscape {
    
    /// <summary>
    /// A pool that has no limit and can expand.
    /// </summary>
    public class ExpandablePool<T> : IEnumerable<T> where T : IPoolable {

        /// <summary>
        /// Item pool.
        /// </summary>
        private List<T> Items = new List<T>();

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

            //Add new object.
            if (NextAvailable > Items.Count - 1) {
                Items.Add(Activator.CreateInstance<T>());
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

        public IEnumerator<T> GetEnumerator() {
            return Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return Items.GetEnumerator();
        }
        
        /// <summary>
        /// Get the index of an item.
        /// </summary>
        /// <param name="item">The item to get the index of.</param>
        /// <returns>The item index.</returns>
        public int IndexOf(T item) {
            return Items.IndexOf(item);
        }

    }

}
