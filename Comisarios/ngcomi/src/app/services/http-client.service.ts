import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { map, filter, catchError } from 'rxjs/operators';
import { MensajeDto } from '../dtos/mensaje-dto';
import { WindowsRefService } from './windows-ref.service';
import { HttpErrorResponse } from '@angular/common/http';

@Injectable()
/**
 * Este clase usa HttpCliente en vez de http porque HttpClient es lo nuevo 05/02/2018
 * esto es solo con el objetivo de compararlo con lo que antes utilizaba con http
 * Mas informacion al respecto
 * http://blog.enriqueoriol.com/2017/11/httpclient-vs-http-angular.html
 */
export class HttpClientService {

  constructor(private _http: HttpClient, private _windowRef: WindowsRefService) { }

  postObject(objeto, apiUrl: string): Observable<MensajeDto> {
    const url = this._windowRef.urlBase() + apiUrl;
    return this._http.post<MensajeDto>(url, objeto)
      .pipe(
      catchError(this.handleError)
      );
  }
  getMensajeDto(apiUrl: string): Observable<MensajeDto> {
    return this._http.get<MensajeDto>(this._windowRef.urlBase() + apiUrl)
      .pipe(
      catchError(this.handleError)
      );
  }
  getArray<T>(apiUrl: string): Observable<T[]> {
    return this._http.get<T[]>(this._windowRef.urlBase() + apiUrl)
      .pipe(
      catchError(this.handleError)
      );
  }
  private handleError(error: HttpErrorResponse) {
    if (error.error instanceof ErrorEvent) {
      // A client-side or network error occurred. Handle it accordingly.
      console.error('Ocurrio un error:', error.error.message);
      alert('Ocurrio un error:' + error.error.message);
    } else {
      // The backend returned an unsuccessful response code.
      // The response body may contain clues as to what went wrong,
      console.error(
        `Backend retorno el codigo ${error.status}, ` +
        `body was: ${error.error}`);
      if (error.status === 401) {
        alert('No tienes permiso para realizar esta operacion');
      } else {
        alert(
          `Backend retorno el codigo ${error.status}, ` +
          `body was: ${error.error}`);
      }
    }
    // return an observable with a user-facing error message
    return throwError(
      'Ocurrio un error; por favor intentar mas tarde.');
  }
}
