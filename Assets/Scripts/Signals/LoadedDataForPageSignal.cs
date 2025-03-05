using System.Collections.Generic;

public class LoadedDataForPageSignal : ISignal
{
    public readonly IList<DataItem> DataItems;
    public readonly int PageIndex;

    public LoadedDataForPageSignal(IList<DataItem> dataItems, int pageIndex)
    {
        DataItems = dataItems;
        PageIndex = pageIndex;
    }
}