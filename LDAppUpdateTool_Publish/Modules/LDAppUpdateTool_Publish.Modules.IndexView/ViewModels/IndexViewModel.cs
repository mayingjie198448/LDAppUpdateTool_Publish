using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using LDAppUpdateTool_Publish.Modules.IndexView.Models;
using LDAppUpdateTool_Publish.Services.InterFaces;
using LDAppUpdateTool_Publish.Share;
using LDAppUpdateTool_Publish.Share.EventModels;
using LDAppUpdateTool_Publish.Share.Models;
using Ookii.Dialogs.Wpf;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;

namespace LDAppUpdateTool_Publish.Modules.IndexView.ViewModels;

public class IndexViewModel : BindableBase
{
    private readonly IEventAggregator _eventAggregator;

    private readonly IUpdateService _updateService;

    private string _binPath;

    private DirectoryNodeModel _directoryNode;

    private string _appVersion;

    private string _zipPath;

    private ObservableCollection<string> _files;

    private ObservableCollection<OperateFileModel> _addOrModifiedFiles;

    private ObservableCollection<OperateFileModel> _deletedFiles;

    private ObservableCollection<string> _selectedFiles;

    private ObservableCollection<OperateFileModel> _operateFiles;

    public IndexViewModel(IEventAggregator eventAggregator, IUpdateService updateService)
    {
        _eventAggregator = eventAggregator;
        _updateService = updateService;
    }

    public string AppVersion {
        get => _appVersion;
        set => SetProperty(ref _appVersion, value);
    }

    public string ZipPath {
        get => _zipPath;
        set => SetProperty(ref _zipPath, value);
    }

    public string BinPath {
        get => _binPath;
        set => SetProperty(ref _binPath, value);
    }

    public DirectoryNodeModel DirectoryNode {
        get => _directoryNode;
        set {
            SetProperty(ref _directoryNode, value);
            MessageBox.Show($"文件夹 {value.DirectoryName} 文件数量 {value.Files.Count} 加载完成。");
        }
    }

    public ObservableCollection<string> SelectedFiles {
        get => _selectedFiles ??= new();
        set => SetProperty(ref _selectedFiles, value);
    }

    public ObservableCollection<string> Files {
        get => _files ??= new();
        set => SetProperty(ref _files, value);
    }

    public ObservableCollection<OperateFileModel> AddOrModifiedFiles {
        get => _addOrModifiedFiles ??= new();
        set => SetProperty(ref _addOrModifiedFiles, value);
    }

    public ObservableCollection<OperateFileModel> DeletedFiles {
        get => _deletedFiles ??= new();
        set => SetProperty(ref _deletedFiles, value);
    }

    public ObservableCollection<OperateFileModel> OperateFiles {
        get => _operateFiles ??= new();
        set => SetProperty(ref _operateFiles, value);
    }


    public DelegateCommand SelectBinPathCommand => new(SelectBinPath);

    public DelegateCommand AddFilesCommand => new DelegateCommand(AddFiles);

    public DelegateCommand DeleteFilesCommand => new DelegateCommand(DeleteFiles);
    
    public DelegateCommand StartUpdateCommand => new DelegateCommand(StartUpdate);

    public DelegateCommand SelectZipPathCommand => new DelegateCommand(SelectZipPath);

    private void SelectZipPath()
    {
        var dialog = new VistaFolderBrowserDialog();
        if ((bool) dialog.ShowDialog()!)
        {
            ZipPath = dialog.SelectedPath;
        }
    }

    private async void StartUpdate()
    {
        var updateData = AppShareData.Instance!.UpdateData;
        updateData.AppVersion = AppVersion;
        updateData.AddOrUpdateFiles = AddOrModifiedFiles.Select(file => file.FileName).ToList();
        updateData.DeleteFiles = DeletedFiles.Select(file => file.FileName).ToList();


        
        updateData.Hash = _updateService.GetZipFileHash(Path.Combine(ZipPath, AppConstant.ZipFileName));
        var createConfigTask = new Task(() => _updateService.CreateUpdateConfigFile(ZipPath));
        var createZipTask = new Task(() => _updateService.CreateAddOrUpdateZipFile(ZipPath));
        
        createConfigTask.Start();
        createZipTask.Start();
        await Task.WhenAll(createConfigTask, createZipTask);
        MessageBox.Show("创建更新文件完成");
    }

    private void DeleteFiles()
    {
        this.HasOperateFiles();
        DeletedFiles.AddRange(SelectedFiles.Select(file =>
            new OperateFileModel() {FileName = file, BackgroundColor = AppConstant.ErrorColor,}));
        Files = new(Files.Except(SelectedFiles));
        AddIntoOperateFiles();
        foreach (var item in DeletedFiles)
        {
            _eventAggregator.GetEvent<LogEventModel>().Publish(new LogModel()
            {
                Message = $"删除文件 {item.FileName}",
                BackgroundColor = AppConstant.SuccessColor
            });
        }
    }

    private void AddIntoOperateFiles()
    {
        OperateFiles.Clear();
        OperateFiles.AddRange(AddOrModifiedFiles.Select(file => file));
        OperateFiles.AddRange(DeletedFiles.Select(file => file));


    }


    private void AddFiles()
    {
        this.HasOperateFiles();
        AddOrModifiedFiles.AddRange(SelectedFiles.Select(file =>
            new OperateFileModel() {FileName = file, BackgroundColor = AppConstant.SuccessColor,}));
        Files = new(Files.Except(SelectedFiles));
        AddIntoOperateFiles();
        foreach (var item in AddOrModifiedFiles)
        {
            _eventAggregator.GetEvent<LogEventModel>().Publish(new LogModel()
            {
                Message = $"添加文件 {item.FileName}",
                BackgroundColor = AppConstant.SuccessColor
            });
        }
    }

    private void SelectBinPath()
    {
        // 目录选择对话框，选择应用程序目录
        var dialog = new VistaFolderBrowserDialog();
        if ((bool) dialog.ShowDialog()!)
        {
            BinPath = dialog.SelectedPath;
            AppShareData.BinPath = BinPath;
            // var directoryNode = new DirectoryNodeModel(BinPath);
            //
            // directoryNode.LoadSubDirectoriesAndFiles();
            // DirectoryNode = directoryNode;

            Files = new ObservableCollection<string>(GetFiles(BinPath));
        }
    }

    private List<string> GetFiles(string path)
    {
        var files = new List<string>();
        files.AddRange(Directory.GetFiles(path).Select(file => file.Replace(BinPath, "").TrimStart('\\')));
        foreach (var dir in new DirectoryInfo(path).GetDirectories())
        {
            files.AddRange(GetFiles(dir.FullName).Select(file => file.Replace(BinPath, "")));
        }

        return files;
    }

    // 执行文件操作前，先检查SelectedFiles是否为空
    private bool HasOperateFiles()
    {
        if (SelectedFiles.Count != 0)
            return true;
        _eventAggregator.GetEvent<LogEventModel>().Publish(new LogModel() {
            Message = "请选择文件",
            BackgroundColor = AppConstant.ErrorColor
        });
        return false;
    }
}
