import {Injectable} from '@angular/core';
import environments from '../../environments/environments.local';
import {HttpClient} from '@angular/common/http';
import ApiRepositoryService from './api-repository.service';
import CreateAdminModel from '../services/admin/models/CreateAdminModel';
import DeleteAdminModel from '../services/admin/models/DeleteAdminModel';
import CreateCompanyOwnerModel from '../services/admin/models/CreateCompanyOwnerModel';
import GetUsersModel from '../services/admin/models/GetUsersModel';
import GetCompaniesModel from '../services/admin/models/GetCompanies';

@Injectable({providedIn: 'root',})

export class AdminApiRepositoryService extends ApiRepositoryService {
  constructor(http: HttpClient) {
    super(environments.smartHomeApi, '', http);
  }

  createAdmin(model: CreateAdminModel) {
    return this.post(model, 'admins');
  }

  deleteAdmin(model: DeleteAdminModel) {
    let adminId = model.adminId;
    return this.delete(`admins/${adminId}`, adminId);
  }

  createCompanyOwner(model: CreateCompanyOwnerModel) {
    return this.post(model, 'companyOwners');
  }

  getUsers(model: GetUsersModel) {
    const query = `name=${model.name}&lastName=${model.lastName}&role=${model.role}&offset=${model.offset}&limit=${model.limit}`;
    return this.get('users', query);
  }

  getCompanies(model: GetCompaniesModel) {
    const query = `companyName=${model.companyName}&ownerName=${model.ownerName}&ownerLastName=${model.ownerLastName}&offset=${model.offset}&limit=${model.limit}`;
    return this.get('companies', query);
  }

  changeRole() {
    return this.patch('changeAdminRoles');
  }
}
