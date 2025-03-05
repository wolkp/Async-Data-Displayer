using Moq;
using NUnit.Framework;
using Zenject;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using System;
using UnityEngine.TestTools;
using System.Collections;
using System.Text.RegularExpressions;

[TestFixture]
public class PanelControllerTests : ZenjectUnitTestFixture
{
    private const string _installerPathPrefix = "Installers/";
    private string _dataInstallerPath = $"{_installerPathPrefix}{nameof(DataInstaller)}";
    private string _signalsInstallerPath = $"{_installerPathPrefix}{nameof(PanelSignalsInstaller)}";
    private string _categoryConfigInstallerPath = $"{_installerPathPrefix}{nameof(CategoryConfigInstaller)}";
    private string _panelInstallerPath = $"{_installerPathPrefix}{nameof(PanelInstaller)}";

    private Mock<IDataServer> _mockDataServer;

    private PanelController _panelController;

    [SetUp]
    public void BindInterfaces()
    {
        DataInstaller.InstallFromResource(_dataInstallerPath, Container);
        PanelSignalsInstaller.InstallFromResource(_signalsInstallerPath, Container);
        CategoryConfigInstaller.InstallFromResource(_categoryConfigInstallerPath, Container);
        PanelInstaller.InstallFromResource(_panelInstallerPath, Container);

        _mockDataServer = new Mock<IDataServer>();
        
        Container.Rebind<IDataServer>().FromInstance(_mockDataServer.Object).AsCached();

        _panelController = Container.Resolve<PanelController>();
    }

    [TearDown]
    public void TearDown()
    {
        Container.UnbindAll();
    }

    [UnityTest]
    public IEnumerator LoadPage_ShouldLoadAndCacheData()
    {
        var mockDataItems = new List<DataItem>
        {
            new DataItem(DataItem.CategoryType.RED, "Test Data 1", false),
            new DataItem(DataItem.CategoryType.GREEN, "Test Data 2", false)
        };

        _mockDataServer.Setup(ds => ds.DataAvailable(It.IsAny<CancellationToken>())).ReturnsAsync(mockDataItems.Count);
        _mockDataServer.Setup(ds => ds.RequestData(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(mockDataItems);

        var loadPageTask = _panelController.LoadPage();
        yield return new WaitUntil(() => loadPageTask.IsCompleted);

        var pageCache = _panelController.GetPageCache();
        Assert.AreEqual(1, pageCache.Count);
        Assert.AreEqual(mockDataItems, pageCache[0]);
    }

    [UnityTest]
    public IEnumerator NavigateNext_ShouldIncreaseIndexAndLoadNextPage()
    {
        var mockDataItems = new List<DataItem> { 
            new DataItem(DataItem.CategoryType.RED, "Test Data 1", false),
            new DataItem(DataItem.CategoryType.RED, "Test Data 2", false),
            new DataItem(DataItem.CategoryType.RED, "Test Data 3", false),
            new DataItem(DataItem.CategoryType.RED, "Test Data 4", false),
            new DataItem(DataItem.CategoryType.RED, "Test Data 5", false),
            new DataItem(DataItem.CategoryType.RED, "Test Data 6", false)
        };

        _mockDataServer.Setup(ds => ds.DataAvailable(It.IsAny<CancellationToken>())).ReturnsAsync(mockDataItems.Count);
        _mockDataServer.Setup(ds => ds.RequestData(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(mockDataItems);

        var loadPageTask = _panelController.LoadPage();
        yield return new WaitUntil(() => loadPageTask.IsCompleted);

        var navigateNextTask = _panelController.NavigateNext();
        yield return new WaitUntil(() => navigateNextTask.IsCompleted);

        Assert.AreEqual(5, _panelController.GetCurrentPageIndex());
    }

    [UnityTest]
    public IEnumerator NavigatePrevious_ShouldDecreaseIndexAndLoadPreviousPage()
    {
        var mockDataItems = new List<DataItem> { new DataItem(DataItem.CategoryType.RED, "Test Data 1", false) };

        _mockDataServer.Setup(ds => ds.DataAvailable(It.IsAny<CancellationToken>())).ReturnsAsync(mockDataItems.Count);
        _mockDataServer.Setup(ds => ds.RequestData(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(mockDataItems);

        var loadPageTask = _panelController.LoadPage();
        yield return new WaitUntil(() => loadPageTask.IsCompleted);

        var navigateNextTask = _panelController.NavigateNext();
        yield return new WaitUntil(() => navigateNextTask.IsCompleted);

        var navigatePreviousTask = _panelController.NavigatePrevious();
        yield return new WaitUntil(() => navigatePreviousTask.IsCompleted);

        Assert.AreEqual(0, _panelController.GetCurrentPageIndex());
    }

    [UnityTest]
    public IEnumerator LoadPage_ShouldHandleErrorAndLogIt()
    {
        _mockDataServer.Setup(ds => ds.DataAvailable(It.IsAny<CancellationToken>()))
                       .ThrowsAsync(new Exception("Data fetch failed"));

        LogAssert.Expect(LogType.Error, new Regex("Error loading data: Data fetch failed"));

        var loadPageTask = _panelController.LoadPage();
        yield return new WaitUntil(() => loadPageTask.IsCompleted);
    }
}