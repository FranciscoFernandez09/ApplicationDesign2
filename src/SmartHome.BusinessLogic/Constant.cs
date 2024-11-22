namespace SmartHome.BusinessLogic;

public static class Constant
{
    public const string ValidatorsPathAddedToCurrent = "Validators";
    public const string ImportersPathAddedToCurrent = "Importers";

    #region Permissions

    #region Strings

    // Admin
    public const string CreateAdmin = "11111111-1111-1111-1111-1234567890ab";
    public const string DeleteAdmin = "22222222-2222-2222-2222-1234567890ab";
    public const string CreateCompanyOwner = "33333333-3333-3333-3333-1234567890ab";
    public const string GetUsers = "44444444-4444-4444-4444-1234567890ab";
    public const string GetCompanies = "55555555-5555-5555-5555-1234567890ab";
    public const string AdminModifyRole = "90123456-9012-9012-9012-1234567890ab";

    // CompanyOwner
    public const string CreateCompany = "66666666-6666-6666-6666-1234567890ab";
    public const string CreateDevice = "77777777-7777-7777-7777-1234567890ab";
    public const string CompanyOwnerModifyRole = "01234567-0123-0123-0123-1234567890ab";

    // HomeOwner
    public const string CreateHome = "99999999-9999-9999-9999-1234567890ab";
    public const string AddHomePermission = "12345678-1234-1234-1234-1234567890ab";
    public const string GetNotifications = "23456789-2345-2345-2345-1234567890ab";
    public const string AddMember = "34567890-3456-3456-3456-1234567890ab";
    public const string AddSmartDevice = "45678901-4567-4567-4567-1234567890ab";
    public const string GetHomeMembers = "56789012-5678-5678-5678-1234567890ab";
    public const string GetHomeDevices = "67890123-6789-6789-6789-1234567890ab";
    public const string ActivateNotification = "78901234-7890-7890-7890-1234567890ab";
    public const string DeactivateNotification = "89012345-8901-8901-8901-1234567890ab";
    public const string ModifyHomeName = "12345678-1234-1234-1234-1234567aaaab";
    public const string ModifyHomeDeviceName = "b3e1a8d2-4c5f-4b8e-9a2e-1f3b5d6a7c8e";
    public const string AddRoom = "b3eee8d2-4c5f-4b8e-9a2e-1f3b5d6a7c8e";
    public const string AddDeviceToRoom = "b3eee8dd-4c5f-4b8e-9a2e-1f3b5d6a7c8e";
    public const string GetHomes = "d2f1e8a4-3b6e-4b8e-9a2e-1f3b5d6a7c8e";

    // Roles
    private const string AdminRole = "12345678-1234-1234-1234-1234567890ab";
    private const string CompanyOwnerRole = "23456789-2345-2345-2345-1234567890ab";
    private const string HomeOwnerRole = "34567890-3456-3456-3456-1234567890ab";
    private const string AdminHomeOwnerRole = "45678901-4567-4567-4567-123456789aab";
    private const string CompanyAndHomeOwnerRole = "41111111-4567-4567-4567-123456789aab";

    // System Permission
    private const string CreateAdminSystem = "08129fcb-bb32-4440-9d6d-daf74040c137";
    private const string DeleteAdminSystem = "0b66a103-82fe-4b3c-8046-e7c09f9ff094";
    private const string CreateCompanyOwnerSystem = "10af3fc3-7a3f-4c21-b535-2e15f231caf3";
    private const string GetUsersSystem = "13fbcb26-eccc-41cb-a4d0-203c8039aeab";
    private const string GetCompaniesSystem = "5030f84f-e0fb-43ba-b3b8-3363dccf213d";
    private const string AdminModifyRoleSystem = "f7b3b3b3-3b3b-3b3b-3b3b-3b3b3b3b3b3b";

    private const string CreateCompanySystem = "5ee2f16e-8c00-494a-98b4-ca24e354ec35";
    private const string CreateDeviceSystem = "8bd3fb56-6430-48e1-b955-8b6fcd7f6544";
    private const string CompanyOwnerModifyRoleSystem = "f7a3a3b3-3b3b-3b3b-3b3b-3b3b3b3b3b3b";

    private const string CreateHomeSystem = "9f407a6c-95c2-4fee-bdc6-4a8aeb8181e9";
    private const string AddHomePermissionSystem = "a01983cf-6fc8-479f-9eba-7c0647e9cd5f";
    private const string GetNotificationsSystem = "a6a4097e-eb63-41ba-9d57-c7df777e3218";
    private const string AddMemberSystem = "b65f7a2f-a549-4fa2-974d-d6e073707ded";
    private const string AddSmartDeviceSystem = "d03f54ec-1562-4c3f-a763-02da0fb8d2f3";
    private const string GetHomeMembersSystem = "d1e45413-f67f-46c5-ac84-71881356c307";
    private const string GetHomeDevicesSystem = "d619e786-fd3e-47e4-8866-a091db79f523";
    private const string ActivateNotificationSystem = "da63d420-8bce-44dc-b96f-da3f9e4bf1bc";
    private const string DeactivateNotificationSystem = "fe965e49-84a3-4cc6-aed0-a1ea2ed39562";
    private const string ModifyHomeNameSystem = "ffa3a3b3-3b3b-3b3b-3b3b-3b3b3b3b3b3b";
    private const string ModifyHomeDeviceNameSystem = "bbb1a8d2-4c5f-4b8e-9a2e-1f3b5d6a7c8e";
    private const string AddRoomSystem = "b3eeeed2-4c5f-4b8e-9a2e-1f3b5d6a7c8e";
    private const string AddDeviceToRoomSystem = "b3eeeed2-4454-4b8e-9a2e-1f3b5d6a7c8e";
    private const string GetHomesSystem = "e3b0c442-98fc-1c14-9ddf-4b6e6e6e6e6e";

    private const string ACreateAdminSystem = "08129fcb-bb32-4440-aaaa-daf74040c137";
    private const string ADeleteAdminSystem = "0b66a103-82fe-4b3c-aaaa-e7c09f9ff094";
    private const string ACreateCompanyOwnerSystem = "10af3fc3-7a3f-4c21-aaaa-2e15f231caf3";
    private const string AGetUsersSystem = "13fbcb26-eccc-41cb-aaaa-203c8039aeab";
    private const string AGetCompaniesSystem = "5030f84f-e0fb-43ba-aaaa-3363dccf213d";
    private const string ACreateHomeSystem = "9f407a6c-95c2-4fee-aaaa-4a8aeb8181e9";
    private const string AAddHomePermissionSystem = "a01983cf-6fc8-479f-aaaa-7c0647e9cd5f";
    private const string AGetNotificationsSystem = "a6a4097e-eb63-41ba-aaaa-c7df777e3218";
    private const string AAddMemberSystem = "b65f7a2f-a549-4fa2-aaaa-d6e073707ded";
    private const string AAddSmartDeviceSystem = "d03f54ec-1562-4c3f-aaaa-02da0fb8d2f3";
    private const string AGetHomeMembersSystem = "d1e45413-f67f-46c5-aaaa-71881356c307";
    private const string AGetHomeDevicesSystem = "d619e786-fd3e-47e4-aaaa-a091db79f523";
    private const string AActivateNotificationSystem = "da63d420-8bce-44dc-aaaa-da3f9e4bf1bc";
    private const string ADeactivateNotificationSystem = "fe965e49-84a3-4cc6-aaaa-a1ea2ed39562";
    private const string AModifyHomeNameSystem = "ffa3a3b3-3b3b-3b3b-aaaa-3b3b3b3b3b3b";
    private const string AModifyHomeDeviceNameSystem = "bbb1a8d2-4c5f-4b8e-aaaa-1f3b5d6a7c8e";
    private const string AAddRoomSystem = "b3eeeed2-4c5f-4b8e-aaaa-1f3b5d6a7c8e";
    private const string AAddDeviceToRoomSystem = "b3eeeed2-4454-4b8e-aaaa-1f3b5d6a7c8e";
    private const string AGetHomesSystem = "f47ac10b-58cc-4372-aaaa-0e02b2c3d479";

    private const string CCreateCompanySystem = "5ee2f16e-8c00-494a-cccc-ca24e354ec35";
    private const string CCreateDeviceSystem = "8bd3fb56-6430-48e1-cccc-8b6fcd7f6544";
    private const string CCreateHomeSystem = "9f407a6c-95c2-4fee-cccc-4a8aeb8181e9";
    private const string CAddHomePermissionSystem = "a01983cf-6fc8-479f-cccc-7c0647e9cd5f";
    private const string CGetNotificationsSystem = "a6a4097e-eb63-41ba-cccc-c7df777e3218";
    private const string CAddMemberSystem = "b65f7a2f-a549-4fa2-cccc-d6e073707ded";
    private const string CAddSmartDeviceSystem = "d03f54ec-1562-4c3f-cccc-02da0fb8d2f3";
    private const string CGetHomeMembersSystem = "d1e45413-f67f-46c5-cccc-71881356c307";
    private const string CGetHomeDevicesSystem = "d619e786-fd3e-47e4-cccc-a091db79f523";
    private const string CActivateNotificationSystem = "da63d420-8bce-44dc-cccc-da3f9e4bf1bc";
    private const string CDeactivateNotificationSystem = "fe965e49-84a3-4cc6-cccc-a1ea2ed39562";
    private const string CModifyHomeNameSystem = "ffa3a3b3-3b3b-3b3b-cccc-3b3b3b3b3b3b";
    private const string CModifyHomeDeviceNameSystem = "bbb1a8d2-4c5f-4b8e-cccc-1f3b5d6a7c8e";
    private const string CAddRoomSystem = "b3eeeed2-4c5f-4b8e-cccc-1f3b5d6a7c8e";
    private const string CAddDeviceToRoomSystem = "b3eeeed2-4454-4b8e-cccc-1f3b5d6a7c8e";
    private const string CGetHomesSystem = "a1b2c3d4-e5f6-7890-cccc-ef1234567890";

    #endregion

    #region Guids

    // Admin
    public static readonly Guid CreateAdminId = Guid.Parse(CreateAdmin);
    public static readonly Guid DeleteAdminId = Guid.Parse(DeleteAdmin);
    public static readonly Guid CreateCompanyOwnerId = Guid.Parse(CreateCompanyOwner);
    public static readonly Guid GetUsersId = Guid.Parse(GetUsers);
    public static readonly Guid GetCompaniesId = Guid.Parse(GetCompanies);
    public static readonly Guid AdminModifyRoleId = Guid.Parse(AdminModifyRole);

    // CompanyOwner
    public static readonly Guid CreateCompanyId = Guid.Parse(CreateCompany);
    public static readonly Guid CreateDeviceId = Guid.Parse(CreateDevice);
    public static readonly Guid CompanyOwnerModifyRoleId = Guid.Parse(CompanyOwnerModifyRole);

    // HomeOwner
    public static readonly Guid CreateHomeId = Guid.Parse(CreateHome);
    public static readonly Guid AddHomePermissionId = Guid.Parse(AddHomePermission);
    public static readonly Guid GetNotificationsId = Guid.Parse(GetNotifications);
    public static readonly Guid AddMemberId = Guid.Parse(AddMember);
    public static readonly Guid AddSmartDeviceId = Guid.Parse(AddSmartDevice);
    public static readonly Guid GetHomeMembersId = Guid.Parse(GetHomeMembers);
    public static readonly Guid GetHomeDevicesId = Guid.Parse(GetHomeDevices);
    public static readonly Guid ActivateNotificationId = Guid.Parse(ActivateNotification);
    public static readonly Guid DeactivateNotificationId = Guid.Parse(DeactivateNotification);
    public static readonly Guid ModifyHomeNameId = Guid.Parse(ModifyHomeName);
    public static readonly Guid ModifyHomeDeviceNameId = Guid.Parse(ModifyHomeDeviceName);
    public static readonly Guid AddRoomId = Guid.Parse(AddRoom);
    public static readonly Guid AddDeviceToRoomId = Guid.Parse(AddDeviceToRoom);
    public static readonly Guid GetHomesId = Guid.Parse(GetHomes);

    // Roles
    public static readonly Guid AdminRoleId = Guid.Parse(AdminRole);
    public static readonly Guid CompanyOwnerRoleId = Guid.Parse(CompanyOwnerRole);
    public static readonly Guid HomeOwnerRoleId = Guid.Parse(HomeOwnerRole);
    public static readonly Guid AdminHomeOwnerRoleId = Guid.Parse(AdminHomeOwnerRole);
    public static readonly Guid CompanyAndHomeOwnerRoleId = Guid.Parse(CompanyAndHomeOwnerRole);

    // System Permission
    public static readonly Guid CreateAdminSystemId = Guid.Parse(CreateAdminSystem);
    public static readonly Guid DeleteAdminSystemId = Guid.Parse(DeleteAdminSystem);
    public static readonly Guid CreateCompanyOwnerSystemId = Guid.Parse(CreateCompanyOwnerSystem);
    public static readonly Guid GetUsersSystemId = Guid.Parse(GetUsersSystem);
    public static readonly Guid GetCompaniesSystemId = Guid.Parse(GetCompaniesSystem);
    public static readonly Guid AdminModifyRoleSystemId = Guid.Parse(AdminModifyRoleSystem);

    public static readonly Guid CreateCompanySystemId = Guid.Parse(CreateCompanySystem);
    public static readonly Guid CreateDeviceSystemId = Guid.Parse(CreateDeviceSystem);
    public static readonly Guid CompanyOwnerModifyRoleSystemId = Guid.Parse(CompanyOwnerModifyRoleSystem);

    public static readonly Guid CreateHomeSystemId = Guid.Parse(CreateHomeSystem);
    public static readonly Guid AddHomePermissionSystemId = Guid.Parse(AddHomePermissionSystem);
    public static readonly Guid GetNotificationsSystemId = Guid.Parse(GetNotificationsSystem);
    public static readonly Guid AddMemberSystemId = Guid.Parse(AddMemberSystem);
    public static readonly Guid AddSmartDeviceSystemId = Guid.Parse(AddSmartDeviceSystem);
    public static readonly Guid GetHomeMembersSystemId = Guid.Parse(GetHomeMembersSystem);
    public static readonly Guid GetHomeDevicesSystemId = Guid.Parse(GetHomeDevicesSystem);
    public static readonly Guid ActivateNotificationSystemId = Guid.Parse(ActivateNotificationSystem);
    public static readonly Guid DeactivateNotificationSystemId = Guid.Parse(DeactivateNotificationSystem);
    public static readonly Guid ModifyHomeNameSystemId = Guid.Parse(ModifyHomeNameSystem);
    public static readonly Guid ModifyHomeDeviceNameSystemId = Guid.Parse(ModifyHomeDeviceNameSystem);
    public static readonly Guid AddRoomSystemId = Guid.Parse(AddRoomSystem);
    public static readonly Guid AddDeviceToRoomSystemId = Guid.Parse(AddDeviceToRoomSystem);
    public static readonly Guid GetHomesSystemId = Guid.Parse(GetHomesSystem);

    public static readonly Guid ACreateAdminSystemId = Guid.Parse(ACreateAdminSystem);
    public static readonly Guid ADeleteAdminSystemId = Guid.Parse(ADeleteAdminSystem);
    public static readonly Guid ACreateCompanyOwnerSystemId = Guid.Parse(ACreateCompanyOwnerSystem);
    public static readonly Guid AGetUsersSystemId = Guid.Parse(AGetUsersSystem);
    public static readonly Guid AGetCompaniesSystemId = Guid.Parse(AGetCompaniesSystem);
    public static readonly Guid ACreateHomeSystemId = Guid.Parse(ACreateHomeSystem);
    public static readonly Guid AAddHomePermissionSystemId = Guid.Parse(AAddHomePermissionSystem);
    public static readonly Guid AGetNotificationsSystemId = Guid.Parse(AGetNotificationsSystem);
    public static readonly Guid AAddMemberSystemId = Guid.Parse(AAddMemberSystem);
    public static readonly Guid AAddSmartDeviceSystemId = Guid.Parse(AAddSmartDeviceSystem);
    public static readonly Guid AGetHomeMembersSystemId = Guid.Parse(AGetHomeMembersSystem);
    public static readonly Guid AGetHomeDevicesSystemId = Guid.Parse(AGetHomeDevicesSystem);
    public static readonly Guid AActivateNotificationSystemId = Guid.Parse(AActivateNotificationSystem);
    public static readonly Guid ADeactivateNotificationSystemId = Guid.Parse(ADeactivateNotificationSystem);
    public static readonly Guid AModifyHomeNameSystemId = Guid.Parse(AModifyHomeNameSystem);
    public static readonly Guid AModifyHomeDeviceNameSystemId = Guid.Parse(AModifyHomeDeviceNameSystem);
    public static readonly Guid AAddRoomSystemId = Guid.Parse(AAddRoomSystem);
    public static readonly Guid AAddDeviceToRoomSystemId = Guid.Parse(AAddDeviceToRoomSystem);
    public static readonly Guid AGetHomesSystemId = Guid.Parse(AGetHomesSystem);

    public static readonly Guid CCreateCompanySystemId = Guid.Parse(CCreateCompanySystem);
    public static readonly Guid CCreateDeviceSystemId = Guid.Parse(CCreateDeviceSystem);
    public static readonly Guid CCreateHomeSystemId = Guid.Parse(CCreateHomeSystem);
    public static readonly Guid CAddHomePermissionSystemId = Guid.Parse(CAddHomePermissionSystem);
    public static readonly Guid CGetNotificationsSystemId = Guid.Parse(CGetNotificationsSystem);
    public static readonly Guid CAddMemberSystemId = Guid.Parse(CAddMemberSystem);
    public static readonly Guid CAddSmartDeviceSystemId = Guid.Parse(CAddSmartDeviceSystem);
    public static readonly Guid CGetHomeMembersSystemId = Guid.Parse(CGetHomeMembersSystem);
    public static readonly Guid CGetHomeDevicesSystemId = Guid.Parse(CGetHomeDevicesSystem);
    public static readonly Guid CActivateNotificationSystemId = Guid.Parse(CActivateNotificationSystem);
    public static readonly Guid CDeactivateNotificationSystemId = Guid.Parse(CDeactivateNotificationSystem);
    public static readonly Guid CModifyHomeNameSystemId = Guid.Parse(CModifyHomeNameSystem);
    public static readonly Guid CModifyHomeDeviceNameSystemId = Guid.Parse(CModifyHomeDeviceNameSystem);
    public static readonly Guid CAddRoomSystemId = Guid.Parse(CAddRoomSystem);
    public static readonly Guid CAddDeviceToRoomSystemId = Guid.Parse(CAddDeviceToRoomSystem);
    public static readonly Guid CGetHomesSystemId = Guid.Parse(CGetHomesSystem);

    #endregion

    #endregion
}
