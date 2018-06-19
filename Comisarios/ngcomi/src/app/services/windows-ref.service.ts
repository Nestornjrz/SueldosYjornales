import { Injectable } from '@angular/core';
function _window(): any {
  // return the global native browser window object
  return window;
}

@Injectable()
export class WindowsRefService {

  constructor() { }
  get nativeWindow(): any {
    return _window();
  }

  urlBase(): string {
    let protocol = this.nativeWindow.location.protocol;
    const hostname = this.nativeWindow.location.hostname;
    let port = this.nativeWindow.location.port;
    let aplicacion = '';
    if (port === '4200') {// Si se hace en el puerto que generalmente es de desarrollo con angular-cli
      port = '44364'; // Es el puerto de desarrollo cuando se ejecuta la web api para este proyecto
      protocol = 'https:';
    }
    if (hostname === 'www.laaragonesa.com.py') {
      aplicacion = '/Comisarios';
    }
    return protocol + '//' + hostname + ':' + port + aplicacion;
  }
}
