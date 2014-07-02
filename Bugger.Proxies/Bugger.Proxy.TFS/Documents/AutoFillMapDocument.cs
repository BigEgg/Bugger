using Bugger.Proxy.Models;
using Bugger.Proxy.TFS.Models;
using System.Collections.Generic;

namespace Bugger.Proxy.TFS.Documents
{
    public class AutoFillMapDocument
    {
        #region Fields
        private readonly Dictionary<string, AutoFillMapModel> autoFillFields;
        #endregion


        /// <summary>
        /// Initializes a new instance of the <see cref="AutoFillMapDocument"/> class.
        /// </summary>
        public AutoFillMapDocument()
        {
            this.autoFillFields = new Dictionary<string, AutoFillMapModel>();

            foreach (var name in BugHelper.GetPropertyNames())
            {
                this.autoFillFields.Add(name, new AutoFillMapModel());
            }
        }


        /// <summary>
        /// Gets the property's automatic fill fields.
        /// </summary>
        /// <value>
        /// The property's automatic fill fields.
        /// </value>
        public IDictionary<string, AutoFillMapModel> AutoFillFields { get { return this.autoFillFields; } }
    }
}
