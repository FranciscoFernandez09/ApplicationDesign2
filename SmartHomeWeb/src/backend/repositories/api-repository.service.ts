import {HttpClient, HttpErrorResponse, HttpHeaders,} from '@angular/common/http';
import {catchError, Observable, retry, throwError} from 'rxjs';

export default abstract class ApiRepositoryService {
  protected fullEndpoint: string;

  constructor(
    protected readonly _apiOrigin: string,
    protected readonly _endpoint: string,
    protected readonly _http: HttpClient
  ) {
    this.fullEndpoint = `${this._apiOrigin}`;
    if (this._endpoint !== '') {
      this.fullEndpoint += `/${this._endpoint}`;
    }
  }

  protected get headers() {
    return {
      headers: new HttpHeaders({
        accept: 'application/json',
        Authorization: localStorage.getItem('token') ?? '',
      }),
    };
  }

  protected get<T>(extraResource = '', query = ''): Observable<T> {
    query = query ? `?${query}` : '';
    extraResource = extraResource ? `/${extraResource}` : '';

    return this._http
      .get<T>(`${this.fullEndpoint}${extraResource}${query}`, this.headers)
      .pipe(retry(3), catchError(this.handleError));
  }

  protected post<T>(body: any, extraResource = ''): Observable<T> {
    extraResource = extraResource ? `/${extraResource}` : '';

    return this._http
      .post<T>(`${this.fullEndpoint}${extraResource}`, body, this.headers)
      .pipe(retry(3), catchError(this.handleError));
  }

  protected putById<T>(
    id: string,
    body: any = null,
    extraResource = ''
  ): Observable<T> {
    extraResource = extraResource ? `/${extraResource}` : '';

    return this._http
      .put<T>(`${this.fullEndpoint}/${id}${extraResource}`, body, this.headers)
      .pipe(retry(3), catchError(this.handleError));
  }

  protected delete<T>(extraResource = '', query = ''): Observable<T> {
    query = query ? `?${query}` : '';

    extraResource = extraResource ? `/${extraResource}` : '';

    return this._http
      .delete<T>(`${this.fullEndpoint}${extraResource}${query}`, this.headers)
      .pipe(retry(3), catchError(this.handleError));
  }

  protected patch<T>(extraResource = '', body: any = null): Observable<T> {
    extraResource = extraResource ? `/${extraResource}` : '';

    return this._http
      .patch<T>(`${this.fullEndpoint}${extraResource}`, body, this.headers)
      .pipe(retry(3), catchError(this.handleError));
  }

  protected handleError(error: HttpErrorResponse) {
    if (error.error instanceof ErrorEvent) {
      // A client-side or network error occurred. Handle it accordingly.
      console.error('An error occurred:', error.error.message);
    } else {
      // The backend returned an unsuccessful response code.
      // The response body may contain clues as to what went wrong.
      console.error(
        `Backend returned code ${error.status}, ` + `body was: ${error.error}`
      );
    }

    // Return an observable with a user-facing error message.
    let returnResponse: any = {};

    switch (error.status) {
      case 401:
        returnResponse.details = 'Unauthorized access';
        break;
      case 402:
        returnResponse.details = 'Payment required';
        break;
      case 403:
        returnResponse.details = 'Forbidden access';
        break;
      case 404:
        returnResponse.details = 'Resource not found, check and try again.';
        break;
      case 405:
        returnResponse.details = 'Method not allowed';
        break;
      default:
        returnResponse = {
          innerCode: error.error.code,
          message: error.error.message,
          details: error.error.details ? error.error.details : 'An error occurred, please try again later.',
        };
    }

    return throwError(returnResponse);
  }
}
