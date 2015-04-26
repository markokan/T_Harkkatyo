using System;

namespace Samuxi.WPF.Harjoitus.Model
{
    /// <summary>
    /// Attribute to classes or enums to give info Resource key to find current translation.
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class LocalizationAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the name of the resource.
        /// </summary>
        /// <value>
        /// The name of the resource.
        /// </value>
        public string ResourceName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizationAttribute"/> class.
        /// </summary>
        /// <param name="resourceName">Name of the resource.</param>
        public LocalizationAttribute(string resourceName)
        {
            ResourceName = resourceName;
        }
    }
}
