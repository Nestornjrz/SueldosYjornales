import { Component, OnInit, Input } from '@angular/core';
import { PrestamoSimpleDto } from "app/dtos/prestamo-simple-dto";
import { MovEmpleadoDetDto } from "app/dtos/mov-empleado-det-dto";

import * as _ from 'lodash';

@Component({
  selector: 'app-prestamo-individual',
  templateUrl: './prestamo-individual.component.html',
  styleUrls: ['./prestamo-individual.component.css']
})
export class PrestamoIndividualComponent implements OnInit {
  @Input() prestamoSimple: PrestamoSimpleDto;
  constructor() { }

  ngOnInit() {
    // se calcula el saldo
    let saldo = this.prestamoSimple.monto;;
    this.prestamoSimple.cuotasMov.forEach((cuo) => {
      if (cuo.movEmpleadoIDdeLaLiquidacion > 0) {
        saldo = saldo - cuo.debito;
      }
      cuo.saldo = saldo;
    });
  }
  onEditInit(cuota: MovEmpleadoDetDto) {
    //console.info("on edit init");
    //console.log(cuota);
  }
  onChangeInput(cuota: MovEmpleadoDetDto) {
    console.info("onChangeInput")
    console.log(cuota);
    //se verifica si coincide la sumatoria de todas las cuota con el monto total
    let sumaCuotas = _.sumBy(this.prestamoSimple.cuotasMov, (cuotas) => {
      return cuotas.debito;
    });
    if (this.prestamoSimple.monto == sumaCuotas) {
      console.log(`Los montos son iguales ${sumaCuotas}`);
    } else {
      console.log(`Los montos no son GUIALES Monto ${this.prestamoSimple.monto} != ${sumaCuotas}`)
      // Ahora se recorre las cuotas y se agrega la diferencia al siguiente
      let contador = 1;
      this.prestamoSimple.cuotasMov.sort((n1, n2) => n1.movEmpleadoDetID - n2.movEmpleadoDetID)
        .forEach((cuotaF) => {
          console.log(` id: ${cuota.movEmpleadoDetID}`);
          if (cuotaF.movEmpleadoDetID > cuota.movEmpleadoDetID && 
              contador == 1                                    && 
              this.prestamoSimple.cuotasMov.length != contador) {//Significa que es el siguiente
            cuotaF.debito += this.prestamoSimple.monto - sumaCuotas;
            contador++;
          }
        });
      //Se ve si el contador y la cantidad de cuotas son iguales, si son iguales
      //significa que es la ultima cuota y se avisa que no se puede modificar
      if(contador == this.prestamoSimple.cuotasMov.length){
        alert("Es la ultima cuota, no se puede modificar");
      }

      // Se ve de nuevo si la sumatorias son iguales
      sumaCuotas = _.sumBy(this.prestamoSimple.cuotasMov, (cuotas) => {
        return cuotas.debito;
      });
      if (this.prestamoSimple.monto == sumaCuotas) {
        console.log(`Los montos son iguales ${sumaCuotas}`);
      } else {
        console.warn(`Los montos no son GUIALES Monto ${this.prestamoSimple.monto} != ${sumaCuotas}`);
        alert(`Los montos no son GUIALES Monto ${this.prestamoSimple.monto} != ${sumaCuotas}`);
      }
      
    }
  }
  // Funciones 
  cambioMontoCuota() {

  }
}
