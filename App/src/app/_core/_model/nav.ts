export class Nav {
    id: number;
    title: string;
    icon: string;
    status: boolean;
    type: string;
    Nav = [
        {
            id: 1,
            title: 'Account type',
            path: '/system/account-type',
            icon: 'fas fa-atlas',
            status: true,
            index: 1,
            type: 'admin',
        },
        {
            id: 2,
            title: 'Account role',
            path: '/system/account-role',
            icon: 'fas fa-cog',
            status: true,
            index: 2,
            type: 'admin',
        },
        {
            id: 3,
            title: 'Account group',
            path: '/system/account-group',
            icon: 'fas fa-users',
            status: true,
            index: 3,
            type: 'admin',
        },
        {
            id: 4,
            title: 'Account permission',
            path: '/system/account-permission',
            icon: 'fas fa-user',
            status: true,
            index: 4,
            type: 'admin',
        },
        {
            id: 5,
            title: 'Employee',
            path: '/system/employee',
            icon: 'fas fa-satellite-dish',
            status: true,
            index: 5,
            type: 'admin',
        },
        {
          id: 6,
          title: 'Account',
          path: '/system/account',
          icon: 'fas fa-user-cog',
          status: true,
          index: 6,
          type: 'admin',
        },
        {
          id: 7,
          title: '2. Farm setting',
          path: '/system/farm-setting',
          icon: 'fas fa-tools',
          status: true,
          index: 7,
          type: 'FarmSetting',
        },
        {
          id: 8,
          title: 'Pig Kind',
          path: '/system/pig-kind',
          icon: 'fas fa-piggy-bank',
          status: true,
          index: 8,
          type: 'BOM',
        },
        {
          id: 9,
          title: 'Vaccine',
          path: '/system/vaccine',
          icon: 'fas fa-bug',
          status: true,
          index: 9,
          type: 'BOM',
        },
        {
          id: 10,
          title: 'Vaccine method',
          path: '/system/method',
          icon: 'fas fa-cog',
          status: true,
          index: 10,
          type: 'BOM',
        },
        {
          id: 11,
          title: 'Food',
          path: '/system/food',
          icon: 'fas fa-hamburger',
          status: true,
          index: 11,
          type: 'BOM',
        },
        {
          id: 12,
          title: 'Feeding',
          path: '/system/feeding',
          icon: 'fas fa-shopping-cart',
          status: true,
          index:12,
          type: 'BOM',
        },
        {
          id: 13,
          title: 'Setup',
          path: '/system/setup',
          icon: 'fas fa-tasks',
          status: true,
          index: 13,
          type: 'BOM',
        },
        {
          id: 14,
          title: '0. System Language',
          path: '/system/system-language',
          icon: 'fas fa-cogs',
          status: true,
          index: 14,
          type: 'Setting',
        },
        {
          id: 15,
          title: '0. Function System',
          path: '/system/function-system',
          icon: 'fas fa-cogs',
          status: true,
          index: 15,
          type: 'admin',
        },
    ];
    constructor() {}
    // constructor(id, title, icon, status) {
    //     this.id = id;
    //     this.title = title;
    //     this.icon = icon;
    //     this.status = status;
    // }
    getFarmSetting() {
      return this.Nav.filter(x => x.type == 'FarmSetting')[0];
    }
    getBOM() {
      return this.Nav.filter(x => x.type == 'BOM');
    }
    getAdmin() {
      return this.Nav.filter(x => x.type == 'admin');
    }
    getSetting() {
      return this.Nav.filter(x => x.type == 'Setting')[0];
    }
    getNavAdmin(showDashboard = false) {
        if (showDashboard) {
            return this.Nav.filter(this.isAdmin);
        } else {
            return this.Nav.filter(this.isAdminShowDash);
        }
    }
    getNavClient() {
        return this.Nav.filter(this.isClient);
    }
    getNavEc() {
        return this.Nav.filter(this.isEc);
    }
    private isAdminShowDash(element, index, array) {
        return (element.type === 'admin');
    }
    private isAdmin(element, index, array) {
        return (element.type === 'admin' && element.title !== 'Home');
    }
    private isClient(element, index, array) {
        return (element.type === 'client');
    }
    private isEc(element, index, array) {
        return (element.type === 'ec');
    }
}


