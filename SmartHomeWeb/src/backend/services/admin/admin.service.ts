import {Injectable} from '@angular/core';
import {AdminApiRepositoryService} from '../../repositories/admin-api-repository.service';
import CreateAdminModel from './models/CreateAdminModel';
import DeleteAdminModel from './models/DeleteAdminModel';
import CreateCompanyOwnerModel from './models/CreateCompanyOwnerModel';
import GetUsersModel from './models/GetUsersModel';
import GetCompaniesModel from './models/GetCompanies';

@Injectable({
  providedIn: 'root',
})
export class AdminService {
  constructor(private readonly _repository: AdminApiRepositoryService) {
  }

  public createAdmin(request: CreateAdminModel) {
    return this._repository.createAdmin(request);
  }

  public deleteAdmin(request: DeleteAdminModel) {
    return this._repository.deleteAdmin(request);
  }

  public createCompanyOwner(request: CreateCompanyOwnerModel) {
    return this._repository.createCompanyOwner(request);
  }

  public getUsers(request: GetUsersModel) {
    return this._repository.getUsers(request);
  }

  public getCompanies(request: GetCompaniesModel) {
    return this._repository.getCompanies(request);
  }

  public changeRole() {
    return this._repository.changeRole();
  }
}
