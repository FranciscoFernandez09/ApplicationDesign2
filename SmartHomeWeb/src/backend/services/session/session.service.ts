import {Injectable} from "@angular/core";
import {SessionApiRepositoryService} from "../../repositories/session-api-repository.service";
import {BehaviorSubject, Observable, tap} from "rxjs";
import UserCredentialsModel from "./models/UserCredentialsModel";
import SessionCreatedModel from "./models/SessionCreatedModel";
import UserLoggedModel from "./models/UserLoggedModel";

@Injectable({
  providedIn: "root",
})
export class SessionService {
  private readonly _userLogged$: BehaviorSubject<UserLoggedModel | null> =
    new BehaviorSubject<UserLoggedModel | null>(null);

  constructor(private readonly _repository: SessionApiRepositoryService) {
  }

  get userLogged(): Observable<UserLoggedModel | null> {
    if (!this._userLogged$.value) {
      const token = localStorage.getItem("token");

      if (token) {
        const role = localStorage.getItem("role") || "";
        this._userLogged$.next({token, role});
      }
    }

    return this._userLogged$.asObservable();
  }

  public login(credentials: UserCredentialsModel): Observable<SessionCreatedModel> {
    return this._repository.login(credentials).pipe(
      tap((response) => {
        localStorage.setItem('isLoggedIn', 'true');
        localStorage.setItem("token", response.token);
        localStorage.setItem("role", response.role);
        localStorage.setItem("name", response.name);
        this._userLogged$.next(response);
      })
    );
  }

  public logout(): void {
    this._repository.logout();
    localStorage.removeItem('isLoggedIn');
    localStorage.removeItem('token');
    localStorage.removeItem('role');
    localStorage.removeItem('name');
    window.location.href = '/landing';
  }
}
