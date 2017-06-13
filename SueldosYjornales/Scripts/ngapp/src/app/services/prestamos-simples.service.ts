import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/map';
import { WindowsRefService } from 'app/services/windows-ref.service';
import { PrestamoSimpleDto } from "app/dtos/prestamo-simple-dto";

@Injectable()
export class PrestamosSimplesService {
  private _prestamosSimplesByEmpleadoID_ConCuotasApiUrl = '';
  constructor(private _http: Http, private _windowRef: WindowsRefService) {
    // Esta parte se coloca porque cuando se publica en el servidor local la aplicacion web
    // esta en el segundo nivel de la aplicacion principal, en este caso la aplicacion Syj en IIS8
    let syj = '';
    if (this._windowRef.nativeWindow.location.hostname === 'www.laaragonesa.com.py') {
      syj = '/Syj';
    }
    if (this._windowRef.nativeWindow.location.origin === 'http://localhost:4200') {
      this._prestamosSimplesByEmpleadoID_ConCuotasApiUrl = 'http://localhost:59315/api/PrestamosSimples/ByEmpleadoID/ConCuotas/';
    } else {
      this._prestamosSimplesByEmpleadoID_ConCuotasApiUrl = this._windowRef.nativeWindow.location.origin + syj
        + '/api/PrestamosSimples/ByEmpleadoID/ConCuotas/';
    }
  }
  GetByEmpleadoIDConCuotas(empleadoID:number): Observable<PrestamoSimpleDto[]> {
    return this._http.get(this._prestamosSimplesByEmpleadoID_ConCuotasApiUrl + `?empleadoID=${empleadoID}`)
      .map((response: Response) => <PrestamoSimpleDto[]>response.json())
      .do(data => {
        // console.log('ALL: ' + JSON.stringify(data))
      })
      .catch(this.handleError);
  }
  private handleError(error: Response) {
    console.error(error);
    return Observable.throw(error.json().error || 'Server error');
  }
}
