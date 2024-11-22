const Users = [
  {name: 'Get Devices', url: '/users/get-devices'},
  {name: 'Get Device Types', url: '/users/get-device-types'},
  {name: 'Modify Profile Image', url: '/users/modify-profileImage'},
]

const StandardAdmin = [
  {name: 'Create Admin', url: '/admin/create'},
  {name: 'Delete Admin', url: '/admin/delete'},
  {name: 'Create Company Owner', url: '/admin/company-owner'},
  {name: 'Get Users', url: '/admin/users'},
  {name: 'Get Companies', url: '/admin/companies'}
]

const StandardCompanyOwner = [
  {name: 'Create Company', url: '/company-owner/company'},
  {name: 'Create Camera', url: '/company-owner/camera'},
  {name: 'Create Motion Sensor', url: '/company-owner/motion-sensor'},
  {name: 'Create Smart Lamp', url: '/company-owner/smart-lamp'},
  {name: 'Create Window Sensor', url: '/company-owner/window-sensor'},
  {name: 'Import Devices', url: '/company-owner/import-devices'},
]


export const HomeOwnerElements = [
  ...Users,
  {type: 'divider-big'},
  {name: 'Create Home', url: '/home-owner/create-home'},
  {name: 'Add Device', url: '/home-owner/add-device'},
  {name: 'Add Home Permission', url: '/home-owner/add-home-permission'},
  {name: 'Add Member', url: '/home-owner/add-member'},
  {name: 'Get Home Devices', url: '/home-owner/get-home-devices'},
  {name: 'Get Home Members', url: '/home-owner/get-home-member'},
  {name: 'Modify Home Name', url: '/home-owner/modify-home-name'},
  {type: 'divider'},
  {name: 'Create Room', url: '/home-owner/add-room'},
  {name: 'Add Device To Room', url: '/home-owner/add-device-to-room'},
  {type: 'divider'},
  {name: 'Modify Home Device Name', url: '/home-owner/modify-home-device-name'},
  {name: 'Change Device State', url: '/home-owner/change-device-state'},
  {name: 'Manage lamps', url: '/home-owner/manage-lamp'},
  {type: 'divider'},
  {name: 'Manage member notifications', url: '/home-owner/manage-member-notifications'},
];

export const AdminElements = [
  ...StandardAdmin,
  {name: 'Change To Home Owner Role', url: 'admin/change-role'},
  {type: 'divider-big'},
  ...Users
];

export const CompanyOwnerElements = [
  ...StandardCompanyOwner,
  {name: 'Change To Home Owner Role', url: '/company-owner/change-role'},
  {type: 'divider-big'},
  ...Users
];

export const AdminHomeOwnerElements = [
  ...StandardAdmin,
  {type: 'divider-big'},
  ...HomeOwnerElements
];

export const CompanyHomeOwnerElements = [
  ...StandardCompanyOwner,
  {type: 'divider-big'},
  ...HomeOwnerElements
];
