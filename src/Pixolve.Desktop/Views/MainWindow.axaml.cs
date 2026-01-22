using Avalonia.Controls;
using Avalonia.Input;
using Pixolve.Desktop.ViewModels;
using System.Linq;

namespace Pixolve.Desktop.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        // Wire up the StorageProvider when the window is opened
        Opened += (sender, args) =>
        {
            if (DataContext is MainWindowViewModel viewModel)
            {
                viewModel.SetStorageProvider(StorageProvider);
            }
        };

        // Set up drag & drop
        AddHandler(DragDrop.DropEvent, Drop);
        AddHandler(DragDrop.DragOverEvent, DragOver);
        AddHandler(DragDrop.DragLeaveEvent, DragLeave);
        AddHandler(DragDrop.DragEnterEvent, DragEnter);
    }

    private void DragEnter(object? sender, DragEventArgs e)
    {
        if (DataContext is MainWindowViewModel viewModel)
        {
            viewModel.IsDraggingOver = true;
        }
    }

    private void DragLeave(object? sender, DragEventArgs e)
    {
        if (DataContext is MainWindowViewModel viewModel)
        {
            viewModel.IsDraggingOver = false;
        }
    }

    private void DragOver(object? sender, DragEventArgs e)
    {
        // Only allow copy operation
        e.DragEffects = DragDropEffects.Copy;

        // Only allow if files are present
#pragma warning disable CS0618 // Type or member is obsolete
        if (!e.Data.Contains(DataFormats.Files))
#pragma warning restore CS0618 // Type or member is obsolete
        {
            e.DragEffects = DragDropEffects.None;
        }
    }

    private async void Drop(object? sender, DragEventArgs e)
    {
        if (DataContext is MainWindowViewModel vm)
        {
            vm.IsDraggingOver = false;

#pragma warning disable CS0618 // Type or member is obsolete
            if (e.Data.Contains(DataFormats.Files))
            {
                var files = e.Data.GetFiles()?.Select(f => f.Path.LocalPath).ToArray();
#pragma warning restore CS0618 // Type or member is obsolete

                if (files != null && files.Length > 0)
                {
                    await vm.HandleFilesDropped(files);
                }
            }
        }
    }
}