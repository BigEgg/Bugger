namespace Bugger.Documents
{
    /// <summary>
    /// The document type interface to save and open the document
    /// </summary>
    /// <typeparam name="T">The document's type.</typeparam>
    public interface IDocumentType<T> where T : IDocument
    {
        /// <summary>
        /// Opens the document with the specified file name.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        /// <returns>The document</returns>
        T Open(string fileName);

        /// <summary>
        /// Saves the document with the specified file name.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        /// <param name="document">The document.</param>
        void Save(string fileName, T document);
    }
}
