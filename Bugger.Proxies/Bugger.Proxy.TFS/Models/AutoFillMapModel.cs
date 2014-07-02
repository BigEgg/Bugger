using System.Collections.Generic;

namespace Bugger.Proxy.TFS.Models
{
    public class AutoFillMapModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AutoFillMapModel"/> class.
        /// </summary>
        public AutoFillMapModel()
        {
            FieldsName = new List<string>();
            IsMandatory = true;
        }


        /// <summary>
        /// Gets the fields name that can be auto mapped.
        /// </summary>
        /// <value>
        /// The fields name that can be auto mapped.
        /// </value>
        public IList<string> FieldsName { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether this property is mandatory for mapping.
        /// </summary>
        /// <value>
        /// <c>true</c> if this property is mandatory for mapping; otherwise, <c>false</c>.
        /// </value>
        public bool IsMandatory { get; internal set; }
    }
}
