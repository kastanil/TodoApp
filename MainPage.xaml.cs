using System.Collections.ObjectModel;
using System.Text.Json;

namespace TodoApp;

public class TaskItem
{
    public string Title { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
}

public partial class MainPage : ContentPage
{
    public ObservableCollection<TaskItem> Tasks { get; set; } = new();
    string filePath = Path.Combine(FileSystem.AppDataDirectory, "tasks.json");

    public MainPage()
    {
        InitializeComponent();
        LoadTasks();
        TasksCollection.ItemsSource = Tasks;
    }

    private void OnAddClicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(TaskEntry.Text))
        {
            Tasks.Add(new TaskItem { Title = TaskEntry.Text, IsCompleted = false });
            TaskEntry.Text = string.Empty;
        }
    }

    private void OnDeleteClicked(object sender, EventArgs e)
    {
        var task = (sender as Button)?.CommandParameter as TaskItem;
        if (task != null) Tasks.Remove(task);
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        var json = JsonSerializer.Serialize(Tasks);
        await File.WriteAllTextAsync(filePath, json);
        await DisplayAlert("Успех", "Список сохранен в AppData", "OK");
    }

    private void LoadTasks()
    {
        if (File.Exists(filePath))
        {
            var json = File.ReadAllText(filePath);
            var loadedTasks = JsonSerializer.Deserialize<List<TaskItem>>(json);
            if (loadedTasks != null)
            {
                Tasks.Clear();
                foreach (var task in loadedTasks) Tasks.Add(task);
            }
        }
    }
}
