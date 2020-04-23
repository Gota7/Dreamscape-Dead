using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dreamscape {
    
    /// <summary>
    /// Something that can be put into a heap.
    /// </summary>
    public interface IPoolable {

        /// <summary>
        /// Initialize the object.
        /// </summary>
        /// <param name="parameters">Initialization parameters.</param>
        void Initialize(object parameters);

        /// <summary>
        /// De-initialize the object when it is about to be initialized by another caller.
        /// </summary>
        void DeInitialize();

        /// <summary>
        /// Rank of the item. If there is no more space available, the item with the lowest rank in the pool will be automatically recycled for the new object.
        /// </summary>
        /// <returns>Object rank.</returns>
        int Rank();

    }

}
