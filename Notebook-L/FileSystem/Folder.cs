using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notebook_L.FileSystem
{
    interface IFolder
    {
        IFolder ParentFolder();
        IEnumerable<IFolder> Folders();
        IEnumerable<IDocument> Documents();
    }
}
