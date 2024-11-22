import {Component, OnInit} from '@angular/core';
import {NavMenuComponent} from '../../../components/navmenu/navmenu.component';
import {
  AdminElements,
  AdminHomeOwnerElements,
  CompanyHomeOwnerElements,
  CompanyOwnerElements,
  HomeOwnerElements
} from '../elements';
import {SessionService} from '../../../../backend/services/session/session.service';
import {NgIf} from '@angular/common';
import {RoleConstants} from '../../../../globals/roles-constants';
import {Router} from '@angular/router';

@Component({
  selector: 'app-main-menu',
  standalone: true,
  imports: [NavMenuComponent, NgIf],
  templateUrl: './main-menu.component.html',
  styleUrl: './main-menu.component.css'
})
export class MainMenuComponent implements OnInit {
  userRole: string = '';
  name: string = '';
  elements: any = [];
  protected readonly RoleConstants = RoleConstants;

  constructor(private sessionService: SessionService, private router: Router) {
  }

  ngOnInit() {
    this.name = localStorage.getItem('name') || '';
    this.userRole = localStorage.getItem('role') || '';
    switch (this.userRole) {
      case RoleConstants.adminId:
        this.elements = AdminElements
        break;
      case RoleConstants.companyOwnerId:
        this.elements = CompanyOwnerElements
        break;
      case RoleConstants.homeOwnerId:
        this.elements = HomeOwnerElements
        break;
      case RoleConstants.adminHomeOwnerId:
        this.elements = AdminHomeOwnerElements
        break;
      case RoleConstants.companyAndHomeOwnerId:
        this.elements = CompanyHomeOwnerElements
        break;
    }
  }

  getRoleName(): string {
    switch (this.userRole) {
      case RoleConstants.adminId:
        return 'Admin';
      case RoleConstants.companyOwnerId:
        return 'Company Owner';
      case RoleConstants.homeOwnerId:
        return 'Home Owner';
      case RoleConstants.adminHomeOwnerId:
        return 'Admin Home Owner';
      case RoleConstants.companyAndHomeOwnerId:
        return 'Company and Home Owner';
      default:
        return 'Unknown Role';
    }
  }

  navigateToNotifications() {
    this.router.navigate(['/notifications']);
  }

  logout() {
    this.sessionService.logout();
  }
}
