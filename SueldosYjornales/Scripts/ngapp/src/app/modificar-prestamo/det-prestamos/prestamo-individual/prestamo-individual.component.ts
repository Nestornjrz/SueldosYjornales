import { Component, OnInit, Input } from '@angular/core';
import { PrestamoSimpleDto } from "app/dtos/prestamo-simple-dto";
import { MovEmpleadoDetDto } from "app/dtos/mov-empleado-det-dto";

import * as _ from 'lodash';
import { Message } from "primeng/primeng";

@Component({
  selector: 'app-prestamo-individual',
  templateUrl: './prestamo-individual.component.html',
  styleUrls: ['./prestamo-individual.component.css']
})
export class PrestamoIndividualComponent implements OnInit {
  @Input() prestamoSimple: PrestamoSimpleDto;
  copiaCuotas: MovEmpleadoDetDto[] = [];
  msgs: Message[] = [];
  classPanel:string = 'panel';
  tituloPrestamo:string = '';
  desabilitarBoton:boolean = false;
  constructor() { }

  ngOnInit() {
    this.calcularSaldo();
    let copiaCuotas = JSON.parse(JSON.stringify(this.prestamoSimple.cuotasMov));
    this.copiaCuotas = copiaCuotas;
    this.verSiElPrestamoEstaCancelado();
  }
  onEditInit(cuota: MovEmpleadoDetDto) {
    //console.info("on edit init");
    //console.log(cuota);
  }
  onChangeInput(cuotaModificada: MovEmpleadoDetDto) {
    let mcm = new ManejadorCuotaModificada(cuotaModificada, this.prestamoSimple, this.copiaCuotas);
    this.msgs = mcm.controlarModificacion();
    let mensaje = mcm.verificarSilosMontosSonIguales();
    if (mensaje != null) {
      this.msgs.push(mensaje);
    }
    this.calcularSaldo();
  }
  onGuardarCambios(){
    alert("Cambios guardados");
  }
  // Funciones 
  verSiElPrestamoEstaCancelado(){
    let cuotas:MovEmpleadoDetDto[] = this.prestamoSimple.cuotasMov;    
    let cantCuotasCanceladas = 0;
    for (var index = 0; index < cuotas.length; index++) {
      let cuo = cuotas[index];
      if(cuo.movEmpleadoIDdeLaLiquidacion > 0){
        cantCuotasCanceladas++;
      }
    }
    if(cuotas.length == 0){
      this.tituloPrestamo = 'CUOTAS NO GENERADAS';
      this.classPanel += ' panel-info';
      this.desabilitarBoton = true;
    }else if(cuotas.length == cantCuotasCanceladas){
      this.classPanel += ' panel-danger'
      this.tituloPrestamo = 'CANCELADO';
      this.desabilitarBoton = true;
    }else{
      this.classPanel += ' panel-primary';
      this.desabilitarBoton = false;
    }
  }
  calcularSaldo() {
    // se calcula el saldo
    let saldo = this.prestamoSimple.monto;;
    this.prestamoSimple.cuotasMov.forEach((cuo) => {
      if (cuo.movEmpleadoIDdeLaLiquidacion > 0) {
        saldo = saldo - cuo.debito;
      }
      cuo.saldo = saldo;
    });
  }
}
export class ManejadorCuotaModificada {
  private cuotaModificada: MovEmpleadoDetDto;
  private prestamoSimple: PrestamoSimpleDto;
  private copiaCuotas: MovEmpleadoDetDto[];
  constructor(cuotaModificada: MovEmpleadoDetDto, prestamoSimple: PrestamoSimpleDto, copiaCuotas: MovEmpleadoDetDto[]) {
    this.cuotaModificada = cuotaModificada;
    this.prestamoSimple = prestamoSimple;
    this.copiaCuotas = copiaCuotas;
  }
  controlarModificacion(): Message[] {
    console.clear();
    console.info("onChangeInput")
    console.log(this.cuotaModificada);
    let mensaje: Message[] = [];
    //se verifica si coincide la sumatoria de todas las cuota con el monto total
    let sumaCuotas = _.sumBy(this.prestamoSimple.cuotasMov, (cuotas) => {
      return cuotas.debito;
    });
    if (this.prestamoSimple.monto == sumaCuotas) {
      console.log(`Los montos son iguales ${sumaCuotas}`);
    } else {
      console.warn(`Los montos no son GUIALES Monto ${this.prestamoSimple.monto} != ${sumaCuotas}`)
      // Ahora se recorre las cuotas y se agrega la diferencia al siguiente
      for (let i = 0; i <= this.prestamoSimple.cuotasMov.length; i++) {
        console.log(` id: ${this.cuotaModificada.movEmpleadoDetID}`);
        if (this.prestamoSimple.cuotasMov[i].movEmpleadoDetID == this.cuotaModificada.movEmpleadoDetID &&
          this.prestamoSimple.cuotasMov[i].movEmpleadoIDdeLaLiquidacion > 0) {//Si la cuota modificada tiene liquidacion
          this.cuotaModificada.debito = this.copiaCuotas.find((cuota) => { return cuota.movEmpleadoDetID == this.prestamoSimple.cuotasMov[i].movEmpleadoDetID }).debito;
          mensaje.push({ severity: 'error', summary: 'No se puede modificar', detail: 'Ya tiene liquidacion' });
          break;
        } else if (this.prestamoSimple.cuotasMov[i].movEmpleadoDetID == this.cuotaModificada.movEmpleadoDetID &&
          i + 1 == this.prestamoSimple.cuotasMov.length) {//Si la cuota modificada es la ultima cuota        
          //this.cuotaModificada.debito = this.copiaCuotas.find((cuota) => { return cuota.movEmpleadoDetID == this.prestamoSimple.cuotasMov[i].movEmpleadoDetID }).debito;
          //Devido a que es la ultima cuota se agrega una nueva cuota
          let cuotasCopia = [...this.prestamoSimple.cuotasMov];

          let nuevaCuota: MovEmpleadoDetDto = new MovEmpleadoDetDto();
          nuevaCuota.movEmpleadoID = this.cuotaModificada.movEmpleadoID;
          nuevaCuota.movEmpleadoDetID = this.cuotaModificada.movEmpleadoDetID + 1;
          nuevaCuota.debito = this.prestamoSimple.monto - sumaCuotas;
          nuevaCuota.credito = 0;
          nuevaCuota.mesAplicacion = new Date(this.cuotaModificada.mesAplicacion.toString());//Se le asigna el mes de la ultima cuota
          nuevaCuota.mesAplicacion.setMonth(nuevaCuota.mesAplicacion.getMonth() + 1);//Se le añade un mes
          nuevaCuota.liquidacionConcepto = this.cuotaModificada.liquidacionConcepto;
          nuevaCuota.movEmpleadoIDdeLaLiquidacion = 0;

          cuotasCopia.push(nuevaCuota);

          this.prestamoSimple.cuotasMov = cuotasCopia;//Se refresca la tabla

          //mensaje.push({ severity: 'error', summary: 'No se puede modificar', detail: 'Es la ultima cuota' });
          break;
        } else if (this.prestamoSimple.cuotasMov[i].movEmpleadoDetID > this.cuotaModificada.movEmpleadoDetID) {//Significa que es el siguiente cuota
          this.prestamoSimple.cuotasMov[i].debito += this.prestamoSimple.monto - sumaCuotas;
          break;
        }
      }
      return mensaje;
    }
  }
  verificarSilosMontosSonIguales(): Message {
    // Se ve de nuevo si la sumatorias son iguales
    let sumaCuotas = _.sumBy(this.prestamoSimple.cuotasMov, (cuotas) => {
      return cuotas.debito;
    });
    if (this.prestamoSimple.monto == sumaCuotas) {
      console.log(`Los montos son iguales ${sumaCuotas}`);
      return null;
    } else {
      console.warn(`Los montos no son GUIALES Monto ${this.prestamoSimple.monto} != ${sumaCuotas}`);
      //alert(`Los montos no son GUIALES Monto ${this.prestamoSimple.monto} != ${sumaCuotas}`);
      return { severity: 'error', summary: 'ERROR GRAVE', detail: `Los montos no son iguales al finalizar el calculo  ${this.prestamoSimple.monto} != ${sumaCuotas}` };
    }
  }
}
