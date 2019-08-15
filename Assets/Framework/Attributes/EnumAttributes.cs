using System;
using UnityEngine;

namespace Framework.Attributes
{
    /// <summary>
    /// Attributes that can be added to the <see cref="Enum"/> class;
    /// </summary>
    public class EnumAttributes
    {
        /// <summary>
        /// This attribute is used to mark an enum varialbe to be drawn by 
        /// the inspector using a custom drawer.
        /// </summary>
        public class EnumFlagAttribute : PropertyAttribute
        {
        }
    }
}