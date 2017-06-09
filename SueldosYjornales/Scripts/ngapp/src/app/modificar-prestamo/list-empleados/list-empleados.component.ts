import { Component, OnInit } from '@angular/core';

// import { debounce } from "lodash";
import * as _ from "lodash";

import { EmpleadosService } from "app/services/empleados.service";
import { EmpleadoDto } from "app/dtos/empleado-dto";
import { SucursaleDto } from "app/dtos/sucursale-dto";

export class GroupBySucursal {
  constructor(sucursal: SucursaleDto) {
    this.sucursal = sucursal;
  }
  sucursal: SucursaleDto;
  empleados: EmpleadoDto[] = [];
}

@Component({
  selector: 'app-list-empleados',
  templateUrl: './list-empleados.component.html',
  styleUrls: ['./list-empleados.component.css']
})
export class ListEmpleadosComponent implements OnInit {
  empleados: EmpleadoDto[];
  empleadosPorSucursal:GroupBySucursal[];
  constructor(private _empleadosService: EmpleadosService) { }

  ngOnInit() {
    this._empleadosService.getEmpleados()
      .subscribe((empleados) => {
        this.empleados = empleados;
        this.empleadosPorSucursal = this.agruparPorSucursal(empleados);
      });
  }

  /// Funciones privadas
  private agruparPorSucursal(empleados: EmpleadoDto[]): GroupBySucursal[] {
    // Se hace el listado de sucursales
    let sucursales: SucursaleDto[] = [];
    this.empleados.forEach((empleado, index) => {
      sucursales.push(empleado.sucursale);
    });
    sucursales = _.uniqBy(sucursales, (x: SucursaleDto) => {
      return x.sucursalID
    });
    console.log(sucursales);
    // Se agrupa 
    let empleadosPorSucursal: GroupBySucursal[] = [];
    sucursales.forEach(sucursal => {
      let grupo = new GroupBySucursal(sucursal);
      grupo.empleados = _.filter(empleados, (e: EmpleadoDto) => {
        return e.sucursale.sucursalID == sucursal.sucursalID;
      })
      empleadosPorSucursal.push(grupo);
    });
    console.log(empleadosPorSucursal);
    return empleadosPorSucursal;
  }

}
