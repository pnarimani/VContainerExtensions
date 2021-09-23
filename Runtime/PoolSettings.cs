using System;

namespace VContainer
{
    [Serializable]
    public class PoolSettings
    {
        /// <summary>
        /// The name of the object which will hold the inactive objects. If empty, no parent object will be created. 
        /// </summary>
        public string ParentName;
        
        /// <summary>
        /// Number of object which will be created on the startup of the factory.
        /// </summary>
        public int PrewarmCount;
        
        /// <summary>
        /// Maximum number of objects which will be kept in the pool. This doesn't include the active objects which are in use.
        /// </summary>
        public int MaxCount;
    }
}