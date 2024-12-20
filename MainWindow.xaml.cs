using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace TreeViewCollapsingIssue;

/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MainWindow : Window
{
    public TreeViewTestGroupItem[] GroupsItemSource { get; }

    public ObservableCollection<TreeViewTestMemberItem> MembersItemSource { get; } =
    [
        new("Group Member"),
        new("Group Member"),
        new("Group Member")
    ];

    public MainWindow()
    {
        InitializeComponent();

        GroupsItemSource =
        [
            new TreeViewTestGroupItem("Group", MembersItemSource),
        ];

        MembersItemSource.CollectionChanged += (s, e) =>
        {
            Debug.WriteLine($"CollectionChanged: <{e.Action}>");
        };
    }

    private void Button_Click_1(object sender, RoutedEventArgs e)
    {
        if (MembersItemSource.Count == 0)
        {
            return;
        }

        // Using "MembersItemSource.Clear();" instead will not cause any issues.
        MembersItemSource.RemoveAt(0);
    }

    private void Button_Click_2(object sender, RoutedEventArgs e)
    {
        if (MembersItemSource.Count > 2)
        {
            return;
        }

        MembersItemSource.Add(new($"Group Member"));
    }
}

public sealed class TreeViewTestGroupItem(
    string title,
    ObservableCollection<TreeViewTestMemberItem> membersItemSource)
{
    public string Title { get; } = title;

    public ObservableCollection<TreeViewTestMemberItem> MembersItemSource { get; } = membersItemSource;
}

public sealed class TreeViewTestMemberItem(string name)
{
    public string Name { get; } = name;
}

public sealed class TreeViewTemplateSelector : DataTemplateSelector
{
    public DataTemplate? GroupTemplate { get; set; }

    public DataTemplate? MemberTemplate { get; set; }

    protected override DataTemplate? SelectTemplateCore(object item)
    {
        return item switch
        {
            TreeViewTestGroupItem => GroupTemplate,
            TreeViewTestMemberItem => MemberTemplate,
            _ => throw new ArgumentOutOfRangeException(nameof(item))
        };
    }
}