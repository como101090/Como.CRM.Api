namespace Como.CRM.Api.Common;

public static class PermissionCodes
{
    public const string GroupsView = "Groups.View";
    public const string GroupsCreate = "Groups.Create";
    public const string GroupsEdit = "Groups.Edit";
    public const string GroupsDelete = "Groups.Delete";
    public const string GroupsRestore = "Groups.Restore";

    public static readonly IReadOnlyList<string> All = new[]
    {
        GroupsView,
        GroupsCreate,
        GroupsEdit,
        GroupsDelete,
        GroupsRestore
    };
}
