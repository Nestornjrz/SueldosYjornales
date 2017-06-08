import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/map';
import { WindowsRefService } from "app/services/windows-ref.service";
import { EmpleadoDto } from "app/dtos/empleado-dto";


@Injectable()
export class EmpleadosService {
  private _empleadosApiUrl = "";
  constructor(private _http: Http, private _windowRef: WindowsRefService) {
    //Esta parte se coloca porque cuando se publica en el servidor local la aplicacion web
    //esta en el segundo nivel de la aplicacion principal, en este caso la aplicacion Syj en IIS8
    let syj = "";
    if (this._windowRef.nativeWindow.location.hostname == "www.laaragonesa.com.py") {
      syj = '/Syj';
    }
    if (this._windowRef.nativeWindow.location.origin == "http://localhost:4200") {
      this._empleadosApiUrl = 'http://localhost:59315/api/Empleados/';
    }
    else {
      this._empleadosApiUrl = this._windowRef.nativeWindow.location.origin + syj + '/api/Empleados/';
    }
  }
  getEmpleados(): Observable<EmpleadoDto[]> {
    console.log(this._empleadosApiUrl);
    return this._http.get(this._empleadosApiUrl)
      .map((response: Response) => <EmpleadoDto[]>response.json())
      .do(data => {
        //console.log('ALL: ' + JSON.stringify(data))
      })
      .catch(this.handleError);
  }
  private handleError(error: Response) {
    console.error(error);
    return Observable.throw(error.json().error || 'Server error');
  }
}
