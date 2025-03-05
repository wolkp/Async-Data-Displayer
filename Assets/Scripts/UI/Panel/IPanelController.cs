using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

public interface IPanelController
{
    Task LoadPage();
    Task NavigateNext();
    Task NavigatePrevious();
    ReadOnlyDictionary<int, IList<DataItem>> GetPageCache();
    int GetCurrentPageIndex();
}