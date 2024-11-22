import {Component} from "@angular/core";
import {Router} from "@angular/router";
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {SessionService} from "../../../../backend/services/session/session.service";
import UserCredentialsModel from "../../../../backend/services/session/models/UserCredentialsModel";

@Component({
  selector: "app-sign-in",
  templateUrl: "./sign-in.component.html",
  styleUrls: ["./sign-in.component.scss"],
})
export class SignInComponent {
  readonly formField: any = {
    email: {
      name: "email",
      required: "Email is required",
      email: "Email is invalid",
    },
    password: {
      name: "password",
      required: "Password is required",
      minlength: "Password must be at least 6 characters",
    },
  };
  message: string = "";
  isSuccess: boolean = false;

  readonly loginForm = new FormGroup({
    [this.formField.email.name]: new FormControl("", [
      Validators.required,
      Validators.email,
    ]),
    [this.formField.password.name]: new FormControl("", [
      Validators.required,
      Validators.minLength(6),
    ]),
  });

  loginStatus: {
    loading?: true;
    error?: string;
  } | null = null;
  formElements = [
    {name: 'email', type: 'text', label: 'Email', required: true},
    {name: 'password', type: 'password', label: 'Password', required: true},
  ];

  constructor(
    private readonly _router: Router,
    private readonly _sessionService: SessionService,
  ) {
  }

  onFormSubmit(formData: any) {
    console.log('Form Data:', formData);
    let values: UserCredentialsModel = {email: formData.email, password: formData.password};

    this.loginStatus = {loading: true};

    this._sessionService.login(values).subscribe({
      next: () => {
        this.loginStatus = null;
        this.isSuccess = true;
        this.message = "Login successful";
        this._router.navigate(["/welcome"]);
      },
      error: (error) => {
        this.loginStatus = {error};
        this.message = error.details;
      },
    });
  }

  navigateToLogin() {
    this._router.navigate(['/login']);
  }
}
